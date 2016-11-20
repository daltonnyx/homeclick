using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Models;

namespace Homeclick.Controllers
{
    
    public class CartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            return View(GetCartItems());
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        /// <param name="Id">Mã option của sản phẩm</param>
        /// <param name="returnUrl">Liên kết trả về sau khi thêm thành công</param>
        /// <returns></returns>
        public ActionResult AddItemToCard(string returnUrl)
        {
            var optionId = int.Parse(Request.Form["option"]);
            //Lấy thông tin sản phẩm
            var option = db.Product_Options.Find(optionId);

            //Kiểm tra sản phẩm và màu sắc
            if (option == null)
                return HttpNotFound();

            //Lấy số lượng từ form
            var quantity = int.Parse(Request.Form["quantity"]);

            //Lấy ra session giỏ hàng
            var cartItems = GetCartItems();

            //Kiểm tra xem sản phẩm cùng màu sắc này đã có trong giỏ hàng hay chưa
            var cartItem = cartItems.Find(o => o.product_Option.Id == optionId);
            //Sau đó...
            if (cartItem == null) //Nếu chưa có thì thêm vào giỏ
                cartItems.Add(new CartItem(option)
                {
                    quantity = quantity,
                });
            else //có rồi thì chỉ đặt lại số lượng
                cartItem.quantity += quantity;

            return Redirect(returnUrl);
        }

        /// <summary>
        /// Cập nhật giỏ hàng
        /// </summary>
        /// <param name="id">Mã item trong giỏ, không phải mã sản phẩm</param>
        /// <param name="form">Form nguồn</param>
        /// <returns></returns>
        public ActionResult UpdateCartItem(string id, FormCollection form)
        {
            //Lấy danh sách item từ giỏ
            var cartItems = GetCartItems();

            //Lọc ra 1
            var cartItem = cartItems.Find(o => o.id == id);

            //Lấy sản phẩm từ item
            var product = db.Products.Find(cartItem.product_Option.Id);

            //Kiểm tra item lẫn sản phẩm có tồn tại
            if (cartItem == null && product == null)
                return HttpNotFound();

            //Đặt lại số lượng
            cartItem.quantity = int.Parse(form["quantity"].ToString());

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Xóa sản phẩm từ giỏ
        /// </summary>
        /// <param name="id">Mã item trong giỏ, không phải mã sản phẩm</param>
        /// <returns></returns>
        public ActionResult DeleteItemInCart(string id)
        {
            //Lấy danh sách item từ giỏ
            var cartItems = GetCartItems();

            //Lọc ra 1
            var cartItem = cartItems.Find(o => o.id == id);

            //Lấy sản phẩm từ item
            var product = db.Products.Find(cartItem.product_Option.Id);

            //Kiểm tra item lẫn sản phẩm có tồn tại
            if (cartItem == null && product == null)
                return HttpNotFound();

            //Loại item khỏi giỏ hàng
            cartItems.Remove(cartItem);

            return RedirectToAction("Index");
        }

        #region PartialView
        /// <summary>
        /// Widget giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult _headerWidget()
        {
            //Giỏ hàng trống?
            var isCartEmpty = Quantity() == 0;

            ViewBag.Empty = isCartEmpty ? "Empty Cart" : "View Cart";
            ViewBag.Soluong = isCartEmpty ? 0 : Quantity();
            ViewBag.Tongtien = isCartEmpty ? 0 : TotalPrice();

            return PartialView();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Lấy danh sách item trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public List<CartItem> GetCartItems()
        {
            var listCart = Session["Cart"] as List<CartItem>;
            if (listCart == null)
                Session["Cart"] = new List<CartItem>();
            return listCart ?? new List<CartItem>();
        }

        /// <summary>
        /// Tính tổng số lượng sản phẩm trong giỏ
        /// </summary>
        /// <returns></returns>
        private int Quantity()
        {
            int iQuantity = 0;
            var listCart = GetCartItems();
            if (listCart != null)
                iQuantity = listCart.Sum(n => n.quantity);
            return iQuantity;
        }

        /// <summary>
        /// Tính tổng tiền của giỏ
        /// </summary>
        /// <returns></returns>
        private decimal TotalPrice()
        {
            decimal dTotalPrice = 0;
            var listCart = GetCartItems();
            if (listCart != null)
                dTotalPrice = listCart.Sum(n => n.total);
            return dTotalPrice;
        }
        #endregion
    }
}