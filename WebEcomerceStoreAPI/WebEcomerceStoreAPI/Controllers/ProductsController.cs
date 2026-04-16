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
    [Authorize(Roles ="Admin, User")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAll")]
        public async Task<IBussinessResult> GetList()
        {
            return await _productService.GetAllProduct();
        }
        [HttpGet("{id}")]
        public async Task<IBussinessResult>GetById(Guid id)
        {
            return await _productService.GetByIdProduct(id);
        }
        [HttpGet("GetByName")]
        public async Task<IBussinessResult>GetProductByName(string name)
        {
            return await _productService.GetProductByName(name);
        }
        [HttpPost("addOrUpdate")]
        public async Task<IBussinessResult>AddOrUpdateProduct(AddOrUpdateProductRequest request)
        {
            return await _productService.AddOrUpdateProduct(request);
        }
        [HttpDelete("delete")]
        public async Task<IBussinessResult>DeleteProduct(Guid Id)
        {
            return await _productService.DeleteProduct(Id);
        }
       

    }
}
