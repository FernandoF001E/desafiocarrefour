var base_url = $('#hdnUrlBase').val();

// Tenta criar o objeto request
try {
	request = new XMLHttpRequest();
} catch (ee) {
	try {
		request = new ActiveXObject("Msxml2.XMLHttp");
	} catch (e) {
		try {
			request = new ActiveXObject("Microsoft.XMLHttp");
		} catch (E) {
			request = false;
		}
	}
}

//Fila de conex�es
fila = [];
ifila = 0;

function CriaID() {
	// Gera o ID
	var data = new Date();
	return data.getFullYear() + "" + data.getMonth() + "" + data.getDate() + "" + data.getHours() + "" + data.getMinutes() + "" + data.getSeconds() + "" + data.getMilliseconds();
}

function executaScript(texto) {
	// inicializa o inicio ><
	var ini = 0;

	// loop enquanto achar um script
	while (ini != -1) {

		// procura uma tag de script
		ini = texto.indexOf('<script', ini);

		// se encontrar
		if (ini >= 0) {
			// define o inicio para depois do fechamento dessa tag
			ini = texto.indexOf('>', ini) + 1;

			// procura o final do script
			var fim = texto.indexOf('</script>', ini);

			// extrai apenas o script
			codigo = texto.substring(ini, fim);

			// executa o script
			eval(codigo);
		}
	}
}
// Carrega via request a url recebida e coloca seu valor no objeto com o id recebido
function ajaxHTML(id, url, parametros, metodo, imagem, mensagem, assincrono) {
	if (assincrono == null) {
		assincrono = true;
	}
	// Padrão a exibir a mensagem
	if (mensagem == "") {
		mensagem = "<lang:message key='js.aguarde_carregando_informacoes' />";
	}

	if (imagem == 1) {
		strImagem = "<img src=\"img/ico_processando_circular.gif\">";
	} else if (imagem == 2) {
		strImagem = "<img src=\"img/ico_processando_barra.gif\">";
		mensagem = " " + mensagem;
	} else {
		strImagem = "";
	}

	// Trata a URL
	if (metodo == "GET") {
		if (parametros == "") { // Ainda não tem parâmetros
			parametros = "id=" + CriaID();
		} else {
			parametros = parametros + "&id=" + CriaID();
		}
	}

	// Carregando...
	document.getElementById(id).innerHTML = "<span class=\"carregando\">" + strImagem + mensagem + "</span>";

	// Adiciona à fila
	fila[fila.length] = [id, url, parametros, metodo];

	// Se não há conexões pendentes, executa    
	if ((ifila + 1) == fila.length) {
		ajaxRun(assincrono);
	}
}

function carregaJSPagina(divInserir) {

	// Pegando a div que receberá o Javascript        
	var conteudoJS = document.getElementById("divFuncoesJS");

	if (conteudoJS != null) {
		// Declarando a criação de uma nova tag 
		var newElement = document.createElement("script");

		//Pegando os valores das Tags que estão na página carregada pelo AJAX
		var scripts = divInserir.getElementsByTagName("script");

		//Inserindo o conteúdo da tag que pegamos na linha acima
		if (scripts != null && scripts.length > 0) {
			//newElement.text += scripts[0].innerHTML;
			for (var i = 0; i < scripts.length; i++) {
				newElement.text += scripts[i].innerHTML;
			}
		}

		// Agora, inserimos a nova tag dentro da div na página inicial
		conteudoJS.appendChild(newElement);
	}
}

// Executa a próxima conexão da fila
function ajaxRun(assinc) {

	// Abre a conexão
	if (fila[ifila][3] == "GET") {
		request.open(fila[ifila][3], fila[ifila][1] + "?" + fila[ifila][2], assinc);
	} else {
		request.open(fila[ifila][3], fila[ifila][1], assinc);
	}

	// Cabeçalhos para evitar cache
	request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;");
	request.setRequestHeader("Cache-Control", "no-store, no-cache, must-revalidate");
	request.setRequestHeader("Cache-Control", "post-check=0, pre-check=0");
	request.setRequestHeader("Pragma", "no-cache");
	//request.setRequestHeader("Content-Type","text/plain; charset=ISO-8859-1");

	// Funçao para tratamento do retorno
	request.onreadystatechange = function () {
		if (request.readyState == 4 && request.status == 200) {
			// Mostra o HTML recebido
			//            retorno = unescape(request.responseText.replace(/\+/g," "));
			retorno = unescape(request.responseText);

			document.getElementById(fila[ifila][0]).innerHTML = retorno;
			// Executa o script da página, caso tenha

			executaScript(retorno);
			//Adicionando os scripts da página chamado por AJAX na página que executou a operação
			carregaJSPagina(document.getElementById(fila[ifila][0]));

			// Roda o próximo, independente se deu erro ou nao
			ifila++;

			if (ifila < fila.length) {
				setTimeout("ajaxRun(true)", 20);
			}
		} else if (request.readyState == 4 && request.status != 200) {
			document.getElementById(fila[ifila][0]).innerHTML = "<lang:message key='js.ocorreu_erro_durante_processamento' />";
			// Roda o próximo, independente se deu erro ou nao
			ifila++;
			if (ifila < fila.length) {
				setTimeout("ajaxRun(true)", 20);
			}
		}
	}

	// Executa
	if (fila[ifila][3] == "GET") {
		request.send(null);
	} else {
		request.send(fila[ifila][2]);
	}
}