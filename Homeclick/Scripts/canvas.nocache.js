/*
 * Project Name: Furniture building
 * Author: Dalton Nyx
 * License:  GNU GENERAL PUBLIC
 *
 */
jQuery(document).ready(function($){

  // init object and variable
  var canvas = new fabric.Canvas('tutorial');
  var canvasObj = $("#tutorial");
  var p,isDragable = false,src,srcW,srcH,srcName,srcImage,srcZdata,srcPrice,srcScale,centerX,centerY,_isInside = false,srcOnWall;
  const srcMultiple = 50;
  var isInside = function(p,obj) {
    if(typeof(p) == 'undefined' || p == null)
      return false;
    if((p.x >= obj.offset().left) && (p.x <= (obj.offset().left + obj.width())) && (p.y >= obj.offset().top) && (p.y <= (obj.offset().top + obj.height())))
        return true;
    return false;
  }

  // Find obj part
  var objectPart = function(p) {
    var cW = canvas.width / 2;
    var cH = canvas.height / 2;
    //console.log(cW + "-" + cH + "-" + p.x + "-" + p.y);
    if(p.x < cW && p.y < cH)
        return 1;
    if(p.x > cW && p.y < cH)
        return 2;
    if(p.x > cW && p.y > cH)
        return 3;
    if(p.x < cW && p.y > cH)
        return 4;
    return 0;
  };

  var getEndPoint = function(obj,c) { //Find nearest point to canvas border
    var maxX,maxY,minX,minY;
      maxX = minX = obj.oCoords.tl.x;
    maxY = minY = obj.oCoords.tl.y;
    for(var corkey in obj.oCoords)
    {
      if (obj.oCoords.hasOwnProperty(corkey) && corkey != "mtr") {
        var cor = obj.oCoords[corkey];
        if(cor.x < minX)
          minX = cor.x;
        if(cor.x > maxX)
          maxX = cor.x;
        if(cor.y < minY)
          minY = cor.y;
        if(cor.y > maxY)
          maxY = cor.y;
      }
    }
    switch (c){
      case 1 :
        return {x:minX,y:minY};break;
      case 2 :
        return {x:maxX,y:minY};break;
      case 3 :
        return {x:maxX,y:maxY};break;
      case 4 :
        return {x:minX,y:maxY};break;
      default :
        return {};break;
    }
  };

  var getO = function(o,c){ // Get offset between object base point and end point
    var e = getEndPoint(o,c);
    return  {x:Math.abs(o.left - e.x),y:Math.abs(o.top - e.y)};
  };

  //Get SVG file to import
  $(document).on("mousedown",".svg-item",function(event){
    isDragable = true;
    srcOnWall = null;
    src = $(event.target).data("svg");
    srcName = $(event.target).data("name");
    srcScale = $(event.target).data("can-scale");
    srcZdata = $(event.target).data("zdata");
    srcImage = $(event.target).data('image');
    srcPrice = $(event.target).data('price');
    if(event.target.hasAttribute('data-on-wall')) // Stick line object for window, door, etc...
    {
      srcOnWall = $(event.target).data("on-wall");
      srcOnWall = srcOnWall.replace(/\'/g,'"');
      srcOnWall = JSON.parse(srcOnWall);
    }
    srcH = $(event.target).height();
    srcW = $(event.target).width();
    centerX = event.pageX - $(event.target).offset().left;
    centerY = event.pageY - $(event.target).offset().top;
  });

  //prevent default drag event -> can hook drop event
  $("html").on("dragenter",function(e){
    e.preventDefault();
  });
  $("html").on("dragleave",function(e){
    e.preventDefault();
  });
  $("html").on("dragover",function(e){
    e.preventDefault();
  });

  // Check svg inside canvas and render object
  $("html").on("drop",function(event){
    event.preventDefault();
    // pageX and pageY IN originalEvent but NOT event
    var e = event.originalEvent;
    p = {x:e.pageX,y:e.pageY};
    if(isInside(p,canvasObj) == true && isDragable && src != null)
    {
      fabric.loadSVGFromURL(src,function(objects,options){
        var obj = fabric.util.groupSVGElements(objects,options);
        obj.pathToFill = []; //set pathToFill property
        obj.srcSVG = src;
        obj.ProName = srcName;
        obj.isLock = false;
        obj.scale(1/srcMultiple);
        obj.zData = srcZdata;
        obj.realImage = srcImage;
        obj.price = srcPrice;
        obj.set({
          left: canvas.getPointer(e).x - centerX,
          top: canvas.getPointer(e).y - centerY,
          hoverCursor: "move",
          lockUniScaling: true,
          lockScalingFlip: true,
          centeredScaling: true,
          centeredRotation: true
        });
        if(srcOnWall != null)
        {
          obj.onWall = srcOnWall;
          obj.onStick = false;
        }
        if(srcScale == 'off')
        {
          obj.set({
            lockScalingX: true,
            lockScalingY: true
          });
        }
        for(var i = 0; i < obj.paths.length; i++)
        {
          if(obj.paths[i].fill == "")
          {
            obj.paths[i].setFill("#ffffff");
            obj.pathToFill.push(i);
          }
          obj.paths[i].strokeWidth = obj.paths[i].strokeWidth * 10;
        }
        obj.setControlsVisibility({mtr:false,tr:false,bl:false});
        canvas.add(obj);
      });
      isDragable = false;
    }
  });

  var z = 1;
  //Zoom control
  var zoom = function(){
    z = $(this).val() / 100;
    var c = canvas.getCenter();
    canvas.zoomToPoint({x:c.left,y:c.top},z);
  };

  $('input[name="zoom_value"]').change(zoom);
  $('input[name="zoom_slider"]').on('input',zoom);

  // Pans control
  var isHold = false,isMoveObject = false,isPermentPans = false;
  canvas.on("mouse:up",function(e){//Mouse up event
    
      if(isPermentsZoom || isPermentPans || isDrawMode)
        return;
    //If pointer still in point area then load control
    if(typeof this.findTarget(e.e) == 'groupLiPolygon'){
      e = e.e;
      loadWallControl(e);
    }
    else if(typeof(this.findTarget(e.e)) != 'undefined')
    {
      e = e.e;
      loadObjectControl(e);
    }

  });

  jQuery(document).on('mouseup',function(e){
    // Reset check state - No need to check condition
     isHold = false;
      canvas.setCursor('default');
       canvas.selection = true;
      isMoveObject = false;
      isChangeCorner = -1;
      isChangeWall = -1;
      affecteds = [];
      src = null;

      canvas.renderAll();
    });
  var loadWallControl = function(e){//Load Wall control
    if(typeof pWall ==  'undefined')
      return;
    var f = canvas.getPointer(e),
        m = {x:e.pageX,y:e.pageY},
        c = onCorner(f,pWall),
        l = onLineWall(f,pWall);  
    if(c != -1){
      var control = jQuery(".wall-control");
      var p = pWall.points[c];
      control.css({"display":"block","position":"absolute","left":m.x - 120 + "px","top":m.y - 80 + "px"});
      control.find("#_i").val(c);
      control.find("#_x").val(f.x);
      control.find("#_y").val(f.y);
      control.find("#floorArea").text((pWall.calcArea() * Math.pow(20,2) / 1000000).toFixed(2) + " m2");
      control.find("#delete-cor").css("display","block");
      control.find("#add-cor").css("display","none");
      return;
    }
    else if(l != -1)
    {
        var control = jQuery(".wall-control");
        control.css({"display":"block","position":"absolute","left":m.x - 120 + "px","top":m.y - 80 + "px"});
        control.find("#_i").val(l);
        control.find("#_x").val(f.x);
        control.find("#_y").val(f.y);
        control.find("#floorArea").text((pWall.calcArea() * Math.pow(srcMultiple,2) / 1000000).toFixed(2) + " m2");
        control.find("#add-cor").css("display","block");
        control.find("#delete-cor").css("display","none");
        return;
    }
    else
    {
      var m = {x:e.pageX,y:e.pageY}, //Load floor control
          control = jQuery(".object-control"),
          area = (pWall.calcArea() * Math.pow(srcMultiple,2) / 1000000).toFixed(2);
      control.css({
      "display":"block",
      "position":"absolute",
      "left":m.x - (control.width() / 2) - 25 + "px",
      "top":m.y - control.height() -15 +  "px"});
      control.addClass('floor-control');
      control.find("h4.product-name").text(pWall.ProName);
      control.find(".product-area span.value").text(area + "m2");
      control.find(".product-price .value").text(currencyFormat(area * pWall.floorPrice));
    }
    //pWall = null;
  };

  var loadObjectControl = function(e){ //Load Object control
    var obj = canvas.findTarget(e),
        m = {x:e.pageX,y:e.pageY},
        control = jQuery(".object-control"),
        container = jQuery("#tutorial"),
        f = {x:e.pageX,y:e.pageY};
    //console.log(e);
    //Reset everything
    control.find("h4.product-name").text("No Name");
    control.find(".product-image").html('');
    control.find(".product-price .value").text('');
    control.find('.product-price').css('display', 'block');
    control.css({
      "display":"block",
      "position":"absolute",
      "left":f.x - control.width() / 2   + "px",
      "top":f.y - control.height() -65  + "px"});
    jQuery(".width-dimession").text((obj.getWidth() * srcMultiple / 1000).toFixed(2) + ' m');
    jQuery(".height-dimession").text((obj.getHeight() * srcMultiple / 1000).toFixed(2) + ' m');
    updateControl(obj);
    control.removeClass("floor-control");
    if(typeof obj.ProName != 'undefined')
      control.find("h4.product-name").text(obj.ProName);
    if(typeof obj.realImage != 'undefined' && obj.realImage != undefined){
      var realImg = document.createElement('IMG');
      realImg.src = obj.realImage;
      $(realImg).css('width', '100%');
      control.find(".product-image").html(realImg);
    }
    if(typeof obj.price != 'undefined')
        control.find('.product-price .value').text(currencyFormat(obj.price));
    if(canvas._activeGroup != null) //Disable width, height and color control when multiple objects is selected
    {
      //control.find(".product-image").css("display","none");
      control.find("#button-color").css("display","none");
      return;
    }
    control.find(".product-image").css("display","inline-block");
    if(typeof obj.pathToFill != 'undefined' && obj.pathToFill.length > 0 && obj.isLock === false)
      control.find("#button-color").css("display","inline-block");
    else
      control.find("#button-color").css("display","none");
    control.find(".product-dimession li.x .value").text((obj.getWidth() * srcMultiple / 1000).toFixed(2) + ' m');
    control.find(".product-dimession li.y .value").text((obj.getHeight() * srcMultiple / 1000).toFixed(2) + ' m');
    if(typeof obj.zData != 'undefined')
      control.find(".product-dimession li.z .value").text(obj.zData / 1000 + ' m');
  };

  var fx,fy;
  canvas.on("mouse:down",function(e){ //Change canvas state
    e = e.e; //Replace event object with originalEvent
    if(isPermentsZoom == true || isDrawMode)
      return;
    if(this.findTarget(e))
    {
      isMoveObject = true;
      return;
    }
    if(charCode == 17 || isPermentPans == true)
    {
      isHold = true;
      canvas.setCursor('grabbing');
      canvas.selection = false;
    }
    fx = e.offsetX;
    fy = e.offsetY;
  });

  var charCode;
  document.onkeydown = function(e){
    e = e || window.event;
    charCode = e.charCode || e.keyCode;
  };
  document.onkeyup = function(e){
    charCode = null;
  }


 //   //Setting wall
 //  var wallPoints = [
 //  [
 //    {x:5000 / 20, y:0},
 //    {x:8000 / 20, y:0},
 //    {x:8000 / 20, y: 3500 / 20,hide:true},
 //    {x:5000 / 20, y: 3500 / 20,hide:true}
 //  ],
 //  [
 //    {x: 5000 / 20,y: 3500 / 20},
 //    {x: 8000 / 20,y: 3500 / 20},
 //    {x: 8000 / 20,y: 7000 / 20},
 //    {x: 5000 / 20,y: 7000 / 20,hide:true},
    
 //  ],
 //  [
 //    {x:0,y:0},
 //    {x:5000 / 20,y:0},
 //    {x:5000 / 20,y: 3500 /20},
 //    {x: 5000 / 20,y: 7000 / 20},
 //    //{x:5000 / srcMultiple,y:7000 / srcMultiple},
 //    {x:0,y:7000 / 20}
 //  ]
 //  ];
  var polWall = new fabric.groupLiPolygon([],{

    strokeWidth: 10,
    stroke: "#000000",
    strokeLineCap: "round",
    fill: "#ddd",
    hasControls: true,
    hasBorders: false,
    lockMovementX: true,
    lockMovementY: true,
    perPixelTargetFind: true,
   //padding: 4294967295 // get the fuck out, border
  },[7,7,7,7]);
  polWall.ProName = "Sàn";
  polWall.floorPrice = 0;
  // polWall.add(new fabric.LiPolygon([
  //         { x: 50, y:30},
  //         { x: 50, y:250},
  //         { x: 550, y:250},
  //         { x: 550, y:30},
  //         { x: 50, y:30},
  //   ],{strokeWidth: 10,
  //   stroke: "#000000",
  //   strokeLineCap: "round",
  //   fill: "#ddd",
  //   hasControls: true,
  //   hasBorders: false,
  //   lockMovementX: true,
  //   lockMovementY: true,
  //   perPixelTargetFind: true,},[7]));
  //polWall.redrawPolygons();
  canvas.add(polWall);


  //Pans and wall line interactive
  canvas.on("mouse:move",function(e){ // Use as less function as you can
    e = e.e;
    if(isChangeCorner != -1 || isChangeWall != -1)
      return;
    var moX = e.offsetX - fx;
    var moY = e.offsetY - fy;
    fx = e.offsetX;
    fy = e.offsetY;
    if(isMoveObject == true) //Update object when mouse close to edge
    {
        return;
    }
    if(isHold == false)
      return;
    canvas.relativePan({x: moX, y: moY});
  });

  //Toolbar Section

  var isreverseZoom = false,
      resetState = function(){
        isPermentPans = false;
        isPermentsZoom = false;
        isHold = false;
        isMoveObject = false;
        isDrawMode = false;
        getCanvasElement(canvas).removeClass('grab zoomin');
        jQuery('.toolbar div').removeClass('active');
      },
      getCanvasElement = function(canvas){
        var canvasElement = canvas.getElement(),
        upper = jQuery(canvasElement).siblings('canvas');
        return jQuery(upper);
      
      };

  jQuery(".toolbar div.pans").on('click', 'span.pans', function(event) {
    resetState();
    isPermentPans = true;
    getCanvasElement(canvas).addClass('grab');
    var d = event.delegateTarget;
    jQuery(d).addClass('active');
  });

  jQuery(".toolbar div.pen").on('click', 'span.pen', function(event) {
    resetState();
    isDrawMode = true;
    var d = event.delegateTarget;
    jQuery(d).addClass('active');
  });

  jQuery(".toolbar div.pointer").on('click', 'span.pointer', function(event) {
    resetState();
    var d = event.delegateTarget;
    jQuery(d).addClass('active');
  });


  //Zoom toolbar
  jQuery(".toolbar div.zoom-pointer").on('click', 'span.zoom-pointer', function(event) {
    resetState();
    isPermentsZoom = true;
    getCanvasElement(canvas).addClass('zoomin');
    var d = event.delegateTarget;
    jQuery(d).addClass('active');
  });
  var isPermentsZoom = false;

  jQuery(document).on('keydown', function(event) {
    if(event.keyCode == 17){
      isreverseZoom = true;
      getCanvasElement(canvas).addClass('out');
    }
  });

  jQuery(document).on('keyup', function(event) {
    if(event.keyCode == 17){
      isreverseZoom = false;
      getCanvasElement(canvas).removeClass('out');
    }
  });

  canvas.on('mouse:down',function(event){
    if(isDrawMode) return;
    if(isPermentsZoom == true){
      var p = canvas.getPointer(event.e);
      if(isreverseZoom)
        canvas.zoomToPoint(p,canvas.getZoom() - 0.1);
      else  
        canvas.zoomToPoint(p,canvas.getZoom() + 0.1);
      canvas.renderAll();
    }
  });

  var getDistance = function(p0,p1,p2){ //Get point to line distance
    var m = Math.sqrt( Math.pow( p2.y - p1.y, 2 ) + Math.pow( p2.x - p1.x, 2 ) );
    var t = Math.abs(p0.x * (p2.y - p1.y) - p0.y * (p2.x - p1.x) + p2.x * p1.y - p2.y * p1.x);
    return t/m;
  };

  var isBetween = function(p0,p1,p2){ //Check if between 2 points
    if( (p1.x < p0.x && p2.x > p0.x) || (p1.x > p0.x && p2.x < p0.x) )
      return true;
    if( (p1.y < p0.y && p2.y > p0.y) || (p1.y > p0.y && p2.y < p0.y) )
      return true;
    return false;
  };

  var onLineWall = function(p,w,k){ //Check on wall line
    p = w.toLocalPoint(w.group.toLocalPoint(p,'center','center'),'center','center');
    k = typeof k !== 'undefined' ? k : 10;
    for(var i = 0; i < w.points.length;i++)
    {
      var p1 = w.points[i];
      var p2  = (i == w.points.length - 1) ? w.points[0] : w.points[i+1];
      var d = getDistance(p,p1,p2);
      if((d < k) && (isBetween(p,p1,p2)) )
        return i;
    }
    return -1;
  };

  var onCorner = function(p,w){ //Check on wall corner
    p = w.toLocalPoint(w.group.toLocalPoint(p,'center','center'),'center','center');
    for(var i=0; i < w.points.length;i++)
    {
      var p2 = w.points[i];
      if(p.distanceFrom(p2) < 10)
        return i;
    }
    return -1;
  };
  var pWall;
  var isChangeWall = -1;
  var isChangeCorner = -1;
  var affecteds = [];
  canvas.on("mouse:down",function(e) { //Start change Wall
    if(isPermentsZoom == true || isDrawMode)
      return;
    jQuery(".wall-control").css("display","none");
    jQuery(".object-control").css("display","none");
    jQuery(".dimession").css("display","none");
    jQuery(".delete-button").css("display","none");
    jQuery(".rotate-button").css("display","none")
    if(typeof this.findTarget(e.e) == 'groupLiPolygon')
    {
      e = e.e;
      var f = canvas.getPointer(e);
      if(typeof pWall == 'undefined' || pWall == null)
        pWall = polWall.getActiveObject(f);
      if(typeof pWall == 'undefined')
        return;
      var c = onCorner(f,pWall);
      if(c != -1)
      {
        isChangeCorner = c;
        fx = pWall.toLocalPoint(f,'center','center').x;
        fy = pWall.toLocalPoint(f,'center','center').y;
      }
      var l = onLineWall({x:f.x,y:f.y},pWall);
      if(l != -1)
      {
        isChangeWall = l;
        fx = pWall.toLocalPoint(f,'center','center').x;
        fy = pWall.toLocalPoint(f,'center','center').y;
      }
      //Find affected points
      for (var i = polWall._objects.length - 1; i >= 0; i--) {
        var affected = polWall._objects[i];
        if(i == polWall._objects.indexOf(pWall))
          continue;
        if(c != 1)
        {
          
        }
        else if (l != 1)
        {

        }
      }
    }
  });
  var moX,moY; 
  canvas.on("mouse:move",function(e){ // Change wall line, point position

    if(isHold || isPermentPans || isDrawMode)
      return;
    if(typeof pWall == 'undefined')
      return;
    if(isChangeCorner != -1)
    {
      var i = isChangeCorner;
      e = e.e;
      moX = pWall.toLocalPoint(canvas.getPointer(e),'center','center').x - fx;
      moY = pWall.toLocalPoint(canvas.getPointer(e),'center','center').y - fy;
      fx = pWall.toLocalPoint(canvas.getPointer(e),'center','center').x;
      fy = pWall.toLocalPoint(canvas.getPointer(e),'center','center').y;
      pWall.points[i].x += moX;
      pWall.points[i].y += moY;
    }
    else if (isChangeWall != -1)
    {
      
      var i = isChangeWall,j = (i == pWall.points.length - 1) ? 0 : i + 1;
      e = e.e;
      moX = pWall.toLocalPoint(canvas.getPointer(e),'center','center').x - fx;
      moY = pWall.toLocalPoint(canvas.getPointer(e),'center','center').y - fy;
      fx = pWall.toLocalPoint(canvas.getPointer(e),'center','center').x;
      fy = pWall.toLocalPoint(canvas.getPointer(e),'center','center').y;
      pWall.points[i].x += moX;
      pWall.points[j].x += moX;
      pWall.points[i].y += moY;
      pWall.points[j].y += moY;
    }
    if(affecteds.length > 0)
    {
      for (var i = affecteds.length - 1; i >= 0; i--) {
        var otherMove = polWall._objects[affecteds[i].index];
        var corner = (typeof affecteds[i].corner != 'undefined') ? affecteds[i].corner : null;
        var line = (typeof affecteds[i].wall != 'undefined') ? affecteds[i].wall : null;
        if(corner != null)
        {
          otherMove.points[corner].x += moX;
          otherMove.points[corner].y += moY;
        }
        else if (line != null)
        {
          var lineNext = (line == otherMove.points.length - 1) ? 0 : line + 1;
          otherMove.points[line].x += moX;
          otherMove.points[lineNext].x += moX;
          otherMove.points[line].y += moY;
          otherMove.points[lineNext].y += moY;
        }
      }
    }
  });

  canvas.on("object:selected",function(e){ // Remove the Wall from selected object
    cloneOffset = 10;
    var control = jQuery(".object-control");
    control.find("#button-group").addClass('button-group').removeClass('button-ungroup');
    control.find("#button-group i").addClass('fa-object-group').removeClass('fa-object-ungroup');
    control.find('#object-color').spectrum('set', e.target.hexCode);
    if(canvas._activeGroup == null)
    {
       control.find("#button-group").css("opacity","0.3");
       if(e.target.isUserGroup)
      {
        control.find("#button-group").css("opacity","1");
        control.find("#button-group").removeClass('button-group').addClass('button-ungroup');
        control.find("#button-group i").removeClass('fa-object-group').addClass('fa-object-ungroup');
      }
      if(e.target.isLock === false)
      {
        jQuery(".control-button a, .control-button .uk-button").css("display","inline-block");
        control.find('#button-lock').css({
          display: 'inline-block',
        }).addClass('button-lock').removeClass('button-unlock');
        control.find('#button-lock i').addClass('fa-lock').removeClass('fa-unlock');
      }
      else
      {
        jQuery(".control-button a, .control-button .uk-button").css("display","none");
        control.find('#button-lock').css({
          display: 'inline-block',
        }).removeClass('button-lock').addClass('button-unlock');
        control.find('#button-lock i').removeClass('fa-lock').addClass('fa-unlock');
      }
      return;
    }
    canvas._activeGroup.removeWithUpdate(polWall);
    
    control.css({"display":"block","position":"absolute","left":canvas._activeGroup.left - (control.width()) + "px","top":canvas._activeGroup.top - 50 + "px"});
    //control.find(".product-image").css("display","none");
    control.find("#button-color").css("display","none");
    control.find("#button-group").css("opacity","1");
    // control.find("#_w").val("");
    // control.find("#_h").val("");

  });
  
  jQuery("#saveJSON").click(function(e){ //Save
    e.preventDefault();
    var jsdaa = canvas.toJSON();
    jQuery("#loadArea").val(JSON.stringify(canvas.toJSON(['isLock','srcSVG','hexCode','pathToFill','left','top','strokeWidth','strokeLineCap','fill','hasControls','hasBorders','lockMovementY','lockMovementX','perPixelTargetFind','padding'])));
    UIkit.notify({
    message : '<i class="uk-icon-check"></i> Saved!',
    status  : 'success',
    timeout : 2000,
    pos     : 'top-center'
    });
  });
  
  jQuery("#loadJSON").click(function(e){//Load
    e.preventDefault();
    var jsonString = jQuery("#loadArea").val();
    var JSONData = JSON.parse(jsonString);
    canvas.loadFromJSON(JSONData,canvas.renderAll.bind(canvas),function(o,object){ //o js json object, object is fabric object
      if(object.type == 'liPolygon')
        polWall = object;
    });
  });

  //Control part
  var cloneOffset = 10;
  jQuery(document).on("click",".wall-control #delete-cor",function(e){ //Delete Wall point
    e.preventDefault();
    var idx = jQuery(this).parent(".wall-control").find("#_i").val();
    if(idx != 'undefined')
    {
      polWall.points.splice(idx,1);
      polWall.lineWidths.splice(idx,1);
      jQuery(this).parent(".wall-control").css("display","none");
      canvas.renderAll();
    }
  });

  jQuery(document).on("click",".wall-control #add-cor",function(e){ //Add Wall point
    e.preventDefault();
    var idx = jQuery(this).parent(".wall-control").find("#_i").val();
    idx = parseInt(idx);
    if(idx != 'undefined')
    {
      var px = jQuery(this).parent(".wall-control").find("#_x").val();
      var py = jQuery(this).parent(".wall-control").find("#_y").val();
      if(pWall == null)
      {
        pWall = pWalltmp;
      }
      var s = pWall.toLocalPoint(pWall.group.toLocalPoint({x:px,y:py},'center','center'),'center','center');
      var lineX = pWall.lineWidths[idx];
      pWall.lineWidths.splice(idx + 1,0,lineX);
      pWall.points.splice(idx + 1,0,s);
      canvas.renderAll();
      jQuery(this).parent(".wall-control").find("#_i").val(idx + 1);
      jQuery(this).css("display","none");
      jQuery(this).parent(".wall-control").find("#delete-cor").css("display","block");
      pWall = null;
    }
  });

  jQuery(document).on("click",".object-control #button-clone",function(e){ // Clone objects
    e.preventDefault();
    if(canvas._activeGroup != null) //For group
    {
        return;
    }
    var c = canvas.getActiveObject();
    var cC = fabric.Path.makeClone(c,cloneOffset,canvas); //Create whole new object with c.options not clone
    cloneOffset += 10;
  });

  jQuery(document).on("click",".object-control .button-group",function(e){ //Group objects
    e.preventDefault();
    if(canvas._activeGroup == null)
      return;
    var selecteds = canvas._activeGroup._objects;
    var groups = new fabric.Group(selecteds,{
      left: canvas._activeGroup.left,
      top: canvas._activeGroup.top,
      angle: canvas._activeGroup.getAngle(),
      width: canvas._activeGroup.getWidth(), // For zoom in/out
      height: canvas._activeGroup.getHeight()
    });
    groups.isUserGroup = true;
    groups.isLock = false;
    for (var i = groups._objects.length - 1; i >= 0; i--) {
      var gObj = groups._objects[i];
      gObj.set({
        left: (gObj.left + canvas.viewportTransform[4]), // => Force object stay right place
        top:  (gObj.top + canvas.viewportTransform[5]),
      });
    };
    for (var i = 0; i <= selecteds.length - 1; i++) {
      canvas.remove(selecteds[i]);
    };
    jQuery(this).removeClass('button-group').addClass('button-ungroup');
    jQuery(this).find('i').removeClass('fa-object-group').addClass('fa-object-ungroup');
    canvas.add(groups);
    jQuery(this).closest(".object-control").css("display","none");
    jQuery(".object-button").css("display","none");
    jQuery(".dimession").css("display","none");
    canvas.discardActiveGroup(); // Remove control border
    canvas.discardActiveObject(); // Need both discard
  });

  jQuery(document).on("click",".object-control .button-ungroup",function(e){ //UnGroup Object
     e.preventDefault();
    if(canvas._activeGroup != null)
      return;
    var group = canvas.getActiveObject(),
        center = group.getCenterPoint();
    var a = fabric.util.degreesToRadians(group.getAngle());
    var cosa = Math.cos(a),sina = Math.sin(a);
    for (var i = 0; i <= group._objects.length - 1; i++) {
      var item = group._objects[i].clone(function(item){ // <= use clone and callback function
        item.set({
          left: center.x + (item.left * cosa - item.top * sina), // Set object position 
          top:  center.y + (item.left * sina + item.top * cosa),
          angle: item.getAngle() + group.getAngle(),
          hasControls: true,
          hasBorders:true 
        });
        item.setControlsVisibility({mtr:false,tr:false,bl:false,tl:true,br:true});
        canvas.add(item);
      },['isLock','srcSVG','hexCode','lockScalingX','lockScalingY','lockUniScaling','centeredScaling','centeredScaling','rotatingPointOffset','pathToFill','left','top','strokeWidth','strokeLineCap','fill','hasControls','hasBorders','scaleX','scaleY']);
    };
    canvas.remove(group);
    jQuery(this).addClass('button-group').removeClass('button-ungroup');
    jQuery(this).find('i').addClass('fa-object-group').removeClass('fa-object-ungroup');
    jQuery(this).closest(".object-control").css("display","none");
    jQuery(".object-button").css("display","none");
    jQuery(".dimession").css("display","none");
    canvas.discardActiveGroup(); // Remove control border
    canvas.discardActiveObject(); // Need both discard
  });

  jQuery(document).on("click",".object-control #button-rotate-right",function(e){
    e.preventDefault();
    if(canvas._activeGroup != null) // For group
    {
      return;
    }
    var cR = canvas.getActiveObject();
    cR.rotate(cR.getAngle() + 45);
    cR.setCoords();
    canvas.renderAll();
    updateControl(cR);
  });

  jQuery(document).on("click",".object-control #button-rotate-left",function(e){
    e.preventDefault();
    if(canvas._activeGroup != null) // For group
    {
      return;
    }
    var cR = canvas.getActiveObject();
    cR.rotate(cR.getAngle() - 45);
    cR.setCoords();
    canvas.renderAll();
    updateControl(cR);
  });

  jQuery(document).on("click",".object-control #button-remove",function(e){ // Object remove
    e.preventDefault();
    if(canvas._activeGroup != null) // For group
    {
        for(var i = 0;i < canvas._activeGroup._objects.length;i++)
        {
          canvas.remove(canvas._activeGroup._objects[i]);
        }
        canvas.remove(canvas._activeGroup._objects[0]); //Remove last object
        jQuery(this).closest(".object-control").css("display","none");
        jQuery(".object-button").css("display","none");
        jQuery(".dimession").css("display","none");
        canvas.discardActiveGroup(); // Remove control border
        canvas.discardActiveObject(); // Need both discard
        return;
    }
    var cR = canvas.getActiveObject();
    canvas.remove(cR);
    jQuery(this).closest(".object-control").css("display","none");
    jQuery(".object-button").css("display","none");
    jQuery(".dimession").css("display","none");
  });

   jQuery(document).on("click",".object-control #button-lock",function(e){ // Lock and unlock button
    e.preventDefault();
    if(canvas._activeGroup != null) // For group
    {
        return;
    }
    var cR = canvas.getActiveObject();
    if(cR.isLock === false)
    {
      cR.isLock = true;
      jQuery(".control-button a,.control-button .uk-button").css("display","none");
      jQuery(this).css({
        display: 'inline-block',
      }).removeClass('button-lock').addClass('button-unlock');
      jQuery(this).find('i').removeClass('fa-lock').addClass('fa-unlock');
    }
    else
    {
      cR.isLock = false;
      jQuery(".control-button a,.control-button .uk-button").css("display","inline-block");
      if(typeof cR.pathToFill == 'undefined' || cR.pathToFill.length == 0)
      {
        jQuery("#button-color").css("display","none");
      }
      jQuery(this).addClass('button-lock').removeClass('button-unlock');
      jQuery(this).find('i').addClass('fa-lock').removeClass('fa-unlock');
    }
    updateControl(cR);
   });

  jQuery(document).on("click",".object-control #button-bring-to-front",function(e){ // bring to front button
    e.preventDefault();
    if(canvas._activeGroup != null) // For group
    {
        return;
    }
    var cR = canvas.getActiveObject();
    cR.bringForward(true);
  });

  jQuery(document).on("click",".object-control #button-send-to-back",function(e){ // send to back button
    e.preventDefault();
    if(canvas._activeGroup != null) // For group
    {
        return;
    }
    var cR = canvas.getActiveObject();
    cR.sendBackwards(true);
  });

  jQuery(document).on("change",".object-control #object-color",function(){ //Color-change
    var hexCode = jQuery(this).spectrum('get').toHexString();
    //console.log(hexCode);
    if(canvas._activeGroup != null) //For group
    {
        return;
    }
    var c = canvas.getActiveObject();
    if(c.pathToFill.length > 0)
    {
      c.hexCode = hexCode; //reference for clone
      for(var i = 0; i < c.pathToFill.length;i++)
      {
        var j = c.pathToFill[i];
        c.paths[j].setFill(hexCode);
      }
    }
    c.render(canvas.getContext()); //Don't render here -> it will be rendered bug
  });

 
  //Delete button
  jQuery(".delete-button").on("click",function(){
    if(canvas._activeGroup != null) // For group
    {
        for(var i = 0;i < canvas._activeGroup._objects.length;i++)
        {
          canvas.remove(canvas._activeGroup._objects[i]);
        }
        canvas.remove(canvas._activeGroup._objects[0]); //Remove last object
        jQuery(".object-button").css("display","none");
        jQuery(".dimession").css("display","none");
        jQuery(".object-control").css("display","none");
        canvas.discardActiveGroup(); // Remove control border
        canvas.discardActiveObject(); // Need both discard
        return;
    }
    var cR = canvas.getActiveObject();
    canvas.remove(cR);
    jQuery(".object-button").css("display","none");
    jQuery(".dimession").css("display","none");
    jQuery(".object-control").css("display","none");
  });

  //Rotate Button
  var isRotate = false,rF;
  jQuery(".rotate-button").on('mousedown',function(event) { // Init rotate event
    isRotate = true;
    var control = jQuery(".object-control");
    control.css({
      "display": 'none'
    });
    rF = canvas.getPointer(event);
  });
  canvas.on('mouse:up', function(event) { //Clear rotate evnt
    isRotate = false;
  });
  jQuery(document).on('mouseup', function(event) { //Clear rotate evnt
    isRotate = false;
  });
  canvas.on("mouse:move",function(e){ // Mouse move event when rotate object
   
    if(!isRotate || isDrawMode)
      return;
    var oR = canvas.getActiveObject();
    if(oR == null)
      return;
    var rL = canvas.getPointer(e.e),
        rC = oR.getCenterPoint(),
        curAngle = oR.getAngle(),
        angle = curAngle + calcAngle(rC,rL,rF);
    if(Math.abs(angle % 90) < 3)
    {
      if(angle > 0)
        angle = Math.floor(angle / 90) * 90;
      else
        angle = Math.ceil(angle / 90) * 90;
    }
    else if (Math.abs(angle % 90) > 87)
    {
      if(angle > 0)
        angle = Math.ceil((angle / 90)) * 90;
      else
        angle = Math.floor(angle / 90) * 90;
    }
    oR.rotate(angle);
    oR.setCoords();
    canvas.renderAll();
    updateControl(oR);
    rF = rL;
  });

  jQuery(".close-button").on("click",function(e){
    e.preventDefault();
    jQuery(this).closest('.object-control').hide(50);
  });

  jQuery(window).on('mousewheel DOMMouseScroll', function(event){ // Mouse wheel event - Only work with window object
    if(event.target.nodeName != 'CANVAS')
      return;
    event.preventDefault();
    var c = canvas.getCenter();
    if (event.originalEvent.wheelDelta > 0 || event.originalEvent.detail < 0) {
        // scroll up
        if (z >= 20)
        {
          z = 20;
          return;
        }
        z += 0.05;
    }
    else {
        // scroll down
        if (z <= 0.05)
        {
          z = 0.05;
          return;
        }
        z -= 0.05;
    }
    var oZ = canvas.getActiveObject();
    
    jQuery('.object-control').hide(50);
    canvas.zoomToPoint({x:c.left,y:c.top},z);
    if(oZ != null)
    {
        updateControl(oZ);
    }
  });

  jQuery(document).on("mouseleave","#canvas-holder",function(e){
    e.preventDefault();
    jQuery('.object-control').hide(50);
  });

  ////////////////////////////////////////////////////////////////////////////
  /////////////             Door and window section              /////////////
  ////////////////////////////////////////////////////////////////////////////
  canvas.on("object:moving",function(e){
    var o = e.target;
    e = e.e;
    var oldpWall = null;
    //console.log(e.offsetX + "-" + e.offsetY);
    p = canvas.getPointer(e);
    var l = o.onLine,m;
    var idx = canvas._objects.indexOf(o);

    if(typeof o.onWall == 'undefined')
      return;
    pWalltmp = polWall.getActiveObject(canvas.getPointer(e));
    if(typeof(pWalltmp) != 'undefined' && pWalltmp != null)
    {
      if(pWall != pWalltmp)
       oldpWall = pWall;
      pWall = pWalltmp;
    }

    if(typeof pWall == 'undefined' || pWall == null)
      return;
    if(typeof o.onLine != 'undefined')
    {
      m = (l == pWall.points.length - 1) ? 0 : l + 1;
      if(typeof pWall.points[m].byPassLines != 'undefined')
      {
        pWall.points[m].byPassLines[idx.toString()] = null;
      }
      if(oldpWall != null && typeof oldpWall.points[m].byPassLines != 'undefined')
      {
        oldpWall.points[m].byPassLines[idx.toString()] = null;
      }

    }
    var ang = o.getAngle();
    //console.log(pWall);
    l = onLineWall(p,pWall,15);
    if(l == -1)
    {
      o.onLine = l;
      return;
    }
    if(pWall.points[l].hide == true)
    {
      return;
    }
    if(typeof o.onLine  == 'undefined' || o.onLine == -1)
    {
      if(typeof o.isFlipped == 'undefined' || o.isFlipped == false)
        o.isFlipped = true;
      else if(o.isFlipped == true)
        o.isFlipped = false;
    }
    m = (l == pWall.points.length - 1) ? 0 : l + 1;
    var c = o.getCenterPoint(),
        p1 = pWall.points[l], p2 = pWall.points[m];
    c = pWall.toLocalPoint(pWall.group.toLocalPoint(c,'center','center'),'center','center');
    var dc = getDistance(c,p1,p2);

    var s = c,a;
    if(o.onWall.axis = 'x')
    {
      a =Math.atan((p2.y - p1.y)/(p2.x - p1.x));
      if(o.isFlipped)
        a += Math.PI;
      s.x -= o.onWall.offset * Math.sin(a);
      s.y += o.onWall.offset * Math.cos(a);
      a = fabric.util.radiansToDegrees(a) ;
    }
    else if(o.onWall.axis = 'y')
    {
      a = Math.atan((p2.y - p1.y)/(p2.x - p1.x));
      if(o.isFlipped)
        a += Math.PI;
      s.x += o.onWall.offset * Math.cos(a);
      s.y -= o.onWall.offset * Math.sin(a);
      a = fabric.util.radiansToDegrees(a);
      a = 90 - a;
    }
    var sP = lerp(p1,p2,s);
    var oA = o.getAngle();
    var cM = canvas.getPointer(e);
    o.rotate(a);
    o.set({
      left: o.left + (sP.x - s.x),
      top: o.top + (sP.y - s.y),
      originX: "left",
      originY: "top"
    });
    var oX = canvas._currentTransform.offsetX,
        oY = canvas._currentTransform.offsetY,
        signoA = (a-oA)?(a-oA)<0?1:-1:-1,
        signA = (a) ? (a < 0) ? 1:-1:-1;
    o.onLine = l;
    var xx = pWall.toLocalPoint(pWall.group.toLocalPoint({x:o.left,y:o.top},'center','center'),"center","center");
    //Update mouse offset related to object
    canvas._currentTransform.offsetX = oX * Math.cos(fabric.util.degreesToRadians(Math.abs(a - oA))) +
                                      oY * Math.sin(fabric.util.degreesToRadians(Math.abs(a - oA))) * signoA;
    canvas._currentTransform.offsetY = oY * Math.cos(fabric.util.degreesToRadians(Math.abs(a - oA))) -
                                      oX * Math.sin(fabric.util.degreesToRadians(Math.abs(a - oA))) * signoA;
    if(pWall.doors.indexOf(idx) == -1)
      pWall.doors.push(idx);
    var otl = new fabric.Point(o.left,o.top),
        otr = new fabric.Point(
          o.left + o.getWidth() * Math.cos(fabric.util.degreesToRadians(Math.abs(a))) +
                   o.getHeight() * Math.sin(fabric.util.degreesToRadians(Math.abs(a))) * signA,
          o.top + o.getHeight() * Math.cos(fabric.util.degreesToRadians(Math.abs(a))) -
                  o.getWidth() * Math.sin(fabric.util.degreesToRadians(Math.abs(a))) * signA
        );
    var ltl = pWall.toLocalPoint(pWall.group.toLocalPoint(otl,'center','center'),"center","center"),
        ltr = pWall.toLocalPoint(pWall.group.toLocalPoint(otr,'center','center'),"center","center");
    var lerpTl = lerp(p1,p2,ltl),
        lerpTr = lerp(p1,p2,ltr);
    var lbsp = new fabric.Point(lerpTl.x,lerpTl.y),
        lbep = new fabric.Point(lerpTr.x,lerpTr.y);
    var bypassLine = new fabric.Line();
    bypassLine.x1 = lbsp.x; bypassLine.y1 = lbsp.y;
    bypassLine.x2 = lbep.x; bypassLine.y2 = lbep.y;
    if(typeof pWall.points[m].byPassLines == 'undefined')
      pWall.points[m].byPassLines = new Object;
    pWall.points[m].byPassLines[idx.toString()] = bypassLine;
  });

  canvas.on("mouse:move",function(e){
    if(isChangeWall == -1 && isChangeCorner == -1)
      return;
    //pWall = polWall.getActiveObject(canvas.getPointer(e));
    if(typeof pWall == 'undefined')
    {
      // try
      // {
      //   //pWall = polWall.getActiveObject(canvas.getPointer(e));
      //   if(typeof pWall == 'undefined')
      //     return;
      // }
      // catch(e)
      // {
        return;
      //}
    }
    for (var i = pWall.doors.length - 1; i >= 0; i--) {
      var idx = pWall.doors[i];
      var o = canvas._objects[idx],
          l = o.onLine,
          m = (l == pWall.points.length - 1) ? 0 : l + 1,
          c = o.getCenterPoint(),
          p1 = pWall.points[l], p2 = pWall.points[m];
        c = pWall.toLocalPoint(pWall.group.toLocalPoint(c,'center','center'),'center','center');
        //console.log(e + ',' + o + ',' + p1 + ',' + p2 + ',' + l + ',' + m + ',' + c + ',' + idx);
        updateDoor(e,o,p1,p2,l,m,c,idx);
    };
  });


  var updateDoor = function(e,o,p1,p2,l,m,c,idx){
        var s = c,a;
        if(o.onWall.axis == 'x')
        {
          a = Math.atan((p2.y - p1.y)/(p2.x - p1.x));
          if(o.isFlipped)
           a += Math.PI;
          s.x -= o.onWall.offset * Math.sin(a);
          s.y += o.onWall.offset * Math.cos(a);
          a = fabric.util.radiansToDegrees(a);
        }
        else if(o.onWall.axis == 'y')
        {
          a = Math.atan((p2.y - p1.y)/(p2.x - p1.x));
          if(o.isFlipped)
             a += Math.PI;
          s.x += o.onWall.offset * Math.cos(a);
          s.y -= o.onWall.offset * Math.sin(a);
          a = fabric.util.radiansToDegrees(a);
          a = 90 - a;
        }
        var sP = lerp(p1,p2,s),
            cM = canvas.getPointer(e),
            iM = (isChangeWall == -1) ? isChangeCorner : isChangeWall,
            iN = (iM == 0) ? pWall.points.length - 1 : iM - 1,
            iO = (isChangeWall == -1) ? -1 : (iM == pWall.points.length - 1) ? 0 : iM + 1;
        if(!sP)
          return;
        o.rotate(a);
        if((l == iM) || (l == iN) || (l == iO))
        {
          o.set({
            left: o.left + moX + (sP.x - s.x),
            top: o.top + moY + (sP.y - s.y),
            originX: "left",
            originY: "top"
          });
        }
        else
        {
          o.set({
            left: o.left + (sP.x - s.x),
            top: o.top  + (sP.y - s.y),
            originX: "left",
            originY: "top"
          });
        }
        var signA = (a) ? (a < 0) ? 1:-1:-1;
        var otl = new fabric.Point(o.left,o.top),
        otr = new fabric.Point(
          o.left + o.getWidth() * Math.cos(fabric.util.degreesToRadians(Math.abs(a))) +
                   o.getHeight() * Math.sin(fabric.util.degreesToRadians(Math.abs(a))) * signA,
          o.top + o.getHeight() * Math.cos(fabric.util.degreesToRadians(Math.abs(a))) -
                  o.getWidth() * Math.sin(fabric.util.degreesToRadians(Math.abs(a))) * signA
        );
    var ltl = pWall.toLocalPoint(pWall.group.toLocalPoint(otl,'center','center'),"center","center"),
        ltr = pWall.toLocalPoint(pWall.group.toLocalPoint(otr,'center','center'),"center","center");
        var lerpTl = lerp(p1,p2,ltl),
            lerpTr = lerp(p1,p2,ltr);
        if(!lerpTl || !lerpTr)
          return;
        var lbsp = new fabric.Point(lerpTl.x,lerpTl.y),
            lbep = new fabric.Point(lerpTr.x,lerpTr.y);
        var bypassLine = new fabric.Line();
        bypassLine.x1 = lbsp.x; bypassLine.y1 = lbsp.y;
        bypassLine.x2 = lbep.x; bypassLine.y2 = lbep.y;
        if(typeof pWall.points[m].byPassLines == 'undefined')
          pWall.points[m].byPassLines = new Object;
        pWall.points[m].byPassLines[idx.toString()] = bypassLine;
  }

  var lerp = function(pt1,pt2,pt)
  {
    var r = {};
    var U = ((pt.y - pt1.y) * (pt2.y - pt1.y)) + ((pt.x - pt1.x) * (pt2.x - pt1.x));
    var Udenom = Math.pow(pt2.y - pt1.y, 2) + Math.pow(pt2.x - pt1.x, 2);
    U /= Udenom;
    r.x = pt1.x + (U * (pt2.x - pt1.x));
    r.y = pt1.y + (U * (pt2.y - pt1.y));
    //console.log(r);
    if(  (r.x < pt1.x && r.x < pt2.x) || (r.x > pt1.x && r.x > pt2.x) 
      || (r.y < pt1.y && r.y < pt2.y) || (r.y > pt1.y && r.y > pt2.y) )
      return false;
    return r;
  }

  canvas.on("mouse:up",function(e){
    var o;
    if(typeof pWall == 'undefined' || pWall == null || isDrawMode)
      return;
    for (var i = pWall.doors.length - 1; i >= 0; i--) {
      o = canvas._objects[pWall.doors[i]];
      o.setCoords();
    };
    pWalltmp = pWall;
    pWall = null; 
    canvas.renderAll();
  });
  ////////////////////////////////////////////////////////////////////////////
  /////////////             End door and window section          /////////////
  ////////////////////////////////////////////////////////////////////////////

  ////////////////////////////////////////////////////////////////////////////
  /////////////////            Begin floor section          //////////////////
  ////////////////////////////////////////////////////////////////////////////

    var pattern = null, floorPrice = 0,isFillPattern = false;

   $(document).on("mousedown","*[data-pattern]",function(event){
    pattern = $(this).data('pattern');
    floorPrice = $(this).data('price');
    isFillPattern = true;
   });

  $("html").on("drop", function(e){
      e.preventDefault();
      if(!isFillPattern || isDragable || pattern == null)
        return;
      pWall = polWall.getActiveObject(canvas.getPointer(e.originalEvent));
      if(typeof(pWall) == 'undefined')
        return;
      var price = floorPrice,
          area = (pWall.calcArea() * Math.pow(srcMultiple,2) / 1000000).toFixed(2);
      if(pattern){
      var src = pattern;
        fabric.Image.fromURL(src,function(img){
          img.scaleToWidth(64);
          pWall.fillPattern(img);
          canvas.renderAll();
        });
      }
      // else if($(img).data('color'))
      // {
      //   var color = $(img).data('color');
      //   pWall.setFill(color);
      //   canvas.renderAll();
      // }
      pWall.floorPrice = price;
      jQuery(".object-control").find(".product-price .value").text(currencyFormat(area * pWall.floorPrice));
      pattern = null;
      floorPrice = 0;
      isFillPattern = false;
 });

  jQuery(document).on('mouseup',function(){
    pattern = null;
      floorPrice = 0;
      isFillPattern = false;
  });

  ////////////////////////////////////////////////////////////////////////////
  /////////////////             End floor section           //////////////////
  ////////////////////////////////////////////////////////////////////////////


  ////////////////////////////////////////////////////////////////////////////
  ////////////////              Drawing Section             //////////////////
  ////////////////////////////////////////////////////////////////////////////


  var isDrawMode = false,
  isDrawing = false,
  pointRef = [],
  interSectpoint = new fabric.Circle({
    radius: 0,
    fill: 'rgba(0,0,112,0.6)',
    stroke: '#333',
    hasBorders: false,
    hasControls: false,
    lockMovementX: true,
    lockMovementY: true,
    visible: false,
    originX: "center",
    originY: "center",
  });
  canvas.add(interSectpoint);
  polyLine = {};
  //var FuckingFirstSplice;
  canvas.on("mouse:up",function(e){
    
    if(!isDrawMode) return;
    if(!isDrawing)
    { 
      var firstPoint = {};
      if(interSectpoint.getVisible()){ // Neu intersectpoint xuat hien thi diem dau tien = intersectpoint
        firstPoint = {x: interSectpoint.intersectPoint.x,y: interSectpoint.intersectPoint.y};
        var oIdx = interSectpoint.intersectObject,
            lIdx = interSectpoint.intersectLine,
            inPoint = polWall._objects[oIdx].toLocalPoint(polWall.toLocalPoint({x: firstPoint.x + 5,y: firstPoint.y + 5},"center","center"),"center","center");
        firstPoint.insect = lIdx;
        firstPoint.obj = oIdx;
        polWall._objects[oIdx].points.splice(lIdx, 0, inPoint);
        //FuckingFirstSplice = lIdx;
      }
      else {
        firstPoint = canvas.getPointer(e.e);
        firstPoint.insect = null;
        firstPoint.obj = null;
      }
      //console.log(firstPoint); 
      pointRef.push({x:firstPoint.x - 3.5,y:firstPoint.y - 3.5,insect: firstPoint.insect,obj: firstPoint.obj});
      polyLine = new fabric.Polyline([
        {x: firstPoint.x - 3.5,y:firstPoint.y - 3.5},
        {x: firstPoint.x + 5,y:firstPoint.y + 5},
      ],{
        stroke: 'red',
        strokeWidth: 10,
        fill: 'transparent',
        strokeLineCap: "round",
      });
      canvas.add(polyLine);
      isDrawing = true;
      
    }
    else
    {
      var nextPoint = polyLine.toLocalPoint(canvas.getPointer(e.e));
      if(nextPoint.x == polyLine.points[polyLine.points.length - 2].x && 
         nextPoint.y == polyLine.points[polyLine.points.length - 2].y)
      {
        isDrawing = false;
        var defopts = {
          strokeWidth: 10,
          stroke: "#000000",
          strokeLineCap: "round",
          fill: "#ddd",
          hasControls: true,
          hasBorders: false,
          lockMovementX: true,
          lockMovementY: true,
          perPixelTargetFind: true,
          //originX: "center",
          //originY: "center",
        };
        
        var newRoom = new fabric.LiPolygon(pointRef, defopts,[7]);
        newRoom.isClosed = false;
        polWall.add(newRoom);
        canvas.renderAll();
        canvas.remove(polyLine);
        polyLine = {};
        pointRef = [];
        //isDrawMode = false;
        return;
      }

      // Find insect point
      var polWallLocalPr = polWall.getPoints(),
          firstInsect = null,
          LocalPointer = polWall.getLocalPointer(e.e),
          lastPointRef = pointRef[pointRef.length - 1];
      for (var i = 0; i <= polWallLocalPr.length - 1; i++) {
        var j = (i == polWallLocalPr.length - 1) ? 0 : i + 1;
         if(pointRef.length > 0 && polWallLocalPr[i].i == polWallLocalPr[j].i)
         {
            if(lastPointRef.insect == polWallLocalPr[i].linei && lastPointRef.obj == polWallLocalPr[i].i)
              continue;
            var LocalpointRef1 = polWall.toLocalPoint(lastPointRef,"left","top");
            var insPoint = line_lineIntersect(LocalpointRef1, LocalPointer, polWallLocalPr[i], polWallLocalPr[j]);
            if(insPoint && insPoint.x != LocalpointRef1.x && insPoint.y != LocalpointRef1.y  && pointsDistance(LocalpointRef1,insPoint) > 5)
            {
              if(firstInsect == null || pointsDistance(LocalpointRef1,insPoint) < pointsDistance(LocalpointRef1,firstInsect) ) {
                firstInsect = insPoint;
                firstInsect.intersectLine = polWallLocalPr[i].linei;
                firstInsect.intersectObject = polWallLocalPr[i].i;
              }
            }
         }
      }
      if(firstInsect == null) {
        if(interSectpoint.getVisible()) {
          var insertPoint = {x: interSectpoint.getLeft() - 5,y: interSectpoint.getTop() - 5};
          canvas.remove(polyLine);
          polyLine = {};
          if(interSectpoint.splitPoint)
          {
            var dsplitPoint = {x: insertPoint.x,y: insertPoint.y};
            pointRef.splice(interSectpoint.splitPoint,0,dsplitPoint);
          }
          pointRef.push(insertPoint);
          //pointRef.reverse();
          interSectpoint.set("visible",false);
          isDrawing = false;
          var defopts = {
            strokeWidth: 10,
            stroke: "#000000",
            strokeLineCap: "round",
            fill: "#ddd",
            hasControls: true,
            hasBorders: false,
            lockMovementX: true,
            lockMovementY: true,
            perPixelTargetFind: true,
            //originX: "center",
            //originY: "center",
          };
          //console.log(pointRef);
          splicePointRef = [pointRef[pointRef.length - 1]];
          var firstRpoint = pointRef.pop();
          for (var i = pointRef.length - 1; i >= 0; i--) {
            
            var splicePoint = {x: pointRef[i].x, y: pointRef[i].y};
              splicePointRef.push(splicePoint);
            if(pointRef[i].x != firstRpoint.x || pointRef[i].y != firstRpoint.y)
            {
              pointRef.pop();
              i = pointRef.length;
            }
            else
            {
              var newRoom = new fabric.LiPolygon(splicePointRef, defopts,[7]);  
              splicePointRef = new Array();
              splicePointRef.push(pointRef[i]);
              firstRpoint = pointRef.pop();
              i = pointRef.length;
              polWall.add(newRoom);
              canvas.renderAll();
            }

          }
          
          if(splicePointRef.length > 1) {
            
            //console.log(splicePointRef);
            var lastRoom = new fabric.LiPolygon(splicePointRef, defopts,[7]);
            lastRoom.set("fill","transparent");  
            //lastRoom.set("strokeWidth",7);  
           lastRoom.isClosed = false;
            polWall.add(lastRoom);
            canvas.renderAll();
          }
          //isDrawMode = false;
          pointRef = [];
          return;
        }
        else {
          polyLine.points.push({
            x: nextPoint.x,
            y: nextPoint.y
          });
          pointRef.push(canvas.getPointer(e.e));   
        }
      }
      else {

        isDrawing = false;
        pointRef.push({x:firstInsect.x,y:firstInsect.y});
        pointRef.reverse();
        //pointRef.pop();   
        var oIdx2 = firstInsect.intersectObject,
            lIdx2 = firstInsect.intersectLine,
            defopts = {
              strokeWidth: 10,
              stroke: "#000000",
              strokeLineCap: "round",
              fill: "#ddd",
              hasControls: true,
              hasBorders: false,
              lockMovementX: true,
              lockMovementY: true,
              perPixelTargetFind: true,
              //originX: "center",
              //originY: "center",
            };

        var inPoint2 = polWall._objects[oIdx2].toLocalPoint(polWall.toLocalPoint({x: pointRef[0].x + 5,y:pointRef[0].y + 5},"center","center"),"center","center");
        polWall._objects[oIdx2].points.splice(lIdx2, 0, inPoint2);
        pointRef[0].x -= 3.5;
        pointRef[0].y -= 3.5;
        var newRoom = new fabric.LiPolygon(pointRef, defopts,[7]);
        polWall.add(newRoom);
        canvas.renderAll();
        polWall.redrawPolygons();
        canvas.remove(polyLine);
        canvas.renderAll();
        firstInsect = null;
        pointRef = [];
        return;
      }
      polyLine.setCoords();
    }
    canvas.renderAll();
  });

  canvas.on("mouse:move",function(e){
    var pointer = canvas.getPointer(e.e);
    interSectpoint.set("visible",false);
    canvas.renderAll();
    if(polWall._objects.length > 0) //neu polWall co chua doi tuong thi stick voi doi tuong trong polWall bang interSectpoint
    {
      var polWallLocalPr = polWall.getPoints(),
          firstInsect = null,
          LocalPointer = polWall.getLocalPointer(e.e); 
      //console.log(polWallLocalPr);
      for (var i = 0; i < polWallLocalPr.length - 1; i++) {
        var j = (i == polWallLocalPr.length - 1) ? 0 : i + 1;

         if(getDistance(LocalPointer, polWallLocalPr[i], polWallLocalPr[j]) < 10 
          && polWallLocalPr[i].i == polWallLocalPr[j].i && pointRef.length == 0) {
            var lerpP = lerp(polWallLocalPr[i],polWallLocalPr[j],LocalPointer);
            
              if(lerpP && isDrawMode){
                interSectpoint.set("visible",true);
                interSectpoint.set("left",lerpP.x + 5);
                interSectpoint.set("top",lerpP.y + 5);
                interSectpoint.set("radius",10);
                interSectpoint.intersectLine = polWallLocalPr[i].linei;
                interSectpoint.intersectObject = polWallLocalPr[i].i;
                interSectpoint.intersectPoint = lerpP;
                canvas.renderAll();
                return;
              }
         }
         if(pointRef.length > 0)
         {
            var LocalpointRef1 = polWall.toLocalPoint(pointRef[pointRef.length - 1],"left","top");
            var insPoint = line_lineIntersect(LocalpointRef1, LocalPointer, polWallLocalPr[i], polWallLocalPr[j]);
            if(insPoint)
            {
              firstInsect = (firstInsect == null || pointsDistance(LocalpointRef1,insPoint) < pointsDistance(LocalpointRef1,firstInsect) ) ? insPoint : firstInsect;
            }
         }
      }
    }
    if(!isDrawMode || !isDrawing || typeof polyLine == 'undefined' || polyLine.type != 'polyline') return;
    
    //interSectpoint.set("visible",false);
    var pLinePoints = polyLine.points,
        lastLine = pLinePoints[pLinePoints.length - 2],
        pLinePointer = polyLine.getLocalPointer(e.e);
      for (var i = 0; i <= pLinePoints.length - 4; i++) {
        var j = i + 1;
        //console.log(getDistance(pLinePointer, pLinePoints[i], pLinePoints[j]));
       if(pointRef.length != 0) {
          var insectPoint = line_lineIntersect(pLinePoints[i],pLinePoints[j],lastLine,pLinePointer);
            //console.log('ab');
            if(insectPoint && isDrawMode){ // khong vao duoc day
              //console.log('ad');
              interSectpoint.set("visible",true);
              interSectpoint.set("left",insectPoint.x + 7.5 + polyLine.getLeft());
              interSectpoint.set("top",insectPoint.y + 7.5 + polyLine.getTop());
              interSectpoint.set("radius",10);
              interSectpoint.splitPoint = j;
              interSectpoint.bringToFront();
            }
          canvas.renderAll();
       }
    }

    var nextPoint = polyLine.toLocalPoint(pointer);
    polyLine.points[polyLine.points.length - 1].x = nextPoint.x;
    polyLine.points[polyLine.points.length - 1].y = nextPoint.y;
    

    canvas.renderAll();
  });


  ////////////////////////////////////////////////////////////////////////////
  ////////////////            End drawing Section           //////////////////
  ////////////////////////////////////////////////////////////////////////////


});

var zoom_change = function(e) { //Change zoom level
  var sl = e.target;
  var tx = document.getElementsByName("zoom_value");
  tx[0].value = sl.value;
};

fabric.Path.makeClone = function(o,cOffset,ca){ // Custom clone object function 
  fabric.loadSVGFromURL(o.srcSVG,function(objects,options){
        var c = fabric.util.groupSVGElements(objects,options);
        c.hexCode = o.hexCode;
        c.pathToFill = o.pathToFill; //set pathToFill property
        c.srcSVG = o.srcSVG;
        c.ProName = o.ProName;
        c.zData = o.zData;
        c.realImage = o.realImage;
        c.price = o.price;
        c.isLock = o.isLock;
        c.scale(o.scaleX);
        c.set({
          left: o.left + cOffset,
          top: o.top + cOffset,
          hoverCursor: "move",
          lockUniScaling: true,
          lockScalingFlip: true,
          centeredScaling: true,
          lockScalingX: o.lockScalingX,
          lockScalingY: o.lockScalingY
        });
        if(typeof c.hexCode == 'undefined')
          c.hexCode = "#ffffff";
        if(c.pathToFill.length > 0)
        {
          for(var i = 0; i < c.pathToFill.length;i++)
          {
            var j = c.pathToFill[i];
            c.paths[j].setFill(c.hexCode);
          }
        }
        for (var i = c.paths.length - 1; i >= 0; i--) {
          c.paths[i].strokeWidth = c.paths[i].strokeWidth * 10;
        };
        c.setControlsVisibility({mtr:false,tr:false,bl:false});
        ca.add(c);
      });
}

var getTopPoint = function(o){ // Get smallest Y-point
  var t;
  for(var i in o.oCoords)
  {
    p = o.oCoords[i];
    if(typeof t == 'undefined' || t == null)
    {
      t = p;
    }
    if(t.y > p.y)
    {
      t = p;
    }
  }
  return t;
}


var calcAngle = function(p0,p1,p2) { //Calculate Angle when rotating
  var x0 = p0.x,y0 = p0.y,
      x1 = p1.x,y1 = p1.y,
      x2 = p2.x,y2 = p2.y;
  var angle = Math.atan2((x1-x0)*(y2-y0)-(x2-x0)*(y1-y0),
                (x1-x0)*(x2-x0)+(y1-y0)*(y2-y0));
  return fabric.util.radiansToDegrees(angle*(-1));
}

var updateControl = function(o) { //Update corner control
  var   rotate_button = jQuery(".rotate-button"),
        dimession_width = jQuery(".width-dimession"),
        dimession_height = jQuery(".height-dimession"),
        delete_button = jQuery(".delete-button"),
        container = jQuery("#tutorial");

  rotate_button.css({
      "display": 'block',
      "left": o.oCoords.bl.x - 10 + container.offset().left + "px",
      "top":  o.oCoords.bl.y - 12 + container.offset().top + "px",
      "transform" : "rotate("+o.getAngle()+"deg)"
  });
  delete_button.css({
      "display": 'block',
      "left": o.oCoords.tr.x - 8 + container.offset().left + "px",
      "top":  o.oCoords.tr.y - 8 + container.offset().top + "px"
  });
  var radAngle = fabric.util.degreesToRadians(o.getAngle());
  var dOffsetY = -18;//17 * Math.cos(Math.PI / 2 - radAngle) - 17 * Math.sin(Math.PI / 2 - radAngle),
      dOffsetX = (dimession_width.width() / (-2));//17 * Math.sin(radAngle) - 17 * Math.cos(radAngle);
  dimession_width.css({
      "display": 'block',
      "left": o.oCoords.mt.x + dOffsetX + container.offset().left + "px",
      "top" : o.oCoords.mt.y + dOffsetY + container.offset().top + "px",
      "transform" : "rotate("+o.getAngle()+"deg)",
      //"font-size" : 12 * o.canvas.getZoom() + "px",
      "transform-origin": "center 18px"
  });
  var hOffsetY = -18,
      hOffsetX = (-1) * dimession_height.width() / 2;
  dimession_height.css({
      "display": 'block',
      "left": o.oCoords.mr.x + hOffsetX + container.offset().left + "px",
      "top" : o.oCoords.mr.y + hOffsetY + container.offset().top + "px",
      "transform" : "rotate("+ (o.getAngle() + 90) +"deg)",
      //"font-size" : 12 * o.canvas.getZoom() + "px",
      "transform-origin": "center 18px"
      
  });
   if(o.isLock == true)
  {
    rotate_button.css("display","none");
    delete_button.css("display","none");
    return;
  }
}

var line_lineIntersect =function(l1, l2, l3, l4){
  var p = {},
  x1 = l1.x, y1 = l1.y,
  x2 = l2.x, y2 = l2.y,
  x3 = l3.x, y3 = l3.y,
  x4 = l4.x, y4 = l4.y;
  p.x = ( (x1*y2 - y1*x2)*(x3 - x4) - (x1 - x2)*(x3*y4 - y3*x4) ) / ( (x1 - x2)*(y3 - y4) - (y1 - y2)*(x3 - x4) );
  p.y = ( (x1*y2 - y1*x2)*(y3 - y4) - (y1 - y2)*(x3*y4 - y3*x4) ) / ( (x1 - x2)*(y3 - y4) - (y1 - y2)*(x3 - x4) );
  if( (p.x > x1 && p.x > x2) || (p.x < x1 && p.x < x2) || 
      (p.y > y1 && p.y > y2) || (p.y < y1 && p.y < y2) ||
      (p.x > x3 && p.x > x4) || (p.x < x3 && p.x < x4) || 
      (p.y > y3 && p.y > y4) || (p.y < y3 && p.y < y4) )
    return false;
  return p;
}

var pointsDistance = function(p1,p2){
  return Math.sqrt( Math.pow(p2.x - p1.x,2) +  Math.pow(p2.y - p1.y,2) );
}

var minValue = function(array,property) {
  var min = array[0][property];
  for (var i = array.length - 1; i >= 1; i--) {
    min = Math.min(array[i][property],min);
  };
  return min;
}

var currencyFormat = function(n){
  return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
}