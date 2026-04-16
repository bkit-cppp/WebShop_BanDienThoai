using System.ComponentModel.DataAnnotations;

namespace WebEcomerceStoreAPI.Common
{
    public class AddOrUpdateCategoryRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Chưa nhâp tên")]
        public string Name { get; set; }
        [Required (ErrorMessage ="Chưa nhập mô tả")]   
        public string Description { get; set; }
    }
}
