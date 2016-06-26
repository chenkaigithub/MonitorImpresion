using System;
using System.IO;
using System.Collections;
using log4net;

namespace MonitorTrabajosImpresion
{
    /** \brief Con esta clase se analizan los ficheros de spool con contenido EMF
     */
    public class EmfSpool: Spool
    {
        // El fichero de Spool contiene registros DevMode
        //public bool RecordDevmode { get; set; }

        // Se crea el Logger con nombre: EmfSpool
        private static readonly ILog Log = LogManager.GetLogger("EmfSpool");

        /** \brief Construtor EmfSpool
         * \param PathFile ruta fichero de spool formato Emf
         */
        public EmfSpool(string PathFile) : base (PathFile)
        {
            Log.Info(">>>> Fichero de Spool para analisis EMF: " + PathFile);
            //RecordDevmode = false;
        }



        /** \brief Lectura registro DEVMODE
         * \details Lectura y analisis registro DEVMODE, a partir de la posición en el fichero de Spool. Su estructura se define en el documento MS-RPRN 2.2.2.1
         * \param EmfFile referencia a fichero EMF
         * \param PositionDEVMODE posicion del registro DEVMODE en el fichero EMF
         * \param PrintJob Estructura pasada por referencia para guardar las propiedades del trabajo de impresión
         */
        //private void LeerDevmode(string PathFile, int posicionDEVMODE)
        private void LeerDevmode(ref BinaryReader EmfFile, int PositionDEVMODE, ref STRUCT_PRINT_JOB PrintJob)
        {
            STRUCT_API_PRINTING.DEVMODE RegDevmode = new STRUCT_API_PRINTING.DEVMODE();
            //BinaryReader EmfFile = new BinaryReader(File.Open(PathFile, FileMode.Open));

            /// Se obtiene el contenido del registro EMRI_DEVMODE en una estructura tipo DEVMODE
            EmfFile.BaseStream.Seek(PositionDEVMODE, SeekOrigin.Begin);
            RegDevmode.DeviceName = Util.DevmodeString32(EmfFile.ReadBytes(64));

            RegDevmode.SpecVersion = (ushort)EmfFile.ReadInt16();
            RegDevmode.DriverVersion = (ushort)EmfFile.ReadInt16();
            RegDevmode.Size = (ushort)EmfFile.ReadInt16();
            RegDevmode.DriverExtra = (ushort)EmfFile.ReadInt16();
            RegDevmode.Fields = (uint)EmfFile.ReadInt32();
            RegDevmode.Orientation = EmfFile.ReadInt16();
            RegDevmode.PaperSize = EmfFile.ReadInt16();
            RegDevmode.PaperLength = EmfFile.ReadInt16();
            RegDevmode.PaperWidth = EmfFile.ReadInt16();
            RegDevmode.Scale = EmfFile.ReadInt16();
            RegDevmode.Copies = EmfFile.ReadInt16();
            RegDevmode.DefaultSource = EmfFile.ReadInt16();
            RegDevmode.PrintQuality = EmfFile.ReadInt16();
            RegDevmode.Color = EmfFile.ReadInt16();
            RegDevmode.Duplex = EmfFile.ReadInt16();
            RegDevmode.YResolution = EmfFile.ReadInt16();
            RegDevmode.TTOption = EmfFile.ReadInt16();
            RegDevmode.Collate = EmfFile.ReadInt16();
            RegDevmode.FormName = Util.DevmodeString32(EmfFile.ReadBytes(64));

            RegDevmode.LogPixels = (ushort)EmfFile.ReadInt16();
            RegDevmode.BitsPerPel = (uint)EmfFile.ReadInt32();
            RegDevmode.PelsWidth = (uint)EmfFile.ReadInt32();
            RegDevmode.PelsHeight = (uint)EmfFile.ReadInt32();
            RegDevmode.Nup = (uint)EmfFile.ReadInt32();
            RegDevmode.DisplayFrequency = (uint)EmfFile.ReadInt32();
            RegDevmode.ICMMethod = (uint)EmfFile.ReadInt32();
            RegDevmode.ICMIntent = (uint)EmfFile.ReadInt32();
            RegDevmode.MediaType = (uint)EmfFile.ReadInt32();
            RegDevmode.DitherType = (uint)EmfFile.ReadInt32();
            RegDevmode.Reserved1 = (uint)EmfFile.ReadInt32();
            RegDevmode.Reserved2 = (uint)EmfFile.ReadInt32();
            RegDevmode.PanningWidth = (uint)EmfFile.ReadInt32();
            RegDevmode.PanningHeight = (uint)EmfFile.ReadInt32();
            /// Generación de log con el contenido del registro DEVMODE
            Log.Info("<<<<<<<<<<<<<<<      Registro DEVMODE en fichero de Spool con lenguaje EMF      >>>>>>>>>>>>>>>>");
            Log.Info("         - DeviceName: " + RegDevmode.DeviceName);
            Log.Info("         - SpecVersion: " + RegDevmode.SpecVersion);
            Log.Info("         - DriverVersion: " + RegDevmode.DriverVersion);
            Log.Info("         - Size: " + RegDevmode.Size);
            Log.Info("         - DriverExtra: " + RegDevmode.DriverExtra);
            Log.Info("         - Fields: " + RegDevmode.Fields);
            Log.Info("         - Orientation: " + RegDevmode.Orientation);
            Log.Info("         - PaperSize: " + RegDevmode.PaperSize);
            Log.Info("         - PaperLength: " + RegDevmode.PaperLength);
            Log.Info("         - PaperWidth: " + RegDevmode.PaperWidth);
            Log.Info("         - Scale: " + RegDevmode.Scale);
            Log.Info("         - Copies: " + RegDevmode.Copies);
            Log.Info("         - DefaultSource: " + RegDevmode.DefaultSource);
            Log.Info("         - PrintQuality: " + RegDevmode.PrintQuality);
            Log.Info("         - Color: " + RegDevmode.Color);
            Log.Info("         - Duplex: " + RegDevmode.Duplex);
            Log.Info("         - YResolution: " + RegDevmode.YResolution);
            Log.Info("         - TTOption: " + RegDevmode.TTOption);
            Log.Info("         - Collate: " + RegDevmode.Collate);
            Log.Info("         - FormName: " + RegDevmode.FormName);
            Log.Info("         - LogPixels: " + RegDevmode.LogPixels);
            Log.Info("         - BitsPerPel: " + RegDevmode.BitsPerPel);
            Log.Info("         - PelsWidth: " + RegDevmode.PelsWidth);
            Log.Info("         - PelsHeight: " + RegDevmode.PelsHeight);
            Log.Info("         - Nup: " + RegDevmode.Nup);
            Log.Info("         - DisplayFrequency: " + RegDevmode.DisplayFrequency);
            Log.Info("         - ICMMethod: " + RegDevmode.ICMMethod);
            Log.Info("         - ICMIntent: " + RegDevmode.ICMIntent);
            Log.Info("         - MediaType: " + RegDevmode.MediaType);
            Log.Info("         - DitherType: " + RegDevmode.DitherType);
            Log.Info("         - Reserved1: " + RegDevmode.Reserved1);
            Log.Info("         - Reserved2: " + RegDevmode.Reserved2);
            Log.Info("         - PanningWidth: " + RegDevmode.PanningWidth);
            Log.Info("         - PanningHeight: " + RegDevmode.PanningHeight);
            Log.Info("");

            /// Propiedades del Trabajo de impresión
            PrintJob.ID_PRINTER = RegDevmode.DeviceName;
            PrintJob.N_LENGTH = RegDevmode.PaperLength;
            PrintJob.N_WIDTH = RegDevmode.PaperWidth;
            PrintJob.ID_MEDIASIZE = DICTIONARY_API_PRINTING.PAPERSIZE[RegDevmode.PaperSize];
            PrintJob.N_MEDIASIZE = RegDevmode.PaperSize;
            PrintJob.ID_ORIENTATION = DICTIONARY_API_PRINTING.ORIENTATION[RegDevmode.Orientation];
            PrintJob.N_ORIENTATION = RegDevmode.Orientation;
            PrintJob.N_COPIES = RegDevmode.Copies;
            PrintJob.ID_COLOR = DICTIONARY_API_PRINTING.COLORSPACE[RegDevmode.Color];
            PrintJob.N_COLOR = RegDevmode.Color;
            PrintJob.ID_DUPLEX = DICTIONARY_API_PRINTING.DUPLEX_PAGEMODE[RegDevmode.Duplex];
            PrintJob.N_DUPLEX = RegDevmode.Duplex;
            PrintJob.N_MEDIATYPE = (int) RegDevmode.MediaType;

        } // LeerDevmode


