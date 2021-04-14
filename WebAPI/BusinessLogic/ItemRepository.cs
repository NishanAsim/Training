using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nishan.Data;

namespace BusinessLogic
{
    public interface IItemRepository
    {
        Task<Item> GetItem(int itemId, CancellationToken cancellationToken);
        Task<List<Item>> GetItems(CancellationToken cancellationToken);
    }

    public class ItemRepository : IItemRepository
    {
        public ItemRepository(IDataAccessFactory dataAccessFactory)
        {
            DataAccessFactory = dataAccessFactory ?? throw new ArgumentNullException(nameof(dataAccessFactory));
        }

        internal IDataAccessFactory DataAccessFactory { get; }

        public async Task<Item> GetItem(int itemId, CancellationToken cancellationToken)
        {
            var dataAccess = DataAccessFactory.GetDataAccess("requestId");
            var dataRepository = new DataRepository(dataAccess);
            var result = await dataRepository.GetItem(itemId, cancellationToken);
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Item>> GetItems(CancellationToken cancellationToken)
        {
            var dataAccess = DataAccessFactory.GetDataAccess("requestId");
            var dataRepository = new DataRepository(dataAccess);
            return await dataRepository.GetItem(null, cancellationToken);
        }
    }
}
