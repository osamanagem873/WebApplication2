using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
    }
}
