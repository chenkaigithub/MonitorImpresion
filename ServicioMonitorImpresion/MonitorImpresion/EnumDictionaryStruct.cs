using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using log4net;

namespace MonitorTrabajosImpresion
{
    /** \brief Clase con enumeraciones para analizar cadenas JPL.
     */
    public class ENUM_JPL
    {
        /** \brief Propiedades de los trabajos de impresion 
        */  
        public enum PROPERTY //:byte
        {
            ID_JOBNAME = 1,
            ID_FUENTE = 2,
            N_JOB = 3,
            F_PRINTJOB = 4,
            ID_LOGIN = 5,
            ID_PRINTSERVER = 6,
            ID_PRINTER = 7,
            ID_DOCUMENT = 8,
            N_PAGES = 9,
            N_PAGES_PRINTED = 10,
            N_COLORPAGES = 11,
            N_LENGTH = 12,
            N_WIDTH = 13,
            ID_MEDIASIZE = 14,
            N_MEDIASIZE = 15,
            N_ORIENTATION = 16,
            ID_ORIENTATION = 17,
            ID_COLOR = 18,
            N_COLOR = 19,
            ID_DUPLEX = 20,
            N_DUPLEX = 21,
            N_COPIES = 22,
            ID_STATUS = 23,
        }
    }

    /** \brief Clase con diccionarios para analizar cadenas JPL.
     */
    public class DICTIONARY_JPL
    {
        public static Dictionary<string, ENUM_JPL.PROPERTY> PROPERTY = LoadPropertyJPL();
        /** \brief Carga el diccionario EMRI_RECORD */
        private static Dictionary<string, ENUM_JPL.PROPERTY> LoadPropertyJPL()
        {
            /// cada entrada en el diccionario se corresponde con un parámetro JPL de interés
            /// Para cada parámetro se busca si existe en la cadena JPL
            /// Si es así se extrae el valor y se pasa en un resgistro el valor extraido y el valor del diccioanrio que
            /// es el valor de la enumeracion
            Dictionary<string, ENUM_JPL.PROPERTY> Param = new Dictionary<string, ENUM_JPL.PROPERTY>();
            
            Param.Add("SET TIMESTAMP", ENUM_JPL.PROPERTY.F_PRINTJOB);
            Param.Add("JobAcct4", ENUM_JPL.PROPERTY.F_PRINTJOB);
            Param.Add("Username", ENUM_JPL.PROPERTY.ID_LOGIN);
            Param.Add("JobAcct1", ENUM_JPL.PROPERTY.ID_LOGIN);
            Param.Add("JobAcct2", ENUM_JPL.PROPERTY.ID_PRINTSERVER);
            Param.Add("Target Printer", ENUM_JPL.PROPERTY.ID_PRINTER);
            Param.Add("JOB NAME", ENUM_JPL.PROPERTY.ID_DOCUMENT);
            Param.Add("SET JOBNAME", ENUM_JPL.PROPERTY.ID_DOCUMENT);
            Param.Add("SET PAPERLENGTH", ENUM_JPL.PROPERTY.N_LENGTH);
            Param.Add("SET PAPERWIDTH", ENUM_JPL.PROPERTY.N_WIDTH);
            Param.Add("SET PAPER=", ENUM_JPL.PROPERTY.ID_MEDIASIZE);
            Param.Add("SET PAPER =", ENUM_JPL.PROPERTY.ID_MEDIASIZE); 
            Param.Add("SET ORIENTATION", ENUM_JPL.PROPERTY.ID_ORIENTATION);
            Param.Add("SET RENDERMODE", ENUM_JPL.PROPERTY.ID_COLOR);
            Param.Add("SET COLORSPACE", ENUM_JPL.PROPERTY.ID_COLOR);
            Param.Add("SET GRAYSCALE", ENUM_JPL.PROPERTY.ID_COLOR);
            Param.Add("SET COLORMODE", ENUM_JPL.PROPERTY.ID_COLOR);
            Param.Add("SET DUPLEX", ENUM_JPL.PROPERTY.ID_DUPLEX);
            Param.Add("ENTER LANGUAGE", ENUM_JPL.PROPERTY.ID_FUENTE);

            return Param;
        } //LoadParam()
    }

    /** \brief Contenedor enumeraciones PCLXL
     */
    public class ENUM_PCLXL
    {
        public enum TAGTYPE : byte
        {
            Attribute = 0x01,
            AttributeDefiner = 0x02,
            DataType = 0x03,
            EmbedDataDefiner = 0x04,
            Operator = 0x05,
            Whitespace = 0x06,
            Indefinido = 0x07
        }

        public enum ATTRIBUTE
        {
            PaletteDepth = 0x02,
            ColorSpace = 0x03,
            NullBrush = 0x04,
            NullPen = 0x05,
            PaletteData = 0x06,
            PaletteIndex = 0x07, // **
            PatternSelectID = 0x08,
            GrayLevel = 0x09,
            RGBColor = 0x0b,
            PatternOrigin = 0x0c,
            NewDestinationSize = 0x0d,
            PrimaryArray = 0x0e,
            PrimaryDepth = 0x0f,
            AllObjectTypes = 0x1d,
            TextObjects = 0x1e,
            VectorObjects = 0x1f,
            RasterObjects = 0x20,
            DeviceMatrix = 0x21,
            DitherMatrixDataType = 0x22,
            DitherOrigin = 0x23,
            MediaDestination = 0x24, // **
            MediaSize = 0x25,
            MediaSource = 0x26,
            MediaType = 0x27, // **
            Orientation = 0x28,
            PageAngle = 0x29,
            PageOrigin = 0x2a,
            PageScale = 0x2b,
            ROP3 = 0x2c,
            TxMode = 0x2d,
            CustomMediaSize = 0x2f,
            CustomMediaSizeUnits = 0x30,
            PageCopies = 0x31,
            DitherMatrixSize = 0x32,
            DitherMatrixDepth = 0x33,
            SimplexPageMode = 0x34,
            DuplexPageMode = 0x35,
            DuplexPageSide = 0x36,
            ArcDirection = 0x41,
            BoundingBox = 0x42,
            DashOffset = 0x43,
            EllipseDimension = 0x44,
            EndPoint = 0x45,
            FillMode = 0x46,
            LineCapStyle = 0x47,
            LineJoinStyle = 0x48,
            MiterLength = 0x49,
            LineDashStyle = 0x4a,
            PenWidth = 0x4b,
            Point = 0x4c,
            NumberOfPoints = 0x4d,
            SolidLine = 0x4e,
            StartPoint = 0x4f,
            PointType = 0x50,
            ControlPoint1 = 0x51,
            ControlPoint2 = 0x52,
            ClipRegion = 0x53,
            ClipMode = 0x54,
            ColorDepthArray = 0x61, // **
            ColorDepth = 0x62,
            BlockHeight = 0x63,
            ColorMapping = 0x64,
            CompressMode = 0x65,
            DestinationBox = 0x66,
            DestinationSize = 0x67,
            PatternPersistence = 0x68,
            PatternDefineID = 0x69,
            SourceHeight = 0x6b,
            SourceWidth = 0x6c,
            StartLine = 0x6d,
            PadBytesMultiple = 0x6e,
            BlockByteLength = 0x6f,
            NumberOfScanLines = 0x73,
            PrintableArea = 0x74, // **
            TumbleMode = 0x75, // **
            ContentOrientation = 0x76, // **
            FeedOrientation = 0x77, // **
            ColorTreatment = 0x78,
            CommentData = 0x81,
            DataOrg = 0x82,
            Measure = 0x86,
            SourceType = 0x88,
            UnitsPerMeasure = 0x89,
            QueryKey = 0x8a, // **
            StreamName = 0x8b,
            StreamDataLength = 0x8c,
            PCLSelectFont = 0x8d,
            ErrorReport = 0x8f,
            VUExtension = 0x91, // **
            VUDataLength = 0x92, // **
            VUAttr1 = 0x93, // **
            VUAttr2 = 0x94, // **
            VUAttr3 = 0x95, // **
            VUAttr4 = 0x96, // **
            VUAttr5 = 0x97, // **
            VUAttr6 = 0x98, // **
            VUAttr7 = 0x99, // **
            VUAttr8 = 0x9a, // **
            VUAttr9 = 0x9b, // **
            VUAttr10 = 0x9c, // **
            VUAttr11 = 0x9d, // **
            VUAttr12 = 0x9e, // **
            EnableDiagnostics = 0xa0, // **
            CharAngle = 0xa1,
            CharCode = 0xa2,
            CharDataSize = 0xa3,
            CharScale = 0xa4,
            CharShear = 0xa5,
            CharSize = 0xa6,
            FontHeaderLength = 0xa7,
            FontName = 0xa8,
            FontFormat = 0xa9,
            SymbolSet = 0xaa,
            TextData = 0xab,
            CharSubModeArray = 0xac,
            WritingMode = 0xad,
            BitmapCharScaling = 0xae, // **
            XSpacingData = 0xaf,
            YSpacingData = 0xb0,
            CharBoldValue = 0xb1, // **
        }

        public enum OPERATOR
        {
            BeginSession = 0x41,
            EndSession = 0x42,
            BeginPage = 0x43,
            EndPage = 0x44,
            VendorUnique = 0x46,
            Comment = 0x47,
            OpenDataSource = 0x48,
            CloseDataSource = 0x49,
            EchoComment = 0x4a,
            Query = 0x4b,
            BeginFontHeader = 0x4f,
            ReadFontHeader = 0x50,
            EndFontHeader = 0x51,
            BeginChar = 0x52,
            ReadChar = 0x53,
            EndChar = 0x54,
            RemoveFont = 0x55,
            SetCharAttributes = 0x56,
            SetDefaultGS = 0x57,
            SetColorTreatment = 0x58,
            BeginStream = 0x5b,
            ReadStream = 0x5c,
            EndStream = 0x5d,
            ExecStream = 0x5e,
            RemoveStream = 0x5f,
            PopGS = 0x60,
            PushGS = 0x61,
            SetClipReplace = 0x62,
            SetBrushSource = 0x63,
            SetCharAngle = 0x64,
            SetCharScale = 0x65,
            SetCharShear = 0x66,
            SetClipIntersect = 0x67,
            SetClipRectangle = 0x68,
            SetClipToPage = 0x69,
            SetColorSpace = 0x6a,
            SetCursor = 0x6b,
            SetCursorRel = 0x6c,
            SetHalftoneMethod = 0x6d,
            SetFillMode = 0x6e,
            SetFont = 0x6f,
            SetLineDash = 0x70,
            SetLineCap = 0x71,
            SetLineJoin = 0x72,
            SetMiterLimit = 0x73,
            SetPageDefaultCTM = 0x74,
            SetPageOrigin = 0x75,
            SetPageRotation = 0x76,
            SetPageScale = 0x77,
            SetPatternTxMode = 0x78,
            SetPenSource = 0x79,
            SetPenWidth = 0x7a,
            SetROP = 0x7b,
            SetSourceTxMode = 0x7c,
            SetCharBoldValue = 0x7d,
            SetNeutralAxis = 0x7e,
            SetClipMode = 0x7f,
            SetPathToClip = 0x80,
            SetCharSubMode = 0x81,
            CloseSubPath = 0x84,
            NewPath = 0x85,
            PaintPath = 0x86,
            ArcPath = 0x91,
            SetColorTrapping = 0x92,
            BezierPath = 0x93,
            SetAdaptiveHalfto = 0x94,
            BezierRelPath = 0x95,
            Chord = 0x96,
            ChordPath = 0x97,
            Ellipse = 0x98,
            EllipsePath = 0x99,
            LinePath = 0x9b,
            LineRelPath = 0x9d,
            Pie = 0x9e,
            PiePath = 0x9f,
            Rectangle = 0xa0,
            RectanglePath = 0xa1,
            RoundRectangle = 0xa2,
            RoundRectanglePat = 0xa3,
            Text = 0xa8,
            TextPath = 0xa9,
            SystemText = 0xaa,
            BeginImage = 0xb0,
            ReadImage = 0xb1,
            EndImage = 0xb2,
            BeginRastPattern = 0xb3,
            ReadRastPattern = 0xb4,
            EndRastPattern = 0xb5,
            BeginScan = 0xb6,
            EndScan = 0xb8,
            ScanLineRel = 0xb9,
            PassThrough = 0xbf
        }

        public enum ATTRIBUTE_DEFINER
        {
            ubyte = 0xf8,
            uint16 = 0xf9
        }

        public enum DATATYPE
        {
            ubyte = 0xc0,
            uint16 = 0xc1,
            uint32 = 0xc2,
            sint16 = 0xc3,
            sint32 = 0xc4,
            real32 = 0xc5,
            ubyte_array = 0xc8,
            uint16_array = 0xc9,
            uint32_array = 0xca,
            sint16_array = 0xcb,
            sint32_array = 0xcc,
            real32_array = 0xcd,
            ubyte_xy = 0xd0,
            uint16_xy = 0xd1,
            uint32_xy = 0xd2,
            sint16_xy = 0xd3,
            sint32_xy = 0xd4,
            real32_xy = 0xd5,
            ubyte_box = 0xe0,
            uint16_box = 0xe1,
            uint32_box = 0xe2,
            sint16_box = 0xe3,
            sint32_box = 0xe4,
            real32_box = 0xe5
        }

        public enum EMBED_DATA_DEFINER
        {
            dataLengthInteger = 0xfa,
            dataLengthByte = 0xfb,
        }

        public enum WHITESPACE
        {
            Null = 0x00,
            HorizontalTab = 0x09,
            LineFeed = 0x0a,
            VerticalTab = 0x0b,
            FormFeed = 0x0c,
            CarriageReturn = 0x0d,
            Space = 0x20
        }

        public enum COLOR
        {
            eBiLevel = 0,
            eGray = 1,
            eRGB = 2,
            eSRGB = 6,
            eGraySub = 7,
        }

        public enum DUPLEX
        {
            eDuplexHorizontalBinding = 0,
            eDuplexVerticalBinding = 1,
        }

