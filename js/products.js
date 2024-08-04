$('.sort-cate-right h3').on('click', function (e) {
    e.preventDefault();
    if ($(window).width() <= 991) {
        var $this = $(this);
        $this.parents('.sort-cate-right').find('ul').stop().slideToggle();
        $(this).toggleClass('active');
        return false;
    }
});

$(window).on('resize', function() {
    if ($(window).width() > 991) {
        $('.sort-cate-right ul').slideDown();
    } else {
        $('.sort-cate-right ul').slideUp();
    }
});
