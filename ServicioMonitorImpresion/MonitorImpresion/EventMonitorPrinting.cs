using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader; // Para acceder a los eventos del sistema
using System.Diagnostics;
using System.Xml;
using log4net;
using System.Threading;


namespace MonitorTrabajosImpresion
{
    /**
     * \brief Clase para monitorizar los eventos del Sistema de Impresión.
     * \details
     * Con esta clase nos subscribimos a los eventos del Sistema de Impresión.
     * Detectamos cuando se ha imprimido un documento y cual es su trabajo de impresión asociado.
     * Esta información la pasamos a la clase adecuada con la que se extraen los detalles de impresión
     */
    public class EventMonitorPrinting
    {
        /// Logger para generación de logs: EventMonitorPrinting
        private static readonly ILog Log = LogManager.GetLogger("EventMonitorPrinting");

        /// Definimos el evento impresión _ID_EVEN_CONTROL = 9999 para controlar que si estamos subscritos a los eventos de impresión.
        private const int _ID_EVEN_CONTROL = 9999;

        /// Varible para chequear que se capturan eventos de impresión
        private static bool IsEvenCapture = false;

        /// Ultimo evento capturado
        private static int LastEvent = _ID_EVEN_CONTROL;

        /// Contiene el valor del evento de control _ID_EVEN_CONTROL. Esta propiedad se utiliza en los test automatizados.
        public static int IdEvenControl {
            get
            {
                return _ID_EVEN_CONTROL;
            }
        }

        /// Indica si se está suscrito a los eventos de impresión
        public static bool IsEventSigned
        {
            get
            {
                return IsEvenCapture;
            }
        }

        /// Último evento de impresión capturado
        public static int CurrentEvent
        {
            get
            {
                return LastEvent;
            }
        }

        ///  Estable la relación con la clase AnalizeJobId 
        internal AnalizeJobId AnalizeJobId
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /**
         * \brief Suscripción eventos de impresión.
         * \details se definen los eventos que se quieren monitorizar y suscribimos el metodo delegado EvenCapture que tratara dichos evento de impresión. Los eventos de impresión se guardan en formato XML
         */ 
        public static void PrintingEven()
        {
            Log.Debug(" >>> Método PrintingEven");
            /// - Se define el contexo o canal para Referenciar los eventos del Servicios Impresion: "Microsoft-Windows-PrintService/Operational".
            string Channel = "Microsoft-Windows-PrintService/Operational";

            /// - Se restringen los eventos que se quieren monitorizar, estos los tenemos controlados en la enumeración ENUM_SYSTEM_PRINTING.EVENTS. Se construye una Query similar a esta:
            ///string queryString = "*[(System/EventID=303) or (System/EventID=304) or (System/EventID=306) or (System/EventID=307) or (System/EventID=310) or (System/EventID=800) or " +
            ///    "(System/EventID=801) or (System/EventID=802) or (System/EventID=805) or (System/EventID=812) or (System/EventID=842) or (System/EventID=" + _ID_EVEN_CONTROL + ")]";.
            string queryString = "*[(System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.PRINTER_PAUSED).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.PRINTER_UNPAUSED).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.PRINTER_SET).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.DOCUMENT_PRINTED).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.DOCUMENT_RESUMED).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.DOCUMENT_DELETED).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.JOB_DIAG).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.JOB_DIAG_PRINTING).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.DELETE_JOB_DIAG).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.RENDER_JOB_DIAG).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.FILE_OP_FAILED).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS.PRINT_DRIVER_SANDBOX_JOB_PRINTPROC).ToString() + ")";
            queryString += " or (System/EventID=" + ((int)ENUM_SYSTEM_PRINTING.EVENTS._ID_EVEN_CONTROL).ToString() + ")]";

            /// - Se crea el objeto de consulta de eventos: "eventQueryImpresion" indicando el contexto o canal: "Channel"
            /// y la consulta con los eventos restringidos: "queryString".
            EventLogQuery eventQueryImpresion = new EventLogQuery(Channel, System.Diagnostics.Eventing.Reader.PathType.LogName, queryString);

            /// - Creamos el ojeto visor de eventos: "suscribeEventoImpresion" restringiendolo a los eventos definidos en el objeto
            /// de consulta de eventos: "eventQueryImpresion".
            EventLogWatcher suscribeEventoImpresion = new EventLogWatcher(eventQueryImpresion);

