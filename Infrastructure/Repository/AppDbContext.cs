using Core.Models.Domain;
using Core.Models.Domain.Core.Models.Domain;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repository {
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string> {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentPage> DocumentPages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DocumentTag> DocumentTags { get; set; }



        public DbSet<RefreshToken> RefreshTokens { get; set; }



        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }



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




            modelBuilder.Entity<AppUser>(entity => {
                entity.Property(e => e.FullName).HasMaxLength(100);
            });

            modelBuilder.Entity<AppRole>(entity => {
                entity.Property(e => e.Description).HasMaxLength(200);
            });

            modelBuilder.Entity<AppUser>().ToTable("Users");
            modelBuilder.Entity<AppRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");


            modelBuilder.Entity<RefreshToken>()
                           .HasOne(t => t.User)
                           .WithMany()
                           .HasForeignKey(t => t.UserId);


        }
    }

}
