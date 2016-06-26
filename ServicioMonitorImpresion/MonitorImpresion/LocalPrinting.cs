using System;
using System.Printing;
using log4net;


namespace MonitorTrabajosImpresion
{
    /** \brief Obtenemos propiedades de Impresión con System\.Printing.
     * \details Obtenemos las propiedades de los trabajos de impresión del servidor de impresión local utilizando el
     * namespace System.Printing
     * 
     * \remarks Como mejora podíamos implementar un método que guardase en BD los distintos tamaños de papel soportados por cada impresora
     * de la que se captura un trabajo de impresión.
     * 
     * Registro:
     *  - Path Spool Impresion: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Print\Printers
     *  - Guardar trabajos de impresión: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Print\Printers\HP Universal Printing PCL 6\DsSpooler\printKeepPrintedJobs
     *  - http://www.undocprint.org/winspool/registry
     *  - http://stackoverflow.com/questions/12110010/setting-printers-keepprinteddocuments-property-in-net
     *  - http://www.lessanvaezi.com/changing-printer-settings-using-the-windows-api/
     *  - http://www.vbforums.com/showthread.php?733849-How-to-determine-if-a-printer-is-connected-and-available&p=4507829 Values Atributte en PRINTER_INFO_5
     *  - Atributos de estructura PRINTER_INFO_5 en [MS-RPRN].pdf
     */
    public class LocalPrinting
    {

        // Se crea el Logger con nombre: LocalPrinting
        private static readonly ILog Log = LogManager.GetLogger("LocalPrinting");

        public static string PathPrintSpool()
        {
            LocalPrintServer ServidorImpresionLocal = null;
            try
            {
                ServidorImpresionLocal = new LocalPrintServer();
                //Log.Debug("Directorio de Spool del servidor de Impresión '" + ServidorImpresionLocal.Name + "': " + ServidorImpresionLocal.DefaultSpoolDirectory.ToUpper());
                return ServidorImpresionLocal.DefaultSpoolDirectory;
            }
            catch (Exception e)
            {
                Log.Error("No se puede acceder al Servidor de Impresion Local.");
                return null;
            }
        }