        /** \brief Lectura registro EmrHeader
         * \details Lectura y analisis registro EMR_HEADER, Su estructura se define en el documento MS-EMF, título 2.3.4.2.
         * \param ArrayEmrHeader Array con el contenido del registro EMR_HEADER.
         * \param PrintJob Estructura pasada por referencia para guardar las propiedades del trabajo de impresión
         * \param Position Posición del registro EMR_HEADER en el fichero de Spool
         */
        private void AnalizeEmrHeader(byte [] ArrayEmrHeader, ref STRUCT_PRINT_JOB PrintJob, int Position)
        {
            // Primeros elementos del registro:
            //  - Type: 4 bytes (ya leido)
            //  - Size: 4 bytes (ya leido)
            //  - Bounds: 16 bytes.
            //  - Frame: 16 bytes. PuntoOrigen (Ox, Oy) Punto Final (Fx, Fy)
            // El resto de elementos no nos analizamos por que no lo necesitamos.

            int PosicionFrame = 24; // (Type + Size + Bounds) bytes

            // Coordenadas del frame
            uint Ox, Oy, Fx, Fy;
            Ox = (uint) BitConverter.ToInt32(ArrayEmrHeader, PosicionFrame);
            PosicionFrame += 4;
            Oy = (uint) BitConverter.ToInt32(ArrayEmrHeader, PosicionFrame);
            PosicionFrame += 4;
            Fx = (uint) BitConverter.ToInt32(ArrayEmrHeader, PosicionFrame);
            PosicionFrame += 4;
            Fy = (uint) BitConverter.ToInt32(ArrayEmrHeader, PosicionFrame);

            Log.Info("         " + Util.MostrarHex((UInt32)(Position + PosicionFrame)) + "    Frame:   " + "(" + Ox.ToString() + ", " + Oy.ToString() + "), (" + Fx.ToString() + ", " + Fy.ToString() + ")");

            // El siguiente If es por si un trabajo de impresión no tuviese el mismo tamaño de página para todas las páginas.
            // Comparamos con el tamaño de página guardado en el trabajo de impresión. Si el tamaño de página del EMR_HEADER es mayor actualizamos en el trabajo de impresion.
            if ((PrintJob.N_WIDTH * PrintJob.N_LENGTH) < ((Fx - Ox) * (Fy - Oy)))
            {
                PrintJob.N_WIDTH = (int)(Fy - Oy);
                PrintJob.N_LENGTH = (int)(Fx - Ox);
            }


        } // Fin LeerEmrHeader


