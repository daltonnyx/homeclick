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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Script.Serialization;

namespace Homeclick.vn.Module.Web.Editors
{
    [PropertyEditor(typeof(string), false)]
    public class SVGParseEditor : ASPxStringPropertyEditor
    {
        private ASPxUploadControl uploadButton;
        protected string SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
        private Webs.WebControl currentControl;
        public SVGParseEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        public new ASPxTextBox Editor
        {
            get { return new ASPxTextBox(); }
        }
        protected override System.Web.UI.WebControls.WebControl CreateEditModeControlCore()
        {
            currentControl = base.CreateEditModeControlCore() as Webs.WebControl;
            uploadButton = new ASPxUploadControl();
            
            uploadButton.UploadMode = UploadControlUploadMode.Auto;
            uploadButton.AutoStartUpload = true;
            uploadButton.ShowProgressPanel = true;
            uploadButton.AdvancedModeSettings.EnableDragAndDrop = true;
            uploadButton.AdvancedModeSettings.EnableFileList = false;
            uploadButton.AdvancedModeSettings.EnableMultiSelect = false;
            uploadButton.ClientSideEvents.FileUploadComplete = "function(s, e) { if(e.isValid){document.getElementById(\"upload_image\").src = \"/Upload/Images/\" + e.callbackData;} }";
            uploadButton.FileUploadComplete += uploadButton_FileUploadComplete;
            currentControl.Controls.Add(uploadButton);
            //currentControl.Controls.Add(preview);
            return currentControl;
        }

        protected string SavePostedFile(UploadedFile uploadedFile)
        {
            if (!uploadedFile.IsValid)
                return string.Empty;
            string fileName = uploadedFile.FileName;
            string fullFileName = CombinePath(fileName);
            using (Image original = Image.FromStream(uploadedFile.FileContent))
            {
                Image size1 = UploadPropertyEditor.CropImage(original, UploadPropertyEditor.getCropSize(original));
                size1.Save(AppDomain.CurrentDomain.BaseDirectory + UploadPropertyEditor.getName(fullFileName, "_cropped"), ImageFormat.Png);
                original.Save(AppDomain.CurrentDomain.BaseDirectory + fullFileName, ImageFormat.Png);
            }
            PropertyValue = fullFileName;
            return fileName;
        }

        protected string CombinePath(string fileName)
        {
            if (fileName == null)
                return null;
            return Path.Combine(SavePath, fileName);
        }

        void uploadButton_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(e.UploadedFile.FileContent);
            XmlNodeList groups = xml.DocumentElement.GetElementsByTagName("g");
            bool 
            foreach(XmlNode group in groups)
            {
                group.Attributes["id"]
            }
        }

        protected override object GetControlValueCore()
        {
            return this.PropertyValue;
        }

    }
}
