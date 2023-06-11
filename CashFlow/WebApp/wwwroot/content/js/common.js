var base_url = $('#hdnUrlBase').val();
var base_url_session_timeout = $('#hdnUrlBase').val() + "/authentication/login";
var base_controller = "";
var base_form = "";
var base_form_search = "";
var quotationId = "0";
var orderId = "0";
var errorColor = "#bd362f";
var timeOutError = 5000;
var timeOutWarning = 3000;
var timeOutSucess = 2000;
var timeOutInfo = 7000;
var currFFZoom = 1;
var currIEZoom = 100;
var oTable = "";
var oTableBullarium = "";
var REGEX_EMAIL = '([a-z0-9!#$%&\'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&\'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)';
var pageTableAccount = 1;
var pageTableModal = 1;
var pageTableModalSupplier = 1;
var pageTableModalPurchaseOrder = 1;
var pageTable = 1;

const SUCCESS = 0;
const INVALID_PARAMETER = -1;
const UNDEFINED_ERROR = -2;
const ACTION_NOT_PERFORMED = -3;
const SEARCH_ERROR = -4;
const NOTFOUND = -5;
const PARAMETER_CAN_NOT_NULL = -6;
const UNVERIFIED_ACCOUNT = -7;
const INVALID_AUTHENTICATION = -8;
const SESSION_EXPIRED = -9;
const INVALID_TOKEN = -10;
const INVALID_MODEL = -11;
const RECORDALREADYEXISTS = -12;
const RECORDSELECT = -13;
const UNAUTHORIZED = -401;
const FORBIDDEN = -403;
const NOTACCEPTABLE = -406;
const INVALID_APPICATIONKEY = -400;
const EXCEPTION = -99;
const INVALID_DATA = -98;

_keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

function fJS_EncodeBase64(input) {
	var output = "";
	var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
	var i = 0;

	input = fJS_utf8_encode(input);

	while (i < input.length) {

		chr1 = input.charCodeAt(i++);
		chr2 = input.charCodeAt(i++);
		chr3 = input.charCodeAt(i++);

		enc1 = chr1 >> 2;
		enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
		enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
		enc4 = chr3 & 63;

		if (isNaN(chr2)) {
			enc3 = enc4 = 64;
		} else if (isNaN(chr3)) {
			enc4 = 64;
		}

		output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);
	}
	return output;
}

function fJS_DecoBase64(input) {
	var output = "";
	var chr1, chr2, chr3;
	var enc1, enc2, enc3, enc4;
	var i = 0;

	input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

	while (i < input.length) {

		enc1 = this._keyStr.indexOf(input.charAt(i++));
		enc2 = this._keyStr.indexOf(input.charAt(i++));
		enc3 = this._keyStr.indexOf(input.charAt(i++));
		enc4 = this._keyStr.indexOf(input.charAt(i++));

		chr1 = (enc1 << 2) | (enc2 >> 4);
		chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
		chr3 = ((enc3 & 3) << 6) | enc4;

		output = output + String.fromCharCode(chr1);

		if (enc3 != 64) {
			output = output + String.fromCharCode(chr2);
		}
		if (enc4 != 64) {
			output = output + String.fromCharCode(chr3);
		}
	}

	output = fJS_utf8_decode(output);

	return output;
}

function fJS_utf8_encode(string) {
	string = string.toString().replace(/\r\n/g, "\n");
	var utftext = "";

	for (var n = 0; n < string.length; n++) {

		var c = string.charCodeAt(n);

		if (c < 128) {
			utftext += String.fromCharCode(c);
		}
		else if ((c > 127) && (c < 2048)) {
			utftext += String.fromCharCode((c >> 6) | 192);
			utftext += String.fromCharCode((c & 63) | 128);
		}
		else {
			utftext += String.fromCharCode((c >> 12) | 224);
			utftext += String.fromCharCode(((c >> 6) & 63) | 128);
			utftext += String.fromCharCode((c & 63) | 128);
		}
	}
	return utftext;
}

