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

//Jquery Extend
$.extend({
    getJsonFromUrl: function (url, urlData) {
        var result = null;
        $.ajax({
            type: "GET",
            url: url,
            data: urlData,
            dataType: "json",
            async: false,
            traditional: true,
            success: function (jsonResult) {
                result = jsonResult;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve data.');
            }
        })
        return result;
    },

    ajaxCall : function (url, urlData) {
    return $.ajax({
        type: "GET",
        url: url,
        data: urlData,
        dataType: "json",
        traditional: true
    })
}
});
