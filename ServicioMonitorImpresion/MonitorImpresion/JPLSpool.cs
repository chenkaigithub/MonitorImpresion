using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;

namespace MonitorTrabajosImpresion
{
    public class JPLSpool:Spool
    {

        // Posiciones que delimitan el bloque PCLXL
        private int BeginBlockPCLXL = 0;
        private int EndBlockPCLXL = 0;
        // Tiene contenido PCLXL
        public bool ContenidoPCLXL { get; set; }

        // Indicador de analisis PCLXL
        public bool AnalisisPCLXLCompletado { get; set; }


        // Tag para delimitar bloques JPL
        private static string _COMANDO_UEL = (Convert.ToChar(Util._ESC) + "%-12345X");
        // Tag para indicar nueva instrucción JPL
        private static string _JPL_COMANDO = "@PJL";


        // Se crea el Logger con nombre: JPLSpool
        private static readonly ILog Log = LogManager.GetLogger("JPLSpool");


        /** \brief En el constructor se guarda el Path del fichero de spool y el tamaño.
         * \details Se comprueba si existe el fichero de spool de impresión y se guarda el Path.
         * \param PathFile Path del fichero de spool a analizar
         */
        public JPLSpool(string PathFile) : base (PathFile)
        //public JPLSpool()
        {
            Log.Debug("---- Constructor JPLSpool. Fichero de spool a analizar: " + PathFile);
            this.AnalisisPCLXLCompletado = false;
            this.ContenidoPCLXL = false;
        } // Fin JPLSpool()


