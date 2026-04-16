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
        public ProductServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                        Name=request.Name,
                        Description=request.Description,
                        Price=request.Price,
                        Brand=request.Brand,
                        PictureUrl=request.PictureUrl,
                        QuantityStock=request.QuantityStock,
                        
                    };
                    await _unitOfWork.Product.CreateAsync(newProduct);
                    await _unitOfWork.Product.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Cập nhật thành công");
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
                    await _unitOfWork.Product.UpdateAsync(product);
                    await _unitOfWork.Product.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công");
                }
               
               
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
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
            var product = await _unitOfWork.Product.GetAllAsync();
            if(product==null || product.Any())
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            var productList = product.Select(i => new ProductResponse()
            {
                ProductId=Guid.NewGuid(),
                ProductName=i.Name,
                ProductDescription=i.Description,
                Price=i.Price,
                Brand=i.Brand,
                Type=i.Type,
                QuantityStock=i.QuantityStock
            });
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", productList);
         
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
                ProductId=Guid.NewGuid(),
                ProductDescription=product.Description,
                ProductName=product.Name,
                Price=product.Price,
                Brand=product.Brand,
                Type=product.Type
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công");
        }
    }
}
