/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $deleteBtn = $(".delete-user"),
        $changingRoleBtn = $(".change-role"),
        $changeAcceptBtn = $("#btnChangeRole"),
        $cancelChangingBtn = $("#btnCancelChangeRole"),
        $popUp = $(".pop-up"),
        $changingRole = $(".changing-role"),
        userId,
        $currentRole;

    $(".admin-toolbox").on("click", ".change-role", function () {
        $currentRole = $(".role>input[value =" + $(this).val() + "]");
        userId = $(this).siblings("button.delete-user").val();
        if (!$popUp.hasClass("show-block")) {
            $popUp.addClass("show-block");
        }
        if (!$changingRole.hasClass("show-block")) {
            $changingRole.addClass("show-block");
        }
        if (!$currentRole.attr("disabled")) {
            $currentRole.attr("disabled", "disabled");
        }
        if (!$currentRole.parent().hasClass("current-role")) {
            $currentRole.parent().addClass("current-role");
        }
    });

    $cancelChangingBtn.click(function () {
        var $roleChooise = $('input[name=roles]:checked', '.roles-field');
        userId = undefined;
        if ($popUp.hasClass("show-block")) {
            $popUp.removeClass("show-block");
        }
        if ($changingRole.hasClass("show-block")) {
            $changingRole.removeClass("show-block");
        }
        if ($currentRole.attr("disabled")) {
            $currentRole.removeAttr("disabled");
        }
        if ($currentRole.parent().hasClass("current-role")) {
            $currentRole.parent().removeClass("current-role");
        }
        $currentRole = undefined;
        if ($roleChooise) {
            $roleChooise.prop("checked", false);
        }
    });

    $changeAcceptBtn.click(function () {
        var $roleChooise = $('input[name=roles]:checked', '.roles-field');
        if ($roleChooise && userId && userId > 0) {
            $.ajax({
                type: "POST",
                url: "/Users/ChangeRole",
                data: {
                    userId: userId,
                    newRole: $roleChooise.val()
                },
                success: function (data, statusText, jqXHR) {
                    if (jqXHR.status == 200) {
                        $(".delete-user[value = " + userId + "]").siblings(".change-role").val($roleChooise.val());
                        $roleChooise.prop("checked", false);
                        userId = undefined;
                        if ($popUp.hasClass("show-block")) {
                            $popUp.removeClass("show-block");
                        }
                        if ($changingRole.hasClass("show-block")) {
                            $changingRole.removeClass("show-block");
                        }
                        if ($currentRole.attr("disabled")) {
                            $currentRole.removeAttr("disabled");
                        }
                        if ($currentRole.parent().hasClass("current-role")) {
                            $currentRole.parent().removeClass("current-role");
                        }
                        $currentRole = undefined;
                    }
                }
            });
        }
    });

    $deleteBtn.click(function () {
        if (confirm("Do you really want to delete this user? It permanently delete all his albums!")) {
            var userId = $(this).val();
            var $currentBtn = $(this);
            if (userId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/Users/Delete",
                    data: {
                        userId: userId                        
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            $currentBtn.closest("div.user-cell").remove();
                        }
                    }
                });
            }
        }
    });
}());