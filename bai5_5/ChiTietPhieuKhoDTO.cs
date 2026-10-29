namespace DTO
{
    public class ChiTietPhieuKhoDTO
    {
        public int MaPhieu { get; set; }
        public string MaVatTu { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}