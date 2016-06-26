using System;
using log4net;

//namespace WCFDB
namespace WCFDB
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "BDImpresion" en el código y en el archivo de configuración a la vez.
    public class DB : WCFDB.IDB
    {
        // Se crea el Logger con nombre: PCLXLSpool
        private static readonly ILog Log = LogManager.GetLogger("[Class: DB]");

        public DBOracle DBOracle
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /* \brief Metodo para probar la interface WCF
         */
        public string Saludo(string value)
        {
            Log.Info("Prueba metodo WCF: Saludo");
            Log.Info("Hola " + value);
            return string.Format("Hola {0}", value);
            
        }

        /** \brief Lanza una instrucción SQL a la BD.
         * \param SQL string con la sentencia SQL
         * \return bool si se ha realizado la operación correctamente
         */
        public bool EjecutaSQL(string SQL)
        {
            Log.Info(" ---- Inicio ----");
            int NumFilas = 0;

            Log.Info("Se lanza la sentencia SQL: ");
            Log.Info(SQL);
            
            
            try
            {
                /// Filtramos sentencias SQL no permitidas. Sólo inserciones en la tabla SPOOL_PRINTJOBS
                if (!SQL.ToUpper().StartsWith("INSERT INTO SPOOL_"))
                {
                    Log.Error("No es un INSERT sobre la tablas SPOOL_*");
                    throw new Exception("SQL no admitida. " + (char)13 + SQL + (char)13);
                }

                DBOracle DB = new DBOracle();
                bool ok = DB.EjecutaSQL(SQL, ref NumFilas);
                if (ok) Log.Info("SQL Ok. Número de filas afectadas: " + NumFilas.ToString());
                else Log.Info("SQL erronea");
                DB.Dispose();
                return ok;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
        }

    }
}
