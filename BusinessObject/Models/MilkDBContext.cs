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
        public virtual DbSet<TblCategory> TblCategories { get; set; } = null!;
        public virtual DbSet<TblCollection> TblCollections { get; set; } = null!;
        public virtual DbSet<TblDiscountCode> TblDiscountCodes { get; set; } = null!;
        public virtual DbSet<TblDiscountOfAccount> TblDiscountOfAccounts { get; set; } = null!;
        public virtual DbSet<TblMemberPoint> TblMemberPoints { get; set; } = null!;
        public virtual DbSet<TblOrder> TblOrders { get; set; } = null!;
        public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; } = null!;
        public virtual DbSet<TblPackage> TblPackages { get; set; } = null!;
        public virtual DbSet<TblPackageSetting> TblPackageSettings { get; set; } = null!;
        public virtual DbSet<TblProduct> TblProducts { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblSlotShip> TblSlotShips { get; set; } = null!;
        public virtual DbSet<TblStation> TblStations { get; set; } = null!;
        public virtual DbSet<TblStatusByOrder> TblStatusByOrders { get; set; } = null!;
        public virtual DbSet<TblSubscriptionDay> TblSubscriptionDays { get; set; } = null!;
        public virtual DbSet<TblVerificationCode> TblVerificationCodes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=USER88\\SQLEXPRESS;TrustServerCertificate=True;Database=Milk_System;Uid=sa;password=123;Integrated Security=True");
                //optionsBuilder.UseSqlServer("Server=DESKTOP-5FOPUL4.\\SQLEXPRESS;TrustServerCertificate=True;Database=Milk_System;Uid=sa;password=1;Integrated Security=True");
                //optionsBuilder.UseSqlServer("workstation id = Milk-DB.mssql.somee.com; packet size = 4096; user id = tiensidiien_SQLLogin_1; pwd = uaeovuatgl; data source = Milk-DB.mssql.somee.com; persist security info = False; initial catalog = Milk-DB ; Encrypt=false;TrustServerCertificate=true");
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

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.HashedPassword).HasMaxLength(100);

                entity.Property(e => e.ImageCard).HasMaxLength(100);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsVerified).HasColumnName("isVerified");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SaltPassword).HasMaxLength(100);

                entity.Property(e => e.StationId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("StationID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccount_tblRole");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("FK_tblAccount_tblStation");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Image)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<TblCollection>(entity =>
            {
                entity.ToTable("tblCollection");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderDetailId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OrderDetailID");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ProductID");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.TblCollections)
                    .HasForeignKey(d => d.OrderDetailId)
                    .HasConstraintName("FK_tblCollection_tblOrderDetail");
            });

            modelBuilder.Entity<TblDiscountCode>(entity =>
            {
                entity.ToTable("tblDiscountCode");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CodeDescription).HasMaxLength(200);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            });

            modelBuilder.Entity<TblDiscountOfAccount>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("tblDiscountOfAccount");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.BeginTime).HasColumnType("date");

                entity.Property(e => e.DiscountCodeId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblDiscountOfAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDiscountOfAccount_tblAccount");

                entity.HasOne(d => d.DiscountCode)
                    .WithMany(p => p.TblDiscountOfAccounts)
                    .HasForeignKey(d => d.DiscountCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDiscountOfAccount_tblDiscountCode");
            });

            modelBuilder.Entity<TblMemberPoint>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("tblMemberPoint");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.TblMemberPoint)
                    .HasForeignKey<TblMemberPoint>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMemberPoint_tblAccount");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.AdditionalRequest).HasMaxLength(10);

                entity.Property(e => e.Address).HasMaxLength(9);

                entity.Property(e => e.DiscountCodeId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DiscountCodeID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.OrderDetailD)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PackageId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PackageID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StationId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("StationID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrder_tblAccount");

                entity.HasOne(d => d.DiscountCode)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.DiscountCodeId)
                    .HasConstraintName("FK_tblOrder_tblDiscountCode");

                entity.HasOne(d => d.OrderDetailDNavigation)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.OrderDetailD)
                    .HasConstraintName("FK_tblOrder_tblOrderDetail");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrder_tblPackage");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrder_tblStation");
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.ToTable("tblOrderDetail");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DateOrder).HasColumnType("date");

                entity.Property(e => e.SlotShipId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SlotShipID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.SlotShip)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.SlotShipId)
                    .HasConstraintName("FK_tblOrderDetail_tblSlotShip");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrderDetail_StatusByOrder");
            });

            modelBuilder.Entity<TblPackage>(entity =>
            {
                entity.ToTable("tblPackage");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Image)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasMany(d => d.SubscriptionDays)
                    .WithMany(p => p.Packages)
                    .UsingEntity<Dictionary<string, object>>(
                        "TblPackageSubscriptionDay",
                        l => l.HasOne<TblSubscriptionDay>().WithMany().HasForeignKey("SubscriptionDayId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPackageSubscriptionDay_tblSubscriptionDay"),
                        r => r.HasOne<TblPackage>().WithMany().HasForeignKey("PackageId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPackageSubscriptionDay_tblPackage"),
                        j =>
                        {
                            j.HasKey("PackageId", "SubscriptionDayId");

                            j.ToTable("tblPackageSubscriptionDay");

                            j.IndexerProperty<string>("PackageId").HasMaxLength(50).IsUnicode(false).HasColumnName("PackageID");

                            j.IndexerProperty<string>("SubscriptionDayId").HasMaxLength(50).IsUnicode(false).HasColumnName("SubscriptionDayID");
                        });
            });

            modelBuilder.Entity<TblPackageSetting>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.CategoryId });

                entity.ToTable("tblPackageSetting");

                entity.Property(e => e.PackageId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PackageID");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CategoryID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblPackageSettings)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPackageSetting_tblCategory");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.TblPackageSettings)
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPackageSetting_tblPackage");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tblProduct");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Image)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(200);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduct_tblCategory");
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

            modelBuilder.Entity<TblSlotShip>(entity =>
            {
                entity.ToTable("tblSlotShip");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.SlotDescription).HasMaxLength(200);

                entity.Property(e => e.SlotTitle).HasMaxLength(50);
            });

            modelBuilder.Entity<TblStation>(entity =>
            {
                entity.ToTable("tblStation");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.StationAddress).HasMaxLength(200);

                entity.Property(e => e.StationDescription).HasMaxLength(200);

                entity.Property(e => e.StationName).HasMaxLength(200);
            });

            modelBuilder.Entity<TblStatusByOrder>(entity =>
            {
                entity.ToTable("tblStatusByOrder");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<TblSubscriptionDay>(entity =>
            {
                entity.ToTable("tblSubscriptionDay");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(200);
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

                entity.Property(e => e.ExpirationTime).HasColumnType("date");

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
