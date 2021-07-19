function GetQueryStringParams(sParam, sifundefined) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
    return '';
}
$.fn.repeater_changer = function (new_id) {
    var ob = $(this);
    ob.attr('id', $(this).attr('id') + new_id);

    return this.each(function () {
        //alert('b:' + $(this).attr('id'));
        $(this).find('input, textarea, select').each(function () {
            var ob = $(this);
            ob.attr('id', this.id + new_id);
            ob.attr('name', ob.attr('id'));
            //ob.attr('name', this.name + new_id);
            //ob.attr('id', this.id.replace(/_\d$/, '_' + new_id));
            //ob.attr('name', this.name.replace(/_\d$/, '_' + new_id));
        });

        $(this).find('.repeatupdateid').each(function () {
            var ob = $(this);
            ob.attr('id', this.id + new_id);
            //ob.attr('id', this.id.replace(/_\d$/, '_' + new_id));
        });

        $(this).find('.repeatupdatefor').each(function () {
            var ob = $(this);
            val = ob.attr('for');
            //val = val.replace(/_\d$/, '_' + new_id);
            val = val + new_id;
            ob.attr('for', val);
        });

        $(this).find('.repeatupdatehtml').each(function () {
            var ob = $(this);
            val = ob.html();
            val = val.replace(/\d$/, new_id);
            //val = val + new_id;
            ob.html(val);
        });

        $(this).find('.repeatupdatehref').each(function () {
            var ob = $(this);
            val = ob.attr('href');
            //val = val.replace(/_\d$/, '_' + new_id);
            val = val + new_id;
            ob.attr('href', val);
        });

        $(this).find('.repeatupdateclass').each(function () {
            var ob = $(this);
            val = ob.attr('class');
            //val = val.replace(/_\d$/, '_' + new_id);
            val = val + new_id;
            ob.attr('class', val);
        });

        $(this).find('.repeatupdatevalidgroup').each(function () {
            var ob = $(this);
            val = ob.attr('class');
            //val = val.replace(/_\d$/, '_' + new_id);
            val = val + new_id;
            ob.attr('class', val);
        });

    });

};
var CSS_COLOR_NAMES = ["AliceBlue", "AntiqueWhite", "Aqua", "Aquamarine", "Azure", "Beige", "Bisque", "Black", "BlanchedAlmond", "Blue", "BlueViolet", "Brown", "BurlyWood", "CadetBlue", "Chartreuse", "Chocolate", "Coral", "CornflowerBlue", "Cornsilk", "Crimson", "Cyan", "DarkBlue", "DarkCyan", "DarkGoldenRod", "DarkGray", "DarkGrey", "DarkGreen", "DarkKhaki", "DarkMagenta", "DarkOliveGreen", "Darkorange", "DarkOrchid", "DarkRed", "DarkSalmon", "DarkSeaGreen", "DarkSlateBlue", "DarkSlateGray", "DarkSlateGrey", "DarkTurquoise", "DarkViolet", "DeepPink", "DeepSkyBlue", "DimGray", "DimGrey", "DodgerBlue", "FireBrick", "FloralWhite", "ForestGreen", "Fuchsia", "Gainsboro", "GhostWhite", "Gold", "GoldenRod", "Gray", "Grey", "Green", "GreenYellow", "HoneyDew", "HotPink", "IndianRed", "Indigo", "Ivory", "Khaki", "Lavender", "LavenderBlush", "LawnGreen", "LemonChiffon", "LightBlue", "LightCoral", "LightCyan", "LightGoldenRodYellow", "LightGray", "LightGrey", "LightGreen", "LightPink", "LightSalmon", "LightSeaGreen", "LightSkyBlue", "LightSlateGray", "LightSlateGrey", "LightSteelBlue", "LightYellow", "Lime", "LimeGreen", "Linen", "Magenta", "Maroon", "MediumAquaMarine", "MediumBlue", "MediumOrchid", "MediumPurple", "MediumSeaGreen", "MediumSlateBlue", "MediumSpringGreen", "MediumTurquoise", "MediumVioletRed", "MidnightBlue", "MintCream", "MistyRose", "Moccasin", "NavajoWhite", "Navy", "OldLace", "Olive", "OliveDrab", "Orange", "OrangeRed", "Orchid", "PaleGoldenRod", "PaleGreen", "PaleTurquoise", "PaleVioletRed", "PapayaWhip", "PeachPuff", "Peru", "Pink", "Plum", "PowderBlue", "Purple", "Red", "RosyBrown", "RoyalBlue", "SaddleBrown", "Salmon", "SandyBrown", "SeaGreen", "SeaShell", "Sienna", "Silver", "SkyBlue", "SlateBlue", "SlateGray", "SlateGrey", "Snow", "SpringGreen", "SteelBlue", "Tan", "Teal", "Thistle", "Tomato", "Turquoise", "Violet", "Wheat", "White", "WhiteSmoke", "Yellow", "YellowGreen"];

