using System;
using Microsoft.Data.SqlClient;

using DTO;

namespace DAL
{
    public class PhieuKhoDAL
    {
        public bool LuuPhieuKho(PhieuKhoDTO phieu, out string errorMessage)
        {
            errorMessage = string.Empty;

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction(); // Bắt đầu Transaction

                try
                {
                    // 1. Thêm vào bảng PhieuKho
                    string queryPhieu = @"INSERT INTO PhieuKho (LoaiPhieu, NgayLap, MaNCC, TongTien) 
                                         VALUES (@LoaiPhieu, @NgayLap, @MaNCC, @TongTien);
                                         SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdPhieu = new SqlCommand(queryPhieu, conn, trans);
                    cmdPhieu.Parameters.AddWithValue("@LoaiPhieu", phieu.LoaiPhieu);
                    cmdPhieu.Parameters.AddWithValue("@NgayLap", phieu.NgayLap);
                    cmdPhieu.Parameters.AddWithValue("@MaNCC", (object)phieu.MaNCC ?? DBNull.Value);
                    cmdPhieu.Parameters.AddWithValue("@TongTien", phieu.TongTien);

                    int maPhieuGenerated = Convert.ToInt32(cmdPhieu.ExecuteScalar());

                    // 2. Thêm vào ChiTietPhieuKho và cập nhật tồn kho
                    foreach (var item in phieu.ChiTietList)
                    {
                        string queryChiTiet = @"INSERT INTO ChiTietPhieuKho (MaPhieu, MaVatTu, SoLuong, DonGia) 
                                               VALUES (@MaPhieu, @MaVatTu, @SoLuong, @DonGia);";
                        SqlCommand cmdChiTiet = new SqlCommand(queryChiTiet, conn, trans);
                        cmdChiTiet.Parameters.AddWithValue("@MaPhieu", maPhieuGenerated);
                        cmdChiTiet.Parameters.AddWithValue("@MaVatTu", item.MaVatTu);
                        cmdChiTiet.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                        cmdChiTiet.Parameters.AddWithValue("@DonGia", item.DonGia);
                        cmdChiTiet.ExecuteNonQuery();

                        if (phieu.LoaiPhieu == "XUAT")
                        {
                            // Kiểm tra tồn kho trước khi xuất
                            string queryCheck = "SELECT SoLuongTon FROM VatTu WHERE MaVatTu = @MaVatTu";
                            SqlCommand cmdCheck = new SqlCommand(queryCheck, conn, trans);
                            cmdCheck.Parameters.AddWithValue("@MaVatTu", item.MaVatTu);
                            decimal tonKho = Convert.ToDecimal(cmdCheck.ExecuteScalar());

                            if (tonKho < item.SoLuong)
                            {
                                trans.Rollback(); // Không đủ hàng -> Hủy giao dịch lập tức
                                errorMessage = $"Không đủ hàng trong kho cho vật tư {item.MaVatTu}. Tồn hiện tại: {tonKho}";
                                return false;
                            }

                            string queryTru = "UPDATE VatTu SET SoLuongTon = SoLuongTon - @SoLuong WHERE MaVatTu = @MaVatTu";
                            SqlCommand cmdTru = new SqlCommand(queryTru, conn, trans);
                            cmdTru.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                            cmdTru.Parameters.AddWithValue("@MaVatTu", item.MaVatTu);
                            cmdTru.ExecuteNonQuery();
                        }
                        else if (phieu.LoaiPhieu == "NHAP")
                        {
                            string queryCong = "UPDATE VatTu SET SoLuongTon = SoLuongTon + @SoLuong WHERE MaVatTu = @MaVatTu";
                            SqlCommand cmdCong = new SqlCommand(queryCong, conn, trans);
                            cmdCong.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                            cmdCong.Parameters.AddWithValue("@MaVatTu", item.MaVatTu);
                            cmdCong.ExecuteNonQuery();
                        }
                    }

                    trans.Commit(); // Thành công -> Lưu dữ liệu chính thức
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback(); // Lỗi bất ngờ -> Hoàn tác toàn bộ
                    errorMessage = "Lỗi hệ thống: " + ex.Message;
                    return false;
                }
            }
        }
    }
}