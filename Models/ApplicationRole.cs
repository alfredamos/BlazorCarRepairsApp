using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlazorCarRepairsApp.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    // Optional: Add custom fields for your roles
    [MaxLength(300)]
    public string? Description { get; set; }
}