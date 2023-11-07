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
    public partial class QL_NhaCungCap : Form
    {
        public QL_NhaCungCap()
        {
            InitializeComponent();
        }

        private void QL_NhaCungCap_Load(object sender, EventArgs e)
        {
            try
            {
                // Đại diện cho database
                VatLieu2PM_ContextDB context = new VatLieu2PM_ContextDB();

                // Lấy danh sách nhà cung cấp
                List<NhaCungCap> listNCC = context.NhaCungCaps.ToList();

                BindGrid(listNCC);
                txt_MaNCC.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<NhaCungCap> listNCC)
        {
            // Xoá hết đi
            dgvNCC.Rows.Clear();
            foreach (var item in listNCC)
            {
                int index = dgvNCC.Rows.Add();
                dgvNCC.Rows[index].Cells[0].Value = item.MaNCC;
                dgvNCC.Rows[index].Cells[1].Value = item.TenNCC;
                dgvNCC.Rows[index].Cells[2].Value = item.DiaChi;
                dgvNCC.Rows[index].Cells[3].Value = item.SDT;
                dgvNCC.Rows[index].Cells[4].Value = item.Email;
            }
        }

        private void EmptyTexbox()
        {
            txt_MaNCC.Text = txt_TenNCC.Text = txt_DiaChi.Text = txt_Email.Text = txt_SDT.Text = string.Empty;
        }

        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvNCC.Rows.Count)
                {
                    using (var context = new VatLieu2PM_ContextDB())
                    {
                        txt_MaNCC.Text = dgvNCC.Rows[e.RowIndex].Cells[0].Value.ToString();
                        txt_TenNCC.Text = dgvNCC.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txt_DiaChi.Text = dgvNCC.Rows[e.RowIndex].Cells[2].Value.ToString();
                        if (dgvNCC.Rows[e.RowIndex].Cells[3].Value == null)
                            txt_SDT.Text = String.Empty;
                        else txt_SDT.Text = dgvNCC.Rows[e.RowIndex].Cells[3].Value.ToString();
                        txt_Email.Text = dgvNCC.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txt_MaNCC.Text) || string.IsNullOrEmpty(txt_TenNCC.Text) ||
                        string.IsNullOrEmpty(txt_DiaChi.Text) || (string.IsNullOrEmpty(txt_Email.Text)))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin! Có thể để trống SDT");
                        return;
                    }

                    // Kiểm tra ID nhà cung cấp
                    if (txt_MaNCC.Text.Length != 10)
                    {
                        MessageBox.Show("Mã nhà cung cấp phải có 10 ký tự!");
                        return;
                    }

                    // Tạo mới nhà cung cấp
                    NhaCungCap ncc = new NhaCungCap();
                    ncc.MaNCC = txt_MaNCC.Text;
                    ncc.TenNCC = txt_TenNCC.Text;
                    ncc.DiaChi = txt_DiaChi.Text;
                    ncc.SDT = txt_SDT.Text;
                    ncc.Email = txt_Email.Text;

                    // Thêm xuống vào table
                    context.NhaCungCaps.Add(ncc);
                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<NhaCungCap> listNCC = context.NhaCungCaps.ToList();
                    BindGrid(listNCC);

                    // Hiển thị rỗng các field
                    EmptyTexbox();

                    // Hiển thị thông báo
                    MessageBox.Show("Thêm mới thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_MaNCC.Focus();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txt_MaNCC.Text) || string.IsNullOrEmpty(txt_TenNCC.Text) ||
                        string.IsNullOrEmpty(txt_DiaChi.Text) || (string.IsNullOrEmpty(txt_Email.Text)))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin! Có thể để trống SDT");
                        return;
                    }

                    // Tìm nhà cung cấp 
                    NhaCungCap ncc = context.NhaCungCaps.FirstOrDefault(x => x.MaNCC == txt_MaNCC.Text);
                    if (ncc == null)
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var result = MessageBox.Show("Bạn có chắc muốn xóa nhà cung cấp này không?", "Thông tin từ hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Xóa nhà cung cấp
                        context.NhaCungCaps.Remove(ncc);
                        // Lưu xuống database
                        context.SaveChanges();

                        // Hiển thị lại trên giao diện danh sách
                        List<NhaCungCap> listNCC = context.NhaCungCaps.ToList();
                        BindGrid(listNCC);

                        // Hiển thị rỗng các field
                        EmptyTexbox();

                        // Hiển thị thông báo
                        MessageBox.Show("Xóa nhà cung cấp thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_MaNCC.Focus();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new VatLieu2PM_ContextDB())
                {
                    // Kiểm tra nhập thiếu thông tin
                    if (string.IsNullOrEmpty(txt_MaNCC.Text) || string.IsNullOrEmpty(txt_TenNCC.Text) ||
                        string.IsNullOrEmpty(txt_DiaChi.Text) || (string.IsNullOrEmpty(txt_Email.Text)))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin! Có thể để trống SDT");
                        return;
                    }

                    // Tìm nhà cung cấp
                    NhaCungCap ncc = context.NhaCungCaps.FirstOrDefault(x => x.MaNCC == txt_MaNCC.Text);
                    if (ncc == null)
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp!", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cập nhật thông tin nhà cung cấp
                    ncc.MaNCC = txt_MaNCC.Text;
                    ncc.TenNCC = txt_TenNCC.Text;
                    ncc.DiaChi = txt_DiaChi.Text;
                    ncc.SDT = txt_SDT.Text;
                    ncc.Email = txt_Email.Text;

                    // Lưu xuống database
                    context.SaveChanges();

                    // Hiển thị lại trên giao diện danh sách
                    List<NhaCungCap> listNCC = context.NhaCungCaps.ToList();
                    BindGrid(listNCC);

                    // Hiển thị rỗng các field
                    EmptyTexbox();

                    // Hiển thị thông báo
                    MessageBox.Show("Cập nhật thành công", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_MaNCC.Focus();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng thử lại", "Thông tin từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ");
        }
    }
}
