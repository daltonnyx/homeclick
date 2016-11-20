using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class JSONController : Controller
    {
        vinabits_homeclickEntities db = new Models.vinabits_homeclickEntities();

        public JsonResult _GetRooms(int? layout_id)
        {
            var list = db.ProjectItems.Where(o => o.LockedOut == false && o.ParentId == layout_id).ToList();
            var json = new List<object>();
            foreach (var item in list)
            {
                json.Add(new
                {
                    id = item.Id,
                    name = item.Name,
                    areaCoords = item.AreaCoords
                });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _Collections(int? room_id, int? layout_id)
        {
            var list = new List<ProjectLayout_Collection>();
            if (room_id != null)
                list = db.ProjectLayout_Collections.Where(o => o.LockedOut == false && o.LayoutId == room_id).ToList();
            else
            {
                var temp = db.ProjectItems.Where(o => o.ParentId == layout_id).ToList();
                foreach (var item in temp)
                {
                    var v = db.ProjectLayout_Collections.Where(o => o.LockedOut == false && o.LayoutId == item.Id).ToList();
                    list.AddRange(v);
                }
            }

            var json = new List<object>();
            foreach (var item in list)
            {
                json.Add(new
                {
                    id = item.Id,
                    roomId = item.LayoutId,
                    name = item.Name,
                    image = item.Image,
                    area = item.Area
                });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectsData(int? category_id)
        {
            IEnumerable<Project> projects;

            projects = category_id == null || category_id == -1 ? db.Projects.ToList() :
                                db.Projects.Where(o => o.CategoryId == category_id).ToList();

            var json = new List<object>();
            foreach (var project in projects)
            {
                json.Add(new
                {
                    id = project.Id,
                    name = project.Name,
                    image = project.Image,
                    address = project.Address,
                    city = project.CityId,
                    state = project.StateId,

                    i = project.Investor,
                    a = project.ArchitetualDesignAgency,
                    v = project.ViewDesignAgency,
                    f = project.FurnitureDesignAgency,
                    c = project.ConstructionAgency,
                    d = project.DistributionAgency,
                    m = project.Manager,

                    statu = Convert.ToInt32(project.Completed),
                    type = project.CategoryId,
                });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStates(int city_id)
        {
            var states = db.States.Where(o => o.CityId == city_id);
            var jsonResult = new List<object>();
            foreach (var state in states)
            {
                jsonResult.Add(new
                {
                    id = state.Id,
                    name = state.Name,
                    city = state.CityId
                });
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}