var populatelink = '<a id="populate" href="javascript:void(0)">Populate</a>'
$("body").prepend(populatelink);

$("#populate").click(function () {
    $('input, textarea, select').each(function () {
        if (this.id.substring(0, 2) == '__' || this.id.substr(this.id.length - 1) == '_' || ($(this).attr('readonly') == 'readonly') || $(this).is(":hidden") || this.id == "g-recaptcha-response") {
            //alert(this.id + ', ' + this.id.substr(this.id.length - 1) + ', ' + $(this).attr('readonly'));
        } else {
            thetype = this.type;
            //console.log(thetype);
            if ($(this).val() == '') {
                populate = $(this).data('populate');
                if (typeof populate !== "undefined") {
                    if (populate.substring(0, 2) == '@D') {
                        this.value = '15 Mar 2017'
                    } else {
                        this.value = populate;
                    }
                } else {
                    switch (thetype) {
                        case 'email':
                            this.value = 'greg@datainn.co.nz';
                            break;
                        case 'select-one':
                            $(this).val($('#' + this.id + ' option:last').val());
                            break;
                        case 'checkbox':
                            $(this).prop('checked', true);
                            break;
                        case 'file':
                            break;
                        case 'hidden':
                            break;
                        case 'submit':
                            break;
                        default:
                            if (this.id.indexOf('phone') > -1 || $(this).hasClass('phone')) {
                                this.value = '027 123456';
                            } else if ($(this).hasClass('numeric')) {
                                this.value = 999;
                            } else {
                                //alert(this.id + ', ' + thetype);
                                this.value = this.id;
                            }
                    }
                    maxlen = $(this).attr('maxLength');
                    if (typeof maxlen !== "undefined") {
                        this.value = this.value.substring(0, maxlen);
                    }
                }
            }
        }
    })
})