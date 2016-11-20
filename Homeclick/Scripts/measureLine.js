'use strict';

if(typeof fabric == 'undefined')
  var fabric;

fabric.measureLine =  fabric.util.createClass(fabric.Line,{
  type: 'measureLine',
  initialize: function(points, options) {
      options || (options = { });
      this.callSuper('initialize', points, options);

  },
  _render: function(ctx) {
    this.callSuper('_render', ctx);

    ctx.fillStyle = '#666';
    var length = this.lineLength();
    var label = length.toFixed(2) + " cm";
    var angle = this.calcAngleWithX();
    var p = this.calcLinePoints();
    var offset = 7;
    ctx.moveTo(p.x1 + offset * Math.sin(this.calcAngleWithX()),p.y1 - offset * Math.cos(this.calcAngleWithX()));
    ctx.lineTo(p.x1 - offset * Math.sin(this.calcAngleWithX()),p.y1 + offset * Math.cos(this.calcAngleWithX()));
    ctx.moveTo(p.x2 + offset * Math.sin(this.calcAngleWithX()),p.y2 - offset * Math.cos(this.calcAngleWithX()));
    ctx.lineTo(p.x2 - offset * Math.sin(this.calcAngleWithX()),p.y2 + offset * Math.cos(this.calcAngleWithX()));
    ctx.stroke();
    console.log(this.x1 + ',' + this.x2);
    angle = ( this.x1 > this.x2 || (this.x1.toFixed(2) == this.x2.toFixed(2) && this.y1 > this.y2 ) ) ? angle + Math.PI : angle;
    ctx.rotate(angle);
    ctx.font = 13 /  this.canvas.getZoom() + "px Arial";
    ctx.fillText(label, 0, -5);
  },
  lineLength: function(){
    return Math.sqrt( Math.pow(this.x2 - this.x1,2) +  Math.pow(this.y2 - this.y1,2) );
  },
  calcAngleWithX: function() { //Calculate Angle when rotating
    var deltaY = this.y2 - this.y1,
        deltaX = this.x2 - this.x1,
        angle = Math.atan2(deltaY, deltaX);
    return angle;
  }
});

fabric.measureLine.calcLength = function(p1,p2) {
  return Math.sqrt( Math.pow(p2.x - p1.x,2) +  Math.pow(p2.y - p1.y,2) );
}
