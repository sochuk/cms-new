$(document).ready(function () {    
    setTimeout(function () {
        $('table tr td').each(function () {
            var exp = $(this).text();
            if (exp === "EXPIRED") $(this).closest('table').closest('div').addClass("d-none").remove();
        });
        $(document).find('a[href*="www.devexpress.com"]').closest('table').closest('div').addClass("d-none").remove();
        $(document).find('a[href="www.devexpress.com/purchase"]').closest('table').closest('div').addClass("d-none").remove();

    }, 2000)
    
    
});
function setup() {    
    setTimeout(function () {
        $('table tr td').each(function () {
            var exp = $(this).text();
            if (exp === "EXPIRED") $(this).closest('table').closest('div').addClass("d-none").remove();
        });
        $(document).find('a[href*="www.devexpress.com"]').closest('table').closest('div').addClass("d-none").remove();
        $(document).find('a[href="www.devexpress.com/purchase"]').closest('table').closest('div').addClass("d-none").remove();
    }, 2000)
}