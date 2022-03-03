function CreateJob() {

        $(".selectMultiOptions1").select2({
            placeholder: "Job Type"
        });
        $(".selectMultiOptions2").select2({
            placeholder: "Social Media"
        });
}

function focusElement(element) {
    element.focus();
}