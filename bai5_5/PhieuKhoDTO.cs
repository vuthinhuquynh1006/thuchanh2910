using System;
using System.Collections.Generic;

namespace DTO
{
    public class PhieuKhoDTO
    {
        public int MaPhieu { get; set; }
        public string LoaiPhieu { get; set; } // "NHAP" hoặc "XUAT"
        public DateTime NgayLap { get; set; }
        public int? MaNCC { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietPhieuKhoDTO> ChiTietList { get; set; } = new List<ChiTietPhieuKhoDTO>();
    }
}