        /** \details
         * Buscar cadenas del bloque JPL en el fichero de spool de impresion. Se localizan cadenas que comienzan por:
         * - "%-12345X@PJL": Cabecera de los comandos JPL
         * - "@PJL": Instrucciones JPL.
         * 
         * También guarda la posición de inicio y fin del Bloque PCLXL en las variable:
         *  - BeginBlockPCLXL
         *  - EndBlockPCLXL
         *  \param PrintJob Estructura del tipo STRUCT_PRINT_JOB pasada por referencia en la que se guardan los detalles del trabajo de impresión obtenidos.
         */
        public new void Analize(ref STRUCT_PRINT_JOB PrintJob) //string StringJPL, string PathFileJPL)
        {
            int PosicionJPL = 0; // Posición en el archivo de Spool
            // Para indicar si ya se ha encontrado la cabecera JPL
            bool HeaderJPLFound = false;
            // Para indicar si se ha encontrado el fin del contenido JPL/
            bool FooterJPLFound = false;

            // Indica el tamaño del bloque que utilizamos para leer el fichero de Spool
            const int SIZE_BLOCKB_JPL = 1024 * 1024 * 64;

            // Se van cargando las instrucciones JPL contenidas en el fichero de spool para su analisis.
            string CadenaJPL;
            int i;
            // Objeto para fichero binario con el que hacemos el análisis del fichero de spool
            BinaryReader JPLFile;

            /// Se hacen busquedas de los siguientes Tags JPL en el fichero de Spool
            /// - Delimitador de bloque JPL
            byte[] ArrayJPLDelimitadorBloque = Util.ArrayCharToByte(_COMANDO_UEL.ToCharArray());
            /// - Cabecera instrucción JPL
            byte[] ArrayJPLTag = Util.ArrayCharToByte(_JPL_COMANDO.ToCharArray());

            // Indice de posición para Tag de cabecera JPL
            int PosicionJPLCabecera = 0;
            // Indice de posición para Tag de instrucción JPL
            int PosicionJPLTag = 0;

            // Se guardan bloques del fichero con contenido JPL para su análisis
            byte[] ArrayJPL; //  = new byte[SIZE_BLOCKB_JPL];

            // Objeto para abrir contenido PCLXL del fichero JPL/PCLXL
            PCLXLSpool MiPCLXLSpool;

            Log.Info("/*------------------------------------------------------------------------------------------*/");
            Log.Info(" ");
            Log.Info(" ");
            Log.Info("                                INICIO ANALISIS JPL ");
            Log.Info(" ");
            Log.Info(" ");
            Log.Info("/*------------------------------------------------------------------------------------------*/");

            


            // Abrimos el fichero en la posición  0 y empezamos a recorrerlo
            try
            {
                JPLFile = new BinaryReader(File.Open(this.PathSpoolFile, FileMode.Open));
                Log.Info("AnalisisCompletado JPL del fichero: " + this.PathSpoolFile);
            }
            catch (Exception e) 
            {
                Log.Error("No se tiene acceso al fichero de Spool: " + this.PathSpoolFile);
                throw e;
            }

            // Inicializamos reloj para controlar el tiempo que tarda del análisis
            var Watch = new Stopwatch();
            Watch.Start();
            try
            {
                // Se analiza todo el contenido del fichero. Valor inicial de PosicionJPL=0
                while (PosicionJPL < this.SizeFile)
                {
                    //Nos posicionamos en el archivo JPL para leer un nuevo bloque de datos
                    JPLFile.BaseStream.Seek(PosicionJPL, SeekOrigin.Begin);

                    // Leemos nuevo bloque de datos
                    if ((this.SizeFile - PosicionJPL) >= SIZE_BLOCKB_JPL)
                    {
                        // Todavía no hemos comprobado el fichero completo
                        ArrayJPL = new byte[SIZE_BLOCKB_JPL];
                        ArrayJPL = JPLFile.ReadBytes(SIZE_BLOCKB_JPL);
                    }
                    else
                    {
                        // Quedan por leer menos bytes que el tamaño de bloque de lectura con el que trabajamos
                        ArrayJPL = new byte[this.SizeFile - PosicionJPL];
                        ArrayJPL = JPLFile.ReadBytes(this.SizeFile - PosicionJPL);
                    }

                    /// Analizamos el bloque de datos en busca de Cabecera e instrucciones JPL
                    for (i = 0; i < ArrayJPL.Length; i++, PosicionJPL++)
                    {
                        // Para la búsqueda de la cabecera JPL
                        if ((ArrayJPL[i] == ArrayJPLDelimitadorBloque[PosicionJPLCabecera]))
                        {
                            if (PosicionJPLCabecera == (ArrayJPLDelimitadorBloque.Length - 1))
                            {
                                // Hemos encontrado la cabecera JPL
                                if (HeaderJPLFound)
                                {
                                    // Segunda ocurrencia Cabecera JPL. Se cambia de PCLXL a JPL. Siguientes ocurrencias no se tienen en cuenta.
                                    if (!(FooterJPLFound))
                                    {
                                    this.EndBlockPCLXL = PosicionJPL - ArrayJPLDelimitadorBloque.Length;
                                    }

                                    FooterJPLFound = true;
                                }
                                else
                                {
                                    HeaderJPLFound = true;
                                }
                                // Volvemos a buscar otra nueva cabecera. Posicionamos la búsqueda al comienzo del Tag de cabecera a buscar
                                PosicionJPLCabecera = 0;
                                Log.Info("Posición " + Util.MostrarHex((UInt32)(PosicionJPL - ArrayJPLDelimitadorBloque.Length + 1)) + ": " + _COMANDO_UEL);
                            }
                            else
                            {
                                PosicionJPLCabecera++;
                            } // End if
                        }
                        else
                        {
                            // Inicializamos la busqueda de la cabecera
                            PosicionJPLCabecera = 0;
                        } // End if búsqueda Tags Cabecera

                        // Para la búsqueda Tags JPL
                        if ((ArrayJPL[i] == ArrayJPLTag[PosicionJPLTag]) & (PosicionJPLCabecera == 0))
                        {
                            if (PosicionJPLTag == (ArrayJPLTag.Length - 1))
                            {
                                //Hemos encontrado el Tag JPL. Volvemos a inicializar la variable de control de búsqueda de Tag JPL.
                                PosicionJPLTag = 0;
                                // Obtenemos la instrucción JPL completa.
                                CadenaJPL = ReadStringJPL(PosicionJPL - ArrayJPLTag.Length + 1, ref JPLFile);

                                // Se analiza la instrucción JPL.
                                AnalizeStringJPL(CadenaJPL, ref PrintJob);


                                Log.Info("Posición " + Util.MostrarHex((UInt32)(PosicionJPL - ArrayJPLTag.Length + 1)) + ": " + CadenaJPL);

                                // Instrucción JPL de cambio a modo PCLXL. Se guarda la posición de la cabecera PCLXL.
                                if (JPLToPCLXL(CadenaJPL))
                                {
                                    this.BeginBlockPCLXL = PosicionJPL - ArrayJPLTag.Length + CadenaJPL.Length + 1;
                                    CadenaJPL = ReadStringJPL(this.BeginBlockPCLXL, ref JPLFile);
                                    Log.Info("Posición bloque PCLXL " + Util.MostrarHex((UInt32)(this.BeginBlockPCLXL)) + ": " + CadenaJPL);

                                    // El fichero de Spool utiliza PCLXL para guardar el trabajo de impresión.
                                    this.ContenidoPCLXL = true;
                                }
                            }
                            else
                            {
                                PosicionJPLTag++;
                            } // End if
                        }
                        else
                        {
                            // Inicializamos la busqueda de la cabecera
                            PosicionJPLTag = 0;
                        } // End if 
                    } // End for            
                } //while
            } //try
            catch (Exception e)
            {
                JPLFile.Close();
                Log.Info("El fichero de Spool no se ha podido analizar mediante lenguaje JPL");
                throw e;
            }

            // Detenemos el reloj para controlar el tiempo que ha tardado del análisis JPL.
            Watch.Stop();
            Log.Info("Tiempo transcurrido en Milisegundos del analisis JPL: " + Watch.ElapsedMilliseconds + " ms" + (char) Util._END_LINE1 + (char) Util._END_LINE1);

            Log.Debug("Cerramos el fichero analizado mediante lenguaje JPL");
            JPLFile.Close();
            
            Log.Info("/*------------------------------------------------------------------------------------------*/");
            Log.Info(" ");
            Log.Info(" ");
            Log.Info("                                FIN ANALISIS JPL");
            Log.Info(" ");
            Log.Info(" ");
            Log.Info("/*------------------------------------------------------------------------------------------*/");

            this.AnalisisCompletado = true;


            // Analizamos si tiene contenido PCLXL
            if (this.ContenidoPCLXL)
            {
                try
                {

                    MiPCLXLSpool = new PCLXLSpool(this.PathSpoolFile, this.BeginBlockPCLXL, this.EndBlockPCLXL);
                    MiPCLXLSpool.Analize(ref PrintJob);

                    // Para control en la realización de pruebas
                    this.AnalisisPCLXLCompletado = true;

                }
                catch (Exception e)
                {
                    Log.Error("Error análisis PCLXL");
                    Log.Error(e);
                } 
            }

            // Para control en la realización de pruebas
            this.AnalisisCompletado = true;

        } // Fin AnalizeJPL_PCLXL()


