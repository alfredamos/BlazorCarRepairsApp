using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlazorCarRepairsApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ImagePath { get; set; } =string.Empty; //ProfileImageUrl

        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        public DateOnly Birthdate { get; set; }

        public Customer? Customer { get; set; }
    
        public Technician? Technician { get; set; }

//----> Navigation property for the "Many" side
        public ICollection<Token> Tokens { get; set; } = new List<Token>();

    }

}
