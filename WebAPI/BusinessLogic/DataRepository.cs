using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Nishan.Data;

namespace BusinessLogic
{
    internal class DataRepository
    {
        public DataRepository(IDataAccess dataAccess)
        {
            DataAccess = dataAccess ?? throw new System.ArgumentNullException(nameof(dataAccess));
        }

        internal IDataAccess DataAccess { get; }

        internal async Task<List<Item>> GetItem(int? itemId, CancellationToken cancellationToken)
        {
            List<SqlCommandParameter> commandParameters = new List<SqlCommandParameter>
            {
                new SqlCommandParameter()
                {
                    Name = "pItemId",
                    Value = itemId,
                    ParameterDirection = SqlParameterDirection.In
                }
            };

            var result = await DataAccess.ExecuteRoutineAsync("GetItems",
                                                               commandParameters,
                                                               cancellationToken);
            return ReadItems(result.Tables[0]);
        }

        private static List<Item> ReadItems(DataTable table)
        {
            List<Item> items = new List<Item>();
            foreach(DataRow dataRow in table.Rows)
            {
                items.Add(ReadItemRow(dataRow));
            }

            return items;
        }
        private static Item ReadItemRow(DataRow itemRow)
        {
            Item item = new Item();
            ReadMasterData(itemRow, item);
            return item;
        }

        private static void ReadMasterData(DataRow row, MasterBase baseItem)
        {
            baseItem.Id = row.GetInteger("Id");
            baseItem.Code = row.GetString("Code");
            baseItem.Description = row.GetString("Description");
        }
    }
}