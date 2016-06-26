using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorTrabajosImpresion;
using System.Threading;


//using System.Collections.Generic;
//using System.Collections;
//using System.Runtime.InteropServices;
//using System.ComponentModel;


namespace MonitorImpresionTest
{
    /** \brief Pruebas sobre la clase EvenMonitorPrinting
    */
    [TestClass]
    public class EvenMonitorPrintingTest
    {
        /** \brief Prueba el método "PrintingEven()".
        *   \details  Este método suscribe el método delegado: "EvenCapture" a los eventos de impresión. La prueba consiste en lanzar el método y comprobar:
        *   - Comprobar que se ha suscrito correctamente consultando la propiedad "EventMonitorPrinting.IsEventSigned".
        *   - Comprobar que el último evento capturado es el de control de eventos de impresión.
        */
        [TestMethod]
        public void EvenMonitorPrinting_PrintingEven_SuscripcionSinErrorEventosImpresion()
        {
            /// Inicialización (Arrange)
            ///  - No es necesaria inicialización

            /// Ejecución (Act)
            /// - Suscripción a los eventos de impresión con el método PrintingEven()
            EventMonitorPrinting.PrintingEven();

            /// Comprobación (Assert)
            /// - Comprobar si estamos suscritos a los eventos de Impresión.
            Assert.IsTrue(EventMonitorPrinting.IsEventSigned);
            /// - Comprobar que el evento capturado es el de control.
            Assert.IsTrue(EventMonitorPrinting.CurrentEvent == EventMonitorPrinting.IdEvenControl);
        }


        /** \brief Prueba del método "IsAliveEvent()".
        *   \details  Este método comprueba que se está suscrito a los eventos de impresión y si no se vuelve a suscribir.
        *   - La prueba consiste en lanzar el método y comprobar que el úlitmo evento capturado es el de control.
        *   \remarks Depende de los métodos "PrintingEven" y "EvenCapture". Por tanto con este test probamos 3 métodos.
        */
        [TestMethod]
        public void EvenMonitorPrinting_IsAliveEvent_CompruebaSeEstanCapturandoEventosImpresion()
        {

            /// Inicialización (Arrange)
            /// - Suscripción a los eventos de impresión
            EventMonitorPrinting.PrintingEven();

            /// Ejecución (Act)
            /// - Se lanza el Evento de control: IsAliveEvents()
            object iSender = new object();
            EventArgs iArgs = new EventArgs();
            EventMonitorPrinting.IsAliveEvents(iSender, iArgs);

            /// Comprobación (Assert)
            /// - Al suscribir a los eventos de impresión el úlitmo evento de impresión es el evento de control (el generado por el método)
            Assert.IsTrue(EventMonitorPrinting.CurrentEvent == EventMonitorPrinting.IdEvenControl);
        }
    }

    /** \brief Pruebas sobre la clase Util
    */
    [TestClass]
    public class UtilTest : Util
    {
        /** \brief Prueba el método "MostrarHex()".
        *   \details  Este método devuelve una cadena con la representación hexadecimal de un número. La prueba consiste en lanzar el método con distintos números y comprobar:
        *   - Que se obtiene una cadena con su representación Hexadecimal
        *   \remarks es un método sobrecardo que admite como valores de entrada los tipos: byte, UInt16, Uint32 y byte[].
        */
        [TestMethod]
        public void Util_MostrarHex_ConvierteNumeroEnCadenaRepresentacionHexadecimal()
        {
            /// Inicialización (Arrange)
            /// - Variables con todos los tipos númericos admitidos por el método inicializadas con un valor.
            /// - Variables con la representación en cadena hexadeximal de variables numéricas anteriores.
            byte Numero1byte = (byte)0xAB;
            UInt16 Numero2byte = (UInt16)0xABCD;
            UInt32 Numero4byte = (UInt32)0xAABBCCDD;
            byte[] Arraybytes = { 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };

            string StringNumero1byte = "0xAB";
            string StringNumero2byte = "0xABCD";
            string StringNumero4byte = "0xAABBCCDD";
            string StringArraybytes = "0x0A0B0C0D0E0F";


            /// Ejecución (Act)
            /// - Directamente en el Assert

            /// Comprobación (Assert)
            Assert.IsTrue(MostrarHex(Numero1byte) == StringNumero1byte);
            Assert.IsTrue(MostrarHex(Numero2byte) == StringNumero2byte);
            Assert.IsTrue(MostrarHex(Numero4byte) == StringNumero4byte);
            Assert.IsTrue(MostrarHex(Arraybytes) == StringArraybytes);
        }

