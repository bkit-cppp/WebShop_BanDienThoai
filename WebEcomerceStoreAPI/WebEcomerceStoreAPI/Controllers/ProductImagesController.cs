using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin, User")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImagesService _productImages;
        public ProductImagesController(IProductImagesService productImages)
        {
            _productImages = productImages;
        }
        [HttpGet("get-all")]
        public async Task<IBussinessResult> GetAll()
        {
            return await _productImages.GetAllImages();
        }
        [HttpGet("{Id}")]
        public async Task<IBussinessResult>GetByIdImages(Guid id)
        {
            return await _productImages.GetByIdImages(id);
        }
        [HttpPost("addImages")]
        public async Task<IBussinessResult>AddProductImages([FromForm] UploadProductImagesRequest res)
        {
            return await _productImages.AddImages(res.file, res.Id);
        }
    }
}
