﻿using VCMS.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class DesignController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Design/
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult DesignSlider()
        {
            return PartialView();
        }

        public ActionResult Design()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getDepartments(int? id = null)
        {
            var model = db.Departments.Where(d => d.ProjectId == id).OrderBy(d => d.Name).ToList();
            return PartialView("Departments", model);
        }

        [HttpGet]
        public ActionResult getProjects(int? id = null)
        {
            var model = db.Projects.Where(d => d.DistrictId == id).OrderBy(p => p.Name).ToList();
            return PartialView("Projects", model);
        }

        [HttpGet]
        public ActionResult getDistricts(int? id = null)
        {
            var model = db.Districts.Where(d => d.CityId == id).OrderBy(d => d.Name).ToList();
            return PartialView("Districts", model);
        }

        [HttpGet]
        public ActionResult getCities()
        {
            var model = db.Cities.OrderBy(c => c.Name).ToList<City>();
            return PartialView("Cities", model);
        }

        [HttpGet]
        public ActionResult getFloors(int? id)
        {
            var model = db.Floors.Where<Floor>(f => f.Department.Id == id).OrderBy(f => f.Name).ToList<Floor>();
            return PartialView("Floors", model);
        }

        [HttpGet]
        public ActionResult getRooms(int? id)
        {
            var model = db.Rooms.Where<Room>(f => f.FloorId == id).OrderBy(r => r.Name).ToList<Room>();
            return PartialView("Rooms", model);
        }

        public ActionResult Floor(int? floor_id)
        {
            floor_id = Convert.ToInt16(Request["floor"].ToString());
            var model = db.Floors.Find(floor_id);
            return View(model);
        }

        public ActionResult Canvas(int? id)
        {
            var categoryTypes = db.Category_types.ToList();

            categoryTypes.Remove(categoryTypes.Find(o => o.Id == 17));
            categoryTypes.Remove(categoryTypes.Find(o => o.Id == 18));

            ViewBag.SelectedTypeId = categoryTypes.FirstOrDefault().Id;
            ViewBag.CategoryTypes = categoryTypes;

            var model = db.Rooms.Find(id);
            if(model != null)
                return PartialView(model);
            else
                throw new HttpException(404, "This room doesn't exist!");
        }

        [HttpPost]
        public ActionResult Print()
        {
            var formData = this.Request.Form;
            foreach(string key in formData.AllKeys)
            {
                ViewData[key] = formData[key];
            }
            return PartialView();
        }

        public ActionResult _CategoryOptions(int? CategoryTypeId)
        {
            var categories = db.Categories.Where(o => o.CategoryTypeId == CategoryTypeId).ToList();
            return PartialView(categories);
        }

        public ActionResult _ProductTypes()
        {
            var types = db.Product_Types.ToList<Product_Type>();
            return PartialView(types);
        }

    }
}