using log4net;
using System.ServiceProcess;


namespace MonitorTrabajosImpresion
{


    class Programa
    {
        // Se crea el Logger con nombre: Programa
        private static readonly ILog Log = LogManager.GetLogger("Programa");

        public ServicePrintMonitor ServicePrintMonitor
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        //static void descomentar_linea_anterior_para_generar_instalador()
        {
            /// Leemos la configuración de App.config para log4net y generar archivos de log
            //Path de los ejecutables del Servicio Monitor de Impresión
            string PathAppConfig = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // Nombre fichero ejecutable del Servicio Monitor de Impresión
            string NameAppConfig = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Log.Info("PATH de App.config: " + PathAppConfig + @"\" + NameAppConfig + ".config");

            // Aviso reinicio servicio
            //Log.Warn("Se reinicia el servicio  Monitor Impresion");

            /// Inicializamos log4net para que recargue configuración en tiempo de ejecución
            System.IO.FileInfo FileAppConfig = new System.IO.FileInfo(PathAppConfig + @"\" + NameAppConfig + ".config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(FileAppConfig);


            Log.Info("");
            Log.Info("");
            Log.Info("");
            Log.Info("");
            Log.Info("");
            Log.Info("");
            Log.Info("");
            Log.Info("===========================================================================================");
            Log.Info("-----------                                                                    ------------");
            Log.Info("-----------                INICIAMOS EL MONITOR DE IMPRESION                   ------------");
            Log.Info("-----------                                                                    ------------");
            Log.Info("===========================================================================================");
            Log.Info("Directorio de Spool: " + LocalPrinting.PathPrintSpool());

            // Revisamos la congiguracion de las impresoras. Se actualizan sus propiedades en caso necesario para que se guarden los documentos impresos
            LocalPrinting.ConfigurePrinters();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServicePrintMonitor()
            };
            ServiceBase.Run(ServicesToRun);
        }


    } // Class Programa


} // Fin NameSpace MonitorTrabajosImpresion