        public enum MEDIA_SIZE
        {
            eLetterPaper = 0,
            eLegalPaper = 1,
            eJB4Paper = 10,
            eJB5Paper = 11,
            eB5Envelope = 12,
            eB5Paper = 13,
            eJPostcard = 14,
            eJDoublePostcard = 15,
            eA5Paper = 16,
            eA6Paper = 17,
            eJB6Paper = 18,
            eJIS8KPaper = 19,
            eA4Paper = 2,
            eJIS16KPaper = 20,
            eJISExecPaper = 21,
            eExecPaper = 3,
            eLedgerPaper = 4,
            eA3Paper = 5,
            eCOM10Envelope = 6,
            eMonarchEnvelope = 7,
            eC5Envelope = 8,
            eDLEnvelope = 9,
            eDefaultPapersize = 96,
        }

        public enum ORIENTATION
        {
            ePortraitOrientation = 0,
            eLandscapeOrientation = 1,
            eReversePortrait = 2,
            eReverseLandscape = 3,
            eDefaultOrientation = 4,
        }
    }

    /** \brief Definición diccionarios para analisis PCLXL */
    public class DICTIONARY_PCLXL
    {
        /** \brief Para un TAG PCLXL devuelve su tipo */
        public static Dictionary<byte, string> TAG_TO_TYPE = LoadTagToType();
        /** \brief Carga el diccionario TAG_TO_TYPE */
        private static Dictionary<byte, string> LoadTagToType()
        {
            Dictionary<byte, string> PCLXL_DiccionarioTagToType = new Dictionary<byte, string>();
            // Carga diccionario para tipos a través de un Tag
            PCLXL_DiccionarioTagToType.Add(0x41, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x42, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x43, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x44, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x46, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x47, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x48, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x49, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x4a, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x4b, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x4f, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x50, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x51, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x52, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x53, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x54, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x55, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x56, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x57, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x58, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x5b, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x5c, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x5d, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x5e, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x5f, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x60, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x61, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x62, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x63, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x64, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x65, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x66, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x67, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x68, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x69, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x6a, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x6b, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x6c, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x6d, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x6e, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x6f, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x70, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x71, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x72, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x73, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x74, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x75, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x76, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x77, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x78, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x79, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x7a, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x7b, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x7c, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x7d, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x7e, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x7f, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x80, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x81, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x84, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x85, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x86, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x91, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x92, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x93, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x94, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x95, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x96, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x97, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x98, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x99, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x9b, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x9d, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x9e, "Operator");
            PCLXL_DiccionarioTagToType.Add(0x9f, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xa0, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xa1, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xa2, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xa3, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xa8, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xa9, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xaa, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb0, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb1, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb2, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb3, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb4, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb5, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb6, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb8, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xb9, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xbf, "Operator");
            PCLXL_DiccionarioTagToType.Add(0xc0, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc1, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc2, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc3, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc4, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc5, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc8, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xc9, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xca, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xcb, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xcc, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xcd, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xd0, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xd1, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xd2, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xd3, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xd4, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xd5, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xe0, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xe1, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xe2, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xe3, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xe4, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xe5, "DataType");
            PCLXL_DiccionarioTagToType.Add(0xf8, "AttributeDefiner");
            PCLXL_DiccionarioTagToType.Add(0xfa, "EmbedDataDefiner");
            PCLXL_DiccionarioTagToType.Add(0xfb, "EmbedDataDefiner");
            return PCLXL_DiccionarioTagToType;
        } // Fin LoadTagToType()

        /** \brief A un tipo de TAG le asigna un código */
        public static Dictionary<string, ENUM_PCLXL.TAGTYPE> TYPE_TO_TAG = LoadTypeToTag();
        /** \brief Carga el diccionario TYPE_TO_TAG */
        private static Dictionary<string, ENUM_PCLXL.TAGTYPE> LoadTypeToTag()
        {
            Dictionary<string, ENUM_PCLXL.TAGTYPE> PCLXL_DiccionarioTypeToTag = new Dictionary<string, ENUM_PCLXL.TAGTYPE>();
            // Carga diccionario para obtener Tipos de tags
            PCLXL_DiccionarioTypeToTag.Add("Attribute", ENUM_PCLXL.TAGTYPE.Attribute);
            PCLXL_DiccionarioTypeToTag.Add("AttributeDefiner", ENUM_PCLXL.TAGTYPE.AttributeDefiner);
            PCLXL_DiccionarioTypeToTag.Add("DataType", ENUM_PCLXL.TAGTYPE.DataType);
            PCLXL_DiccionarioTypeToTag.Add("EmbedDataDefiner", ENUM_PCLXL.TAGTYPE.EmbedDataDefiner);
            PCLXL_DiccionarioTypeToTag.Add("Operator", ENUM_PCLXL.TAGTYPE.Operator);
            PCLXL_DiccionarioTypeToTag.Add("Whitespace", ENUM_PCLXL.TAGTYPE.Whitespace);
            PCLXL_DiccionarioTypeToTag.Add("Indefinido", ENUM_PCLXL.TAGTYPE.Indefinido);
            return PCLXL_DiccionarioTypeToTag;
        } // Fin LoadTypeToTag()

        /** \brief Para cada Tipo de dato simple se obtiene su tamaño en bytes */
        public static Dictionary<string, int> DATA_TYPE_SIMPLE_LEN = LoadDataTypeSimpleLen();
        /** \brief Carga el diccionario DATA_TYPE_SIMPLE_LEN */
        private static Dictionary<string, int> LoadDataTypeSimpleLen()
        {
            Dictionary<string, int> PCLXL_DiccionarioDataTypeSimpleLen = new Dictionary<string, int>();
            // Diccionario para datos simples
            PCLXL_DiccionarioDataTypeSimpleLen.Add("ubyte", 1);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("uint16", 2);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("uint32", 4);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("sint16", 2);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("sint32", 4);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("real32", 4);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("ubyte_xy", 2);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("uint16_xy", 4);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("uint32_xy", 8);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("sint16_xy", 4);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("sint32_xy", 8);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("real32_xy", 8);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("ubyte_box", 4);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("uint16_box", 8);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("uint32_box", 16);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("sint16_box", 8);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("sint32_box", 16);
            PCLXL_DiccionarioDataTypeSimpleLen.Add("real32_box", 16);
            return PCLXL_DiccionarioDataTypeSimpleLen;
        } // Fin LoadDataTypeSimpleLen()

        /** \brief Para cada Tipo de array se obtiene el tamaño en bytes de cada elemento */
        public static Dictionary<string, int> DATA_TYPE_ARRAY_LEN = LoadDataTypeArrayLen();
        /** \brief Carga el diccionario DATA_TYPE_ARRAY_LEN */
        private static Dictionary<string, int> LoadDataTypeArrayLen ()
        {
            Dictionary<string, int> PCLXL_DiccionarioDataTypeArrayLen = new Dictionary<string, int>();
            // Diccionario para Arrays
            PCLXL_DiccionarioDataTypeArrayLen.Add("ubyte_array", 1);
            PCLXL_DiccionarioDataTypeArrayLen.Add("uint16_array", 2);
            PCLXL_DiccionarioDataTypeArrayLen.Add("uint32_array", 4);
            PCLXL_DiccionarioDataTypeArrayLen.Add("sint16_array", 2);
            PCLXL_DiccionarioDataTypeArrayLen.Add("sint32_array", 4);
            PCLXL_DiccionarioDataTypeArrayLen.Add("real32_array", 4);
            return PCLXL_DiccionarioDataTypeArrayLen;
        } // Fin LoadDataTypeArrayLen ()

        /** \brief Indica el número de bytes necesarios para definir el tipo de datos */
        public static Dictionary<string, int> EMBED_DATA_DEFINER_LEN = LoadEmbedDataDefinerLen();
        /** \brief Carga el diccionario EMBED_DATA_DEFINER_LEN */
        private static Dictionary<string, int> LoadEmbedDataDefinerLen()
        {
            Dictionary<string, int> PCLXL_DiccionarioEmbedDataDefinerLen = new Dictionary<string, int>();
            // Diccionario para EmbedData
            PCLXL_DiccionarioEmbedDataDefinerLen.Add("dataLengthInteger", 4);
            PCLXL_DiccionarioEmbedDataDefinerLen.Add("dataLengthByte", 1);
            return PCLXL_DiccionarioEmbedDataDefinerLen;
        } // Fin LoadEmbedDataDefinerLen()


        /** \brief Dado el código numérico de atributo se obtiene su descripción */
        public static Dictionary<int, string> ATTRIBUTE = LoadAttribute();
        /** \brief Carga el diccionario ATTTRIBUTE */
        private static Dictionary<int, string> LoadAttribute()
        {
            Dictionary<int, string> PCLXL_DiccionarioAttribute = new Dictionary<int, string>();
            // Diccionario para Attribute
            PCLXL_DiccionarioAttribute.Add(0x02, "PaletteDepth");
            PCLXL_DiccionarioAttribute.Add(0x03, "ColorSpace");
            PCLXL_DiccionarioAttribute.Add(0x04, "NullBrush");
            PCLXL_DiccionarioAttribute.Add(0x05, "NullPen");
            PCLXL_DiccionarioAttribute.Add(0x06, "PaletteData");
            PCLXL_DiccionarioAttribute.Add(0x07, "PaletteIndex");
            PCLXL_DiccionarioAttribute.Add(0x08, "PatternSelectID");
            PCLXL_DiccionarioAttribute.Add(0x09, "GrayLevel");
            PCLXL_DiccionarioAttribute.Add(0x0b, "RGBColor");
            PCLXL_DiccionarioAttribute.Add(0x0c, "PatternOrigin");
            PCLXL_DiccionarioAttribute.Add(0x0d, "NewDestinationSize");
            PCLXL_DiccionarioAttribute.Add(0x0e, "PrimaryArray");
            PCLXL_DiccionarioAttribute.Add(0x0f, "PrimaryDepth");
            PCLXL_DiccionarioAttribute.Add(0x1d, "AllObjectTypes");
            PCLXL_DiccionarioAttribute.Add(0x1e, "TextObjects");
            PCLXL_DiccionarioAttribute.Add(0x1f, "VectorObjects");
            PCLXL_DiccionarioAttribute.Add(0x20, "RasterObjects");
            PCLXL_DiccionarioAttribute.Add(0x21, "DeviceMatrix");
            PCLXL_DiccionarioAttribute.Add(0x22, "DitherMatrixDataType");
            PCLXL_DiccionarioAttribute.Add(0x23, "DitherOrigin");
            PCLXL_DiccionarioAttribute.Add(0x24, "MediaDestination");
            PCLXL_DiccionarioAttribute.Add(0x25, "MediaSize");
            PCLXL_DiccionarioAttribute.Add(0x26, "MediaSource");
            PCLXL_DiccionarioAttribute.Add(0x27, "MediaType");
            PCLXL_DiccionarioAttribute.Add(0x28, "Orientation");
            PCLXL_DiccionarioAttribute.Add(0x29, "PageAngle");
            PCLXL_DiccionarioAttribute.Add(0x2a, "PageOrigin");
            PCLXL_DiccionarioAttribute.Add(0x2b, "PageScale");
            PCLXL_DiccionarioAttribute.Add(0x2c, "ROP3");
            PCLXL_DiccionarioAttribute.Add(0x2d, "TxMode");
            PCLXL_DiccionarioAttribute.Add(0x2f, "CustomMediaSize");
            PCLXL_DiccionarioAttribute.Add(0x30, "CustomMediaSizeUnits");
            PCLXL_DiccionarioAttribute.Add(0x31, "PageCopies");
            PCLXL_DiccionarioAttribute.Add(0x32, "DitherMatrixSize");
            PCLXL_DiccionarioAttribute.Add(0x33, "DitherMatrixDepth");
            PCLXL_DiccionarioAttribute.Add(0x34, "SimplexPageMode");
            PCLXL_DiccionarioAttribute.Add(0x35, "DuplexPageMode");
            PCLXL_DiccionarioAttribute.Add(0x36, "DuplexPageSide");
            PCLXL_DiccionarioAttribute.Add(0x41, "ArcDirection");
            PCLXL_DiccionarioAttribute.Add(0x42, "BoundingBox");
            PCLXL_DiccionarioAttribute.Add(0x43, "DashOffset");
            PCLXL_DiccionarioAttribute.Add(0x44, "EllipseDimension");
            PCLXL_DiccionarioAttribute.Add(0x45, "EndPoint");
            PCLXL_DiccionarioAttribute.Add(0x46, "FillMode");
            PCLXL_DiccionarioAttribute.Add(0x47, "LineCapStyle");
            PCLXL_DiccionarioAttribute.Add(0x48, "LineJoinStyle");
            PCLXL_DiccionarioAttribute.Add(0x49, "MiterLength");
            PCLXL_DiccionarioAttribute.Add(0x4a, "LineDashStyle");
            PCLXL_DiccionarioAttribute.Add(0x4b, "PenWidth");
            PCLXL_DiccionarioAttribute.Add(0x4c, "Point");
            PCLXL_DiccionarioAttribute.Add(0x4d, "NumberOfPoints");
            PCLXL_DiccionarioAttribute.Add(0x4e, "SolidLine");
            PCLXL_DiccionarioAttribute.Add(0x4f, "StartPoint");
            PCLXL_DiccionarioAttribute.Add(0x50, "PointType");
            PCLXL_DiccionarioAttribute.Add(0x51, "ControlPoint1");
            PCLXL_DiccionarioAttribute.Add(0x52, "ControlPoint2");
            PCLXL_DiccionarioAttribute.Add(0x53, "ClipRegion");
            PCLXL_DiccionarioAttribute.Add(0x54, "ClipMode");
            PCLXL_DiccionarioAttribute.Add(0x61, "ColorDepthArray");
            PCLXL_DiccionarioAttribute.Add(0x62, "ColorDepth");
            PCLXL_DiccionarioAttribute.Add(0x63, "BlockHeight");
            PCLXL_DiccionarioAttribute.Add(0x64, "ColorMapping");
            PCLXL_DiccionarioAttribute.Add(0x65, "CompressMode");
            PCLXL_DiccionarioAttribute.Add(0x66, "DestinationBox");
            PCLXL_DiccionarioAttribute.Add(0x67, "DestinationSize");
            PCLXL_DiccionarioAttribute.Add(0x68, "PatternPersistence");
            PCLXL_DiccionarioAttribute.Add(0x69, "PatternDefineID");
            PCLXL_DiccionarioAttribute.Add(0x6b, "SourceHeight");
            PCLXL_DiccionarioAttribute.Add(0x6c, "SourceWidth");
            PCLXL_DiccionarioAttribute.Add(0x6d, "StartLine");
            PCLXL_DiccionarioAttribute.Add(0x6e, "PadBytesMultiple");
            PCLXL_DiccionarioAttribute.Add(0x6f, "BlockByteLength");
            PCLXL_DiccionarioAttribute.Add(0x73, "NumberOfScanLines");
            PCLXL_DiccionarioAttribute.Add(0x74, "PrintableArea");
            PCLXL_DiccionarioAttribute.Add(0x75, "TumbleMode");
            PCLXL_DiccionarioAttribute.Add(0x76, "ContentOrientation");
            PCLXL_DiccionarioAttribute.Add(0x77, "FeedOrientation");
            PCLXL_DiccionarioAttribute.Add(0x78, "ColorTreatment");
            PCLXL_DiccionarioAttribute.Add(0x81, "CommentData");
            PCLXL_DiccionarioAttribute.Add(0x82, "DataOrg");
            PCLXL_DiccionarioAttribute.Add(0x86, "Measure");
            PCLXL_DiccionarioAttribute.Add(0x88, "SourceType");
            PCLXL_DiccionarioAttribute.Add(0x89, "UnitsPerMeasure");
            PCLXL_DiccionarioAttribute.Add(0x8a, "QueryKey");
            PCLXL_DiccionarioAttribute.Add(0x8b, "StreamName");
            PCLXL_DiccionarioAttribute.Add(0x8c, "StreamDataLength");
            PCLXL_DiccionarioAttribute.Add(0x8d, "PCLSelectFont");
            PCLXL_DiccionarioAttribute.Add(0x8f, "ErrorReport");
            PCLXL_DiccionarioAttribute.Add(0x91, "VUExtension");
            PCLXL_DiccionarioAttribute.Add(0x92, "VUDataLength");
            PCLXL_DiccionarioAttribute.Add(0x93, "VUAttr1");
            PCLXL_DiccionarioAttribute.Add(0x94, "VUAttr2");
            PCLXL_DiccionarioAttribute.Add(0x95, "VUAttr3");
            PCLXL_DiccionarioAttribute.Add(0x96, "VUAttr4");
            PCLXL_DiccionarioAttribute.Add(0x97, "VUAttr5");
            PCLXL_DiccionarioAttribute.Add(0x98, "VUAttr6");
            PCLXL_DiccionarioAttribute.Add(0x99, "VUAttr7");
            PCLXL_DiccionarioAttribute.Add(0x9a, "VUAttr8");
            PCLXL_DiccionarioAttribute.Add(0x9b, "VUAttr9");
            PCLXL_DiccionarioAttribute.Add(0x9c, "VUAttr10");
            PCLXL_DiccionarioAttribute.Add(0x9d, "VUAttr11");
            PCLXL_DiccionarioAttribute.Add(0x9e, "VUAttr12");
            PCLXL_DiccionarioAttribute.Add(0xa0, "EnableDiagnostics");
            PCLXL_DiccionarioAttribute.Add(0xa1, "CharAngle");
            PCLXL_DiccionarioAttribute.Add(0xa2, "CharCode");
            PCLXL_DiccionarioAttribute.Add(0xa3, "CharDataSize");
            PCLXL_DiccionarioAttribute.Add(0xa4, "CharScale");
            PCLXL_DiccionarioAttribute.Add(0xa5, "CharShear");
            PCLXL_DiccionarioAttribute.Add(0xa6, "CharSize");
            PCLXL_DiccionarioAttribute.Add(0xa7, "FontHeaderLength");
            PCLXL_DiccionarioAttribute.Add(0xa8, "FontName");
            PCLXL_DiccionarioAttribute.Add(0xa9, "FontFormat");
            PCLXL_DiccionarioAttribute.Add(0xaa, "SymbolSet");
            PCLXL_DiccionarioAttribute.Add(0xab, "TextData");
            PCLXL_DiccionarioAttribute.Add(0xac, "CharSubModeArray");
            PCLXL_DiccionarioAttribute.Add(0xad, "WritingMode");
            PCLXL_DiccionarioAttribute.Add(0xae, "BitmapCharScaling");
            PCLXL_DiccionarioAttribute.Add(0xaf, "XSpacingData");
            PCLXL_DiccionarioAttribute.Add(0xb0, "YSpacingData");
            PCLXL_DiccionarioAttribute.Add(0xb1, "CharBoldValue");
            return PCLXL_DiccionarioAttribute;
        } // LoadAttribute()


