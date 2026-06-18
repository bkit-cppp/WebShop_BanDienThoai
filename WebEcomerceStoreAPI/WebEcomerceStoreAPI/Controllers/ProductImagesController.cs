using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;
using WebEcomerceStoreAPI.Services;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    [Consumes("multipart/form-data")]
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

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddImage(Guid productId, [FromForm] UploadProductImagesRequest request)
        {
            var result = await _productImages.AddImages(request.file, productId, request.isMain);
            return Ok(result);
        }


    }
}
