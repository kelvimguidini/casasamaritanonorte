﻿// Adicionamos o método ranger para sobrepor o método original e corrigir os valores passados,
// retirando o ponto milhar e trocando a virgula decimal por ponto decimal, 
// Com isto solucionamos os conflitos ao tentar definir um range para um valor quando utilizamos o atributo NumeroBrasil 
$.validator.addMethod('range',
    function (value, element, params) {

        // Coleta os parametros passados, valor maximo e minimo
        var min = params[0];
        var max = params[1];

        // Retira todo ponto de milhar formatado no campo valor
        var pos = value.indexOf('.');
        while (pos > -1) {
            value = value.replace('.', '');
            pos = value.indexOf('.');
        }

        // Troca virgula decimal por ponto decimal
        value = value.replace(',', '.');

        // Faz a validacao do valor, comparando com o valor minimo e maximo
        //if (value < min) {
        //    return false;
        //}

        //if (value > max) {
        //    return false;
        //}

        return true;
    });

// Adicionamos o metodo number para sobrepor o metodo padrão e atribuindo sempre um retorno verdadeiro para garantir 
// que o JQuery não vai tratar os numeros exibindo mensagens indesejadas. 
$.validator.addMethod('number',
    function (value, element) {
        return true;
    });

// Aqui adicionamos os metodos respeitando os nomes gerado em nossa classe atributo
// os nomes devem estar em caixa baixa tanto na integração como aqui no JavaScript (Nome atributo, parametro)
$.validator.unobtrusive.adapters.addSingleVal('numerobrasil', 'params');

//Aqui temos a funcao que Valida do lado do cliente os números digitados utilizando uma expressão regular
$.validator.addMethod('numerobrasil',
    function (value, element, params) {

        // Separamos o parametros recebidos
        var parametros = params.split(',');

        // Verifica o Ponto de milhar
        if (parametros[1].toString() == "True") {
            var pos = value.indexOf('.');
            while (pos > -1) {
                value = value.replace('.', '');
                pos = value.indexOf('.');
            }
        }

        // Valida caso o valor não tenha sido preenchido
        if (value.length == 0) {
            return true;
        }

        var expReg = '';

        // atribui a expressao regular com ou sem ponto decimal
        if (parametros[0].toString() == 'False') {
            expReg = /^[-+]?\d*$/;
        }
        else {
            expReg = /^[-+]?\d*\,?\d*$/;
        }

        // Valida a expressão, se for compativel vai retornar validando a campo, caso contrario exibe a mensagem de erro informada no atributo;
        return true;//value.match(expReg);
    });