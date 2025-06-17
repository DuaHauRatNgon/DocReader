using Application.Services;
using DocReader.Service.DocumentProcessor;
using Infrastructure.FileStorage;
using Infrastructure.PdfProcessing;
using Infrastructure.Repository;
using Core.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

using Infrastructure.Identity;
using Infrastructure.Email;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Models.Domain;

namespace API {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));
            builder.Services.AddScoped<DocumentUploadService>();
            builder.Services.AddScoped<IDocumentProcessor, PdfSharpDocumentProcessor>();
            builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
            builder.Services.AddScoped<DocumentLoadService>();
            builder.Services.AddScoped<DocumentModifyService>();
            builder.Services.AddScoped<DocumentRemoveService>();
            builder.Services.AddScoped<DocumentLoadingByBatchService>();
            builder.Services.AddScoped<DocumentPageRepository>();
            builder.Services.AddScoped<TagService>();
            builder.Services.AddScoped<TagRepository>();
            builder.Services.AddScoped<DocumentSearchService>();

            builder.Services.AddHttpContextAccessor();


            builder.Services.AddIdentity<AppUser, AppRole>(options => {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();



            builder.Services.AddScoped<RoleSeeder>();


            builder.Services.AddCors(options => {
                options.AddDefaultPolicy(policy => {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            builder.Services.AddScoped<EmailSender>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();


            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "/api/auth/logout", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token ở dạng: Bearer {token}"
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                });
            });


            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options => {
            //        options.TokenValidationParameters = new TokenValidationParameters {
            //        };
            //    });

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
                        options.Events = new JwtBearerEvents {
                            OnAuthenticationFailed = context => {
                                Console.WriteLine($"Authentication failed: {context.Exception}");
                                return Task.CompletedTask;
                            },
                            OnChallenge = context => {
                                Console.WriteLine($"OnChallenge: {context.Error}");
                                return Task.CompletedTask;
                            }
                        };

                        options.TokenValidationParameters = new TokenValidationParameters {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(key)
                        };
                    });



            builder.Services.AddAuthorization();

            builder.Services.AddScoped<CommentRepository>();
            builder.Services.AddScoped<CommentService>();


            var app = builder.Build();




            using (var scope = app.Services.CreateScope()) {
                var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
                await roleSeeder.SeedRolesAsync();
            }



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(Directory.GetCurrentDirectory(), "storage", "documents")),
                RequestPath = "/storage/documents"
            });




            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}