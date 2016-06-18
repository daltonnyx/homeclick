using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.HtmlPropertyEditor.Web;
using DevExpress.Web.ASPxHtmlEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webs = System.Web.UI.WebControls;
using DevExpress.Web;

namespace Homeclick.vn.Module.Web.Editors
{
    [PropertyEditor(typeof(string),false)]
    public class CustomHtmlEditor : ASPxPropertyEditor
    {
        public CustomHtmlEditor(Type objectType, IModelMemberViewItem model) : base (objectType, model) { }
        private ASPxHtmlEditor control;

        protected override object CreateControlCore()
        {
           return base.CreateControlCore();
        }

        protected override void WriteValueCore()
        {
            base.WriteValueCore();
        }

        void control_HtmlChanged(object sender, EventArgs e)
        {
            WriteValue();
        }

        protected override Webs.WebControl CreateEditModeControlCore()
        {
            return new ASPxHtmlEditor();
        }


        protected override void ReadValueCore()
        {
            control = ((ASPxHtmlEditor)Editor);
            control.ClientInstanceName = "content";
            control.Html = PropertyValue.ToString();
            control.HtmlChanged += control_HtmlChanged;
        }
        protected override object GetControlValueCore()
        {
            return base.GetControlValueCore();
        }
    }
}
