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
using System.IO;
using Drawing = System.Drawing;
using Homeclick.vn.Module.BusinessObjects;
using DevExpress.ExpressApp.Web;

namespace Homeclick.vn.Module.Web.Editors
{

    [PropertyEditor(typeof(string),false)]
    public class MultiTypeEditor : ASPxStringPropertyEditor
    {
         private ASPxUploadControl uploadButton;
        private Webs.WebControl preview;
        private Webs.WebControl currentControl;
        private string filename;
        protected string SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
        public MultiTypeEditor(Type objectType, IModelMemberViewItem imodel) : base(objectType, imodel) { }

        protected override System.Web.UI.WebControls.WebControl CreateEditModeControlCore()
        {

            currentControl = base.CreateEditModeControlCore();
            uploadButton = new ASPxUploadControl();
            uploadButton.UploadMode = UploadControlUploadMode.Auto;
            uploadButton.AutoStartUpload = true;
            uploadButton.ShowProgressPanel = true;
            
            uploadButton.ShowAddRemoveButtons = true;
            preview = new Webs.WebControl(System.Web.UI.HtmlTextWriterTag.Div);
            preview.Attributes.Add("id", "upload_image");
            if(((IDetail)this.CurrentObject).type == 2)
            {
                uploadButton.FileUploadComplete -= uploadButton_FileUploadComplete2;

                if(PropertyValue != null)
                {
                    string[] urls = ((string)PropertyValue).Split(';');
                    foreach(string url in urls)
                    {
                        if (url.Trim() == string.Empty)
                            continue;
                        Webs.Image previewChild = new Webs.Image();
                        previewChild.Attributes.Add("src", url.ToString());
                        previewChild.Style.Add("display", "inline-block");
                        previewChild.Style.Add("width", "128px");
                        preview.Controls.Add(previewChild);
                    }
                }
                uploadButton.AdvancedModeSettings.EnableDragAndDrop = true;
                uploadButton.AdvancedModeSettings.EnableFileList = false;
                
                uploadButton.AdvancedModeSettings.EnableMultiSelect = true;
                uploadButton.ClientSideEvents.FileUploadComplete = "function(s,e){if(e.isValid&&e.callbackData){var fileData=e.callbackData.split('|');var fileUrl=fileData[1];var img=document.createElement('img');img.src=fileUrl;img.style=\"display:inline-block;width:128px\";document.getElementById(\"upload_image\").appendChild(img);}}";
                uploadButton.FileUploadComplete += uploadButton_FileUploadComplete2;

                currentControl.Controls.Add(uploadButton);
                currentControl.Controls.Add(preview);
                return currentControl;
            }
            else if (((IDetail)this.CurrentObject).type == 1) {
                uploadButton.FileUploadComplete -= uploadButton_FileUploadComplete;
                if(PropertyValue != null)
                {
                    Webs.Image previewChild = new Webs.Image();
                    previewChild.Attributes.Add("src", PropertyValue.ToString());
                    previewChild.Style.Add("display", "inline-block");
                    previewChild.Style.Add("width", "128px");
                    preview.Controls.Add(previewChild);
                }
                preview.Style.Add("width", "100%");
                uploadButton.AdvancedModeSettings.EnableMultiSelect = false;
                uploadButton.ShowAddRemoveButtons = false;
                uploadButton.AutoStartUpload = true;
                uploadButton.ClientSideEvents.FileUploadComplete = "function(s,e){if(e.isValid){var img=document.createElement('img');img.src=\"/Upload/Images/\"+e.callbackData;img.style=\"display:inline-block;width:100%;\";document.getElementById(\"upload_image\").appendChild(img);}}";
                uploadButton.FileUploadComplete +=uploadButton_FileUploadComplete;

                currentControl.Controls.Add(uploadButton);
                currentControl.Controls.Add(preview);
                return currentControl;
            }
            currentControl = RenderHelper.CreateASPxMemo();
            SetupASPxMemo((ASPxMemo)currentControl);
            ((ASPxMemo)currentControl).MaxLength = int.MaxValue;
            ((ASPxMemo)currentControl).TextChanged += MultiTypeEditor_TextChanged;
            return currentControl;
        }

        void MultiTypeEditor_TextChanged(object sender, EventArgs e)
        {
            this.PropertyValue = ((ASPxMemo)sender).Text;
        }
        protected override void OnValueRead()
        {
            base.OnValueRead();
            if (filename == null && PropertyValue != null)
                filename = PropertyValue.ToString();
        }

        void uploadButton_FileUploadComplete2(object sender, FileUploadCompleteEventArgs e)
        {
            string name = CombinePath(e.UploadedFile.FileName);
            filename += name + ';';
            string url = SavePostedFile(e.UploadedFile);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + CombinePath(url) + "|" + sizeText;
            PropertyValue = filename;
        }

        void uploadButton_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            e.CallbackData = SavePostedFile(e.UploadedFile);
            filename = e.CallbackData;
        }

        protected override void ReadEditModeValueCore()
        {
            if (((IDetail)this.CurrentObject).type == 2)
            {
                Editor.Value = (filename != null) ? filename : PropertyValue;
            }
            else if (((IDetail)this.CurrentObject).type == 1)
            {
                Editor.Value = (CombinePath(filename) != null) ? CombinePath(filename) : PropertyValue;
            }
            else
            {
                base.ReadEditModeValueCore();
                return;
            }
        }
        protected override object GetControlValueCore()
        {
            if(((IDetail)this.CurrentObject).type == 2)
            {
                return filename;

            }
            else if (((IDetail)this.CurrentObject).type == 1)
            { 
                return CombinePath(filename);
            }
            return base.GetControlValueCore();
        }
        protected string SavePostedFile(UploadedFile uploadedFile)
        {
            if (!uploadedFile.IsValid)
                return string.Empty;
            string fileName = uploadedFile.FileName;
            string fullFileName = CombinePath(fileName);
            using (Drawing.Image original = Drawing.Image.FromStream(uploadedFile.FileContent))
            {
                Drawing.Image size1 = UploadPropertyEditor.CropImage(original, UploadPropertyEditor.getCropSize(original));
                size1.Save(AppDomain.CurrentDomain.BaseDirectory + UploadPropertyEditor.getName(fullFileName, "_cropped"), Drawing.Imaging.ImageFormat.Png);
                original.Save(AppDomain.CurrentDomain.BaseDirectory + fullFileName, Drawing.Imaging.ImageFormat.Png);
            }
            return fileName;
        }
        protected string CombinePath(string fileName)
        {
            if (fileName == null)
                return null;
            return Path.Combine(SavePath, fileName);
        }
    }
}
