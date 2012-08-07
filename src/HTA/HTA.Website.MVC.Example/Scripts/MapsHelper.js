/// <reference path="google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="google-maps-3-vs-1-0.js" />

function init_draggable_map(map_canvas_id, lat, lng, zoomLevel, title, latBox, lngBox, icon) {

    var cafeIcon = new google.maps.MarkerImage();
    cafeIcon.url = icon;

    lat = parseFloat(lat);
    lng = parseFloat(lng);

    var myLatLng = new google.maps.LatLng(lat, lng);

    var options = {
        zoom: zoomLevel,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.HYBRID
    };

    var marker = {
        title: title,
        position: new google.maps.LatLng(lat, lng),
        icon: cafeIcon,
        draggable: true
    };

    var map_canvas = document.getElementById(map_canvas_id);

    var map = new google.maps.Map(map_canvas, options);

    var marker = new google.maps.Marker(marker);
    marker.setMap(map);

    google.maps.event.addListener(marker, 'drag', function () {
        document.getElementById(latBox).value = marker.getPosition().lat();
        document.getElementById(lngBox).value = marker.getPosition().lng();
    });
    google.maps.event.addListener(marker, 'dragend', function () {
        
        document.getElementById(latBox).value = marker.getPosition().lat();
        document.getElementById(lngBox).value = marker.getPosition().lng();
    });
}

function getMarkerFromAddress(icon) {
    var address = document.getElementById('Location_Address').value;
    getMarkerFromAddress(icon, address);
}
function getMarkerFromAddress(icon, address) {

    alert("address: " + address);
    var geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            placeMarker(results[0].geometry.location, icon);
        } else {
            alert('Sorry, could not find that one. Try again (' + status + ')');
        }
    });
}

function placeMarker(position, icon) {

    document.getElementById('my_map').style.display = 'block';
    init_draggable_map('my_map', position.lat(), position.lng(), 20, '', 'Location_Lat', 'Location_Lon', icon);

    document.getElementById('Location_Lat').value = position.lat();
    document.getElementById('Location_Lon').value = position.lng();
}

/***************************************/

function IndexPageGoogleMapsInitialize() {
    map = null;
    infowindow = new google.maps.InfoWindow();

    markers = new Array();
    address = new Array();

    var mapOptions = {
        zoom: 3,     /// Tweak this to change the zoom scale of the edit windows.
        mapTypeId: google.maps.MapTypeId.TERRAIN,
        center: new google.maps.LatLng(40, -100)
    };

    map = new google.maps.Map(document.getElementById("divGoogleMap"), mapOptions);
}

function map_addMarker(id, lat, lon, description, title, icon, url) {

    var position = new google.maps.LatLng(lat, lon);

    var marker = new google.maps.Marker({
        map: map,
        position: position,
        title: unescape(title),
        icon: icon
    });

    google.maps.event.addListener(marker, "click", function () {

        var clickableTitle = '<b>' + url + '</b>';
        var latText = 'Lat: ' + lat;
        var lonText = 'Lon: ' + lon;
        var text = clickableTitle + '<br />' + description + '<br />' + latText + '<br />' + lonText
        infowindow.setContent(text);
        infowindow.open(map, marker);

    });

    markers[id] = marker;
}

clusteredMarkers = [];
function map_addClusteredMarker(id, lat, lon, description, title, icon, url) {
    var position = new google.maps.LatLng(lat, lon);

    var marker = new google.maps.Marker({
        position: position,
        title: unescape(title),
        icon: icon
    });

    google.maps.event.addListener(marker, "click", function () {

        var clickableTitle = '<b>' + url + '</b>';
        var latText = 'Lat: ' + lat;
        var lonText = 'Lon: ' + lon;
        var text = clickableTitle + '<br />' + description + '<br />' + latText + '<br />' + lonText
        infowindow.setContent(text);
        infowindow.open(map, marker);

    });

    clusteredMarkers.push(marker);
}

function map_finalizeClusteredMarkers() {
    var markerCluster = new MarkerClusterer(map, clusteredMarkers);
}