        /** \brief Analisis EMF
         * \details analiza fichero de spool formato EMF para extraer los detalles relevantes de impresión.
         * Como detalles de impresión importantes en este análisis se extraen el número total de páginas impresas y de
         * ellas cuantas son en color.
         * También analizamos los registros DEVMODE.
         * \param PrintJob Estructura pasada por referencia para guardar las propiedades del trabajo de impresión
         */
        public new void Analize(ref STRUCT_PRINT_JOB PrintJob)
        {
            int PosicionEmf=0;
            int TipoRegistroEmf;
            int SizeRegistroEmf;
            ENUM_EMF.EMRI_RECORD TipoEmfRecord;
            BinaryReader EmfFile;

            // ResumenEMR: Objeto de la clase Tags para guardar un resumes de los Tags utilizados
            Tags ResumenEMF = new Tags();

            /// Apertura fichero de Spool
            try
            {
                EmfFile = new BinaryReader(File.Open(this.PathSpoolFile, FileMode.Open));
            }
            catch (Exception e)
            {
                Log.Error("No se puede abrir el fichero de spool: " + this.PathSpoolFile);
                throw e;
            }


            /// Análisis fichero de Spool
            try
            {
                Log.Info("Analisis mediante lenguaje EMF del ficheros de Spool: " + this.PathSpoolFile);
                /// Mientras existan registros EMFSPOOL
                while (PosicionEmf < this.SizeFile)
                {
                    /// Nos posicionamos para leer el siguiente registro EMFSPOOL
                    EmfFile.BaseStream.Seek(PosicionEmf, SeekOrigin.Begin);
                    /// Obtenemos el tipo y tamaño de rsgistro EMFSPOOL
                    TipoRegistroEmf = EmfFile.ReadInt32();
                    SizeRegistroEmf = EmfFile.ReadInt32();
                    // Determinar el tipo de registro en el enum EmfRecord
                    TipoEmfRecord = (ENUM_EMF.EMRI_RECORD)TipoRegistroEmf;
                    Log.Debug("Posición: (" + Util.MostrarHex((UInt32)PosicionEmf) + ") registro EMF: " + DICTIONARY_EMF.EMRI_RECORD[TipoRegistroEmf] + " Tamaño: " + Util.MostrarHex((UInt32)(SizeRegistroEmf)) + ". ");

                    // Guardamos información del Tag Para presentar resumen al final del análisis
                    ResumenEMF.Agregar(DICTIONARY_EMF.EMRI_RECORD[TipoRegistroEmf]);

                    /// Análisis del registro EMFSPOOL
                    // Según el tipo de registro EMF calculamos la siguiente posicion
                    switch (TipoEmfRecord)
                    {
                        case ENUM_EMF.EMRI_RECORD.HEADER_RECORD:
                            // Analisis Registro Cabecera
                            PosicionEmf = SizeRegistroEmf;
                            break;
                        case ENUM_EMF.EMRI_RECORD.EMRI_DEVMODE: // OK 
                            /// Si es registro EMRI_DEVMODE guardamos en el Log su contenido y guardamos propiedades del trabajo de impresión en la estructura con las propiedades del trabajo
                            /// de impresión.
                            int PosicionEmfDevmode = PosicionEmf + 8;
                            // El siguiente método lee el registro DEVMODE y genera el log. Falta implementar el analisis
                            LeerDevmode(ref EmfFile, PosicionEmfDevmode, ref PrintJob);
                            PosicionEmf = PosicionEmf + 8 + SizeRegistroEmf;
                            break;

                        // Los 5 siguientes casos contienen el contenido de la página a imprimir
                        case ENUM_EMF.EMRI_RECORD.EMRI_METAFILE:
                        case ENUM_EMF.EMRI_RECORD.EMRI_FORM_METAFILE:
                        case ENUM_EMF.EMRI_RECORD.EMRI_BW_METAFILE:
                        case ENUM_EMF.EMRI_RECORD.EMRI_BW_FORM_METAFILE:
                        case ENUM_EMF.EMRI_RECORD.EMRI_METAFILE_DATA:
                            /// Si es un registro EMFSPOOL con contenido de página, realizamos un análisis EMF de su contenido. Para ello se utiliza el método AnalizeEMR().
                            AnalizeEMR(ref EmfFile, PosicionEmf + 8, SizeRegistroEmf, ref ResumenEMF, ref PrintJob);
                            PosicionEmf = PosicionEmf + 8 + SizeRegistroEmf;
                            break;                            

                        // Los dos siguiente casos marcan el final del contenido de página
                        /// Si el registro EMFSPOOL es del tipo EMRI_METAFILE_EXT -> El contenido de la Pagina es en color
                        case ENUM_EMF.EMRI_RECORD.EMRI_METAFILE_EXT: // Página con contenido en color
                            PrintJob.N_COLORPAGES++;
                            goto case ENUM_EMF.EMRI_RECORD.EMRI_BW_METAFILE_EXT;
                        // Si el registro EMFSPOOL es del tipo EMRI_BW_METAFILE_EXT -> El contenido de la página sólo es en Blanco y Negro.
                        case ENUM_EMF.EMRI_RECORD.EMRI_BW_METAFILE_EXT: //Página con contenido solo en Blanco-Negro
                            PrintJob.N_PAGES++;
                            PrintJob.N_PAGES_PRINTED++;
                            PosicionEmf = PosicionEmf + 8 + SizeRegistroEmf;
                            break;

                        case ENUM_EMF.EMRI_RECORD.EMRI_PS_JOB_DATA:
                        case ENUM_EMF.EMRI_RECORD.EMRI_ENGINE_FONT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_TYPE1_FONT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_DESIGNVECTOR:
                        case ENUM_EMF.EMRI_RECORD.EMRI_SUBSET_FONT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_DELTA_FONT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_ENGINE_FONT_EXT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_TYPE1_FONT_EXT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_DESIGNVECTOR_EXT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_SUBSET_FONT_EXT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_DELTA_FONT_EXT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_EMBED_FONT_EXT:
                        case ENUM_EMF.EMRI_RECORD.EMRI_PRESTARTPAGE:
                            PosicionEmf = PosicionEmf + 8 + SizeRegistroEmf;
                            break;
                        default:
                            Log.Fatal("No existe el tipo de registro EMF: " + Util.MostrarHex((UInt32)TipoEmfRecord));
                            throw new Exception("No existe el tipo de registro EMF: " + Util.MostrarHex((UInt32)TipoEmfRecord));
                    } //switch

                    Log.Debug("Posicion siguiente registro EMFSPOOL: (" + Util.MostrarHex((UInt32)(PosicionEmf)) + ")");
                } //while
            } //try
            catch (Exception e)
            {
                EmfFile.Close();
                Log.Info("El fichero de Spool no se ha podido analizar mediante lenguaje EMF");
                throw e;
            }
            Log.Info("Cerramos el fichero EMF");

            /// Se cierra el fichero de Spool
            EmfFile.Close();

            /// Se genera log con el resumen del análisis EMF del fichero de Spool
            ResumenEMF.ResumenTags();

            Log.Info("FIN Analisis EMF");
            Log.Info("----------------------------------------------");
            AnalisisCompletado = true;
        } //AnalizeEmf()