        /** \brief Prueba el método "ConvertStringHexToInt()".
        *   \details  Convierte una cadena que contiene la representación de un número Hexadecimal en formato "little endian" en un Int32. 
        *   - Se comprueba que dada una cadena con un número hexadecimal en formato little endian el método devuelve su valor en un Int32 correctamente.
        */
        [TestMethod]
        public void Util_ConvertStringHexToInt_ConvierteCadenaHexadecimalEnEntero()
        {
            /// Inicialización (Arrange)
            /// - Se asigna a una cadena la representación de un número hexadecimal en Little Endian.
            string EnteroHex = "0x45AF002F"; // => "0x2F00AF45" (Big endian)

            /// Ejecución (Act)
            Int32 Entero  = 788574021; // "0x2F00AF45"

            /// Comprobación (Assert)
            Assert.IsTrue(ConvertStringHexToInt(EnteroHex) == Entero);
        }

        /** \brief Prueba el método "DevmodeString32()".
        *   \details  Con este método a partir de un array (byte[]) de 64 bytes genera una cadena de 32 caracteres. A partir de cada pareja de bytes del array se genera un caracter sólo con el primer byte (el segundo byte se ignora por que no tiene contenido). Comprobar que:
        *   - A partir de un byte[] de 64 bytes se obtiene una cadena de 32 caracteres con los bytes significativos.
        */
        [TestMethod]
        public void Util_DevmodeString32_ConvierteArrayTipoByteLongitud64enStringLongitud32ParejaBytesQuitaSegundo()
        {
            /// Inicialización (Arrange)
            /// - Inicializamos un array 64 bytes.
            /// - Inicializamos una cadena con el resultado que debería devolver el método a partir del array de 64 bytes.
            byte[] Array64 = { (byte)'H' ,  0,
                               (byte)'o' ,  0,
                               (byte)'l' ,  0,
                               (byte)'a' ,  0,
                               (byte)' ' ,  0,
                               (byte)'c' ,  0,
                               (byte)'a' ,  0,
                               (byte)'r' ,  0,
                               (byte)'a' ,  0,
                               (byte)'c' ,  0,
                               (byte)'o' ,  0,
                               (byte)'l' ,  0,
                               (byte)'a' ,  0,
                               (byte)',' ,  0,
                               (byte)' ' ,  0,
                               (byte)'H' ,  0,
                               (byte)'o' ,  0,
                               (byte)'l' ,  0,
                               (byte)'a' ,  0,
                               (byte)' ' ,  0,
                               (byte)'c' ,  0,
                               (byte)'a' ,  0,
                               (byte)'r' ,  0,
                               (byte)'a' ,  0,
                               (byte)'c' ,  0,
                               (byte)'o' ,  0,
                               (byte)'l' ,  0,
                               (byte)'a' ,  0,
                               (byte)',' ,  0,
                               (byte)' ' ,  0,
                               (byte)'H' ,  0,
                               (byte)'o' ,  0,
            };
            string cadena = "Hola caracola, Hola caracola, Ho";

            /// Ejecución (Act)
            /// - Obtenemos una cadena a partir del método ejecutado con el array definido.
            string cadena32 = DevmodeString32(Array64);
            Console.WriteLine("Contenido del array de longitud 64: *" + cadena32 + "* y de la cadena a comparar: *" + cadena + "*");

            /// Comprobación (Assert)
            /// - Comprobamos si la cadena obtenida es la que se esperaba
            Assert.IsTrue(cadena.Equals(cadena32));
        }


        /** \brief Prueba el método "ArrayCharToByte()".
        *   \details  Este método convierte un array de char (char[]) en un array de bytes (byte[]).
        *   - Para probarlo definimos un string con un valor determinado y guardamos su representación en un array de char.
        *   - Al array de char le aplicamos el método que estamos probando, ArrayCharToByte(), obteniendo su representación en un array de byte.
        *   - Al array de byte le apliacamos el método ConverArrayToString(). El resultado debe ser el string original
        *   /remarks Con esta prueba además del método "ArrayCharToByte()" probamos el método "ConverArrayToString()"
        */
        [TestMethod]
        public void Util_ArrayCharToByte_ConvierteArraDeCharEnArrayDeByte()
        {
            /// Inicialización (Arrange)
            /// - Inicializamos un string
            /// - Inicializamos un char[] con la representación del string anterior en un array de char
            string cadena = "Hola caracola"; 
            char[] arrayChar = cadena.ToCharArray();

            /// Ejecución (Act)
            /// - Ejecutamos el método a probar, "ArrayCharToByte()",con el char[] y obtenemos un byte[]
            /// - Convertimos el byte[] en un string 
            byte[] arrayByte = ArrayCharToByte(arrayChar);
            string cadenaDesdeArrayByte = ConverArrayToString(arrayByte);

            /// Comprobación (Assert)
            // Comprobamos que el contenido del string original y el obtenido tras la ejecución es la mismo
            Assert.IsTrue(cadena.Equals(cadenaDesdeArrayByte));
        }

