﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitorImpresion.DB {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DB.IDB")]
    public interface IDB {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDB/Saludo", ReplyAction="http://tempuri.org/IDB/SaludoResponse")]
        string Saludo(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDB/EjecutaSQL", ReplyAction="http://tempuri.org/IDB/EjecutaSQLResponse")]
        bool EjecutaSQL(string SQL);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDBChannel : MonitorImpresion.DB.IDB, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DBClient : System.ServiceModel.ClientBase<MonitorImpresion.DB.IDB>, MonitorImpresion.DB.IDB {
        
        public DBClient() {
        }
        
        public DBClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DBClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DBClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DBClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Saludo(string value) {
            return base.Channel.Saludo(value);
        }
        
        public bool EjecutaSQL(string SQL) {
            return base.Channel.EjecutaSQL(SQL);
        }
    }
}
