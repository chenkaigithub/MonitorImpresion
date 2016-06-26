using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// La información general sobre un ensamblado se controla mediante el siguiente 
// conjunto de atributos. Cambie estos atributos para modificar la información
// asociada con un ensamblado.
[assembly: AssemblyTitle("MonitorImpresion")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MonitorImpresion")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Si establece ComVisible como false, los tipos de este ensamblado no estarán visibles 
// para los componentes COM. Si necesita obtener acceso a un tipo de este ensamblado desde 
// COM, establezca el atributo ComVisible como true en este tipo.
[assembly: ComVisible(false)]

// El siguiente GUID sirve como identificador de typelib si este proyecto se expone a COM
[assembly: Guid("cc186831-81e1-4205-9517-4831cd35c796")]

// La información de versión de un ensamblado consta de los cuatro valores siguientes:
//
//      Versión principal
//      Versión secundaria 
//      Número de compilación
//      Revisión
//
// Puede especificar todos los valores o establecer como predeterminados los números de versión de compilación y de revisión 
// mediante el asterisco ('*'), como se muestra a continuación:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Para que coja la configuracion de la librería de log: log4net
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

//private static readonly log4net.Core.Level GeneralLevel = new log4net.Core.Level(50000, "General");
//[assembly: log4net.Util.LogLog.InternalDebugging=true]
//[assembly: log4net.LogManager.GetRepository().LevelMap.Add(GeneralLevel)]
