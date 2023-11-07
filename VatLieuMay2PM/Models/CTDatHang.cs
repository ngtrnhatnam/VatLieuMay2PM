namespace VatLieuMay2PM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTDatHang")]
    public partial class CTDatHang
    {
        [Key]
        [StringLength(8)]
        public string MaCTDatHang { get; set; }

        [Required]
        [StringLength(12)]
        public string MaDonHang { get; set; }

        [Required]
        [StringLength(10)]
        public string MaVatLieu { get; set; }

        public int SoLuong { get; set; }

        public virtual DonDatHang DonDatHang { get; set; }

        public virtual NguyenVatLieu NguyenVatLieu { get; set; }
    }
}
