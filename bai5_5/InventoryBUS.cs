using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class InventoryBUS
    {
        private VatTuDAL vatTuDAL = new VatTuDAL();
        private PhieuKhoDAL phieuKhoDAL = new PhieuKhoDAL();

        // Nghiệp vụ tìm kiếm vật tư an toàn
        public List<VatTuDTO> TimKiemVatTu(string keyword)
        {
            return vatTuDAL.TimKiemVatTu(keyword ?? string.Empty);
        }

        // Nghiệp vụ kiểm tra và lưu phiếu kho
        public bool ThucHienLuuPhieu(PhieuKhoDTO phieu, out string msg)
        {
            if (phieu.ChiTietList == null || phieu.ChiTietList.Count == 0)
            {
                msg = "Phiếu kho phải có ít nhất một dòng vật tư chi tiết!";
                return false;
            }

            if (phieu.LoaiPhieu == "NHAP" && !phieu.MaNCC.HasValue)
            {
                msg = "Phiếu nhập bắt buộc phải chọn Nhà Cung Cấp!";
                return false;
            }

            // Gọi xuống DAL xử lý giao dịch Transaction (ACID)
            return phieuKhoDAL.LuuPhieuKho(phieu, out msg);
        }
    }
}