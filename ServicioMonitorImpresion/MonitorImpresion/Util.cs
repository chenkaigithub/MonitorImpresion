using System;
using log4net;
using System.IO;

namespace MonitorTrabajosImpresion
{
    public class Util
    {
        // Constantes

        public const byte _END_LINE1 = 0x0A;
        public const byte _END_LINE2 = 0x0D;
        public const byte _BINDING_FORMAT_COMILLA = 0x27;
        public const byte _BINDING_FORMAT_PARENTESIS_DERECHO = 0x29;
        public const byte _BINDING_FORMAT_PARENTESIS_IZQUIERDO = 0x28;

        public const byte _ESC = 0x1B;

        // Se crea el Logger con nombre: Util
        private static readonly ILog Log = LogManager.GetLogger("Util");

        /** \brief Convierte un byte como hexadecinal en una cadena
            \param IByte con contenido hexadecimal
            \return cadena con el contenido hexadecimal
        */
        public static string MostrarHex(byte IByte)
        {
            // Convertir byte como hexadecimal en una variable cadena
            string HexValue = IByte.ToString("X");
            return "0x" + HexValue.PadLeft(2, '0');
        }

        /** \brief Convierte un entero16 como hexadecinal en una cadena
            \param Entero16 con contenido hexadecimal
            \return cadena con el contenido hexadecimal
        */
        public static string MostrarHex(UInt16 Entero16)
        {
            string HexValue = Entero16.ToString("X");
            return "0x" + HexValue.PadLeft(4, '0');
        }

        /** \brief Convierte un entero32 como hexadecinal en una cadena
            \param Entero32 Array con contenido hexadecimal
            \return cadena con el contenido hexadecimal
        */
        public static string MostrarHex(UInt32 Entero32)
        {
            string HexValue = Entero32.ToString("X");
            return "0x" + HexValue.PadLeft(8, '0');
        }


        /** \brief Convierte un byte[] con contenido hexadecimal en una cadena
            \param ArrayBytes Array con contenido hexadecimal
            \return cadena con el contenido hexadecimal
        */
        public static string MostrarHex(byte[] ArrayBytes)
        {
            // Convertir byte[] como hexadecimal en una variable cadena
            string HexValue = "";
            for (int i = 0; i < ArrayBytes.Length; i++)
            {
                HexValue += ArrayBytes[i].ToString("X").PadLeft(2,'0');
            }

            return "0x" + HexValue; //.PadLeft(ArrayBytes.Length * 2, '0');

        }

        /** \brief Convierte cadena con número formato hexadecimal a int
         * \param StringHex cadena con número hexadecimal formato little endian
         * \return int con el valor númerico de la cadena de entrada
         */
        public static Int32 ConvertStringHexToInt(string StringHex)
        {
            string Hex = "";
            string Value = "";

            // Si la cadena empieza por "0x" lo quitamos
            if (StringHex.Substring(0, 2) == "0x")
            {
                Hex = StringHex.Substring(2, StringHex.Length - 2);

            }
            else
            {
                Hex = StringHex;
            }
            
            // Cadena hexadecimal a revertir 
            if (BitConverter.IsLittleEndian)
            {   
                // Revertimos los bytes
                for (int i = 0; i < (Hex.Length / 2); i++)
                {
                    Value += Hex.Substring(Hex.Length - (2 * i)  - 2, 2);
                }
            }
            else
            {
                Value = Hex;
            }
            return Convert.ToInt32(Value, 16);
        }

        /** \brief Convierte Array en string
         * \param Array array tipo byte
         * \return string resultado de convertir los elementos del array en char
         */
        public static string ConverArrayToString(byte[] Array)
        {
            string Cadena = "";
            for (int i = 0; i < Array.Length; i++)
            {
                Cadena += (char) Array[i];
            }
            return Cadena;
        }


        /** \brief Extracción blancos de cadena en array de bytes de longitud 64
         * \details Dada una cadena con codificación UNICODE se extrae el primer byte de cada pareja de bytes.
         * Es el carácter ASCII
         * \param Cadena string que hay que analizar
         */
        public static string DevmodeString32(byte [] Cadena)
        {
            string cadena32 = "";
            int i;
            Log.Debug("Dentro de DevmodeString32(string Cadena). Valor de Cadena : " + Cadena);
            
            for (i = 0; i < 63; i = i + 2)
            {
                cadena32 = cadena32 + (char) Cadena[i];
            }
            return cadena32;
        } // Fin DevmodeString32()

