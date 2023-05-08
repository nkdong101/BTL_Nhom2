using BTL_Nhom2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_Nhom2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        TinhDauDB db = new TinhDauDB();
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
    }
}