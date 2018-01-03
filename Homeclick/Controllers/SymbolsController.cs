using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using VCMS.Lib.Models;
using System.Net;

namespace Homeclick.Controllers
{
    public class SymbolsController : Controller
    {
        
        public ApplicationDbContext db = new ApplicationDbContext();
        // GET: Symbols
        public ActionResult CanvasList(int? type)
        {
            IList<Symbol> symbols = (from symbol in db.Symbols
                                       select symbol).ToList<Symbol>();

            
            if (type != null)
            {
                symbols = (from symbol in symbols
                            where symbol.ProductType.Id == type
                            select symbol).ToList<Symbol>();
            }
            return PartialView(symbols);
        }
    }
}