        /** \brief Dado el código numérico del DataType se obtiene su descripción */
        public static Dictionary<int, string> DATATYPE = LoadDataType();
        /** \brief Carga el diccionario DATATYPE */
        private static Dictionary<int, string> LoadDataType()
        {
            Dictionary<int, string> PCLXL_DiccionarioDataType = new Dictionary<int, string>();
            // Diccionario para DataType
            PCLXL_DiccionarioDataType.Add(0xc0, "ubyte");
            PCLXL_DiccionarioDataType.Add(0xc1, "uint16");
            PCLXL_DiccionarioDataType.Add(0xc2, "uint32");
            PCLXL_DiccionarioDataType.Add(0xc3, "sint16");
            PCLXL_DiccionarioDataType.Add(0xc4, "sint32");
            PCLXL_DiccionarioDataType.Add(0xc5, "real32");
            PCLXL_DiccionarioDataType.Add(0xc8, "ubyte_array");
            PCLXL_DiccionarioDataType.Add(0xc9, "uint16_array");
            PCLXL_DiccionarioDataType.Add(0xca, "uint32_array");
            PCLXL_DiccionarioDataType.Add(0xcb, "sint16_array");
            PCLXL_DiccionarioDataType.Add(0xcc, "sint32_array");
            PCLXL_DiccionarioDataType.Add(0xcd, "real32_array");
            PCLXL_DiccionarioDataType.Add(0xd0, "ubyte_xy");
            PCLXL_DiccionarioDataType.Add(0xd1, "uint16_xy");
            PCLXL_DiccionarioDataType.Add(0xd2, "uint32_xy");
            PCLXL_DiccionarioDataType.Add(0xd3, "sint16_xy");
            PCLXL_DiccionarioDataType.Add(0xd4, "sint32_xy");
            PCLXL_DiccionarioDataType.Add(0xd5, "real32_xy");
            PCLXL_DiccionarioDataType.Add(0xe0, "ubyte_box");
            PCLXL_DiccionarioDataType.Add(0xe1, "uint16_box");
            PCLXL_DiccionarioDataType.Add(0xe2, "uint32_box");
            PCLXL_DiccionarioDataType.Add(0xe3, "sint16_box");
            PCLXL_DiccionarioDataType.Add(0xe4, "sint32_box");
            PCLXL_DiccionarioDataType.Add(0xe5, "real32_box");
            return PCLXL_DiccionarioDataType;
        } //LoadDataType()

        /** \brief Dado el código numérico del Operator se obtiene su descripción */
        public static Dictionary<int, string> OPERATOR = LoadOperator();
        /** \brief Carga el diccionario OPERATOR */
        private static Dictionary<int, string> LoadOperator()
        {
            Dictionary<int, string> PCLXL_DiccionarioOperator = new Dictionary<int, string>();
            // Diccionario para Operator
            PCLXL_DiccionarioOperator.Add(0x41, "BeginSession");
            PCLXL_DiccionarioOperator.Add(0x42, "EndSession");
            PCLXL_DiccionarioOperator.Add(0x43, "BeginPage");
            PCLXL_DiccionarioOperator.Add(0x44, "EndPage");
            PCLXL_DiccionarioOperator.Add(0x46, "VendorUnique");
            PCLXL_DiccionarioOperator.Add(0x47, "Comment");
            PCLXL_DiccionarioOperator.Add(0x48, "OpenDataSource");
            PCLXL_DiccionarioOperator.Add(0x49, "CloseDataSource");
            PCLXL_DiccionarioOperator.Add(0x4a, "EchoComment");
            PCLXL_DiccionarioOperator.Add(0x4b, "Query");
            PCLXL_DiccionarioOperator.Add(0x4f, "BeginFontHeader");
            PCLXL_DiccionarioOperator.Add(0x50, "ReadFontHeader");
            PCLXL_DiccionarioOperator.Add(0x51, "EndFontHeader");
            PCLXL_DiccionarioOperator.Add(0x52, "BeginChar");
            PCLXL_DiccionarioOperator.Add(0x53, "ReadChar");
            PCLXL_DiccionarioOperator.Add(0x54, "EndChar");
            PCLXL_DiccionarioOperator.Add(0x55, "RemoveFont");
            PCLXL_DiccionarioOperator.Add(0x56, "SetCharAttributes");
            PCLXL_DiccionarioOperator.Add(0x57, "SetDefaultGS");
            PCLXL_DiccionarioOperator.Add(0x58, "SetColorTreatment");
            PCLXL_DiccionarioOperator.Add(0x5b, "BeginStream");
            PCLXL_DiccionarioOperator.Add(0x5c, "ReadStream");
            PCLXL_DiccionarioOperator.Add(0x5d, "EndStream");
            PCLXL_DiccionarioOperator.Add(0x5e, "ExecStream");
            PCLXL_DiccionarioOperator.Add(0x5f, "RemoveStream");
            PCLXL_DiccionarioOperator.Add(0x60, "PopGS");
            PCLXL_DiccionarioOperator.Add(0x61, "PushGS");
            PCLXL_DiccionarioOperator.Add(0x62, "SetClipReplace");
            PCLXL_DiccionarioOperator.Add(0x63, "SetBrushSource");
            PCLXL_DiccionarioOperator.Add(0x64, "SetCharAngle");
            PCLXL_DiccionarioOperator.Add(0x65, "SetCharScale");
            PCLXL_DiccionarioOperator.Add(0x66, "SetCharShear");
            PCLXL_DiccionarioOperator.Add(0x67, "SetClipIntersect");
            PCLXL_DiccionarioOperator.Add(0x68, "SetClipRectangle");
            PCLXL_DiccionarioOperator.Add(0x69, "SetClipToPage");
            PCLXL_DiccionarioOperator.Add(0x6a, "SetColorSpace");
            PCLXL_DiccionarioOperator.Add(0x6b, "SetCursor");
            PCLXL_DiccionarioOperator.Add(0x6c, "SetCursorRel");
            PCLXL_DiccionarioOperator.Add(0x6d, "SetHalftoneMethod");
            PCLXL_DiccionarioOperator.Add(0x6e, "SetFillMode");
            PCLXL_DiccionarioOperator.Add(0x6f, "SetFont");
            PCLXL_DiccionarioOperator.Add(0x70, "SetLineDash");
            PCLXL_DiccionarioOperator.Add(0x71, "SetLineCap");
            PCLXL_DiccionarioOperator.Add(0x72, "SetLineJoin");
            PCLXL_DiccionarioOperator.Add(0x73, "SetMiterLimit");
            PCLXL_DiccionarioOperator.Add(0x74, "SetPageDefaultCTM");
            PCLXL_DiccionarioOperator.Add(0x75, "SetPageOrigin");
            PCLXL_DiccionarioOperator.Add(0x76, "SetPageRotation");
            PCLXL_DiccionarioOperator.Add(0x77, "SetPageScale");
            PCLXL_DiccionarioOperator.Add(0x78, "SetPatternTxMode");
            PCLXL_DiccionarioOperator.Add(0x79, "SetPenSource");
            PCLXL_DiccionarioOperator.Add(0x7a, "SetPenWidth");
            PCLXL_DiccionarioOperator.Add(0x7b, "SetROP");
            PCLXL_DiccionarioOperator.Add(0x7c, "SetSourceTxMode");
            PCLXL_DiccionarioOperator.Add(0x7d, "SetCharBoldValue");
            PCLXL_DiccionarioOperator.Add(0x7e, "SetNeutralAxis");
            PCLXL_DiccionarioOperator.Add(0x7f, "SetClipMode");
            PCLXL_DiccionarioOperator.Add(0x80, "SetPathToClip");
            PCLXL_DiccionarioOperator.Add(0x81, "SetCharSubMode");
            PCLXL_DiccionarioOperator.Add(0x84, "CloseSubPath");
            PCLXL_DiccionarioOperator.Add(0x85, "NewPath");
            PCLXL_DiccionarioOperator.Add(0x86, "PaintPath");
            PCLXL_DiccionarioOperator.Add(0x91, "ArcPath");
            PCLXL_DiccionarioOperator.Add(0x92, "SetColorTrapping");
            PCLXL_DiccionarioOperator.Add(0x93, "BezierPath");
            PCLXL_DiccionarioOperator.Add(0x94, "SetAdaptiveHalfto");
            PCLXL_DiccionarioOperator.Add(0x95, "BezierRelPath");
            PCLXL_DiccionarioOperator.Add(0x96, "Chord");
            PCLXL_DiccionarioOperator.Add(0x97, "ChordPath");
            PCLXL_DiccionarioOperator.Add(0x98, "Ellipse");
            PCLXL_DiccionarioOperator.Add(0x99, "EllipsePath");
            PCLXL_DiccionarioOperator.Add(0x9b, "LinePath");
            PCLXL_DiccionarioOperator.Add(0x9d, "LineRelPath");
            PCLXL_DiccionarioOperator.Add(0x9e, "Pie");
            PCLXL_DiccionarioOperator.Add(0x9f, "PiePath");
            PCLXL_DiccionarioOperator.Add(0xa0, "Rectangle");
            PCLXL_DiccionarioOperator.Add(0xa1, "RectanglePath");
            PCLXL_DiccionarioOperator.Add(0xa2, "RoundRectangle");
            PCLXL_DiccionarioOperator.Add(0xa3, "RoundRectanglePat");
            PCLXL_DiccionarioOperator.Add(0xa8, "Text");
            PCLXL_DiccionarioOperator.Add(0xa9, "TextPath");
            PCLXL_DiccionarioOperator.Add(0xaa, "SystemText");
            PCLXL_DiccionarioOperator.Add(0xb0, "BeginImage");
            PCLXL_DiccionarioOperator.Add(0xb1, "ReadImage");
            PCLXL_DiccionarioOperator.Add(0xb2, "EndImage");
            PCLXL_DiccionarioOperator.Add(0xb3, "BeginRastPattern");
            PCLXL_DiccionarioOperator.Add(0xb4, "ReadRastPattern");
            PCLXL_DiccionarioOperator.Add(0xb5, "EndRastPattern");
            PCLXL_DiccionarioOperator.Add(0xb6, "BeginScan");
            PCLXL_DiccionarioOperator.Add(0xb8, "EndScan");
            PCLXL_DiccionarioOperator.Add(0xb9, "ScanLineRel");
            PCLXL_DiccionarioOperator.Add(0xbf, "PassThrough");
            return PCLXL_DiccionarioOperator;
        } //LoadOperator()

