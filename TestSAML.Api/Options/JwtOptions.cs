using System.ComponentModel.DataAnnotations;

namespace TestSAML.Api.Options;

public sealed class JwtOptions
{
    public const string Section = "Jwt";
    
    [Required]
    [MinLength(32)]
    public string SigningKey { get; set; } = null!;
}