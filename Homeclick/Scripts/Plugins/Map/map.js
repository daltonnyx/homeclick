var styles = [{ "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] }, { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }] }, { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": -100 }, { "lightness": 45 }] }, { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }] }, { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#46bcec" }, { "visibility": "on" }] }];

var pics = null;
var infoWindow = null;
var map = null;
var markerCluster = null;
var markers = [];
var titles = [];

var markerIcon = {
    url: "/scripts/plugins/map/marker.png",
    // This marker is 20 pixels wide by 32 pixels high.


};

function mapInit(jsonData)
{
    var styledMap = new google.maps.StyledMapType(styles, { name: "Styled Map" });

    var customMapTypeId = 'custom_style';
    var myOptions =
      {
          zoom: 6,
          center: new google.maps.LatLng(16.059360770014724, 108.17491027700804),
          mapTypeControlOptions: {
              mapTypeId: [google.maps.MapTypeId.ROADMAP, customMapTypeId]
          }
      };

    map = new google.maps.Map(document.getElementById('gmap_canvas'), myOptions);

    infoWindow = new google.maps.InfoWindow();

    map.mapTypes.set(customMapTypeId, styledMap);
    map.setMapTypeId(customMapTypeId);
    showMarkers(jsonData);

    google.maps.event.addListener(infoWindow, 'domready', function () {

        // Reference to the DIV which receives the contents of the infowindow using jQuery
        var iwOuter = $('.gm-style-iw');

        /* The DIV we want to change is above the .gm-style-iw DIV.
         * So, we use jQuery and create a iwBackground variable,
         * and took advantage of the existing reference to .gm-style-iw for the previous DIV with .prev().
         */
        var iwBackground = iwOuter.prev();

        // Remove the background shadow DIV
        iwBackground.children(':nth-child(2)').css({ 'display': 'none' });

        // Remove the white background DIV
        iwBackground.children(':nth-child(4)').css({ 'display': 'none' });

        iwBackground.children(':nth-child(2)').css({ 'display': 'none' });
        // Moves the shadow of the arrow 76px to the left margin 
        iwBackground.children(':nth-child(1)').attr('style', function (i, s) { return s + '' });

        // Moves the arrow 76px to the left margin 
        iwBackground.children(':nth-child(3)').attr('style', function (i, s) { return s + 'z-index:9999' });


        var iwCloseBtn = iwOuter.next();

        // Apply the desired effect to the close button
        iwCloseBtn.css({
            opacity: '1', // by default the close button has an opacity of 0.7
            right: '40px', top: '17px', // button repositioning
        });
    });
}



function showMarkers(jsonMarkersData) {

    if (markerCluster) {
        markerCluster.clearMarkers();
        markers = [];
        titles = [];
    }
    //jsonData = jsonMarkersData;
    var panel = document.getElementById('markerList');
    panel.innerHTML = '';
    for (var i = 0; i < jsonMarkersData.length; i++) {
        var data = jsonMarkersData[i],
        //Add to list
         item = document.createElement('li'),
         title = document.createElement('a');

        title.href = '#';
        title.className = 'title';
        title.innerHTML = data.name;
        titles.push(title);
        item.appendChild(title);
        panel.appendChild(item);

        // Creating a marker and putting it on the map
        geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': data.address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var latLng = results[0].geometry.location;
                var marker = new google.maps.Marker({
                    position: latLng,
                    map: map,
                    title: data.name,
                    icon: markerIcon
                });
                markers.push(marker);
                var i = jsonMarkersData.length;
                if (markers.length == i) {
                    for (var i = 0; i < markers.length; i++) {
                        var fn = markerClickFunction(jsonMarkersData[i], markers[i].position);
                        google.maps.event.addListener(markers[i], 'click', fn);
                        google.maps.event.addDomListener(titles[i], 'click', fn);
                    }
                    markerCluster = new MarkerClusterer(map, markers);
                }
            }
        });
    }
}

var markerClickFunction = function(data, latlng) {
    return function(e) {
        e.cancelBubble = true;
        e.returnValue = false;
        if (e.stopPropagation) {
            e.stopPropagation();
            e.preventDefault();
        }

        var infoHtml = '<div class="info project-tile">' +
                            '<h3>' + data.name + '</h3>' +
                            '<div class="project-tile-image fullscreener fsr-container info-body">' +
                                '<img src="' + data.image + '" class="attachment-project-image size-project-image wp-post-image fsr-image fsr-hidden" alt="Helsinki-Central-Library">' +
                                
                                '<div class="project-tile-overlay">' +
                                    '<a href="' + data.link + '" class="button tiny">Xem chi tiết <i class="fa fa-search" aria-hidden="true"></i></a>' +
                                '</div>' +
                                '</div>' +
                        '</div>';

        map.setZoom(12);
        map.panTo(latlng);
        infoWindow.setContent(infoHtml);
        infoWindow.setPosition(latlng);
        infoWindow.open(map);
    };
};

//Filters

function filterByField(data, fieldName, fieldValue) {
    return jLinq.from(data).contains(fieldName, fieldValue).select()
}

function filterByCity(data, city) {
    return jLinq.from(data).match("city", city).select();
}

function filterByType(data, type) {
    return jLinq.from(data).match("type", type).select()
}

function filterByStatu(data, statu) {
    return jLinq.from(data).match("statu", statu).select();
}

