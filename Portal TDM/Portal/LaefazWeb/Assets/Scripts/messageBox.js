(function ($) {

    $.MessageBoxOK = function (options) {
        var defaults = {
            "titulo": "Mensagem",
            "height": "auto",
            "width": "auto",
            "funcaoBotaoOK": null,
            "argFuncao": null

        };

        var settings = $.extend({}, defaults, options);

        $("#MessageBoxOK").remove();
        $('body').append('<div id="MessageBoxOK"></div>');
        

        var dlg = $("#MessageBoxOK")
            .html("<br/>" + settings.mensagem)
            .dialog({
                width: (settings.width),
                height: (settings.height),
                modal: true,
                resizable: false,
                title: settings.titulo,
                buttons: {
                    Ok: function () {
                        if (settings.funcaoBotaoOK != null && typeof (settings.funcaoBotaoOK) === "function") {
                            if (settings.argFuncao != null) {
                                settings.funcaoBotaoOK(settings.argFuncao);
                            } else {
                                settings.funcaoBotaoOK();
                            }
                        }
                        $(this).dialog("close");

                    }
                }
            });
        dlg.dialog("open");
    };


    $.MessageBoxSimNao = function (options) {
        var defaults = {
            "titulo": "Mensagem",
            "height": "auto",
            "width": "auto",
            "funcaoBotaoSim": null,
            "argFuncaoBotaoSim": null,
            "funcaoBotaoNao": null,
            "argFuncaoBotaoNao": null

        };

        var settings = $.extend({}, defaults, options);

        $("#MessageBoxSimNao").remove();
        $('body').append('<div id="MessageBoxSimNao"></div>');

        var dlg = $("#MessageBoxSimNao")
            .html("<br/>" + settings.mensagem)
            .dialog({
                width: (settings.width),
                height: (settings.height),
                modal: true,
                resizable: false,
                title: settings.titulo,
                buttons: {
                    Sim: function () {
                        if (settings.funcaoBotaoSim != null && typeof (settings.funcaoBotaoSim) === "function") {
                            if (settings.argFuncaoBotaoSim != null) {
                                settings.funcaoBotaoSim(settings.argFuncaoBotaoSim);
                            } else {
                                settings.funcaoBotaoSim();
                            }
                        }
                        $(this).dialog("close");
                    },
                    Não: function () {
                        if (settings.funcaoBotaoNao != null && typeof (settings.funcaoBotaoNao) === "function") {
                            if (settings.argFuncaoBotaoNao != null) {
                                settings.funcaoBotaoNao(settings.argFuncaoBotaoNao);
                            } else {
                                settings.funcaoBotaoNao();
                            }
                        }
                        $(this).dialog("close");
                    }
                }
            });
        dlg.dialog("open");
    };

    $.MessageBoxErro = function (options) {
        var defaults = {
            "height": "auto",
            "width": "auto"
        };

        var settings = $.extend({}, defaults, options);

        $("#MessageBoxErro").remove();
        $('body').append('<div id="MessageBoxErro"></div>');

        var html = "";
        html = html + '<div>'
        html = html + '<img alt="erro" src="../../Content/Images/error.png" height="35px"  style="float:left;" />'
        html = html + '</div>'
        html = html + '<br/>'
        html = html + '<div>'
        html = html + settings.mensagem
        html = html + '</div>'
        var dlg = $("#MessageBox")
            .html(html)
            .dialog({
                width: (settings.width),
                height: (settings.height),
                modal: true,
                resizable: false,
                title: "Ocorreu o Seguinte Erro.",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        dlg.dialog("open");
    }
})(jQuery);
