$('.table-section-header').click(function () {
    $(this).toggleClass("active-show");
    $(this).parent('.product-list-section').addClass("list-show");
    $('.list-show .category-product-detail').slideToggle("slow");
    event.preventDefault();
});

//Slider
if (jQuery().owlCarousel) {
    $(".owl-carousel").owlCarousel({
        slideSpeed: 300,
        paginationSpeed: 400,
        singleItem: true
    });
}

