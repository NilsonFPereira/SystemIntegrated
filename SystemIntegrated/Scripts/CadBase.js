function formatar_data_atual(data) {

    var dia = ('0' + data.getDate()).slice(-2);
    var mes = ('0' + (data.getMonth() + 1)).slice(-2);

    return dia  +"/" + mes + "/" + data.getFullYear() ;

}

function formatar_data(dados) {

    var dia = dados.substring(0, 2),
        mes = dados.substring(3, 5),
        ano = dados.substring(6, 10);

    return ano + '-' + mes + '-' + dia;

}

function formatar_data_brasil(dados){

    var ano = dados.substring(0, 4),
        mes = dados.substring(5, 7),
        dia = dados.substring(8, 10);

    return dia + '/' + mes + '/' + ano;

}

function formatar_decimal(dados) {

    var ret = "";

    if (dados != null && dados != "") {

        ret = dados.replace(".", "").replace(",", ".");

    }
    return ret;
}

function remove_ponto(dados) {

    var ret = "";

    if (dados != null && dados != "") {

        ret = dados.replace(".", "");

    }
    return ret;
}

function getMoney(str) {

    return parseInt(str.replace(/[\D]+/g, ''));
}

function formatReal(int) {
    var tmp = int + '';
    tmp = tmp.replace(/([0-9]{2})$/g, ",$1");
    if (tmp.length > 6)
        tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

    return tmp;
}

function parseJsonDate(jsonDateString) {

    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}