using System;
using System.Drawing;
using System.Windows.Forms;
using BUS;

namespace GUI_Form
{
    public partial class Form1 : Form
    {
        private InventoryBUS bus = new InventoryBUS();

        private TextBox txtTimKiem;
        private DataGridView dgvVatTu;
        private Label lblTieuDe;

        public Form1()
        {
            InitializeComponent();
            KhoiTaoGiaoDienBangCode();
        }

        private void KhoiTaoGiaoDienBangCode()
        {
            this.Text = "Hệ thống Quản lý Kho (3-Tier)";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTieuDe = new Label();
            lblTieuDe.Text = "TRA CỨU THÔNG TIN VẬT TƯ";
            lblTieuDe.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTieuDe.Location = new Point(30, 20);
            lblTieuDe.AutoSize = true;
            this.Controls.Add(lblTieuDe);

            txtTimKiem = new TextBox();
            txtTimKiem.Location = new Point(30, 60);
            txtTimKiem.Size = new Size(300, 25);
            txtTimKiem.Font = new Font("Arial", 10);
            txtTimKiem.TextChanged += new EventHandler(txtTimKiem_TextChanged);
            this.Controls.Add(txtTimKiem);

            dgvVatTu = new DataGridView();
            dgvVatTu.Location = new Point(30, 100);
            dgvVatTu.Size = new Size(720, 320);
            dgvVatTu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVatTu.ReadOnly = true;
            this.Controls.Add(dgvVatTu);

            LoadDanhSachVatTu("");
        }

        private void LoadDanhSachVatTu(string keyword)
        {
            dgvVatTu.DataSource = bus.TimKiemVatTu(keyword);
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachVatTu(txtTimKiem.Text);
        }
    }
}