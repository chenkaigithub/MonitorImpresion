using System;
using System.IO;
using log4net;
using System.Collections;


namespace MonitorTrabajosImpresion
{
    /** \brief clase que implementa el análisis de ficheros de Spool con contenido PCLXL
     */ 
    class PCLXLSpool: Spool
    {
        /// Se crea el Logger con nombre: PCLXLSpool
        private static readonly ILog Log = LogManager.GetLogger("PCLXLSpool");
        /// Posición inicial con contenido PCLXL en el fichero de Spool.
        public int BeginBlockPCLXL { get; }
        /// Posición final con contenido PCLXL en el fichero de Spool.
        public int EndBlockPCLXL { get; }

        /// <summary>
        ///  Array en el que se guarda bloques de datos binarios del fichero de Spool para su análisis.
        /// </summary>
        private byte[] ArrayPCLXLSpool;

        /// <summary>
        /// Estructura para guardar la posición de los Tag PCLXL que se van analizando.
        /// </summary>
        public struct PositionTagPCLXL
            {
                public int AnalizeTag;
                public int Value;
                public int Attribute;
            }


        /** \brief Carga fichero de Trabajo de Impresión en un array.
         * \param PathFile Path del fichero de Spool
         * \param IBeginBlockPCLXL Posición inicial del bloque PCLXL en el fichero de Spool
         * \param IEndBlockPCLXL Posición final del bloque PCLXL en el fichero de Spool 
         */
        public PCLXLSpool(string PathFile, int IBeginBlockPCLXL, int IEndBlockPCLXL) : base(PathFile)
        {
            /// Inicializa propiedades de objeto de Spool
            Log.Info("OBJETO DE LA CLASE PCLXLSpool. Fichero en siguiente ruta: " + PathFile);
            this.BeginBlockPCLXL = IBeginBlockPCLXL;
            this.EndBlockPCLXL = IEndBlockPCLXL;

            /// Abrimos el fichero de Spool
            FileStream PCLXLFile;
            PCLXLFile = File.OpenRead(this.PathSpoolFile);
            PCLXLFile.Seek(0, SeekOrigin.Begin);

            /// - Carga contenido fichero de Spool en una array
            ArrayPCLXLSpool = new byte[this.SizeFile];
            PCLXLFile.Read(ArrayPCLXLSpool, 0, this.SizeFile);

            /// - Generamos Log
            Log.Info("Tamaño de fichero de Spool PCLXL: " + this.SizeFile);
            PCLXLFile.Close();
        } // Fin PCLXLSpool()



