using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace VatLieuMay2PM.Models
{
    public partial class VatLieu2PM_ContextDB : DbContext
    {
        public VatLieu2PM_ContextDB()
            : base("name=VatLieu2PM_ContextDB")
        {
        }

        public virtual DbSet<ChucVu> ChucVus { get; set; }
        public virtual DbSet<CTDatHang> CTDatHangs { get; set; }
        public virtual DbSet<DonDatHang> DonDatHangs { get; set; }
        public virtual DbSet<KhoHang> KhoHangs { get; set; }
        public virtual DbSet<NguyenVatLieu> NguyenVatLieux { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChucVu>()
                .HasMany(e => e.NhanViens)
                .WithRequired(e => e.ChucVu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CTDatHang>()
                .Property(e => e.MaCTDatHang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTDatHang>()
                .Property(e => e.MaDonHang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTDatHang>()
                .Property(e => e.MaVatLieu)
                .IsUnicode(false);

            modelBuilder.Entity<DonDatHang>()
                .Property(e => e.MaDonHang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DonDatHang>()
                .HasMany(e => e.CTDatHangs)
                .WithRequired(e => e.DonDatHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhoHang>()
                .Property(e => e.MaKhoHang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KhoHang>()
                .Property(e => e.DungLuongChua)
                .IsUnicode(false);

            modelBuilder.Entity<NguyenVatLieu>()
                .Property(e => e.MaVatLieu)
                .IsUnicode(false);

            modelBuilder.Entity<NguyenVatLieu>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NguyenVatLieu>()
                .HasMany(e => e.CTDatHangs)
                .WithRequired(e => e.NguyenVatLieu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .HasMany(e => e.NguyenVatLieux)
                .WithRequired(e => e.NhaCungCap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.MaNhanVien)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.TenTaiKhoan)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);
        }
    }
}