        /** \brief Se analiza una instrucción JPL 
         * \details
         * Devuelve la instrucción JPL localizada en la posición del fichero indicada en el parámetro entrada.
         * Se utiliza la copia auxiliar del fichero de spool
         * \param PosicionJPL Indica posisón donde comienza el bloque JPL 
         * \param JPLFile con contenio JPL/PCLXL
         */
        private string ReadStringJPL(int PosicionJPL, ref BinaryReader JPLFile)
        {
            byte JPLByte;

            // Leer hasta que encuentre el byte fin de linea _END_LINE1 o _END_LINE2, 
            string cadenaJPL ="";
            try
            {
                // Nos posicionamos en el fichero para lectura y leemos la cadena JPL
                JPLFile.BaseStream.Seek(PosicionJPL, SeekOrigin.Begin);
                JPLByte = JPLFile.ReadByte();
                while ((JPLByte != Util._END_LINE1) && (JPLByte != Util._END_LINE2))
                {
                    cadenaJPL = cadenaJPL + (char)JPLByte;
                    JPLByte = JPLFile.ReadByte();
                }
                return cadenaJPL;
            }
            catch (Exception e)
            {
                Log.Error("ReadStringJPL: ", e);
                throw e;
            }
        } // Fin ReadStringJPL()

        /** \brief Analiza cadena JPL.
         * \details y si es necesario se actualiza la del Trabajo de Impresión.
         * \param StringJPL cadena JPL a analizar.
         * \param PrintJob struct por referencia con las propiedades del trabajo de impresión
         * \remarks (Fuente: recorrer un diccionario C#)
         * @see http://karlozarba.blogspot.com.es/2011/06/recorrer-un-dictionary-con-c.html
         */
        private void AnalizeStringJPL(string StringJPL, ref STRUCT_PRINT_JOB PrintJob)
        {
            const string ESCAPE_CHARS = @";="" ";

            /// Comprobamos en el diccionario JPL si la cadena JPL a analizar contiene alguna propiedad de Trabajo de impresión
            foreach (KeyValuePair<string , ENUM_JPL.PROPERTY> Par in DICTIONARY_JPL.PROPERTY)
            {
                // La cadena JPL contiene un parámetro de análisis JPL
                if (StringJPL.Contains(Par.Key))
                {
                    // Buscamos la posición siguiente al valor llave en el diccionario de datos
                    int VALUE_POSITION = StringJPL.IndexOf(Par.Key) + Par.Key.Length;
                    // Con la siguiente instruccion calculamos realmente la posición donde se encuentra el valor
                    VALUE_POSITION = this.PositionValueJPL(StringJPL, VALUE_POSITION);
                    
                    string Value = "";
                    bool Delimiter = false;

                    // Recordamos si es un atributo delimitado por comillas
                    if (StringJPL[VALUE_POSITION] == '"')
                    {
                        Delimiter = true;
                        VALUE_POSITION++;
                    }

                    /// - Obtenemos el valor de la propiedad del Trabajo de Impresión
                    for (int i = VALUE_POSITION; i < StringJPL.Length ; i++)
                    {
                        if (!Delimiter)
                        {
                            if (ESCAPE_CHARS.Contains(StringJPL[i].ToString()))
                            {
                                // Forzamos condición de salida del bucle
                                i = StringJPL.Length;
                            }
                            else
                            {
                                Value += StringJPL[i];
                            }
                        }
                        else
                        {
                            if (StringJPL[i] == '"') //Caracter delimitador
                            {
                                // Forzamos condición de salida del bucle
                                i = StringJPL.Length;
                            }
                            else
                            {
                                Value += StringJPL[i];
                            }
                        } //if
                    } //for

                    Log.Debug("Diccionario: '" + Par.Key + "' Valor de parámetro encontrado: " + Value);

                    /// - Guardamos el valor de la propiedad del Trabajo de Impresión
                    //
                    Log.Debug("(Par.key, Par.Value): (" + Par.Key + ", " + Par.Value.ToString() + ")");

                    switch (Par.Value)
                    {
                        case ENUM_JPL.PROPERTY.ID_FUENTE: PrintJob.ID_FUENTE =  "JPL/" + Value;
                            Log.Info("Se estable como fuente de analisis: " + Value);
                            break;
                        case ENUM_JPL.PROPERTY.F_PRINTJOB:
                            try
                            {
                                PrintJob.F_PRINTJOB = Value.Substring(6, 2) + "/" + Value.Substring(4, 2) + "/" + Value.Substring(0, 4) + " " + Value.Substring(8, 2) + ":" +
                                                       Value.Substring(10, 2) + ":" + Value.Substring(12, 2);
                                Log.Debug("AnalizeStringJPL. ENUM_JPL.PROPERTY.F_PRINTJOB: " + PrintJob.F_PRINTJOB);
                            }
                            catch (Exception e)
                            {
                                Log.Error(e + "La cadena '" + Value + "' no tiene formato de fecha");
                            }
                            break;
                        case ENUM_JPL.PROPERTY.ID_LOGIN: PrintJob.ID_LOGIN =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_PRINTSERVER: PrintJob.ID_PRINTSERVER =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_PRINTER: PrintJob.ID_PRINTER =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_DOCUMENT: PrintJob.ID_DOCUMENT =  Value; break;
                        case ENUM_JPL.PROPERTY.N_LENGTH: PrintJob.N_LENGTH =  Convert.ToInt32(Value);break;
                        case ENUM_JPL.PROPERTY.N_WIDTH: PrintJob.N_WIDTH = Convert.ToInt32(Value); break;
                        case ENUM_JPL.PROPERTY.ID_MEDIASIZE: PrintJob.ID_MEDIASIZE =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_ORIENTATION: PrintJob.ID_ORIENTATION =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_COLOR: PrintJob.ID_COLOR =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_DUPLEX: PrintJob.ID_DUPLEX =  Value; break;
                        case ENUM_JPL.PROPERTY.ID_STATUS: PrintJob.ID_STATUS =  Value; break;
                        default:
                            Log.Error("AnalizeStringJPL: Atributo JPL desconocido: " + Par.Value.ToString());
                            break;
                    } //switch
                    Log.Debug("La cadena JPL: '" + StringJPL + "' contiene el parámetro: '" + Par.Key + "'"); 
                } //if. Contiene cadena del diccionario

            } //foreach
            //return StructJPL;
        } //AnalizeStringJPL()

