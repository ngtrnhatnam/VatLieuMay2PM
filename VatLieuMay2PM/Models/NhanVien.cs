namespace VatLieuMay2PM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [Key]
        [StringLength(6)]
        public string MaNhanVien { get; set; }

        [Required]
        [StringLength(40)]
        public string TenNhanVien { get; set; }

        public short MaChucVu { get; set; }

        [Required]
        [StringLength(12)]
        public string SDT { get; set; }

        [StringLength(40)]
        public string Email { get; set; }

        [Required]
        [StringLength(60)]
        public string DiaChiHoKhau { get; set; }

        public virtual ChucVu ChucVu { get; set; }
    }
}
