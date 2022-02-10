function Register() {

    var loginForm = $("#signup-form")[0];
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

    return proceed;
}