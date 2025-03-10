using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using TestSAML.Api.Options;
using CorsOptions = TestSAML.Api.Options.CorsOptions;

var bld = WebApplication.CreateBuilder();
bld.Configuration.AddJsonFile("appsettings.json", optional: false);

bld.Services
    .AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

bld.Services
    .AddOptions<CorsOptions>()
    .BindConfiguration(CorsOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

bld.Services
    .AddOptions<SamlOptions>()
    .BindConfiguration(SamlOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var samlOptions = bld.Configuration
    .GetRequiredSection(SamlOptions.Section)
    .Get<SamlOptions>()!;

var corsOptions = bld.Configuration
    .GetRequiredSection(CorsOptions.Section)
    .Get<CorsOptions>()!;

bld.Services
    .AddAuthorization()
    .AddCors(x =>
    {
        x.AddDefaultPolicy(policy => policy
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod()
            .WithOrigins(corsOptions.AllowedOrigins)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
        );
    })
    .AddControllers();

bld.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = Saml2Defaults.Scheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.LoginPath = "/saml/login";
        opt.ReturnUrlParameter = "returnUrl";
        opt.Cookie.Name = "saml";
        opt.Cookie.SameSite = SameSiteMode.Lax;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.None;
        opt.Cookie.HttpOnly = false;
    })
    .AddSaml2(opt =>
    {
        opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.ClaimsIssuer = samlOptions.EntityId;
        
        // Set up our EntityId, this is our application.
        opt.SPOptions.EntityId = new EntityId(samlOptions.EntityId);

        // Single logout messages should be signed according to the SAML2 standard, so we need
        // to add a certificate for our app to sign logout messages with to enable logout functionality.

        foreach (var certificateName in samlOptions.CertificateNames)
        {
            opt.SPOptions.ServiceCertificates.Add(new X509Certificate2(certificateName));
        }
        
        // Add an identity provider.
        opt.IdentityProviders.Add(new IdentityProvider(
            // The identityprovider's entity id.
            new EntityId("authentik"),
            opt.SPOptions)
        {
            MetadataLocation = samlOptions.MetadataLocationUrl,
            LoadMetadata = true
        });
    });

bld.Services.AddSwaggerGen();
bld.Services.AddEndpointsApiExplorer();

var app = bld.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();