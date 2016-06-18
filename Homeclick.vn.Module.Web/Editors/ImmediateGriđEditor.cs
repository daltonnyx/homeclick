using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Homeclick.vn.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Homeclick.vn.Module.Web.Editors
{
    [PropertyEditor(typeof(IList<Product>),false)]
    public class ImmediateGriđEditor : ASPxPropertyEditor, IComplexViewItem
    {
        IObjectSpace objspc;
        XafApplication app;
        ASPxGridView control;

        public ImmediateGriđEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override System.Web.UI.WebControls.WebControl CreateEditModeControlCore()
        {
            control = new ASPxGridView();
            grid_render(ref control);
            return control;
        }

        private void grid_render(ref ASPxGridView control)
        {
            if(View.CurrentObject is Category)
            {
                Category cat = (Category)View.CurrentObject;
                IList<DanhSachProductsyDanhSachCategories> products = cat.ProductToCategoryLinks;
                control.DataSource = products;
                control.DataBind();
                control.Columns["DanhSachCategories"].Visible = false;
                control.Columns["DanhSachProducts"].Caption = "Tên sản phẩm";
                control.Columns["Quantity"].Caption = "Số lượng";
                control.Columns["OID"].Visible = false;
                control.KeyFieldName = "OID";
                control.ClientInstanceName = "gridthamso";
                control.EnableRowsCache = false;
                control.ID = "gridthamso";
                control.ClientSideEvents.RowDblClick = @"function(s, e) { if (gridthamso.IsEditing() == true) { index = e.visibleIndex; s.UpdateEdit(); } else { s.SetFocusedRowIndex(e.visibleIndex);  s.StartEditRow(e.visibleIndex); } } ";
                
            }
            
        }



        protected override System.Web.UI.WebControls.WebControl CreateViewModeControlCore()
        {
            control = new ASPxGridView();
            grid_render(ref control);
            return control;
        }

        protected override void SetupControl(WebControl control)
        {

            base.SetupControl(control);
        }

        protected new ASPxGridView Editor
        {
            get
            {
                return (ASPxGridView)base.Editor;
            }
        }

        protected new ASPxGridView InplaceViewModelEditor
        {
            get
            {
                return (ASPxGridView)base.InplaceViewModeEditor;
            }
        }

        public void Setup(DevExpress.ExpressApp.IObjectSpace objectSpace, DevExpress.ExpressApp.XafApplication application)
        {
            this.objspc = objectSpace;
            this.app = application;
        }
    }
}
