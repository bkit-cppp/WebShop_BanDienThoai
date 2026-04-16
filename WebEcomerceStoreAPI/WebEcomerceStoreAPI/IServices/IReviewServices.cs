using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;

namespace WebEcomerceStoreAPI.IServices
{
    public interface IReviewServices
    {
        Task<IBussinessResult> GetListReviews();                                                                                      
        Task<IBussinessResult> GetByIdReview(Guid id);
        Task<IBussinessResult> AddOrUpdateReviews(AddOrUpdateReviewsRequest request);
        Task<IBussinessResult> DeleteReviews(Guid id);
        Task<IBussinessResult> GetByReViews(string reviewName);
      //  Task<IBussinessResult> PaginationReview(Guid ?cursorId, int limit);
    }
}
