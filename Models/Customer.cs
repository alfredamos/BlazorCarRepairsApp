using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCarRepairsApp.Models;

public class Customer
{
    [DatabaseGenerated( DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Address { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    [MaxLength(1000)]
    public string? Notes { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }
    
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}