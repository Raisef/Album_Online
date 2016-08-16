/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $searchAlbums = $("#searchAlbums"),
        $searchUsers = $("#searchUsers"),
        $showAllTags = $("#allTags"),
        $usersField = $(".searching-users"),
        $albumsField = $(".searching-albums"),
        $tagsField = $(".all-tags"),
        $commonScreen = $(".common-screen"),
        $userName = $("#userName"),
        $albumInput = $("#albumInput"),
        $searchToggle = $("#searchToggle"),
        $startSearching = $(".start-searching"),
        $alertUser = $(".incorrect-input", ".searching-users"),
        $alertAlbum = $(".incorrect-input", ".searching-albums");


    if ($(".album-cell").length > 0) {
        $albumsField.addClass("show-block");
    }
    if ($(".user-cell").length > 0) {
        $usersField.addClass("show-block");
    }

    $searchToggle.change(function () {
        if ($searchToggle.is(":checked")) {
            $(".by-name").removeClass("show-span");
            $(".by-tag").addClass("show-span");
        }
        if (!$searchToggle.is(":checked")) {
            $(".by-name").addClass("show-span");
            $(".by-tag").removeClass("show-span");
        }
    });

    $searchAlbums.click(function () {
        $commonScreen.removeClass("show-block");
        $searchUsers.parent().removeClass("active");
        $showAllTags.parent().removeClass("active");
        $searchAlbums.parent().addClass("active");
        $usersField.removeClass("show-block");
        $tagsField.removeClass("show-block");
        $albumsField.addClass("show-block");
    });

    $searchUsers.click(function () {
        $commonScreen.removeClass("show-block");
        $showAllTags.parent().removeClass("active");
        $searchAlbums.parent().removeClass("active");
        $searchUsers.parent().addClass("active");
        $tagsField.removeClass("show-block");
        $albumsField.removeClass("show-block");
        $usersField.addClass("show-block");
    });

    $showAllTags.click(function () {
        $commonScreen.removeClass("show-block");
        $searchUsers.parent().removeClass("active");
        $searchAlbums.parent().removeClass("active");
        $showAllTags.parent().addClass("active");
        $usersField.removeClass("show-block");
        $albumsField.removeClass("show-block");
        $tagsField.addClass("show-block");
    });

    $startSearching.click(function () {
        var from = $(this).val();
        
        if (from == 'users') {
            $searchResults = $(".search-results", ".searching-users");
            var namePath = $userName.val();
            $alertUser.removeClass("show-span");
            $userName.removeClass("danger-input");
            if (namePath.length == 0) {
                $alertUser.addClass("show-span");
                $userName.addClass("danger-input");
            } else {
                $searchResults.empty();
                $.ajax({
                    type: "POST",
                    url: "/Search/Index",
                    data: {
                        obj : 'user',
                        userName: namePath
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            if (data != "") {
                                $searchResults.append(data);
                            }
                        }
                    }
                });
            }
        } else if (from == 'albums') {
            var albumInput = $albumInput.val(),
                isTagId = $searchToggle.is(":checked"),
                $searchResults = $(".search-results", ".searching-albums");

            $alertAlbum.removeClass("show-span");
            $albumInput.removeClass("danger-input");

            if ((albumInput.length == 0) || (isTagId && albumInput <= 0)) {
                $alertAlbum.addClass("show-span");
                $albumInput.addClass("danger-input");
            } else if (isTagId && albumInput.length > 0) {
                $searchResults.empty();
                $.ajax({
                    type: "POST",
                    url: "/Search/Index",
                    data: {
                        obj: 'album',
                        tagName: albumInput
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            if (data != "") {
                                $searchResults.append(data);
                            }
                        }
                    }
                });
            } else if (!isTagId && albumInput.length > 0) {
                $searchResults.empty();
                $.ajax({
                    type: "POST",
                    url: "/Search/Index",
                    data: {
                        obj: 'album',
                        albumName: albumInput
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            if (data != "") {
                                $searchResults.append(data);
                            }
                        }
                    }
                });
            }
        }
    });

    $albumInput.keyup(function () {
        if (event.keyCode == 13) {
            $('.start-searching[value="albums"]').click();
        }
    });

    $userName.keyup(function () {
        if (event.keyCode == 13) {
            $('.start-searching[value="users"]').click();
        }
    });
}());