function showChosenOption(chosen, opt) {
    opt.show().prop("disabled", false);;
    chosen.trigger("chosen:updated");
    return true;
}

function hideChosenOption(chosen, opt) {
    opt.hide().prop("disabled", true);;
    var opts = chosen.find('option:enabled:eq(0)');
    if (opts.length > 0)
        chosen.val(opts.first().val());
    chosen.trigger("chosen:updated");
    return true;
}

if (window.jQuery) {
    if (jQuery().validate) {
        if (jQuery().formToWizard) {
            $('.formToWizard').validate({ errorElement: 'em' });
            $('.formToWizard').formToWizard({
                submitButton: 'submit',
                nextBtnClass: 'btn btn-primary next pull-right',
                prevBtnClass: 'btn btn-default prev',
                buttonTag: 'button',
                validateBeforeNext: function (form, step) {
                    var stepIsValid = true;
                    var validator = form.validate();
                    $(':input', step).each(function (index) {
                        var xy = validator.element(this);
                        stepIsValid = stepIsValid && (typeof xy == 'undefined' || xy);
                    });
                    return stepIsValid;
                }
            });
        }
    }
}