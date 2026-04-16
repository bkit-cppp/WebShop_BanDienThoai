using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Entities;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly UnitOfWork _unitOfWork;
        public ReviewServices( UnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBussinessResult> AddOrUpdateReviews(AddOrUpdateReviewsRequest request)
        {
            if(request==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "không đọc được dữ liệu");
            }
            try
            {
                var existingReview = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
                if(existingReview == null)
                {
                    var review = new Reviews()
                    {
                        ReviewId=Guid.NewGuid(),
                        Comment=request.Comment,
                        Rating=request.Rating,
                        CreatedDate=DateTime.Now

                    };
                     await _unitOfWork.Reviews.CreateAsync(review);
                    await _unitOfWork.Reviews.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_CREATE_CODE, "Thêm du lieu thanh cong ");
                }
                else
                {
                    existingReview.Comment = request.Comment;
                    existingReview.Rating = request.Rating;
                    existingReview.CreatedDate = DateTime.Now;
                    await _unitOfWork.Reviews.UpdateAsync(existingReview);
                    await _unitOfWork.Reviews.SaveChangeAsync();
                    return new BussinessResult(Const.SUCCESS_UPDATE_CODE, "Cap nhat thong tin thanh cong");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }

        public async Task<IBussinessResult> DeleteReviews(Guid id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if(review==null)
            {
                return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xoa Thanh Cong");
            }
            await _unitOfWork.Reviews.RemoveAsync(review);
            await _unitOfWork.Reviews.SaveChangeAsync();
            return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xoa Thanh Cong");
        }

        public async Task<IBussinessResult> GetByIdReview(Guid id)
        {
            var reviewListById = await _unitOfWork.Reviews.GetByIdAsync(id);
            if(reviewListById==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            var reviewResponse = new ReviewResponse()
            {
                Id = id,
                Comment = reviewListById.Comment,
                Rating = reviewListById.Rating,
                CreatedDate = DateTime.Now
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Tìm thấy dữ liệu", reviewResponse);
        }

        public async Task<IBussinessResult> GetByReViews(string reviewName)
        {
            var review = await _unitOfWork.Reviews.GetByName(reviewName);
            if(review==null)
            {
                return new BussinessResult(Const.WARNING_NO_DATA_CODE, "Không có dữ liệu");
            }
            var reviewNameresponse = new ReviewResponse()
            {
                Id=Guid.NewGuid(),
                Comment=review.Comment,
                Rating=review.Rating,
                CreatedDate=DateTime.Now
            };
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công");
                
         }

        public async Task<IBussinessResult> GetListReviews()
        {
            var reviewList = await _unitOfWork.Reviews.GetAllAsync();
            if(reviewList==null)
            {
                return new BussinessResult(Const.FAIL_READ_CODE, "Chưa đọc được danh sách");
            }
            var review = reviewList.Select(r=>new ReviewResponse
            {
                Id=Guid.NewGuid(),
                Comment=r.Comment,
                CreatedDate=DateTime.Now,
                Rating=r.Rating
            });
            return new BussinessResult(Const.SUCCESS_READ_CODE, "Đọc dữ liệu thành công", review);

        }

        //public async Task<IBussinessResult> PaginationReview(Guid? cursorId, int limit)
        //{
        //    if (limit <= 0)
        //        limit = 10;
        //    var query = _unitOfWork.Reviews.GetAll().OrderBy(i => i.ReviewId);
        //    if(cursorId.HasValue && cursorId.Value!=Guid.Empty)
        //    {
        //        query=query.Where()
        //    }    

        //}

    
    }
}
