if (jQuery('.detail-slider').length) {

    $('.detail-slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: false,
        asNavFor: '.slider-nav-thumbnails',
    });

    $('.slider-nav-thumbnails').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.detail-slider',
        dots: false,
        arrows: false,
        focusOnSelect: true
    });

    $('.slider-nav-thumbnails .slick-slide').removeClass('slick-active');

    $('.slider-nav-thumbnails .slick-slide').eq(0).addClass('slick-active');

    $('.detail-slider').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
        var mySlideNumber = nextSlide;
        $('.slider-nav-thumbnails .slick-slide').removeClass('slick-active');
        $('.slider-nav-thumbnails .slick-slide').eq(mySlideNumber).addClass('slick-active');
    });
}