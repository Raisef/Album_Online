/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $btnRate1 = $("#rate1"),
        $btnRate2 = $("#rate2"),
        $btnRate3 = $("#rate3"),
        $btnRate4 = $("#rate4"),
        $btnRate5 = $("#rate5"),
        $btnSetRate = $(".rate-btn"),
        $btnNext = $("#nextImage"),
        $btnPrev = $("#prevImage"),
        $rating = $(".rating"),
        $rateBtnPanel = $(".rate-btn-panel"),
        $setTitleBtn = $("#setTitleImage"),
        $removeImageBtn = $("#removeImageBtn"),
        watcher = $btnRate1.attr("name"),
        _requestCompete = true;

    function TryPrev($btnPrev) {
        $btnPrev.removeClass("btn-disabled");
        $btnPrev.removeAttr("disabled");
        if ($(".current-image").is(":first-child")) {
            $btnPrev.attr("disabled", "disabled");
            $btnPrev.addClass("btn-disabled");
        }
    }
    function TryNext($btnNext) {
        $btnNext.removeClass("btn-disabled");
        $btnNext.removeAttr("disabled");
        if ($(".current-image").next().is("button")) {
            $btnNext.attr("disabled", "disabled");
            $btnNext.addClass("btn-disabled");
        }
    }

    TryPrev($btnPrev);
    TryNext($btnNext);

    function GetRates($rating) {
        _requestCompete = false;
        $.ajax({
            type: "POST",
            url: "/Images/Rate",
            data: {
                action: "get",
                watcher: watcher,
                id: $(".current-image").attr("id"),
            },
            success: function (data, statusText, jqXHR) {
                if (jqXHR.status == 200) {
                    if (data != null) {
                        $rating.append(data);
                        if ($rateBtnPanel.length > 0) {
                            var hasWatcher = $(".users-rates>.watcher", $rating);
                            if (hasWatcher.length > 0) {
                                $rateBtnPanel.addClass("hide");
                            }
                            else {
                                $rateBtnPanel.removeClass("hide");
                            }
                        }
                    }
                    _requestCompete = true;
                }
            }
        });
    }

    GetRates($rating);

    function IsTitle() {
        var currentImageId = $(".current-image").attr("id");

        if ($("#setTitleImage").length != 0) {
            var currentTitleId = $("#setTitleImage").val();

            if (currentImageId == currentTitleId) {
                var $titleSign = $('<i id="' + currentImageId + '" class="title-sign glyphicon glyphicon-picture"></i>');

                $("#setTitleImage").replaceWith($titleSign);
            }
        }
        else {
            var $currentTitleSign = $(".title-sign");
                
            if (currentImageId != $currentTitleSign.attr("id")) {
                var $newSetTitleBtn = $('<button id="setTitleImage" class="set-title-btn glyphicon glyphicon-picture" value="' + $currentTitleSign.attr("id") + '"></button>');
                $currentTitleSign.replaceWith($newSetTitleBtn);
            }
        }
    }

    IsTitle();

    $btnPrev.click(function (){
        var $currentImg = $(".current-image"),
            $prevImg = $currentImg.prev();
        $currentImg.removeClass("current-image");
        $prevImg.addClass("current-image");
        $rating.empty();
        GetRates($rating);
        IsTitle();
        TryPrev($btnPrev);
        TryNext($btnNext);
    });

    $btnNext.click(function (){
        var $currentImg = $(".current-image"),
            $nextImg = $currentImg.next();
        $currentImg.removeClass("current-image");
        $nextImg.addClass("current-image");
        $rating.empty();
        GetRates($rating);
        IsTitle();
        TryPrev($btnPrev);
        TryNext($btnNext);
    });

    $btnSetRate.click(function () {
        var currentImgId = $(".current-image").attr("id"),
            userName = $(this).attr("name"),
            rate = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Images/Rate",
            data: {
                action: "set",
                id: currentImgId,
                userName: userName,
                rate: rate
            },
            success: function (data, statusText, jqXHR) {
                if (jqXHR.status == 200) {
                    $rating.empty();
                    GetRates($rating);
                }
            }
        });
    });

    $btnRate2.hover(
        function () { $btnRate1.addClass("shine"); },
        function () { $btnRate1.removeClass("shine"); }
    );
    $btnRate3.hover(
        function () {
            $btnRate1.addClass("shine");
            $btnRate2.addClass("shine");
        },
        function () {
            $btnRate1.removeClass("shine");
            $btnRate2.removeClass("shine");
        }
    );
    $btnRate4.hover(
        function () {
            $btnRate1.addClass("shine");
            $btnRate2.addClass("shine");
            $btnRate3.addClass("shine");
        },
        function () {
            $btnRate1.removeClass("shine");
            $btnRate2.removeClass("shine");
            $btnRate3.removeClass("shine");
        }
    );
    $btnRate5.hover(
        function () {
            $btnRate1.addClass("shine");
            $btnRate2.addClass("shine");
            $btnRate3.addClass("shine");
            $btnRate4.addClass("shine");
        },
        function () {
            $btnRate1.removeClass("shine");
            $btnRate2.removeClass("shine");
            $btnRate3.removeClass("shine");
            $btnRate4.removeClass("shine");
        }
    );

    $(".owner-controls").on("click", ".set-title-btn", function () {
        var currentImageId = $(".current-image").attr("id");
        var $currentBtn = $(this);

        $.ajax({
            type: "POST",
            url: "/Images/SetTitleImage",
            data: {
                id: currentImageId
            },
            success: function (data, statusText, jqXHR) {
                if (jqXHR.status == 200) {
                    var $titleSign = $('<i id="' + currentImageId + '" class="title-sign glyphicon glyphicon-picture"></i>');

                    $currentBtn.replaceWith($titleSign);
                }
            }
        });
    });

    function AfterRemove() {
        var $currentImg = $(".current-image");
        if ($btnNext.attr("disabled") && !$btnPrev.attr("disabled")) {
            var $prevImg = $currentImg.prev();

            $currentImg.remove();
            $prevImg.addClass("current-image");
            $rating.empty();
            GetRates($rating);
            IsTitle();
            TryPrev($btnPrev);
            TryNext($btnNext);
        } else if ($btnPrev.attr("disabled") && !$btnNext.attr("disabled")) {
            var $nextImg = $currentImg.next();

            $currentImg.remove();
            $nextImg.addClass("current-image");
            $rating.empty();
            GetRates($rating);
            IsTitle();
            TryPrev($btnPrev);
            TryNext($btnNext);
        } else {
            $currentImg.remove();
            document.location.href("~");
        }
    }

    $(".owner-controls").on("click", ".remove-image-btn", function () {
        var currentId = $(".current-image").attr("id");
        

        $.ajax({
            type: "POST",
            url: "/Images/Remove",
            data: {
                id: currentId
            },
            success: function (data, statusText, jqXHR) {
                if (jqXHR.status == 200) {
                    AfterRemove();
                }
            }
        });
    });



}());