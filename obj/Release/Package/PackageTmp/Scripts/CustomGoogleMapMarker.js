

function CustomMarker(latlng, map, args) {
	this.latlng = latlng;	
	this.args = args;	
	this.setMap(map);	
}

CustomMarker.prototype = new google.maps.OverlayView();

CustomMarker.prototype.draw = function () {

    alert('custom marker map');
    var self = this;

    var div = this.div;

    if (!div) {

        div = this.div = document.createElement('div');

        div.className = 'marker';

        div.style.position = 'absolute';
        div.style.cursor = 'pointer';
        div.style.width = '200px';
        div.style.height = '200px';
        div.style.background = 'url("speechbubble/about.png") no-repeat rgba(0, 0, 0, 0.4) !important';

        /*
        background: url("images/about.png") no-repeat rgba(0, 0, 0, 0.4) !important;
        background-size: 18px 18px;
        */

        if (typeof (self.args.marker_id) !== 'undefined') {
            div.dataset.marker_id = self.args.marker_id;
        }

        google.maps.event.addDomListener(div, "click", function (event) {
            alert('You clicked on a custom marker!');
            google.maps.event.trigger(self, "click");
        });

        var panes = this.getPanes();
        panes.overlayImage.appendChild(div);
    }

    var point = this.getProjection().fromLatLngToDivPixel(this.latlng);

    if (point) {
        div.style.left = (point.x - 10) + 'px';
        div.style.top = (point.y - 20) + 'px';
    }
};

CustomMarker.prototype.remove = function() {
	if (this.div) {
		this.div.parentNode.removeChild(this.div);
		this.div = null;
	}	
};

CustomMarker.prototype.getPosition = function() {
	return this.latlng;	
};




