fabric.groupLiPolygon = fabric.util.createClass(fabric.Group,{
	type: 'groupLiPolygon',
	childOptions: [],
	initialize: function(pointArray, options, lineWidths) {
		options = options || [];
		this._objects = [];
		for (var i = 0; i <= pointArray.length - 1; i++) {
			var object = new fabric.LiPolygon(pointArray[i],options,lineWidths);
			object.ProName = this.ProName;
			this._objects.push(object);
		};
		options['padding'] = 4294967295;
		this.childOptions = options;
	    this.callSuper('initialize',this._objects, options);
	},
	getActiveObject: function(f){
		f = this.toLocalPoint(f,'center','center');
		for (var i = this._objects.length - 1; i >= 0; i--) {
			    console.log(i + " " +this._objects[i].toLocalPoint(f,'center','center'));
			if(this._objects[i].containsPoint(this._objects[i].toLocalPoint(f,'center','center')))
				return this._objects[i];
		};
	},

	getPoints: function() {
		var points = new Array();
		for (var i = 0; i <= this._objects.length - 1; i++) {
			var objCenterPoint = this._objects[i].getCenterPoint();
			for (var j = 0; j <= this._objects[i].points.length - 1; j++) {
				var insrtPoint = new Object();
				insrtPoint.x = this._objects[i].points[j].x + objCenterPoint.x;
				insrtPoint.y = this._objects[i].points[j].y + objCenterPoint.y;
				insrtPoint.i = i;
				insrtPoint.linei = (this._objects[i].isClosed) ? j + 1: j;
				points.push(insrtPoint);
			}
		}
		return points;
	}, 

	getLines: function() {
		var lines = new Array();
		for (var i = 0; i <= this._objects.length - 1; i++) {
			var objCenterPoint = this._objects[i].getCenterPoint(),
				endPoint = (this._objects[i].isClosed) ? this._objects[i].points.length - 2 : this._objects[i].points.length - 1;
			for (var j = 0; j <= endPoint; j++) {
				if(!this._objects[i].isClosed && j == endPoint)
					continue;
				var k = (j == endPoint) ? 0 : j + 1,
					fPoint = new Object(),
					ePoint = new Object();
				fPoint.x = Math.round( (this._objects[i].points[j].x + objCenterPoint.x) * 1000 ) / 1000;
				fPoint.y = Math.round( (this._objects[i].points[j].y + objCenterPoint.y) * 1000 ) / 1000;
				ePoint.x = Math.round( (this._objects[i].points[k].x + objCenterPoint.x) * 1000 ) / 1000;
				ePoint.y = Math.round( (this._objects[i].points[k].y + objCenterPoint.y) * 1000 ) / 1000;

				lines.push({fPoint: fPoint, ePoint: ePoint, visited: 0,o: i});
			}
		}
		return lines;
	},

	in_array: function(arr,ele){
		var result = false;
		for (var i = 0; i <= arr.length - 1; i++) {
			if(arr[i].x == ele.x && arr[i].y == ele.y)
				result = true;
		}
	return result;
	},

	pointsDistance: function(p1,p2){
	  return Math.sqrt( Math.pow(p2.x - p1.x,2) +  Math.pow(p2.y - p1.y,2) );
	},



	get3PointAngle: function(a,b,c) {
		var vectorA = {x: b.x - a.x, y: b.y - a.y},
			vectorB = {x: c.x - b.x, y: c.y - b.y},
			pXY = vectorA.y * vectorB.x - vectorB.y * vectorA.x,
			dX = pointsDistance(a,b),
			dY = pointsDistance(b,c);
			return Math.acos(pXY / (dX * dY));
	},

	sortEdges: function(edges) {
		var sortedEdges = [],stillRun = true,maxLength = edges.length - 1;
		sortedEdges.push(edges.shift());
		while(sortedEdges.length < maxLength && stillRun) {
			var calcedEdges = [],
				startPoint = sortedEdges[sortedEdges.length - 1].fPoint,
				endPoint = sortedEdges[sortedEdges.length - 1].ePoint;
			stillRun = false;
			for (var i = 0; i <= edges.length - 1; i++) {
				if(edges[i].fPoint.x == endPoint.x && edges[i].fPoint.y == endPoint.y) {
					stillRun = true;
					edges[i].angle = this.get3PointAngle(startPoint,endPoint,edges[i].ePoint);
					console.log(edges[i].angle);
					calcedEdges.push(edges[i]);
					edges.splice(i,1);
				}
				else if(edges[i].ePoint.x == endPoint.x && edges[i].ePoint.y == endPoint.y) {
					stillRun = true;
					var tmp = edges[i].ePoint;
					edges[i].ePoint = edges[i].fPoint;
					edges[i].fPoint = tmp;
					edges[i].angle = this.get3PointAngle(startPoint,endPoint,edges[i].ePoint);
					calcedEdges.push(edges[i]);
					console.log(edges[i].angle);
					edges.splice(i,1);
				}
			}
			for (var f = 0; f <= calcedEdges.length - 1; f++) {
				for(var g = f + 1; g <= calcedEdges.length - 1;g++) {
					if(calcedEdges[f].angle == calcedEdges[g].angle)
					{
						if(pointsDistance(calcedEdges[f].ePoint,calcedEdges[f].fPoint) < pointsDistance(calcedEdges[g].ePoint,calcedEdges[g].fPoint))
						{
							calcedEdges.splice(g,1);
						}
						else
						{
							calcedEdges.splice(f,1);
						}
					}
				}
				
			}
			calcedEdges.sort(function(a,b){
				if(a.angle > b.angle)
					return 1;
				else if(a.angle < b.angle)
					return -1;
				else
					return 0;
			});
			sortedEdges = sortedEdges.concat(calcedEdges);
		}
		return sortedEdges.concat(edges);
	},

	redrawPolygons : function() {
		//var verties = this.getPoints();
		var edges = this.getLines(),
			pols = [],
			//sortedEdges = [],
			lastPop = [],
			branchPoint = {x: null,y: null},
			out = [],
			done = false,
			markedPoint = edges[0].fPoint;
		edges = this.sortEdges(edges);
		while(!done)
		{
			if(out.length == 0) {
				//stackEdge.push(edges[i]);
				out.push(edges[0].fPoint);
				//backedges.push(edges[0].ePoint);
				out.push(edges[0].ePoint);
				edges[0].visited++;
			}
			var beforeCount = out.length,
				adjPoint = out[out.length - 1],
				nearLastPoint = out[out.length - 2];
			for (var i = 1; i <= edges.length - 1; i++) {
				
				if( edges[i].visited < 1 && adjPoint.x == edges[i].fPoint.x && adjPoint.y == edges[i].fPoint.y && 
					nearLastPoint.x != edges[i].ePoint.x && nearLastPoint.y != edges[i].ePoint.y &&
					!this.in_array(lastPop,edges[i].ePoint)) {
					out.push(edges[i].ePoint);
					edges[i].visited++;
					break;
				}
				else if( edges[i].visited < 1 && adjPoint.x == edges[i].ePoint.x && adjPoint.y == edges[i].ePoint.y && 
					nearLastPoint.x != edges[i].fPoint.x && nearLastPoint.y != edges[i].fPoint.y &&
					!this.in_array(lastPop,edges[i].fPoint))
				{
					out.push(edges[i].fPoint);
					edges[i].visited++;
					break;
				}

			}
			var lastCount = out.length, 
				lastPoint = out[out.length - 1];
			//backedges = out[out.length - 2];
			if(lastCount != beforeCount) {
				for (var j = 0; j <= out.length - 2; j++) {
					if(lastPoint.x == out[j].x && lastPoint.y == out[j].y)
					{
						var pol = out.slice(j);
						pols.push(pol);
						if(branchPoint.x != out[out.length - 2].x && branchPoint.y != out[out.length - 2].y){
							lastPop = [];
							branchPoint = out[out.length - 2];
						}
						lastPop.push(out.pop());
						break;
					}
				}
			}
			else
			{
				if(branchPoint.x != out[out.length - 2].x && branchPoint.y != out[out.length - 2].y){
					lastPop = [];
					branchPoint = out[out.length - 2];
				}
				lastPop.push(out.pop());
			}
			if(out.length <= 1)
			{
				done = true;
			}

		}
		//pols = this.removeOverlapping(pols);
		
		var polsWithOffset = [];
		for (var i = pols.length - 1; i >= 0; i--) {
			var polWithOffset = [];
			for (var j = pols[i].length - 1; j >= 0; j--) {
				var xWithOffset = pols[i][j].x + this.getLeft() - 3.5,
					yWithOffset = pols[i][j].y + this.getTop() - 3.5; 
					polWithOffset.push({x: xWithOffset,y:yWithOffset});
			}
			polsWithOffset.push(polWithOffset);
		}
		// polsWithOffset.sort(function(a,b){
		// 	if(a[0].x < b[0].x)
		// 		return -1;
		// 	else if(a[0].y < b[0].y)
		// 		return -1;
		// 	else
		// 		return 1;
		// });
		console.log(polsWithOffset);
		this._objects = [];
		var options = {
			  strokeWidth: 10,
	          stroke: "#000000",
	          strokeLineCap: "round",
	          fill: "#ddd",
	          hasControls: true,
	          hasBorders: false,
	          lockMovementX: true,
	          lockMovementY: true,
	          perPixelTargetFind: true,
		};

		for (var i = polsWithOffset.length - 1; i >= 0; i--) {
			var object = new fabric.LiPolygon(polsWithOffset[i],options,[7]);
			object.ProName = this.ProName;
			this.add(object);
		}
	}
}); 