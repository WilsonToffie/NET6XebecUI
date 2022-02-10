function Login() {

    var loginForm = $("#sign-in-form")[0];
    var forgotPwdForm = $("#forgot-password-form")[0];
    var proceed = false;

    if (!loginForm.checkValidity()) {
        if (loginForm.reportValidity) {
            loginForm.reportValidity();
        } else {
            //warn IE users somehow
        }
    }
    else {
        proceed = true;
    }

    if (!forgotPwdForm.checkValidity()) {
        if (forgotPwdForm.reportValidity) {
            forgotPwdForm.reportValidity();
        } else {
            //warn IE users somehow
        }
    }
    else {
        proceed = true;
    }

    return proceed;
}