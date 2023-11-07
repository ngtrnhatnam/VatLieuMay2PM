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
    public partial class QL_TaiKhoan : Form
    {
        public QL_TaiKhoan()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ");
        }

        private void QL_TaiKhoan_Load(object sender, EventArgs e)
        {
            try
            {
                txtUsername.Focus();
                // Đại diện cho database
                VatLieu2PM_ContextDB context = new VatLieu2PM_ContextDB();

                // Lấy danh sách tài khoản
                List<TaiKhoan> listAccount = context.TaiKhoans.ToList();

                BindGrid(listAccount);
                txtNgayTao.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<TaiKhoan> listAccount)
        {
            // Xoá hết đi
            dgvTaiKhoan.Rows.Clear();
            foreach (var item in listAccount)
            {
                int index = dgvTaiKhoan.Rows.Add();
                dgvTaiKhoan.Rows[index].Cells[0].Value = item.TenTaiKhoan;
                dgvTaiKhoan.Rows[index].Cells[1].Value = item.NgayTao;
                dgvTaiKhoan.Rows[index].Cells[2].Value = item.MatKhau;
            }
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvTaiKhoan.Rows.Count)
                {
                    using (var context = new VatLieu2PM_ContextDB())
                    {
                        txtUsername.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[0].Value.ToString();
                        txtNgayTao.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txtPassword.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[2].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Kiểm tra ID nhân viên
                    if (txtUsername.Text.Length < 4)
                    {
                        MessageBox.Show("Tên tài khoản phải có trên 3 ký tự!");
                        return;
                    }

                    if (txtPassword.Text.Length < 7)
                    {
                        MessageBox.Show("Mật khẩu phải có trên 6 ký tự!");
                        return;
                    }

                    // Tạo mới
                    TaiKhoan taiKhoan = new TaiKhoan();
                    taiKhoan.TenTaiKhoan = txtUsername.Text;
                    taiKhoan.MatKhau = txtPassword.Text;
                    taiKhoan.NgayTao = DateTime.Parse(txtNgayTao.Text);

                    // Thêm xuống vào table
                    context.TaiKhoans.Add(taiKhoan);
                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<TaiKhoan> listAccount = context.TaiKhoans.ToList();
                    BindGrid(listAccount);

                    // Hiển thị rỗng các field
                    EmptyTexbox();

                    // Hiển thị thông báo
                    MessageBox.Show("Thêm mới thành công\nVui lòng ghi nhớ mật khẩu!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EmptyTexbox()
        {
            txtUsername.Text = txtPassword.Text = String.Empty;
            txtNgayTao.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Clear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EmptyTexbox();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return;
                    }

                    // Tìm tài khoản
                    TaiKhoan taiKhoan = context.TaiKhoans.FirstOrDefault(x => x.TenTaiKhoan == txtUsername.Text);
                    if (taiKhoan == null)
                    {
                        MessageBox.Show("Không tìm thấy tài khoản!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var result = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này không?", "Thông tin từ hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes && txtUsername.Text != "admin")
                    {
                        // Xóa nhân viên
                        context.TaiKhoans.Remove(taiKhoan);
                        // Lưu xuống database
                        context.SaveChanges();

                        // Hiển thị lại trên giao diện danh sách
                        List<TaiKhoan> listAccount = context.TaiKhoans.ToList();
                        BindGrid(listAccount);

                        // Hiển thị rỗng các field
                        EmptyTexbox();

                        // Hiển thị thông báo
                        MessageBox.Show("Xóa tài khoản thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtUsername.Text == "admin")
                    {
                        MessageBox.Show("Không thể xóa tài khoản này!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Clear_Click(object sender, EventArgs e)
        {
            EmptyTexbox();
        }
    }
}
