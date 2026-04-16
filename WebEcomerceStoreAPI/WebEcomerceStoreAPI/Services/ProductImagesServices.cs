using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class ProductImagesServices : IProductImagesService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductImagesServices(UnitOfWork unitOfWork, ICloudinaryService cloudinaryService )
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<IBussinessResult> AddImages(IFormFile file, Guid imageId )
        {
            if (file == null || file.Length == 0)
                return new BussinessResult(Const.FAIL_CREATE_CODE, "Yêu cầu hình ảnh");
            try
            {
                var res_Images = await _cloudinaryService.UploadImages(file, $"productImages/{imageId}",imageId.ToString());
                if (res_Images == null || string.IsNullOrEmpty(res_Images.ToString()))
                    return new BussinessResult(Const.FAIL_CREATE_CODE, " Không tạo được hình ảnh");
                var productImage = new ProductImages
                {
                    ImageId=imageId,
                    ImageUrl=res_Images.Data.ToString()
                };
                var result = await _unitOfWork.ProductImages.CreateAsync(productImage);
                if(result>0)
                
                    return new BussinessResult
                    {
                        Status=Const.SUCCESS_CREATE_CODE,
                        Message="Ảnh đã được tạo thành công",
                        Data=productImage,

                    };
                return new BussinessResult(Const.FAIL_CREATE_CODE, "Không tạo ảnh thành công");
                   
            }catch(Exception ex)
            {
                return new BussinessResult(Const.FAIL_CREATE_CODE, ex.Message);
            }
        }

        public Task<IBussinessResult> DeleteImages(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IBussinessResult> GetAllImages()
        {
            var imagesProduct = await _unitOfWork.ProductImages.GetAllAsync();
            if (imagesProduct == null)
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            return new BussinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, imagesProduct);
                }

        public async Task<IBussinessResult> GetByIdImages(Guid Id)
        {
            var imagesProductById = await _unitOfWork.ProductImages.GetByIdAsync(Id);
            if (imagesProductById == null)
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không hiển thị dữ liệu");
            return new BussinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, imagesProductById);
        }

        public Task<IBussinessResult> UpdateImages()
        {
            throw new NotImplementedException();
        }
    }
}
