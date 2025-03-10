using System.ComponentModel.DataAnnotations;

namespace TestSAML.Api.Options;

public sealed class SamlOptions
{
    public const string Section = "SAML";

    [Required] public string EntityId { get; init; } = null!;
    [Required] public string[] CertificateNames { get; init; } = [];
    [Required] public string MetadataLocationUrl { get; init; } = null!;
}