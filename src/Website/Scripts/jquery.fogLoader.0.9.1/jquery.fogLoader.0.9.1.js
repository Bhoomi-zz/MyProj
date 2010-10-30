/* 
* fogLoader 0.9.1
* Author: Corbin Camp (ccamp@onebox.com)
* licensed under GPL licenses 
*/
(function($){
        
    $.fn.fogLoader = function(options){
        var defaults = {
            message: 'Loading',
            animated: false,
            closeOnEscape: true,
            height: 25,
            maxHeight: false,
            maxWidth: false,
            minHeight: 15,
            minWidth: 20,
            position: 'center',
            width: 130,
            textAlign: 'center',
            wrapText: 'nowrap',
            fontSize: '1.2em',
            fontFamily: null,
            fontWeight: 'normal',
            borderRadius: null,
            borderWidth: '1px',
            style: 'message',
            progressMax: 10,
            progressChar: '.',
            progressSpell: false,
            progressDelay: 250,
            progressBarImage: null,
            progressValue: 0
        };

        var _method = null;
        var _opts = null;
        var _spellCount = 1;
        var _spellIntervalID = '';

        var _methods = {
            close: function(){
                      $('.ui-progressbar').remove();
                      clearInterval(_spellIntervalID);
                      $(this).dialog('close');
                   },
            destroy: function(){
                       $('.ui-progressbar').remove();
                       clearInterval(_spellIntervalID);
                       $(this).dialog('destroy');
                   },
            updateProgress: function(v){
                        fillProgressBar(v);          
                   }
        };
      
        // process any _method calls
        if(typeof options === 'object' || ! options){ // initialize the dialog
            _opts = $.extend({}, defaults, options);
        }else{
            _method = options;
            if(_methods[_method]){
                return _methods[_method].apply(this, Array.prototype.slice.call(arguments,1));
            }else{
                $.error('_method ' + _method + ' does not exist on jQuery.fogLoader');
            }   
        };

        // private functions
        function fillProgressBar(v){
            if(v <= 100){
                $('.ui-progressbar-value').css('width',v+'%');
                if(v==100){
                    $('.ui-progressbar-value').addClass('ui-corner-all');
                    $('.ui-progressbar-value').css({width:'102%'});
                }        
            }
        }

        
        $(this).each(function(){
            var msg = _opts.message; // the orgininal message
            var elmt = '#ui-dialog-title-'+$(this).attr('id');
            var displayItem; // the rendered message or element

            // set the displayItem
            if(_opts.style=='message'){
                if(_opts.animated){
                    if(_opts.progressSpell){
                        displayItem = _opts.message.substring(0,1);
                    }else{
                        displayItem = _opts.message;
                    }
                }else{
                    displayItem = _opts.message + '&hellip;';
                }
            }
            
            $(this).dialog({
                modal: true,
                resizable: false,
                closeText: '',
                draggable: false,
                closeOnEscape: _opts.closeOnEscape,
                height: _opts.height,
                width: _opts.width,
                maxHeight: _opts.maxHeight,
                minHeight: _opts.minHeight,
                maxWidth: _opts.maxWidth,
                minWidth: _opts.minWidth,
                position: _opts.position,
                title: displayItem,
                beforeclose: function(event,ui){
                       $('#ui-dialog-title-loader-progressbar').remove();
                       clearInterval(_spellIntervalID);
                       $(this).dialog('destroy');
                }
            });
            $('.ui-dialog-titlebar').addClass('ui-state-default')
                .removeClass('ui-widget-header')
                .css({fontSize:_opts.fontSize, 
                      fontWeight:_opts.fontWeight, 
                      whiteSpace:_opts.wrapText,
                      'border-width':'0px'});
            if(_opts.fontFamily != null){
                $('.ui-dialog-titlebar').css({fontFamil:_opts.fontFamily});
            }
            $('.ui-dialog-titlebar-close').remove();
            $('.ui-dialog').css({padding:'0',borderWidth:_opts.borderWidth});
            // adjust border radii
            if(_opts.borderRadius != null){
                $('.ui-dialog-titlebar').css({'-moz-border-radius':_opts.borderRadius, '-webkit-border-radius':_opts.borderRadius});
                $('.ui-dialog').css({'-moz-border-radius':_opts.borderRadius,'-webkit-border-radius':_opts.borderRadius});
            };
            if(_opts.style == 'progressbar'){
                var pb = $('<div />');
                pb.attr('id',elmt+'-'+_opts.style);
                pb.css('height', (_opts.height - 5)+'px');
                pb.progressbar({value:_opts.progressValue});
                $('.ui-dialog-titlebar').hide();
                $(this).css({height:'auto',padding:1,overflow:'hidden'});
                $(this).append(pb);
                $('.ui-progressbar-value').css('margin','-2px');
                if(!$.support.htmlSerialize && _opts.progressBarImage == null){ // ie fix
                    $('.ui-progressbar-value').css('height',(_opts.height + 5)+'px');
                }
                if(_opts.progressBarImage != null){
                    $('.ui-progressbar-value').css({backgroundImage: 'url('+_opts.progressBarImage+')'});
                    $('.ui-progressbar-value').removeClass('ui-widget-header');
                    pb.removeClass('ui-widget-content');
                    $('.ui-dialog').removeClass('ui-widget-content');
                }else{
                    $('.ui-dialog .ui-widget-content').css('border-width','0px');
                }
                fillProgressBar(_opts.progressValue);    
            }else{
                $(this).hide();
            }
            if(_opts.animated && _opts.style != 'progressbar'){
                _spellIntervalID =  setInterval(function(){
                                        var currText = $(elmt).html();
                                        if(currText == null){
                                            clearInterval(_spellIntervalID);
                                        }else{
                                            if(currText.length < (msg.length + (_opts.progressMax - msg.length))){
                                                if(_opts.progressSpell && _opts.animated){
                                                    if(_spellCount < msg.length){
                                                        currText += msg.substring(_spellCount, _spellCount+1); 
                                                    }else{
                                                        currText += _opts.progressChar;
                                                    }
                                                    _spellCount++;
                                                }else{
                                                    currText += _opts.progressChar;
                                                }
                                            }else{
                                                _spellCount = 1;
                                                currText = _opts.progressSpell ? _opts.message.substring(0,1) : msg;
                                            }
                                            $(elmt).html(currText);
                                        }
                                    }, _opts.progressDelay);
            };
            $('.ui-widget-overlay').css('position','fixed');
        });
    }
})( jQuery );
