using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EM.Sample.DataRepository.Models;

namespace EM.Sample.DataRepository.Context
{
    public partial class ContentleverageContext : DbContext
    {
        public ContentleverageContext() : base()
        {
        }

        public ContentleverageContext(DbContextOptions<ContentleverageContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=ContentLeverage;Integrated Security=True");
            }
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<AuthorMetaData> AuthorMetaData { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogCategory> BlogCategory { get; set; }
        public virtual DbSet<BlogPost> BlogPost { get; set; }
        public virtual DbSet<BlogStatus> BlogStatus { get; set; }
        public virtual DbSet<BlogTag> BlogTag { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CommentStatus> CommentStatus { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<LoginProvider> LoginProvider { get; set; }
        public virtual DbSet<MetaKey> MetaKey { get; set; }
        public virtual DbSet<MimeType> MimeType { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostAuthor> PostAuthor { get; set; }
        public virtual DbSet<PostCategory> PostCategory { get; set; }
        public virtual DbSet<PostComment> PostComment { get; set; }
        public virtual DbSet<PostStatus> PostStatus { get; set; }
        public virtual DbSet<PostTag> PostTag { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagGroup> TagGroup { get; set; }
        public virtual DbSet<TagGroupMember> TagGroupMember { get; set; }
        public virtual DbSet<Models.ValueType> ValueType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .IsUnique();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Alias).HasMaxLength(150);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.LastName).HasMaxLength(150);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.PortraitImg)
                    .WithMany(p => p.Author)
                    .HasForeignKey(d => d.PortraitImgId)
                    .HasConstraintName("FK_Author_Image");
            });

            modelBuilder.Entity<AuthorMetaData>(entity =>
            {
                entity.HasIndex(e => e.AuthorId);

                entity.HasIndex(e => new { e.AuthorId, e.MetaKeyId })
                    .HasName("IX_AuthorMetaData");

                entity.Property(e => e.MetaValue).IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.AuthorMetaData)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorMetaData_Author");

                entity.HasOne(d => d.MetaKey)
                    .WithMany(p => p.AuthorMetaData)
                    .HasForeignKey(d => d.MetaKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorMetaData_MetaKey");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.PrimaryAuthorId })
                    .HasName("IX_Blog")
                    .IsUnique();

                entity.Property(e => e.BlogStatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.GUID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlogStatus)
                    .WithMany(p => p.Blog)
                    .HasForeignKey(d => d.BlogStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blog_BlogStatus");

                entity.HasOne(d => d.PrimaryAuthor)
                    .WithMany(p => p.Blog)
                    .HasForeignKey(d => d.PrimaryAuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blog_Author");
            });

            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.HasIndex(e => new { e.BlogId, e.CategoryId })
                    .HasName("IX_BlogCategory")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasIndex(e => new { e.BlogId, e.PostId })
                    .HasName("IX_BlogPost")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogPost)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogPost_Blog");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.BlogPost)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogPost_Post");
            });

            modelBuilder.Entity<BlogStatus>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BlogTag>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.TagOrder).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogTag)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogTag_Blog");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.BlogTag)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogTag_Tag");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<CommentStatus>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_Image_UploaderAuthorId");

                entity.Property(e => e.BlobFullImageUri)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BlobImageThumbnailUri)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BlobUri)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalImageFilename)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.MimeType)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.MimeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_MimeType");

                entity.HasOne(d => d.UploaderAuthor)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.UploaderAuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Author");
            });

            modelBuilder.Entity<LoginProvider>(entity =>
            {
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<MetaKey>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_MetaKey")
                    .IsUnique();

                entity.Property(e => e.DataKey)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MimeType>(entity =>
            {
                entity.HasIndex(e => e.MediaType);

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MediaType)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Suffix)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.CommentCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Excerpt).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(450);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.CommentStatus)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.CommentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_CommentStatus");

                entity.HasOne(d => d.PostStatus)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.PostStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_PostStatus");
            });

            modelBuilder.Entity<PostAuthor>(entity =>
            {
                entity.HasIndex(e => new { e.PostId, e.AuthorId })
                    .HasName("IX_PostAuthor")
                    .IsUnique();

                entity.Property(e => e.IsPrimary)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ListOrder).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.PostAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostAuthor_Author");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostAuthor)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostAuthor_Post");
            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.PostCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostCategory_Category");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostCategory)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostCategory_Post");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.Property(e => e.Comment).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.ApprovedByAuthor)
                    .WithMany(p => p.PostCommentApprovedByAuthor)
                    .HasForeignKey(d => d.ApprovedByAuthorId)
                    .HasConstraintName("FK_PostComment_ApprovingAuthor");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.PostCommentAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostComment_Author");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_PostComment_PostComment");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComment)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostComment_Post");
            });

            modelBuilder.Entity<PostStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.TagOrder).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostTag_Post");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostTag_Tag");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TagGroup>(entity =>
            {
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TagGroupMember>(entity =>
            {
                entity.HasIndex(e => new { e.TagGroupId, e.TagId })
                    .HasName("IX_TagGroupMember");

                entity.Property(e => e.TagOrder).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.TagGroup)
                    .WithMany(p => p.TagGroupMember)
                    .HasForeignKey(d => d.TagGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagGroupMember_TagGroup");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagGroupMember)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagGroupMember_Tag");
            });

            modelBuilder.Entity<Models.ValueType>(entity =>
            {
                entity.Property(e => e.FormatString)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}