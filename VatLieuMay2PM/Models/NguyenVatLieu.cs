namespace VatLieuMay2PM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguyenVatLieu")]
    public partial class NguyenVatLieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguyenVatLieu()
        {
            CTDatHangs = new HashSet<CTDatHang>();
        }

        [Key]
        [StringLength(10)]
        public string MaVatLieu { get; set; }

        [Required]
        [StringLength(50)]
        public string TenVatLieu { get; set; }

        [Required]
        [StringLength(20)]
        public string Loai { get; set; }

        public int SoLuong { get; set; }

        public DateTime NgayNhap { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNCC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDatHang> CTDatHangs { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