        /** \brief Posición valor atributo JPL en cadena JPL.
         *  \details Dada la posición inicial donde buscar el Valor se ignoran los espacios en blanco y identificadores de comienzo de valor
         *  como ":" y "="
         * \param StringJPL cadena JPL a analizar.
         * \param InitialPosition Posición inicial en la que comenzar a buscar el valor del atributo dentro de la cadena JPL
         * \returns Posición real donde comienza el Valor del atributo dentro de la cadena JPL
         */
        private int PositionValueJPL(string StringJPL, int InitialPosition)
        {
            const string Delimitadores = " :="; 

            //Log.Info("***********************************************************");
            //Log.Info(StringJPL + ">>> Posición inicial para buscar valor del atributo: " + InitialPosition.ToString()); 
            //Log.Info("***********************************************************");

            char[] ArrayStringJPL = StringJPL.ToCharArray();
            int PositionValue = InitialPosition;
            while (Delimitadores.Contains(ArrayStringJPL[PositionValue].ToString()))
            {
                PositionValue++;
            }
            return PositionValue;
        }

        /** \brief Analiza cadena JPL para determinar si se cambia a lenguaje PCLXL
         * \param StringJPL cadena JPL a analizar.
         * \returns True si se cambia a lenguaje PCLXL
         * \remarks Si la cadena JPL de entrada quitandole espacios en blanco y final de línea es igual que "@PJLENTERLANGUAGE=PCLXL",
         * significa que se conmuta a lenguaje PCLXL
         */
        private bool JPLToPCLXL(string StringJPL)
        {
            // A la cadena StrinJPL se le quita el final de linea (Substring) que precede a la subcadena "PCLXL" y también los espacios en blanco(Replace)
            if (StringJPL.Substring(0, StringJPL.IndexOf("PCLXL") + "PCLXL".Length).Replace(" ", "") == "@PJLENTERLANGUAGE=PCLXL")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal PCLXLSpool PCLXLSpool
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }

}
