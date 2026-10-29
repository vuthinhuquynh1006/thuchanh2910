using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

using DTO;

namespace DAL
{
    public class VatTuDAL
    {
        public List<VatTuDTO> TimKiemVatTu(string keyword)
        {
            List<VatTuDTO> list = new List<VatTuDTO>();
            string query = "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu WHERE MaVatTu LIKE @kw OR TenVatTu LIKE @kw";

            using (SqlConnection conn = Database.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                // Dùng SqlParameter phòng chống SQL Injection tuyệt đối
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new VatTuDTO
                    {
                        MaVatTu = reader["MaVatTu"].ToString(),
                        TenVatTu = reader["TenVatTu"].ToString(),
                        DVT = reader["DVT"].ToString(),
                        SoLuongTon = Convert.ToDecimal(reader["SoLuongTon"])
                    });
                }
            }
            return list;
        }
    }
}