            /// - Suscribimos como delegado al objeto: "suscribeEventoImpresion" el método: "EvenCapture".
            suscribeEventoImpresion.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(EvenCapture);
            IsEvenCapture = true;

            /// - Habilitamos la generación de los eventos para que lance el metodo delegado.
            suscribeEventoImpresion.Enabled = true;
        } // PrintingEven

        /**
         * \brief Metodo delegado con el que se capturamos los eventos de impresión
         * \details Este metodo se lanza cuando se produce un evento de impresion. Se invoca el método AnalizeXmlParameters() con el que se analiza el evento. 
         */
        private static void EvenCapture(object obj, EventRecordWrittenEventArgs arg)
        {
            /// Registra el evento capturado en el Log
            if (arg.EventRecord != null)
            {
                /// Guardamos en el log del servicio MonitorImpresión información del evento capturado.
                Log.Info("====================================================================================================");
                Log.Info("- Capturado el ID de Evento: " + arg.EventRecord.Id + ". Con Registro: " + arg.EventRecord.RecordId + ".");
                Log.Info("====================================================================================================");
                Log.Info("- Description: " + arg.EventRecord.FormatDescription());
                Log.Info("- RecordId: " + arg.EventRecord.RecordId); // Número de registro
                Log.Info("- TimeCreated " + arg.EventRecord.TimeCreated);
                Log.Info("- Número total de Properties: " + arg.EventRecord.Properties.Count);
                Log.Info("____________________________________________________________________________________________________");

                /// Se pasa al método: "AnalizeXmlParameters" la información del evento capturado en formato XML para su análisis.
                AnalizeXmlParameters(arg.EventRecord.ToXml());
            }
            else
            {
                Log.Fatal("EvenCapture(): Se ha capturado un evento de impresión sin embargo no hay información del Evento.");
            }
        } // EvenCapture



