using Microsoft.EntityFrameworkCore;
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
        private readonly ILogger<ProductImagesServices> _logger;
        public ProductImagesServices(UnitOfWork unitOfWork, ICloudinaryService cloudinaryService, ILogger<ProductImagesServices>logger )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinaryService = cloudinaryService;
        }
        /*Hàm AddImages thực hiện quy trình upload ảnh cho sản phẩm. Đầu tiên hệ thống kiểm tra file hợp lệ và kiểm tra Product có tồn tại hay không.
         * Sau đó upload file lên Cloudinary và lấy SecureUrl trả về. URL này được dùng để tạo một bản ghi ProductImages. Nếu ảnh được đánh dấu 
         * là ảnh chính hoặc sản phẩm chưa có ảnh đại diện thì hệ thống cập nhật cột PictureUrl của Product. Cuối cùng SaveChangesAsync sẽ
         * thực hiện đồng thời INSERT ProductImages và UPDATE Product trong một transaction của DbContext rồi trả DTO kết quả về cho Frontend.*/
        public async Task<IBussinessResult> AddImages(IFormFile file, Guid productId, bool isMain)
        {
            if (file == null || file.Length == 0)
                return new BussinessResult(Const.FAIL_CREATE_CODE, "Yêu cầu hình ảnh");

            try
            {
                var product = await _unitOfWork.Product.Context() .Products
                    .FirstOrDefaultAsync(x => x.ProductId == productId);

                if (product == null)
                    return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy sản phẩm");
                var allowedTypes = new []{ "image/png","image/jpeg","image/webp"};
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return new BussinessResult(Const.FAIL_CREATE_CODE, "Không hỗ trợ hình ảnh");
                }
                var imageId = Guid.NewGuid();

                var uploadResult = await _cloudinaryService.UploadImages(
                    file,
                    "productImages",
                    imageId.ToString()
                );

                if (uploadResult == null || uploadResult.Data == null)
                    return new BussinessResult(Const.FAIL_CREATE_CODE, "Upload ảnh thất bại");

                var imageUrl = uploadResult.Data.ToString();

                var productImage = new ProductImages
                {
                    ImageId = imageId,
                    ProductId = productId,
                    ImageUrl = imageUrl,
                    IsMain = isMain
                };

                await _unitOfWork.ProductImages.Context() .ProductImages.AddAsync(productImage);

                if (isMain || string.IsNullOrEmpty(product.PictureUrl))
                {
                    product.PictureUrl = imageUrl;
                }

                await _unitOfWork.Product.Context().SaveChangesAsync();
                var response = new ProductImagesResponse()
                {
                    ImageId=productImage.ImageId,
                    ProductId=productImage.ProductId,
                    ImageUrl=productImage.ImageUrl,
                    IsMain=productImage.IsMain
                };

                return new BussinessResult
                {
                    Status = Const.SUCCESS_CREATE_CODE,
                    Message = "Upload ảnh thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BussinessResult(Const.FAIL_CREATE_CODE, ex.Message);
            }
        }

        public async Task<IBussinessResult> DeleteImages(Guid Id)
        {
            try
            {
                var image=await _unitOfWork.ProductImages.GetByIdAsync(Id);
                if(image == null)
                {
                    return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
                }
                var resCloudinary= await _cloudinaryService.DeleteImages(image.ImageUrl);
                if (resCloudinary.Status != Const.SUCCESS_DELETE_CODE)
                    return resCloudinary;
                var res = await _unitOfWork.ProductImages.RemoveAsync(image);
                  if(res)
                return new BussinessResult(Const.SUCCESS_DELETE_CODE,"xóa ảnh thành công");
                return new BussinessResult(Const.FAIL_DELETE_CODE, "xóa ảnh chưa thành công");
            }
            catch (Exception ex)
            {
                return new BussinessResult(Const.FAIL_DELETE_CODE, ex.Message);
            }
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
