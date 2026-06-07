using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Platform { get; set; }
    }
}