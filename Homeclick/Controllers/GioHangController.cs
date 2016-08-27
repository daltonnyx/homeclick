using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;

namespace Homeclick.Controllers
{
    
    public class GioHangController : Controller
    {
        
        // GET: /GioHang/
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public List<Giohang> LayGioHang()
        {
            List<Giohang> listGioHang = Session["GioHang"] as List<Giohang>;
            if (listGioHang == null)
            {
                listGioHang = new List<Giohang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }

        [Authorize]
        public ActionResult ThemSanPham(int Id, string strUrl)
        {
            string name = Request.Form["txtqty"];
            int a = Int32.Parse(name);
            Product product = db.Products.SingleOrDefault(s => s.Id == Id);
            Dictionary<string, object> arrayItem = product.ToArray(product);
            Dictionary<string, object> details = arrayItem["Product_detail"] as Dictionary<string, object>;
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<Giohang> listSanPham = LayGioHang();
            //Kiểm tra xem sách này đã có trong giỏ hàng
            Giohang sp = listSanPham.Find(b => b.iProduct_id == Id);
            if (sp == null)
            {
                //sp.iNumberProduct = Inumber;

                sp = new Giohang(Id);
                sp.iNumberProduct = a;
                listSanPham.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                //sp.iNumberProduct = int.Parse(f["txtqty"].ToString());
                sp.iNumberProduct += a;
                return Redirect(strUrl);
            }
        }

        //Cập nhật giỏ hàng

        public ActionResult Capnhatgiohang(int iProduct_id, FormCollection f)
        {
            //Kiểm sản phẩm
            Product product = db.Products.SingleOrDefault(n => n.Id == iProduct_id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Giohang> listproduct = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong giỏ hàng của session
            Giohang lsproduct = listproduct.Find(b => b.iProduct_id == iProduct_id);
            if (lsproduct != null)
            {
                lsproduct.iNumberProduct = int.Parse(f["txtSoLuong"].ToString());
            }
            if (listproduct == null)
            {
                ViewBag.itemcart = "Không có sản phẩm nào !";
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("SuaGioHang");

        }

        public ActionResult XoaGioHang(int iProduct_id)
        {
            //Kiểm sản phẩm
            Product product = db.Products.SingleOrDefault(n => n.Id == iProduct_id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Giohang> listproduct = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong giỏ hàng của session
            Giohang lsproduct = listproduct.Find(b => b.iProduct_id == iProduct_id);
            if (lsproduct != null)
            {
                listproduct.RemoveAll(n => n.iProduct_id == iProduct_id);
            }
            if (listproduct.Count == 0)
            {
                RedirectToAction("Index", "Home");
            }
            return RedirectToAction("SuaGioHang");
        }

        //Xây dựng trang giỏ hàng
        [Authorize]
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            List<Giohang> listproduct = LayGioHang();
            return View(listproduct);
        }
        //Tính số lượng/tổng tiền
        private int SoLuong()
        {
            int iSoLuong = 0;
            List<Giohang> listGioHang = Session["GioHang"] as List<Giohang>;
            if (listGioHang != null)
            {
                iSoLuong = listGioHang.Sum(n => n.iNumberProduct);
            }
            return iSoLuong;
        }
        private decimal TongTien()
        {
            decimal dTongtien = 0;
            List<Giohang> listGioHang = Session["GioHang"] as List<Giohang>;
            if (listGioHang != null)
            {
                dTongtien = listGioHang.Sum(n => n.iTotal);
            }
            return dTongtien;
        }

        public ActionResult ShoppingCartPartial()
        {
            if (SoLuong() == 0)
            {
                ViewBag.Empty = "Empty Cart";
                ViewBag.Soluong = "0";
                ViewBag.Tongtien = "0.00";
                return PartialView();
            }
            else
            {
                ViewBag.Empty = "View Cart";
                ViewBag.Soluong = SoLuong();
                ViewBag.Tongtien = TongTien();
                return PartialView();
            }
        }
        [Authorize]
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            if (SoLuong() == 0)
            {
                ViewBag.noproduct = 0;
            }
            else
            {
                ViewBag.noproduct = 1;
            }
            List<Giohang> listproduct = LayGioHang();
            return View(listproduct);
        }
    }
}