        /*
         * Convierte un array de tipo caracter en un array tipo byte.
         * Util para hacer comparaciones en ficheros binarios.
         * Por ejemplo buscar cadena "%-12345X@PJL" en un archivo Binario.
         * Se pasa a la función como parámetro de entrada un array de elementos tipo char:
         * 
         * - byte[] ArrayByte = ArrayCharToByte("%-12345X@PJL".ToCharArray());
         */
        public static byte[] ArrayCharToByte(char[] ArrayChar)
        {
            int i;
            byte[] CharToByte = new byte[ArrayChar.Length];
            for (i = 0; i < ArrayChar.Length; i++)
            {
                CharToByte[i] = (byte)ArrayChar[i];
            }

            Console.Write("Recalculamos cadema a partir de byte: -");
            for (i = 0; i < ArrayChar.Length; i++)
            {
                Console.Write("{0}", (char)CharToByte[i]);
            }
            Console.WriteLine();
            return CharToByte;
        }
        /** \brief Comprobar si existe un fichero
        *   \param Path del archivo
        *   \return valor booleano que indica si existe el fichero
        */
        public static bool ExisteFichero(string Path)
        {
            if (File.Exists(Path))
            {
                Log.Debug("Existe el fichero: " + Path);
                return true;
            }
            else {
                Log.Debug("No existe el fichero: " + Path);
                return false;
            }
        }

        /** \brief Se comprueba si el fichero tiene contenido
        *   \param Path Path del fichero a comprobar
        *   \return Devuelve true si el fichero no tiene contenido
        */
        public static int SizeFile(string Path)
        {
            int SizeFile = 0;
            FileStream Fichero;
            if (ExisteFichero(Path)) { 
                Fichero = File.OpenRead(Path);
                SizeFile = (int)Fichero.Length;
                Fichero.Close();
            }
            return SizeFile;
        }


        /** \brief Comprobar existencia de un texto en una cadena
        *   \details no tiene en mayúsculas o minúculas
        *   \param Cadena donde buscar el texto.
        *   \param Texto a comprobar si existe en la Cadena
        *   \return bool 
        */
        public static bool ContieneTexto(string Cadena, string Texto)
        {
            if (Cadena.ToUpper().Contains(Texto.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /** \brief Backup del fichero indicado como parámetro
         *  \param SpoolFile Contiene una structura  tipo STRUCT_WATCHIO.BackupFile con información para hacer un backup de un fichero
         */
        public static void BackupFile(Object SpoolFile)
        {
            // Estructura con información del fichero de spool y el directorio donde se va a guardar un backup
            STRUCT_WATCHIO.BackupFile StructBackupFile;
            StructBackupFile = new STRUCT_WATCHIO.BackupFile();
            StructBackupFile = (STRUCT_WATCHIO.BackupFile)SpoolFile;

            //Fecha para guardar un backup existente de fichero de spool
            string Fecha = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "_" +
                           DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();


            // --- ficheros de Spool ---
            // Fichero de spool del que se quiere hacer copia
            string FicheroSpool = StructBackupFile.PathDirectory + @"\" + StructBackupFile.FileName;
            // Backup del fichero de spool
            string FicheroSpoolBackup = StructBackupFile.PathDirectory + @"\" + StructBackupFile.PathDirectoryBackup + @"\" + StructBackupFile.FileNameBackup;

            // Backup del fichero de Spool
            try
            {
                // Comprobamos si existe el directorio de backup para Spool de impresión. Si no existe se crea
                DirectoryInfo VSpoolDirectoryBackup = new DirectoryInfo(StructBackupFile.PathDirectory + @"\" + StructBackupFile.PathDirectoryBackup);
                if (!VSpoolDirectoryBackup.Exists)
                {
                    VSpoolDirectoryBackup.Create();
                    Log.Debug("Se crea el Subdirectorio de backup de Spool de Impresión: " + StructBackupFile.PathDirectory + @"\" + StructBackupFile.PathDirectory);
                }

                // Guardamos copia del fichero.
                File.Copy(FicheroSpool, FicheroSpoolBackup, true);
                Log.Info("Se hace copia del fichero de Spool: " + (char)13 + " - " + FicheroSpool + " en:" + (char)13 + "- " + FicheroSpoolBackup);
            }
            catch (Exception e)
            {
                Log.Error("Se ha producido el error: " + e.Message + (char)13 + " Al copiar el fichero de spool: " + FicheroSpool + " en " + FicheroSpoolBackup);
            }
        }


    }

    /* \brief clase para controlar un tiempo de espera
     */
    public class ElapsedTime
    {
        public const int _TIEMPO_ESPERA = 120; // 30 segundos
        private int _SEGUNDOS;
        private DateTime FechaControl;
        public ElapsedTime(int Segundos)
        {
            this._SEGUNDOS = Segundos;
            this.FechaControl = DateTime.Now;
        }

        public ElapsedTime()
        {
            this._SEGUNDOS = _TIEMPO_ESPERA;
            this.FechaControl = DateTime.Now;
        }

        public int Elapsed()
        {
            TimeSpan tspan;
            tspan = DateTime.Now.Subtract(this.FechaControl).Duration();
            return (int)tspan.TotalSeconds + (int)tspan.TotalMinutes * 60;
        }

        /** \brief Comprueba si ha sobrepasado el tiempo límite de espera
         *  \return verdadero si se ha sobrepasado el tiempo de espera
         */
        public bool OverElapsedTime()
        {
            if (Elapsed() > _SEGUNDOS) return true;
            else return false;
        }
    }

}
