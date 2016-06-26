using System.ServiceModel;

namespace WCFDB
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IBDImpresion" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IDB
    {
        [OperationContract]
        string Saludo(string value);

        [OperationContract]
        bool EjecutaSQL(string SQL);
    }

}

