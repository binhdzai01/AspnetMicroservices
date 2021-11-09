using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogServies;
        private readonly IOrderService _orderServices;
        private readonly IBasketService _basketService;

        public ShoppingController(ICatalogService catalogServies, IOrderService orderServices, IBasketService basketService)
        {
            _catalogServies = catalogServies ?? throw new ArgumentNullException(nameof(catalogServies));
            _orderServices = orderServices ?? throw new ArgumentNullException(nameof(orderServices));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }


        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);
            foreach (var item in basket.Items)
            {
                var product = await _catalogServies.GetCatalog(item.ProductId);
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            var orders = await _orderServices.GetOrdersByUserName(userName);
            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };
            return Ok(shoppingModel);
        }

    }
}
