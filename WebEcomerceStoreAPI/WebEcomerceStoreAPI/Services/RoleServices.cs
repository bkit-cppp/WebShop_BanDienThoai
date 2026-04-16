using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class RoleServices : IRoleServices
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBussinessResult> AddOrUpdateRoleAsync(AddOrUpdateRoleRequest request)
        {
            if (request==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy dữ liệu");
            }
            try
            {
                var existingRole = await _unitOfWork.Role.GetByIdAsync(request.RoleId);
                if(existingRole==null)
                {
                    var role = new Roles()
                    {
                        RoleId = request.RoleId,
                        RoleName = request.RoleName
                    };
                    await _unitOfWork.Role.CreateAsync(role);
                    await _unitOfWork.Role.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Thêm mới thành công");
                }
                else
                {
                    existingRole.RoleId = request.RoleId;
                    existingRole.RoleName = request.RoleName;
                    await _unitOfWork.Role.UpdateAsync(existingRole);
                    await _unitOfWork.Role.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công");
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IBussinessResult> DeleteRoleAsync(Guid Id)
        {
            var role = await _unitOfWork.Role.GetByIdAsync(Id);
            if(role==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Khong co dữ liệu để xóa");
            }
             await _unitOfWork.Role.RemoveAsync(role);
             await _unitOfWork.Role.SaveChangeAsync();
            return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xóa thành công");
        }

        public async Task<IBussinessResult> GetByIdRole(int roleId)
        {
            var listById = await _unitOfWork.Role.GetByIdAsync(roleId);
            if(listById==null)
            {
                return new BussinessResult(Const.FAIL_READ_CODE, "không có Id ");
            }
            var role = new RoleResponse()
            {
                Id=roleId,
                RoleName=listById.RoleName
            };
            return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Đọc dữ liệu thành công", role);
        }

        public async Task<IBussinessResult> GetListRole()
        {
            var listRole = await _unitOfWork.Role.GetAllAsync();
            if(listRole==null)
            {
                return new BussinessResult(Const.FAIL_READ_CODE, "danh sách rỗng");
            }
            var role = listRole.Select(r => new RoleResponse
            {
                Id=r.RoleId,
                RoleName=r.RoleName

            });
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", role);
        }
    }
}
