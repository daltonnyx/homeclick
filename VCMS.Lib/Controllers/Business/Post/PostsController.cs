using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Business;
using VCMS.Lib.Models.Business.Datatables;

namespace VCMS.Lib.Controllers
{
    public class PostsController : CategoriesController
    {
        public PostViewModel ModelToViewModel(Post model)
        {
            var viewModel = new PostViewModel
            {
                postId = model.Id,
                title = model.Title,
                excerpt = model.Excerpt,
                htmlContent = model.Content,
                previewImageId = model.ImageId,
                previewImage = model.ImageFile.FullFileName,
                categoryIds = model.Categories.Select(o => o.Id).ToArray(),   
                status = model.Status
            };
            return viewModel;
        }

        public Post ViewModelToModel(PostViewModel viewModel)
        {
            var model = db.Posts.Find(viewModel.postId);
            if (model == null)
            {
                model = new Post
                {
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now,
                };
            }

            model.Title = viewModel.title;
            model.Excerpt = viewModel.excerpt;
            model.Content = viewModel.htmlContent;
            model.ImageId = viewModel.previewImageId;
            model.Status = viewModel.status;
            model.Featured = false;
            model.Alias = viewModel.title.ToUnsignString();

            //Categories
            var viewModelCategoryIds = viewModel.categoryIds.ToList();
            foreach (var modelRoom in model.Categories.ToList())
            {
                var found = false;
                foreach (var roomId in viewModel.categoryIds)
                {
                    if (modelRoom.Id == roomId)
                    {
                        found = true;
                        viewModelCategoryIds.Remove(roomId);
                        break;
                    }
                }
                if (!found)
                    model.Categories.Remove(modelRoom);
            }

            foreach (var categoryId in viewModelCategoryIds)
            {
                var category = db.Categories.Find(categoryId);
                if (category != null && !model.Categories.Contains(category))
                    model.Categories.Add(category);
            }
            return model;
        }
    }
}
