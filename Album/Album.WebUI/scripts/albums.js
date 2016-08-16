/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $creatingAlbum = $(".creating-album"),
        $newAlbumName = $("#newAlbumName"),
        $createAlbumBtn = $("#createAlbumBtn"),
        $createBtn = $("#btnCreateNewAlbum"),
        $userAlbums = $(".user-albums"),
        $btnCreateCancel = $("#btnCreateCancel"),
        $renamingAlbum = $(".renaming-album"),
        $reNameInput = $("#reName"),
        $btnRenameAlbum = $("#btnRenameAlbum"),
        $btnCancelRename = $("#btnCancelRename"),
        $addingTag = $(".adding-tag"),
        $newTag = $("#newTag"),
        $btnAddTag = $("#btnAddTag"),
        $btnCancelAddingTag = $("#btnCancelAddingTag"),
        albumId;


    
    $userAlbums.on("click", ".delete-album-btn", function () {
        if (confirm("Do you really want to delete album with all images?")) {
            albumId = $(this).val();
            if (albumId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/Album/Delete",
                    data: {
                        albumId: albumId
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            $("div#" + albumId).remove();
                            albumId = 0;
                        }
                    }
                });
            }
        }
    });

    $userAlbums.on("click", ".remove-tag", function () {
        var tagId = $(this).val(),
            $currentBtn = $(this);
        albumId = $(this).closest("div.album-cell").attr("id");

        if (tagId > 0 && albumId > 0) {
            $.ajax({
                type: "POST",
                url: "/Album/RemoveTag",
                data: {
                    albumId: albumId,
                    tagId: tagId
                },
                success: function (data, statusText, jqXHR) {
                    if (jqXHR.status == 200) {
                        if ($currentBtn.is(":last-child")) {
                            $currentBtn.prevUntil("button.remove-tag").remove();
                            $currentBtn.replaceWith('<span class="no-tags">No tags yet.</span>');
                        }
                    }
                }
            });
        }
        albumId = 0;
    });

    function ShowVeil($showArea) {
        if (!$popUp.hasClass("show-block")) {
            $popUp.addClass("show-block");
        }
        if (!$showArea.hasClass("show-block")) {
            $showArea.addClass("show-block");
        }
    }

    $userAlbums.on("click", ".rename-album-btn", function () {
        albumId = $(this).val();
        ShowVeil($renamingAlbum);
    });

    $userAlbums.on("click", ".add-tag-btn", function () {
        albumId = $(this).val();
        ShowVeil($addingTag);
    });

    $createAlbumBtn.click(function () {
        ShowVeil($creatingAlbum);
    });

    $btnCreateCancel.click(function () {
        $newAlbumName.val("");
        $popUp.removeClass("show-block");
        $creatingAlbum.removeClass("show-block");
        $newAlbumName.removeClass("danger-border");
        $newAlbumName.siblings(".incorrect-input").removeClass("show-block");
        $newAlbumName.siblings(".incorrect-name").removeClass("show-block");
    });

    function InputAlertClear($input) {
        if ($input.hasClass("danger-border")) {
            $input.removeClass("danger-border");
        }
        if ($input.siblings(".incorrect-input").hasClass("show-block")) {
            $input.siblings(".incorrect-input").removeClass("show-block");
        }
        if ($input.siblings(".incorrect-name").hasClass("show-block")) {
            $input.siblings(".incorrect-name").removeClass("show-block");
        }
    };

    $createBtn.click(function () {
        var albumName = $newAlbumName.val(),
            userId = $createAlbumBtn.val(),
            nameRegEx = new RegExp("^[^\\s](.{0,48}[^\\s])?$");

        InputAlertClear($newAlbumName);

        if ($newAlbumName.length == 0 || !nameRegEx.test(albumName)) {
            if (!$newAlbumName.hasClass("danger-border")) {
                $newAlbumName.addClass("danger-border");
            }
            if (!$newAlbumName.siblings(".incorrect-input").hasClass("show-block")) {
                $newAlbumName.siblings(".incorrect-input").addClass("show-block");
            }
        } else {
            var existAlbum = $(".album-name:contains(" + albumName + ")");
            if (existAlbum.length != 0) {
                if (!$newAlbumName.hasClass("danger-border")) {
                    $newAlbumName.addClass("danger-border");
                }
                if (!$newAlbumName.siblings(".incorrect-name").hasClass("show-block")) {
                    $newAlbumName.siblings(".incorrect-name").addClass("show-block");
                }
            } else {
                $.ajax({
                    type: "POST",
                    url: "/Album/Create",
                    data: {
                        userId: userId,
                        albumName: albumName
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            if (data != "") {
                                $userAlbums.append(data);
                                $popUp.removeClass("show-block");
                                $creatingAlbum.removeClass("show-block");
                            }
                        }
                    }
                });
            }
        }
    });

    $newAlbumName.keyup(function () {
        if (event.keyCode == 13) {
            $createBtn.click();
        }
    });

    $btnCancelRename.click(function () {
        $reNameInput.val("");
        $popUp.removeClass("show-block");
        $renamingAlbum.removeClass("show-block");
        InputAlertClear($reNameInput);
    });

    $btnRenameAlbum.click(function () {
        var albumName = $reNameInput.val(),
            nameRegEx = new RegExp("^[^\\s](.{0,48}[^\\s])?$");

        InputAlertClear($reNameInput);

        if ($reNameInput.length == 0 || !nameRegEx.test(albumName)) {
            if (!$reNameInput.hasClass("danger-border")) {
                $reNameInput.addClass("danger-border");
            }
            if (!$reNameInput.siblings(".incorrect-input").hasClass("show-block")) {
                $reNameInput.siblings(".incorrect-input").addClass("show-block");
            }
        } else {
            var existsAlbum = $(".album-name:contains(" + albumName + ")");
            
            if (existsAlbum.length != 0) {
                var isExist = false;
                for(var i = 0; i < existsAlbum.length;i++){
                    if (existsAlbum[i].text == albumName) {
                        isExist = true;
                        break;
                    }
                }
                if (isExist) {
                    if (!$reNameInput.hasClass("danger-border")) {
                        $reNameInput.addClass("danger-border");
                    }
                    if (!$reNameInput.siblings(".incorrect-name").hasClass("show-block")) {
                        $reNameInput.siblings(".incorrect-name").addClass("show-block");
                    }
                    return;
                }                
            }
            $.ajax({
                type: "POST",
                url: "/Album/Rename",
                data: {
                    albumId: albumId,
                    albumName: albumName
                },
                success: function (data, statusText, jqXHR) {
                    if (jqXHR.status == 200) {
                        $("div#" + albumId + ">.album-info>.album-name>a").text(albumName);
                        $reNameInput.val("");
                        $popUp.removeClass("show-block");
                        $renamingAlbum.removeClass("show-block");
                    }
                }
            });
        }
    });

    $reNameInput.keyup(function () {
        if (event.keyCode == 13) {
            $btnRenameAlbum.click();
        }
    });

    $btnCancelAddingTag.click(function () {
        albumId = undefined;
        $newTag.val("");
        $popUp.removeClass("show-block");
        $addingTag.removeClass("show-block");
        InputAlertClear($newTag);
    });

    $btnAddTag.click(function () {
        var tagName = $newTag.val(),
            nameRegEx = new RegExp("^[^\\s](.{0,48}[^\\s])?$");
        InputAlertClear($newTag);

        if ($newTag.length == 0 || !nameRegEx.test(tagName)) {
            if (!$newTag.hasClass("danger-border")) {
                $newTag.addClass("danger-border");
            }
            if (!$newTag.siblings(".incorrect-input").hasClass("show-block")) {
                $newTag.siblings(".incorrect-input").addClass("show-block");
            }
        } else {
            var existsTag = $("div#" + albumId + " .tag-name:contains(" + tagName + ")");
            if (existsTag.length != 0) {
                var isExist = false;
                for (var i = 0; i < existsTag.length; i++) {
                    if (existsTag[i].text == tagName) {
                        isExist = true;
                        break;
                    }
                }
                if (isExist) {
                    if (!$newTag.hasClass("danger-border")) {
                        $newTag.addClass("danger-border");
                    }
                    if (!$newTag.siblings(".incorrect-name").hasClass("show-block")) {
                        $newTag.siblings(".incorrect-name").addClass("show-block");
                    }
                    return;
                }                
            }
            $.ajax({
                type: "POST",
                url: "/Album/AddTag",
                data: {
                    albumId: albumId,
                    tagName: tagName
                },
                success: function (data, statusText, jqXHR) {
                    if (jqXHR.status == 200) {
                        if (data != 0) {
                            var $albumTags = $("div#" + albumId + ">.album-info>.album-tags"),
                                $noTagsSpan = $albumTags.children(".no-tags");
                            if ($noTagsSpan.length == 0) {
                                $albumTags.append($('<span class="comma">,</span>' + data));
                            } else{
                                $noTagsSpan.replaceWith(data);
                            }
                        }
                        $newTag.val("");
                        $popUp.removeClass("show-block");
                        $addingTag.removeClass("show-block");
                    }
                }
            });
        }
    });

    $newTag.keyup(function () {
        if (event.keyCode == 13) {
            $btnAddTag.click();
        }
    });
    
}());