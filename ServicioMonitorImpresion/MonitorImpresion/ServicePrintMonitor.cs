using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
using System.ServiceProcess;
using System.Timers;
using log4net;
//using System.Text;

namespace MonitorTrabajosImpresion
{
    /** \brief Servicio para activar el monitor de impresión en segundo plano
     * 
     * fuente de información https://www.youtube.com/watch?v=4fH3n8u-Zes
     */
    public partial class ServicePrintMonitor : ServiceBase
    {
        public Timer Tiempo;
        //public string Fecha;
        //public string filename;

        // Se crea el Logger con nombre: ApiImpresion
        private static readonly ILog Log = LogManager.GetLogger("ServicePrintMonitor");

        public ServicePrintMonitor()
        {
            Log.Info("-- Se levanta el Servicio para Monitorizar el Servicio de Impresión --");
            InitializeComponent();
            
            Log.Info("-- Nos suscribimos a los eventos de impresión --");
            EventMonitorPrinting.PrintingEven();

            // Descomentar para suscribirnos a los eventos de creación de ficheros de spool
            Log.Info("Nos suscribimos a los eventos de creación de ficheros de Spool");
            WatchIO.FileEvent(LocalPrinting.PathPrintSpool());

            // Definimos un Timer para lanzar una tarea de forma periodica.
            Tiempo = new Timer();
            Tiempo.Interval = 3600000; // 1 hora
                                       //Tiempo.Interval = 10000; // 10 sg

            //Tiempo.Elapsed += new ElapsedEventHandler(Metodo_delegado a ejecutar);
            //Tiempo.Elapsed += new ElapsedEventHandler(Programa.PruebaServicio);
            //Tiempo.Elapsed += new ElapsedEventHandler(Programa.Inicio);

            // Tarea periodica: Comprobar que estamos suscritos a los eventos de impresión
            Log.Info("Lanzamos EventMonitorPrinting.IsAliveEvents() de forma periódica");
            Tiempo.Elapsed += new ElapsedEventHandler(EventMonitorPrinting.IsAliveEvents);

            // Descomentar para la suscripción a los eventos de creación de ficheros de spool
            Tiempo.Elapsed += new ElapsedEventHandler(WatchIO.IsAliveEvents);

            //Fecha = DateTime.Now.ToShortDateString().Replace("/", "-");

        }

        internal EventMonitorPrinting EventMonitorPrinting
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal WatchIO WatchIO
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
            Tiempo.Enabled = true;
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
        }

        /*
        public void PruebaServicio(object sender, EventArgs e)
        {
            Log.Info("Hola Caracola");

        }
         */

    }
}
