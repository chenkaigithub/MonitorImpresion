using System;
using System.IO;
using log4net;
using MonitorImpresion.DB;


namespace MonitorTrabajosImpresion
{
    /** 
     * \brief Con esta clase se obtienen las propiedades de los trabajos de impresión.
     * \details Para ello se utilizan los distintos métodos implementados y se guarda para cada uno de ellos la información obtenida en BD. 
     */
    public class AnalizeJobId
    {
        /// Propiedades del trabajo de impresión independiente de la fuente de obtención de información
        private string ID_JOBNAME; /// - Identificador único para cada Trabajo de Impresión
        private int N_JOB;         /// - Número de trabajo de impresión 
        private string ID_PRINTER; /// - Nombre de impresora
        private string F_PRINTJOB; /// - Fecha de impresión del documento

        private string Printer;
        private string JobId;

        // Se crea el Logger con nombre: PCLXLSpool
        private static readonly ILog Log = LogManager.GetLogger("AnalizeJobId");

        internal ApiImpresion ApiImpresion
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal EmfSpool EmfSpool
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal JPLSpool JPLSpool
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal LocalPrinting LocalPrinting
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /** \brief Constructor para análizar el trabajo de impresión lanzado por una impresora. 
         * \param Printer Nombre de la impresora por la que se ha imprimido el documento.
         * \param JobId: Número de trabajo de impresión
         */
        public AnalizeJobId(string Printer, string JobId)
        {
            this.Printer = Printer;
            this.JobId = JobId;
            AnalizePrintJob();
        }


