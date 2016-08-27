function setCoordinates() {
    $('#mapContainer').append($('.imgmapMainImage'));
    $('#mapContainer').children('div').remove();
    $('.imgmapMainImage').removeClass('maphilighted');
    //$('canvas').remove();

    hightlight();
}

function hightlight() {
    $('.imgmapMainImage').maphilight({
        strokeColor: '4F95EA',
        alwaysOn: true,
        fillColor: '365E71',
        fillOpacity: 0.2,
        shadow: true,
        shadowColor: '000000',
        shadowRadius: 5,
        shadowOpacity: 0.6,
        shadowPosition: 'outside'
    });
}

function clearMap() {
    $('#mapContainer').find('map').empty();
    hightlight();
}

function removeOldMapAndValues() {
    if ($('.imgmapMainImage').hasClass('maphilighted')) {
        $('#mapContainer').append($('.imgmapMainImage'));
        $('#mapContainer').children('div').remove();
        $('.imgmapMainImage').removeClass('maphilighted').css('opacity', 1);
        $('#map').children('area').remove();
    }
}