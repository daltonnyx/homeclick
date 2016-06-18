using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;

namespace Homeclick.vn.Module.BusinessObjects
{
    [MetadataType(typeof(Post_metadata))]
    [DefaultClassOptions]
    [XafDisplayName("Bài viết"),DefaultProperty("title")]
    public partial class Post
    {
        public Post(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        private bool isNewObject = false;

        [NonPersistent]
        public ProductEnum StatusEnum
        {
            get { return (ProductEnum)this.status; }
            set { this.status = (short)value; }
        }

        protected override void OnSaving()
        {
            if (this.Session.IsNewObject(this))
            {
                isNewObject = true;
                this.created_date = DateTime.Now;
            }
            this.updated_date = DateTime.Now;
            base.OnSaving();

        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (!this.isNewObject)
                return;
            using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer))
            {
                //Post_detail postDetail = new Post_detail(uow);
                //postDetail.name = "product_gallery";
                //postDetail.caption = "Hình ảnh";
                //postDetail.typeEnum = TypeEnum.multiupload;
                //postDetail.value = string.Empty;
                //postDetail.PostId = uow.FindObject<Post>(new BinaryOperator("Id", this.Id));
                //postDetail.Save();

                //Post_detail postDetail = new Post_detail(uow);
                //postDetail.name = "post_";
                //postDetail.caption = "Giá";
                //postDetail.typeEnum = TypeEnum.str;
                //postDetail.value = string.Empty;
                //postDetail.PostId = uow.FindObject<Post>(new BinaryOperator("Id", this.Id));
                //postDetail.Save();


                if (this.excerpt == string.Empty)
                {
                    string[] words = HtmlHelper.StripTagsCharArray(this.content).Split(' ');
                    string the_excerpt = string.Empty;
                    for (int i = 0; i < 50; i++)
                    {
                        the_excerpt += words[i];
                        if (i < 50)
                            the_excerpt += " ";
                    }
                    this.excerpt = the_excerpt;
                }

                uow.CommitChanges();
            }
        }
    }
    public class Post_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 parent_id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 author_id
        {
            get;
            set;
        }
    }
}
