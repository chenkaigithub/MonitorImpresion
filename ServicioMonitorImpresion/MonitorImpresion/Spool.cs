using System;
using log4net;

namespace MonitorTrabajosImpresion
{
    /** \brief Clase base para análisis de ficheros de Spool
     */
    public class Spool
    {
        /// Path del fichero de spool a analizar.
        protected string PathSpoolFile { get; set; }

        /// Tamaño en bytes del fichero de spool.
        public int SizeFile { get; set; }

        /// Indica si se ha completado el análisis del fichero de Spool.
        public bool AnalisisCompletado { get; set; }

        // Se crea el Logger con nombre: JPLSpool
        private static readonly ILog Log = LogManager.GetLogger("Spool");



        /** \brief En el constructor se guarda el Path del fichero de spool y el tamaño.
         * \details Se comprueba si existe el fichero de spool de impresión y se guarda el Path.
         * \param PathFile Path del fichero de spool a analizar
         */
        protected Spool(string PathFile)
        {
            Log.Debug("---- Constructor Spool. Fichero de spool a analizar: " + PathFile);
            /// Se establece el ditectorio de la cola de impresión
            this.PathSpoolFile = PathFile;

            this.AnalisisCompletado = false;

            /// Si existe el fichero de Spool
            if (Util.ExisteFichero(this.PathSpoolFile))
            {
                /// Se guarda el tamaño del fichero de Spool, necesario para realizar un análisis correcto del mismo.
                this.SizeFile = Util.SizeFile(this.PathSpoolFile);
                Log.Info("Tamaño de fichero de Spool: " + this.SizeFile);
            }
            else
            {
                /// Si no existe el fichero de spool se guarda registro del error en el log y se genera una excepción de error.
                this.SizeFile = 0;
                Log.Error("No existe o no se puede acceder al contenido del fichero: " + this.PathSpoolFile);
                throw new Exception("No existe o no se puede acceder al contenido del fichero: " + this.PathSpoolFile);
            }
        } // Fin Spool()


        /** \brief Con este método se realiza el análisis del fichero de Spool.
         * \details Su implementación se desarrolla en las clases hijas.
         */
        protected void Analize(ref STRUCT_PRINT_JOB PrintJob)
        {

        }


    }
}