        /** \brief Prueba el método "ContieneTexto()"
        *   \details Dada una cadena se comprueba si contiene un texto. No se distingue si el texto a buscar está en mayúsculas o o minúsculas.
        *   Para probarlo definimos una cadena y buscamos una Texto con varias representaciones equivalentes si no se tiene en cuenta minúsculas/mayúsculas.
        */
        [TestMethod]
        public void Util_ContieneTexto_ComprobarTextoContenidoenCadena()
        {

            /// Inicialización (Arrange)
            /// - Cadena
            /// - Texto

            string Cadena = "Esto es un Plotter de prueba";
            string Texto1 = "Plotter";
            string Texto2 = "PloTtEr";
            string Texto3 = "PLOTTER";
            string Texto4 = "PlOtTeR";

            bool Resul = false;

            /// Ejecución (Act)
            Resul = ContieneTexto(Cadena, Texto1);
            Resul = Resul && ContieneTexto(Cadena, Texto2);
            Resul = Resul && ContieneTexto(Cadena, Texto3);
            Resul = Resul && ContieneTexto(Cadena, Texto4);

            /// Comprobación (Assert)
            Assert.IsTrue(Resul);
        }



    } // UtilTest

    /** \brief Pruebas sobre la clase EmfSpool
    */
    [TestClass]
    public class EmfSpoolTest
    {

        /** \brief Prueba el método "AnalizeEmf()".
        *   \details  Este método análiza ficheros de spool con contenido EMF. La prueba consiste en utilizar el método con todos los ficheros de Spool con contenido EMF contenidos
        *   en un directorio y comprobar:
        *   - Que no se produce ningún error durante el análisis de los ficheros de Spool.
        *   \remarks para esta prueba también se utilizan los métodos: EmfSpool, LeerDevmode y AnalizeEMR. Por lo que en esta prueba estamos probando la clase EmfSpool completa.
        */
        [TestMethod]
        public void EmfSpool_AnalizeEmf_AnalizaFicherosSpoolEMF()
        {
            // Struct para almacenar detalles de impresión (el contenido para el test es indiferente
            STRUCT_PRINT_JOB PrintJob = new STRUCT_PRINT_JOB();
            EmfSpool MiEmfSpool;
            // Path con los ficheros de Spool
            //string Path = @"D:\SpoolImpresora\Test\EMF";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF";
            //string Path = @"D:\SpoolImpresora\Test";


            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2015-07-23";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-03-30";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-04-01";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-04-07";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-04-27_2016-04-29";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-05-05";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-04-18_2016-04-29";


            //string Path = @"D:\TestMonitorImpresion\EMF";
            string Path = @"D:\TestMonitorImpresion\PlotterEMF";


            
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-04-12";
            //string Path = @"D:\SpoolImpresora\Test\PlotterEMF\2016-04-15";
            //string Path = @"D:\SpoolImpresora";


            // Directorio donde se encuentran los ficheros de Spool
            DirectoryInfo Directorio = new DirectoryInfo(Path);

            foreach (var Archivo in Directorio.GetFiles("*.SPL"))
            {
                /// Inicialización (Arrange)
                /// - Inicializamos la clase para analizar un fichero de Spool
                MiEmfSpool = new EmfSpool(Path + @"\" + Archivo);

                /// Ejecución (Act)
                /// - Ejecutamos el método 
                PrintJob = new STRUCT_PRINT_JOB();
                MiEmfSpool.Analize(ref PrintJob);
                AnalizeJobId.LogPrintJob(ref PrintJob);

                /// Comprobación (Assert)
                /// - Se comprueba que el fichero de Spool tiene contenido (no está vacio)
                /// - Comprobamos que se ha analizado sin errores el fichero de Spool completo
                Assert.IsTrue(MiEmfSpool.AnalisisCompletado && (MiEmfSpool.SizeFile > 0));
            }
        }

    }

