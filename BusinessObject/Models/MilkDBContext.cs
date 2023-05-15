using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObject.Models
{
    public partial class MilkDBContext : DbContext
    {
        public MilkDBContext()
        {
        }

        public MilkDBContext(DbContextOptions<MilkDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAccount> TblAccounts { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblVerificationCode> TblVerificationCodes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=USER88\\SQLEXPRESS;TrustServerCertificate=True;Database=Milk_System;Uid=sa;password=123;Integrated Security=True");
                //optionsBuilder.UseSqlServer("Server=DESKTOP-5FOPUL4.\\SQLEXPRESS;TrustServerCertificate=True;Database=Milk_System;Uid=sa;password=1;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.ToTable("tblAccount");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.HashedPassword).HasMaxLength(100);

                entity.Property(e => e.ImageCard).HasMaxLength(100);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsVerified).HasColumnName("isVerified");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SaltPassword).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccount_tblRole");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TblVerificationCode>(entity =>
            {
                entity.ToTable("tblVerificationCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.ExpirationTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblVerificationCodes)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVerificationCode_tblAccount");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
