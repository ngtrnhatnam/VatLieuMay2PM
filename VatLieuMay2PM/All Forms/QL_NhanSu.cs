using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VatLieuMay2PM.Models;

namespace VatLieuMay2PM.All_Forms
{
    public partial class QL_NhanSu : Form
    {
        public QL_NhanSu()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ");
        }

        private void QL_NhanSu_Load(object sender, EventArgs e)
        {
            try
            {
                // Đại diện cho database
                VatLieu2PM_ContextDB context = new VatLieu2PM_ContextDB();

                // Lấy danh sách nhân viên
                List<NhanVien> listNS = context.NhanViens.ToList();
                // Lấy danh sách chức vụ
                List<ChucVu> listPosition = context.ChucVus.ToList();

                FillPositionCombobox(listPosition);
                BindGrid(listNS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<NhanVien> listNS)
        {
            // Xoá hết đi
            dgvNhanSu.Rows.Clear();
            foreach (var item in listNS)
            {
                int index = dgvNhanSu.Rows.Add();
                dgvNhanSu.Rows[index].Cells[0].Value = item.MaNhanVien;
                dgvNhanSu.Rows[index].Cells[1].Value = item.TenNhanVien;
                dgvNhanSu.Rows[index].Cells[2].Value = item.ChucVu.ChucVuNV;
                dgvNhanSu.Rows[index].Cells[3].Value = item.SDT;
                dgvNhanSu.Rows[index].Cells[4].Value = item.Email;
                dgvNhanSu.Rows[index].Cells[5].Value = item.DiaChiHoKhau;
            }
        }

        private void FillPositionCombobox(List<ChucVu> listPosition)
        {
            this.cmbChucVu.DataSource = listPosition;
            this.cmbChucVu.DisplayMember = "ChucVuNV";
            this.cmbChucVu.ValueMember = "MaChucVu";
        }
        private void dgvNhanSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvNhanSu.Rows.Count)
                {
                    using (var context = new VatLieu2PM_ContextDB())
                    {
                        txt_MaNV.Text = dgvNhanSu.Rows[e.RowIndex].Cells[0].Value.ToString();
                        txt_TenNV.Text = dgvNhanSu.Rows[e.RowIndex].Cells[1].Value.ToString();

                        List<ChucVu> listPositions = context.ChucVus.ToList();
                        var position = listPositions.FirstOrDefault(x => x.ChucVuNV == dgvNhanSu.Rows[e.RowIndex].Cells[2].Value.ToString());

                        if (position != null)
                        {
                            cmbChucVu.SelectedValue = position.MaChucVu;
                        }
                        else
                        {
                            cmbChucVu.SelectedIndex = -1; // Bỏ chọn bất kỳ giá trị nào trong ComboBox
                        }

                        txt_SDT.Text = dgvNhanSu.Rows[e.RowIndex].Cells[3].Value.ToString();
                        txt_Email.Text = dgvNhanSu.Rows[e.RowIndex].Cells[4].Value.ToString();
                        txt_DiaChi.Text = dgvNhanSu.Rows[e.RowIndex].Cells[5].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txt_MaNV.Text) || string.IsNullOrEmpty(txt_TenNV.Text) || string.IsNullOrEmpty(txt_DiaChi.Text) ||
                        string.IsNullOrEmpty(txt_Email.Text) || string.IsNullOrEmpty(txt_SDT.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Kiểm tra ID nhân viên
                    if (txt_MaNV.Text.Length != 6)
                    {
                        MessageBox.Show("Mã nhân viên phải có 6 ký tự!");
                        return;
                    }

                    // Tạo mới nhân viên
                    NhanVien nhanvien = new NhanVien();
                    nhanvien.MaNhanVien = txt_MaNV.Text;
                    nhanvien.TenNhanVien = txt_TenNV.Text;
                    nhanvien.MaChucVu = short.Parse(cmbChucVu.SelectedValue.ToString());
                    nhanvien.SDT = txt_SDT.Text;
                    nhanvien.Email = txt_Email.Text;
                    nhanvien.DiaChiHoKhau = txt_DiaChi.Text;

                    // Thêm xuống vào table
                    context.NhanViens.Add(nhanvien);
                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<NhanVien> listNhanVien = context.NhanViens.ToList();
                    BindGrid(listNhanVien);

                    // Hiển thị rỗng các field
                    EmptyTexbox();

                    // Hiển thị thông báo
                    MessageBox.Show("Thêm mới thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EmptyTexbox()
        {
            txt_MaNV.Text = txt_TenNV.Text = txt_DiaChi.Text = txt_Email.Text = txt_SDT.Text = string.Empty;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txt_MaNV.Text) || string.IsNullOrEmpty(txt_TenNV.Text) || string.IsNullOrEmpty(txt_DiaChi.Text) ||
                        string.IsNullOrEmpty(txt_Email.Text) || string.IsNullOrEmpty(txt_SDT.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Tìm nhân viên
                    NhanVien nhanvien = context.NhanViens.FirstOrDefault(x => x.MaNhanVien == txt_MaNV.Text);
                    if (nhanvien == null)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cập nhật thông tin nhân viên
                    nhanvien.MaNhanVien = txt_MaNV.Text;
                    nhanvien.TenNhanVien = txt_TenNV.Text;
                    nhanvien.MaChucVu = short.Parse(cmbChucVu.SelectedValue.ToString());
                    nhanvien.DiaChiHoKhau = txt_DiaChi.Text;
                    nhanvien.SDT = txt_SDT.Text;
                    nhanvien.Email = txt_Email.Text;

                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<NhanVien> listNhanVien = context.NhanViens.ToList();
                    BindGrid(listNhanVien);

                    // Hiển thị rỗng các field
                    EmptyTexbox();

                    // Hiển thị thông báo
                    MessageBox.Show("Cập nhật thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txt_MaNV.Text) || string.IsNullOrEmpty(txt_TenNV.Text) || string.IsNullOrEmpty(txt_DiaChi.Text) ||
                        string.IsNullOrEmpty(txt_Email.Text) || string.IsNullOrEmpty(txt_SDT.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Tìm nhân viên
                    NhanVien nhanvien = context.NhanViens.FirstOrDefault(x => x.MaNhanVien == txt_MaNV.Text);
                    if (nhanvien == null)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Thông tin từ hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Xóa nhân viên
                        context.NhanViens.Remove(nhanvien);
                        // Lưu xuống database
                        context.SaveChanges();

                        // Hiển thị lại trên giao diện danh sách
                        List<NhanVien> listNhanVien = context.NhanViens.ToList();
                        BindGrid(listNhanVien);

                        // Hiển thị rỗng các field
                        EmptyTexbox();

                        // Hiển thị thông báo
                        MessageBox.Show("Xóa nhân viên thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
