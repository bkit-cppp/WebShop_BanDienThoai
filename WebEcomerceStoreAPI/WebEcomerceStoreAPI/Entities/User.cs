using System.Data;

namespace WebEcomerceStoreAPI.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public virtual Roles Role { get; set; }
        
        public string Password { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Reviews> Reviews { get; set; }
       
    }
}
