
$(document).ready(function () {
    alert("2222")
            //Zoom single product
            $(".zoom_03").elevateZoom(
                  {
                      gallery: 'gallery_01',
                      cursor: 'pointer',
                      galleryActiveClass: "active",
                      imageCrossfade: true,
                      loadingIcon: "",
                      zoomWindowWidth: 400,
                      zoomWindowHeight: 400,
                      responsive: true,
                      borderSize: 1

                  });

            $(".zoom_03").bind("click", function (e) {
                var ez = $('.zoom_03').data('elevateZoom');
                ez.closeAll(); //NEW: This function force hides the lens, tint and window
                $.fancybox(ez.getGalleryList());
                return false;
            });

          
        });
