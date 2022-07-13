(function ($) {
  'use strict';
  $(function () {
    var sidebar = $('.mdc-drawer-menu');
    var body = $('body');

    if($('.mdc-drawer').length) {
      var drawer = mdc.drawer.MDCDrawer.attachTo(document.querySelector('.mdc-drawer'));
      // toggler icon click function
      document.querySelector('.sidebar-toggler').addEventListener('click', function () {
          drawer.open = !drawer.open;
      });
    }

    // Initially collapsed drawer in below desktop
    if(window.matchMedia('(max-width: 991px)').matches) {
      if(document.querySelector('.mdc-drawer.mdc-drawer--dismissible').classList.contains('mdc-drawer--open')) {
        document.querySelector('.mdc-drawer.mdc-drawer--dismissible').classList.remove('mdc-drawer--open'); 
      }
    }

    //Add active class to nav-link based on url dynamically
    //Active class can be hard coded directly in html file also as required
    var current = location.pathname.split("/").slice(-1)[0].replace(/^\/|\/$/g, '').toLowerCase();
    var link;
    var container = null;
    $('.mdc-drawer-item .mdc-drawer-link', sidebar).each(function () {
       var $this = $(this);        
      if (current === "") {
        //for root url
          if ($this.attr('href').toLowerCase().indexOf("./") !== -1) {
          $(this).addClass('active');
          if ($(this).parents('.mdc-expansion-panel').length) {
              $(this).closest('.mdc-expansion-panel').addClass('expanded');
          }
        }
      } else {
        //for other url
          if ($this.attr('href').indexOf(current) !== -1) {              
              if (current === "cpanel") $(this).addClass('active');              
              if ($(this).parents('.mdc-expansion-panel').length == 1) {                  
                  link = window.location.href.toLowerCase();
                  if (window.location.hash) {
                      link = link.replace(window.location.hash, "");
                  } else {
                      link = link.replace("#", "");
                  }
                  if ($this.attr('href').toLowerCase() == link) {
                      $(this).closest('.mdc-expansion-panel').closest('.mdc-list-item').find('a:first').addClass('expanded');
                      $(this).closest('.mdc-expansion-panel').show();
                      $(this).addClass('active');
                  }                  
              }

              if ($(this).parents('.mdc-expansion-panel').length == 2) {                  
                  container = $(this).parents('.mdc-expansion-panel');
                  link = window.location.href.toLowerCase();
                  if (window.location.hash) {
                      link = link.replace(window.location.hash, "");
                  } else {
                      link = link.replace("#", "");
                  }
                  if ($this.attr('href').toLowerCase() == link) {                      
                      $(this).addClass('active');
                      $(this).closest('.mdc-expansion-panel').closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded');
                      $(this).closest('.mdc-expansion-panel').show();
                  }                  
              }

              if ($(this).parents('.mdc-expansion-panel').length == 3) {
                  container = $(this).parents('.mdc-expansion-panel');
                  link = window.location.href.toLowerCase();
                  if (window.location.hash) {
                      link = link.replace(window.location.hash, "");
                  } else {
                      link = link.replace("#", "");
                  }
                  if ($this.attr('href').toLowerCase() == link) {
                      $(this).addClass('active');
                      $(this).closest('.mdc-expansion-panel').closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded');
                      $(this).closest('.mdc-expansion-panel').show();
                  }
              }

              if ($(this).parents('.mdc-expansion-panel').length == 4) {
                  container = $(this).parents('.mdc-expansion-panel');
                  link = window.location.href.toLowerCase();
                  if (window.location.hash) {
                      link = link.replace(window.location.hash, "");
                  } else {
                      link = link.replace("#", "");
                  }
                  if ($this.attr('href').toLowerCase() == link) {
                      $(this).addClass('active');
                      $(this).closest('.mdc-expansion-panel').closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded');
                      $(this).closest('.mdc-expansion-panel').show();
                  }
              }

              if ($(this).parents('.mdc-expansion-panel').length == 5) {
                  container = $(this).parents('.mdc-expansion-panel');
                  link = window.location.href.toLowerCase();
                  if (window.location.hash) {
                      link = link.replace(window.location.hash, "");
                  } else {
                      link = link.replace("#", "");
                  }
                  if ($this.attr('href').toLowerCase() == link) {
                      $(this).addClass('active');
                      $(this).closest('.mdc-expansion-panel').closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded')
                          .closest('.mdc-expansion-panel').show().closest('.mdc-list-item').find('a:first').addClass('expanded');
                      $(this).closest('.mdc-expansion-panel').show();
                  }
              }

              //console.log($(this).parents('.mdc-expansion-panel').length)
          }
      }
        
      });


      
    
    // Toggle Sidebar items    
    $('[data-toggle="expansionPanel"]').on('click', function () {
      // close other items
      //$('.mdc-expansion-panel').not($('#' + $(this).attr("data-target"))).hide(300);
      //$('.mdc-expansion-panel').not($('#' + $(this).attr("data-target"))).prev('[data-toggle="expansionPanel"]').removeClass("expanded");
      // Open toggle menu
      $('#' + $(this).attr("data-target")).slideToggle(300, function() {
        $('#' + $(this).attr("data-target")).toggleClass('expanded');
      });      
    });

	$('li.mdc-list-item').on('click', function(){
	  var a = $(this).find('div > h6 > a');
	  //console.log($(a).attr('href'))
	  if($(a).attr('href')) window.location.href = $(a).attr('href');
	});
	
	$('button[type=button]').on('click', function(){
	  var a = $(this).find('a');
	  //console.log($(a).attr('href'))
	  if($(a).attr('href')) window.location.href = $(a).attr('href');
	});
	
    // Add expanded class to mdc-drawer-link after expanded
    $('.mdc-drawer-item .mdc-expansion-panel').each(function () {
      $(this).prev('[data-toggle="expansionPanel"]').on('click', function () {
        $(this).toggleClass('expanded');
      })
    });

    //Applying perfect scrollbar to sidebar
    if (!body.hasClass("rtl")) {
      if ($('.mdc-drawer .mdc-drawer__content').length) {
        new PerfectScrollbar('.mdc-drawer .mdc-drawer__content');
      }
    }

  });
})(jQuery);

