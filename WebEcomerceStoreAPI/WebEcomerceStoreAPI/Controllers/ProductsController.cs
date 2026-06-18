using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common.RequestModel;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IBussinessResult> GetAllProduct()
        {
            return await _productService.GetAllProduct();
        }
        [HttpGet("{id}")]
        public async Task<IBussinessResult>GetById(Guid id)
        {
            return await _productService.GetByIdProduct(id);
        }
        [HttpGet("by-name")]
        public async Task<IBussinessResult>GetProductByName(string name)
        {
            return await _productService.GetProductByName(name);
        }
        [HttpGet("by-price")]
        public async Task<IBussinessResult>GetProductByPrice(long price)
        {
            return await _productService.GetProductByPrice(price);
        }
        [HttpPost]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
        public async Task<IBussinessResult>AddOrUpdateProduct(AddOrUpdateProductRequest request)
        {
            return await _productService.AddOrUpdateProduct(request);
        }
        [HttpDelete("{id}")]
        public async Task<IBussinessResult>DeleteProduct(Guid Id)
        {
            return await _productService.DeleteProduct(Id);
        }
    }
}
