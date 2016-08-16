/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $addingPhoto = $(".adding-photo");
        $uploader = $("#uploader"),
        $uploadedField = $(".uploaded-field"),
        $btnUpload = $("#btnUpload"),
        $btnCancelUpload = $("#btnCancelUpload"),
        $popUp = $(".pop-up")
        uploadedImages = {},
        albumId = 0,
        lastId = 0,
        uploading = false;

        $(".user-albums").on("click", ".add-photo-btn", function () {
        albumId = $(this).val();
        $popUp.removeClass("show-block");
        $popUp.addClass("show-block");
        $addingPhoto.removeClass("show-block");
        $addingPhoto.addClass("show-block");
    });

    $uploader.change(function (event) {
        var files = event.target.files,
            i = 0;
        uploading = true;
        for (i = 0; i < files.length; i++) {
            var file = files[i];
            var picReader = new FileReader();
            if (!files[i].type.match("image")) {
                continue;
            }
            
            picReader.addEventListener("load", function (event) {
                lastId++;
                var picFile = event.target;
                var $newDiv = $('<div class="image-cell"><button value="' + lastId + '" class="remove-btn glyphicon glyphicon-remove-sign"></button></div>');
                $newDiv.append('<img class="thumbnail img-responsive img-rounded" src="' + picFile.result + '"/>');
                $uploadedField.append($newDiv);
                uploadedImages[lastId] = {AlbumId : albumId, Content : picFile.result, Type : file.type};
            });
             file;
            picReader.readAsDataURL(file);
        }
        uploading = false;
    });

    $uploadedField.on("click", ".remove-btn", function () {
        var id = $(this).val();

        $(this).parent().remove();

        delete uploadedImages[id];
    });

    $btnCancelUpload.click(function () {
        if (!uploading) {
            albumId = 0;
            uploadedImages = {};
            $uploadedField.empty();

            $popUp.removeClass("show-block");
            $addingPhoto.removeClass("show-block");
        }
    });

    $btnUpload.click(function () {
        if (!uploading && uploadedImages.length != 0) {
            var i = 1;
            for (i; i <= lastId; i++) {
                if (!uploadedImages[i]) {
                    continue;
                }
                var jsonData,
                    iData = [];
                iData[0] = uploadedImages[i].AlbumId;
                iData[1] = uploadedImages[i].Content;
                iData[2] = uploadedImages[i].Type;
                jsonData = JSON.stringify({ images: iData });
                $.ajax({
                    type: "POST",
                    url: "WebServices.asmx/UploadImages",
                    data: jsonData,
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data != null && data.d) {
                            console.log("Uploaded");
                            //albumId = 0;
                            //uploadedImages = {};
                            //$uploadedField.empty();

                            //$popUp.removeClass("show-block");
                            //$addingPhoto.removeClass("show-block");;
                        }
                        else {
                            console.log("show-alert");
                        }
                    }
                });
            }
        }
    });
    
}());