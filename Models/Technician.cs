using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCarRepairsApp.Models;

public class Technician
{
    [DatabaseGenerated( DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Specialty { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }
    
    public ICollection<AssignedTicket> AssignedTickets { get; set; } = new List<AssignedTicket>();
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

}