function fJS_utf8_decode(string) {
	var string = "";
	var i = 0;
	var c = c1 = c2 = 0;

	while (i < utftext.length) {

		c = utftext.charCodeAt(i);

		if (c < 128) {
			string += String.fromCharCode(c);
			i++;
		}
		else if ((c > 191) && (c < 224)) {
			c2 = utftext.charCodeAt(i + 1);
			string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
			i += 2;
		}
		else {
			c2 = utftext.charCodeAt(i + 1);
			c3 = utftext.charCodeAt(i + 2);
			string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
			i += 3;
		}
	}
	return string;
}

function fJS_FormatCurrency(obj) {
	return obj.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' });
}

function fJS_FormatDate(obj) {
	var d = obj.substring(8, 10);
	var m = obj.substring(5, 7);
	var y = obj.substring(0, 4);
	var insertDate = (d + "/" + m + "/" + y);
	return insertDate;
}

function fJS_DataAtualFormatada() {
	var data = new Date(),
		dia = data.getDate().toString(),
		diaF = (dia.length == 1) ? '0' + dia : dia,
		mes = (data.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
		mesF = (mes.length == 1) ? '0' + mes : mes,
		anoF = data.getFullYear();
	return anoF + "-" + mesF + "-" + diaF;
}

function fJS_Remove_Space_String(obj) {
	obj = obj.split('+').join(' ');
	obj = obj.trim();
	return obj;
}

function fJS_Mask(objeto, mascara) {
	obj = objeto
	masc = mascara
	setTimeout("fMascEx()", 1)
}

function fMascEx() {
	obj.value = masc(obj.value)
}

function mTel(tel) {
	tel = tel.replace(/\D/g, "")
	tel = tel.replace(/^(\d)/, "($1")
	tel = tel.replace(/(.{3})(\d)/, "$1)$2")
	if (tel.length == 9) {
		tel = tel.replace(/(.{1})$/, "-$1")
	} else if (tel.length == 10) {
		tel = tel.replace(/(.{2})$/, "-$1")
	} else if (tel.length == 11) {
		tel = tel.replace(/(.{3})$/, "-$1")
	} else if (tel.length == 12) {
		tel = tel.replace(/(.{4})$/, "-$1")
	} else if (tel.length > 12) {
		tel = tel.replace(/(.{4})$/, "-$1")
	}
	return tel;
}

function mCNPJ(cnpj) {
	cnpj = cnpj.replace(/\D/g, "")
	cnpj = cnpj.replace(/^(\d{2})(\d)/, "$1.$2")
	cnpj = cnpj.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
	cnpj = cnpj.replace(/\.(\d{3})(\d)/, ".$1/$2")
	cnpj = cnpj.replace(/(\d{4})(\d)/, "$1-$2")
	return cnpj
}

function mCPF(cpf) {
	cpf = cpf.replace(/\D/g, "")
	cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2")
	cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2")
	cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2")
	return cpf
}

function mCEP(cep) {
	cep = cep.replace(/\D/g, "")
	cep = cep.replace(/^(\d{2})(\d)/, "$1.$2")
	cep = cep.replace(/\.(\d{3})(\d)/, ".$1-$2")
	return cep
}

function mNum(num) {
	num = num.replace(/\D/g, "")
	return num
}

function mData(data) {
	data = data.replace(/\D/g, "")
	data = data.replace(/^(\d{2})(\d)/, "$1/$2")
	data = data.replace(/^(\d{2})(\d)/, "/$1/$2")
	return data
}

function mValor(num) {
	var elemento = num;
	var valor = elemento;

	valor = valor + '';
	valor = valor.replace(/[\D]+/g, '');
	valor = valor + '';
	valor = valor.replace(/([0-9]{2})$/g, ",$1");

	if (valor.length > 6) {
		valor = valor.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");
	}

	elemento = valor;
	if (valor == 'NaN') elemento = '';

	return elemento;
}

function fJS_FormatCode(code, sizeCode) {
	var size = code.length;
	var zero = "";

	if (sizeCode == null || sizeCode == "" || sizeCode == "NaN") {
		sizeCode = 6;
	}

	for (i = size; i < sizeCode; i++) {
		zero += "0";
	}
	return zero.toString() + code.toString();
}

function fJS_ClearForm(name) {
	debugger
	var dataForm = $("#" + name + "").serialize();

	if (dataForm != null) {
		var result = dataForm.split("&");

		for (var i in result) {
			var data = result[i].split("=");
			$("#" + data[0]).val("");
		}
	}
	/*
	$("#" + name + "").each(function ()
	{
		this.reset();
	});
	*/
}

function fJS_ShowWarningTextRegister(message, data) {
	if (data != null && data != "") {
		$("#" + data[0]).focus();
		$("label[for=" + data[0] + "]").css("color", errorColor);
		$("#" + data[0]).css({ "border-color": errorColor });
	}

	$("#warning-container-message").html(message);
	$("#warning-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#warning-container-message").html('');;
	}, timeOutWarning);
}

