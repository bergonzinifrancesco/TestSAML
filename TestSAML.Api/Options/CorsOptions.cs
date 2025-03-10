using System.ComponentModel.DataAnnotations;

namespace TestSAML.Api.Options;

public sealed class CorsOptions
{
    public const string Section = "Cors";

    [Required] public string[] AllowedOrigins { get; init; } = [];
}