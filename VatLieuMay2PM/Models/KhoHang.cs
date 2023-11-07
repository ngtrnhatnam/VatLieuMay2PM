namespace VatLieuMay2PM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhoHang")]
    public partial class KhoHang
    {
        [Key]
        [StringLength(15)]
        public string MaKhoHang { get; set; }

        [Required]
        [StringLength(30)]
        public string TenKhoHang { get; set; }

        [Required]
        [StringLength(50)]
        public string DiaChiKho { get; set; }

        [Required]
        [StringLength(11)]
        public string DungLuongChua { get; set; }
    }
}
