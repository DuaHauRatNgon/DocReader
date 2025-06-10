using Core.Models.Domain;
using Core.Models.Domain.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repository {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentPage> DocumentPages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DocumentTag> DocumentTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>()
                .HasMany(d => d.Pages)
                .WithOne(p => p.Document)
                .HasForeignKey(p => p.DocumentId);

            modelBuilder.Entity<DocumentTag>()
           .HasKey(dt => new { dt.DocumentId, dt.TagId });

            modelBuilder.Entity<DocumentTag>()
                .HasOne(dt => dt.Document)
                .WithMany(d => d.Tags)
                .HasForeignKey(dt => dt.DocumentId);

            modelBuilder.Entity<DocumentTag>()
                .HasOne(dt => dt.Tag)
                .WithMany(t => t.DocumentTags)
                .HasForeignKey(dt => dt.TagId);
        }
    }

}
