using System;
using System.IO;
using System.Security.Permissions;
using log4net;

namespace MonitorTrabajosImpresion
{
    /** \brief Realiza backup de los ficheros de Spool
     */
    public class WatchIO
    {
        /// Se crea el Logger con nombre: WatchIO
        private static readonly ILog Log = LogManager.GetLogger("WatchIO");

        /// Subdirectorio para backup de los ficheros de spool relativo al directorio de Spool 
        public const string _DIR_SPOOL_BACKUP = "MonitorImpresion";

        /// Variable para control de subscripción Eventos
        private static bool IsEvenCapture = false;

        /// Indica si se está suscrito a los eventos del Sistema de Archivos
        public static bool IsEventSigned
        {
            get
            {
                return IsEvenCapture;
            }
        }

        /** \brief Suscripcion Eventos Sistema de Archivos
         * \param Path Directorio de Spool de Impresión
         * \remarks (Basado en MSDN)
         * @see https://msdn.microsoft.com/es-es/library/system.io.filesystemwatcher.created%28v=vs.110%29.aspx
         */
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")] //Asegurarnos que el usuario tiene permisos suficientes. Se puede comentar va a ser un proceso de sistema
        public static void FileEvent(string Path)
        {
            /// Comprobamos si existe el directorio de backup para Spool de impresión. Si no existe se crea
            DirectoryInfo VSpoolDirectoryBackup = new DirectoryInfo(Path + @"\" + _DIR_SPOOL_BACKUP);
            if (!VSpoolDirectoryBackup.Exists)
            {
                VSpoolDirectoryBackup.Create();
                Log.Info("Se crea el Subdirectorio de backup de Spool de Impresión: " + Path + @"\" + _DIR_SPOOL_BACKUP);
            }

            /// Creación del evento que supervisa el directorio de Spool de impresión
            // Creación nuevo FileSystemWatcher y sus propiedades. 
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path;
            
            /// Supervisamos cambios, en tiempos de último acceso y úlitma escritura, y renombrado de ficheros y directorios
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            /// Filtro ficheros de Spool: Vamos a monitorizar ficheros de Spool .SHD y .SPL
            watcher.Filter = "?????.S??";

            /// Suscribimos el método delegado al evento de creación de fichero (con modificado y borrado daba errores)
            watcher.Created += new FileSystemEventHandler(OnCreated); //Creado

            /// Actualizamos la variable de control de subscripción
            IsEvenCapture = true;

            /// Activamos la suscripción al evento
            watcher.EnableRaisingEvents = true;
            

        } //WatchSpoolIO()

        /** \brief Método delegado suscrito al evento de creación de archivo.
         *  \details que realizar una copia del fichero de Spool anexando la fecha y hora de creación a la copia del fichero. 
         * \param source instancia de objeto que lanza el evento
         * \param edata datos para los eventos de directorio
         */
        private static void OnCreated(object source, FileSystemEventArgs edata)
        {
            // Fecha para guardar un backup existente de fichero de spool
            string Fecha = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "_" +
                           DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            /// Nombre del fichero poniendole como sufio en el nombre fecha y hora
            string FileNameBackup = edata.Name.ToUpper().Replace(".SPL", "") + "_" + Fecha + ".SPL";


            /// Si se ejecuta este método es por que está suscrito a los eventos del Servidor de Impresión.
            IsEvenCapture = true;

            // Tiempo espera a que el fichero no este bloqueado (sg)
            const int _TIEMPO_ESPERA = 30;

            FileInfo MiFileInfo = new FileInfo(edata.FullPath);

            /// Guardamos en el Log información del fichero
            Log.Info(edata.FullPath + ", Estado: " + edata.ChangeType.ToString());



            /// Se chequea que el fichero no esté bloqueado. Limitamos tiempo de espera a 30 sg.
            Log.Debug("Chequeamos hasta que el fichero no esté bloqueado. Limitamos tiempo de espera a 30 sg.");
            DateTime Inicio = DateTime.Now;
            TimeSpan tspan;
            int sg = 0;
            while (IsFileLocked(MiFileInfo) & (sg < _TIEMPO_ESPERA))
            {
                //Log.Error("El fichero: " + edata.FullPath + " esta bloqueado. " + (DateTime.Now.Millisecond - Inicio.Millisecond).ToString() + " msg.");
                tspan = DateTime.Now.Subtract(Inicio).Duration();
                sg = (int) tspan.TotalSeconds + (int) tspan.TotalMinutes * 60;
                System.Threading.Thread.Sleep(10);
            }


            /// Hacemos una copia del fichero de Spool
            try
            {
                Log.Info("Creación fichero: " + edata.FullPath);
                Log.Info("Copia en: " + edata.FullPath.Replace(@"\" + edata.Name, "") + @"\" + _DIR_SPOOL_BACKUP + @"\" + FileNameBackup);

                // Guardamos copia del fichero
                File.Copy(edata.FullPath, edata.FullPath.Replace(@"\" + edata.Name, "") + @"\" + _DIR_SPOOL_BACKUP + @"\" + FileNameBackup, true);
            }
            catch (Exception e)
            {
                Log.Fatal("Error al copiar archivo " + edata.FullPath + ". Revisar propiedades de impresora para conservar documentos después de impresión", e);
            }
        } //OnCreated()


        /** \brief Comprobar si un fichero está bloqueado
         * \param FileInfo contiene las propiedades de un fichero
         * \remarks (Fuente para comprobar si un archivo está en uso) 
         * @see http://www.iteramos.com/pregunta/1769/hay-una-manera-de-comprobar-si-un-archivo-esta-en-uso
         */
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // el archivo no está disponibl por alguna de estas razones:
                // - Todavía se está escribiendo su contenido
                // - Está siendo procesado por otro thread
                // - El fichero no existe
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /** \brief Test creación archivo
         * \details Creamos un archivo para testear si estamos suscritos a los eventos de Sistema de Archivos
         */

        public static void IsAliveEvents(object sender, EventArgs Args)
        {

            Log.Debug("IsAliveEvents(INICIO) -> Valor de IsEvenCapture: " + IsEvenCapture.ToString());


            // Vamos a chequear si estamos suscritos a los eventos del Sistema de Archivos
            Log.Debug("IsAliveEvents -> Ponemos la variable de control IsEvenCampture = false");
            IsEvenCapture = false;

            // Creamos un fichero para que se produzca un evento de creación de archivo del Sistema de Archivos
            Log.Debug("Creamos el fichero de testeo: " + LocalPrinting.PathPrintSpool() + @"\" + "test.SPL");
            File.Create(LocalPrinting.PathPrintSpool() + @"\" + "test.SPL").Dispose();

            //Esperamos 3000  milisegundos
            Log.Debug("IsAliveEvents -> Esperamos 3 sg");
            System.Threading.Thread.Sleep(3000);

            // Borramos los ficheros de test
            try
            {
                File.Delete(LocalPrinting.PathPrintSpool() + @"\" + "test.SPL");
            }
            catch (Exception e)
            {
                Log.Error("No se ha podido borrar el fichero de test: " + LocalPrinting.PathPrintSpool() + @"\" + "test.SPL");
            }

            try
            {
                File.Delete(LocalPrinting.PathPrintSpool() + @"\" + _DIR_SPOOL_BACKUP + @"\" + "test.SPL");
            }
            catch (Exception e)
            {
                Log.Error("No se ha podido borrar el fichero de test: " + LocalPrinting.PathPrintSpool() + @"\" + _DIR_SPOOL_BACKUP + @"\" + "test.SPL");
            }

            /* 
             * Chequeamos la variable de control para ver si efectivamente estamos suscritos.
             * Si estamos subscritos está variable se tenía que haber actualizado a través del metodo suscrito
             * OnCreated().
             * 
             * En caso de que no estemos suscritos lanzamos el método FileEvent() para subscribir el método
             * OnCreated().
             */
            if (IsEvenCapture)
            {
                Log.Info("Suscripción método WatchIO.OnCreated() a eventos del Sistema Archivos: OK");

            }
            else
            {
                Log.Error("No suscrito a los eventos del Sistema Archivos. Nos suscribimos.");
                try
                {
                    FileEvent(LocalPrinting.PathPrintSpool());
                    IsEvenCapture = true;

                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

            }

        } //Fin IsAliveEvents()


    }
}
