function getParameterByName(name) {
    name = name.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
    var regexS = '[\\?&]' + name + '=([^&#]*)';
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return '';
    else
        return decodeURIComponent(results[1].replace(/\+/g, ' '));
}

var pages = ['round_display.html','overall_scores_0.html','overall_scores_1.html','last_round.html'];

function onLoad() {
    var timeout = getParameterByName('refresh');
    if (timeout != '') {
        setTimeout('refresh()', timeout * 1000);
    }
}

function refresh() {    
    var next = getParameterByName('next');
    if (next == '') {
        window.location.reload(true);
    }
    else {
        var params = [];
        
        var timeout = getParameterByName('refresh');
        if (timeout != '') {
            params.push('refresh=' + timeout);
        }

        var loc = pages[next];
        var all = getParameterByName('all');        
        
        next = parseInt(next) + 1;
        if (next == pages.length) {                        
            if( all == '' ) {
                next = 1;                
            }
            else {
                
                next = 0;
            }
        }

        params.push('next=' + next);
        if (all != '') params.push('all=true');
        
        window.location = loc + '?' + params.join('&');
    }
}