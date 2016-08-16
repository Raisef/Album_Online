/// <reference path="jquery-3.1.0.min.js" />
/// <reference path="jquery-3.1.0.intellisense.js" />

(function () {
    var $form = $(".reg-form");
    var $nick = $("#nickname");
    var $pass = $("#password");
    var $passConfirm = $("#confirmPass");
    var $btnSignUp = $("#btnSignUp");
    

    if ($btnSignUp) {

        $btnSignUp.click(function () {
            var nickRegEx = new RegExp("^[\\w]{2,}$");
            var passRegEx = new RegExp("^[^\\s]{4,}$");
            var nickSuccess = nickRegEx.test($nick.val());
            var passSuccess = passRegEx.test($pass.val());
            var confirmSuccess = $pass.val() == $passConfirm.val();
            var ok = true;

            $nick.parent().removeClass("input-incorrect");
            $pass.parent().removeClass("input-incorrect");
            $passConfirm.parent().removeClass("input-incorrect");

            if (!nickSuccess) {
                ok = false;
                $nick.parent().addClass("input-incorrect");
            } 
            if (!passSuccess) {
                ok = false;
                $pass.parent().addClass("input-incorrect");
            }
            if (!confirmSuccess) {
                ok = false;
                $passConfirm.parent().addClass("input-incorrect");
           
            } 
            if (ok) {
                $form.submit();
            }
        })

        $form.keyup(function () {
            if (event.keyCode == 13) {
                $btnSignUp.click();
            }
        })
    }

}());