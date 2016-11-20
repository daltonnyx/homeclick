//Custom Polygon
//http://stackoverflow.com/questions/24384804/fabric-js-subclassing-fabric-group-error-cannot-read-property-async-of-und

'use strict';
if(typeof(fabric) == 'undefined')
  var fabric;
fabric.LiPolygon = fabric.util.createClass(fabric.Polygon,{ //Need to assign class to fabric
  type: 'liPolygon',                                            // Otherwise loadfromJSON wont work
  lineWidths: null, //Define separate width for each line,
  doors: null,
  points: null,
  originalPoints: null,
  isClosed: true,
  minX: 0,
  minY: 0,
  initialize: function(points, options, lineWidths) {
    options = options || { };
    this.points = points || [];
    this.originalPoints = JSON.parse(JSON.stringify(points)) || [];
    this.lineWidths = lineWidths || [ ];
    //this.pointColors = pointColors || [ ];
    this.doors = [];

    this.callSuper('initialize', points, options);
    this._calcDimensions();
    if( this.points.length < 1 || this.points[0].x != this.points[this.points.length - 1].x || this.points[0].y != this.points[this.points.length - 1].y)
      this.isClosed = false;
    if (!('top' in options)) {
      this.top = this.minY;
    }
    if (!('left' in options)) {
      this.left = this.minX;
    }
  },

  _calcDimensions: function() {

      var points = this.points,
          minX = fabric.util.array.min(points, 'x'),
          minY = fabric.util.array.min(points, 'y'),
          maxX = fabric.util.array.max(points, 'x'),
          maxY = fabric.util.array.max(points, 'y');

      this.width = (maxX - minX) || 0;
      this.height = (maxY - minY) || 0;

      this.minX = minX || 0,
      this.minY = minY || 0;
    },

  _render: function(ctx) {
     //this._renderFill(ctx);
     if(this.isClosed) {
       if(!this.callSuper("commonRender",ctx))
        return;
       this._renderFill(ctx);
      if (this.stroke || this.strokeDashArray) {
      ctx.closePath();
      this._renderStroke(ctx);
      //ctx.stroke();
     }

    }

     //this.callSuper('_renderFill',ctx);

     if(!this.commonRender(ctx))
      return;

  },

  makePoint : function(rPoints) {
    for (var i = rPoints.length - 1; i >= 0; i--) {
      this.points.unshift({x: rPoints[i].x, y: rPoints[i].y });
    }
  },
  /**
   * @private
   * @param {CanvasRenderingContext2D} ctx Context to render on
   */
  commonRender: function(ctx) {
    var point, len = this.points.length;
    // if(this.isClosed == false)
    // {
    //   len--;
    // }
    if (!len || isNaN(this.points[len - 1].y)) {
      // do not draw if no points or odd points
      // NaN comes from parseFloat of a empty string in parser
      return false;
    }
    //console.log(this.points.length);
    //console.log(this.points);
    ctx.beginPath();
    if (this._applyPointOffset) {
      if (!(this.group && this.group.type === 'path-group')) {
        this._applyPointOffset();
      }
      this._applyPointOffset = null;
    }

    for (var i = 1; i <= len; i++) {
      var j = i;
      //this._renderStroke(ctx);
      if(typeof(this.lineWidths[j-1]) != 'undefined' && this.lineWidths[j-1] != null) {
        //ctx.save();
        //ctx.beginPath();
        this._setStrokeStyles(ctx,this.lineWidths[j-1]);
      }
      if(i == len && this.isClosed)
        j = 0;
      if(i == len && !this.isClosed)
        return true;
      point = this.points[j];
      ctx.moveTo(this.points[i-1].x, this.points[i-1].y);
      if(typeof point.byPassLines != 'undefined')
      {
        var byPassLines = point.byPassLines;
        var startPoint = this.points[i - 1];
        var sortLine = [];
        for(var x in byPassLines)
        {
          var bLine = byPassLines[x];
          if(bLine == null)
            continue;
          sortLine.push(bLine);
        }
        sortLine.sort(function(a,b){
          var aP = new fabric.Point(a.x1,a.y1);
          var bP = new fabric.Point(b.x1,b.y1);
          if(aP.distanceFrom(startPoint) <= bP.distanceFrom(startPoint))
            return -1;
          else
            return 1;
        });
        for (var k = 0; k < sortLine.length;k++){
          var bLine = sortLine[k];
          var sp = new fabric.Point(bLine.x1,bLine.y1), ep = new fabric.Point(bLine.x2,bLine.y2);
          var isReversed = (sp.distanceFrom(startPoint) >= ep.distanceFrom(startPoint));
          if(isReversed)
          {
            var temp = sp;
            sp = ep;
            ep = temp;
          }
          ctx.lineTo(sp.x,sp.y);
          ctx.moveTo(ep.x,ep.y);
        };

      }
      if(!this.points[i - 1].hide)
        ctx.lineTo(point.x, point.y);
      else
        ctx.moveTo(point.x, point.y);
      ctx.stroke();
    }
    return true;
  },

  _applyPointOffset: function() {
      // change points to offset polygon into a bounding box
      // executed one time
      this.points.forEach(function(p) {
        p.x -= (this.minX + this.width / 2);
        p.y -= (this.minY + this.height / 2);
      }, this);
    },

  _setStrokeStyles: function(ctx,lineWidth){
      this.strokeWidth = lineWidth;
      this.callSuper('_setStrokeStyles',ctx);
  },
  toObject: function(propertiesToInclude){ // Keep fix this shit
      return fabric.util.object.extend(this.callSuper('toObject', propertiesToInclude), {
        originalPoints: this.get('originalPoints'),
        lineWidths: this.get('lineWidths')
    });
  },
  calcArea: function() {
    var j,j1,j2,x1,x2,y1t,y2t,Stotal = 0,Ssub = 0;
    var lop = [];
    var k = this.points.length;
    while(k--) { lop[k] = this.points[k]; }
    //lop.push(this.points[0]);
    for (var i = 0; i <= lop.length - 1; i++) {
      j = (i == lop.length - 1) ? 0 : i + 1;
      j1 = ((j + 1) == lop.length) ? 0 : j + 1;
      j2 = ((j1 + 1) == lop.length) ? 0 : j1 + 1;
      var pi = lop[i],
          pj = lop[j],
          pj1 = lop[    j1],
          pj2 = lop[j2];
      var a = (pj1.y - pj.y) / (pj1.x - pj.x),
          b = pj.y - (a * pj.x);
      x1 = pi.x;
      x2 = pj2.x;
      y1t = a * x1 + b;
      y2t = a * x2 + b;
      if((y1t - pi.y) * (y2t - pj2.y) < 0)
      {
        Ssub += this.retangleArea(pj,pj1,pj2);
        lop.splice(j1,1);
        i--;
      }
    };
    for (var i = 2; i <= lop.length - 1; i++) {
      Stotal += this.retangleArea(lop[0],lop[i - 1],lop[i]);
    };
    return Stotal - Ssub;

  },
  retangleArea: function(a,b,c){
    var s;
    s = Math.abs((a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) / 2);
    return s;
  },
  fillPattern: function(patternImg){
    var patternCanvas = new fabric.StaticCanvas();
    patternCanvas.add(patternImg);
    var pattern = new fabric.Pattern({
      source: function(){
        patternCanvas.setDimensions({
        width: patternImg.getWidth(),
        height:patternImg.getHeight()
        });
        return patternCanvas.getElement();
      },
      repeat: "repeat"
    });
    this.callSuper('setFill',pattern);
  },
  containsPoint: function(f){
    var isInside = false;
    var pointCount = this.points.length - 1,points = this.points;
    var j = pointCount;
    //console.log(points);
    for (var i = 0; i <= pointCount; i++) {
      if ((points[i].y < f.y && points[j].y >= f.y
          || points[j].y < f.y && points[i].y >= f.y)
    &&  (points[i].x <= f.x || points[j].x <= f.x)) {
      if (points[i].x + (f.y - points[i].y) / (points[j].y - points[i].y) * (points[j].x - points[i].x) < f.x)
        isInside = !isInside;
      }
      j = i;
    }
    return isInside;
  }
});

fabric.LiPolygon.fromObject = function(object){
    return new fabric.LiPolygon(object.points, object, object.lineWidths, true);
};
