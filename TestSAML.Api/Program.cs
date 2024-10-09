using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
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

bld.Services
    .AddAuthorization()
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
    })
    .AddSaml2(opt =>
    {
        opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.ClaimsIssuer = "webion";
        
        // Set up our EntityId, this is our application.
        opt.SPOptions.EntityId = new EntityId("webion");

        // Single logout messages should be signed according to the SAML2 standard, so we need
        // to add a certificate for our app to sign logout messages with to enable logout functionality.
        opt.SPOptions.ServiceCertificates.Add(new X509Certificate2("keystore.p12", password: "webion"));
        opt.SPOptions.ReturnUrl = new Uri("https://localhost:5001/saml/login");
        
        // Add an identity provider.
        opt.IdentityProviders.Add(new IdentityProvider(
            // The identityprovider's entity id.
            new EntityId("http://localhost:8080/realms/master"),
            opt.SPOptions)
        {
            MetadataLocation = "http://localhost:8080/realms/master/protocol/saml/descriptor"
        });
    });

bld.Services.AddSwaggerGen();
bld.Services.AddEndpointsApiExplorer();

var app = bld.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();