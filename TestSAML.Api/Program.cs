using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using TestSAML.Api.Options;

var bld = WebApplication.CreateBuilder();
bld.Configuration.AddJsonFile("appsettings.json", optional: false);
bld.Services
    .AddOptions<JwtOptions>()
    .ValidateDataAnnotations()
    .BindConfiguration(JwtOptions.Section);

string[] origins = ["http://localhost:4200", "https://localhost:4200", "http://saml.kaire.webion.it/*"];

bld.Services
    .AddAuthorization()
    .AddCors(x =>
    {
        x.AddDefaultPolicy(policy => policy
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod()
            .WithOrigins(origins)
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
        opt.ClaimsIssuer = "webion";
        
        // Set up our EntityId, this is our application.
        opt.SPOptions.EntityId = new EntityId("webion");

        // Single logout messages should be signed according to the SAML2 standard, so we need
        // to add a certificate for our app to sign logout messages with to enable logout functionality.
        
        opt.SPOptions.ServiceCertificates.Add(new X509Certificate2("keystore.p12"));
        
        // Add an identity provider.
        opt.IdentityProviders.Add(new IdentityProvider(
            // The identityprovider's entity id.
            new EntityId("authentik"),
            opt.SPOptions)
        {
            MetadataLocation = "http://saml.kaire.webion.it/application/saml/sofidel/metadata/",
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