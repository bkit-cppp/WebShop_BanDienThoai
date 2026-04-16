using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class DisCountCodeServices : IDisCountCodeServices
    {
        private readonly UnitOfWork _unitOfWork;

        public DisCountCodeServices( UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBussinessResult> AddOrUpdateDisCountCodeAsync(AddOrUpdateDisCountCodeRequest request)
        {
            if (request == null)
                return new BussinessResult(Const.FAIL_CREATE_CODE, "Không có dữ liệu");
            try
            {
                var existingDisCountCode = await _unitOfWork.DisCountCode.GetByIdAsync(request.DisCountId);
                if (existingDisCountCode == null)
                {
                    var discountCode = new DisCountCode()
                    {
                        DiscountId=Guid.NewGuid(),
                        Code=request.Code,
                        DiscountAmount=request.DiscountAmount,
                        DiscountPercent=request.DiscountPercent,
                        StartDate=DateTime.Now,
                        EndDate=request.EndDate,
                        IsActive=request.IsActive
                      
                    };
                    await _unitOfWork.DisCountCode.CreateAsync(discountCode);
                    await _unitOfWork.DisCountCode.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Thêm dữ liệu thành công");
                }
                else
                {
                    existingDisCountCode.Code = request.Code;
                    existingDisCountCode.DiscountAmount = request.DiscountAmount;
                    existingDisCountCode.DiscountPercent = request.DiscountPercent;
                    existingDisCountCode.EndDate = request.EndDate;
                    existingDisCountCode.StartDate = request.StartDate;
                    existingDisCountCode.IsActive = request.IsActive;
                    await _unitOfWork.DisCountCode.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật dữ liệu thành công");
                }
            }catch(Exception ex)
            {
                return new BussinessResult(Const.FAIL_CREATE_CODE, ex.Message);
            }
        }

        public async Task<IBussinessResult> DeleteDisCountCodeAsync(Guid Id)
        {
            var existingCode = await _unitOfWork.DisCountCode.GetByIdAsync(Id);
            if (existingCode == null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            await _unitOfWork.DisCountCode.RemoveAsync(existingCode);
            await _unitOfWork.DisCountCode.SaveChangeAsync();
            return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Đã xóa thành công");
            

        }

        public async Task<IBussinessResult> GetDisCountCodeByIdAsync(string code)
        {
            var disCountCode = await _unitOfWork.DisCountCode.GetByIdAsync(code);
            if (disCountCode == null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            var disCountCodeId = new DisCountCodeResponse
            {
                DiscountId=Guid.NewGuid(),
                Code=disCountCode.Code,
                StartDate=DateTime.Now,
                EndDate=DateTime.Now,
                DiscountAmount=disCountCode.DiscountAmount,
                DiscountPercent=disCountCode.DiscountPercent,
                IsActive=disCountCode.IsActive
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE,"Đọc Dữ liệu thành công",disCountCode);
        }

        public async Task<bool> IsValidDisCountCodeAsync(Guid disCountcode )
        {
            var code = await _unitOfWork.DisCountCode.GetDisCountCodeByIdAsync(disCountcode);
            if (code == null)
                return false;
            var currentDate = DateTime.Now;
            if(code.StartDate>=currentDate)
            {
                return code.IsActive = true;
            }
            else
            {
                return code.IsActive = false;
            }
        }
    }
}
