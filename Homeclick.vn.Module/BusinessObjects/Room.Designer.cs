//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homeclick.vn.Module.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    
    using DevExpress.Xpo;
    using System.ComponentModel.DataAnnotations.Schema;
    using DevExpress.ExpressApp.ConditionalAppearance;
    using DevExpress.ExpressApp.Editors;
    using DevExpress.Persistent.Base;
    using DevExpress.ExpressApp.Model;
    public partial class Room: XPLiteObject
    {
    	//VNB: Room
        public Room()
        {
            //VNB disable: this.Canvas = new List<Canva>();
        }
    
        private int _id;
    	[DevExpress.Xpo.Key(true)]
    	[Required]
    	public int Id 
    	{ 
    		get { return _id; } 
    		set
    		{
    			if (value != _id) {
    				_id = value;
    				 OnIdChanged();
    			}
    		} 
    	}
    	partial void OnIdChanged(); 
    
        private string _name;
    	[Required]
    	public string name 
    	{ 
    		get { return _name; } 
    		set
    		{
    			if (value != _name) {
    				_name = value;
    				 OnnameChanged();
    			}
    		} 
    	}
    	partial void OnnameChanged(); 
    
        private string _description;
    	public string description 
    	{ 
    		get { return _description; } 
    		set
    		{
    			if (value != _description) {
    				_description = value;
    				 OndescriptionChanged();
    			}
    		} 
    	}
    	partial void OndescriptionChanged(); 
    
        private int _floor_id;
    	[Required]
    	public int floor_id 
    	{ 
    		get { return _floor_id; } 
    		set
    		{
    			if (value != _floor_id) {
    				_floor_id = value;
    				 Onfloor_idChanged();
    			}
    		} 
    	}
    	partial void Onfloor_idChanged(); 
    
        private string _coordinates;
    	[Required]
        [Size(SizeAttribute.Unlimited)]
        public string coordinates 
    	{ 
    		get { return _coordinates; } 
    		set
    		{
    			if (value != _coordinates) {
    				_coordinates = value;
    				 OncoordinatesChanged();
    			}
    		} 
    	}
    	partial void OncoordinatesChanged(); 
    
        private string _canvas_data;
    	public string canvas_data 
    	{ 
    		get { return _canvas_data; } 
    		set
    		{
    			if (value != _canvas_data) {
    				_canvas_data = value;
    				 Oncanvas_dataChanged();
    			}
    		} 
    	}
    	partial void Oncanvas_dataChanged(); 
    
    
        private Floor _FloorId;
    	[ImmediatePostData]
    	[Association(@"RoomsFloorsReferences", typeof(Floor))]
    	public  Floor FloorId { get {return _FloorId;} set{ SetPropertyValue<Floor>("FloorId", ref _FloorId, value); } }
    
        [Association(@"CanvasRoomsReferences", typeof(Canva))]
    	public  XPCollection<Canva> DanhSachCanvas { get{{ return GetCollection<Canva>("DanhSachCanvas"); }} }
    
    }
}
