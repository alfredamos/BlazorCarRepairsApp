using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCarRepairsApp.Models;

public class Token
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string AccessToken { get; set; } =string.Empty;

    [Required]
    [MaxLength(1000)]
    public string RefreshToken { get; set; } =string.Empty;

    public bool Expired { get; set; } = false;

    public bool Revoked { get; set; } = false;
    
    [Required]
    [EnumDataType(typeof(TokenType))]
    public TokenType TokenType { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
    
    public  Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}