using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nishan.Data;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private ILogger<WeatherForecastController> _logger;

        public ItemController(ILogger<WeatherForecastController> logger, IItemRepository itemRepository)
        {


            _logger = logger;
            ItemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public IDataAccessFactory DataAccessFactory { get; }
        public IItemRepository ItemRepository { get; }

        [HttpGet("{id?}")]
        public async Task<ActionResult<List<Item>>> GetItems(int? id, CancellationToken cancellationToken)
        {
            // IItemRepository repository = new ItemRepository(DataAccessFactory);
            List<Item> result;
            if (id.HasValue)
            {
                var item = await ItemRepository.GetItem(id.Value, cancellationToken);
                if (item != null)
                {
                    result = new List<Item>() { item };
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = await ItemRepository.GetItems(cancellationToken);
            }

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
