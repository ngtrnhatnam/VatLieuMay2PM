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
    public partial class KhoHang : Form
    {
        public KhoHang()
        {
            InitializeComponent();
        }

        private void KhoHang_Load(object sender, EventArgs e)
        {
            if (Login.Instance.txt.Text != "admin")
                txtMaVL.Enabled = txtTenVL.Enabled = txtSoLuong.Enabled = txtLoai.Enabled = txtNgayNhap.Enabled =
                cmbMaNCC.Enabled = btnAdd.Enabled = btnDelete.Enabled = btnEdit.Enabled = false;
            txtNgayNhap.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt");
            cmb_Find.SelectedIndex = 0;
            try
            {
                // Đại diện cho database
                VatLieu2PM_ContextDB context = new VatLieu2PM_ContextDB();

                // Lấy danh sách vật liệu
                List<NguyenVatLieu> listVatLieu = context.NguyenVatLieux.ToList();
                List<NhaCungCap> listNCC = context.NhaCungCaps.ToList();

                BindGrid(listVatLieu);
                FillNCCCombobox(listNCC);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillNCCCombobox(List<NhaCungCap> listNCC)
        {
            cmbMaNCC.DataSource = listNCC;
            cmbMaNCC.DisplayMember = "TenNCC";
            cmbMaNCC.ValueMember = "MaNCC";
        }

        private void BindGrid(List<NguyenVatLieu> listVatLieu)
        {
            // Xoá hết đi
            dgvKho.Rows.Clear();
            foreach (var item in listVatLieu)
            {
                int index = dgvKho.Rows.Add();
                dgvKho.Rows[index].Cells[0].Value = item.MaVatLieu;
                dgvKho.Rows[index].Cells[1].Value = item.TenVatLieu;
                dgvKho.Rows[index].Cells[2].Value = item.Loai;
                dgvKho.Rows[index].Cells[3].Value = item.SoLuong;
                dgvKho.Rows[index].Cells[4].Value = item.NgayNhap;
                dgvKho.Rows[index].Cells[5].Value = item.NhaCungCap.TenNCC;
            }
        }

        private void dgvKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvKho.Rows.Count)
                {
                    using (var context = new VatLieu2PM_ContextDB())
                    {
                        txtMaVL.Text = dgvKho.Rows[e.RowIndex].Cells[0].Value.ToString();
                        txtTenVL.Text = dgvKho.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txtLoai.Text = dgvKho.Rows[e.RowIndex].Cells[2].Value.ToString();
                        txtSoLuong.Text = dgvKho.Rows[e.RowIndex].Cells[3].Value.ToString();
                        txtNgayNhap.Text = dgvKho.Rows[e.RowIndex].Cells[4].Value.ToString();

                        List<NhaCungCap> listNCC = context.NhaCungCaps.ToList();
                        var position = listNCC.FirstOrDefault(x => x.TenNCC == dgvKho.Rows[e.RowIndex].Cells[5].Value.ToString());

                        if (position != null)
                        {
                            cmbMaNCC.SelectedValue = position.MaNCC;
                        }
                        else
                        {
                            cmbMaNCC.SelectedIndex = -1; // Bỏ chọn bất kỳ giá trị nào trong ComboBox

                        }
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
                    if (string.IsNullOrEmpty(txtMaVL.Text) || string.IsNullOrEmpty(txtTenVL.Text) ||
                        string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtLoai.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Kiểm tra ID vật liệu
                    if (txtMaVL.Text.Length > 10)
                    {
                        MessageBox.Show("Mã vật liệu phải có dưới 10 ký tự!");
                        return;
                    }

                    // Thêm vật liệu mới
                    NguyenVatLieu vatLieu = new NguyenVatLieu();
                    vatLieu.MaVatLieu = txtMaVL.Text;
                    vatLieu.TenVatLieu = txtTenVL.Text;
                    vatLieu.SoLuong = int.Parse(txtSoLuong.Text);
                    vatLieu.Loai = txtLoai.Text;
                    vatLieu.NgayNhap = DateTime.Now;
                    vatLieu.MaNCC = cmbMaNCC.SelectedValue.ToString();

                    // Thêm xuống vào table
                    context.NguyenVatLieux.Add(vatLieu);
                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<NguyenVatLieu> listVatLieu = context.NguyenVatLieux.ToList();
                    BindGrid(listVatLieu);

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
            txtLoai.Text = txtMaVL.Text = txtSoLuong.Text = txtTenVL.Text = string.Empty;
            txtNgayNhap.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txtMaVL.Text) || string.IsNullOrEmpty(txtTenVL.Text) ||
                        string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtLoai.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Tìm vật liệu
                    NguyenVatLieu vatLieu = context.NguyenVatLieux.FirstOrDefault(x => x.MaVatLieu == txtMaVL.Text);
                    if (vatLieu == null)
                    {
                        MessageBox.Show("Không tìm thấy nguyên vật liệu!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var result = MessageBox.Show("Bạn có chắc muốn xóa nguyên vật liệu này không?", "Thông tin từ hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Xóa vật liệu
                        context.NguyenVatLieux.Remove(vatLieu);
                        // Lưu xuống database
                        context.SaveChanges();

                        // Hiển thị lại trên giao diện danh sách
                        List<NguyenVatLieu> listVatLieu = context.NguyenVatLieux.ToList();
                        BindGrid(listVatLieu);

                        // Hiển thị rỗng các field
                        EmptyTexbox();

                        // Hiển thị thông báo
                        MessageBox.Show("Xóa nguyên vật liệu thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txtMaVL.Text) || string.IsNullOrEmpty(txtTenVL.Text) ||
                        string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtLoai.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Tìm vật liệu
                    NguyenVatLieu vatLieu = context.NguyenVatLieux.FirstOrDefault(x => x.MaVatLieu == txtMaVL.Text);
                    if (vatLieu == null)
                    {
                        MessageBox.Show("Không tìm thấy nguyên vật liệu!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cập nhật thông tin nguyên vật liệu
                    vatLieu.MaVatLieu = txtMaVL.Text;
                    vatLieu.TenVatLieu = txtTenVL.Text;
                    vatLieu.SoLuong = int.Parse(txtSoLuong.Text);
                    vatLieu.Loai = txtLoai.Text;
                    vatLieu.MaNCC = cmbMaNCC.SelectedValue.ToString();

                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<NguyenVatLieu> listVatLieu = context.NguyenVatLieux.ToList();
                    BindGrid(listVatLieu);

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

        private void Clear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EmptyTexbox();
            cmbMaNCC.SelectedIndex = -1; // Bỏ chọn bất kỳ giá trị nào trong ComboBox
            txtNgayNhap.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt");
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            dgvKho.ClearSelection();
            int rowIndex = -1;
            if (cmb_Find.SelectedIndex == 0)
            {
                try
                {
                    DataGridViewRow row = dgvKho.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals(txt_Find.Text)).FirstOrDefault();
                    rowIndex = row.Index;
                    dgvKho.Rows[rowIndex].Selected = true;
                }
                catch
                {
                    MessageBox.Show("Không thể tìm thấy, vui lòng kiểu tra lại");
                }
            }
            else if (cmb_Find.SelectedIndex == 1)
            {
                try
                {
                    DataGridViewRow row = dgvKho.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[1].Value.ToString().Equals(txt_Find.Text)).FirstOrDefault();
                    rowIndex = row.Index;
                    dgvKho.Rows[rowIndex].Selected = true;
                }
                catch
                {
                    MessageBox.Show("Không thể tìm thấy, vui lòng kiểu tra lại");
                }
            }
        }
    }
}

