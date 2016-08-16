/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />
/// <reference path="D:\Project\Album\Album.WebUI\" />

(function () {
    $albumContent = $(".album-images");
    $btnAddImages = $("#btnAddImages");


    $albumContent.on("click", ".set-title-btn", function () {
        var $thisBtn = $(this),
            $currentTitle = $(".title-sign"),
            $titleSign = $('<i id="' + $thisBtn.val() + '" class="title-sign glyphicon glyphicon-picture"></i>'),
            $setTitleBtn;

        $.ajax({
            type: "POST",
            url: "/Images/SetTitleImage",
            data: {
                id: $(this).val()
            },
            success: function (data, statusText, jqXHR) {
                if (jqXHR.status == 200) {
                    $thisBtn.replaceWith($titleSign);
                    if ($currentTitle) {
                        $setTitleBtn = $('<button class="set-title-btn glyphicon glyphicon-picture" value="' + $currentTitle.attr('id') + '"></button>');
                        $currentTitle.replaceWith($setTitleBtn);
                    }
                }
            }
        });
    });

    $albumContent.on("click", ".remove-image-btn", function () {
        var $imageCell = $(this).parent();

        $.ajax({
            type: "POST",
            url: "/Images/Remove",
            data: {
                id: $(this).val()
            },
            success: function (data, statusText, jqXHR) {
                if (jqXHR.status == 200) {
                    $imageCell.remove();
                }
            }
        });
    });

}());