    /** \brief Pruebas sobre la clase JPLSpoolTest
    */
    [TestClass]
    public class JPLSpoolTest
    {
        /** \brief Prueba el método "AnalizePJL-PCLXL()".
        *   \details  Este método análiza ficheros de spool con contenido PJL/PCLXL. La prueba consiste en utilizar el método con todos los ficheros de Spool con contenido PJL/PCLXL contenidos
        *   en un directorio y comprobar:
        *   - Si los ficheros tienen contenido JPL y PCLXL se comprueba que tanto el análisis JPL como el PCLXL no han producido ningún error y se ha analizado el contenido del fichero completo.
        *   - Si el fichero tiene contenido JPL pero no tiene contenido PCLXL, caso de algún plotter que genera contenido HP-GL2, sólo se comprueba que el análisis JPL se ha completado.
        *   \remarks Al comprobar este metodo se está comprobando al mismo tiempo el cosntructor de la clase JPLSpool(), ReadStringJPL(), AnalizeStringJPL(), PositionValueJPL() y JPLToPCLXL().
        *   también se está probando la clase PCLXLSpool. 
        */
        [TestMethod]
        public void JPLSpool_AnalizePJLPClXl_AnalizaContenidoSpoolPJLPClXl()
        {
            // Struct para almacenar detalles de impresión (el contenido para el test es indiferente
            STRUCT_PRINT_JOB PrintJob; // = new STRUCT_PRINT_JOB();
            JPLSpool MiJPLSpool;
            //string Path = @"D:\SpoolImpresora\Test\JPL-PCLXL";
            string Path = @"D:\TestMonitorImpresion\JPL-PCLXL";
            //string Path = @"D:\TestMonitorImpresion";


            //string Path = @"D:\SpoolImpresora\Test\PlotterJPLPCLXL";

            // ************** Test con 231 trabajos de impresión pasado correctamente. 8 minutos y 54 segundos ******************
            //string Path = @"D:\SpoolImpresora\Test\JPL-PCLXL\HP5500";
            // *****************************************************************************************

            //string Path = @"D:\SpoolImpresora\Test\JPL-PCLXL\HP5500\01_LOTE";
            //string Path = @"D:\SpoolImpresora\Test";




            // Directorio donde se encuentran los ficheros de Spool
            DirectoryInfo Directorio = new DirectoryInfo(Path);

            foreach (var Archivo in Directorio.GetFiles("*.SPL"))
            {
                //Console.WriteLine(archivo.Name);
                /// Inicialización (Arrange)
                MiJPLSpool = new JPLSpool(Path + @"\" + Archivo);

                /// Ejecución (Act)
                PrintJob = new STRUCT_PRINT_JOB();
                MiJPLSpool.Analize(ref PrintJob);
                AnalizeJobId.LogPrintJob(ref PrintJob);
                

                /// Comprobación (Assert)
                if (MiJPLSpool.ContenidoPCLXL)
                {
                    Assert.IsTrue(MiJPLSpool.AnalisisCompletado && MiJPLSpool.AnalisisPCLXLCompletado && (MiJPLSpool.SizeFile > 0));
                }
                else
                {
                    Assert.IsTrue(MiJPLSpool.AnalisisCompletado && (MiJPLSpool.SizeFile > 0));
                }

            }
        }
    }

    /** \brief Pruebas sobre la clase Tags
    */
    [TestClass]
    public class TagsTest
    {
        /** \brief Prueba el método "TotalTags()".
        *   \details  Este método devuelve el número total de Tags insertados en un objeto de la clase Tags. La prueba consiste en:
        *   - Crer un objeto de la clase Tags y añadir varios Tags.
        *   - Comprobar que el número total de tags del objeto coincide con el número de tags agregados.
        *   /remarks Esta clase se utiliza para guardar guardar en el Log un resumen de los Tags utilizados en el análisis de los ficheros de Spool (Tags EMF y PCLXL).
        *   Además del método TotalTags() para hacer la prueba se necesita utilizar el método Agregar(). Y para ver la salida de la prueba se utiliza el método ResumenTags()
        */
        [TestMethod]
        public void Tags_TotalTags_ComprobarElTotaldeTagsIntroducidos()
        {
            /// Inicialización (Arrange)
            /// - Se crea un objeto tipo Tags
            Tags MisTags = new Tags();

            /// Ejecución (Act)
            /// - Se agregan varios Tags, donde se repiten algunos de ellos. Total 10 elementos
            MisTags.Agregar("Uno");
            MisTags.Agregar("Dos");
            MisTags.Agregar("Dos");
            MisTags.Agregar("Tres");
            MisTags.Agregar("Tres");
            MisTags.Agregar("Tres");
            MisTags.Agregar("Cuatro");
            MisTags.Agregar("Cuatro");
            MisTags.Agregar("Cuatro");
            MisTags.Agregar("Cuatro");

            MisTags.ResumenTags();

            /// Comprobación (Assert)
            /// - Se comprueba que el método devuelve el resultado esperado.
            Assert.IsTrue(MisTags.TotalTags() == 10);
        }

    }

