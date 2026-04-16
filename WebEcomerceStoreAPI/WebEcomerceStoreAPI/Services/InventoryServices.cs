using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class InventoryServices : IInventoryServices
    {
        private readonly UnitOfWork _unitOfWork;
        public InventoryServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBussinessResult> GetAllInventory()
        {
            var list = await _unitOfWork.Inventory.GetAllAsync();
            if(list==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy dữ liệu");
            }
            var inventoryResponse = list.Select(i=>new InventoryResponse
            {
                Id =i.InventoryId,
                Quantity=i.Quantity,
                LastDated=i.LastDated
            });
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Dữ liệu trả về", inventoryResponse);
        }

        public async Task<IBussinessResult> GetByIdInventory(Guid id)
        {
            var listbyId = await _unitOfWork.Inventory.GetByIdAsync(id);
            if(listbyId==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy dữ liệu");
            }
            var inventoryResponse = new InventoryResponse()
            {
                Id = listbyId.InventoryId,
                Quantity = listbyId.Quantity,
                LastDated = listbyId.LastDated
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Dữ liệu trả về", inventoryResponse);
        }

        public async Task<IBussinessResult> AddOrUpdateInventory(AddOrUpdateInventoryRequest request)
        {
            if(request==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không tìm thấy dữ liệu");
            }
            try
            {
                var existingInventory = await _unitOfWork.Inventory.GetByIdAsync(request.Id);
                if(existingInventory==null)
                {
                    var inventory = new Inventory()
                    {
                        InventoryId=Guid.NewGuid(),
                        Quantity=request.Quantity,
                        LastDated=request.LastDated
                    };
                    await _unitOfWork.Inventory.CreateAsync(inventory);
                    await _unitOfWork.Inventory.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE," Thêm mới thành công");
                }
                else
                {
                    existingInventory.Quantity = request.Quantity;
                    existingInventory.LastDated = request.LastDated;
                    await _unitOfWork.Inventory.UpdateAsync(existingInventory);
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cập nhật thành công");
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IBussinessResult> DeleteInventory(int id)
        {
            var inventoryid = await _unitOfWork.Inventory.GetByIdAsync(id);
            if(inventoryid==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            await _unitOfWork.Inventory.RemoveAsync(inventoryid);
            await _unitOfWork.Inventory.SaveChangeAsync();
            return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xóa thành công");
        }

        public async Task<IBussinessResult> GetPaginationInventory(Guid? cursorId, int limit)
        {
            if (limit <= 0)
                limit = 10;
            var query = _unitOfWork.Inventory.GetAll().OrderBy(i => i.InventoryId);
            if(cursorId.HasValue&& cursorId.Value!=Guid.Empty)
            {
                query = query.Where(c => c.InventoryId.CompareTo(cursorId.Value) > 0).OrderBy(c => c.InventoryId);
            }
            var inventory = query.Take(limit + 1).ToList();
            bool hasNextPage = query.Count() > limit;
            var data = query.Take(limit).Select(i => new InventoryResponse
            {
              LastDated=DateTime.Now,
              Id=Guid.NewGuid(),
              Quantity=i.Quantity
            }).ToList();
            string? nextCursor = hasNextPage ? data.Last().Id.ToString() : null;
            var response = new PagingationResponse<InventoryResponse>()
            {
                Data=data,
                HasNextPage=hasNextPage,
                Next=nextCursor
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Hiển thị trang mới", response);
        }

        public async Task<IBussinessResult> GetByNameInventory(string name)
        {
            var inventory = await _unitOfWork.Inventory.GetByName(name);
            if(inventory==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            var inventoryResonse = new InventoryResponse()
            {
                Id = Guid.NewGuid(),
                Quantity = inventory.Quantity,
                LastDated = DateTime.Now
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Hiển Thị Thành công", inventoryResonse);
        }
    }
}
