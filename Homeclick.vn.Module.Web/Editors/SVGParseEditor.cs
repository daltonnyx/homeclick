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



        void uploadButton_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(e.UploadedFile.FileContent);

            //e.CallbackData = SavePostedFile(e.UploadedFile);
            //we will do something here
            //filename = e.CallbackData;
            //((ASPxTextBox)Editor).Value = e.CallbackData;
            XmlNodeList paths = xml.DocumentElement.GetElementsByTagName("path");
            IList<object> value = new List<object>();
            foreach (XmlNode path in paths)
            {
                string style = path.Attributes["style"].Value;
                string dpath = path.Attributes["d"].Value;
                string[] styleCanva = style.Split(';');
                Regex reg = new Regex(@"([mlca]|[MLCA])(\s+-*\d+\.?\d*,\s*-*\d+\.?\d*)+");
                IList<object> points = new List<object>();
                MatchCollection svgPatterns = reg.Matches(dpath);
                foreach(Match pattern in svgPatterns)
                {
                    string patternValue = pattern.Value;
                    string[] pointstext = patternValue.Split(' ');
                    try
                    {
                        if (pointstext[0] == "m" || pointstext[0] == "l" || pointstext[0] == "c" || pointstext[0] == "a")
                        {
                        
                            points.Add(new { x = Convert.ToSingle(pointstext[1].Split(',')[0]), y = Convert.ToSingle(pointstext[1].Split(',')[1]) });
                            for(int i = 2; i < pointstext.Length; i++)
                            {
                                string[] xy = pointstext[i].Split(',');
                                dynamic lastPoint = points[i - 2];
                                points.Add(new { x = lastPoint.x + Convert.ToSingle(xy[0]), y = lastPoint.y + Convert.ToSingle(xy[1]) });
                            }
                        }
                        else if (pointstext[0] == "M" || pointstext[0] == "L" || pointstext[0] == "C" || pointstext[0] == "A") 
                        {
                            points.Add(new { x = Convert.ToSingle(pointstext[1].Split(',')[0]), y = Convert.ToSingle(pointstext[1].Split(',')[1]) });
                            for (int i = 2; i < pointstext.Length; i++)
                            {
                                string[] xy = pointstext[i].Split(',');
                                dynamic lastPoint = points[i - 2];
                                points.Add(new { x = Convert.ToSingle(xy[0]), y = Convert.ToSingle(xy[1]) });
                            }
                        }
                        if (dpath.Last() == 'z')
                        {
                            points.Add(points[0]);
                        }
                        
                    }
                    catch
                    {
                        continue;
                    }
                }
                value.Add(points);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.PropertyValue = serializer.Serialize(value);
        }

        protected override object GetControlValueCore()
        {
            return this.PropertyValue;
        }

    }
}