        /** \brief Analisis EMF Metafile record
         * \param EmfFile referencia al fichero de spool EMF
         * \param positionMetafileRecord Posición comienzo Metafile
         * \param Size tamaño del registro a analizar
         * \param ResumenEMR Objeto de la clase Tags para guardar un resumes de los Tags utilizados
         * \param PrintJob Estructura pasada por referencia para guardar las propiedades del trabajo de impresión
         * \details Se analizan los registros que guardan el contenido de las páginas a imprimir definidas mediante lenguaje EMF.
         */
        private void AnalizeEMR(ref BinaryReader EmfFile, int positionMetafileRecord, int Size, ref Tags ResumenEMR, ref STRUCT_PRINT_JOB PrintJob)
        {
            int Position = positionMetafileRecord;
            int EmrRecord = 0;
            int EmrSize = 0;
            int EmrValue = 0;

            /// recorremos el registro EMFSPOOL con contenido de página identificando los registros tipo EMR de definición de contenido 
            while (Position < (positionMetafileRecord + Size))
            {
                /// Nos posicionamos en el siguiente registro EMF a analizar
                EmfFile.BaseStream.Seek(Position, SeekOrigin.Begin);
                EmrRecord = EmfFile.ReadInt32();
                EmrSize = EmfFile.ReadInt32();
                /// Se genera log con información de los registros EMF que se van identificando
                Log.Debug("Posición " + Util.MostrarHex((UInt32)(Position)) + " " + DICTIONARY_EMF.EMR_RECORD[EmrRecord] + ". Tamaño registro: " + Util.MostrarHex((UInt32)(EmrSize)));

                // Guardamos información del Tag Para presentar resumen al final del análisis
                ResumenEMR.Agregar(DICTIONARY_EMF.EMR_RECORD[EmrRecord]);


                if ((DICTIONARY_EMF.EMR_RECORD[EmrRecord] == "EMR_SETTEXTCOLOR") || 
                    (DICTIONARY_EMF.EMR_RECORD[EmrRecord] == "EMR_SELECTOBJECT") || 
                    (DICTIONARY_EMF.EMR_RECORD[EmrRecord] == "EMR_DELETEOBJECT"))
                {
                    EmrValue = EmfFile.ReadInt32();
                    //Log.Debug("         " + Util.MostrarHex((UInt32)(Position + 8)) + "    Value  " + Util.MostrarHex((UInt32)(EmrValue)));
                }

                /// Si es un registro de cabecera EMR_HEADER lo analizamos utilizando el método AnalizeEmrHeader()
                if ((DICTIONARY_EMF.EMR_RECORD[EmrRecord] == "EMR_HEADER"))
                {
                    byte[] ArrayEmrHeader = new byte[EmrSize];
                    EmfFile.BaseStream.Seek(Position, SeekOrigin.Begin);
                    EmfFile.Read(ArrayEmrHeader, 0, EmrSize);

                    // Analizamos el registro EMR_HEADER para obtener el tamaño de página
                    AnalizeEmrHeader(ArrayEmrHeader, ref PrintJob, Position);
                }


                Position += EmrSize; 
            } //while
        } // AnalizeEmfMetafileRecord()
    }
}
