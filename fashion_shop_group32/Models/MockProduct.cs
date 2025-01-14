﻿using Databases;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace fashion_shop_group32.Models
{
    public class MockProduct : IProduct
    {
        private List<Product> _productList;
        public MockProduct()
        {
            _productList = new List<Product>();

        }
        public Boolean isContain(string name)
        {
            for (int i = 0; i < _productList.Count; i++)
            {
                if (_productList[i].ten_sp == name) return true;
            }
            return false;
        }
        public Boolean isContainString(List<string> list, string s)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == s) return true;
            }
            return false;
        }

        public int NumberProductinList(string cat, string loai, string mau, string size, string gia, string keyword)
        {
            int count = 0;
            string filterNew = "";
            string filterQuery = "";
            if (mau != null && mau != "")
            {
                filterQuery += ("and c.ma_mausp='" + mau + "' ");
            }
            if (size != null && size != "")
            {
                filterQuery += ("and d.ma_sizesp='" + size + "' ");

            }
            if (gia != null && gia != "")
            {
                string s = "";
                switch (gia)
                {
                    case "<200":
                        s = " a.gia<200000 ";
                        break;
                    case "200-800":
                        s = " (a.gia>=200000 and a.gia<800000) ";
                        break;
                    case "800-2000":
                        s = " (a.gia>=800000 and a.gia<2020000) ";
                        break;
                    case ">2000":
                        s = " a.gia>=2000000 ";
                        break;
                }
                filterQuery = "and" + s;
            }

            System.Diagnostics.Debug.WriteLine(filterQuery);
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            if (keyword == null || keyword == "")
            {
                System.Diagnostics.Debug.WriteLine("dont search");
                string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active from sanpham a where a.id_sanpham in(SELECT DISTINCT a.id_sanpham from sanpham a, loaisanpham b,mausanpham c,sizesanpham d,chitietsanpham e where a.ma_loaisp = b.ma_loaisp and e.ma_mau=c.ma_mausp and e.ma_size=d.ma_sizesp and a.id_sanpham=e.id_sanpham and a.ma_loaisp =@maloaisp and a.loai=@loai " + filterQuery + ")";
                MySqlParameter maloaisp = new MySqlParameter("@maloaisp", MySqlDbType.String);
                maloaisp.Value = cat;
                MySqlParameter loaisp = new MySqlParameter("@loai", MySqlDbType.String);
                loaisp.Value = loai;
                newCmd.CommandText = queryString;
                newCmd.Parameters.Add(maloaisp);
                newCmd.Parameters.Add(loaisp);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("search");
                string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active from sanpham a where a.id_sanpham in(SELECT DISTINCT a.id_sanpham from sanpham a, loaisanpham b,mausanpham c, sizesanpham d,chitietsanpham e where a.ma_loaisp = b.ma_loaisp and e.ma_mau = c.ma_mausp and e.ma_size = d.ma_sizesp and a.id_sanpham = e.id_sanpham and a.ten_sp like \'%" + keyword + "%\' " + filterQuery + "  )";
                newCmd.CommandText = queryString;


            }
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {

                        if (!isContain(reader[1].ToString()))
                            count++;
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            int numpage = 0;
            numpage = (int)(count / 9);
            if (numpage * 9 < count)
                numpage++;
            return numpage;
        }
        public IEnumerable<Product> GetProductsByCategoryAndLoaiAndFilter(string cat, string loai, string mau, string size, string gia, string keyword, int page)
        {
            int pagenum;
            pagenum = page;
            int begin = (pagenum - 1) * 9;
            string filterNew = "";
            string filterQuery = "";
            if (mau != null && mau != "")
            {
                filterQuery += ("and c.ma_mausp='" + mau + "' ");
            }
            if (size != null && size != "")
            {
                filterQuery += ("and d.ma_sizesp='" + size + "' ");

            }
            if (gia != null && gia != "")
            {
                string s = "";
                switch (gia)
                {
                    case "<200":
                        s = " a.gia<200000 ";
                        break;
                    case "200-800":
                        s = " (a.gia>=200000 and a.gia<800000) ";
                        break;
                    case "800-2000":
                        s = " (a.gia>=800000 and a.gia<2020000) ";
                        break;
                    case ">2000":
                        s = " a.gia>=2000000 ";
                        break;
                }
                filterQuery = "and" + s;
            }

            System.Diagnostics.Debug.WriteLine(filterQuery);
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            if (keyword == null || keyword == "")
            {
                System.Diagnostics.Debug.WriteLine("dont search");

                string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active,b.rate from sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km where a.id_sanpham in(SELECT DISTINCT a.id_sanpham from sanpham a, loaisanpham b,mausanpham c,sizesanpham d,chitietsanpham e where a.ma_loaisp = b.ma_loaisp and e.ma_mau=c.ma_mausp and e.ma_size=d.ma_sizesp and a.id_sanpham=e.id_sanpham and a.ma_loaisp =@maloaisp and a.loai=@loai " + filterQuery + ")limit " + begin + ",9";
                MySqlParameter maloaisp = new MySqlParameter("@maloaisp", MySqlDbType.String);
                maloaisp.Value = cat;
                MySqlParameter loaisp = new MySqlParameter("@loai", MySqlDbType.String);
                loaisp.Value = loai;
                newCmd.CommandText = queryString;
                newCmd.Parameters.Add(maloaisp);
                newCmd.Parameters.Add(loaisp);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("search");

                //string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,e.ma_mau,e.ma_size,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active from sanpham a, loaisanpham b,mausanpham c,sizesanpham d,chitietsanpham e where a.ma_loaisp = b.ma_loaisp and a.ma_mau=c.ma_mausp and a.ma_size=d.ma_sizesp and a.id_sanpham=e.id_sanpham and a.ten_sp like '%" + keyword+"%' " + filterQuery + "  limit " + begin + ",9";
                string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active,b.rate from sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km where a.id_sanpham in(SELECT DISTINCT a.id_sanpham from sanpham a, loaisanpham b,mausanpham c, sizesanpham d,chitietsanpham e where a.ma_loaisp = b.ma_loaisp and e.ma_mau = c.ma_mausp and e.ma_size = d.ma_sizesp and a.id_sanpham = e.id_sanpham and a.ten_sp like \'%" + keyword + "%\' " + filterQuery + "  )limit " + begin + ",9";
                newCmd.CommandText = queryString;


            }
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        Product p = new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString());
                        {
                            p.ma_mau = mau;
                        }
                        if (size != null && size != "")
                        {
                            p.ma_size = size;
                        }
                        //System.Diagnostics.Debug.WriteLine(reader[10]);
                        if (!reader.IsDBNull(10))
                        {
                            p.rateDiscount = reader.GetDouble(10);
                        }
                        if (!isContain(reader[1].ToString()))
                            _productList.Add(p);
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            foreach (Product p in _productList)
            {
                p.imageMain = getImageMain(p.id_sanpham);
            }
            return _productList;
        }
        public string getImageMain(String id)
        {
            string imglink = "";
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();

            string queryString = "select a.link_anh from hinhanh a,chitietsanpham b  where a.id_anh=b.id_anh and b.id_sanpham=@idsp";
            MySqlParameter idsp = new MySqlParameter("@idsp", MySqlDbType.String);
            idsp.Value = id;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(idsp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    reader.Read();
                    imglink = reader[0].ToString();
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            return imglink;
        }
        public Product GetProductsByID(string id)
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string queryString = "select a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active,b.rate FROM sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km where a.id_sanpham=@idsp";
            MySqlParameter idsp = new MySqlParameter("@idsp", MySqlDbType.String);
            idsp.Value = id;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(idsp);
            Product p = null;
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(5))
                        {
                            p = new Product(reader.GetString("id_sanpham"), reader.GetString("ten_sp"), reader.GetString("ma_loaisp"), reader.GetDouble("gia"), reader.GetString("loai"), reader.GetString("id_km"), "", reader.GetInt32("soluongton"), reader.GetString("mota"), reader.GetInt32("active").ToString());
                        }
                        else
                        {
                            p = new Product(reader.GetString("id_sanpham"), reader.GetString("ten_sp"), reader.GetString("ma_loaisp"), reader.GetDouble("gia"), reader.GetString("loai"), "", "", reader.GetInt32("soluongton"), reader.GetString("mota"), reader.GetInt32("active").ToString());

                        }
                        if (!reader.IsDBNull(10))
                        {
                            p.rateDiscount = reader.GetDouble(10);
                        }
                        _productList.Add(p);
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            conn = KetNoi.GetDBConnection();
            conn.Open();
            idsp = new MySqlParameter("@idsp", MySqlDbType.String);
            List<string> imgs = new List<string>();
            newCmd = conn.CreateCommand();
            idsp.Value = id;
            queryString = "select a.link_anh from hinhanh a,chitietsanpham b  where a.id_anh=b.id_anh and b.id_sanpham=@idsp";
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(idsp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {

                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine("hasrow");
                        string imglink = reader[0].ToString();
                        if (!imgs.Contains(imglink))
                            imgs.Add(imglink);
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            System.Diagnostics.Debug.WriteLine(imgs.Count);
            //Dòng code t fix là từ chỗ này(Hiệp>3)
            if (_productList.Count != 1) return null;
            //nó bị lỗi ArgumentOutOfRangeException
            //productlist[0] đc nó xác định là empty nên ko thể thay thế imgs trong đó đc
            //nên t đã thêm 1 dòng dùng để clone nó ra
            Product pro = _productList[0];
            pro.imgs = imgs;
            if (imgs.Count < 1)
            {
                pro.imageMain = "";
            }
            else
            {
                pro.imageMain = imgs[0];
            }
            conn.Close();

            return pro;
        }

        public Product GetProductsByName(string name)
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string queryString = "select * from sanpham  where ten_sp=@namesp";
            MySqlParameter namesp = new MySqlParameter("@namesp", MySqlDbType.String);
            namesp.Value = name;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(namesp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        _productList.Add(new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString()));
                    }
                }
                else Console.WriteLine("No rows found.");
            }

            List<string> imgs = new List<string>();
            newCmd = conn.CreateCommand();
            queryString = "select link_anh from hinhanh a,sanpham b  where a.id_sanpham=b.id_sanpham and b.ten_sp=@namesp";
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(namesp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        imgs.Add(reader[0].ToString());
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            _productList[0].imgs = imgs;
            conn.Close();

            return _productList[0];
        }

        public Product GetProduct(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            string queryString = "select a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active,b.rate FROM sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km limit 0,16";
            MySqlCommand command = new MySqlCommand(queryString, conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        Product p = new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString());
                        if (!reader.IsDBNull(10))
                        {
                            p.rateDiscount = reader.GetDouble(10);
                        }
                        _productList.Add(p);
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            foreach (Product p in _productList)
            {
                p.imageMain = getImageMain(p.id_sanpham);
            }
            return _productList;
        }

        public IEnumerable<Product> GetLatestProducts()
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            string queryString = "select a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active FROM sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km ORDER BY a.createdat desc limit 6";
            MySqlCommand command = new MySqlCommand(queryString, conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        _productList.Add(new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString()));
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            foreach (Product p in _productList)
            {
                p.imageMain = getImageMain(p.id_sanpham);
            }
            return _productList;
        }
        public IEnumerable<Product> GetRandomProducts()
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            string queryString = "select a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active FROM sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km ORDER BY Rand() limit 6";
            MySqlCommand command = new MySqlCommand(queryString, conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        _productList.Add(new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString()));
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            foreach (Product p in _productList)
            {
                p.imageMain = getImageMain(p.id_sanpham);
            }
            return _productList;
        }
        public IEnumerable<Product> GetMostSoldProducts()
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            string queryString = "select a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active from sanpham a LEFT JOIN khuyenmai b on a.id_km=b.id_km INNER JOIN (SELECT id_sanpham from chitiethoadon GROUP BY id_sanpham ORDER BY sum(soluong) DESC limit 6) as c on a.id_sanpham=c.id_sanpham limit 6";
            MySqlCommand command = new MySqlCommand(queryString, conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        _productList.Add(new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString()));
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            foreach (Product p in _productList)
            {
                p.imageMain = getImageMain(p.id_sanpham);
            }
            return _productList;
        }
        public IEnumerable<string> GetColorsByIDProduct(string id)
        {
            List<string> list = new List<string>();
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            //string queryString = "select ma_mau from sanpham  where ten_sp=@namesp";
            string queryString = "select ma_mau from chitietsanpham  where id_sanpham=@id_sanpham";
            MySqlParameter idsp = new MySqlParameter("@id_sanpham", MySqlDbType.String);
            idsp.Value = id;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(idsp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        if (!isContainString(list, reader[0].ToString()))
                            list.Add(reader[0].ToString());
                    }
                }
                else Console.WriteLine("No rows found.");
            }


            conn.Close();

            return list;
        }

        public IEnumerable<string> GetSizesByIDProduct(string id)
        {
            List<string> list = new List<string>();
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string queryString = "select ma_size from chitietsanpham  where  id_sanpham=@id_sanpham";
            MySqlParameter idsp = new MySqlParameter("@id_sanpham", MySqlDbType.String);
            idsp.Value = id;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(idsp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        if (!isContainString(list, reader[0].ToString()))
                            list.Add(reader[0].ToString());
                    }
                }
                else Console.WriteLine("No rows found.");
            }


            conn.Close();

            return list;
        }
        public IEnumerable<Product> GetRelatedProducts(string cat, string loai)
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active from sanpham a, loaisanpham b where a.ma_loaisp = b.ma_loaisp and a.ma_loaisp =@maloaisp and a.loai=@loai order by a.gia limit 4";
            MySqlParameter maloaisp = new MySqlParameter("@maloaisp", MySqlDbType.String);
            maloaisp.Value = cat;
            MySqlParameter loaisp = new MySqlParameter("@loai", MySqlDbType.String);
            loaisp.Value = loai;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(maloaisp);
            newCmd.Parameters.Add(loaisp);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {

                        _productList.Add(new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString()));
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            foreach (Product p in _productList)
            {
                p.imageMain = getImageMain(p.id_sanpham);
            }
            return _productList;
        }
        public IEnumerable<Product> GetProductsBySearch(string keywword)
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string queryString = "SELECT a.id_sanpham,a.ten_sp,a.ma_loaisp,a.gia,a.loai,a.id_km,a.thuonghieu,a.soluongton,a.mota,a.active from sanpham a, loaisanpham b where a.ma_loaisp = b.ma_loaisp and a.ten_sp like *@keyword*";
            MySqlParameter key = new MySqlParameter("@keyword", MySqlDbType.String);
            key.Value = keywword;
            newCmd.CommandText = queryString;
            newCmd.Parameters.Add(key);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                // Kiểm tra có kết quả trả về
                if (reader.HasRows)
                { // Đọc từng dòng kết quả cho đến hết
                    while (reader.Read())
                    {
                        _productList.Add(new Product(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetDouble(3), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader.GetInt32(7), reader[8].ToString(), reader[9].ToString()));
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();

            return _productList;
        }
        public int GetReviewsCount(string pid)
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string query = "select count(*) from review where id_sanpham = @idsp";
            MySqlParameter key = new MySqlParameter("@idsp", MySqlDbType.String);
            key.Value = pid;
            newCmd.CommandText = query;
            newCmd.Parameters.Add(key);
            int count = 0;
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            return count;
        }

        public IEnumerable<Review> GetReviews(string pid, int limit, int amount)
        {
            MySqlConnection conn = KetNoi.GetDBConnection();
            List<Review> _listComments = new List<Review>();
            conn.Open();
            MySqlCommand newCmd = conn.CreateCommand();
            string query = "select * from review where id_sanpham = @idsp limit @limit,@amount";
            MySqlParameter key = new MySqlParameter("@idsp", MySqlDbType.String);
            MySqlParameter limited = new MySqlParameter("@limit", MySqlDbType.Int32);
            MySqlParameter amounts = new MySqlParameter("@amount", MySqlDbType.Int32);
            key.Value = pid;
            limited.Value = limit;
            amounts.Value = amount;
            newCmd.CommandText = query;
            newCmd.Parameters.Add(key);
            newCmd.Parameters.Add(limited);
            newCmd.Parameters.Add(amounts);
            using (MySqlDataReader reader = newCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Review r = new Review();
                        r.id_sanpham = reader.GetString("id_sanpham");
                        r.username = reader.GetString("username");
                        r.sosao = reader.GetInt32("sosao");
                        r.noidung = reader.GetString("noidung");
                        r.date = reader.GetDateTime("createdat").ToString();
                        _listComments.Add(r);
                    }
                }
                else Console.WriteLine("No rows found.");
            }
            conn.Close();
            return _listComments;
        }
    }
}