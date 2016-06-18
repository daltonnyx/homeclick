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

namespace Homeclick.vn.Module.Web.Editors
{
    [PropertyEditor(typeof(string),false)]
    public class UploadPropertyEditor : ASPxStringPropertyEditor
    {
        private ASPxUploadControl uploadButton;
        private Webs.Button submit;
        private Webs.Image preview;
        private Webs.WebControl currentControl;
        private string filename;
        protected string SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
        public UploadPropertyEditor(Type objectType, IModelMemberViewItem imodel) : base(objectType, imodel) { }

        public new ASPxTextBox Editor
        {
            get { return new ASPxTextBox(); }
        }
        protected override System.Web.UI.WebControls.WebControl CreateEditModeControlCore()
        {
            currentControl = base.CreateEditModeControlCore() as Webs.WebControl;
            uploadButton = new ASPxUploadControl();
            preview = new Webs.Image();
            preview.Attributes.Add("id", "upload_image");
            if(PropertyValue != null)
            {
                preview.Attributes.Add("src",PropertyValue.ToString());
            }
            preview.Style.Add("width", "100%");
            uploadButton.UploadMode = UploadControlUploadMode.Auto;
            uploadButton.AutoStartUpload = true;
            uploadButton.ShowProgressPanel = true;
            uploadButton.AdvancedModeSettings.EnableDragAndDrop = true;
            uploadButton.AdvancedModeSettings.EnableFileList = false;
            uploadButton.AdvancedModeSettings.EnableMultiSelect = false;
            uploadButton.ClientSideEvents.FileUploadComplete = "function(s, e) { if(e.isValid){document.getElementById(\"upload_image\").src = \"/Upload/Images/\" + e.callbackData;} }";
            uploadButton.FileUploadComplete +=uploadButton_FileUploadComplete;
            currentControl.Controls.Add(uploadButton);
            currentControl.Controls.Add(preview);
            return currentControl;
        }



        void uploadButton_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
                e.CallbackData = SavePostedFile(e.UploadedFile);
                filename = e.CallbackData;
                //((ASPxTextBox)Editor).Value = e.CallbackData;
        }

        protected override void ReadEditModeValueCore()
        {

             ((ASPxTextBox)Editor).Value = (CombinePath(filename) != null) ? CombinePath(filename) : PropertyValue;
        }
        protected override object GetControlValueCore()
        {
            return CombinePath(filename);
        }
        protected string SavePostedFile(UploadedFile uploadedFile)
        {
            if (!uploadedFile.IsValid)
                return string.Empty;
            string fileName = uploadedFile.FileName;
            string fullFileName = CombinePath(fileName);
            using (Image original = Image.FromStream(uploadedFile.FileContent)) {
                Image size1 = UploadPropertyEditor.CropImage(original,UploadPropertyEditor.getCropSize(original));
                size1.Save(AppDomain.CurrentDomain.BaseDirectory + UploadPropertyEditor.getName(fullFileName,"_cropped"), ImageFormat.Png);
                original.Save(AppDomain.CurrentDomain.BaseDirectory +  fullFileName, ImageFormat.Png);
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

        public static string getName(string filename,string _prefix)
        {
            Match match = Regex.Match(filename,@".+(?=(\.\w+)$)",RegexOptions.IgnoreCase);
            if(match.Success)
            {
                string reg = match.Value.ToString();
                return reg + _prefix + match.Groups[1].Value.ToString();
            }
            return filename;
        }

        public static Bitmap ResizeImage(Image image, int width, int height = 0)
        {
            if(height == 0)
            {
                height = Convert.ToInt32((width * (image.Height * 1.0) / (image.Width * 1.0)));
            }
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Rectangle getCropSize(Image image)
        {
            int x, y, w;
            if(image.Width == image.Height)
            {
                return new Rectangle(0,0,image.Width,image.Height);
            }
            w = (image.Width > image.Height) ? image.Height : image.Width;
            x = (image.Width > image.Height) ? (image.Width / 2) - (image.Height / 2) : 0;
            y = (image.Width > image.Height) ? 0 : (image.Height / 2) - (image.Width / 2);
            return new Rectangle(x,y,w,w);
        }

        public static Bitmap CropImage(Image originalImage, Rectangle sourceRectangle, Rectangle? destinationRectangle = null)
        {
            if (destinationRectangle == null)
            {
                destinationRectangle = new Rectangle(Point.Empty, sourceRectangle.Size);
            }

            var croppedImage = new Bitmap(destinationRectangle.Value.Width,
                destinationRectangle.Value.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(originalImage, destinationRectangle.Value,
                    sourceRectangle, GraphicsUnit.Pixel);
            }
            return croppedImage;
        }
    }
}
