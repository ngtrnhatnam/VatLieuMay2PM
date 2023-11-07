using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VatLieuMay2PM.All_Forms;

namespace VatLieuMay2PM
{
    public partial class Login : Form
    {
        public static Login Instance;
        public TextBox txt;
        public Login()
        {
            InitializeComponent();
            Instance = this;
            txt = txt_Username;
        }
        SqlConnection conn = new SqlConnection(@"Data source=DESKTOP-H7NU3G3\BOOCHAM;initial catalog=QuanLyVatLieu;integrated security=True");

        private void lbl_Exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void lbl_ClearField_MouseHover(object sender, EventArgs e)
        {
            lbl_ClearField.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbl_ClearField_MouseLeave(object sender, EventArgs e)
        {
            lbl_ClearField.BorderStyle = BorderStyle.None;
        }

        private void lbl_Exit_MouseHover(object sender, EventArgs e)
        {
            lbl_Exit.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbl_Exit_MouseLeave(object sender, EventArgs e)
        {
            lbl_Exit.BorderStyle = BorderStyle.None;
        }

        private void lbl_ForgetPassword_MouseHover(object sender, EventArgs e)
        {
            lbl_ForgetPassword.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbl_ForgetPassword_MouseLeave(object sender, EventArgs e)
        {
            lbl_ForgetPassword.BorderStyle = BorderStyle.None;
        }

        private void lbl_ClearField_Click(object sender, EventArgs e)
        {
            txt_Password.Text = txt_Username.Text = String.Empty;
            txt_Username.Focus();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            String username, password;

            username = txt_Username.Text;
            password = txt_Password.Text;

            try
            {
                String querry = "SELECT * FROM TaiKhoan WHERE TenTaiKhoan = '" + txt_Username.Text + "' AND MatKhau = '" + txt_Password.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);

                DataTable dtable = new DataTable();
                sda.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    username = txt_Username.Text;
                    password = txt_Password.Text;

                    Hide();
                    TrangChu home = new TrangChu();
                    home.ShowDialog();
                    home = null;
                    Show();
                    txt_Password.Clear();
                }
                else
                {
                    MessageBox.Show("Thông tin tài khoản hoặc mật khẩu không đúng!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_Password.Clear();

                    txt_Username.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally { conn.Close(); }
        }

        private void lbl_ForgetPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản lý của bạn để lấy lại mật khẩu!");
        }
    }
}
