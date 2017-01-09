Number.prototype.round = function (p) {
    p = p || 10;
    return parseFloat(this.toFixed(p));
};

function randomString(length, chars) {
    var mask = '';
    if (chars.indexOf('a') > -1) mask += 'abcdefghijklmnopqrstuvwxyz';
    if (chars.indexOf('A') > -1) mask += 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    if (chars.indexOf('#') > -1) mask += '0123456789';
    if (chars.indexOf('!') > -1) mask += '~`!@#$%^&*()_+-={}[]:";\'<>?,./|\\';
    var result = '';
    for (var i = length; i > 0; --i) result += mask[Math.floor(Math.random() * mask.length)];
    return result;
}

function CreateMessage(type, content) {
    var result = '<div class="alert alert-' + type.toLowerCase() + '">' +
                    '<button class="close" data-dismiss="alert">×</button>' + content +
                 '</div>';
    return result;
}

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
    $('select[data-child]').on('change', function () {
        var $this = $(this);
        var $child = $($this.data('child'));
        if ($child) {
            if ($child.data('default-value'))
                $child.val($child.data('default-value'));
            else
                $child.val("");
            $child.children().not('[value=""]').hide();
            $child.children().filter('[data-parent="' + $this.val() + '"]').show();
            if ($child.hasClass('chosen'))
                $child.trigger('chosen:updated');
        }
    }).trigger('change');


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