        /** \brief Dado el código numérico de ColorSpace se obtiene su descripción */
        public static Dictionary<int, string> COLORSPACE = LoadColorSpace();
        /** \brief Carga el diccionario COLORSPACE */
        private static Dictionary<int, string> LoadColorSpace()
        {
            Dictionary<int, string> PCLXL_DiccionarioColorSpace = new Dictionary<int, string>();
            // Diccionario para ColorSpace
            PCLXL_DiccionarioColorSpace.Add(0, "eBiLevel");
            PCLXL_DiccionarioColorSpace.Add(1, "eGray");
            PCLXL_DiccionarioColorSpace.Add(2, "eRGB");
            PCLXL_DiccionarioColorSpace.Add(6, "eSRGB");
            PCLXL_DiccionarioColorSpace.Add(7, "eGraySub");
            return PCLXL_DiccionarioColorSpace;
        } //LoadColorSpace()

        /** \brief Dado el código numérico de ColorMapping se obtiene su descripción */
        public static Dictionary<int, string> COLORMAPPING = LoadColorMapping();
        /** \brief Carga el diccionario COLORMAPPING */
        private static Dictionary<int, string> LoadColorMapping()
        {
            Dictionary<int, string> PCLXL_DiccionarioColorMapping = new Dictionary<int, string>();
            // Diccionario para ColorMapping
            PCLXL_DiccionarioColorMapping.Add(0, "eDirectPixel");
            PCLXL_DiccionarioColorMapping.Add(1, "eIndexedPixel");
            PCLXL_DiccionarioColorMapping.Add(2, "eDirectPlane");
            return PCLXL_DiccionarioColorMapping;
        } //LoadColorMapping()

        /** \brief Dado el código numérico de ColorDepth se obtiene su descripción */
        public static Dictionary<int, string> COLORDEPTH = LoadColorDepth();
        /** \brief Carga el diccionario COLORDEPTH */
        private static Dictionary<int, string> LoadColorDepth()
        {
            Dictionary<int, string> PCLXL_DiccionarioColorDepth = new Dictionary<int, string>();
            // Diccionario para ColorDepth
            PCLXL_DiccionarioColorDepth.Add(0, "e1Bit");
            PCLXL_DiccionarioColorDepth.Add(1, "e4Bit");
            PCLXL_DiccionarioColorDepth.Add(2, "e8Bit");
            return PCLXL_DiccionarioColorDepth;
        } //LoadColorDepth()

        /** \brief Dado el código numérico de ColorSpace se obtiene su descripción */
        public static Dictionary<int, string> ORIENTATION = LoadOrientation();
        /** \brief Carga el diccionario ORIENTATION */
        private static Dictionary<int, string> LoadOrientation()
        {
            Dictionary<int, string> PCLXL_DiccionarioOrientation = new Dictionary<int, string>();
            // Diccionario para Orientation
            PCLXL_DiccionarioOrientation.Add(0, "ePortraitOrientation");
            PCLXL_DiccionarioOrientation.Add(1, "eLandscapeOrientation");
            PCLXL_DiccionarioOrientation.Add(2, "eReversePortrait");
            PCLXL_DiccionarioOrientation.Add(3, "eReverseLandscape");
            PCLXL_DiccionarioOrientation.Add(4, "eDefaultOrientation");
            return PCLXL_DiccionarioOrientation;
        } //LoadOrientation()


        /** \brief Dado el código numérico de DuplexPageMode se obtiene su descripción */
        public static Dictionary<int, string> DUPLEX_PAGEMODE = LoadDuplexPageMode();
        /** \brief Carga el diccionario DUPLEX_PAGEMODE */
        private static Dictionary<int, string> LoadDuplexPageMode()
        {
            Dictionary<int, string> PCLXL_DiccionarioDuplexPageMode = new Dictionary<int, string>();
            // Diccionario para DuplexPageMode
            PCLXL_DiccionarioDuplexPageMode.Add(0, "eDuplexHorizontalBinding");
            PCLXL_DiccionarioDuplexPageMode.Add(1, "eDuplexVerticalBinding");
            return PCLXL_DiccionarioDuplexPageMode;
        } //LoadDuplexPageMode()


        /** \brief Dado el código numérico de impresión color PCLXL, se obtiene código Impresión Color Windows. */
        public static Dictionary<int, int> COLOR_PRINTING = LoadColorPrinting();
        /** \brief Carga el diccionario COLOR_PRINTING */
        private static Dictionary<int, int> LoadColorPrinting()
        {
            Dictionary<int, int> PCLXL_DiccionarioColorPrinting = new Dictionary<int, int>();
            // Diccionario para ColorPrinting
            PCLXL_DiccionarioColorPrinting.Add(0, 1);
            PCLXL_DiccionarioColorPrinting.Add(1, 1);
            PCLXL_DiccionarioColorPrinting.Add(2, 2);
            PCLXL_DiccionarioColorPrinting.Add(6, 2);
            PCLXL_DiccionarioColorPrinting.Add(7, 1);
            
            return PCLXL_DiccionarioColorPrinting;
        } //LoadColorPrinting()


        /** \brief Dado el código numérico de impresión doble cara PCLXL, se obtiene código Impresión doble cara Windows. */
        public static Dictionary<int, int> DUPLEX_PRINTING = LoadDuplexPrinting();
        /** \brief Carga el diccionario DUPLEX_PRINTING */
        private static Dictionary<int, int> LoadDuplexPrinting()
        {
            Dictionary<int, int> PCLXL_DiccionarioDuplexPrinting = new Dictionary<int, int>();
            // Diccionario para DuplexPrinting
            PCLXL_DiccionarioDuplexPrinting.Add(0, 3);
            PCLXL_DiccionarioDuplexPrinting.Add(1, 2);

            return PCLXL_DiccionarioDuplexPrinting;
        } //LoadDuplexPrinting()


        /** \brief Dado el código numérico de orientación papel PCLXL, se obtiene código orientación papel Windows. */
        public static Dictionary<int, int> ORIENTATION_PRINTING = LoadOrientationPrinting();
        /** \brief Carga el diccionario ORIENTATION_PRINTING */
        private static Dictionary<int, int> LoadOrientationPrinting()
        {
            Dictionary<int, int> PCLXL_DiccionarioOrientationPrinting = new Dictionary<int, int>();
            // Diccionario para OrientationPrinting
            PCLXL_DiccionarioOrientationPrinting.Add(0, 1);
            PCLXL_DiccionarioOrientationPrinting.Add(1, 2);
            PCLXL_DiccionarioOrientationPrinting.Add(2, 1);
            PCLXL_DiccionarioOrientationPrinting.Add(3, 2);
            PCLXL_DiccionarioOrientationPrinting.Add(4, 1);
            return PCLXL_DiccionarioOrientationPrinting;
        } //LoadOrientationPrinting()

