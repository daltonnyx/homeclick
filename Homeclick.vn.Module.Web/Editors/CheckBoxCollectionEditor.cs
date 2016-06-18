using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.HtmlPropertyEditor.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Xpo;
using Homeclick.vn.Module.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Homeclick.vn.Module.Web.Editors
{
    [PropertyEditor(typeof(object),false)]
    public class CheckBoxCollectionEditor : ASPxPropertyEditor, IComplexViewItem
    {
        public CheckBoxCollectionEditor(Type objectType, IModelMemberViewItem modelviewitem) : base(objectType, modelviewitem) { }

        XafApplication application;
        IObjectSpace objectSpace;

        protected override WebControl CreateEditModeControlCore()
        {
            return this.Render();
        }

        protected override WebControl CreateViewModeControlCore()
        {
            return this.Render();
        }

        XPBaseCollection checkedItems;
        IList<Category> checkedCats;

        //protected override void ReadValueCore()
        //{
        //    base.ReadValueCore();
        //    //if (Editor != null)
        //    //    Editor.Controls.Clear();
        //    //if (InplaceViewModeEditor != null)
        //    //    InplaceViewModeEditor.Controls.Clear();
        //    //if (PropertyValue is XPBaseCollection && MemberInfo.ListElementType.Name == "Category")
        //    //{
        //    //    TypeFor typeFor = (MemberInfo.Owner.Type.Name == "Product") ? TypeFor.product : TypeFor.other;
        //    //    IList<Category_type> types = objectSpace.GetObjects<Category_type>(CriteriaOperator.Parse("type_for = ?", (short)typeFor));
        //    //    foreach (Category_type type in types)
        //    //    {
        //    //        ASPxCheckBoxList control = new ASPxCheckBoxList();
        //    //        control.Style.Add("display", "inline-block");
        //    //        control.Caption = type.caption;
        //    //        control.SelectedIndexChanged -= new EventHandler(Control_SelectedIndexChanged);
        //    //        checkedItems = (XPBaseCollection)PropertyValue;
        //    //        IList dataSource = this.application.CreateObjectSpace().GetObjects(MemberInfo.ListElementTypeInfo.Type, CriteriaOperator.Parse("Category_typeId.Id = ?",type.Id));
        //    //        IModelClass classInfo = application.Model.BOModel.GetClass(MemberInfo.ListElementTypeInfo.Type);
        //    //        control.DataSource = dataSource;
        //    //        control.TextField = classInfo.DefaultProperty;
        //    //        control.ValueField = classInfo.KeyProperty;
        //    //        control.ValueType = classInfo.TypeInfo.KeyMember.MemberType;
        //    //        control.RepeatLayout = RepeatLayout.Flow;
        //    //        control.RepeatDirection = RepeatDirection.Vertical;
        //    //        control.RepeatColumns = 10;
        //    //        control.DataBind();
        //    //        control.UnselectAll();
        //    //        foreach (object obj in checkedItems)
        //    //        {
        //    //            ListEditItem item = control.Items.FindByValue(objectSpace.GetKeyValue(obj));
        //    //            if(item != null)
        //    //                item.Selected = true;
        //    //        }
        //    //        control.SelectedIndexChanged += new EventHandler(Control_SelectedIndexChanged);
        //    //        if (Editor != null) { 
        //    //            Editor.Controls.Add(control);
        //    //        }
        //    //        if (InplaceViewModeEditor != null)
        //    //            InplaceViewModeEditor.Controls.Add(control);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ASPxCheckBoxList control = new ASPxCheckBoxList();
        //    //    control.SelectedIndexChanged -= new EventHandler(Control_SelectedIndexChanged);
        //    //    checkedItems = (XPBaseCollection)PropertyValue;
        //    //    XPCollection dataSource = new XPCollection(checkedItems.Session, MemberInfo.ListElementType);
        //    //    IModelClass classInfo = application.Model.BOModel.GetClass(MemberInfo.ListElementTypeInfo.Type);
        //    //    if (checkedItems.Sorting.Count > 0)
        //    //    {
        //    //        dataSource.Sorting = checkedItems.Sorting;
        //    //    }
        //    //    else if (!String.IsNullOrEmpty(classInfo.DefaultProperty))
        //    //    {
        //    //        dataSource.Sorting.Add(new SortProperty(classInfo.DefaultProperty, DevExpress.Xpo.DB.SortingDirection.Ascending));
        //    //    }
        //    //    control.DataSource = dataSource;
        //    //    control.TextField = classInfo.DefaultProperty;
        //    //    control.ValueField = classInfo.KeyProperty;
        //    //    control.ValueType = classInfo.TypeInfo.KeyMember.MemberType;
        //    //    control.RepeatLayout = RepeatLayout.Flow;
        //    //    control.RepeatDirection = RepeatDirection.Horizontal;
        //    //    control.RepeatColumns = 10;
        //    //    control.DataBind();
        //    //    control.UnselectAll();
        //    //    foreach (object obj in checkedItems)
        //    //    {
        //    //        control.Items.FindByValue(objectSpace.GetKeyValue(obj)).Selected = true;
        //    //    }
        //    //    control.SelectedIndexChanged += new EventHandler(Control_SelectedIndexChanged);
        //    //    if (Editor != null) { 
        //    //        Editor.Controls.Add(control);
        //    //        //Editor.SelectedIndexChanged += Control_SelectedIndexChanged;
        //    //    }
        //    //    if (InplaceViewModeEditor != null)
        //    //        InplaceViewModeEditor.Controls.Add(control);
        //    //}
        //}

        //protected override object GetControlValueCore()
        //{
 	        
        //    foreach(object control in Editor.Controls)
        //    {
        //        if(control is ASPxCheckBoxList)
        //        {
        //            foreach(ListEditItem item in ((ASPxCheckBoxList)control).Items)
        //            {
        //                object obj = objectSpace.GetObjectByKey(MemberInfo.ListElementTypeInfo.Type, item.Value);
        //                if (item.Selected)
        //                {
        //                    checkedItems.BaseAdd(obj);
        //                }
        //                else
        //                {
        //                    checkedItems.BaseRemove(obj);
        //                }
        //            }
        //            PropertyValue = checkedItems;
        //            return PropertyValue;
        //        }
        //    }
        //    return base.GetControlValueCore();
        //}

        public void Control_SelectedIndexChanged(object sender, EventArgs e)
        {
            ASPxCheckBoxList control = (ASPxCheckBoxList)sender;
            foreach (ListEditItem item in control.Items)
            {
                object obj = objectSpace.GetObjectByKey(MemberInfo.ListElementTypeInfo.Type, item.Value);
                if (item.Selected)
                {
                    checkedItems.BaseAdd(obj);
                }
                else
                {
                    checkedItems.BaseRemove(obj);
                }
            }
            OnControlValueChanged();
            objectSpace.SetModified(CurrentObject);
        }

        public void Control_SelectedIndexChangedCat(object sender, EventArgs e)
        {
            ASPxCheckBoxList control = (ASPxCheckBoxList)sender;
            foreach (ListEditItem item in control.Items)
            {
                Category obj = objectSpace.GetObjectByKey<Category>(item.Value);
                if (item.Selected)
                {
                    checkedCats.Add(obj);
                }
                else
                {
                    checkedCats.Remove(obj);
                }
            }
            OnControlValueChanged();
            objectSpace.SetModified(CurrentObject);
        }

        //protected override void SetImmediatePostDataScript(string script)
        //{
        //    Editor.ClientSideEvents.SelectedIndexChanged = script;
        //}

        protected override bool IsMemberSetterRequired()
        {
            return false;
        }

        private bool CanEditValue
        {
            get { return this.ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.View ? false : true; }
        }

       private WebControl Render()
        {
            WebControl container = new WebControl(System.Web.UI.HtmlTextWriterTag.Div);
            if (PropertyValue is IList<Category>)
            {
                TypeFor typeFor = (MemberInfo.Owner.Type.Name == "Product") ? TypeFor.product : TypeFor.other;
                IList<Category_type> types = objectSpace.GetObjects<Category_type>(CriteriaOperator.Parse("type_for = ?", (short)typeFor));
                foreach (Category_type type in types)
                {
                    ASPxCheckBoxList control = new ASPxCheckBoxList();
                    control.Style.Add("display", "inline-block");
                    control.Caption = type.caption;
                    control.SelectedIndexChanged -= new EventHandler(Control_SelectedIndexChanged);
                    checkedCats = (IList<Category>)PropertyValue;
                    IList dataSource = this.application.CreateObjectSpace().GetObjects(MemberInfo.ListElementTypeInfo.Type, CriteriaOperator.Parse("Category_typeId.Id = ?", type.Id));
                    IModelClass classInfo = application.Model.BOModel.GetClass(MemberInfo.ListElementTypeInfo.Type);
                    control.DataSource = dataSource;
                    control.TextField = classInfo.DefaultProperty;
                    control.ClientEnabled = this.CanEditValue;
                    control.ValueField = classInfo.KeyProperty;
                    control.ValueType = classInfo.TypeInfo.KeyMember.MemberType;
                    control.RepeatLayout = RepeatLayout.Flow;
                    control.RepeatDirection = RepeatDirection.Vertical;
                    control.RepeatColumns = 10;
                    control.DataBind();
                    control.UnselectAll();
                    foreach (object obj in checkedCats)
                    {
                        ListEditItem item = control.Items.FindByValue(objectSpace.GetKeyValue(obj));
                        if (item != null)
                            item.Selected = true;
                    }
                    control.SelectedIndexChanged += new EventHandler(Control_SelectedIndexChangedCat);
                    container.Controls.Add(control);
                }
            }
            else
            {
                ASPxCheckBoxList control = new ASPxCheckBoxList();
                control.SelectedIndexChanged -= new EventHandler(Control_SelectedIndexChanged);
                checkedItems = (XPBaseCollection)PropertyValue;
                XPCollection dataSource = new XPCollection(checkedItems.Session, MemberInfo.ListElementType);
                IModelClass classInfo = application.Model.BOModel.GetClass(MemberInfo.ListElementTypeInfo.Type);
                if (checkedItems.Sorting.Count > 0)
                {
                    dataSource.Sorting = checkedItems.Sorting;
                }
                else if (!String.IsNullOrEmpty(classInfo.DefaultProperty))
                {
                    dataSource.Sorting.Add(new SortProperty(classInfo.DefaultProperty, DevExpress.Xpo.DB.SortingDirection.Ascending));
                }
                control.DataSource = dataSource;
                control.TextField = classInfo.DefaultProperty;
                control.ValueField = classInfo.KeyProperty;
                control.ClientEnabled = this.CanEditValue;
                control.ValueType = classInfo.TypeInfo.KeyMember.MemberType;
                control.RepeatLayout = RepeatLayout.Flow;
                control.RepeatDirection = RepeatDirection.Horizontal;
                control.RepeatColumns = 10;
                control.DataBind();
                control.UnselectAll();
                foreach (object obj in checkedItems)
                {
                    control.Items.FindByValue(objectSpace.GetKeyValue(obj)).Selected = true;
                }
                control.SelectedIndexChanged += new EventHandler(Control_SelectedIndexChanged);
                container.Controls.Add(control);
            }
            return container;
        }
        
        #region IComplexViewItem Members

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;
        }

        #endregion
    }
}