    /** \brief Pruebas sobre la clase ApiImpresion
    *   \details Para probar esta clase se debe hacer revisando los logs generados. Los métodos no devuelven ningún valor y no se guarda estado de los métodos utilizados.
    *   Se pueden probar de forma automatizada KeepSpoolFiles() y de forma indirecta los métodos utilizados: OpenPrinter(), ClosePrinter().
    */
    [TestClass]
    public class ApiImpresionTest
    {
        /** \brief Prueba el método "KeepSpoolFiles()".
        *   \details  Con este método se revisa la propiedad de la impresora que indica si se conserva el documento después de su impresión. La prueba consiste en lanzar el método para la impresora virtual PDFCreator.
        *   - Cambiar la propiedad de la impresora para que no conserve los documentos después de su impresión y lanzar el test comprobando que no se devuelve ningín error y se ha vuelto a conficurar la propiedad para que conserver los trabajos de impresión.
        */
        [TestMethod]
        public void ApiImpresion_KeepSpoolFiles_SeConfiguranImpresorasParaGuardarFicheroSpool()
        {



            /// Inicialización (Arrange)
            /// - Impresora a configurar
            /// - Objeto de la clase ApiImpresion
            string Printer = "PDFCreator";
            ApiImpresion PrintServer = new ApiImpresion();


            /// Ejecución (Act)
            /// - Directamente en el assert

            /// Comprobación (Assert)
            Assert.IsTrue(PrintServer.KeepSpoolFiles(Printer));
        }
    }


    /** \brief Pruebas sobre la clase WatchIO
     */
    [TestClass]
    public class WatchIOTest
    {

        /** \brief Prueba el método "FileEven()".
        *   \details  Este método suscribe el método delegado: "FileEven" a los eventos del sistema de Archivos. La prueba consiste en lanzar el método y comprobar:
        *   - Comprobar que se ha suscrito correctamente consultando la propiedad "WatchIO.IsEventSigned".
        */
        [TestMethod]
        public void WatchIO_FileEven_SuscripcionSinErrorEventosSistemaDeArchivos()
        {
            /// Inicialización (Arrange)
            ///  - No es necesaria inicialización
            //string Path = LocalPrinting.PathPrintSpool();
            string Path = @"D:\TestMonitorImpresion\Prueba";

            /// Ejecución (Act)
            /// - Suscripción a los eventos de impresión con el método PrintingEven()
            WatchIO.FileEvent(Path);
            
            /// Comprobación (Assert)
            /// - Comprobar si estamos suscritos a los eventos de Impresión.
            Assert.IsTrue(WatchIO.IsEventSigned);
        }

        /** \brief Prueba del método "IsAliveEvent()".
        *   \details  Este método comprueba que se está suscrito a los eventos del sistema de archivos y si no se vuelve a suscribir.
        *   - La prueba consiste en lanzar el método y comprobar que se haceel úlitmo evento capturado es el de control.
        *   \remarks Depende de los métodos "PrintingEven" y "EvenCapture". Por tanto con este test probamos 3 métodos.
        */
        [TestMethod]
        public void WatchIO_IsAliveEvent_CompruebaSiseEstanCapturandoEventosDelSistemaDeArchivos()
        {

            /// Inicialización (Arrange)
            /// - Suscripción a los eventos de impresión
            //EventMonitorPrinting.PrintingEven();
            string Path = LocalPrinting.PathPrintSpool();
            WatchIO.FileEvent(Path);

            /// Ejecución (Act)
            /// - Se lanza el Evento de control: IsAliveEvents()
            object iSender = new object();
            EventArgs iArgs = new EventArgs();
            WatchIO.IsAliveEvents(iSender, iArgs);

            /// Comprobación (Assert)
            /// - Al suscribir a los eventos del sistemas de Archivos se comprueba después del método IsAliveEvent que seguimos suscritos por que se copian los ficheros
            Assert.IsTrue(WatchIO.IsEventSigned);
        }