        /** \brief Dado el código numérico del tamaño de papel PCLXL, se obtiene código del tamaño de papel Windows. */
        public static Dictionary<int, int> MEDIA_SIZE_PRINTING = LoadMediaSizePrinting();
        /** \brief Carga el diccionario MEDIA_SIZE_PRINTING */
        private static Dictionary<int, int> LoadMediaSizePrinting()
        {
            Dictionary<int, int> PCLXL_DiccionarioMediaSizePrinting = new Dictionary<int, int>();
            // Diccionario para MediaSizePrinting
            PCLXL_DiccionarioMediaSizePrinting.Add(5, 8);
            PCLXL_DiccionarioMediaSizePrinting.Add(2, 9);
            PCLXL_DiccionarioMediaSizePrinting.Add(16, 11);
            PCLXL_DiccionarioMediaSizePrinting.Add(17, 70);
            PCLXL_DiccionarioMediaSizePrinting.Add(12, 34);
            PCLXL_DiccionarioMediaSizePrinting.Add(13, 13);
            PCLXL_DiccionarioMediaSizePrinting.Add(8, 28);
            PCLXL_DiccionarioMediaSizePrinting.Add(6, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(96, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(9, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(3, 7);
            PCLXL_DiccionarioMediaSizePrinting.Add(10, 79);
            PCLXL_DiccionarioMediaSizePrinting.Add(11, 80);
            PCLXL_DiccionarioMediaSizePrinting.Add(18, 89);
            PCLXL_DiccionarioMediaSizePrinting.Add(15, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(20, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(19, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(21, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(14, 256);
            PCLXL_DiccionarioMediaSizePrinting.Add(4, 4);
            PCLXL_DiccionarioMediaSizePrinting.Add(1, 5);
            PCLXL_DiccionarioMediaSizePrinting.Add(0, 1);
            PCLXL_DiccionarioMediaSizePrinting.Add(7, 256);
            return PCLXL_DiccionarioMediaSizePrinting;
        } //LoadMediaSizePrinting()
    } // Fin class PCLXLDictionary

    /** \brief Definición Estructuras para analisis PCLXL */
    public class STRUCT_PCLXL 
    
    {
        /** \brief Definición  estructura para guardar propiedades del analisis de una página PCLXL */
        public struct PCLXLPageState
        {
            private string ColorSpace; // eRGB|eGray
            private bool Color; // true|False 

            
            /** \brief Constructor Inicialización de propiedades
             * \param IColorSpace eRGB|eGray
             * \param IColor true|false (elementos de la página en color)
             */
            public PCLXLPageState(string IColorSpace, bool IColor)
            {
                ColorSpace = IColorSpace;
                Color = IColor;
            }
            
            /** \brief Guardamos el espacio de color
             * \param IColorSpace eRGB: Color|eGray: Escala de grises
             */
            public void SetColorSpace(string IColorSpace)
            {
                try
                {
                    if ((IColorSpace == "eRGB") || (IColorSpace == "eGray"))
                    {
                        this.ColorSpace = IColorSpace;
                    }
                    else throw new Exception("Espacio de Color: " + IColorSpace + " no definido.");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /** \brief Comprobamos si hay impresión a color a través del parámetro de entrada
             * \param IColor eRGB: Color|eGray: Escala de grises
            */
            public void SetColorPage(string IColor)
            {
                const string _NEGRO = "(0, 0, 0)"; 
                const string _BLANCO = "(255, 255, 255)";

                try
                {
                    // Si el color no es negro o blanco es un color RGB
                    if (!((IColor == _NEGRO) || (IColor == _BLANCO)))
                    {
                        this.Color = true;
                    }
                    //else throw new Exception("Espacio de Color: " + IColorSpace + " no definido.");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /** \brief Se actualiza información si la página es en Color o no
             * \param IColor true|false
            */
            public void SetColorPage(bool IColor)
            {
                try
                {
                    this.Color = IColor;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /** \brief Para resetear propiedad de color (Se debe utilizar cuando se detecta una nueva página=
             */
            public void ResetColorPage()
            {
                this.Color = false;
            }

            /** \brief Comprobamos si es un espacio de color RGB
             * \returns bool true Paleta de color RGB
             */
            public bool IsRGBSpace()
            {
                if (this.ColorSpace == "eRGB") return true;
                else return false;
            }

            /** \brief Con este método se puede comprobar si en la página actual hay elementos de impresión en Color
             * \returns bool true página con impresión en Color
             */
            public bool IsColorPage()
            {
                if (this.Color) return true;
                else return false;
            }
        }
    }

    public class ENUM_API_PRINTING
    {
        /** \brief Atributos PRINTER_INFO
         */
        public enum ATTRIBUTE_PRINTER_INFO
        {
            PRINTER_ATTRIBUTE_RECONFIGURE = 0X00000000, // Comando para reconfigurar Atributos
            PRINTER_ATTRIBUTE_QUEUED = 0x00000001, // 1 -> Bit en la posición 1
            PRINTER_ATTRIBUTE_DIRECT = 0x00000002, // 10 -> Bit en la posición 2
            PRINTER_ATTRIBUTE_DEFAULT = 0x00000004, // 100 -> Bit en la posición 3
            PRINTER_ATTRIBUTE_SHARED = 0x00000008, // 1000 -> Bit en la posición 4
            PRINTER_ATTRIBUTE_NETWORK = 0x00000010, // 10000 -> Bit en la posición 5
            PRINTER_ATTRIBUTE_HIDDEN = 0x00000020, // 100000 -> Bit en la posición 6
            PRINTER_ATTRIBUTE_LOCAL = 0x00000040, // 1000000 -> Bit en la posición 7
            PRINTER_ATTRIBUTE_ENABLE_DEVQ = 0x00000080, // 10000000 -> Bit en la posición 8
            PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS = 0x00000100, // 100000000 -> Bit en la posición 9
            PRINTER_ATTRIBUTE_DO_COMPLETE_FIRST = 0x00000200, // 1000000000 -> Bit en la posición 10
            PRINTER_ATTRIBUTE_WORK_OFFLINE = 0x00000400, // 10000000000 -> Bit en la posición 11
            PRINTER_ATTRIBUTE_ENABLE_BIDI = 0x00000800, // 100000000000 -> Bit en la posición 12
            PRINTER_ATTRIBUTE_RAW_ONLY = 0x00001000, // 1000000000000 -> Bit en la posición 13
            PRINTER_ATTRIBUTE_PUBLISHED = 0x00002000, // 10000000000000 -> Bit en la posición 14
            PRINTER_ATTRIBUTE_FAX = 0x00004000,     // 100000000000000 -> Bit en la posición 15
            PRINTER_ATTRIBUTE_TS = 0x00008000,     // 1000000000000000 -> Bit en la posición 16
    }


        /** \brief Tamaños de papel
         * \details Obtenida de http://referencesource.microsoft.com/#System.Drawing/commonui/System/Drawing/Advanced/Gdiplus.cs,57dad1d6ec519608,references */
        public enum PAPER_SIZE 
        {
            DMPAPER_LETTER = 1,
            DMPAPER_LETTERSMALL = 2,
            DMPAPER_TABLOID = 3,
            DMPAPER_LEDGER = 4,
            DMPAPER_LEGAL = 5,
            DMPAPER_STATEMENT = 6,
            DMPAPER_EXECUTIVE = 7,
            DMPAPER_A3 = 8,
            DMPAPER_A4 = 9,
            DMPAPER_A4SMALL = 10,
            DMPAPER_A5 = 11,
            DMPAPER_B4 = 12,
            DMPAPER_B5 = 13,
            DMPAPER_FOLIO = 14,
            DMPAPER_QUARTO = 15,
            DMPAPER_10X14 = 16,
            DMPAPER_11X17 = 17,
            DMPAPER_NOTE = 18,
            DMPAPER_ENV_9 = 19,
            DMPAPER_ENV_10 = 20,
            DMPAPER_ENV_11 = 21,
            DMPAPER_ENV_12 = 22,
            DMPAPER_ENV_14 = 23,
            DMPAPER_CSHEET = 24,
            DMPAPER_DSHEET = 25,
            DMPAPER_ESHEET = 26,
            DMPAPER_ENV_DL = 27,
            DMPAPER_ENV_C5 = 28,
            DMPAPER_ENV_C3 = 29,
            DMPAPER_ENV_C4 = 30,
            DMPAPER_ENV_C6 = 31,
            DMPAPER_ENV_C65 = 32,
            DMPAPER_ENV_B4 = 33,
            DMPAPER_ENV_B5 = 34,
            DMPAPER_ENV_B6 = 35,
            DMPAPER_ENV_ITALY = 36,
            DMPAPER_ENV_MONARCH = 37,
            DMPAPER_ENV_PERSONAL = 38,
            DMPAPER_FANFOLD_US = 39,
            DMPAPER_FANFOLD_STD_GERMAN = 40,
            DMPAPER_FANFOLD_LGL_GERMAN = 41,
            DMPAPER_ISO_B4 = 42,
            DMPAPER_JAPANESE_POSTCARD = 43,
            DMPAPER_9X11 = 44,
            DMPAPER_10X11 = 45,
            DMPAPER_15X11 = 46,
            DMPAPER_ENV_INVITE = 47,
            DMPAPER_RESERVED_48 = 48,
            DMPAPER_RESERVED_49 = 49,
            DMPAPER_LETTER_EXTRA = 50,
            DMPAPER_LEGAL_EXTRA = 51,
            DMPAPER_TABLOID_EXTRA = 52,
            DMPAPER_A4_EXTRA = 53,
            DMPAPER_LETTER_TRANSVERSE = 54,
            DMPAPER_A4_TRANSVERSE = 55,
            DMPAPER_LETTER_EXTRA_TRANSVERSE = 56,
            DMPAPER_A_PLUS = 57,
            DMPAPER_B_PLUS = 58,
            DMPAPER_LETTER_PLUS = 59,
            DMPAPER_A4_PLUS = 60,
            DMPAPER_A5_TRANSVERSE = 61,
            DMPAPER_B5_TRANSVERSE = 62,
            DMPAPER_A3_EXTRA = 63,
            DMPAPER_A5_EXTRA = 64,
            DMPAPER_B5_EXTRA = 65,
            DMPAPER_A2 = 66,
            DMPAPER_A3_TRANSVERSE = 67,
            DMPAPER_A3_EXTRA_TRANSVERSE = 68,
            DMPAPER_DBL_JAPANESE_POSTCARD = 69, /* Japanese Double Postcard 200 x 148 mm */
            DMPAPER_A6 = 70,  /* A6 105 x 148 mm                 */
            DMPAPER_JENV_KAKU2 = 71,  /* Japanese Envelope Kaku #2       */
            DMPAPER_JENV_KAKU3 = 72,  /* Japanese Envelope Kaku #3       */
            DMPAPER_JENV_CHOU3 = 73,  /* Japanese Envelope Chou #3       */
            DMPAPER_JENV_CHOU4 = 74,  /* Japanese Envelope Chou #4       */
            DMPAPER_LETTER_ROTATED = 75,  /* Letter Rotated 11 x 8 1/2 11 in */
            DMPAPER_A3_ROTATED = 76,  /* A3 Rotated 420 x 297 mm         */
            DMPAPER_A4_ROTATED = 77,  /* A4 Rotated 297 x 210 mm         */
            DMPAPER_A5_ROTATED = 78,  /* A5 Rotated 210 x 148 mm         */
            DMPAPER_B4_JIS_ROTATED = 79,  /* B4 (JIS) Rotated 364 x 257 mm   */
            DMPAPER_B5_JIS_ROTATED = 80,  /* B5 (JIS) Rotated 257 x 182 mm   */
            DMPAPER_JAPANESE_POSTCARD_ROTATED = 81, /* Japanese Postcard Rotated 148 x 100 mm */
            DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82, /* Double Japanese Postcard Rotated 148 x 200 mm */
            DMPAPER_A6_ROTATED = 83,  /* A6 Rotated 148 x 105 mm         */
            DMPAPER_JENV_KAKU2_ROTATED = 84,  /* Japanese Envelope Kaku #2 Rotated */
            DMPAPER_JENV_KAKU3_ROTATED = 85,  /* Japanese Envelope Kaku #3 Rotated */
            DMPAPER_JENV_CHOU3_ROTATED = 86,  /* Japanese Envelope Chou #3 Rotated */
            DMPAPER_JENV_CHOU4_ROTATED = 87,  /* Japanese Envelope Chou #4 Rotated */
            DMPAPER_B6_JIS = 88,  /* B6 (JIS) 128 x 182 mm           */
            DMPAPER_B6_JIS_ROTATED = 89,  /* B6 (JIS) Rotated 182 x 128 mm   */
            DMPAPER_12X11 = 90,  /* 12 x 11 in                      */
            DMPAPER_JENV_YOU4 = 91,  /* Japanese Envelope You #4        */
            DMPAPER_JENV_YOU4_ROTATED = 92,  /* Japanese Envelope You #4 Rotated*/
            DMPAPER_P16K = 93,  /* PRC 16K 146 x 215 mm            */
            DMPAPER_P32K = 94,  /* PRC 32K 97 x 151 mm             */
            DMPAPER_P32KBIG = 95,  /* PRC 32K(Big) 97 x 151 mm        */
            DMPAPER_PENV_1 = 96,  /* PRC Envelope #1 102 x 165 mm    */
            DMPAPER_PENV_2 = 97,  /* PRC Envelope #2 102 x 176 mm    */
            DMPAPER_PENV_3 = 98,  /* PRC Envelope #3 125 x 176 mm    */
            DMPAPER_PENV_4 = 99,  /* PRC Envelope #4 110 x 208 mm    */
            DMPAPER_PENV_5 = 100, /* PRC Envelope #5 110 x 220 mm    */
            DMPAPER_PENV_6 = 101, /* PRC Envelope #6 120 x 230 mm    */
            DMPAPER_PENV_7 = 102, /* PRC Envelope #7 160 x 230 mm    */
            DMPAPER_PENV_8 = 103, /* PRC Envelope #8 120 x 309 mm    */
            DMPAPER_PENV_9 = 104, /* PRC Envelope #9 229 x 324 mm    */
            DMPAPER_PENV_10 = 105, /* PRC Envelope #10 324 x 458 mm   */
            DMPAPER_P16K_ROTATED = 106, /* PRC 16K Rotated                 */
            DMPAPER_P32K_ROTATED = 107, /* PRC 32K Rotated                 */
            DMPAPER_P32KBIG_ROTATED = 108, /* PRC 32K(Big) Rotated            */
            DMPAPER_PENV_1_ROTATED = 109, /* PRC Envelope #1 Rotated 165 x 102 mm */
            DMPAPER_PENV_2_ROTATED = 110, /* PRC Envelope #2 Rotated 176 x 102 mm */
            DMPAPER_PENV_3_ROTATED = 111, /* PRC Envelope #3 Rotated 176 x 125 mm */
            DMPAPER_PENV_4_ROTATED = 112, /* PRC Envelope #4 Rotated 208 x 110 mm */
            DMPAPER_PENV_5_ROTATED = 113, /* PRC Envelope #5 Rotated 220 x 110 mm */
            DMPAPER_PENV_6_ROTATED = 114, /* PRC Envelope #6 Rotated 230 x 120 mm */
            DMPAPER_PENV_7_ROTATED = 115, /* PRC Envelope #7 Rotated 230 x 160 mm */
            DMPAPER_PENV_8_ROTATED = 116, /* PRC Envelope #8 Rotated 309 x 120 mm */
            DMPAPER_PENV_9_ROTATED = 117, /* PRC Envelope #9 Rotated 324 x 229 mm */
            DMPAPER_PENV_10_ROTATED = 118, /* PRC Envelope #10 Rotated 458 x 324 mm */
            DMPAPER_LAST = DMPAPER_PENV_10_ROTATED,
            DMPAPER_USER = 256,
        }
    }

    public class  STRUCT_API_PRINTING
    {
        /** \brief Dimensiones del medio de impresión (ancho, largo)
        */
        public struct SIZE {
            uint Width;
            uint Length;  
        }


        /** \brief Caracteristicas de impresión.
         * \details struct utilizado por el sistema de impresión de windows para guardar las
         * características de impresión de un trabajo de impresión. También utilizado en lenguaje EMF.
         */
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            public ushort SpecVersion;
            public ushort DriverVersion;
            public ushort Size; // Número de páginas
            public ushort DriverExtra; //dmDriverExtra
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183565%28v=vs.85%29.aspx
            public uint Fields; // A nivel de bit significa cuales de los siguientes campos han sido inicializados

            public short Orientation;
            public short PaperSize;
            public short PaperLength;
            public short PaperWidth;
            public short Scale;
            public short Copies;
            public short DefaultSource;
            public short PrintQuality;

            public short Color;
            public short Duplex;
            public short YResolution;
            public short TTOption;
            public short Collate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] //32 caracteres
            public string FormName;

            public ushort LogPixels; //reserved0
            public uint BitsPerPel; //reserved1
            public uint PelsWidth; //reserved2
            public uint PelsHeight; //reserved3

            public uint Nup;

            public uint DisplayFrequency; //reserved4

            public uint ICMMethod;
            public uint ICMIntent;
            public uint MediaType;
            public uint DitherType;
            public uint Reserved1; //reserved5
            public uint Reserved2; //reserved6

            public uint PanningWidth; //reserved7
            public uint PanningHeight; //reserved8
        }


        /** \brief Propiedades tipo 2 de la impresora
         * \date 22/09/2015
         * \remarks Referencia MSDN
         *  http://referencesource.microsoft.com/#ReachFramework/MS/Internal/Printing/Configuration/PRINTER_INFO_2.cs
         */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PRINTER_INFO_2
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pServerName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrinterName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pShareName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPortName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDriverName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pComment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pLocation;
            public IntPtr pDevMode;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pSepFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrintProcessor;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDatatype;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pParameters;
            public IntPtr pSecurityDescriptor;
            public uint Attributes; // See note below!
            public uint Priority;
            public uint DefaultPriority;
            public uint StartTime;
            public uint UntilTime;
            public uint Status;
            public uint cJobs;
            public uint AveragePPM;
        }

        /** \brief Propiedades tipo 3 de la impresora
         */
        public struct PRINTER_INFO_5
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrinterName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPortName;
            public uint Attributes; // See note below!
	        public uint device_not_selected_timeout;
	        public uint transmission_retry_timeout;
        }

        /** \brief Información ampliada propiedades trabajo de impresión.
         * \details struct utilizado por el sistema de impresión de windows para guardar las
         * propiedades de un trabajo de impresión. También utilizado en lenguaje EMF.
         * \remarks Documentación de referencia: https://msdn.microsoft.com/en-us/library/cc244678.aspx
         */
        public struct JOB_INFO_2
        {
            public uint JobId;
            public string pPrinterName;
            public string pMachineName;
            public string pUserName;
            public string pDocument;
            public string pNotifyName;
            public string pDatatype;
            public string pPrintProcessor;
            public string pParameters;
            public string pDriverName;
            public IntPtr pDevMode;
            public string pStatus; // Doc referencia no se corresponde ??
            //public PSECURITY_DESCRIPTOR pSecurityDescriptor;
            public uint Status;
            public uint Priority;
            public uint Position;
            public uint StartTime;
            public uint UntilTime;
            public uint TotalPages;
            public uint Size;
            //public ulong Submitted;
            public SYSTEMTIME Submitted;
            public uint Time;
            public uint PagesPrinted;

            public STRUCT_API_PRINTING.DEVMODE? DevMode
            {
                get
                {
                    if (pDevMode != IntPtr.Zero)
                    {
                        Console.WriteLine("struct JOB_INFO_2. Existe estructura DEVMODE asociada");
                        Console.ReadKey();
                        return (STRUCT_API_PRINTING.DEVMODE)Marshal.PtrToStructure(pDevMode, typeof(STRUCT_API_PRINTING.DEVMODE));
                    }
                    else
                    {
                        Console.WriteLine("struct JOB_INFO_2. NO Existe estructura DEVMODE asociada");
                        Console.ReadKey();
                        return null;
                    }
                }
                set
                {
                    if (pDevMode != null)
                    {
                        Marshal.DestroyStructure(pDevMode, typeof(STRUCT_API_PRINTING.DEVMODE));
                        Marshal.FreeHGlobal(pDevMode);
                        pDevMode = IntPtr.Zero;
                    }

                    if (value != null)
                    {
                        pDevMode = Marshal.AllocHGlobal(Marshal.SizeOf(value));
                        Marshal.StructureToPtr(value, pDevMode, false);
                    }
                }
            }
        }  // Fin JOB_INFO_2

        /** \brief Info mínima trabajo de impresión.
         * \details struct utilizado por el sistema de impresión de windows para guardar las
         * propiedades de un trabajo de impresión. También utilizado en lenguaje EMF.
         */
        public struct JOB_INFO_1
        {
            public uint JobId;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public string pPrinterName;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public string pMachineName;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public string pUserName;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public string pDocument;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public string pDatatype;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public string pStatus;
            public uint Status;
            public uint Priority;
            public uint Position;
            public uint TotalPages;
            public uint PagesPrinted;
            public SYSTEMTIME Submitted;
        } // Fin JOB_INFO_!

        /** \brief Fecha completa.
         * \details Desde el año hasta milisegundos.
         */
        public struct SYSTEMTIME
        {
            public Int16 Year;
            public Int16 Month;
            public Int16 DayOfWeek;
            public Int16 Day;
            public Int16 Hour;
            public Int16 Minute;
            public Int16 Second;
            public Int16 Milliseconds;
        }

        public struct PRINTER_DEFAULTS
        {
            public string pDatatype;
            public IntPtr pDevMode;
            public PRINTER_ACCESS_MASK DesiredAccess;

            public STRUCT_API_PRINTING.DEVMODE? DevMode
            {
                get
                {
                    if (pDevMode != IntPtr.Zero)
                    {
                        return (STRUCT_API_PRINTING.DEVMODE)Marshal.PtrToStructure(pDevMode, typeof(STRUCT_API_PRINTING.DEVMODE));
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    if (pDevMode != null)
                    {
                        Marshal.DestroyStructure(pDevMode, typeof(STRUCT_API_PRINTING.DEVMODE));
                        Marshal.FreeHGlobal(pDevMode);
                        pDevMode = IntPtr.Zero;
                    }

                    if (value != null)
                    {
                        pDevMode = Marshal.AllocHGlobal(Marshal.SizeOf(value));
                        Marshal.StructureToPtr(value, pDevMode, false);
                    }
                }
            }
        }

        public enum PRINTER_ACCESS_MASK
        {
            PRINTER_ACCESS_ADMINISTER = 4,
            PRINTER_ACCESS_USE = 8,
            PRINTER_ALL_ACCESS = 0x000F000C
        }

        [FlagsAttribute]
        public enum PRINTER_ENUM // PrinterEnumFlags
        {
            // ReSharper disable UnusedMember.Global
            PRINTER_ENUM_DEFAULT = 0x00000001,
            PRINTER_ENUM_LOCAL = 0x00000002,
            PRINTER_ENUM_CONNECTIONS = 0x00000004,
            PRINTER_ENUM_FAVORITE = 0x00000004,
            PRINTER_ENUM_NAME = 0x00000008,
            PRINTER_ENUM_REMOTE = 0x00000010,
            PRINTER_ENUM_SHARED = 0x00000020,
            PRINTER_ENUM_NETWORK = 0x00000040,
            PRINTER_ENUM_EXPAND = 0x00004000,
            PRINTER_ENUM_CONTAINER = 0x00008000,
            PRINTER_ENUM_ICONMASK = 0x00ff0000,
            PRINTER_ENUM_ICON1 = 0x00010000,
            PRINTER_ENUM_ICON2 = 0x00020000,
            PRINTER_ENUM_ICON3 = 0x00040000,
            PRINTER_ENUM_ICON4 = 0x00080000,
            PRINTER_ENUM_ICON5 = 0x00100000,
            PRINTER_ENUM_ICON6 = 0x00200000,
            PRINTER_ENUM_ICON7 = 0x00400000,
            PRINTER_ENUM_ICON8 = 0x00800000,
            PRINTER_ENUM_HIDE = 0x01000000,
            PRINTER_ENUM_ALL = PRINTER_ENUM_LOCAL + PRINTER_ENUM_NETWORK
            // ReSharper restore UnusedMember.Global
        }
    } // Fin STRUCT_API_PRINTING

    public class DICTIONARY_API_PRINTING
    {
        /** \brief Diccionario con tamaños de papel */
        public static Dictionary<int, string> PAPERSIZE = LoadPaperSize();
        /** \brief Carga diccionario con tamaños de papel */
        private static Dictionary<int, string> LoadPaperSize()
        {
            Dictionary<int, string> DictionaryPaperSize = new Dictionary<int, string>();
            DictionaryPaperSize.Add(1, "DMPAPER_LETTER");
            DictionaryPaperSize.Add(2, "DMPAPER_LETTERSMALL");
            DictionaryPaperSize.Add(3, "DMPAPER_TABLOID");
            DictionaryPaperSize.Add(4, "DMPAPER_LEDGER");
            DictionaryPaperSize.Add(5, "DMPAPER_LEGAL");
            DictionaryPaperSize.Add(6, "DMPAPER_STATEMENT");
            DictionaryPaperSize.Add(7, "DMPAPER_EXECUTIVE");
            DictionaryPaperSize.Add(8, "DMPAPER_A3");
            DictionaryPaperSize.Add(9, "DMPAPER_A4");
            DictionaryPaperSize.Add(10, "DMPAPER_A4SMALL");
            DictionaryPaperSize.Add(11, "DMPAPER_A5");
            DictionaryPaperSize.Add(12, "DMPAPER_B4");
            DictionaryPaperSize.Add(13, "DMPAPER_B5");
            DictionaryPaperSize.Add(14, "DMPAPER_FOLIO");
            DictionaryPaperSize.Add(15, "DMPAPER_QUARTO");
            DictionaryPaperSize.Add(16, "DMPAPER_10X14");
            DictionaryPaperSize.Add(17, "DMPAPER_11X17");
            DictionaryPaperSize.Add(18, "DMPAPER_NOTE");
            DictionaryPaperSize.Add(19, "DMPAPER_ENV_9");
            DictionaryPaperSize.Add(20, "DMPAPER_ENV_10");
            DictionaryPaperSize.Add(21, "DMPAPER_ENV_11");
            DictionaryPaperSize.Add(22, "DMPAPER_ENV_12");
            DictionaryPaperSize.Add(23, "DMPAPER_ENV_14");
            DictionaryPaperSize.Add(24, "DMPAPER_CSHEET");
            DictionaryPaperSize.Add(25, "DMPAPER_DSHEET");
            DictionaryPaperSize.Add(26, "DMPAPER_ESHEET");
            DictionaryPaperSize.Add(27, "DMPAPER_ENV_DL");
            DictionaryPaperSize.Add(28, "DMPAPER_ENV_C5");
            DictionaryPaperSize.Add(29, "DMPAPER_ENV_C3");
            DictionaryPaperSize.Add(30, "DMPAPER_ENV_C4");
            DictionaryPaperSize.Add(31, "DMPAPER_ENV_C6");
            DictionaryPaperSize.Add(32, "DMPAPER_ENV_C65");
            DictionaryPaperSize.Add(33, "DMPAPER_ENV_B4");
            DictionaryPaperSize.Add(34, "DMPAPER_ENV_B5");
            DictionaryPaperSize.Add(35, "DMPAPER_ENV_B6");
            DictionaryPaperSize.Add(36, "DMPAPER_ENV_ITALY");
            DictionaryPaperSize.Add(37, "DMPAPER_ENV_MONARCH");
            DictionaryPaperSize.Add(38, "DMPAPER_ENV_PERSONAL");
            DictionaryPaperSize.Add(39, "DMPAPER_FANFOLD_US");
            DictionaryPaperSize.Add(40, "DMPAPER_FANFOLD_STD_GERMAN");
            DictionaryPaperSize.Add(41, "DMPAPER_FANFOLD_LGL_GERMAN");
            DictionaryPaperSize.Add(42, "DMPAPER_ISO_B4");
            DictionaryPaperSize.Add(43, "DMPAPER_JAPANESE_POSTCARD");
            DictionaryPaperSize.Add(44, "DMPAPER_9X11");
            DictionaryPaperSize.Add(45, "DMPAPER_10X11");
            DictionaryPaperSize.Add(46, "DMPAPER_15X11");
            DictionaryPaperSize.Add(47, "DMPAPER_ENV_INVITE");
            DictionaryPaperSize.Add(48, "DMPAPER_RESERVED_48");
            DictionaryPaperSize.Add(49, "DMPAPER_RESERVED_49");
            DictionaryPaperSize.Add(50, "DMPAPER_LETTER_EXTRA");
            DictionaryPaperSize.Add(51, "DMPAPER_LEGAL_EXTRA");
            DictionaryPaperSize.Add(52, "DMPAPER_TABLOID_EXTRA");
            DictionaryPaperSize.Add(53, "DMPAPER_A4_EXTRA");
            DictionaryPaperSize.Add(54, "DMPAPER_LETTER_TRANSVERSE");
            DictionaryPaperSize.Add(55, "DMPAPER_A4_TRANSVERSE");
            DictionaryPaperSize.Add(56, "DMPAPER_LETTER_EXTRA_TRANSVERSE");
            DictionaryPaperSize.Add(57, "DMPAPER_A_PLUS");
            DictionaryPaperSize.Add(58, "DMPAPER_B_PLUS");
            DictionaryPaperSize.Add(59, "DMPAPER_LETTER_PLUS");
            DictionaryPaperSize.Add(60, "DMPAPER_A4_PLUS");
            DictionaryPaperSize.Add(61, "DMPAPER_A5_TRANSVERSE");
            DictionaryPaperSize.Add(62, "DMPAPER_B5_TRANSVERSE");
            DictionaryPaperSize.Add(63, "DMPAPER_A3_EXTRA");
            DictionaryPaperSize.Add(64, "DMPAPER_A5_EXTRA");
            DictionaryPaperSize.Add(65, "DMPAPER_B5_EXTRA");
            DictionaryPaperSize.Add(66, "DMPAPER_A2");
            DictionaryPaperSize.Add(67, "DMPAPER_A3_TRANSVERSE");
            DictionaryPaperSize.Add(68, "DMPAPER_A3_EXTRA_TRANSVERSE");
            DictionaryPaperSize.Add(69, "DMPAPER_DBL_JAPANESE_POSTCARD");
            DictionaryPaperSize.Add(70, "DMPAPER_A6");
            DictionaryPaperSize.Add(71, "DMPAPER_JENV_KAKU2");
            DictionaryPaperSize.Add(72, "DMPAPER_JENV_KAKU3");
            DictionaryPaperSize.Add(73, "DMPAPER_JENV_CHOU3");
            DictionaryPaperSize.Add(74, "DMPAPER_JENV_CHOU4");
            DictionaryPaperSize.Add(75, "DMPAPER_LETTER_ROTATED");
            DictionaryPaperSize.Add(76, "DMPAPER_A3_ROTATED");
            DictionaryPaperSize.Add(77, "DMPAPER_A4_ROTATED");
            DictionaryPaperSize.Add(78, "DMPAPER_A5_ROTATED");
            DictionaryPaperSize.Add(79, "DMPAPER_B4_JIS_ROTATED");
            DictionaryPaperSize.Add(80, "DMPAPER_B5_JIS_ROTATED");
            DictionaryPaperSize.Add(81, "DMPAPER_JAPANESE_POSTCARD_ROTATED");
            DictionaryPaperSize.Add(82, "DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED");
            DictionaryPaperSize.Add(83, "DMPAPER_A6_ROTATED");
            DictionaryPaperSize.Add(84, "DMPAPER_JENV_KAKU2_ROTATED");
            DictionaryPaperSize.Add(85, "DMPAPER_JENV_KAKU3_ROTATED");
            DictionaryPaperSize.Add(86, "DMPAPER_JENV_CHOU3_ROTATED");
            DictionaryPaperSize.Add(87, "DMPAPER_JENV_CHOU4_ROTATED");
            DictionaryPaperSize.Add(88, "DMPAPER_B6_JIS");
            DictionaryPaperSize.Add(89, "DMPAPER_B6_JIS_ROTATED");
            DictionaryPaperSize.Add(90, "DMPAPER_12X11");
            DictionaryPaperSize.Add(91, "DMPAPER_JENV_YOU4");
            DictionaryPaperSize.Add(92, "DMPAPER_JENV_YOU4_ROTATED");
            DictionaryPaperSize.Add(93, "DMPAPER_P16K");
            DictionaryPaperSize.Add(94, "DMPAPER_P32K");
            DictionaryPaperSize.Add(95, "DMPAPER_P32KBIG");
            DictionaryPaperSize.Add(96, "DMPAPER_PENV_1");
            DictionaryPaperSize.Add(97, "DMPAPER_PENV_2");
            DictionaryPaperSize.Add(98, "DMPAPER_PENV_3");
            DictionaryPaperSize.Add(99, "DMPAPER_PENV_4");
            DictionaryPaperSize.Add(100, "DMPAPER_PENV_5");
            DictionaryPaperSize.Add(101, "DMPAPER_PENV_6");
            DictionaryPaperSize.Add(102, "DMPAPER_PENV_7");
            DictionaryPaperSize.Add(103, "DMPAPER_PENV_8");
            DictionaryPaperSize.Add(104, "DMPAPER_PENV_9");
            DictionaryPaperSize.Add(105, "DMPAPER_PENV_10");
            DictionaryPaperSize.Add(106, "DMPAPER_P16K_ROTATED");
            DictionaryPaperSize.Add(107, "DMPAPER_P32K_ROTATED");
            DictionaryPaperSize.Add(108, "DMPAPER_P32KBIG_ROTATED");
            DictionaryPaperSize.Add(109, "DMPAPER_PENV_1_ROTATED");
            DictionaryPaperSize.Add(110, "DMPAPER_PENV_2_ROTATED");
            DictionaryPaperSize.Add(111, "DMPAPER_PENV_3_ROTATED");
            DictionaryPaperSize.Add(112, "DMPAPER_PENV_4_ROTATED");
            DictionaryPaperSize.Add(113, "DMPAPER_PENV_5_ROTATED");
            DictionaryPaperSize.Add(114, "DMPAPER_PENV_6_ROTATED");
            DictionaryPaperSize.Add(115, "DMPAPER_PENV_7_ROTATED");
            DictionaryPaperSize.Add(116, "DMPAPER_PENV_8_ROTATED");
            DictionaryPaperSize.Add(117, "DMPAPER_PENV_9_ROTATED");
            DictionaryPaperSize.Add(118, "DMPAPER_PENV_10_ROTATED");
            //DictionaryPaperSize.Add(DMPAPER_PENV_10_ROTATED, "DMPAPER_LAST");
            DictionaryPaperSize.Add(256, "DMPAPER_USER");
            return DictionaryPaperSize;
        } //LoadPaperSize()

        /** \brief Dado el código numérico de ColorSpace se obtiene su descripción */
        public static Dictionary<int, string> COLORSPACE = LoadColorSpace();
        /** \brief Carga el diccionario COLORSPACE */
        private static Dictionary<int, string> LoadColorSpace()
        {
            Dictionary<int, string> DiccionarioColorSpace = new Dictionary<int, string>();
            // Diccionario para ColorSpace
            DiccionarioColorSpace.Add(0x0001, "MonoChrome");
            DiccionarioColorSpace.Add(0x0002, "Color");
            return DiccionarioColorSpace;
        } //LoadColorSpace()

        /** \brief Dado el código numérico de DuplexPageMode se obtiene su descripción */
        public static Dictionary<int, string> DUPLEX_PAGEMODE = LoadDuplexPageMode();
        /** \brief Carga el diccionario DUPLEX_PAGEMODE */
        private static Dictionary<int, string> LoadDuplexPageMode()
        {
            Dictionary<int, string> DiccionarioDuplexPageMode = new Dictionary<int, string>();
            // Diccionario para DuplexPageMode
            DiccionarioDuplexPageMode.Add(0x0001, "Simplex");
            DiccionarioDuplexPageMode.Add(0x0002, "Vertical");
            DiccionarioDuplexPageMode.Add(0x0003, "Horizontal");
            return DiccionarioDuplexPageMode;
        } //LoadDuplexPageMode()

        /** \brief Dado el código numérico de la Orientación del papel se obtiene su descripción */
        public static Dictionary<int, string> ORIENTATION = LoadOrientation();
        /** \brief Carga el diccionario ORIENTATION */
        private static Dictionary<int, string> LoadOrientation()
        {
            Dictionary<int, string> DiccionarioOrientation = new Dictionary<int, string>();
            // Diccionario para Orientation
            DiccionarioOrientation.Add(0x0001, "Portrait");
            DiccionarioOrientation.Add(0x0002, "Landscape");
            return DiccionarioOrientation;
        } //LoadOrientation()
    }

    /** \brief Definición enumeraciones para análisis EMF
     */
    public class ENUM_EMF
    {
        public enum EMRI_RECORD
        {
            HEADER_RECORD = 0x00010000,
            EMRI_EOF = 0x00000000,
            EMRI_METAFILE = 0x00000001,
            EMRI_ENGINE_FONT = 0x00000002,
            EMRI_DEVMODE = 0x00000003,
            EMRI_TYPE1_FONT = 0x00000004,
            EMRI_PRESTARTPAGE = 0x00000005,
            EMRI_DESIGNVECTOR = 0x00000006,
            EMRI_SUBSET_FONT = 0x00000007,
            EMRI_DELTA_FONT = 0x00000008,
            EMRI_FORM_METAFILE = 0x00000009,
            EMRI_BW_METAFILE = 0x0000000A,
            EMRI_BW_FORM_METAFILE = 0x0000000B,
            EMRI_METAFILE_DATA = 0x0000000C,
            EMRI_METAFILE_EXT = 0x0000000D,
            EMRI_BW_METAFILE_EXT = 0x0000000E,
            EMRI_ENGINE_FONT_EXT = 0x0000000F,
            EMRI_TYPE1_FONT_EXT = 0x00000010,
            EMRI_DESIGNVECTOR_EXT = 0x00000011,
            EMRI_SUBSET_FONT_EXT = 0x00000012,
            EMRI_DELTA_FONT_EXT = 0x00000013,
            EMRI_PS_JOB_DATA = 0x00000014,
            EMRI_EMBED_FONT_EXT = 0x00000015
        };

        public enum EMR_RECORD
        {
            EMR_HEADER = 0x00000001,
            EMR_POLYBEZIER = 0x00000002,
            EMR_POLYGON = 0x00000003,
            EMR_POLYLINE = 0x00000004,
            EMR_POLYBEZIERTO = 0x00000005,
            EMR_POLYLINETO = 0x00000006,
            EMR_POLYPOLYLINE = 0x00000007,
            EMR_POLYPOLYGON = 0x00000008,
            EMR_SETWINDOWEXTEX = 0x00000009,
            EMR_SETWINDOWORGEX = 0x0000000A,
            EMR_SETVIEWPORTEXTEX = 0x0000000B,
            EMR_SETVIEWPORTORGEX = 0x0000000C,
            EMR_SETBRUSHORGEX = 0x0000000D,
            EMR_EOF = 0x0000000E,
            EMR_SETPIXELV = 0x0000000F,
            EMR_SETMAPPERFLAGS = 0x00000010,
            EMR_SETMAPMODE = 0x00000011,
            EMR_SETBKMODE = 0x00000012,
            EMR_SETPOLYFILLMODE = 0x00000013,
            EMR_SETROP2 = 0x00000014,
            EMR_SETSTRETCHBLTMODE = 0x00000015,
            EMR_SETTEXTALIGN = 0x00000016,
            EMR_SETCOLORADJUSTMENT = 0x00000017,
            EMR_SETTEXTCOLOR = 0x00000018,
            EMR_SETBKCOLOR = 0x00000019,
            EMR_OFFSETCLIPRGN = 0x0000001A,
            EMR_MOVETOEX = 0x0000001B,
            EMR_SETMETARGN = 0x0000001C,
            EMR_EXCLUDECLIPRECT = 0x0000001D,
            EMR_INTERSECTCLIPRECT = 0x0000001E,
            EMR_SCALEVIEWPORTEXTEX = 0x0000001F,
            EMR_SCALEWINDOWEXTEX = 0x00000020,
            EMR_SAVEDC = 0x00000021,
            EMR_RESTOREDC = 0x00000022,
            EMR_SETWORLDTRANSFORM = 0x00000023,
            EMR_MODIFYWORLDTRANSFORM = 0x00000024,
            EMR_SELECTOBJECT = 0x00000025,
            EMR_CREATEPEN = 0x00000026,
            EMR_CREATEBRUSHINDIRECT = 0x00000027,
            EMR_DELETEOBJECT = 0x00000028,
            EMR_ANGLEARC = 0x00000029,
            EMR_ELLIPSE = 0x0000002A,
            EMR_RECTANGLE = 0x0000002B,
            EMR_ROUNDRECT = 0x0000002C,
            EMR_ARC = 0x0000002D,
            EMR_CHORD = 0x0000002E,
            EMR_PIE = 0x0000002F,
            EMR_SELECTPALETTE = 0x00000030,
            EMR_CREATEPALETTE = 0x00000031,
            EMR_SETPALETTEENTRIES = 0x00000032,
            EMR_RESIZEPALETTE = 0x00000033,
            EMR_REALIZEPALETTE = 0x00000034,
            EMR_EXTFLOODFILL = 0x00000035,
            EMR_LINETO = 0x00000036,
            EMR_ARCTO = 0x00000037,
            EMR_POLYDRAW = 0x00000038,
            EMR_SETARCDIRECTION = 0x00000039,
            EMR_SETMITERLIMIT = 0x0000003A,
            EMR_BEGINPATH = 0x0000003B,
            EMR_ENDPATH = 0x0000003C,
            EMR_CLOSEFIGURE = 0x0000003D,
            EMR_FILLPATH = 0x0000003E,
            EMR_STROKEANDFILLPATH = 0x0000003F,
            EMR_STROKEPATH = 0x00000040,
            EMR_FLATTENPATH = 0x00000041,
            EMR_WIDENPATH = 0x00000042,
            EMR_SELECTCLIPPATH = 0x00000043,
            EMR_ABORTPATH = 0x00000044,
            EMR_COMMENT = 0x00000046,
            EMR_FILLRGN = 0x00000047,
            EMR_FRAMERGN = 0x00000048,
            EMR_INVERTRGN = 0x00000049,
            EMR_PAINTRGN = 0x0000004A,
            EMR_EXTSELECTCLIPRGN = 0x0000004B,
            EMR_BITBLT = 0x0000004C,
            EMR_STRETCHBLT = 0x0000004D,
            EMR_MASKBLT = 0x0000004E,
            EMR_PLGBLT = 0x0000004F,
            EMR_SETDIBITSTODEVICE = 0x00000050,
            EMR_STRETCHDIBITS = 0x00000051,
            EMR_EXTCREATEFONTINDIRECTW = 0x00000052,
            EMR_EXTTEXTOUTA = 0x00000053,
            EMR_EXTTEXTOUTW = 0x00000054,
            EMR_POLYBEZIER16 = 0x00000055,
            EMR_POLYGON16 = 0x00000056,
            EMR_POLYLINE16 = 0x00000057,
            EMR_POLYBEZIERTO16 = 0x00000058,
            EMR_POLYLINETO16 = 0x00000059,
            EMR_POLYPOLYLINE16 = 0x0000005A,
            EMR_POLYPOLYGON16 = 0x0000005B,
            EMR_POLYDRAW16 = 0x0000005C,
            EMR_CREATEMONOBRUSH = 0x0000005D,
            EMR_CREATEDIBPATTERNBRUSHPT = 0x0000005E,
            EMR_EXTCREATEPEN = 0x0000005F,
            EMR_POLYTEXTOUTA = 0x00000060,
            EMR_POLYTEXTOUTW = 0x00000061,
            EMR_SETICMMODE = 0x00000062,
            EMR_CREATECOLORSPACE = 0x00000063,
            EMR_SETCOLORSPACE = 0x00000064,
            EMR_DELETECOLORSPACE = 0x00000065,
            EMR_GLSRECORD = 0x00000066,
            EMR_GLSBOUNDEDRECORD = 0x00000067,
            EMR_PIXELFORMAT = 0x00000068,
            EMR_DRAWESCAPE = 0x00000069,
            EMR_EXTESCAPE = 0x0000006A,
            EMR_SMALLTEXTOUT = 0x0000006C,
            EMR_FORCEUFIMAPPING = 0x0000006D,
            EMR_NAMEDESCAPE = 0x0000006E,
            EMR_COLORCORRECTPALETTE = 0x0000006F,
            EMR_SETICMPROFILEA = 0x00000070,
            EMR_SETICMPROFILEW = 0x00000071,
            EMR_ALPHABLEND = 0x00000072,
            EMR_SETLAYOUT = 0x00000073,
            EMR_TRANSPARENTBLT = 0x00000074,
            EMR_GRADIENTFILL = 0x00000076,
            EMR_SETLINKEDUFIS = 0x00000077,
            EMR_SETTEXTJUSTIFICATION = 0x00000078,
            EMR_COLORMATCHTOTARGETW = 0x00000079,
            EMR_CREATECOLORSPACEW = 0x0000007A
        }; 
    } //ENUM_EMF

    /** \brief Definición diccionarios para análisis EMF
     */
    public class DICTIONARY_EMF
    {
        /** \brief Para un registro EMRI devuelve Descripción */
        public static Dictionary<int, string> EMRI_RECORD = LoadEmriRecord();
        /** \brief Carga el diccionario EMRI_RECORD */
        private static Dictionary<int, string> LoadEmriRecord()
        {
            Dictionary<int, string> EmriRecord = new Dictionary<int, string>();
            EmriRecord.Add(0x00010000, "HEADER_RECORD");
            EmriRecord.Add(0x00000000, "EMRI_EOF");
            EmriRecord.Add(0x00000001, "EMRI_METAFILE");
            EmriRecord.Add(0x00000002, "EMRI_ENGINE_FONT");
            EmriRecord.Add(0x00000003, "EMRI_DEVMODE");
            EmriRecord.Add(0x00000004, "EMRI_TYPE1_FONT");
            EmriRecord.Add(0x00000005, "EMRI_PRESTARTPAGE");
            EmriRecord.Add(0x00000006, "EMRI_DESIGNVECTOR");
            EmriRecord.Add(0x00000007, "EMRI_SUBSET_FONT");
            EmriRecord.Add(0x00000008, "EMRI_DELTA_FONT");
            EmriRecord.Add(0x00000009, "EMRI_FORM_METAFILE");
            EmriRecord.Add(0x0000000A, "EMRI_BW_METAFILE");
            EmriRecord.Add(0x0000000B, "EMRI_BW_FORM_METAFILE");
            EmriRecord.Add(0x0000000C, "EMRI_METAFILE_DATA");
            EmriRecord.Add(0x0000000D, "EMRI_METAFILE_EXT");
            EmriRecord.Add(0x0000000E, "EMRI_BW_METAFILE_EXT");
            EmriRecord.Add(0x0000000F, "EMRI_ENGINE_FONT_EXT");
            EmriRecord.Add(0x00000010, "EMRI_TYPE1_FONT_EXT");
            EmriRecord.Add(0x00000011, "EMRI_DESIGNVECTOR_EXT");
            EmriRecord.Add(0x00000012, "EMRI_SUBSET_FONT_EXT");
            EmriRecord.Add(0x00000013, "EMRI_DELTA_FONT_EXT");
            EmriRecord.Add(0x00000014, "EMRI_PS_JOB_DATA");
            EmriRecord.Add(0x00000015, "EMRI_EMBED_FONT_EXT");            
            return EmriRecord;
        } //LoadEmriRecord()

        /** \brief Para un registro EMR devuelve Descripción */
        public static Dictionary<int, string> EMR_RECORD = LoadEmrRecord();
        /** \brief Carga el diccionario EMR_RECORD */
        private static Dictionary<int, string> LoadEmrRecord()
        {
            Dictionary<int, string> EmrRecord = new Dictionary<int, string>();
            EmrRecord.Add(0x00000001, "EMR_HEADER");
            EmrRecord.Add(0x00000002, "EMR_POLYBEZIER");
            EmrRecord.Add(0x00000003, "EMR_POLYGON");
            EmrRecord.Add(0x00000004, "EMR_POLYLINE");
            EmrRecord.Add(0x00000005, "EMR_POLYBEZIERTO");
            EmrRecord.Add(0x00000006, "EMR_POLYLINETO");
            EmrRecord.Add(0x00000007, "EMR_POLYPOLYLINE");
            EmrRecord.Add(0x00000008, "EMR_POLYPOLYGON");
            EmrRecord.Add(0x00000009, "EMR_SETWINDOWEXTEX");
            EmrRecord.Add(0x0000000A, "EMR_SETWINDOWORGEX");
            EmrRecord.Add(0x0000000B, "EMR_SETVIEWPORTEXTEX");
            EmrRecord.Add(0x0000000C, "EMR_SETVIEWPORTORGEX");
            EmrRecord.Add(0x0000000D, "EMR_SETBRUSHORGEX");
            EmrRecord.Add(0x0000000E, "EMR_EOF");
            EmrRecord.Add(0x0000000F, "EMR_SETPIXELV");
            EmrRecord.Add(0x00000010, "EMR_SETMAPPERFLAGS");
            EmrRecord.Add(0x00000011, "EMR_SETMAPMODE");
            EmrRecord.Add(0x00000012, "EMR_SETBKMODE");
            EmrRecord.Add(0x00000013, "EMR_SETPOLYFILLMODE");
            EmrRecord.Add(0x00000014, "EMR_SETROP2");
            EmrRecord.Add(0x00000015, "EMR_SETSTRETCHBLTMODE");
            EmrRecord.Add(0x00000016, "EMR_SETTEXTALIGN");
            EmrRecord.Add(0x00000017, "EMR_SETCOLORADJUSTMENT");
            EmrRecord.Add(0x00000018, "EMR_SETTEXTCOLOR");
            EmrRecord.Add(0x00000019, "EMR_SETBKCOLOR");
            EmrRecord.Add(0x0000001A, "EMR_OFFSETCLIPRGN");
            EmrRecord.Add(0x0000001B, "EMR_MOVETOEX");
            EmrRecord.Add(0x0000001C, "EMR_SETMETARGN");
            EmrRecord.Add(0x0000001D, "EMR_EXCLUDECLIPRECT");
            EmrRecord.Add(0x0000001E, "EMR_INTERSECTCLIPRECT");
            EmrRecord.Add(0x0000001F, "EMR_SCALEVIEWPORTEXTEX");
            EmrRecord.Add(0x00000020, "EMR_SCALEWINDOWEXTEX");
            EmrRecord.Add(0x00000021, "EMR_SAVEDC");
            EmrRecord.Add(0x00000022, "EMR_RESTOREDC");
            EmrRecord.Add(0x00000023, "EMR_SETWORLDTRANSFORM");
            EmrRecord.Add(0x00000024, "EMR_MODIFYWORLDTRANSFORM");
            EmrRecord.Add(0x00000025, "EMR_SELECTOBJECT");
            EmrRecord.Add(0x00000026, "EMR_CREATEPEN");
            EmrRecord.Add(0x00000027, "EMR_CREATEBRUSHINDIRECT");
            EmrRecord.Add(0x00000028, "EMR_DELETEOBJECT");
            EmrRecord.Add(0x00000029, "EMR_ANGLEARC");
            EmrRecord.Add(0x0000002A, "EMR_ELLIPSE");
            EmrRecord.Add(0x0000002B, "EMR_RECTANGLE");
            EmrRecord.Add(0x0000002C, "EMR_ROUNDRECT");
            EmrRecord.Add(0x0000002D, "EMR_ARC");
            EmrRecord.Add(0x0000002E, "EMR_CHORD");
            EmrRecord.Add(0x0000002F, "EMR_PIE");
            EmrRecord.Add(0x00000030, "EMR_SELECTPALETTE");
            EmrRecord.Add(0x00000031, "EMR_CREATEPALETTE");
            EmrRecord.Add(0x00000032, "EMR_SETPALETTEENTRIES");
            EmrRecord.Add(0x00000033, "EMR_RESIZEPALETTE");
            EmrRecord.Add(0x00000034, "EMR_REALIZEPALETTE");
            EmrRecord.Add(0x00000035, "EMR_EXTFLOODFILL");
            EmrRecord.Add(0x00000036, "EMR_LINETO");
            EmrRecord.Add(0x00000037, "EMR_ARCTO");
            EmrRecord.Add(0x00000038, "EMR_POLYDRAW");
            EmrRecord.Add(0x00000039, "EMR_SETARCDIRECTION");
            EmrRecord.Add(0x0000003A, "EMR_SETMITERLIMIT");
            EmrRecord.Add(0x0000003B, "EMR_BEGINPATH");
            EmrRecord.Add(0x0000003C, "EMR_ENDPATH");
            EmrRecord.Add(0x0000003D, "EMR_CLOSEFIGURE");
            EmrRecord.Add(0x0000003E, "EMR_FILLPATH");
            EmrRecord.Add(0x0000003F, "EMR_STROKEANDFILLPATH");
            EmrRecord.Add(0x00000040, "EMR_STROKEPATH");
            EmrRecord.Add(0x00000041, "EMR_FLATTENPATH");
            EmrRecord.Add(0x00000042, "EMR_WIDENPATH");
            EmrRecord.Add(0x00000043, "EMR_SELECTCLIPPATH");
            EmrRecord.Add(0x00000044, "EMR_ABORTPATH");
            EmrRecord.Add(0x00000046, "EMR_COMMENT");
            EmrRecord.Add(0x00000047, "EMR_FILLRGN");
            EmrRecord.Add(0x00000048, "EMR_FRAMERGN");
            EmrRecord.Add(0x00000049, "EMR_INVERTRGN");
            EmrRecord.Add(0x0000004A, "EMR_PAINTRGN");
            EmrRecord.Add(0x0000004B, "EMR_EXTSELECTCLIPRGN");
            EmrRecord.Add(0x0000004C, "EMR_BITBLT");
            EmrRecord.Add(0x0000004D, "EMR_STRETCHBLT");
            EmrRecord.Add(0x0000004E, "EMR_MASKBLT");
            EmrRecord.Add(0x0000004F, "EMR_PLGBLT");
            EmrRecord.Add(0x00000050, "EMR_SETDIBITSTODEVICE");
            EmrRecord.Add(0x00000051, "EMR_STRETCHDIBITS");
            EmrRecord.Add(0x00000052, "EMR_EXTCREATEFONTINDIRECTW");
            EmrRecord.Add(0x00000053, "EMR_EXTTEXTOUTA");
            EmrRecord.Add(0x00000054, "EMR_EXTTEXTOUTW");
            EmrRecord.Add(0x00000055, "EMR_POLYBEZIER16");
            EmrRecord.Add(0x00000056, "EMR_POLYGON16");
            EmrRecord.Add(0x00000057, "EMR_POLYLINE16");
            EmrRecord.Add(0x00000058, "EMR_POLYBEZIERTO16");
            EmrRecord.Add(0x00000059, "EMR_POLYLINETO16");
            EmrRecord.Add(0x0000005A, "EMR_POLYPOLYLINE16");
            EmrRecord.Add(0x0000005B, "EMR_POLYPOLYGON16");
            EmrRecord.Add(0x0000005C, "EMR_POLYDRAW16");
            EmrRecord.Add(0x0000005D, "EMR_CREATEMONOBRUSH");
            EmrRecord.Add(0x0000005E, "EMR_CREATEDIBPATTERNBRUSHPT");
            EmrRecord.Add(0x0000005F, "EMR_EXTCREATEPEN");
            EmrRecord.Add(0x00000060, "EMR_POLYTEXTOUTA");
            EmrRecord.Add(0x00000061, "EMR_POLYTEXTOUTW");
            EmrRecord.Add(0x00000062, "EMR_SETICMMODE");
            EmrRecord.Add(0x00000063, "EMR_CREATECOLORSPACE");
            EmrRecord.Add(0x00000064, "EMR_SETCOLORSPACE");
            EmrRecord.Add(0x00000065, "EMR_DELETECOLORSPACE");
            EmrRecord.Add(0x00000066, "EMR_GLSRECORD");
            EmrRecord.Add(0x00000067, "EMR_GLSBOUNDEDRECORD");
            EmrRecord.Add(0x00000068, "EMR_PIXELFORMAT");
            EmrRecord.Add(0x00000069, "EMR_DRAWESCAPE");
            EmrRecord.Add(0x0000006A, "EMR_EXTESCAPE");
            EmrRecord.Add(0x0000006C, "EMR_SMALLTEXTOUT");
            EmrRecord.Add(0x0000006D, "EMR_FORCEUFIMAPPING");
            EmrRecord.Add(0x0000006E, "EMR_NAMEDESCAPE");
            EmrRecord.Add(0x0000006F, "EMR_COLORCORRECTPALETTE");
            EmrRecord.Add(0x00000070, "EMR_SETICMPROFILEA");
            EmrRecord.Add(0x00000071, "EMR_SETICMPROFILEW");
            EmrRecord.Add(0x00000072, "EMR_ALPHABLEND");
            EmrRecord.Add(0x00000073, "EMR_SETLAYOUT");
            EmrRecord.Add(0x00000074, "EMR_TRANSPARENTBLT");
            EmrRecord.Add(0x00000076, "EMR_GRADIENTFILL");
            EmrRecord.Add(0x00000077, "EMR_SETLINKEDUFIS");
            EmrRecord.Add(0x00000078, "EMR_SETTEXTJUSTIFICATION");
            EmrRecord.Add(0x00000079, "EMR_COLORMATCHTOTARGETW");
            EmrRecord.Add(0x0000007A, "EMR_CREATECOLORSPACEW");
            return EmrRecord;
        } //LoadEmrRecord()


    } //DICTIONARY_EMF

    /** \brief Enumeraciones para System.Printing
    */
    public class ENUM_SYSTEM_PRINTING
    {
        public enum EVENTS
        {
            PRINTER_PAUSED = 303, //Pausar cola impresión
            PRINTER_UNPAUSED = 304, //Reanudar cola impresión
            PRINTER_SET = 306, //Estableciendo configuracion de impresora
            DOCUMENT_PRINTED = 307, //Documento impreso
            DOCUMENT_RESUMED = 309, // Reanudando impresión de documento
            DOCUMENT_DELETED = 310, // Trabajo de impresión eliminado
            JOB_DIAG = 800, //Poniendo trabajo en cola
            JOB_DIAG_PRINTING = 801, //Imprimiendo
            DELETE_JOB_DIAG = 802, //Eliminando trabajo de impresión
            RENDER_JOB_DIAG = 805, //Presentando trabajo de impresión
            FILE_OP_FAILED = 812, //No se pueden eliminar ficheros de Spool
            PRINT_DRIVER_SANDBOX_JOB_PRINTPROC = 842, // El servidor de Impresión envió el trabajo a la impresora
            _ID_EVEN_CONTROL = 9999, // Evento generado por la aplicación para chequear que se estan monitorizando los eventos
        }
    }

    /** \brief struct con las propiedades que recopilamos para cada trabajo de impresión
    */
    public struct STRUCT_PRINT_JOB ///> Estructura con las propiedades de un trabajo de impresión
    {
        ///> JJJJJ(JobID)YYYY(Año)MM(Mes)DD(Día)HH(Hora)MiMi(Minuto)SS(Segundo)
        public string ID_JOBNAME;
        ///> System.Printing, API, JPL/PCLXL, EMF
        public string ID_FUENTE; 
        public int N_JOB;
        public string F_PRINTJOB;
        public string ID_LOGIN;
        public string ID_PRINTSERVER;
        public string ID_PRINTER;
        public string ID_DOCUMENT;
        public int N_PAGES;
        public int N_PAGES_PRINTED;
        public int N_COLORPAGES;
        public int N_LENGTH;
        public int N_WIDTH;
        public string ID_MEDIASIZE;
        public int N_MEDIASIZE;
        public string ID_ORIENTATION;
        public int N_ORIENTATION;
        public string ID_DUPLEX;
        public int N_DUPLEX;
        public string ID_COLOR;
        public int N_COLOR;
        public int N_COPIES; // En PCLXL puede no tener mucho sentido por que para cada página se puede especificar un número diferente de copias
        public string ID_STATUS;
        public string ID_ISPLOTTER; // S/N
        public string ID_MEDIATYPE; // Tipo de papel
        public int N_MEDIATYPE; // Código tipo de papel
        public int N_JOBSIZE; // Tamaño en bytes del trabajo de impresión
    } //PRINT_JOB

    /** \brief
     * Clase con estructuras para interactuar con E/S ficheros
     */
    public class STRUCT_WATCHIO
    {
        // Structura con información de directorios necesarios para hacer backup de un archivo
        public struct BackupFile
        {
            public string PathDirectory;
            public string FileName;
            public string PathDirectoryBackup;
            public string FileNameBackup;
        }
    } // STRUCT_WATCHIO


    /** \brief Clase para almacenar items.
        \details La utilizamos para guardar un resumen de los distintos Items encontrados en el análisis de los ficheros de Spool
    */
    public class Tags
    {
        // Diccionario para almacenar items
        //private Dictionary<string, uint> DiccionarioItems;
        private SortedDictionary<string, uint> DiccionarioItems;

        // Se crea el Logger con nombre: EmfSpool
        private static readonly ILog Log = LogManager.GetLogger("Items");


        /** \brief Constructor
        */
        public Tags()
        {
            //Inicializamos el diccionario
            DiccionarioItems = new SortedDictionary<string, uint>();
        }

        /** \brief Agregar un Tag
        *   \details Si no existe el tag se agrega al diccionario y se contabiliza a 1. Si existe se actualiza el número total ++1.
        *   \param Tag Tag a contabilizar
        */
        public void Agregar(string Tag)
        {
            // Si existe el tag incrementamos en 1 su número de ocurrencias
            if (DiccionarioItems.ContainsKey(Tag))
            {
                uint Value;
                this.DiccionarioItems.TryGetValue(Tag,out Value);
                this.DiccionarioItems[Tag] = Value + 1;
            }
            // Si no existe el tag se inserta en el diccionario inicializando el total en 1
            else
            {
                this.DiccionarioItems.Add(Tag, 1);
            }
        }

        /** \brief Generar log con resumen de los Items registrados
        */
        public void ResumenTags()
        {
            uint TotalOcurrencias = 0;
            Log.Info("--------------------------------------------------------");
            Log.Info("Resumen Tags con el número de ocurrencias.");
            Log.Info("--------------------------------------------------------");
            foreach (KeyValuePair<string, uint> Tag in this.DiccionarioItems)
            {
                Log.Info(Tag.Key + ": " + Tag.Value.ToString());
                TotalOcurrencias += Tag.Value;
            }
            Log.Info("--------------------------------------------------------");
            Log.Info("Total: " + TotalOcurrencias.ToString());
            Log.Info("--------------------------------------------------------");
        }

        /** \brief Total Tags contabiliazados
        *   \return (int) Total de tags analizados
        */
        public uint TotalTags()
        {
            uint TotalOcurrencias = 0;
            foreach (KeyValuePair<string, uint> Tag in this.DiccionarioItems)
            {
                TotalOcurrencias += Tag.Value;
            }
            Log.Info("--------------------------------------------------------");
            Log.Info("Total: " + TotalOcurrencias.ToString());
            Log.Info("--------------------------------------------------------");

            return TotalOcurrencias;
        }
    } // Class Tags
}
