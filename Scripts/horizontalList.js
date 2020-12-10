jQuery(function($){
	'use strict';

	// -------------------------------------------------------------
	//   Basic Navigation
	// -------------------------------------------------------------
	(function () {
		var $frame  = $('#hsscroll1');
		var $slidee = $frame.children('ul').eq(0);
		var $wrap   = $frame.parent();

		// Call Sly on frame
		$frame.sly({
		    horizontal: 1,
		    itemNav: 'basic',
		    smart: 0,
		    scrollBy: 1,
		    mouseDragging: 1,
		    swingSpeed: 0.2,
			scrollBar: $wrap.find('.hsscrollbar'),
		    dragHandle: 1,
			clickBar: 1,
			elasticBounds: 1,
		    speed: 600,
		    startAt: 0,					
		});

	}());

});