        /** \brief Detectar si un fichero está bloqueado antes de copiarlo.
        *   \details   Detectar si deja de estar bloqueado y hacer una copia del fichero
        *   - Se comprueba que se ha podido hace una  una copia del fichero
        *   - Comprobar que ...........
        */
        [TestMethod]
        public void WatchIO_IsFileLocked_ComprobarSiUnFicheroEstaBloqueado()
        {
            // Fecha para guardar un backup existente de fichero de spool
            string Fecha = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "_" +
                           DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();

            // Tiempo de espera
            //const int _TIEMPO_ESPERA = ElapsedTime._TIEMPO_ESPERA;
            ElapsedTime TiempoEspera;
            
            // Objeto con información de un fichero
            FileInfo MiFileInfo;
            // Path donde se encuentran los ficheros a comprobar
            string Path;
            // Subdirectorio de backup
            string PathBackup;
            // Directorio donde se encuentran los ficheros de Spool
            DirectoryInfo Directorio;
            // Indica si el archivo está bloqueado
            bool FileLocked = true;
            // Información de archivo y su directorio de Backup
            STRUCT_WATCHIO.BackupFile FileBackup;
            // Thread para lanzar el método delegado para copiar archivos
            Thread ThreadBackupFile;


            /// Inicialización (Arrange)
            Path = @"D:\TestMonitorImpresion\Prueba";
            PathBackup = "MonitorImpresion";
            Directorio = new DirectoryInfo(Path);


            foreach (var Archivo in Directorio.GetFiles("*.SPL"))
            {

                // Información del archivo
                //MiFileInfo = new FileInfo(Archivo.FullName);


                // Guardamos información en estructura para hacer una copia del archivo
                FileBackup = new STRUCT_WATCHIO.BackupFile();
                FileBackup.PathDirectory = Path;
                FileBackup.FileName = Archivo.Name;
                FileBackup.PathDirectoryBackup = PathBackup;
                FileBackup.FileNameBackup = Archivo.Name.ToUpper().Replace(".SPL", "") + "_" + Fecha + ".SPL";


                // THREADING para copiar fichero
                ThreadBackupFile = new Thread(new ParameterizedThreadStart(Util.BackupFile));
                ThreadBackupFile.Name = "Thread_BackupFile_" + Archivo.Name;
                ThreadBackupFile.Start(FileBackup);

                // Detectar si el fichero está bloqueado
                //FileLocked = WatchIO.IsFileLocked(FileBackup.PathDirectory + @"\" + FileBackup.PathDirectoryBackup + @"\" + FileBackup.FileNameBackup, _TIEMPO_ESPERA);
                MiFileInfo = new FileInfo(FileBackup.PathDirectory + @"\" + FileBackup.PathDirectoryBackup + @"\" + FileBackup.FileNameBackup);

                // Creamos un objeto para controlar un tiempo de espera
                TiempoEspera = new ElapsedTime(20);
                FileLocked = WatchIO.IsFileLocked(MiFileInfo);
                while ((!TiempoEspera.OverElapsedTime()) & FileLocked)
                {
                    FileLocked = WatchIO.IsFileLocked(MiFileInfo);
                }
                
                // Esperamos hasta que finaliza el Thread para hacer un backup
                ThreadBackupFile.Join();

                /// Comprobación (Assert)
                Assert.IsTrue(!FileLocked);
            }
        }
    }

    ///** \brief Pruebas sobre la clase Prueba
    //*/
    //[TestClass]
    //public class PruebaTest
    //{
    //    /** \brief Prueba el método "metodo()".
    //    *   \details  Este método ... que hace ........ La prueba consiste en .......... y comprobar:
    //    *   - Comprobar que ............
    //    *   - Comprobar que ...........
    //    */
    //    [TestMethod]
    //    public void NombreMetodo_ComparaDosNumeros_PrimeroMayorQueSegundo()
    //    {
    //        /// Inicialización (Arrange)
    //        int Primero = 20;
    //        int Segundo = 10;

    //        /// Ejecución (Act)
    //        int Diferencia = Primero - Segundo;

    //        /// Comprobación (Assert)
    //        Assert.IsTrue(Diferencia > 0);
    //    }
    //}
}
