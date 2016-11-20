var styles = [{ "featureType": "landscape.natural", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#e0efef" }] }, { "featureType": "poi", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "hue": "#1900ff" }, { "color": "#c0e8e8" }] }, { "featureType": "road", "elementType": "geometry", "stylers": [{ "lightness": 100 }, { "visibility": "simplified" }] }, { "featureType": "road", "elementType": "labels", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit.line", "elementType": "geometry", "stylers": [{ "visibility": "on" }, { "lightness": 700 }] }, { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#7dcdcd" }] }];

var pics = null;
var infoWindow = null;
var map = null;
var markerCluster = null;
var markers = [];
var titles = [];
var currentLocMarker = null;
var directionsService = null;
var directionsDisplay = null;

var markerIcon = {
    url: "scripts/plugins/map/marker.png",
};

var markerIcon2 = {

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
    setMarkers(jsonData);
    /*
    addYourLocationButton(map);

    directionsService = new google.maps.DirectionsService;
    directionsDisplay = new google.maps.DirectionsRenderer;
    directionsDisplay.setMap(map);
    directionsDisplay.setOptions({ suppressMarkers: true });
    */
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

function addMarker(location, id) {
    var marker = new google.maps.Marker({
        position: location,
        map: map,
        //icon: markerIcon,
        id: id
    });
    markers.push(marker);
}

function setMapOnAll(map) {
    console.log('clear','clear');
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
    markers = [];
    titles = [];
    markerCluster.clearMarkers();
}

function clearMarkers() {
    setMapOnAll(null);
}

function showMarkers(jsonMarkersData) {
    markerCluster.clearMarkers();
    var result = [];
    for (var i = 0; i < markers.length; i++) {
        var found = false;
        for (var j = 0; j < jsonMarkersData.length; j++) {
            if (markers[i].id == jsonMarkersData[j].id) {
                result.push(markers[i]);
                found = true;
                break;
            }
        }
        if (!found) {
            markers[i].setMap(null);
        }
    }
    markerCluster = new MarkerClusterer(map, result);
}

function setMarkers(jsonMarkersData) {
    
    if (markerCluster) {
        clearMarkers();
    }

    for (var i = 0; i < jsonMarkersData.length; i++) {
        var data = jsonMarkersData[i],
            title = document.createElement('a');

        title.href = '#';
        title.className = 'title';
        title.innerHTML = data.name;
        titles.push(title);

        // Creating a marker and putting it on the map
        geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': data.address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var latLng = results[0].geometry.location;
                addMarker(latLng, jsonMarkersData[markers.length].id);

                if (markers.length == jsonMarkersData.length) {
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

        map.panTo(latlng);
        infoWindow.setContent(infoHtml);
        infoWindow.setPosition(latlng);
        infoWindow.open(map);
        /*
        calculateAndDisplayRoute(latlng);
        */
    };
};

function calculateAndDisplayRoute(targetLatLng) {
    directionsService.route({
        origin: targetLatLng,
        destination: currentLocMarker.position,
        travelMode: google.maps.TravelMode.DRIVING
    }, function (response, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}

function addYourLocationButton(map) {
    var controlDiv = document.createElement('div');

    var firstChild = document.createElement('button');
    firstChild.style.backgroundColor = '#fff';
    firstChild.style.border = 'none';
    firstChild.style.outline = 'none';
    firstChild.style.width = '28px';
    firstChild.style.height = '28px';
    firstChild.style.borderRadius = '2px';
    firstChild.style.boxShadow = '0 1px 4px rgba(0,0,0,0.3)';
    firstChild.style.cursor = 'pointer';
    firstChild.style.marginRight = '10px';
    firstChild.style.padding = '0';
    firstChild.title = 'Your Location';
    controlDiv.appendChild(firstChild);

    var secondChild = document.createElement('div');
    secondChild.style.margin = '5px';
    secondChild.style.width = '18px';
    secondChild.style.height = '18px';
    secondChild.style.backgroundImage = 'url(https://maps.gstatic.com/tactile/mylocation/mylocation-sprite-2x.png)';
    secondChild.style.backgroundSize = '180px 18px';
    secondChild.style.backgroundPosition = '0 0';
    secondChild.style.backgroundRepeat = 'no-repeat';
    firstChild.appendChild(secondChild);

    google.maps.event.addListener(map, 'center_changed', function () {
        secondChild.style['background-position'] = '0 0';
    });

    firstChild.addEventListener('click', function () {
        var imgX = '0',
            animationInterval = setInterval(function () {
                imgX = imgX === '-18' ? '0' : '-18';
                secondChild.style['background-position'] = imgX + 'px 0';
            }, 500);

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                map.setCenter(latlng);

                var marker = new google.maps.Marker({
                    position: latlng,
                    map: map,
                    title: 'Your location',
                    icon: markerIcon2
                });
                currentLocMarker = marker;
                clearInterval(animationInterval);
                secondChild.style['background-position'] = '-144px 0';
            });
        } else {
            clearInterval(animationInterval);
            secondChild.style['background-position'] = '0 0';
        }
    });

    controlDiv.index = 1;
    map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(controlDiv);
}

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

