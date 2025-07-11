using API.GraphQL;
using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Core.Models.Domain;
using Core.Models.Identity;
using DocReader.Service.DocumentProcessor;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Data;
using Infrastructure.Email;
using Infrastructure.FileStorage;
using Infrastructure.Hubs;
using Infrastructure.Hubs;
using Infrastructure.Identity;
using Infrastructure.Interface;
using Infrastructure.PdfProcessing;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
            builder.Services.AddScoped<PageBookmarkService>();
            builder.Services.AddScoped<PageBookmarkRepository>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUserContextService, UserContextService>();


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



            //builder.Services.AddScoped<RoleSeeder>();
            builder.Services.AddScoped<IdentityDataSeeder>();



            builder.Services.AddCors(options => {
                options.AddPolicy("AllowFrontend", policy => {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
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
                            RoleClaimType = ClaimTypes.Role,
                            IssuerSigningKey = new SymmetricSecurityKey(key)
                        };
                    });



            //builder.Services.AddAuthorization();
            builder.Services.AddAuthorization(options => {
                // role-based
                options.AddPolicy("RequireAdmin", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("RequireMod", policy =>
                    policy.RequireRole("Mod"));

                // claim based
                options.AddPolicy("CanApproveDocument", policy =>
                    policy.RequireClaim("Permission", "CanApproveDocument"));

                options.AddPolicy("CanDeleteViolationDoc", policy =>
                    policy.RequireClaim("Permission", "CanDeleteViolationDoc"));

                options.AddPolicy("CanManageTags", policy =>
                    policy.RequireClaim("Permission", "CanManageTags"));

                options.AddPolicy("CanBanUser", policy =>
                    policy.RequireClaim("Permission", "CanBanUser"));

                options.AddPolicy("CanDeleteComment", policy =>
                    policy.RequireClaim("Permission", "CanDeleteComment"));

                options.AddPolicy("CanViewAllUsers", policy =>
                    policy.RequireClaim("Permission", "CanViewAllUsers"));

                // vd role  + claim 
                options.AddPolicy("ModCanManageTags", policy =>
                    policy.RequireRole("Mod")
                          .RequireClaim("Permission", "CanManageTags"));
            });












            builder.Services.AddScoped<CommentRepository>();
            builder.Services.AddScoped<CommentService>();


            builder.Services.AddScoped<DocumentVoteRepository>();
            builder.Services.AddScoped<VoteService>();
            builder.Services.AddScoped<DocumentRepository>();
            builder.Services.AddScoped<DocumentRelatedService>();


            builder.Services
                .AddGraphQLServer()
                // root query
                .AddQueryType<Query>()
                .AddFiltering()
                .AddSorting()
                // add projection vi minh select vào dto
                .AddProjections()
            //.AddMutationType<Mutation>();
            ;


            builder.Services.AddSignalR();
            builder.Services.AddScoped<SendNotificationAllUserService>();
            builder.Services.AddScoped<NotificationRepository>();


            builder.Services.AddScoped<ReportReasonOptionService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<ReportRepository>();
            builder.Services.AddScoped<ReportReasonOptionRepository>();


            //builder.Services.AddCors(options => {
            //    options.AddPolicy("AllowNgrok", policy => {
            //        policy.WithOrigins(" https://db81-222-252-26-209.ngrok-free.app")
            //              .AllowAnyHeader()
            //              .AllowAnyMethod()
            //              .AllowCredentials();
            //    });
            //});


            builder.Services.AddScoped<IHighlightQuoteRepository, HighlightQuoteRepository>();
            builder.Services.AddScoped<HighlightQuoteService>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            builder.Services.AddScoped<IPermissionService, PermissionService>();


            builder.Services.AddScoped<IPendingDocumentRepository, PendingDocumentRepository>();
            builder.Services.AddScoped<IUploadService, UploadService>();
            builder.Services.AddScoped<ApprovalService>();
            builder.Services.AddScoped<PendingDocumentService>();



            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IReadingHistoryRepository, ReadingHistoryRepository>();
            builder.Services.AddScoped<IReadingHistoryService, ReadingHistoryService>();


            builder.Services.AddScoped<IFileStorageService, PhysicalFileStorageService>();



            builder.Services.AddScoped<NotificationApprovalSenderService>();


            var app = builder.Build();





            //using (var scope = app.Services.CreateScope()) {
            //    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
            //    await roleSeeder.SeedRolesAsync();
            //}

            using (var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await IdentityDataSeeder.SeedAsync(roleManager, userManager);
            }




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                                          System.IO.Path
                                          .Combine(Directory.GetCurrentDirectory(),
                                                                            "storage", "documents")),
                RequestPath = "/storage/documents"
            });


            app.UseRouting();

            app.UseHttpsRedirection();

            //app.UseCors("AllowNgrok");
            //app.UseCors();
            app.UseCors("AllowFrontend");


            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            //app.UseCors("AllowFrontend");
            //app.MapHub<NotificationHub>("/hubs/notification");
            app.MapHub<NotificationHub>("/hubs/notification").RequireCors("AllowFrontend");

            app.Run();
        }
    }
}