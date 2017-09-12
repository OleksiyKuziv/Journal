//$(document).ready(function () {
//    var form = $("body").find('form:not(#logoutForm)');
//    form = form.filter(function (obj) {
//        return obj.id !== 'logoutForm';
//    });
//    debugger;
//    if (form.length > 0) {
//        $("body").find('form:not(#logoutForm)').find(':submit').submit(function () {
//            var image = $("#loading-image");
//            image.show();
//            debugger;
//        });
//    }
//});
$(document).ready(function () {
    var ss = $('form:not(#logoutForm)');
    ss.bind('submit', function (sender) {
        var a = $(sender.target);
        //$('#divMsg').show();
        var image = $('.image');
        var loading = new Image();
        loading.src = "/Images/giphy.gif";
        loading.height = 50;
        loading.width = 50;
        $('.submit').hide();
        image.empty();
        image.append(loading);        
        if (!a.valid || a.valid()) {
            $(this).find(":submit").prop('disabled', true);
            return true;
        }
        else
        {
            $(this).removeAttr('disabled');            
            image.hide();
            $('.submit').show();
            return false;
        }
    });
});

