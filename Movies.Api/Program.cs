
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Auth;
using Movies.Api.Health;
using Movies.Api.Mapping;
using Movies.Api.Swagger;
using Movies.Application;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.
builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultScheme =JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op => 
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience= config["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience  = true,
    };
   


});

builder.Services.AddAuthorization( op => 
{
    op.AddPolicy(AuthConstants.AdminUserPolicyName, p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

    op.AddPolicy(AuthConstants.TrustedMemberPolicyName, p => p.RequireAssertion(c => c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }
        || c.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" }))));


});

builder.Services.AddApiVersioning(x=> 
{

    x.DefaultApiVersion = new ApiVersion(1.0);
    x.AssumeDefaultVersionWhenUnspecified = true;   
    x.ReportApiVersions = true;

}).AddMvc().AddApiExplorer();

builder.Services.AddControllers();
builder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>();



builder.Services.AddSwaggerGen(x=> x.OperationFilter<SwaggerDefaultValues>());

builder.Services.AddApplication(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x=> 
    {
    foreach(var description in app.DescribeApiVersions())
        {
            x.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    
    });
}

app.MapHealthChecks("_health");
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<ValidationMappingMiddlware>();
app.MapControllers();

app.Run();



