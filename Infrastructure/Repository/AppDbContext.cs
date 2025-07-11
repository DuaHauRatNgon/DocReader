﻿using Core.Models.Domain;
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


        public DbSet<DocumentVote> DocumentVotes { get; set; }


        public DbSet<PageBookmark> PageBookmarks { get; set; }


        public DbSet<Notification> Notifications { get; set; }


        public DbSet<Report> Reports { get; set; }


        public DbSet<ReportReasonOption> ReportReasonOptions { get; set; }


        public DbSet<HighlightQuote> HighlightQuotes { get; set; }



        public DbSet<PendingDocument> PendingDocuments { get; set; }
        public DbSet<PendingDocumentTag> PendingDocumentTags { get; set; }



        public DbSet<ReadingHistory> ReadingHistory { get; set; }


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
                .HasForeignKey(dt => dt.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DocumentTag>()
                .HasOne(dt => dt.Tag)
                .WithMany(t => t.DocumentTags)
                .HasForeignKey(dt => dt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure DocumentTag is properly configured as a join table
            modelBuilder.Entity<DocumentTag>()
                .ToTable("DocumentTags");

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



            modelBuilder.Entity<DocumentVote>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.DocumentId }).IsUnique();
                entity.HasOne(e => e.Document)
                      .WithMany(d => d.Votes)
                      .HasForeignKey(e => e.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            /// user chi duoc bookmark 1 page 1 lan --- unique
            modelBuilder.Entity<PageBookmark>()
                .HasIndex(b => new { b.UserId, b.DocumentId, b.PageNumber })
                .IsUnique();






            modelBuilder.Entity<PendingDocumentTag>()
                .HasKey(pt => new { pt.PendingDocumentId, pt.TagId });

            modelBuilder.Entity<PendingDocumentTag>()
                .HasOne(pt => pt.PendingDocument)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.PendingDocumentId);

            modelBuilder.Entity<PendingDocumentTag>()
                .HasOne(pt => pt.Tag)
                .WithMany()
                .HasForeignKey(pt => pt.TagId);
        }
    }

}
