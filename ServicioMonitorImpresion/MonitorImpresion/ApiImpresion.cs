using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.ComponentModel;
using log4net;

//[Serializable]

namespace MonitorTrabajosImpresion
{

    public class ApiImpresion
    {

        // Se crea el Logger con nombre: ApiImpresion
        private static readonly ILog Log = LogManager.GetLogger("ApiImpresion");

        // ReSharper disable InconsistentNaming
        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        /** \brief API para interactuar con una impresora
        *   \param pPrinterName Nombre de la impresora
        *   \param phPrinter Identificador interno de impresora
        *   \param pPrinterDefaults Puntero a estructura PRINTER_DEFAULTS(características de la impresora)
        *   \remarks ver URL:https://msdn.microsoft.com/en-us/library/windows/desktop/dd162751(v=vs.85).aspx
        */
        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool OpenPrinter(
            string pPrinterName, //Probar con "doPDF 8"
            out IntPtr phPrinter,
            ref STRUCT_API_PRINTING.PRINTER_DEFAULTS pPrinterDefaults);  // Puede ser un valor nulo     

        /** \brief API para obtener las propiedades de una impresora
        *   \param hPrinter Identificador interno de impresora (obtenido a través de OpenPrinter)
        *   \param Level Nivel o tipo de estructura utilizada para obtener información de impresora. Entre 1-9 niveles.
        *   \param pPrinter Estructura de datos utilizada según el nivel que se indica en el parámetro Level. (Utilizamos la estructura:PRINTER_INFO_5
        *   \param cbBuf tamaño en bytes de la estructura apuntada por pPrinter
        *   \param pcbNeeded Puntero a una variable donde la función guarda el tamaño, en bytes, de los datos con información de la impresora.
        *   \remarks ver URL https://msdn.microsoft.com/es-es/library/windows/desktop/dd144911(v=vs.85).aspx
        */
        [DllImport("winspool.Drv", EntryPoint = "GetPrinterA", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool GetPrinter(
            IntPtr hPrinter,
            uint Level,
            IntPtr pPrinter,
            uint cbBuf,
            out uint pcbNeeded);

        /** \brief API para modificar las propiedades de una impresora
        *   \param hPrinter Identificador interno de impresora (obtenido a través de OpenPrinter)
        *   \param Level Nivel o tipo de estructura utilizada para obtener información de impresora. Entre 1-9 niveles.
        *   \param pPrinter Estructura de datos utilizada según el nivel que se indica en el parámetro Level. (Utilizamos la estructura:PRINTER_INFO_5
        *   \param Command Si el valor es 0. La impresora se reconfigura con los valores contenidos en la estructura apuntada por pPrinter
        *   \remarks ver URL https://msdn.microsoft.com/en-us/library/windows/desktop/dd145082(v=vs.85).aspx
        */
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto), PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static extern bool SetPrinter(
            IntPtr hPrinter,
            uint Level,
            IntPtr pPrinter,
            uint Command);

        /** \brief API para Cerrar la comunicación con una la impresora
        *   \param hPrinter Identificador interno de impresora (obtenido a través de OpenPrinter)
        *   \remarks ver URL:https://msdn.microsoft.com/en-us/library/windows/desktop/dd183446(v=vs.85).aspx
        */
        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        /** \brief API para obtener las propiedades de un trabajo de impresión
        *   \param hPrinter Identificador interno de impresora (obtenido a través de OpenPrinter)
        *   \param JobId Número de trabajo de impresión
        *   \param Level 1:JOB_INFO_1, 2:JOB_INFO_2. Según el detalle que queramos obtener del trabajo de impresión
        *   \param pJob Puntero a estructura indicada en Level
        *   \param cbBuf tamaño en bytes de la estructura apuntada por pJob
        *   \param pcbNeeded Puntero a una variable donde la función guarda el tamaño, en bytes, de los datos con información del trabajo de impresión.
        *   \remarks Ver URL:https://msdn.microsoft.com/en-us/library/windows/desktop/dd144894(v=vs.85).aspx
        */
        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool GetJob(
            IntPtr hPrinter, // Lo obtenemos con la funcion OpenPrinter
            uint JobId, // Número de trabajo de impresión
            uint Level, // 1:JOB_INFO_1, 2:JOB_INFO_2
            IntPtr pJob, // Puntero a estructura indicada en Level
            uint cbBuf,
            ref uint pcbNeeded);

        //public static void AnalizePrintJob(string IImpresora, uint IJobId) //(PRINTER_ENUM Flags) 
        /** \brief Extrae las propiedades de un Trabajo de Impresión.
         * \details A través de las APIs proporcionadas a través de la librería winspool.drv
         * \param Printer string con el nombre de la Impresora
         * \param IJobId Número del trabajo de impresión
         * \param PrintJob struct tipo STRUCT_PRINT_JOB en el que se devuelven las propiedades del trabajo de impresión
         * \return No devuelve nada
         */
        public void Analize(string Printer, uint JobId, ref STRUCT_PRINT_JOB PrintJob) //(PRINTER_ENUM Flags) 
        //public static void Analize(string Printer, uint IJobId, ref STRUCT_PRINT_JOB PrintJob) //(PRINTER_ENUM Flags) 
        {
            STRUCT_API_PRINTING.JOB_INFO_2 JobInfo2; ///> Estructura en la que GetJob2() devuelve las propiedades de impresión
            STRUCT_API_PRINTING.DEVMODE DevMode; ///> Estructura contenido en JOB_INFO_2 

            IntPtr WphPrinter;
            IntPtr WpPrinterEnum= IntPtr.Zero;
            IntPtr WpJob = IntPtr.Zero;
            STRUCT_API_PRINTING.PRINTER_DEFAULTS WpPrinterDefaults = new STRUCT_API_PRINTING.PRINTER_DEFAULTS();
            

            Log.Info("=========================================================================");
            Log.Info("                        API WINSPOOL.drv");
            Log.Info("=========================================================================");
            Log.Info("");

            /// Se abre la conexión con la impresora
            if (OpenPrinter(Printer, out WphPrinter, ref WpPrinterDefaults))
            {
                Log.Info("Este es el contenido de WphPrinter: #" + WphPrinter.ToString() + ".");
            }
            else
            {
                Log.Info("No se ha podido encontrar la impresora: -" + Printer);
            }

            /// Recupera estructura tipo JOB_INFO_2 con las propiedades del trabajo de impresión
            JobInfo2 = GetJob2(WphPrinter, JobId);
            DevMode = (STRUCT_API_PRINTING.DEVMODE)Marshal.PtrToStructure(JobInfo2.pDevMode, typeof(STRUCT_API_PRINTING.DEVMODE));

            /// Se recuperan los valores del trabajo de impresión en el struct STRUCT_PRINT_JOB
            //PrintJob.ID_JOBNAME: Se mantiene el valor que tiene. 
            PrintJob.ID_FUENTE = "API";
            PrintJob.N_JOB = (int)JobInfo2.JobId;
            // PrintJob.F_PRINTJOB. Se mantiene la obtenida, debería ser la obtenida antes a través de System.Printing. El campo JobInfo2.Submitted está sin contenido
            PrintJob.ID_LOGIN = JobInfo2.pUserName;
            PrintJob.ID_PRINTSERVER = JobInfo2.pMachineName;
            PrintJob.ID_PRINTER = JobInfo2.pPrinterName;
            PrintJob.ID_DOCUMENT = JobInfo2.pDocument;
            PrintJob.N_PAGES = (int)JobInfo2.Size;
            PrintJob.N_PAGES_PRINTED = PrintJob.N_PAGES; // Se mantiene el valor que tiene
            PrintJob.N_COLORPAGES = PrintJob.N_PAGES; //Se mantiene el valor que tiene
            PrintJob.N_LENGTH = DevMode.PaperLength;
            PrintJob.N_WIDTH = DevMode.PaperWidth;
            try
            {
                PrintJob.ID_MEDIASIZE = DICTIONARY_API_PRINTING.PAPERSIZE[DevMode.PaperSize];
            }
            catch (Exception e)
            {
                Log.Error("No se encuentra en el diccionario DICTIONARY_API_PRINTING.PAPERSIZE el valor: " + DevMode.PaperSize.ToString());
                PrintJob.ID_MEDIASIZE = "";
            }
            PrintJob.N_MEDIASIZE = DevMode.PaperSize;
            //Orientación papel
            try
            {
                PrintJob.ID_ORIENTATION = DICTIONARY_API_PRINTING.ORIENTATION[DevMode.Orientation];
            }
            catch (Exception e)
            {
                Log.Error("No se encuentra en el diccionario DICTIONARY_API_PRINTING.ORIENTATION el valor: " + DevMode.Orientation.ToString());
                PrintJob.ID_ORIENTATION = "";
            }
            PrintJob.N_ORIENTATION = DevMode.Orientation;

            PrintJob.N_COPIES = DevMode.Copies;
            // Color
            try
            {
                PrintJob.ID_COLOR = DICTIONARY_API_PRINTING.COLORSPACE[DevMode.Color];
            }
            catch (Exception e)
            {
                Log.Error("No se encuentra en el diccionario DICTIONARY_API_PRINTING.COLORSPACE el valor: " + DevMode.Color.ToString());
                PrintJob.ID_COLOR = "";
            }
            PrintJob.N_COLOR = DevMode.Color;
            // duplex
            try
            {
                PrintJob.ID_DUPLEX = DICTIONARY_API_PRINTING.DUPLEX_PAGEMODE[DevMode.Duplex];
            }
            catch (Exception e)
            {
                Log.Error("No se encuentra en el diccionario DICTIONARY_API_PRINTING.DUPLEX_PAGEMODE el valor: " + DevMode.Duplex.ToString());
                PrintJob.ID_DUPLEX = "";
            }
            PrintJob.N_DUPLEX = DevMode.Duplex;
            PrintJob.ID_STATUS = JobInfo2.Status.ToString();
            PrintJob.N_MEDIATYPE = (int)DevMode.MediaType;

            /// Se cierra la conexión con la impresora
            try
            {
                ClosePrinter(WphPrinter);
            }
            catch (Exception ex)
            {
                Log.Error("No se ha podido Cerrar la impresora: -" + Printer + (char)13 + ex);
            }

        } //AnalizePrintJob()


        /** \brief Almacena documentos después de impresión
         * \details Comprueba las propiedades de la impresora para ver si se almacenan los documentos después de su impresión.
         * Si no se almacenan se cambia la propiedad para que se almacen.
         * \param Printer Nombre de la impresora
         * http://www.codeproject.com/Articles/6899/Changing-printer-settings-using-C
         */

        public bool KeepSpoolFiles(string Printer)
        {
            IntPtr WphPrinter;
            STRUCT_API_PRINTING.PRINTER_DEFAULTS WpPrinterDefaults = new STRUCT_API_PRINTING.PRINTER_DEFAULTS();
            var PrinterInfo5 = new STRUCT_API_PRINTING.PRINTER_INFO_5();
            IntPtr pPrinterInfo5 = IntPtr.Zero;
            bool ConfiguracionCorrecta = false;

            /// Abrimos la impresora y obtenemos el identificador interno de la impresora
            if (OpenPrinter(Printer, out WphPrinter, ref WpPrinterDefaults))
            { 
                Log.Info("Este es el contenido de WphPrinter: #" + WphPrinter.ToString() + ".");
                /// Obtenemos las propiedades de la impresora
                PrinterInfo5 = GetPrinter5(WphPrinter);

                ///Cerramos la impresora
                ClosePrinter(WphPrinter);

                /// Establecemos las propiedades para actualizar la impresora
                PrinterInfo5.pPrinterName = Printer;
                PrinterInfo5.pPortName = LocalPrinting.PrinterPortName(Printer);

                Log.Debug("PrinterInfo5.pPrinterName: " + PrinterInfo5.pPrinterName);
                Log.Debug("PrinterInfo5.pPortName: " + PrinterInfo5.pPortName);
                Log.Debug("PrinterInfo5.Attributes: " + PrinterInfo5.Attributes.ToString());
                Log.Debug("PrinterInfo5.device_not_selected_timeout: " + PrinterInfo5.device_not_selected_timeout.ToString());
                Log.Debug("PrinterInfo5.transmission_retry_timeout: " + PrinterInfo5.transmission_retry_timeout.ToString());

                /// Si no se guardan los ficheros del spool de impresión:
                uint flag = (uint)PrinterInfo5.Attributes & (uint)ENUM_API_PRINTING.ATTRIBUTE_PRINTER_INFO.PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS;
                if (flag == 0) // No se guardan, No está activo el bit para guardar los trabajos de impresión
                {
                    Log.Info("La impresora " + Printer + " no guarda los ficheros de Spool de impresión.");

                    /// - Modificamos el atributo de impresora para que guarde los ficheros de Spool
                    PrinterInfo5.Attributes += (uint)ENUM_API_PRINTING.ATTRIBUTE_PRINTER_INFO.PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS;

                    /// - Abrimos la impresora con permiso de administardor para modificar sus propiedades
                    WpPrinterDefaults.DesiredAccess = STRUCT_API_PRINTING.PRINTER_ACCESS_MASK.PRINTER_ACCESS_ADMINISTER;
                    if (!OpenPrinter(Printer, out WphPrinter, ref WpPrinterDefaults))
                        Log.Fatal("No se tiene acceso como Administrador a la impresora: -" + Printer);

                    Log.Info("-- Abrimos la impresora: " + Printer + " en modo Administrador.");

                    // - Asignamos el puntero al struc con las propiedades de la impresora
                    pPrinterInfo5 = Marshal.AllocHGlobal(Marshal.SizeOf(PrinterInfo5));
                    Marshal.StructureToPtr(PrinterInfo5, pPrinterInfo5, false);

                    /// - Establecemos las propiedades
                    SetPrinter5(WphPrinter, ref pPrinterInfo5);

                    /// - Cerramos la impresora
                    Log.Info("-- Cerramos la impresora: " + Printer + " en modo Administrador.");
                    ClosePrinter(WphPrinter);
                } // end if
                ConfiguracionCorrecta = true;
            }
            else
            { 
                Log.Info("No se ha podido encontrar la impresora: -" + Printer);
            } // end if principal
            return ConfiguracionCorrecta;

        } // KeepSpoolFiles()


        /** \brief Devuelve propiedades de un Trabajo de impresión.
         * \details Devuelve 
         * \param hPrinter Impresora por la que se ha lanzado el Trabajo de Impresión
         * \param JobId Número de trajado de impresión
         * \returns JOB_INFO_2: Estructura con las propiedades del trabajo de impresión
         */
        private STRUCT_API_PRINTING.JOB_INFO_2 GetJob2(IntPtr hPrinter, uint JobId)
        {
            var Job = new STRUCT_API_PRINTING.JOB_INFO_2();
            var size = Marshal.SizeOf(typeof(STRUCT_API_PRINTING.JOB_INFO_2));
            uint pcbNeeded = 0;
            STRUCT_API_PRINTING.DEVMODE devmode;

            // Debe dar False. Lo hacemos para obtener el tamaño de los datos de JobId: pcbNeeded
            if (GetJob(hPrinter, JobId, 2, IntPtr.Zero, 0, ref pcbNeeded))
            {
                // No debería ejecutarse nunca el contenido del if
                Log.Error("No se ha obtenido correctamente tamaño del trabajo de impresión");
            }

            // Depurar. 
            int UltimoError = Marshal.GetLastWin32Error();
            Log.Info("El ultimo error Win32: " + UltimoError.ToString());

            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER) //Error 122
            {
                //.-+var pPrinters = Marshal.AllocHGlobal((int)pcbNeeded);
                var pJob = Marshal.AllocHGlobal((int)pcbNeeded);
                Console.WriteLine("Tamaño de pcbNeeded después de GetJob: {0}.", pcbNeeded.ToString());
                // Ahora obtenemos el trabajo con los parámetros adecuados
                if (GetJob(hPrinter, JobId, 2, pJob, pcbNeeded, ref pcbNeeded))
                {
                    /// Generamos log de los detalles obtenidos del trabajo de impresión
                    Job = (STRUCT_API_PRINTING.JOB_INFO_2)Marshal.PtrToStructure(pJob, typeof(STRUCT_API_PRINTING.JOB_INFO_2));
                    Log.Info("Job JobId:       - " + Job.JobId);
                    Log.Info("Job PrinterName       - " + Job.pPrinterName);
                    Log.Info("Job pMachineName       - " + Job.pMachineName);
                    Log.Info("Job pUserName       - " + Job.pUserName);
                    Log.Info("Job pDocument       - " + Job.pDocument);
                    Log.Info("Job pDatatype       - " + Job.pDatatype);
                    Log.Info("Job pStatus       - " + Job.pStatus);
                    Log.Info("Job Status       - " + Job.Status);
                    Log.Info("Job Priority       - " + Job.Priority);
                    Log.Info("Job Position       - " + Job.Position);
                    Log.Info("Job TotalPages       - " + Job.TotalPages);
                    Log.Info("Job PagesPrinted       - " + Job.PagesPrinted);
                    Log.Info("Job Size       - " + Job.Size);
                    devmode = (STRUCT_API_PRINTING.DEVMODE)Marshal.PtrToStructure(Job.pDevMode, typeof(STRUCT_API_PRINTING.DEVMODE));
                    Log.Info("DeviceName       - " + devmode.DeviceName);
                    Log.Info("SpecVersion       - " + devmode.SpecVersion);
                    Log.Info("DriverVersion       - " + devmode.DriverVersion);
                    Log.Info("devmode.Size       - " + devmode.Size);
                    Log.Info("DriverExtra       - " + devmode.DriverExtra);
                    Log.Info("--------------");
                    Log.Info("Fields -> !=NULL ha sido inicializado    - " + devmode.Fields);
                    Log.Info("--------------");
                    Log.Info("Orientation       - " + devmode.Orientation);

                    Log.Info("Utilizamos un Diccionario para mostrar el tipo de papel: " + devmode.PaperSize.ToString());
                    try { 
                        Log.Info("PaperSize       - " + DICTIONARY_API_PRINTING.PAPERSIZE[devmode.PaperSize]);
                    }
                    catch (Exception e)
                    {
                        Log.Error("No se encuentra el valor " + devmode.PaperSize.ToString() + " en el diccionario DICTIONARY_API_PRINTING.PAPERSIZE");
                        Log.Error(e);
                    }

                    Log.Info("PaperSize       - " + devmode.PaperSize);
                    Log.Info("PaperLength       - " + devmode.PaperLength);
                    Log.Info("PaperWidth       - " + devmode.PaperWidth);
                    Log.Info("Scale       - " + devmode.Scale);
                    Log.Info("Copies       - " + devmode.Copies);
                    Log.Info("DefaultSource       - " + devmode.DefaultSource);
                    Log.Info("PrintQuality       - " + devmode.PrintQuality);

                    Log.Info("Color       - " + devmode.Color);
                    Log.Info("Duplex       - " + devmode.Duplex);
                    Log.Info("YResolution       - " + devmode.YResolution);
                    Log.Info("TTOption       - " + devmode.TTOption);
                    Log.Info("Collate       - " + devmode.Collate);
                    Log.Info("FormName       - " + devmode.FormName);

                    Log.Info("LogPixels       - " + devmode.LogPixels);
                    Log.Info("BitsPerPel       - " + devmode.BitsPerPel);
                    Log.Info("PelsWidth       - " + devmode.PelsWidth);
                    Log.Info("PelsHeight       - " + devmode.PelsHeight);

                    Log.Info("Nup       - " + devmode.Nup);

                    Log.Info("DisplayFrequency       - " + devmode.DisplayFrequency);

                    Log.Info("ICMMethod       - " + devmode.ICMMethod);
                    Log.Info("ICMIntent       - " + devmode.ICMIntent);
                    Log.Info("MediaType       - " + devmode.MediaType);
                    Log.Info("DitherType       - " + devmode.DitherType);
                    Log.Info("Reserved1       - " + devmode.Reserved1);
                    Log.Info("Reserved2       - " + devmode.Reserved2);
                    Log.Info("PanningWidth       - " + devmode.PanningWidth);
                    Log.Info("PanningHeight       - " + devmode.PanningHeight);
                    Log.Info("Job pPrintProcessor       - " + Job.pPrintProcessor);
                    Log.Info("Job pParameters       - " + Job.pParameters);


                    //Marshal.FreeHGlobal(devmode);
                    Marshal.FreeHGlobal(pJob);
                    return Job;
                }
                return Job;
            }
            else
            {
                Log.Error("No se ha encontrado el trabajo de impresion");
                return Job;
            } // End if
        } //Fin GetJob2()


