using Nishan.Logger;

namespace Nishan.Data
{
    public class DataAccessFactory : IDataAccessFactory
    {
        private readonly IConnectionConfiguration configuration;
        private readonly ILogger logger;

        public DataAccessFactory(IConnectionConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public IDataAccess GetDataAccess(string requestId)
        {
           return new DataAccess(configuration,logger, requestId);
        }
    }
}