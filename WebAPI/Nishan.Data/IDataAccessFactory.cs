namespace Nishan.Data
{
    public interface IDataAccessFactory
    {
        IDataAccess GetDataAccess(string requestId);
    }
}