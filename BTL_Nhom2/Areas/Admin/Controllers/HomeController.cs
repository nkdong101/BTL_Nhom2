using BTL_Nhom2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_Nhom2.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        TinhDauDB db = new TinhDauDB();
        public ActionResult Index()
        {
            if (Session["TenTaiKhoan"] == null)
            {
                return RedirectToAction("Login");
            }
            var products = db.SanPhams.ToList();
            var categories = db.DanhMucs.ToList();
            var receipts = db.HoaDons.ToList();
            DateTime today = DateTime.Today;
            List<HoaDon> hds = db.HoaDons.Where(h => h.NgayDat.Month == today.Month &&
                    h.NgayDat.Year == today.Year && h.TinhTrang.Equals("Đã giao")).ToList();
            decimal tongTienNum = 0;
            foreach (var item in hds)
            {
                tongTienNum += item.GioHang.ChiTietGioHangs.Select(c => c.SoLuongMua * c.Gia).Sum();
            }

            List<HoaDon> hd_trong_nam = hds.Where(h => h.NgayDat.Year == today.Year
                    && h.TinhTrang.Equals("Đã giao")).ToList();
            decimal tongTienTrongNamNum = 0;
            foreach (var item in hd_trong_nam)
            {
                tongTienTrongNamNum += item.GioHang.ChiTietGioHangs.Select(c => c.SoLuongMua * c.Gia).Sum();
            }

            var accounts = db.TaiKhoans.Where(t => t.Quyen == 0).ToList();

            ViewBag.soLuongSp = products.Count;
            ViewBag.soLuongDm = categories.Count;
            ViewBag.soLuongDh = hds.Count;
            ViewBag.soLuongTk = accounts.Count;
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string tongTien = string.Format("{0:0,000}", tongTienNum);
            string tongTienTrongNam = string.Format("{0:0,000}", tongTienTrongNamNum);
            ViewBag.tongTienThangNay = tongTien;
            ViewBag.tongTienTrongNam = tongTienTrongNam;

            return View();
        }
    }
}