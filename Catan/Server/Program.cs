using Microsoft.AspNetCore.ResponseCompression;
using Catan.Server.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Catan.Client;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddSignalR()
	.AddJsonProtocol(options =>
	{
		options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		options.PayloadSerializerOptions.IncludeFields = true;
	});
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.IncludeFields = true;
	});
builder.Services.AddResponseCompression(opts =>
{
	opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
		new[] { "application/octet-stream" });
});


Database.Startup.ConfigureServices(builder.Services);
BLL.Startup.ConfigureServices(builder.Services);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
        var rsa = RSA.Create();
        var publicKey = @"-----BEGIN RSA PUBLIC KEY-----
MIIBCgKCAQEA9hP5tjExFT2+iNb7hAkBCpdOTvHMz2Jc89LQ//grVUXeZRNUZ8Lc
Jcg7ratGv4RYPUq3/Ddfq7WqhZlLv5wyuiQAeSXl5daJZUHKihLUo53Yr6+6Pxa+
Dy7Q6GtajuNaxGJsdMCPqWHFs59rUJothxBOvS0zMNlqOah1zMTvaIgT7z/YWd2S
l8OfpDGV9PFWvVTFagdfHOL3kvjLmVHDqraYv38enq08WjkJyQ0ygh1PzmL4nEhp
UgczWYJ27eUMsEHI2teQ0oCJXX5QoksT2D5DcBA4rum0o+sFN9YAurcoVEhFHQVY
Bam2qBayNhas/r4u32yDWQUZmF19VreMWQIDAQAB
-----END RSA PUBLIC KEY-----";
        rsa.ImportFromPem(publicKey.AsSpan());


        options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters()
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidAudience = configuration["JWT:ValidAudience"],
			ValidIssuer = configuration["JWT:ValidIssuer"],
			IssuerSigningKey = new RsaSecurityKey(rsa)
		};
	});

builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthorization();

var app = builder.Build();


app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatHub>("/chathub");
app.MapHub<GameHub>("/gamehub");
app.MapHub<NotificationHub>("/notificationhub");



app.MapFallbackToFile("index.html");

app.Run();