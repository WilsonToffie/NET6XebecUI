function ProfileApplication() {

    var myform = $("#personal-information-form")[0];
    var proceed = false;
    if (!myform.checkValidity()) {
        if (myform.reportValidity) {
            $('#online-tag').removeClass('active');
            $('#online-profile').removeClass('active');
            $('#personal-tab').addClass('active');
            $('#personal-information').addClass('active');
            myform.reportValidity();
        } else {
            //warn IE users somehow
        }
    }
    else {
        proceed = true;
    }

    return proceed;
}