function maxlength(obj, length) {
    $(obj).attr('maxlength', length);
    $(obj).maxlength();
    $(obj).removeAttr("onfocus").bind('focus', function () { });
    return false;
}

function htmlEncode(html) {
    return document.createElement('a').appendChild(
        document.createTextNode(html)).parentNode.innerHTML;
};

function htmlDecode(html) {
    var a = document.createElement('a'); a.innerHTML = html;
    return a.textContent;
};

function openLink(obj) {
    if ($(obj).find("a").attr("href").length > 0) window.location.href = $(obj).find("a").attr("href");
}

function openUrl(url) {
    window.location.href = host + url;
}

function lockScreen() {
    window.location.href = host + '/account/lock';
}

$(".mdc-checkbox__native-control input").unwrap().addClass("mdc-checkbox__native-control");
$(".mdc-radio__native-control input").unwrap().addClass("mdc-radio__native-control");

$(".bmd-help").closest("div").addClass("mb-4");
$('.content-wrapper input[maxlength].form-control, .content-wrapper textarea[maxlength].form-control').maxlength();

//const sresult = new mdc.list.MDCList(document.querySelector('ul.search-result .mdc-list-item'));
//sresult.listen('MDCSelect:action', () => { alert("AA") });

var ONE_MINUTE = 60 * 1000;
var TWO_MINUTE = 120 * 1000;
var THREE_MINUTE = 180 * 1000;
var FOUR_MINUTE = 240 * 1000;
var FIVE_MINUTE = 300 * 1000;
function repeatEvery(func, interval) {
    // Check current time and calculate the delay until next interval
    var now = new Date(), delay = interval - now % interval;

    function start() {
        // Execute function now...
        func();
        // ... and every interval
        setInterval(func, interval);
    }

    // Delay execution until it's an even interval
    setTimeout(start, delay);
}