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
    $(".product-info input[name='quantity']").TouchSpin({
        min: 1,
        max:99
    });
    $(".btn-add-to-cart").click(function () {
        var id = $(this).data("id");
        var quantity = $("#quantity").val();
        $.ajax({
            url: "/AddToCartQuantity",
            type: "POST",
            data: { id: id, quantity: quantity },
            success: function (response) {
                loadCart(true);
                if (window.innerWidth < 768)
                    document.documentElement.classList.add("prevent-scroll");
                cartWrapper.classList.remove("hidden");
                setTimeout(() => {
                    cartContent.classList.remove("translate-x-full");
                    cartWrapper.style.setProperty("--tw-bg-opacity", 0.5);
                }, 300);
            }
        });
    });
}