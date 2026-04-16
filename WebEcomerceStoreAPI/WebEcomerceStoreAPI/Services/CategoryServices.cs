using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class CategoryServices : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IBussinessResult> AddOrUpdateCategory(AddOrUpdateCategoryRequest request)
        {
            if (request == null)
                return new BussinessResult(Const.FAIL_CREATE_CODE, "Không thể tạo mới");
            try
            {
                var existingCategory = await _unitOfWork.Category.GetByIdAsync(request.Id);
                if(existingCategory==null)
                {
                    var category = new Category()
                    {
                        CategoryId=Guid.NewGuid(),
                        Description=request.Description,
                        Name=request.Name,
                     };
                    await _unitOfWork.Category.CreateAsync(category);
                    await _unitOfWork.Category.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Thêm mới thành công");
                }
                else
                {
                    existingCategory.Name = request.Name;
                    existingCategory.Description = request.Description;
                     await _unitOfWork.Category.UpdateAsync(existingCategory);
                     await _unitOfWork.Category.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công");
                }
            }catch(Exception ex)
            {
                throw new Exception ();
            }
        }

        public async Task<IBussinessResult> DeleteCategory(Guid Id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(Id);
            if(category==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu ");
            }
            var result = await _unitOfWork.Category.RemoveAsync(category);
            if(result)
            {
                return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xóa dữ liệu thành công");
            }
            return new BussinessResult(Const.FAIL_DELETE_CODE, "Xóa dữ liệu không thành công");

        }

        public async Task<IBussinessResult> GetAllCategory()
        {
            var listCategory = await _unitOfWork.Category.GetAllAsync();
            if (listCategory == null || !listCategory.Any())
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy data");
            }
            var categoryResponse = listCategory.Select(c => new CategoryResponse
            {
                CategoryId=c.CategoryId,
                Name=c.Name,
                Description=c.Description
            });
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Kết quả trả về:", categoryResponse);

        }

        public async Task<IBussinessResult> GetByIdCategory(Guid id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy data");
            }
            var categoryResponse = new CategoryResponse
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Data Read Successfull", categoryResponse);
        }

        public async Task<IBussinessResult> GetByNameCategory(string categoryName)
        {
            var res = await _unitOfWork.Category.GetByName (categoryName);
            if(res==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy dữ liệu");
            }
            var categoryResponse = new CategoryResponse
            {
                CategoryId=res.CategoryId,
                Name=res.Name,
                Description=res.Description
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, " Đọc thông tin thành công", categoryResponse);
        }

        public async Task<IBussinessResult> GetPagination(Guid? cursorId, int limit)
        {
            if(limit<=0)
                limit = 10;
                var query = _unitOfWork.Category.GetAll().OrderBy(c => c.CategoryId);
            if(cursorId.HasValue && cursorId.Value  != Guid .Empty)
            {
                query = query.Where(c => c.CategoryId.CompareTo(cursorId.Value) > 0).OrderBy(c => c.CategoryId);
            }
            var categories = query.Take(limit + 1).ToList();
            bool hasNextPage = categories.Count > limit;
            var data = categories.Take(limit).Select(c=>new CategoryResponse
            {
                CategoryId=c.CategoryId,
                Name=c.Name,
                Description=c.Description
            }).ToList();
            string ? nextCusor= hasNextPage ? data.Last().CategoryId.ToString():null;
            var response = new PagingationResponse<CategoryResponse>
            {
                Data = data,
                HasNextPage=hasNextPage,
                Next=nextCusor
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, " Hiển thị trang mới", response );
        }
    }
}