        /** \brief recupera Información de una impresora
         * \param WphPrinter Nombre de la impresora
         * \return Estructura tipo PRINTER_INFO_5 con información de la impresora
         * \remarks http://xomalli.blogspot.com.es/2011/01/operadores-nivel-de-bits-en-c-bitwise.html -> Operaciones nivel bit atributos
         */
        private STRUCT_API_PRINTING.PRINTER_INFO_5 GetPrinter5(IntPtr WphPrinter)
        {
            var PrinterInfo5 = new STRUCT_API_PRINTING.PRINTER_INFO_5();
            var size = Marshal.SizeOf(typeof(STRUCT_API_PRINTING.PRINTER_INFO_5));
            uint pcbNeeded = 0;
            uint cbBuf = 0;

            // Llamamos a GetPrinter con un puntero sin tamaño definido InPtr.Zero.
            // Indicamos nivel 5 para estructura de tipo PRINTER_INFO_5
            // para que nos devuelva en pcbNeeded el tamaño con el que tenemos que inicializar

            if (GetPrinter(WphPrinter, 5, IntPtr.Zero, cbBuf, out pcbNeeded))
            {
                Log.Error("No se ha obtenido correctamente pcbNeeded después de GetPrinter: " + pcbNeeded.ToString());
            }

            cbBuf = pcbNeeded;
            Log.Debug("Tamaño de pcbNeeded después de GetPrinter: " + pcbNeeded.ToString());

            int UltimoError = Marshal.GetLastWin32Error();
            Log.Debug(" El ultimo error Win32: " + UltimoError.ToString());

            // Obtenemos los propiedades de la impresora
            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER) // Error 122
            {
                var pPrinterInfo5 = Marshal.AllocHGlobal((int)cbBuf);
                //if (GetPrinter(WphPrinter, 5, pPrinterInfo5, pcbNeeded, ref pcbNeeded))
                if (GetPrinter(WphPrinter, 5, pPrinterInfo5, cbBuf, out pcbNeeded))
                {
                    Log.Info("cbBuf: " + cbBuf.ToString() + " pcbNeeded: " + pcbNeeded.ToString());
                    PrinterInfo5 = (STRUCT_API_PRINTING.PRINTER_INFO_5)Marshal.PtrToStructure(pPrinterInfo5, typeof(STRUCT_API_PRINTING.PRINTER_INFO_5));
                }
                Log.Info("cbBuf: " + cbBuf.ToString() + " pcbNeeded: " + pcbNeeded.ToString());
            }
            return PrinterInfo5;
        } //GetPrinter5()



