using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;
        public ReviewsController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }
        [HttpGet]
        public async Task<IBussinessResult>GetAll()
        {
            return await _reviewServices.GetListReviews();
        }
        [HttpGet("{id}")]
        public async Task<IBussinessResult> GetByIdReviews(Guid id)
        {
            return await _reviewServices.GetByIdReview(id);
        }
        [HttpPost("addOrUpdate")]
        public async Task<IBussinessResult> AddOrUpdateReview(AddOrUpdateReviewsRequest request)
        {
            return await _reviewServices.AddOrUpdateReviews(request);
        }
        [HttpPost("{id}")]
        public async Task<IBussinessResult>DeleteReview(Guid id)
        {
            return await _reviewServices.DeleteReviews(id);
        }
        [HttpGet("getByName")]
        public async Task<IBussinessResult>GetByReviewName(string reviewName)
        {
            return await _reviewServices.GetByReViews(reviewName);
        }
    }
}
