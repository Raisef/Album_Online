/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $form = $(".login-form");
    var $login = $("#login");
    var $pass = $("#password");
    var $btnLogin = $("#btnLogin");
    var $loginIncorrect = $("#loginIncorrect");
    var $passIncorrect = $("#passIncorrect");
    var $bothIncorrect = $("#bothIncorrect");
    var $badSignIn = $("#badSignIn");
    
    if ($btnLogin) {
        

        $btnLogin.click(function () {
            var loginRegEx = new RegExp("^[\\w]{2,}$");
            var passRegEx = new RegExp("^[^\\s]{4,}$");
            var logSuccess = loginRegEx.test($login.val());
            var passSuccess = passRegEx.test($pass.val());
            var $returnUrl = $("#returnUrl").val();
            $loginIncorrect.removeClass("show-alert");
            $passIncorrect.removeClass("show-alert");
            $bothIncorrect.removeClass("show-alert");
            $badSignIn.removeClass("show-alert");
            $login.removeClass("input-danger");
            $pass.removeClass("input-danger");

            if (!logSuccess && !passSuccess) {
                $login.addClass("input-danger");
                $pass.addClass("input-danger");
                $bothIncorrect.addClass("show-alert");
            } else if (!logSuccess && passSuccess) {
                $login.addClass("input-danger");
                $loginIncorrect.addClass("show-alert");
            } else if (logSuccess && !passSuccess) {
                $pass.addClass("input-danger");
                $passIncorrect.addClass("show-alert");
            } else {
                $.ajax({
                    type: "POST",
                    url: "/Login",
                    data: {
                        Login: $login.val(),
                        Pass: $pass.val()
                    },
                    success: function (data, statusText, jqXHR) {
                        if (jqXHR.status == 200) {
                            if ($returnUrl) {
                                $(location).attr('href', $returnUrl);
                            }
                            else {
                                $(location).attr('href', "");
                            }
                        }
                    },
                    error: function (jqXHR, textStatus) {
                        if (jqXHR.status == 400) {
                            $badSignIn.addClass("show-alert");
                        }
                    }
                });
            }
        })

        $pass.keyup(function () {
            if (event.keyCode == 13) {
                $btnLogin.click();
            }
        })
    }

}());