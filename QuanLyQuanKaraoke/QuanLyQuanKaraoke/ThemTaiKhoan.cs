﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyQuanKaraoke
{
    public partial class ThemTaiKhoan : Form
    {
        public ThemTaiKhoan()
        {
            InitializeComponent();
        }
        
        SqlConnection kn = Connection.GetDBConnection();
        QuanLyTaiKhoan qltk = new QuanLyTaiKhoan();

        private void ThemTaiKhoan_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qL_TaiKhoan._QL_TaiKhoan' table. You can move, or remove it, as needed.
            this.qL_TaiKhoanTableAdapter.Fill(this.qL_TaiKhoan._QL_TaiKhoan);
            // TODO: This line of code loads data into the 'qL_TaiKhoan.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Fill(this.qL_TaiKhoan.NhanVien);
            // TODO: This line of code loads data into the 'qL_TaiKhoan.QL_NhomNguoiDung' table. You can move, or remove it, as needed.
            this.qL_NhomNguoiDungTableAdapter.Fill(this.qL_TaiKhoan.QL_NhomNguoiDung);

            ckbHoatDong.Checked = true;
            kn.Open();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // kiểm tra tên đăng nhập
                if (string.IsNullOrEmpty(this.txtten.Text))
                {
                    MessageBox.Show("Không được bỏ trống tên đăng nhập !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.txtten.Focus();
                    return;
                }
                //kiểm tra tính duy nhất
                string sql_ktMaNV = "select QL_TaiKhoan.MaNV from QL_TaiKhoan where MaNV='" + txtmanv.Text + "'";
                SqlCommand cmd = new SqlCommand(sql_ktMaNV, kn);
                SqlDataReader read = cmd.ExecuteReader();
                if (read.Read())
                {
                    MessageBox.Show("Một nhân viên chỉ được tạo một tài khoản !","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    txtten.Clear();
                    txtten.Focus();
                }
                else
                {
                    // Thêm vào bảng tài khoản
                    bool hd;
                    if (ckbHoatDong.Checked == true)
                        hd = true;
                    else
                        hd = false;
                    //cập nhật quyền
                    qltk.CapNhatQuyenTheoNhomND(cbbnhom.SelectedValue.ToString(), txtten.Text);
                    //update tài khoản
                    this.qL_TaiKhoanTableAdapter.Insert(txtten.Text, txtmatkhau.Text, txtmanv.Text, hd,cbbnhom.SelectedValue.ToString());
                    MessageBox.Show("Thêm Tài Khoản Thành Công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên đăng nhập khác.","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtten.Clear();
                txtten.Focus();
            }
        }

        private void ThemTaiKhoan_FormClosing(object sender, FormClosingEventArgs e)
        {
            QuanLyTaiKhoan qltk = new QuanLyTaiKhoan();
            qltk.Show();
            if (kn.State == ConnectionState.Open)
                kn.Close();
        }


    }
}
