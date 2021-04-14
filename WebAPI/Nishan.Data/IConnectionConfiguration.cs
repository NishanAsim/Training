namespace Nishan.Data
{
    public interface IConnectionConfiguration
    {
        string UserName {get;}
        string Password{get;}
        string  Url {get;}
        string Database {get;}

        bool UseVerboseLog {get;}
    }

}

