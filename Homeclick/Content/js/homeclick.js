$('select[data-child]').on('change', function () {
    var $this = $(this);
    var $child = $($this.data('child'));
    if ($child) {
        if ($child.data('default-value'))
            $child.val($child.data('default-value'));
        else
            $child.val("");
        var $childrenOfSelectChild = $child.children();
        $childrenOfSelectChild.not('[value=""]').hide();
        $child.children().filter('[data-parent="' + $this.val() + '"]').show();
        if ($child.hasClass('chosen'))
            $child.trigger('chosen:updated');
        else {
            $child.val($child.children().filter('[data-parent="' + $this.val() + '"]').first().val()).change();
        }
    }
}).trigger('change');

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

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}