function fJS_ShowWarningText(message) {
	$("#warning-container-message").html(message);
	$("#warning-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#warning-container").slideUp("slow");
	}, timeOutWarning);
	$(window).scrollTop(0);
}

function fJS_ShowWarning(message, data) {
	if (data != null && data != "") {
		$("#" + data[0]).focus();
		$("label[for=" + data[0] + "]").css("color", errorColor);
		$("#" + data[0]).css({ "border-color": errorColor });
	}

	$("#warning-container-message").html(message);
	$("#warning-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#warning-container").slideUp("slow");
	}, timeOutWarning);
	$(window).scrollTop(0);
}

function fJS_ShowWarningGrid(message) {
	$("#warning-container-message").html(message);
	$("#warning-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#warning-container").slideUp("slow");
	}, timeOutError);
	$(window).scrollTop(0);
}

function fJS_ShowError(message) {
	$("#error-container-message").html(message);
	$("#error-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#error-container").slideUp("slow");
	}, timeOutError);
	$(window).scrollTop(0);
}

function fJS_ShowSucess(message) {

	if (message != null && message != "") {
		$("#sucess-container-message").html(message);
	}

	$("#sucess-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#sucess-container").slideUp("slow");
	}, timeOutSucess);

	$(window).scrollTop(0);
}

function fJS_ShowInfo(message) {
	if (message != null && message != "") {
		$("#info-container-message").html(message);
	}

	$("#info-container")
		.hide()
		.removeClass("hide")
		.slideDown("slow");

	setTimeout(function () {
		$("#info-container").slideUp("slow");
	}, timeOutInfo);

	$(window).scrollTop(0);
}

function fJS_IsEmail(email) {
	debugger
	er = /^[a-zA-Z0-9][a-zA-Z0-9\._-]+@([a-zA-Z0-9\._-]+\.)[a-zA-Z-0-9]{2}/;
	if (!er.exec(email)) {
		return false;
	}
	else return true;
}

function fJS_IsValidCPF(cpf) {
	var exp = /\.|\-/g
	cpf = cpf.replace(exp, "");
	var digitoDigitado = eval(cpf.charAt(9) + cpf.charAt(10));
	var soma1 = 0, soma2 = 0;
	var vlr = 11;

	for (i = 0; i < 9; i++) {
		soma1 += eval(cpf.charAt(i) * (vlr - 1));
		soma2 += eval(cpf.charAt(i) * vlr);
		vlr--;
	}
	soma1 = (((soma1 * 10) % 11) == 10 ? 0 : ((soma1 * 10) % 11));
	soma2 = (((soma2 + (2 * soma1)) * 10) % 11);

	if (soma1 == 10 || soma1 == 11) soma1 = 0;
	if (soma2 == 10 || soma2 == 11) soma2 = 0;

	var digitoGerado = (soma1 * 10) + soma2;
	if (digitoGerado != digitoDigitado) {
		return false;
	}

	return true;
}

function fJS_IsValidCNPJ(cnpj) {
	var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
	var dig1 = new Number;
	var dig2 = new Number;

	var exp = /\.|\-|\//g
	cnpj = cnpj.toString().replace(exp, "");
	var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

	for (i = 0; i < valida.length; i++) {
		dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
		dig2 += cnpj.charAt(i) * valida[i];
	}
	dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
	dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

	if (((dig1 * 10) + dig2) != digito) {
		return false;
	}

	return true;
}