        /** \brief Establece las propiedades de la impresora.
         * \param WphPrinter Identificador interno de la impresora
         * \param pPrinterInfo5 entre las propiedades de la estructura está que la impresora guarde los ficheros de Spool
         * \remarks http://xomalli.blogspot.com.es/2011/01/operadores-nivel-de-bits-en-c-bitwise.html -> Operaciones nivel bit atributos
         */
        private void SetPrinter5(IntPtr WphPrinter, ref IntPtr pPrinterInfo5)
        {
            /// Se genera información para el log
            Log.Info("-- Dentro de SetPrinter5");
            STRUCT_API_PRINTING.PRINTER_INFO_5 PrinterInfo5 = new STRUCT_API_PRINTING.PRINTER_INFO_5();
            PrinterInfo5 = (STRUCT_API_PRINTING.PRINTER_INFO_5)Marshal.PtrToStructure(pPrinterInfo5, typeof(STRUCT_API_PRINTING.PRINTER_INFO_5));

            Log.Info("PrinterInfo5.pPrinterName: " + PrinterInfo5.pPrinterName);
            Log.Info("PrinterInfo5.pPortName: " + PrinterInfo5.pPortName);
            Log.Info("PrinterInfo5.Attributes: " + PrinterInfo5.Attributes.ToString());
            Log.Info("PrinterInfo5.device_not_selected_timeout: " + PrinterInfo5.device_not_selected_timeout.ToString());
            Log.Info("PrinterInfo5.transmission_retry_timeout: " + PrinterInfo5.transmission_retry_timeout.ToString());


            try
            {
                if (SetPrinter(WphPrinter, 5, pPrinterInfo5, 0))
                {
                    /// Se modifican las propiedades de la impresora y se genera información en el log
                    Log.Info("Se han modificado las propiedades de la impresora '" + PrinterInfo5.pPrinterName + "' para que se guarden los trabajos de impresión.");
                }
                else
                {
                    /// Si no se pueden actualizar las propiedades de la impesora se genera una excepción y se guarda registro del error en el log.
                    int lastError = Marshal.GetLastWin32Error();
                    Log.Error("Codigo de error: " + Marshal.GetLastWin32Error());
                    Log.Error("No se han podido modificar las propiedades de la Impresora.");
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception e)
            {
                Log.Error("No se han podido actualizar las propiedades de la Impresora: '" + PrinterInfo5.pPrinterName + "'", e);
            }
        } //SetPrinter5()
    } // Fin clase ApiImpresion
}
