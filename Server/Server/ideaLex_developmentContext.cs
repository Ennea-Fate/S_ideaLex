using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Server
{
    public partial class ideaLex_developmentContext : DbContext
    {
        public ideaLex_developmentContext()
        {
        }

        public ideaLex_developmentContext(DbContextOptions<ideaLex_developmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArInternalMetadata> ArInternalMetadata { get; set; }
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<BookEigs> BookEigs { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<ConcordanceBooks> ConcordanceBooks { get; set; }
        public virtual DbSet<Concordances> Concordances { get; set; }
        public virtual DbSet<EigStatusVotes> EigStatusVotes { get; set; }
        public virtual DbSet<Ethnoidioglosses> Ethnoidioglosses { get; set; }
        public virtual DbSet<FrequencyVocabularies> FrequencyVocabularies { get; set; }
        public virtual DbSet<GrammarDictionaries> GrammarDictionaries { get; set; }
        public virtual DbSet<Notebooks> Notebooks { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SchemaMigrations> SchemaMigrations { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ideaLex_development;Username=postgres;Password=KSM14*ort=pg");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArInternalMetadata>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("ar_internal_metadata_pkey");

                entity.ToTable("ar_internal_metadata");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Authors>(entity =>
            {
                entity.ToTable("authors");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.SecondName)
                    .HasColumnName("second_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");
            });

            modelBuilder.Entity<BookEigs>(entity =>
            {
                entity.ToTable("book_eigs");

                entity.HasIndex(e => e.BookId)
                    .HasName("index_book_eigs_on_book_id");

                entity.HasIndex(e => e.EthnoidioglossId)
                    .HasName("index_book_eigs_on_ethnoidiogloss_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.EthnoidioglossId).HasColumnName("ethnoidiogloss_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookEigs)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_332c5ecf80");

                entity.HasOne(d => d.Ethnoidiogloss)
                    .WithMany(p => p.BookEigs)
                    .HasForeignKey(d => d.EthnoidioglossId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_f64190e94c");
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("books");

                entity.HasIndex(e => e.AuthorId)
                    .HasName("index_books_on_author_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_53d51ce16a");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments");

                entity.HasIndex(e => e.EthnoidioglossId)
                    .HasName("index_comments_on_ethnoidiogloss_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("index_comments_on_user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.EthnoidioglossId).HasColumnName("ethnoidiogloss_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Ethnoidiogloss)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.EthnoidioglossId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_daf17b5a1b");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_03de2dc08c");
            });

            modelBuilder.Entity<ConcordanceBooks>(entity =>
            {
                entity.ToTable("concordance_books");

                entity.HasIndex(e => e.BookId)
                    .HasName("index_concordance_books_on_book_id");

                entity.HasIndex(e => e.ConcordanceId)
                    .HasName("index_concordance_books_on_concordance_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.ConcordanceId).HasColumnName("concordance_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ConcordanceBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_466bf1f453");

                entity.HasOne(d => d.Concordance)
                    .WithMany(p => p.ConcordanceBooks)
                    .HasForeignKey(d => d.ConcordanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_c365368c19");
            });

            modelBuilder.Entity<Concordances>(entity =>
            {
                entity.ToTable("concordances");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Concordance).HasColumnName("concordance");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.IsItLemma).HasColumnName("is_it_lemma");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Width).HasColumnName("width");
            });

            modelBuilder.Entity<EigStatusVotes>(entity =>
            {
                entity.ToTable("eig_status_votes");

                entity.HasIndex(e => e.EthnoidioglossId)
                    .HasName("index_eig_status_votes_on_ethnoidiogloss_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("index_eig_status_votes_on_user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.EthnoidioglossId).HasColumnName("ethnoidiogloss_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Ethnoidiogloss)
                    .WithMany(p => p.EigStatusVotes)
                    .HasForeignKey(d => d.EthnoidioglossId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_6930277e57");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EigStatusVotes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_4310e5cef3");
            });

            modelBuilder.Entity<Ethnoidioglosses>(entity =>
            {
                entity.ToTable("ethnoidioglosses");

                entity.HasIndex(e => e.AuthorId)
                    .HasName("index_ethnoidioglosses_on_author_id");

                entity.HasIndex(e => e.BookId)
                    .HasName("index_ethnoidioglosses_on_book_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("index_ethnoidioglosses_on_user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Word)
                    .HasColumnName("word")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Ethnoidioglosses)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_98b4ffac92");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Ethnoidioglosses)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_68f0e347a8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ethnoidioglosses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_a0f90da8b6");
            });

            modelBuilder.Entity<FrequencyVocabularies>(entity =>
            {
                entity.ToTable("frequency_vocabularies");

                entity.HasIndex(e => e.BookId)
                    .HasName("index_frequency_vocabularies_on_book_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.FrequencyVocabularies)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_5867e9931e");
            });

            modelBuilder.Entity<GrammarDictionaries>(entity =>
            {
                entity.ToTable("grammar_dictionaries");

                entity.HasIndex(e => e.BookId)
                    .HasName("index_grammar_dictionaries_on_book_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Dictionary).HasColumnName("dictionary");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.GrammarDictionaries)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_b32b97d8e8");
            });

            modelBuilder.Entity<Notebooks>(entity =>
            {
                entity.ToTable("notebooks");

                entity.HasIndex(e => e.UserId)
                    .HasName("index_notebooks_on_user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notebooks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_d5906d44d9");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.RightsMask)
                    .HasColumnName("rights_mask")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");
            });

            modelBuilder.Entity<SchemaMigrations>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("schema_migrations_pkey");

                entity.ToTable("schema_migrations");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.RoleId)
                    .HasName("index_users_on_role_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SecondName)
                    .HasColumnName("second_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rails_642f17018b");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
