using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Order> Orders { get; set; }
        
    }
}