        /** 
         * \brief Analisis de los Eventos de Impresión.
         * \details Los eventos de impresión se reciben en formato XML. Por lo que se ha construido un método para analizar los eventos en formato XML
         * \param eventoXML objeto con el contenido XML del evento que se va a analizar
         * \remarks 
         *  Se definen los códigos de eventos de impresión en la enumeración: ENUM_SYSTEM_PRINTING.EVENTS
         */
        private static void AnalizeXmlParameters(Object eventoXML)
        {
            /// Si se ejecuta este método es por que está suscrito a los eventos del Servidor de Impresión.
            /// Establecemos por tanto el atributo para controlar si se están capturando los eventos de impresión: "IsEvenCapture" como verdadero.
            IsEvenCapture = true;

            Log.Debug(" -- Inicio AnalizeXmlParameters: --  " + Thread.CurrentThread.Name);

            /// Se carga el contenido XML del evento en un objeto tipo documento XML 
            XmlDocument xDoc = new XmlDocument();
            //Log.Debug("EvenRecord en XML: " +  eventoXML);
            xDoc.LoadXml((string) eventoXML);

            Log.Debug(eventoXML.ToString());
            

            // Estraemos los Parametros del evento
            XmlNodeList ElementoSystem = xDoc.GetElementsByTagName("System");

            /// Obtenemos el evento.
            XmlNodeList nodosEventID = xDoc.GetElementsByTagName("EventID");
            Log.Debug("Nombre del nodo: " + nodosEventID.Item(0).Name + ", Valor: " + nodosEventID.Item(0).InnerXml);
            Int16 IdEvento = Convert.ToInt16(nodosEventID.Item(0).InnerXml);

            // Se actualza la propiedad LastEven con el Evento de impresión que se está analizando. Necesario para realizar las pruebas unitarias.
            LastEvent = IdEvento;


            /// Se analizan los siguientes eventos de impresión y se extaen los parámetros asociados:
            switch (IdEvento)
            {
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.PRINTER_PAUSED: //303:
                    {
                        /// - Pausar cola impresion
                        XmlNodeList PrinterPaused = xDoc.GetElementsByTagName("PrinterPaused");
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.PRINTER_UNPAUSED: // 304:
                    {
                        /// - Reanudar Impresora pausada
                        XmlNodeList PrinterUnPaused = xDoc.GetElementsByTagName("PrinterUnPaused");
                        foreach (XmlElement nodo in PrinterUnPaused)
                        {
                            // Para indexar el nodo actual de los elementos PrinterUnPaused
                            int i = 0;
                            Log.Info("(Id. Evento: " + IdEvento.ToString() + ")" + "Param1: " + nodo.GetElementsByTagName("Param1")[i].InnerXml);
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.PRINTER_SET: // 306:
                    {
                        /// - Estableciendo configuracion de Impresora. Se comprueba que los trabajos de impresión se guardenen disco, sino, se vuelve a configurar 
                        /// para que se guarden.
                        XmlNodeList PrinterSet = xDoc.GetElementsByTagName("PrinterSet");
                        foreach (XmlElement nodo in PrinterSet)
                        {
                            // Para indexar el nodo actual de los elementos PrinterSet
                            int i = 0;
                            Log.Info("(Id. Evento: " + IdEvento.ToString() + ")" + "Param1: " + nodo.GetElementsByTagName("Param1")[i].InnerXml);
                            string Printer = nodo.GetElementsByTagName("Param1")[i].InnerXml;

                            //Esperamos 1 sg.
                            System.Threading.Thread.Sleep(1000);

                            /// - - Con "ApiImpresion.KeepSpoolFiles(Printer)" aseguramos que se guarden los trabajos de impresión.
                            ApiImpresion PrintServer = new ApiImpresion();
                            PrintServer.KeepSpoolFiles(Printer);

                            i++;
                        }
                        break;
                    }
                case (short) ENUM_SYSTEM_PRINTING.EVENTS.DOCUMENT_PRINTED: //307
                    {
                        /// - Se ha imprimido el documento. Este evento en especial es muy importante por que cuando se detecta hacemos que se desencadene el proceso de análisis 
                        /// del trabajo de impresión con la creación de un objeto de la clase "AnalizeJobId". Este evento lleva asociadas las siguientes propiedades del Trabajo 
                        /// de impresión: 
                        /// - - Número de trabajo de impresión
                        /// - - Nombre de documento
                        /// - - Usuario
                        /// - - Servidor de impresión
                        /// - - Impresora
                        /// - - Puerto
                        /// - - Tamaño en bytes del fichero del trabajo de impresión
                        /// - - Número de hojas
                        XmlNodeList DocumentPrinted = xDoc.GetElementsByTagName("DocumentPrinted");
                        foreach (XmlElement nodo in DocumentPrinted)
                        {
                            // Para indexar el nodo actual de los elementos DocumentPrinted
                            int i = 0;
                            
                            // Log de los parámetros
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param1: " + nodo.GetElementsByTagName("Param1")[i].InnerXml); //JobId
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param2: " + nodo.GetElementsByTagName("Param2")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param3: " + nodo.GetElementsByTagName("Param3")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param4: " + nodo.GetElementsByTagName("Param4")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param5: " + nodo.GetElementsByTagName("Param5")[i].InnerXml); //Printer
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param6: " + nodo.GetElementsByTagName("Param6")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param7: " + nodo.GetElementsByTagName("Param7")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param8: " + nodo.GetElementsByTagName("Param8")[i].InnerXml);
                            string JobId = nodo.GetElementsByTagName("Param1")[i].InnerXml;
                            string Printer = nodo.GetElementsByTagName("Param5")[i].InnerXml;

                            // Se analiza el trabajo de impresión con la clase AnalizeJobId
                            AnalizeJobId MyAnalizeJobId = new AnalizeJobId(Printer, JobId);
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.DOCUMENT_RESUMED: // 309:
                    {
                        /// - Reanudación inpresión
                        XmlNodeList DocumentResumed = xDoc.GetElementsByTagName("DocumentResumed");

                        foreach (XmlElement nodo in DocumentResumed)
                        {
                            // Para indexar el nodo actual de los elementos DocumentPrinted
                            int i = 0;
                            //XmlNodeList Param1 = nodo.GetElementsByTagName("Param1"); //JobId

                            // Log de los parámetros
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param1(JobId): " + nodo.GetElementsByTagName("Param1")[i].InnerXml); //JobId
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param2(Documento): " + nodo.GetElementsByTagName("Param2")[i].InnerXml); //Documento
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param3(Usuario): " + nodo.GetElementsByTagName("Param3")[i].InnerXml); //Usuario
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param4(Printer): " + nodo.GetElementsByTagName("Param4")[i].InnerXml); //Printer
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.DOCUMENT_DELETED: // 310:
                    {
                        /// - Trabajo de impresion eliminado
                        XmlNodeList DocumentDeleted = xDoc.GetElementsByTagName("DocumentDeleted");
                        foreach (XmlElement nodo in DocumentDeleted)
                        {
                            // Para indexar el nodo actual de los elementos DocumentDeleted
                            int i = 0;
                            // Log de los parámetros
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param1: " + nodo.GetElementsByTagName("Param1")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param2: " + nodo.GetElementsByTagName("Param2")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param3: " + nodo.GetElementsByTagName("Param3")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Param4: " + nodo.GetElementsByTagName("Param4")[i].InnerXml);
                            i++;
                        }

                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.JOB_DIAG: // 800:
                    {
                        /// - Poniendo el trabajo en cola
                        XmlNodeList JobDiag = xDoc.GetElementsByTagName("JobDiag");
                        foreach (XmlElement nodo in JobDiag)
                        {
                            // Para indexar el nodo actual de los elementos JobDiag
                            int i = 0;
                            // Log de los parámetros.
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "JobId: " + nodo.GetElementsByTagName("JobId")[i].InnerXml);
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.JOB_DIAG_PRINTING: // 801:
                    {
                        /// - Imprimiendo trabajo.
                        XmlNodeList JobDiag = xDoc.GetElementsByTagName("JobDiag");
                        foreach (XmlElement nodo in JobDiag)
                        {
                            // Para indexar el nodo actual de los elementos JobDiag
                            int i = 0;
                            // Log del parámetro
                            Log.Info("Id. Evento: (" + IdEvento.ToString() + ")" + "JobId: " + nodo.GetElementsByTagName("JobId")[i].InnerXml);                            
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.DELETE_JOB_DIAG: // 802:
                    {
                        /// - Se elimina el  trabajo de impresion
                        XmlNodeList DeleteJobDiag = xDoc.GetElementsByTagName("DeleteJobDiag");

                        foreach (XmlElement nodo in DeleteJobDiag)
                        {
                            // Para indexar el nodo actual de los elementos DeleteJobDiag
                            int i = 0;
                            // Log parámetros                                   
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "JobId: " + nodo.GetElementsByTagName("JobId")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "JobSize: " + nodo.GetElementsByTagName("JobSize")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "DataType: " + nodo.GetElementsByTagName("DataType")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Pages: " + nodo.GetElementsByTagName("Pages")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "PagesPerSide: " + nodo.GetElementsByTagName("PagesPerSide")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "FilesOpened: " + nodo.GetElementsByTagName("FilesOpened")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "JobSizeHigh: " + nodo.GetElementsByTagName("JobSizeHigh")[i].InnerXml);
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.RENDER_JOB_DIAG: //805:
                    {
                        /// - Presentando trabajo impresión.
                        XmlNodeList RenderJobDiag = xDoc.GetElementsByTagName("RenderJobDiag");

                        foreach (XmlElement nodo in RenderJobDiag)
                        {
                            // Para indexar el nodo actual de los elementos RenderJobDiag
                            int i = 0;
                            // Log parámetros
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "JobId: " + nodo.GetElementsByTagName("JobId")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "GdiJobSize: " + nodo.GetElementsByTagName("GdiJobSize")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "ICMMethod: " + nodo.GetElementsByTagName("ICMMethod")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Color: " + nodo.GetElementsByTagName("Color")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "XRes: " + nodo.GetElementsByTagName("XRes")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "YRes: " + nodo.GetElementsByTagName("YRes")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Quality: " + nodo.GetElementsByTagName("Quality")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Copies: " + nodo.GetElementsByTagName("Copies")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "TTOption: " + nodo.GetElementsByTagName("TTOption")[i].InnerXml);
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.FILE_OP_FAILED: //812:
                    {
                        /// - No se pudo eliminar el archivo de spool del trabajo de impresión.
                        XmlNodeList FileOpFailed = xDoc.GetElementsByTagName("FileOpFailed");

                        foreach (XmlElement nodo in FileOpFailed)
                        {
                            // Para indexar el nodo actual de los elementos FileOpFailed
                            int i = 0;
                            // Comunicamos el error
                            Log.Error("Id. Evento: " + IdEvento.ToString() + ")" + "No se puede eliminar el fichero de Spool: " + nodo.GetElementsByTagName("Source")[i].InnerXml);
                            i++;
                        }
                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS.PRINT_DRIVER_SANDBOX_JOB_PRINTPROC:  // 842:
                    {
                        /// - El Servidor de impresión envió el trabajo a la impresora.
                        XmlNodeList PrintDriverSandboxJobPrintProc = xDoc.GetElementsByTagName("PrintDriverSandboxJobPrintProc");
                        foreach (XmlElement nodo in PrintDriverSandboxJobPrintProc)
                        {
                            // Para indexar el nodo actual de los elementos PrintDriverSandboxJobPrintProc
                            int i = 0;
                            // Mostramos los valores recogidos
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "JobId: " + nodo.GetElementsByTagName("JobId")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Processor: " + nodo.GetElementsByTagName("Processor")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Printer: " + nodo.GetElementsByTagName("Printer")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Driver: " + nodo.GetElementsByTagName("Driver")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "IsolationMode: " + nodo.GetElementsByTagName("IsolationMode")[i].InnerXml);
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "ErrorCode: " + nodo.GetElementsByTagName("ErrorCode")[i].InnerXml);
                            i++;
                        }

                        break;
                    }
                case (short)ENUM_SYSTEM_PRINTING.EVENTS._ID_EVEN_CONTROL: // 9999: Generado por nosotros
                    {
                        /// - Evento autogenerado por nuestra aplicación para el control de subscripción a los eventos de Impresión.
                        XmlNodeList RenderJobDiag = xDoc.GetElementsByTagName("EventData");

                        foreach (XmlElement nodo in RenderJobDiag)
                        {
                            int i = 0;
                            // Log parametro 
                            Log.Info("Id. Evento: " + IdEvento.ToString() + ")" + "Data: " + nodo.GetElementsByTagName("Data")[i].InnerXml);
                            i++;
                        }
                        break;
                    }
                default:
                    // Evento de impresión no controlado
                    Log.Error(">>>>>>>>> Id. Evento: " + IdEvento.ToString() + ")" + "No se encuentra el evento " + IdEvento.ToString());
                    break;
            }
        } // Fin ExtractXmlParameters()


        /**
         * \brief Comprobamos si se estan recibiendo los eventos de impresion.
         * Si no se reciben, nos subscribimos a los eventos de impresión.
         */
        public static void IsAliveEvents(object sender, EventArgs Args)
        {
            string Origen, Channel, Data, Equipo;

            /// Definimos un evento personalizado _ID_EVEN_CONTROL = 9999
            /// - Se define el origen del evento: "MonitorPrinting".
            Origen = "MonitorPrinting";
            /// - Se define el canal donde se encuentran los eventos: "Microsoft-Windows-PrintService/Operational"
            Channel = "Microsoft-Windows-PrintService/Operational";
            /// - Se establece el texto informativo del evento creado
            Data = "Control subscripcion a los eventos de impresión desde la aplicación MonitorImpresión";
            Equipo = ".";

            Log.Debug("IsAlivePrintEvents(INICIO) -> Valor de IsEvenCapture: " + IsEvenCapture.ToString());
            
            // Si no existe el origen "MonitorPrinting" en los eventos PrintService lo creamos
            if (!EventLog.Exists(Origen))
            {
                EventSourceCreationData objOrigenEvento = new EventSourceCreationData(Origen, Channel);
            }

            // Creamos un evento de control del Servicio de Impresion
            EventLog objEvento = new EventLog(Channel, Equipo, Origen);

            // Vamos a chequear si estamos suscritos a los eventos del Sistema de Impresión para ello ponemos la variable de control a false.
            Log.Debug("IsAlivePrintEvents -> Ponemos la variable IsEvenCampture = false y registramos un evento 9999");
            IsEvenCapture = false;

            /// Registramos el evento de impresión personalizado.
            objEvento.WriteEntry(Data, EventLogEntryType.FailureAudit, _ID_EVEN_CONTROL);
            Log.Debug("IsAlivePrintEvents -> Después de registrar el evento 9999 y dormimos 3 sg");

            // Esperamos 3 segundos.
            System.Threading.Thread.Sleep(3000);
            Log.Debug("IsAlivePrintEvents -> Despertamos después de 3 sg");
            
            /// Chequeamos la variable de control: "IsEvenCapture" para ver si efectivamente estamos suscritos.
            /// Si estamos subscritos está variable se tenía que haber actualizado a través del metodos suscrito EvenCapture() con el valor true.
            /// En caso de que no estemos suscritos lanzamos el método PrintingEven() para subscribir el método IsEvenCapture(). 
            if (IsEvenCapture)
            {
                Log.Info("Suscripción método EventMonitorPrinting.EvenCapture() a eventos del Sistema Impresión: OK");
            }
            else
            {
                Log.Error("No suscrito a los eventos del Sistema Impresión. Nos suscribimos.");
                try
                {
                    PrintingEven();
                    IsEvenCapture = true;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        } //Fin IsAlivePrintEvents()
    } //Fin Class EventMonitorPrinting
}
