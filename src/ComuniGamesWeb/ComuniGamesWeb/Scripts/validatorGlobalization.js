$.validator.methods.number = function (value, element) {
    return this.optional(element) ||
        !isNaN(Globalize.parseFloat(value));
}

$.validator.methods.date = function (value, element) {
    return this.optional(element) ||
        !isNaN(Globalize.parseDate(value));
}


jQuery.extend(jQuery.validator.methods, {
    range: function (value, element, param) {
        //Use the Globalization plugin to parse the value        
        var val = Globalize.parseFloat(value);
        return this.optional(element) || (
            val >= param[0] && val <= param[1]);
    }
});