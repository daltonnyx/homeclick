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