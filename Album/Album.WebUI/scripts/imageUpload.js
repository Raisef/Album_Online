/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $addingPhoto = $(".adding-images");
        $uploader = $("#uploader"),
        $uploadedField = $(".uploaded-field"),
        $btnUpload = $("#btnUpload"),
        $btnCancelUpload = $("#btnCancelUpload"),
        $popUp = $(".pop-up"),
        $albumImages = $(".album-images"),
        uploadedImages = [],
        albumId = 0,
        lastId = -1,
        imagesCount = 0,
        uploading = false;

    function ShowVeil() {
        $popUp.removeClass("show-block");
        $popUp.addClass("show-block");
    };

    $(".user-albums").on("click", ".add-images-btn", function () {
        albumId = $(this).val();
        $uploader.click();
        ShowVeil();
        $addingPhoto.removeClass("show-block");
        $addingPhoto.addClass("show-block");
    });

    $("#addImagesBtn").click(function () {
        albumId = $(this).val();
        $uploader.click();
        ShowVeil();
        $addingPhoto.removeClass("show-block");
        $addingPhoto.addClass("show-block");
    });

    $uploader.change(function (event) {
        var files = event.target.files,
            i = 0,
            picReader = [];
        uploading = true;
        for (i = 0; i < files.length; i++) {
            var file = files[i];
            lastId++;
            
            uploadedImages[lastId] = file;

            picReader[i] = new FileReader();
            if (!files[i].type.match("image")) {
                continue;
            }
            picReader[i].readAsDataURL(file);
            (function (lastId) {
                picReader[i].addEventListener("load", function (event) {

                    imagesCount++;
                    var picFile = event.target;
                    var $newDiv = $('<div class="image-cell"><button value="' + lastId + '" class="remove-btn glyphicon glyphicon-remove-sign" title="Remove from upload"></button></div>');
                    $newDiv.append('<img class="thumbnail img-responsive img-rounded" src="' + picFile.result + '"/>');
                    $uploadedField.append($newDiv);
                });
            })(lastId);
        }
        uploading = false;
    });

    $uploadedField.on("click", ".remove-btn", function () {
        var id = $(this).val();
        imagesCount--;
        $(this).parent().remove();

        delete uploadedImages[id];
    });

    $btnCancelUpload.click(function () {
        if (!uploading) {
            albumId = 0;
            lastId = -1;
            imagesCount = 0;
            $uploadedField.empty();

            $popUp.removeClass("show-block");
            $addingPhoto.removeClass("show-block");
        }
    });

    $btnUpload.click(function () {
        if (!uploading && imagesCount != 0) {
            var form = new FormData(),
                i = 0;
            for (i; i < uploadedImages.length; i++) {
                if (uploadedImages[i] == undefined) {
                    continue;
                }
                else {
                    form.append(albumId, uploadedImages[i]);
                }
            }
            form.append("albumId", albumId);
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState != 4) return;
                if ($albumImages.length != 0) {
                    $albumImages.append(xhr.responseText);
                } else {
                    var albumImagesCount = $(".album-cell#" + albumId + ">.album-count").text();
                    albumImagesCount-=0-imagesCount;
                    $(".album-cell#" + albumId + ">.album-count").text(albumImagesCount);
                }
                albumId = 0;
                lastId = -1;
                imagesCount = 0;
                $uploadedField.empty();
                $popUp.removeClass("show-block");
                $addingPhoto.removeClass("show-block");
            };
            xhr.open("POST", "/Images/UploadingImages");
            xhr.send(form);
        }
    });
    
}());