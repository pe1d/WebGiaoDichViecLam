jQuery(function($){$('body').on('init','.jmcl-tabs-wrapper, .company-listings-tabs',function(){$('.jmcl-tab, .company-listings-tabs .panel:not(.panel .panel)').hide();var $tabs=$(this).find('.jmcl-tabs, ul.tabs').first();$tabs.find('li:first a').click();}).on('click','.jmcl-tabs li a, ul.tabs li a',function(e){e.preventDefault();var $tab=$(this);var $tabs_wrapper=$tab.closest('.jmcl-tabs-wrapper, .company-listings-tabs');var $tabs=$tabs_wrapper.find('.jmcl-tabs, ul.tabs');$tabs.find('li').removeClass('active');$tabs_wrapper.find('.jmcl-tab, .panel:not(.panel .panel)').hide();$tab.closest('li').addClass('active');$tabs_wrapper.find($tab.attr('href')).show();}).on('click','a.company-listings-jobs-link',function(){$('.jobs_tab a').click();return true;});$('.jmcl-tabs-wrapper, .company-listings-tabs, #jobs').trigger('init');if($.isFunction($.fn.select2)){$('.job-manager-category-dropdown ').select2();}});