using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace WebEcomerceStoreAPI.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        [Required(ErrorMessage ="Vui Lòng Nhập vào tên danh mục ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui Lòng Nhập vào phần mô tả")]
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
