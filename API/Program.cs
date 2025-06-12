using Application.Services;
using DocReader.Service.DocumentProcessor;
using Infrastructure.FileStorage;
using Infrastructure.PdfProcessing;
using Infrastructure.Repository;
using Core.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace API {
    public class Program {
        public static void Main(string[] args) {
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

            builder.Services.AddCors(options => {
                options.AddDefaultPolicy(policy => {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });



            var app = builder.Build();

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

            app.UseAuthorization();

            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}