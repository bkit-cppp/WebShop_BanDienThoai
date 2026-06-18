using Microsoft.EntityFrameworkCore;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Common.RequestModel;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class ProductServices : IProductService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<ProductServices> _logger;
        public ProductServices(UnitOfWork unitOfWork, ILogger<ProductServices>logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IBussinessResult> AddOrUpdateProduct(AddOrUpdateProductRequest request)
        {
            if (request == null)
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không thể tạo mới");
            try
            {
                var product = await _unitOfWork.Product.GetByIdAsync(request.ProductId);
                if (product==null)
                {
                    var newProduct = new Product()
                    {
                        ProductId=Guid.NewGuid(),
                        Name=request.Name,
                        Description=request.Description,
                        Price=request.Price,
                        Brand=request.Brand,
                        PictureUrl=request.PictureUrl,
                        QuantityStock=request.QuantityStock,
                        Type=request.Type,
                        CategoryId=request.CategoryId
                        
                    };
                    await _unitOfWork.Product.CreateAsync(newProduct);
                    await _unitOfWork.Product.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Thêm thành công", newProduct);
                }
                else
                {
                    product.ProductId = request.ProductId;
                    product.Name = request.Name;
                    product.Description = request.Description;
                    product.Price = request.Price;
                    product.Brand = request.Brand;
                    product.PictureUrl = request.PictureUrl;
                    product.QuantityStock = request.QuantityStock;
                    product.Type = request.Type;
                    product.CategoryId = request.CategoryId;
                    await _unitOfWork.Product.UpdateAsync(product);
                    await _unitOfWork.Product.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công", product);
                }
               
               
            }catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                return new BussinessResult(Const.ERROR_EXCEPTION, "Hệ thống lỗi");
            }
        }

        public async Task<IBussinessResult> DeleteProduct(Guid id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if(product==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy dữ liệu");
            }
            await _unitOfWork.Product.RemoveAsync(product);
            await _unitOfWork.Product.SaveChangeAsync();
            return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xóa Thành Công");
        }

        public async Task<IBussinessResult> GetAllProduct()
        {
            var product = await _unitOfWork.Product.Context().Products.Include(x=>x.Category).Select(i=>new ProductResponse
            {
                ProductId = i.ProductId,
                ProductName = i.Name,
                ProductDescription = i.Description,
                Price = i.Price,
                Brand = i.Brand,
                PictureUrl=i.PictureUrl,
                Type = i.Type,
                QuantityStock = i.QuantityStock,
                CategoryName=i.Category.Name
            }).ToListAsync();
            if(product==null || !product.Any())
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", product);
         
        }

        public async Task<IBussinessResult> GetByIdProduct(Guid id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if(product==null)
            {
                
                return new BussinessResult (Const.WARNING_NO_DATA_CODE, "Đọc dữ liệu không thành công");
            }
            var productId = new ProductResponse()
            {
               ProductId=Guid.NewGuid(),
               ProductDescription=product.Description,
               ProductName=product.Name,
               Price=product.Price,
               Brand=product.Brand,
               Type=product.Type
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", productId);
        }

        public async Task<IBussinessResult> GetProductByName(string Name)
        {
            var product = await _unitOfWork.Product.GetByName(Name);
            if(product==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Đọc dữ liệu không thành công");
            }
            var productName = new ProductResponse()
            {
                ProductId=product.ProductId,
                ProductDescription=product.Description,
                ProductName=product.Name,
                Price=product.Price,
                Brand=product.Brand,
                Type=product.Type
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", productName);
        }

        public async Task<IBussinessResult> GetProductByPrice(long price)
        {
            var product=await _unitOfWork.Product.GetByPrice(price);
            if (product.Price <= 0||price>1000000000) 
            {
                return new BussinessResult(Const.FAIL_CREATE_CODE, "không có sản phẩm");
            }
            var productbyPrice = new ProductResponse()
            {
                ProductId = product.ProductId,
                ProductDescription = product.Description,
                ProductName = product.Name,
                Price = product.Price,
                Brand = product.Brand,
                Type = product.Type
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", product);
        }
    }
}