        /** \brief Analisis contenido PCLXL del fichero de Spool de impresión.
         * \param PrintJob struct con las propiedades del trabajo de impresión
         */
        public new void Analize(ref STRUCT_PRINT_JOB PrintJob)
        {

            string HeaderPCLXL="";
            //byte PCLXLByte = 0;
            byte PCLXLByte; // Variable principal con la que se va analizando el contenido PCLXL.
            byte PCLXL_Byte_Attribute = 0; // Se guarda el valor del atributo.
            string PCLXL_Value=""; // Atributo con valor simple.
            byte [] PCLXLValueArray = null; // Array donde se guarda el valor del Atributo(byte a byte).
            int PosicionPCLXL = this.BeginBlockPCLXL;
            int PosicionCabeceraPCLXL = 0;
            int NBytes = 0; // Variable utilizada para indicar un número de bytes
            byte[] ArrayPCLXSizeArray; // Utilizada para leer bloques de bytes
            int ArrayPCLXLValue = 0; // Utilizar para indicar el tamaño en bytes

            // Estructura para guardar las propiedades de color de la página PCLXL que se está analizando.
            // Inicialización por defecto sin elementos en color
            STRUCT_PCLXL.PCLXLPageState PCLXLPageState = new STRUCT_PCLXL.PCLXLPageState("eGray", false);

            // Registro para guardar posiciones del Tag PCLXL a analizar
            PositionTagPCLXL PosTagPCLXL;       
            PosTagPCLXL.AnalizeTag = 0;
            PosTagPCLXL.Value = 0;
            PosTagPCLXL.Attribute = 0;
            
            // Colas para guardar información de los atributos asociados a un operador(primero en entrar primero en salir).
            // Utilizado para análisis PCLXL para cada grupo (operador + 1 a N (atributo+valor))
            Queue ColaAtributos = new Queue();
   
            // Variable auxiliar para almacenar el Valor del atributo
            string sValue = "";
            int iValue = 0;

            // Variable para obtener resumen Analisis
            Tags ResumenPCLXL = new Tags();

            Log.Info("/*------------------------------------------------------------------------------------------*/");
            Log.Info("                                INICIO ANALISIS PCLXL ");
            Log.Info("/*------------------------------------------------------------------------------------------*/" + (char) Util._END_LINE1);
            Log.Info("Fichero de Spool a analizar contenido PCLXL: " + this.PathSpoolFile);
            Log.Info("Desde la posición: " + Util.MostrarHex((UInt32)(this.BeginBlockPCLXL)) + ", hasta la posición: " + Util.MostrarHex((UInt32)(this.EndBlockPCLXL)));

            /// Lectura de la cabecera del bloque PCLXL<br>
            // Ignoramos los primeros bytes hasta obtener el identificador de formato de representación(Binding Format): {"'" | ")" | "("}. Comienzo cabecera PCLXL ") HP-PCL XL ......."
            do
            {
                PCLXLByte = this.ArrayPCLXLSpool[PosicionPCLXL];
                PosicionPCLXL++;
            } while ((PCLXLByte != Util._BINDING_FORMAT_COMILLA) && (PCLXLByte != Util._BINDING_FORMAT_PARENTESIS_DERECHO) && (PCLXLByte != Util._BINDING_FORMAT_PARENTESIS_IZQUIERDO));
            //  Encontrado el identificador nos volvemos a posicionar y guardamos la posición de la cabecera
            PosicionPCLXL--;
            PosicionCabeceraPCLXL = PosicionPCLXL;


            // Leemos la cabecera PCLXL
            do
            {
                PCLXLByte = this.ArrayPCLXLSpool[PosicionPCLXL];
                PosicionPCLXL++;
                HeaderPCLXL = HeaderPCLXL + (char) PCLXLByte;
            } while ((PCLXLByte != Util._END_LINE1) && (PCLXLByte != Util._END_LINE2));

            Log.Info("Posición: (" + Util.MostrarHex((UInt32) PosicionCabeceraPCLXL) + "): " + "CABECERA: '" + HeaderPCLXL + "'");

            // Leida la cabecera guardamos la posición en la que empezamos a analizar
            Log.Info("Posición: (" + Util.MostrarHex((UInt32)(PosicionPCLXL)) + "): " + " - Comienza analisis Bloque datos PCLXL.");

            try
            {
                /// Bucle para analizar los TAGS PCLXL del Spool<br>         
                while (PosicionPCLXL <= this.EndBlockPCLXL)
                {
                    /// Nos posicionamos en el siguiente TAG PCLXL a Analizar
                    PosTagPCLXL.AnalizeTag = PosicionPCLXL;
                    PosTagPCLXL.Value = 0;
                    PosTagPCLXL.Attribute = 0;

                    /// Leemos TAG PCLXL y nos movemos a la siguiente posición
                    PCLXLByte = this.ArrayPCLXLSpool[PosicionPCLXL];
                    PosicionPCLXL++;

                    /// Analizamos el TAG PCLXL
                    switch (DICTIONARY_PCLXL.TYPE_TO_TAG[DICTIONARY_PCLXL.TAG_TO_TYPE[PCLXLByte]])
                    {
                        /// - Análisis para TAG Tipo de Dato
                        case ENUM_PCLXL.TAGTYPE.DataType:

                            /// Es un Tipo de Dato Simple
                            if (DICTIONARY_PCLXL.DATA_TYPE_SIMPLE_LEN.ContainsKey(((ENUM_PCLXL.DATATYPE)PCLXLByte).ToString("F")))
                            {
                                // Guardamos la posición del Valor del atributo
                                PosTagPCLXL.Value = PosicionPCLXL;

                                // Número de bytes que ocupa el dato simple. 
                                NBytes = DICTIONARY_PCLXL.DATA_TYPE_SIMPLE_LEN[Enum.GetName(typeof(ENUM_PCLXL.DATATYPE), PCLXLByte)];

                                // Obtnemos el Valor
                                //byte[] SimplePCLXLSizeSimple = new byte[NBytes];
                                PCLXLValueArray = new byte[NBytes];

                                //Array.Copy(this.ArrayPCLXLSpool, PosicionPCLXL, SimplePCLXLSizeSimple, 0, NBytes);
                                Array.Copy(this.ArrayPCLXLSpool, PosicionPCLXL, PCLXLValueArray, 0, NBytes);

                                //PCLXL_Value = Util.MostrarHex(SimplePCLXLSizeSimple);
                                PCLXL_Value = Util.MostrarHex(PCLXLValueArray);

                                // Obtenemos y guardamos la posicion del Atributo
                                PosicionPCLXL = PosicionPCLXL + NBytes;
                                PosTagPCLXL.Attribute = PosicionPCLXL;
                            } // End if Datos Simples

                            /// Es un Tipo de Dato Array 
                            if (DICTIONARY_PCLXL.DATA_TYPE_ARRAY_LEN.ContainsKey(((ENUM_PCLXL.DATATYPE)PCLXLByte).ToString("F")))
                            {
                                /** \remarks
                                 * El tipo de dato para array está compuesto por 3 elementos,  de la siguiente forma:<br>
                                 * <code>(A B C)<br>
                                 * &nbsp;| | '-> (tamaño en bytes obtenido de B). El valor de este elemento indica el tamaño del array<br>
                                 * &nbsp;|  '-> (1 byte). Con su valor se obtiene el tipo (ENUM_PCLXL.DATATYPE) de dato de C (número de bytes del elemento C)<br>
                                 * &nbsp;'-> (1 byte).  Indica que es un array y el tipo de dato de cada elemento del array(PCLXLByte. En ENUM_PCLXL.DATATYPE)<br>
                                 * </code>  
                                 * 
                                 * Ejem-1: 0xc8c002 (Se trata de un array de 2 elementos de tipo ubyte.)
                                 * A: c8(1 byte): indica que es un array de tipos de datos ubyte (ENUM_PCLXL.DATATYPE).
                                 * B: c0(1 byte): Indica el tipo de datos, tamaño, necesario para almacenar la longitud del array ubyte (1 byte) del siguiente campo(02).
                                 * C: 02(1 byte): Valor 2.
                                 * Ejem-2: 0xc8c11000 (Se trata de un array de 16 elementos de tipo ubyte.)
                                 * A: c8: indica que es un array de tipos de datos ubyte (ENUM_PCLXL.DATATYPE).
                                 * B: c1: Tipo de dato numérico uint16 (2 bytes).
                                 * C: 1000: Valor 16 en decimal (Hexadecimal Little Endian: Bytes menos significativos primero).
                                 */

                                // Contenido elemento B (this.ArrayPCLXLSpool[PosicionPCLXL]).   nos da el tamaño en byte del siguiente elemento: C
                                NBytes = DICTIONARY_PCLXL.DATA_TYPE_SIMPLE_LEN[Enum.GetName(typeof(ENUM_PCLXL.DATATYPE), (byte)this.ArrayPCLXLSpool[PosicionPCLXL])];

                                // Nos movemos a la posicion del elemento C
                                PosicionPCLXL++;

                                // Leemos el siguiente campo en un array. Contiene el número de elementos del array
                                // ---------------------------------------------------------------------------------
                                ArrayPCLXLValue = 0; // Número de elementos del array que contiene el valor
                                ArrayPCLXSizeArray = new byte[NBytes]; // Tamaño: número de bytes del tipo de dato
                                Array.Copy(this.ArrayPCLXLSpool, PosicionPCLXL, ArrayPCLXSizeArray, 0, NBytes);

                                // Leemos el tamaño o número de elementos del array: ArrayPCLXLValue
                                switch (NBytes)
                                {
                                    case 1: // 1 byte
                                        ArrayPCLXLValue = (int)ArrayPCLXSizeArray[0];
                                        break;
                                    case 2: // 2 bytes
                                        ArrayPCLXLValue = (int)BitConverter.ToInt16(ArrayPCLXSizeArray, 0);
                                        break;
                                    case 4: // 4 bytes
                                        ArrayPCLXLValue = (int)BitConverter.ToInt32(ArrayPCLXSizeArray, 0);
                                        break;
                                }

                                /// - Obtenemos el Valor
                                // Guardamos la posición del valor del atributo
                                PosicionPCLXL = PosicionPCLXL + NBytes;
                                PosTagPCLXL.Value = PosicionPCLXL;

                                // Calculamos el número total de bytes que ocupa el array
                                NBytes = DICTIONARY_PCLXL.DATA_TYPE_ARRAY_LEN[Enum.GetName(typeof(ENUM_PCLXL.DATATYPE), PCLXLByte)] * ArrayPCLXLValue;

                                /// - Guardamos el valor del atributo en array de datos
                                PCLXLValueArray = new byte[NBytes];
                                Array.Copy(this.ArrayPCLXLSpool, PosTagPCLXL.Value, PCLXLValueArray, 0, NBytes);

                                // Para mostrar en el Log el tamaño del array
                                PCLXL_Value = NBytes.ToString() + " bytes";

                                // Posición del Attributo
                                PosicionPCLXL += NBytes;
                                PosTagPCLXL.Attribute = PosicionPCLXL;
                            }  // if para Arrays

                            /// - Obtenemos el atributo
                            // leemos el AttributeDefiner para ver la longitud del atributo
                            switch (this.ArrayPCLXLSpool[PosicionPCLXL])
                            {
                                case (byte)ENUM_PCLXL.ATTRIBUTE_DEFINER.ubyte:
                                    PosicionPCLXL++;
                                    PCLXL_Byte_Attribute = this.ArrayPCLXLSpool[PosicionPCLXL];
                                    PosicionPCLXL++;
                                    break;
                                // El siguiente caso para atributos de 2 bytes no se utiliza
                                // case (byte)PCLXL_ENUM.PCLXL_ATTRIBUTE_DEFINER.uint16:
                                //    PCLXL_int16_Attribute = PCLXLFile.ReadUInt16();
                                //    PosicionPCLXL = PosicionPCLXL + 3;
                                //    break;
                                default:
                                    Log.Error("Posición: (" + Util.MostrarHex((UInt32)(PosicionPCLXL)) + ")>>> " + "No se ha encontrado el TagAttribute Definer");
                                    PosicionPCLXL++;
                                    break;
                            }
                            /// Disponemos de:
                            ///      - Atributo : PCLXL_Byte_Attribute
                            ///      - Valor : PCLXL_Value
                            ///      - Tipo de Dato: PCLXLByte
							/// Guardamos esta información en una Cola para almacenar posteriormenete en el Log del Monitor de Impresión
                            /// 	- Guardamos el tipo de dato
                            ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.AnalizeTag)) + "     " + "PCLXL  Data Type            " + Util.MostrarHex(PCLXLByte) + "            " +
                                        DICTIONARY_PCLXL.DATATYPE[PCLXLByte]);

                            ///     - Guardamos el Valor.
                            //Para atributos relacionados con el color de impresión, mostramos su valor con el mayor detalle
                            switch ((ENUM_PCLXL.ATTRIBUTE)PCLXL_Byte_Attribute)
                            {
                                case ENUM_PCLXL.ATTRIBUTE.ColorSpace:
                                    sValue = DICTIONARY_PCLXL.COLORSPACE[PCLXLValueArray[0]];
                                    ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Value)) + "     " + "           Value            " + sValue);
                                    // Actualizamos la propiedad SpaceColor en la página actual
                                    PCLXLPageState.SetColorSpace(sValue);
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.RGBColor:
                                    sValue = "(" + PCLXLValueArray[0].ToString() + ", " + PCLXLValueArray[1].ToString() + ", " + PCLXLValueArray[2].ToString() + ")";
                                    ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Value)) + "     " + "           Value            " + sValue);
                                    // Actualizamos la propiedad Color en la página Actual si todavía no contiene ninguna impresión en Color
                                    if (!PCLXLPageState.IsColorPage())
                                    {
                                        PCLXLPageState.SetColorPage(sValue);
                                    }
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.ColorMapping:
                                    ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Value)) + "     " + "           Value            " + DICTIONARY_PCLXL.COLORMAPPING[PCLXLValueArray[0]]);
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.ColorDepth:
                                    ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Value)) + "     " + "           Value            " + DICTIONARY_PCLXL.COLORDEPTH[PCLXLValueArray[0]]);
                                    break;
                                default:
                                    ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Value)) + "     " + "           Value            " + PCLXL_Value);
                                    break;
                            }

                            ///     - Guardamos el Atributo
                            ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Attribute)) + "     " + "  PCLXL  Attribute          " + Util.MostrarHex(PCLXL_Byte_Attribute) + "            " +
                                        DICTIONARY_PCLXL.ATTRIBUTE[PCLXL_Byte_Attribute]);
                            
                            // Atributo MediaSize
                            if (PCLXL_Byte_Attribute == (int) ENUM_PCLXL.ATTRIBUTE.MediaSize)
                            {
                                Log.Debug("============================================> MediaSize");
                            }

                            /// Si el atributo es alguno de estos {DUPLEX; ORIENTATION, MEDIASIZE, COLOR} lo actualizamos en el trabajo de impresión
                            switch ((ENUM_PCLXL.ATTRIBUTE)PCLXL_Byte_Attribute)
                            {
                                case ENUM_PCLXL.ATTRIBUTE.DuplexPageMode: // Valor es un byte
                                    PrintJob.ID_DUPLEX = DICTIONARY_PCLXL.DUPLEX_PAGEMODE[Util.ConvertStringHexToInt(PCLXL_Value)];
                                    PrintJob.N_DUPLEX = Util.ConvertStringHexToInt(PCLXL_Value);
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.Orientation: // Valor es un byte
                                    PrintJob.ID_ORIENTATION = DICTIONARY_PCLXL.ORIENTATION[Util.ConvertStringHexToInt(PCLXL_Value)];
                                    PrintJob.N_ORIENTATION = Util.ConvertStringHexToInt(PCLXL_Value);
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.PageCopies: // Valor es un byte. Propiedad para cada página 

                                    iValue = Util.ConvertStringHexToInt(PCLXL_Value);

                                    Log.Debug("*************************NUMERO DE COPIAS: " + iValue.ToString());

                                    PrintJob.N_COPIES = iValue;

                                    Log.Debug("*************************NUMERO DE PAGINAS ANTES DE ACTUALIZAR: " + PrintJob.N_PAGES.ToString());
                                    PrintJob.N_PAGES = PrintJob.N_PAGES + iValue;
                                    Log.Debug("*************************NUMERO DE PAGINAS DESPUES DE ACTUALIZAR: " + PrintJob.N_PAGES.ToString());
                                    if (PCLXLPageState.IsColorPage())
                                    {
                                        PrintJob.N_COLORPAGES = PrintJob.N_COLORPAGES + iValue;
                                    }
                                    Log.Debug("*************************NUMERO DE PAGINAS COLOR: " + PrintJob.N_COLORPAGES.ToString());
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.MediaSize: // Valor es una cadena(array)
                                    if (PrintJob.ID_MEDIASIZE == "")
                                    {
                                        PrintJob.ID_MEDIASIZE = Util.ConverArrayToString(PCLXLValueArray);
                                    }
                                    //PCLXLValueArray
                                    break;
                                case ENUM_PCLXL.ATTRIBUTE.ColorSpace: // Valor es un byte
                                    PrintJob.ID_COLOR = DICTIONARY_PCLXL.COLORSPACE[Util.ConvertStringHexToInt(PCLXL_Value)];
                                    PrintJob.N_COLOR = Util.ConvertStringHexToInt(PCLXL_Value);

                                    //PaletteDepth, RGBColor, ColorDepthArray, ColorDepth, ColorMapping, ColorTreatment
                                    break;
                                default:
                                    // Marcamos en el log para supervisar en el log los atributos relacionados con el color y con el tamaño de página 
                                    if ("PaletteDepthColorSpaceGrayLevelRGBColorColorDepthColorMappingColorTreatmentNewDestinationSizeMediaSizeCustomMediaSizeCustomMediaSizeUnitsDestinationSize".Contains(DICTIONARY_PCLXL.ATTRIBUTE[PCLXL_Byte_Attribute]))
                                    {
                                        Log.Debug("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Attribute)) + "     " + "  PCLXL  Attribute          " + Util.MostrarHex(PCLXL_Byte_Attribute) + "            " +
                                        DICTIONARY_PCLXL.ATTRIBUTE[PCLXL_Byte_Attribute] + "********************************************************");
                                    }
                                    break;
                            }
                            break;
                        case ENUM_PCLXL.TAGTYPE.AttributeDefiner:
                            break;
                        case ENUM_PCLXL.TAGTYPE.Attribute:
                            Log.Fatal("Nunca se debería obtener el atributo directamente. Cuando se detecta un Tipo de dato se obtiene su valor y a que atributo se vincula.");
                            break;
                        /// - Análisis para TAG EmbedDataDefiner
                        case ENUM_PCLXL.TAGTYPE.EmbedDataDefiner:
                            // Total de bytes del siguiente campo donde se guarda tamaño de los Embedded Data
                            NBytes = DICTIONARY_PCLXL.EMBED_DATA_DEFINER_LEN[Enum.GetName(typeof(ENUM_PCLXL.EMBED_DATA_DEFINER), (byte)PCLXLByte)];
                            ArrayPCLXLValue = 0;
                            ArrayPCLXSizeArray = new byte[NBytes];
                            // Nos posicionamos para leer los Embedded Data
                            Array.Copy(this.ArrayPCLXLSpool, PosicionPCLXL, ArrayPCLXSizeArray, 0, NBytes);
                            PosicionPCLXL += NBytes;
                            PosTagPCLXL.Value = PosicionPCLXL;
                            // ArrayPCLXLValue se guarda el valor del número de bytes de los embedded Data
                            switch (NBytes)
                            {
                                case 1: // 1 byte
                                    ArrayPCLXLValue = (int)ArrayPCLXSizeArray[0];
                                    break;
                                case 4: // 4 bytes
                                    ArrayPCLXLValue = (int)BitConverter.ToInt32(ArrayPCLXSizeArray, 0);
                                    break;
                            }

                            // Nos posicionamos a continuación de los Embedded Data
                            PosicionPCLXL += ArrayPCLXLValue;

                            // Disponemos de:
                            //      - Atributo : PCLXL_Byte_Attribute
                            //      - Valor : PCLXL_Value
                            //      - Tipo de Dato: PCLXLByte

                            // Guardamos el Analisis de los Tags Encontrados para generar el Log
                            ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.AnalizeTag)) + "     " + "PCLXL  Data Type            " + Util.MostrarHex(PCLXLByte) + "            " + Enum.GetName(typeof(ENUM_PCLXL.EMBED_DATA_DEFINER), PCLXLByte));
                            ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.AnalizeTag + 1)) + "     " + "  PCLXL  Embedded Len.      " + Util.MostrarHex((UInt32)(ArrayPCLXLValue)));
                            ColaAtributos.Enqueue("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.Value)) + "     " + "            Data            [" + ArrayPCLXLValue.ToString("D") + " bytes]");
                            break;
                        case ENUM_PCLXL.TAGTYPE.Whitespace:
                            // No se analiza
                            break;

                        /// Análisis para TAG tipo Operador
                        case ENUM_PCLXL.TAGTYPE.Operator:

                            /// Filtramos los operadores que nos interesan Si es final de página actualizamos propiedades del trabajo de impresión
                            /// Si el operador es alguno de estos {DUPLEX; ORIENTATION, MEDIASIZE, COLOR} actualizamos el trabajo de impresión
                            switch ((ENUM_PCLXL.OPERATOR)PCLXLByte)
                            {
                                case ENUM_PCLXL.OPERATOR.EndPage:
                                    ColaAtributos.Enqueue("Número de página: " + PrintJob.N_PAGES.ToString());
                                    goto default;
                                case ENUM_PCLXL.OPERATOR.BeginPage:
                                    // Inicio página todavía no se ha imprimido nada en Color
                                    PCLXLPageState.ResetColorPage();
                                    goto default;
                                case ENUM_PCLXL.OPERATOR.BeginSession:
                                    goto default;
                                case ENUM_PCLXL.OPERATOR.BeginImage:
                                    // Si ColorSpace actual es RGB la imagen se imprime en Color
                                    if (PCLXLPageState.IsRGBSpace()) PCLXLPageState.SetColorPage(true);
                                    goto default;
                                case ENUM_PCLXL.OPERATOR.ReadImage:
                                case ENUM_PCLXL.OPERATOR.EndImage:
                                case ENUM_PCLXL.OPERATOR.SetColorTreatment:
                                case ENUM_PCLXL.OPERATOR.SetColorTrapping:
                                case ENUM_PCLXL.OPERATOR.SetBrushSource:
                                case ENUM_PCLXL.OPERATOR.SetPenSource:
                                case ENUM_PCLXL.OPERATOR.PushGS:
                                case ENUM_PCLXL.OPERATOR.PopGS:
                                case ENUM_PCLXL.OPERATOR.SetColorSpace:
                                case ENUM_PCLXL.OPERATOR.EndSession:
                                default:
                                    // Primero mostramos información de los atributos del operador
                                    while (ColaAtributos.Count > 0)
                                    {
                                        Log.Debug(ColaAtributos.Dequeue());
                                    }

                                    // Información del operador
                                    Log.Debug("   Pos. " + Util.MostrarHex((UInt32)(PosTagPCLXL.AnalizeTag)) + "     " + "PCLXL " + Enum.GetName(typeof(ENUM_PCLXL.TAGTYPE), ENUM_PCLXL.TAGTYPE.Operator) + "  ===>>>      " + Util.MostrarHex(PCLXLByte) + "            " + DICTIONARY_PCLXL.OPERATOR[PCLXLByte]);
                                    Log.Debug("--------------------------------------------------------------------------------");

                                    // Guardar operador para mostrar un Resumen de tags
                                    ResumenPCLXL.Agregar(DICTIONARY_PCLXL.OPERATOR[PCLXLByte]);
                                    break;
                            }

                            if (ENUM_PCLXL.OPERATOR.EndPage == (ENUM_PCLXL.OPERATOR)PCLXLByte)
                            {
                                /// - Código para actualizar propiedades del trabajo de impresión
                            }
                            break;

                        case ENUM_PCLXL.TAGTYPE.Indefinido:
                            Log.Error("El Tag PCLXL: " + Util.MostrarHex(PCLXLByte) + " no está definido");
                            break;
                    }
                } // End While
            }
            catch (Exception)
            {
                throw new Exception("Falla el análisis PCLXL del fichero: " + this.PathSpoolFile + " con " + this.SizeFile + " bytes.");
            }

            Log.Info("Posición: (" + Util.MostrarHex((UInt32)(PosicionPCLXL-1)) + "): " + " - Finaliza analisis Bloque datos PCLXL.");

            // Se genera log con un resumen de los Tags para operadores
            ResumenPCLXL.ResumenTags();

            Log.Info("/*------------------------------------------------------------------------------------------*/");
            Log.Info("                                FIN ANALISIS PCLXL");
            Log.Info("/*------------------------------------------------------------------------------------------*/" + (char)Util._END_LINE1);

            // Control para realizar pruebas automáticas
            this.AnalisisCompletado = true;
        } // Fin AnalizePCLXL()
    }
}

