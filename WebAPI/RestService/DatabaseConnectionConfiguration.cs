using Nishan.Data;

namespace RestService
{
    internal class DatabaseConnectionConfiguration :IConnectionConfiguration
    {
        public string UserName {get;set;}
       //TODO: This property value should not be written during JSON serialization into log file
        public string Password{get;set;}
        public string  Url {get;set;}
        public  string Database {get;set;}

        public bool UseVerboseLog {get;set;}

        
    }
}