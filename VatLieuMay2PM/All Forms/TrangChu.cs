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
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
            if (Login.Instance.txt.Text != "admin")
                tool_QuanLy.Enabled = false;
            lbl_Welcome.Text = "Bạn đang đăng nhập với tư cách " + Login.Instance.txt.Text;
        }

        private void tool_DangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
                MessageBox.Show("Đã đăng xuất!");
                Login.Instance.txt.Text = String.Empty;
                Login.Instance.txt.Focus();
            }
        }

        private void tool_Thoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        private void tool_NhaCungCap_Click(object sender, EventArgs e)
        {
            Hide();
            QL_NhaCungCap nhaCungCap = new QL_NhaCungCap();
            nhaCungCap.ShowDialog();
            nhaCungCap = null;
            Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ");
        }

        private void tool_KhoHang_Click(object sender, EventArgs e)
        {
            Hide();
            KhoHang khoHang = new KhoHang();
            khoHang.ShowDialog();
            khoHang = null;
            Show();
        }

        private void tool_NhanSu_Click(object sender, EventArgs e)
        {
            Hide();
            QL_NhanSu nhanSu = new QL_NhanSu();
            nhanSu.ShowDialog();
            nhanSu = null;
            Show();
        }

        private void tool_TaiKhoan_Click(object sender, EventArgs e)
        {
            Hide();
            QL_TaiKhoan taiKhoan = new QL_TaiKhoan();
            taiKhoan.ShowDialog();
            taiKhoan = null;
            Show();
        }
    }
}
