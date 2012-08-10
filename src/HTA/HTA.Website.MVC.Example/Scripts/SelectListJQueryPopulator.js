// Fill a select list with options using an array of values as the data source
// @param {String, Object} selectElement Reference to the select list to be modified, either the selector string, or the jQuery object itself
// @param {Object} values An array of option values to use to fill the select list. May be an array of strings, or an array of hashes (associative arrays).
// @param {String} [valueKey] If values is an array of hashes, this is the hashkey to the value parameter for the option element
// @param {String} [textKey] If values is an array of hashes, this is the hashkey to the text parameter for the option element
// @param {String} [defaultValue] The default value to select in the select list
// @remark This function will remove any existing items in the select list
// @remark If the values attribute is an array, then the options will use the array values for both the text and value.
// @return {Boolean} false
function setSelectOptions(selectElement, values, valueKey, textKey, defaultValue) {
   
    if (typeof (selectElement) == "string") {
        selectElement = $(selectElement);
    }
    
    selectElement.empty();

    if (typeof (values) == 'object') {
        
        if (values.length) {

            var type = typeof (values[0]);
            var html = "";

            if (type == 'object') {
                // values is array of hashes
                var optionElement = null;

                $.each(values, function () {

                    html += '<option value="' + this[valueKey] + '">' + this[textKey] + '</option>';
                    
                });

            } else {
                // array of strings
                $.each(values, function () {
                    var value = this.toString();
                    html += '<option value="' + value + '">' + value + '</option>';
                });
            }

            selectElement.append(html);

        }

        // select the defaultValue is one was passed in
        if (typeof defaultValue != 'undefined') {
            selectElement.children('option[value="' + defaultValue + '"]').attr('selected', 'selected');
        }

    }

    return false;

}