        /** \brief Detalles del Trabajo de Impresion
         *  \param Printer Nombre de impresora
         *  \param JobId Número de trabajo de impresión
         *  \param PrintJob Estructura para almacenar las propiedades del trabajo de impresión, pasado por referencia
         *  \details 
         * Con este método recuperamos los detalles del trabajo de impresión: Cola, Número de Páginas, Tamaño página, Usuario, etc.
         *  \remarks Hay que revisar si la propiedad PrintQueue.IsRawOnlyEnabled = False indica que el spool del trabajo
         *  se almacena en formato EMF.
         */
        public static void DetailsPrint(string Printer, string JobId, ref STRUCT_PRINT_JOB PrintJob)
        {
            PrintQueue ColaImpresion = null;
            PrintSystemJobInfo JobInfo = null;

            Log.Debug("=========================================================================");
            Log.Debug("                        NAMESPACE System.Printing");
            Log.Debug("=========================================================================");
            Log.Debug("");

            /// Conectamos con el Servidor de impresión local
            LocalPrintServer ServidorImpresionLocal = null;
            try
            {
                ServidorImpresionLocal = new LocalPrintServer();
                Log.Debug("==> Servidor de Impresión: " + ServidorImpresionLocal.Name);
            }
            catch (Exception e)
            {
                Log.Error("No se puede abrir el Servidor de Impresión Local: " + e);
                throw e;
            }

            /// Abrimos la impresora y su trabajo de Impresión pasados como argumentos al método
            Log.Debug("==> Trabajo de impresión: " + JobId + " a través de la  Cola de Impresión: " + Printer);
            try
            {
                Log.Debug("Abrimos conexión con impresora: " + Printer);
                ColaImpresion = (PrintQueue)ServidorImpresionLocal.GetPrintQueue(Printer);
                Log.Debug("Recuperamos trabajo de Impresión: " + JobId);
                ColaImpresion.GetJob(Convert.ToInt32(JobId));
                ColaImpresion.Refresh();

                /// - Generamos log con las propiedades del Trabajo de Impresión
                if (ColaImpresion.IsRawOnlyEnabled)
                {
                    Log.Debug("==> La cola de impresión sólo utiliza RAW.");
                }
                else
                {
                    Log.Debug("==> La cola de impresión admite EMF.");
                }

                /// Se guarda en el log los detalles del trabajo de impresión
                Log.Debug("PageMediaSizeName: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.PageMediaSizeName);
                Log.Debug("PageMediaSize: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize);
                Log.Debug("Width: " +  ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.Width);
                Log.Debug("Height: " +  ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.Height);
                Log.Debug("Número copias: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.CopyCount);
                Log.Debug("OutputColor: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.OutputColor);
                Log.Debug("PageOrientation: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageOrientation);
                Log.Debug("Duplexing: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.Duplexing);
                Log.Debug("OutputColor: " + ColaImpresion.CurrentJobSettings.CurrentPrintTicket.OutputColor);
                Log.Debug("=====>  Propiedades JOB a través PrintSystemJobInfoFO  ====");
                JobInfo = PrintSystemJobInfo.Get(ColaImpresion, Convert.ToInt32(JobId));
                Log.Debug("JobStatus: "  + JobInfo.JobStatus);
                Log.Debug("Name: " + JobInfo.Name);
                Log.Debug("NumberOfPages: " + JobInfo.NumberOfPages);
                Log.Debug("NumberOfPagesPrinted: " + JobInfo.NumberOfPagesPrinted);
                Log.Debug("Submitter: " + JobInfo.Submitter);
                Log.Debug("TimeJobSubmitted: " + JobInfo.TimeJobSubmitted.ToString());
                Log.Debug("JobSize: " + JobInfo.JobSize);

                /// - Almacenamos las propiedades del Trabajo de Impresión en el struct de entrada al método pasado por referencia
                PrintJob.F_PRINTJOB = JobInfo.TimeJobSubmitted.ToLocalTime().Day.ToString().PadLeft(2, '0') + "/" +
                      JobInfo.TimeJobSubmitted.ToLocalTime().Month.ToString().PadLeft(2, '0') + "/" +
                      JobInfo.TimeJobSubmitted.ToLocalTime().Year.ToString().PadLeft(4, '0') + " " +
                      JobInfo.TimeJobSubmitted.ToLocalTime().Hour.ToString().PadLeft(2, '0') + ":" +
                      JobInfo.TimeJobSubmitted.ToLocalTime().Minute.ToString().PadLeft(2, '0') + ":" +
                      JobInfo.TimeJobSubmitted.ToLocalTime().Second.ToString().PadLeft(2, '0');
                PrintJob.ID_JOBNAME = PrintJob.N_JOB.ToString().PadLeft(5, '0') + "_" + PrintJob.F_PRINTJOB;
                PrintJob.ID_FUENTE = "System.Printing";
                PrintJob.ID_LOGIN = JobInfo.Submitter;
                PrintJob.ID_PRINTSERVER = ServidorImpresionLocal.Name;
                PrintJob.ID_DOCUMENT = JobInfo.Name;
                PrintJob.N_PAGES = JobInfo.NumberOfPages;
                PrintJob.N_PAGES_PRINTED = JobInfo.NumberOfPagesPrinted;
                PrintJob.N_LENGTH = (int) ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.Height;
                PrintJob.N_WIDTH = (int) ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.Width;

                PrintJob.ID_MEDIASIZE = ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.ToString(); // PageMediaSizeName;
                PrintJob.N_MEDIASIZE = (int)ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageMediaSize.PageMediaSizeName;
                PrintJob.N_ORIENTATION = (int) ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageOrientation;
                PrintJob.ID_ORIENTATION = ColaImpresion.CurrentJobSettings.CurrentPrintTicket.PageOrientation.ToString();
                PrintJob.N_COPIES = (int) ColaImpresion.CurrentJobSettings.CurrentPrintTicket.CopyCount;
                PrintJob.ID_COLOR = ColaImpresion.CurrentJobSettings.CurrentPrintTicket.OutputColor.ToString();
                PrintJob.N_COLOR = (int) ColaImpresion.CurrentJobSettings.CurrentPrintTicket.OutputColor;
                PrintJob.ID_DUPLEX = ColaImpresion.CurrentJobSettings.CurrentPrintTicket.Duplexing.ToString();
                if (ColaImpresion.CurrentJobSettings.CurrentPrintTicket.Duplexing != null)
                    PrintJob.N_DUPLEX = (int) ColaImpresion.CurrentJobSettings.CurrentPrintTicket.Duplexing;
                PrintJob.ID_STATUS = JobInfo.JobStatus.ToString();
                // ?: Operador condicional ==> (condicion) ? valor_caso_verdadero : valor_caso_falso
                PrintJob.N_COLORPAGES = (PrintJob.ID_COLOR.ToUpper() == "COLOR") ?  PrintJob.N_PAGES_PRINTED: 0;
                PrintJob.N_JOBSIZE = JobInfo.JobSize;
            }
            catch (Exception e)
            {
                Log.Error("No se puede abrir el trabajo de impresión: " + JobId + " a través de la  Cola de Impresión: " + Printer);
                Log.Fatal("Comprobar que en la impresora: " + Printer + " se guardan los trabajos de impresión", e); 
                throw e;
            }
        } // Fin DetailsPrint()



        /**
         * \brief Devuelve el nombre del puerto de la impresora.
         * \param Printer Nombre de la impresora de la que queremos obtener su puerto de impresión.
         * \return Nombre del puerto de la impresosa
         */
        public static string PrinterPortName(string Printer)
        {
            PrintQueue PrintQueue;
            string PortName = "";
            try
            {
                // Conectamos al servidor de Impresión local
                LocalPrintServer localPrintServer = new LocalPrintServer();
                localPrintServer.Refresh();

                // Abrimos la impresora
                PrintQueue = localPrintServer.GetPrintQueue(Printer);
                PrintQueue.Refresh();

                // Obtenemos el nombre del puerto de la impresora
                Log.Debug(">>> Impresora(PrintQueue.Name):   '" + PrintQueue.Name + "'");
                PortName = PrintQueue.QueuePort.Name;
                Log.Info(">Puerto(printQueue.QueuePort.Name):   '" + PrintQueue.QueuePort.Name + "'");

            }
            catch (Exception e)
            {
                Log.Error("LocalPrinting.PrinterPortName. No hemos podido obtener el Nombre del puerto de Impresora.", e);
            }
            return PortName;
        } //Fin PrinterProperties()


        /** 
         * \brief Se establecen las propiedades de las impresoras gestionadas por el Servidor de impresión
         * \details La única propiedad que nos interesa establecer y mantener para poder monitorizar los trabajos de impresión es que
         * "Conserven los documentos después de su impresión" (Se conserven los ficheros de Spool)
         */
        public static void ConfigurePrinters()
        {
            PrintQueue printQueue = null;
            LocalPrintServer localPrintServer;

            try
            {
                /// Accedemos al Servidor de Impresión Local
                localPrintServer = new LocalPrintServer();
                Log.Debug(">>> Servidor de impresion: '" + localPrintServer.Name + "'");

                /// Accedemos a las impresoras gestionadas por el Servidor de Impresión Local
                PrintQueueCollection localPrinterCollection = localPrintServer.GetPrintQueues();
                System.Collections.IEnumerator localPrinterEnumerator =
                    localPrinterCollection.GetEnumerator();

                /// Recorremos todas las impresoras y revisamos su configuración para que se conserven los ficheros de Spool
                while (localPrinterEnumerator.MoveNext())
                {
                    printQueue = (PrintQueue)localPrinterEnumerator.Current;
                    ApiImpresion PrintServer = new ApiImpresion();
                    PrintServer.KeepSpoolFiles(printQueue.Name);
                    printQueue.Refresh();
                } //while
            }
            catch (Exception e)
            {
                Log.Error(e);
            } //try
        } //ConfigurePrinters()

    } //Fin LocalPrinting
}
