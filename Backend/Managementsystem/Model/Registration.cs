using System.ComponentModel.DataAnnotations;

namespace Managementsystem.Model
{
    public class Registration
    {
        public int Id { get; set; }=0;
        public string? Name { get; set; }
        public string? Username { get; set; } 
        public string? Password { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string? Hobbies { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string? Pincode { get; set; }
        public string? FileName { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
