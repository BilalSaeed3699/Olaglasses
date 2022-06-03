$(function () {
	$(window).on('load', function(event) {
        $('#preloader').delay(300).fadeOut(300);
    });
	//   $(".virtual-btn").click(function () {

	//     $(".virtual-model").slideToggle();
	// });
	$(".virtual-btn").click(function () {
		$(".virtual-model").addClass("virtual-show");
	});

	$(".virtual-x").click(function () {
		$(".virtual-model").removeClass("virtual-show");
	});
	$(".virtual-btn").click(function () {
		$("body").addClass("body-overflow");
	});

	$(".virtual-x").click(function () {
		$("body").removeClass("body-overflow");
	});
	$(".click-aria").click(function () {
		$(".dropdown--menu").toggleClass("click-show");
	});
	$(".click-aria").click(function () {
		$(".glasses-item-one").toggleClass("click-show-bg");
	});
	// 
	$(".click-aria2").click(function () {
		$(".dropdown--menu3").toggleClass("click-show2");
	});
	$(".click-aria2").click(function () {
		$(".glasses-item.glasses-item-tow.text-center").toggleClass("click-show-bg2");
	});
	// 
	$(".click-aria3").click(function () {
		$(".dropdown--menu4").toggleClass("click-show3");
	});
	$(".click-aria3").click(function () {
		$(".glasses-item.glasses-item-three.text-center").toggleClass("click-show-bg3");
	});
	$('.blog-aria').slick({
		dots: false,
		arrows: true,
		prevArrow: ' ',
		nextArrow: ' <img class="next" src="../assets/img/model-slider-icon.png">',
		infinite: true,
		speed: 600,
		slidesToShow: 2,
		slidesToScroll: 2,
		vertical: true,
		responsive: [{
				breakpoint: 1199.98,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 1,
					infinite: true,
					dots: false
				}
			},
			{
				breakpoint: 991.98,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 1,
					infinite: true,
					dots: false,
				}
			},
			{
				breakpoint: 767.98,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 1,
					dots: false,
					// vertical: false,
				}
			},
			{
				breakpoint: 575.98,
				settings: {
					dots: false,
					slidesToShow: 1,
					slidesToScroll: 1
				}
			}

			// You can unslick at a given breakpoint now by adding:
			// settings: "unslick"
			// instead of a settings object
		]
	});
	$('#search_content_dsk').click(function () {

		$('#overlay_search').slideToggle();
	});

	$('.hero-aria-bg').slick({
		dots: true,
		arrows: false,
		infinite: true,
		speed: 300,
		slidesToShow: 1,
		slidesToScroll: 1,
		responsive: [{
				breakpoint: 1199.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					infinite: true,
					dots: true
				}
			},
			{
				breakpoint: 991.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					infinite: true,
					dots: true
				}
			},
			{
				breakpoint: 767.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1
				}
			},
			{
				breakpoint: 575.98,
				settings: {
					dots: false,
					slidesToShow: 1,
					slidesToScroll: 1
				}
			}
			// You can unslick at a given breakpoint now by adding:
			// settings: "unslick"
			// instead of a settings object
		]
	});
	$('.about-we-right-slider').slick({
		dots: true,
		arrows: false,
		infinite: true,
		speed: 300,
		slidesToShow: 1,
		slidesToScroll: 1,
		responsive: [{
				breakpoint: 1199.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					infinite: true,
					dots: true
				}
			},
			{
				breakpoint: 991.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					infinite: true,
					dots: false
				}
			},
			{
				breakpoint: 767.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					dots: false,
				}
			},
			{
				breakpoint: 575.98,
				settings: {
					dots: false,
					slidesToShow: 1,
					slidesToScroll: 1
				}
			}

			// You can unslick at a given breakpoint now by adding:
			// settings: "unslick"
			// instead of a settings object

		]
	});
	$('.proccessing-right-slider').slick({
		dots: true,
		arrows: false,
		infinite: true,
		adaptiveHeight: true,
		speed: 300,
		slidesToShow: 1,
		slidesToScroll: 1,
		// vertical: true,
		responsive: [{
				breakpoint: 1199.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					infinite: true,
					dots: true
				}
			},
			{
				breakpoint: 991.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					infinite: true,
					dots: true,
				}
			},
			{
				breakpoint: 767.98,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
					dots: true,
				}
			},
			{
				breakpoint: 575.98,
				settings: {
					dots: true,
					slidesToShow: 1,
					slidesToScroll: 1
				}
			}
			// You can unslick at a given breakpoint now by adding:
			// settings: "unslick"
			// instead of a settings object
		]
	});

	$('.venobox').venobox();


});


// $(document).ready(function () {
//   // MDB Lightbox Init
//   $(function () {
//     $("#mdb-lightbox-ui").load("mdb-addons/mdb-lightbox-ui.html");
//   });
// });
$(window).scroll(function () {
	if ($(window).scrollTop() > 400) {
		$(".has-radious").fadeIn()
	} else {
		$(".has-radious").fadeOut()
	}

})

$(".backtotop").click(function () {
	$("html,body").animate({
		scrollTop: 0,
	})

})

$(document).ready(function () {
	// MDB Lightbox Init
	$(function () {
		$("#mdb-lightbox-ui").load("mdb-addons/mdb-lightbox-ui.html");
	});
});
AOS.init();
$(window).scroll(function () {
	if ($(window).scrollTop() > 50) {
		$(".menu").addClass("fixd-manu")
	} else {
		$(".menu").removeClass("fixd-manu")
	}
})
$(window).scroll(function () {
	if ($(window).scrollTop() > 100) {
		$(".shape").addClass("shape-fixd-manu")
	} else {
		$(".shape").removeClass("shape-fixd-manu")
	}
})