        /** \brief Análisis del trabajo de Impresión. 
         * \return No devuelve nada.
         * \details
         * Se guardan en B.D las propiedades del trabajo de impresión obtenidas a través de las API de impresión, del namespace 
         * System.Printing y del análisis de los ficheros de Spool correspondientes al trabajo de impresión.
         */
        private void AnalizePrintJob() // string Printer, string JobId)
        {

            // Objeto para permitir una espera en caso de que el fichero de spool este abierto
            ElapsedTime TiempoEspera;
            // Fichero de spool que se tiene que analizar
            FileInfo MiFileInfo;

            // Estructura utilizada para guardar los trabajos de impresión 
            STRUCT_PRINT_JOB PrintJob = new STRUCT_PRINT_JOB();

            this.ID_JOBNAME = ""; 
            this.N_JOB = Convert.ToInt32(this.JobId);
            this.ID_PRINTER = this.Printer;

            // La impresora ¿es un plotter?
            if (Util.ContieneTexto(this.Printer, "Plotter"))
            {
                PrintJob.ID_ISPLOTTER = "S";
            }
            else
            {
                PrintJob.ID_ISPLOTTER = "N";
            }
        

            bool AnalizedSpool = false;

            //Se recuperan los primeros detalles del trabajo de impresión
            PrintJob.N_JOB = this.N_JOB;
            PrintJob.ID_PRINTER = this.ID_PRINTER;

            /// - Propiedades obtenidas a través del namespace System.Printing
            try
            {
                // Recuperamos las propiedades
                LocalPrinting.DetailsPrint(PrintJob.ID_PRINTER, this.JobId, ref PrintJob);
                this.ID_JOBNAME = PrintJob.ID_JOBNAME;
                this.F_PRINTJOB = PrintJob.F_PRINTJOB;

                
                Log.Info("  -- Se extraen las propiedades utilizando System.Printing --");
                // Generamos log
                LogPrintJob(ref PrintJob);
                // Guardamos en B.D
                SavePrintJob(ref PrintJob);
            }
            catch (Exception e)
            {
                Log.Fatal("Error extrayendo propiedades a través de System.Printing");
                Log.Fatal(e);
            }

            /// - Propiedades obtenidas a través de las APIs de impresión
            LimpiaStrucPrintJob(ref PrintJob);
            PrintJob.ID_JOBNAME = ID_JOBNAME;
            try
            {
                // Recuperamos las propiedades
                ApiImpresion MiApiImpresion = new ApiImpresion();
                MiApiImpresion.Analize(this.Printer, (uint)Convert.ToInt32(this.JobId), ref PrintJob);

                // Guardamos en B.D las propiedades obtenidas
                Log.Info("  -- Se extraen las propiedades utilizando las API de impresión. --");
                LogPrintJob(ref PrintJob);
                SavePrintJob(ref PrintJob);
                /// Enviamos información del trabajo de impresión via email
                LogPrintJobMail(ref PrintJob);

            }
            catch (Exception e)
            {
                Log.Fatal("No se puede analizar el trabajo de impresión: " + this.JobId + " a través de las APIs de winspool.drv.");
                Log.Fatal(e);

            } //try

            /// Para el analisis de ficheros de Spool, antes de empezar, nos aseguramos de que el fichero no está bloqueado
            // Establecemos un tiempo de espera
            TiempoEspera = new ElapsedTime();
            // Fichero de spool que se tiene que analizar
            MiFileInfo = new FileInfo(LocalPrinting.PathPrintSpool() + @"\" + this.JobId.PadLeft(5, '0') + ".SPL");
            while ((!TiempoEspera.OverElapsedTime()) & WatchIO.IsFileLocked(MiFileInfo))
            {
                // Se da tiempo a que se libere el archivo de Spool
                System.Threading.Thread.Sleep(10);
            }

            /// - Propiedades obtenidas a través del análisis del fichero de Spool con lenguaje EMF
            LimpiaStrucPrintJob(ref PrintJob);
            PrintJob.ID_JOBNAME = ID_JOBNAME;
            if (PrintJob.N_JOBSIZE > 0)
            {
                try
                {
                    Log.Debug("Analizamos si es lenguaje EMF");
                    // Se comenta la siguiente línea al desactivar la suscripción a los eventos del sistema de archivos de la clase WatchIO
                    EmfSpool MyEmfSpool = new EmfSpool(LocalPrinting.PathPrintSpool() + @"\" + this.JobId.PadLeft(5, '0') + ".SPL");
                    MyEmfSpool.Analize(ref PrintJob);
                    AnalizedSpool = true;
                    Log.Info("Establecemos como fuente del análisis: EMF");
                    PrintJob.ID_FUENTE = "EMF";
                    Log.Info("  -- Se extraen las propiedades utilizando analisis EMF. --");
                    LogPrintJob(ref PrintJob);
                    SavePrintJob(ref PrintJob);
                    /// Enviamos información del trabajo de impresión via email
                    LogPrintJobMail(ref PrintJob);

                }
                catch (Exception e)
                {
                    Log.Info("El trabajo de impresión: " + this.JobId + " no utiliza lenguaje EMF.");
                    Log.Fatal(e);
                }//try 
            }

            /// - Propiedades obtenidas a través del análisis del fichero de Spool con lenguaje JPL.
            LimpiaStrucPrintJob(ref PrintJob);
            PrintJob.ID_JOBNAME = ID_JOBNAME;
            if ((!AnalizedSpool) && (PrintJob.N_JOBSIZE > 0))
            {
                try
                {
                    Log.Debug("Analizamos si es lenguaje JPL");
                    // Se comentan las siguientes 2 líneas al desactivar la suscripción a los eventos del sistema de archivos de la clase WatchIO
                    Log.Debug(LocalPrinting.PathPrintSpool() + @"\" +  @"\" + this.JobId.PadLeft(5, '0') + ".SPL");
                    JPLSpool MyJPLSpool = new JPLSpool(LocalPrinting.PathPrintSpool() + @"\" + this.JobId.PadLeft(5, '0') + ".SPL");

                    MyJPLSpool.Analize(ref PrintJob);
                    AnalizedSpool = true;
                    Log.Info("  -- Se extraen las propiedades utilizando analisis JPL. --");
                    LogPrintJob(ref PrintJob);
                    SavePrintJob(ref PrintJob);
                    /// Enviamos información del trabajo de impresión via email
                    LogPrintJobMail(ref PrintJob);

                }
                catch (Exception e)
                {
                    Log.Info("No se puede analizar el trabajo de impresión: " + this.JobId + " mediante análisis JPL.");
                    Log.Fatal(e);
                } //try
            }//if

            Log.Debug("Trabajo de impresión: " + this.JobId + "en impresora " + this.Printer + " analizado con éxito");
        } //AnalizeSpool()

        /** \brief Guardar log propiedades del trabajo de impresión en B.D
         * \param PrintJob struct por referencia con las propiedades del trabajo de impresión
         * \return No devuelve nada
         */
        private void SavePrintJob(ref STRUCT_PRINT_JOB PrintJob)
        {
            string SQL="";
            try
            {
                //int filas = 0;
                /// Guardamos las propiedades del trabajo de impresión en BD                 
                /// - Se construye la instrucción SQL de inserción
                SQL = "INSERT INTO SPOOL_PRINTJOBS(ID_JOBNAME, " +
                      "ID_FUENTE, " +
                      "N_JOB, " +
                      "F_PRINTJOB, " +
                      "ID_LOGIN, " +
                      "ID_PRINTSERVER, " +
                      "ID_PRINTER, " +
                      "ID_DOCUMENT, " +
                      "N_PAGES, " +
                      "N_PAGES_PRINTED, " +
                      "N_COLORPAGES, " +
                      "N_LENGTH, " +
                      "N_WIDTH, " +
                      "ID_MEDIASIZE, " +
                      "N_MEDIASIZE, " +
                      "N_ORIENTATION, " +
                      "N_COPIES, " +
                      "ID_COLOR, " +
                      "ID_DUPLEX, " +
                      "ID_STATUS, " +
                      "ID_ISPLOTTER, " +
                      "ID_MEDIATYPE, " +
                      "N_MEDIATYPE, " +
                      "N_JOBSIZE) VALUES ";
                SQL += "('" + PrintJob.ID_JOBNAME + "', '" +
                       PrintJob.ID_FUENTE + "', " +
                       PrintJob.N_JOB + ", " +
                       "TO_DATE('" + PrintJob.F_PRINTJOB + "', 'DD/MM/YYYY HH24:Mi:SS')" + ", '" +
                       PrintJob.ID_LOGIN + "', '" +
                       PrintJob.ID_PRINTSERVER + "', '" +
                       PrintJob.ID_PRINTER + "', '" +
                       PrintJob.ID_DOCUMENT + "', " +
                       PrintJob.N_PAGES + ", " +
                       PrintJob.N_PAGES_PRINTED + ", " +
                       PrintJob.N_COLORPAGES + ", " +
                       PrintJob.N_LENGTH + ", " +
                       PrintJob.N_WIDTH + ", '" +
                       PrintJob.ID_MEDIASIZE + "', " +
                       PrintJob.N_MEDIASIZE + ", " +
                       PrintJob.N_ORIENTATION + ", " +
                       PrintJob.N_COPIES + ", '" +
                       PrintJob.ID_COLOR + "', '" +
                       PrintJob.ID_DUPLEX + "', '" +
                       PrintJob.ID_STATUS + "', '" +
                       PrintJob.ID_ISPLOTTER + "', '" +
                       PrintJob.ID_MEDIATYPE + "', " +
                       PrintJob.N_MEDIATYPE + ", " +
                       PrintJob.N_JOBSIZE + ")";

                Log.Info("Insert lanzado contra la BD:");
                Log.Info(SQL);

                /// - Conectamos a la BD
                DBClient client = new DBClient();
                
                /// - Lanzamos la instrucción SQL
                if (client.EjecutaSQL(SQL))
                {
                    Log.Info("Se guarda Información del trabajo de impresión en BD. INSERT correcto: ");
                }
                else
                {
                    Log.Fatal("Falla el siguiente INSERT en BD: " + (char)13 + SQL);
                }

                /// - Cerramos la BD 
                client.Close();
                
            }
            catch (Exception e)
            {
                Log.Fatal(e.ToString() + (char) 13 + SQL);
                
            }
        } //GuardarLog()

        /** \brief Log del trabajo de impresión
         */
        public static void LogPrintJob(ref STRUCT_PRINT_JOB PrintJob)
        {
            /// Generamos log con las propiedades del trabajo de impresión
            Log.Info("ID_JOBNAME: " + PrintJob.ID_JOBNAME);
            Log.Info("ID_FUENTE: " + PrintJob.ID_FUENTE);
            Log.Info("N_JOB: " + PrintJob.N_JOB);
            Log.Info("F_PRINTJOB: " + PrintJob.F_PRINTJOB);
            Log.Info("ID_LOGIN: " + PrintJob.ID_LOGIN);
            Log.Info("ID_PRINTSERVER: " + PrintJob.ID_PRINTSERVER);
            Log.Info("ID_PRINTER: " + PrintJob.ID_PRINTER);
            Log.Info("ID_DOCUMENT: " + PrintJob.ID_DOCUMENT);
            Log.Info("N_PAGES: " + PrintJob.N_PAGES);
            Log.Info("N_PAGES_PRINTED: " + PrintJob.N_PAGES_PRINTED);
            Log.Info("N_COLORPAGES: " + PrintJob.N_COLORPAGES);
            Log.Info("N_LENGTH: " + PrintJob.N_LENGTH);
            Log.Info("N_WIDTH: " + PrintJob.N_WIDTH);
            Log.Info("ID_MEDIASIZE: " + PrintJob.ID_MEDIASIZE);
            Log.Info("N_MEDIASIZE: " + PrintJob.N_MEDIASIZE);
            Log.Info("ID_ORIENTATION: " + PrintJob.ID_ORIENTATION);
            Log.Info("N_ORIENTATION: " + PrintJob.N_ORIENTATION);
            Log.Info("N_COPIES: " + PrintJob.N_COPIES);
            Log.Info("ID_COLOR: " + PrintJob.ID_COLOR);
            Log.Info("N_COLOR: " + PrintJob.N_COLOR);
            Log.Info("ID_DUPLEX: " + PrintJob.ID_DUPLEX);
            Log.Info("N_DUPLEX: " + PrintJob.N_DUPLEX);
            Log.Info("ID_STATUS: " + PrintJob.ID_STATUS);
            Log.Info("ID_ISPLOTTER: " + PrintJob.ID_ISPLOTTER);
            Log.Info("ID_MEDIATYPE: " + PrintJob.ID_MEDIATYPE);
            Log.Info("N_MEDIATYPE: " + PrintJob.N_MEDIATYPE);
            Log.Info("N_JOBSIZE: " + PrintJob.N_JOBSIZE);
        }


        /** \brief Log via Email del trabajo de impresión
         */
        private void LogPrintJobMail(ref STRUCT_PRINT_JOB PrintJob)
        {
            string TextoMensaje;

            /// Generamos log con las propiedades del trabajo de impresión
            TextoMensaje = ((char)13).ToString();

            TextoMensaje += (char)9 + "- ID_JOBNAME: " + PrintJob.ID_JOBNAME + (char)13;
            TextoMensaje += (char)9 + "- ID_FUENTE: " + PrintJob.ID_FUENTE + (char)13;
            TextoMensaje += (char)9 + "- N_JOB: " + PrintJob.N_JOB + (char)13;
            TextoMensaje += (char)9 + "- F_PRINTJOB: " + PrintJob.F_PRINTJOB + (char)13;
            TextoMensaje += (char)9 + "- ID_LOGIN: " + PrintJob.ID_LOGIN + (char)13;
            TextoMensaje += (char)9 + "- ID_PRINTSERVER: " + PrintJob.ID_PRINTSERVER + (char)13;
            TextoMensaje += (char)9 + "- ID_PRINTER: " + PrintJob.ID_PRINTER + (char)13;
            TextoMensaje += (char)9 + "- ID_DOCUMENT: " + PrintJob.ID_DOCUMENT + (char)13;
            TextoMensaje += (char)9 + "- N_PAGES: " + PrintJob.N_PAGES + (char)13;
            TextoMensaje += (char)9 + "- N_PAGES_PRINTED: " + PrintJob.N_PAGES_PRINTED + (char)13;
            TextoMensaje += (char)9 + "- N_COLORPAGES: " + PrintJob.N_COLORPAGES + (char)13;
            TextoMensaje += (char)9 + "- N_LENGTH: " + PrintJob.N_LENGTH + (char)13;
            TextoMensaje += (char)9 + "- N_WIDTH: " + PrintJob.N_WIDTH + (char)13;
            TextoMensaje += (char)9 + "- ID_MEDIASIZE: " + PrintJob.ID_MEDIASIZE + (char)13;
            TextoMensaje += (char)9 + "- N_MEDIASIZE: " + PrintJob.N_MEDIASIZE + (char)13;
            TextoMensaje += (char)9 + "- ID_ORIENTATION: " + PrintJob.ID_ORIENTATION + (char)13;
            TextoMensaje += (char)9 + "- N_ORIENTATION: " + PrintJob.N_ORIENTATION + (char)13;
            TextoMensaje += (char)9 + "- N_COPIES: " + PrintJob.N_COPIES + (char)13;
            TextoMensaje += (char)9 + "- ID_COLOR: " + PrintJob.ID_COLOR + (char)13;
            TextoMensaje += (char)9 + "- N_COLOR: " + PrintJob.N_COLOR + (char)13;
            TextoMensaje += (char)9 + "- ID_DUPLEX: " + PrintJob.ID_DUPLEX + (char)13;
            TextoMensaje += (char)9 + "- N_DUPLEX: " + PrintJob.N_DUPLEX + (char)13;
            TextoMensaje += (char)9 + "- ID_STATUS: " + PrintJob.ID_STATUS + (char)13;
            TextoMensaje += (char)9 + "- ID_ISPLOTTER: " + PrintJob.ID_ISPLOTTER + (char)13;
            TextoMensaje += (char)9 + "- ID_MEDIATYPE: " + PrintJob.ID_MEDIATYPE + (char)13;
            TextoMensaje += (char)9 + "- N_MEDIATYPE: " + PrintJob.N_MEDIATYPE + (char)13;
            TextoMensaje += (char)9 + "- N_JOBSIZE: " + PrintJob.N_JOBSIZE;
            
            // Se envía un email con información del Trabajo de impresión en curso
            Log.Warn(TextoMensaje);
        }


        /** \brief inicializa las propiedades del Trabajo de Impresión
         * \param PrintJob Estructura con las propiedades del trabjao de impresión
         */
        private void LimpiaStrucPrintJob(ref STRUCT_PRINT_JOB PrintJob) 
        //public static void LimpiaStrucPrintJob(ref STRUCT_PRINT_JOB PrintJob) 
        {
            Log.Debug("Se Inicializa STRUCT_PRINT_JOB PrintJob");
            PrintJob.ID_JOBNAME = this.ID_JOBNAME;
            PrintJob.ID_FUENTE  = "";
            PrintJob.N_JOB = this.N_JOB;
            PrintJob.ID_PRINTER = this.ID_PRINTER;
            PrintJob.N_PAGES = 0;
            PrintJob.N_PAGES_PRINTED = 0;
            PrintJob.N_COLORPAGES = 0;
            PrintJob.N_LENGTH = 0;
            PrintJob.N_WIDTH = 0;
            PrintJob.ID_MEDIASIZE  = "";
            PrintJob.N_MEDIASIZE = -1;
            PrintJob.ID_ORIENTATION = "";
            PrintJob.N_ORIENTATION = -1;
            PrintJob.N_COLOR = -1;
            PrintJob.ID_DUPLEX = "";
            PrintJob.N_DUPLEX = -1;
            PrintJob.N_COPIES = 0;
            PrintJob.ID_STATUS = "";
            PrintJob.ID_MEDIATYPE = "";
            PrintJob.N_MEDIATYPE = 0;
        } //LimpiaStrucPrintJob()


    } //class AnalizeJobId
}
