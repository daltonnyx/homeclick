using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Homeclick.Models
{
    public partial class Post
    {

        public Dictionary<string, object> ToArray(Post post)
        {

            PropertyInfo[] typeInfos = (typeof(Post)).GetProperties();
            Dictionary<string, object> child = new Dictionary<string, object>();
            foreach (PropertyInfo member in typeInfos)
            {
                if (member.CanRead && member.Name == "Post_detail")
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();
                    ICollection<Post_detail> detailList = member.GetValue(post) as ICollection<Post_detail>;
                    foreach (Post_detail detail in detailList)
                    {
                        details.Add(detail.name, detail.value);

                    }
                    child.Add(member.Name, details);
                }

                //else if (member.CanRead)
                //{
                //    child.Add(member.Name, member.GetValue(p));
                //}
            }
            return child;

        }

        public Dictionary<string, object> ToArray(Category post)
        {

            PropertyInfo[] typeInfos = (typeof(Category)).GetProperties();
            Dictionary<string, object> child = new Dictionary<string, object>();
            foreach (PropertyInfo member in typeInfos)
            {
                if (member.CanRead && member.Name == "Category_detail")
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();
                    ICollection<Category_detail> detailList = member.GetValue(post) as ICollection<Category_detail>;
                    foreach (Category_detail detail in detailList)
                    {
                        details.Add(detail.name, detail.value);

                    }
                    child.Add(member.Name, details);
                }
            }
            return child;

        }




        public object details { get; set; }
    }
}