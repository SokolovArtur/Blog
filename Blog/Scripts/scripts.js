function StickyFooter() {
    var heightBrowser = $(window).height();
    var heightContainerBlock = $(document).height();

    (heightContainerBlock <= heightBrowser) ? $("footer").addClass("fixed-bottom") : $("footer").removeClass("fixed-bottom");
}

$(document).ready(function () {
    StickyFooter();

    $(window).resize(function () {
        StickyFooter();
    });

    // Плавное появление формы
    $("#account").css({
        "opacity": "1",
        "transform": "scale(1)",
        "transition": ".5s",
        "-webkit-transform": "scale(1)",
        "-webkit-transition": ".5s"
    });

    // Блокирует доступ и изменение поля формы
    $(".input-control.disabled").children("input, textarea, select, button").attr("disabled", "disabled");
});