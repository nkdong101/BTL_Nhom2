using BTL_Nhom2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using System.Data.Entity;


namespace BTL_Nhom2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        TinhDauDB db = new TinhDauDB();

        public ActionResult Home(string sortOrder, int? madm, int? beginPrice, int? endPrice, string searchString, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var sp = db.SanPhams.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                sp = sp.Where(s => s.TenSP.ToLower().Contains(searchString.ToLower())).ToList();
                return View(sp.ToPagedList(pageNumber, pageSize));
            }
            if (madm > 0)
            {
                sp = sp.Where(s => s.MaDM == madm).ToList();
            }
            if (sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "sortSL":
                        sp = sp.OrderBy(s => s.SoLuongTon).ToList(); break;
                    case "giaTang":
                        sp = sp.OrderBy(s => s.Gia).ToList(); break;
                    case "giaGiam":
                        sp = sp.OrderByDescending(s => s.Gia).ToList(); break;
                }
            }
            if (beginPrice > 0 && endPrice == 0)
            {
                sp = sp.Where(s => s.Gia <= beginPrice).OrderBy(s => s.Gia).ToList();
            }
            if (beginPrice > 0 && endPrice > 0)
            {
                sp = sp.Where(s => s.Gia >= beginPrice && s.Gia <= endPrice).OrderBy(s => s.Gia).ToList();
            }
            if (beginPrice == 0 && endPrice > 0)
            {
                sp = sp.Where(s => s.Gia >= endPrice).OrderBy(s => s.Gia).ToList();
            }
            return View(sp.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TenTaiKhoan, string MatKhau)
        {
            

            if (string.IsNullOrEmpty(TenTaiKhoan))
            {
                ViewBag.ErrorTenTaiKhoan = "Tên tài khoản không được để trống";
            }
            if (string.IsNullOrEmpty(MatKhau))
            {
                ViewBag.ErrorMatKhau = "Mật khẩu không được để trống";
            }

            var user = db.TaiKhoans.Where(t => t.TenTaiKhoan.Equals(TenTaiKhoan) &&
            t.MatKhau.Equals(MatKhau) && t.TinhTrang == true).ToList();
            if (user.Count() > 0)
            {
                if (user.First().Quyen == 0)
                {
                    Session["TaiKhoan"] = user.FirstOrDefault();
                    Session["TenKhachHang"] = user.FirstOrDefault().TenKhachHang;
                    Session["TenTaiKhoan"] = user.FirstOrDefault().TenTaiKhoan;
                    return RedirectToAction("Index");
                }
                Session["TenTaiKhoan"] = user.FirstOrDefault().TenTaiKhoan;
                if (user.First().Quyen == 1)
                {
                    Session["Quyen"] = "Admin";

                }

                return RedirectToAction("Index", "Admin/Home");
            }
            else
            {
                ViewBag.FailedMessage = "Thông tin đăng nhập không chính xác!";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Signin()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin([Bind(Include = "TenTaiKhoan,MatKhau,Quyen,TinhTrang,TenKhachHang,Email,SoDienThoai,DiaChi")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var taiKhoanFind = db.TaiKhoans.Find(taiKhoan.TenTaiKhoan);
                if (taiKhoanFind == null)
                {
                    db.TaiKhoans.Add(taiKhoan);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.ErrorSign = "Tên tài khoản trùng. Vui lòng nhập tên khác";
                }
            }
            //ViewBag.Infor = taiKhoan.ToString();
            return View(taiKhoan);
        }
    }
    
}