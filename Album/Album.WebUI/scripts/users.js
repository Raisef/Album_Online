/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $setAvatarBtn = $("#setUserAvatar"),
        $addingPhoto = $(".adding-images"),
        $uploader = $("#uploader"),
        $uploadedField = $(".uploaded-field"),
        $btnUpload = $("#btnUpload"),
        $btnCancelUpload = $("#btnCancelUpload"),
        $popUp = $(".pop-up"),
        $changingPass = $(".changing-pass"),
        $changePassBtn = $("#changePassBtn"),
        $oldPass = $("#oldPass"),
        $newPass = $("#newPass"),
        $passConfirm = $("#passConfirm"),
        $btnChangePass = $("#btnChangePass"),
        $btnCancelChange = $("#btnCancelChange"),
        userId = 0,
        uploadedImage = 0,
        uploading = false;

    function ShowVeil() {
        $popUp.removeClass("show-block");
        $popUp.addClass("show-block");
    };

    $changePassBtn.click(function () {
        userId = $(this).val();
        ShowVeil();
        if (!$changingPass.hasClass("show-block")) {
            $changingPass.addClass("show-block");
        }
    });

    $setAvatarBtn.click(function () {
        userId = $(this).val();
        ShowVeil();
        if (!$addingPhoto.hasClass("show-block")) {
            $addingPhoto.addClass("show-block");
        }
    });

    $uploader.change(function (event) {
        var file = event.target.files[0],
        uploading = true;
        uploadedImage = file;
        picReader = new FileReader();
        picReader.readAsDataURL(file);
        picReader.addEventListener("load", function (event) {
            var picFile = event.target;
            var $newDiv = $('<div class="image-cell"><button class="remove-btn glyphicon glyphicon-remove-sign" title="Remove from upload"></button></div>');
            $newDiv.append('<img class="thumbnail img-responsive img-rounded" src="' + picFile.result + '"/>');
            $uploadedField.append($newDiv);
        });
        uploading = false;
    });

    $uploadedField.on("click", ".remove-btn", function () {
        imagesCount--;
        $(this).parent().remove();

        uploadedImage = 0;
    });

    $btnCancelUpload.click(function () {
        if (!uploading) {
            $uploadedField.empty();
            $popUp.removeClass("show-block");
            $addingPhoto.removeClass("show-block");
        }
    });

    $btnUpload.click(function () {
        if (!uploading && uploadedImage != 0) {
            var form = new FormData();
            form.append("avatar", uploadedImage);
            form.append("Id", userId);
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState != 4) return;
                
                $uploadedField.empty();
                $popUp.removeClass("show-block");
                $addingPhoto.removeClass("show-block");
            };
            xhr.open("POST", "/Users/SetAvatar");
            xhr.send(form);
        }
    });

    $btnCancelChange.click(function () {
        $("changing-pass>input").val("");
        userId = undefined;
        $oldPass.parent().removeClass("input-incorrect");
        $newPass.parent().removeClass("input-incorrect");
        $passConfirm.parent().removeClass("input-incorrect");
        $changingPass.removeClass("show-block");
        $popUp.removeClass("show-block");
    });

    $btnChangePass.click(function () {
            var passRegEx = new RegExp("^[^\\s]{4,}$");
            var oldPassSuccess = passRegEx.test($oldPass.val());
            var newPassSuccess = passRegEx.test($newPass.val());
            var confirmSuccess = $newPass.val() == $passConfirm.val();
            var ok = true;

            $oldPass.parent().removeClass("input-incorrect");
            $newPass.parent().removeClass("input-incorrect");
            $passConfirm.parent().removeClass("input-incorrect");

            if (!oldPassSuccess) {
                ok = false;
                $oldPass.parent().addClass("input-incorrect");
            }
            if (!newPassSuccess) {
                ok = false;
                $newPass.parent().addClass("input-incorrect");
            }
            if (!confirmSuccess) {
                ok = false;
                $passConfirm.parent().addClass("input-incorrect");

            }
            if (ok) {
                $.ajax({
                    type: "POST",
                    url: "/Users/ChangePassword",
                    data: {
                        userId: userId,
                        oldPass: $oldPass.val(),
                        newPass: $newPass.val()
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            $("changing-pass>input").val("");
                            userId = undefined;
                            $changePassBtn.removeClass("show-block");
                            $popUp.removeClass("show-block");
                        }
                    }
                });
            }
    });
}());
