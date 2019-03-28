$(document).ready(function() {
    var tela = jQuery(window);
    var label = jQuery("#lblLoading");
    // Click
    $(".formLoading").submit(function() {
        if ($(this).valid()) {
            document.body.style.cursor = 'wait';
            $("#Loading").show();
            label.css({
                'margin-top': 200,
                'margin-left': tela.width() / 2 - label.width() / 2
            });
        };
    });
});

// override jquery validate plugin defaults
$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element) {
        $(element).closest('.form-group').removeClass('has-error');
    },
    errorElement: 'span',
    errorClass: 'help-block',
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});

$.validator.addMethod('filesize', function (value, element, param) {
    // param = size (en bytes) 
    // element = element to validate (<input>)
    // value = value of the element (file name)
    return this.optional(element) || (element.files[0].size <= param)
});

$.fn.dataTableExt.afnSortData['dom-text'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input', td).val();
    });
};

// File upload handler with progress bar
function uploadFileAnalise(fileInput, tipoPlanilha, urlReceberArquivo, progressBar) {
    var d = $.Deferred();

    if (fileInput.files.length === 0) {
        d.resolve([]);
    } else {
        var results = [];
        var sentFiles = 0;
        
        var ins = fileInput.files.length;
        var percentTotal = ins * 100;
        var currentPercent = [];

        for (var x = 0; x < ins; x++) {
            var data = new FormData();
            data.append("SelectedFile", fileInput.files[x]);
            data.append('TipoPlanilha', tipoPlanilha);

            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4) {
                    sentFiles++;
                    results.push(JSON.parse(request.response));
                    if (sentFiles == ins) {
                        d.resolve(results);
                    }
                }
            };

            request.upload.addEventListener('progress', function (e) {
                var percent = (Math.ceil(e.loaded / e.total) * 100) + '%';
                $(progressBar).find('span').text(percent);
                progressBar.style.width = percent;
            }, false);

            request.open('POST', urlReceberArquivo, false);
            request.send(data);
        }
    }

    return d.promise();
}

function uploadAllAnalisesFiles(estIniFileInput, estFimFileInput,
    entradasInput, saidasInput, urlEnviarArquivo, totalBar, currentFileBar) {
    var d = $.Deferred();

    var allFiles = [];

    uploadFileAnalise(estIniFileInput, 1, urlEnviarArquivo, currentFileBar.find('.progress-bar').get(0)).done(function (result) {
        $.merge(allFiles, result);
        totalBar.find('.progress-bar').get(0).style.width = '25%';
        totalBar.find('.progress-bar').find('span').text('25%');
        currentFileBar.find('.progress-bar').get(0).style.width = '0%';
        currentFileBar.find('.progress-bar').find('span').text('0%');

        uploadFileAnalise(estFimFileInput, 2, urlEnviarArquivo, currentFileBar.find('.progress-bar').get(0)).done(function (result) {
            $.merge(allFiles, result);
            totalBar.find('.progress-bar').get(0).style.width = '50%';
            totalBar.find('.progress-bar').find('span').text('50%');
            currentFileBar.find('.progress-bar').get(0).style.width = '0%';
            currentFileBar.find('.progress-bar').find('span').text('0%');

            uploadFileAnalise(entradasInput, 3, urlEnviarArquivo, currentFileBar.find('.progress-bar').get(0)).done(function (result) {
                $.merge(allFiles, result);
                totalBar.find('.progress-bar').get(0).style.width = '75%';
                totalBar.find('.progress-bar').find('span').text('75%');
                currentFileBar.find('.progress-bar').get(0).style.width = '0%';
                currentFileBar.find('.progress-bar').find('span').text('0%');

                uploadFileAnalise(saidasInput, 4, urlEnviarArquivo, currentFileBar.find('.progress-bar').get(0)).done(function (result) {
                    $.merge(allFiles, result);
                    totalBar.find('.progress-bar').get(0).style.width = '100%';
                    totalBar.find('.progress-bar').find('span').text('100%');
                    currentFileBar.find('.progress-bar').get(0).style.width = '0%';
                    currentFileBar.find('.progress-bar').find('span').text('0%');

                    d.resolve(allFiles);
                });
            });
        });
    });

    return d.promise();
}