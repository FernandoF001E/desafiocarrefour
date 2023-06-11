var nLimits = "10";

$(function () {
    $("#hdnCode").val("");
    $("#btnPrevious").hide();
    $("#btnNext").hide();
});

$("#txtSearchDate").datepicker({
    autoclose: true
});

$("#txtDate").datepicker({
    autoclose: true
});

$("#btnCancel").click(function () {
    fJS_ClearForm("frmFinancialRecords")
    $("#hdnCode").val("");
});

$("#btnSave").click(function () {
    var dataForm = $("#frmFinancialRecords").serialize();
    var result = dataForm.split("&");
    var message = "";
    var data = "";

    $("label").css("color", "rgb(87,86,85)");

    for (var i in result) {
        var data = result[i].split("=");

        $("#" + data[0]).css({ "border-color": "rgb(204,204,204)" });

        switch (data[0]) {
            case "txtDescription":
                var str = fJS_Remove_Space_String(data[1]);
                if (str == "") {
                    message = JSResource.MsgInsertDataField + " " + "<strong>" + $("label[for=" + data[0] + "]").text().replace(':', '') + "</strong>";
                    fJS_ShowWarning(message, data);
                    return false;
                }
                break;
            case "txtDate":
                var str = fJS_Remove_Space_String(data[1]);
                if (str == "") {
                    message = JSResource.MsgInsertDataField + " " + "<strong>" + $("label[for=" + data[0] + "]").text().replace(':', '') + "</strong>";
                    fJS_ShowWarning(message, data);
                    return false;
                }
            case "txtValue":
                var str = fJS_Remove_Space_String(data[1]);
                if (str == "") {
                    message = JSResource.MsgInsertDataField + " " + "<strong>" + $("label[for=" + data[0] + "]").text().replace(':', '') + "</strong>";
                    fJS_ShowWarning(message, data);
                    return false;
                }
                break;
            case "cboType":
                var str = fJS_Remove_Space_String(data[1]);
                if (str == "0") {
                    message = JSResource.MsgInsertDataField + " " + "<strong>" + $("label[for=" + data[0] + "]").text().replace(':', '') + "</strong>";
                    fJS_ShowWarning(message, data);
                    return false;
                }
                break;
        }
    }

    $.ajax({
        url: base_url + '/financialrecords/add',
        type: 'Post',
        data: dataForm,
        dataType: 'json',
        success: function (response) {
            var obj = response;
            if (obj != null && parseInt(obj.code) == SUCCESS) {
                $(window).scrollTop(0);
                $("#hdnCode").val("");
                fJS_ClearForm("frmFinancialRecords")
                fJS_ShowSucess('Operação Realizada com Sucesso !');
            }
            else if (obj != null && parseInt(obj.code) == SESSION_EXPIRED) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
                setTimeout(function () { window.location = base_url_session_timeout; }, timeOutError);
            }
            else if (obj != null && parseInt(obj.code) == EXCEPTION) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
            }
            else if (obj != null && parseInt(obj.code) == RECORDALREADYEXISTS) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            }
            else if (obj != null && parseInt(obj.code) == RECORDSELECT) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            }
            else {
                $(window).scrollTop(0);
                fJS_ShowError(JSResource.ErrorRequest);
            }
            return false;
        },
        error: function (e) {
            $(window).scrollTop(0);
            fJS_ShowError(JSResource.ErrorRequest + " -- " + e.responseText);
            return false;
        }
    });
});

$("#btnSearch").click(function () {
    fJF_LoadGrid(false, false, nLimits);
});

$("#btnPrevious").click(function () {
    fJF_LoadGrid(true, false, nLimits);
});

$("#btnNext").click(function () {
    fJF_LoadGrid(true, true, nLimits);
})

function fJF_GetShowRecords(sel) {
    nLimits = sel.value;
    pageTable = 1;
    fJF_LoadGrid(false, false, nLimits);
    $(window).scrollTop(0);
}

function fJF_LoadGrid(pagination, next, pageLimits) {
    if (pagination) {
        if (next) {
            pageTable++;
        } else {
            pageTable--;
        }
    } else {
        $("#btnNext").show();
    }

    $.ajax({
        url: base_url + '/financialrecords/search',
        type: 'Post',
        data: {
            'pageLimits': parseInt(pageLimits),
            'pageTable': parseInt(pageTable),
            'searchDescription': $("#txtSearchDescription").val(),
            'searchDate': $("#txtSearchDate").val(),
            'searchRecordType': $("#cboSearchRecordType").val()
        },
        success: function (response) {
            var obj = response;
            if (obj != null && parseInt(obj.code) == SUCCESS) {
                var tableDefault = $("#tableDefault");
                if (pageTable >= obj.totalPages) {
                    $("#btnNext").hide();
                }
                if (pageTable < obj.totalPages) {
                    $("#btnNext").show();
                }
                if (pageTable == 1) {
                    $("#btnPrevious").hide();
                } else {
                    $("#btnPrevious").show();
                }

                $("#gridFinancialRecords").empty();

                for (var i in obj.data) {
                    var dataId = obj.data[i].financialRecordsId;
                    var textType = "";
                    var tdStatus = "";
                    var btnAction = "";

                    if (obj.data[i].recordStatus > 0) {
                        textType = "text-default";
                        tdStatus = "<span class='label label-success' title='" + JSResource.Active + "'>" + JSResource.Active + "</span>";
                        btnAction = "<a href='javascript:void(0);' onclick='javascript:fJS_Change(" + dataId + ",0);' rel='tooltip' title='" + JSResource.InactivateRegister + "'><span class='meta'><span class='icon text-success'><i class='fa fa-check'></i></span></span></a>";
                    }
                    else {
                        textType = "text-muted text-danger";
                        tdStatus = "<span class='label label-danger' title='" + JSResource.Inactive + "'>" + JSResource.Inactive + "</span>";
                        btnAction = "<a href='javascript:void(0);' onclick='javascript:fJS_Change(" + dataId + ",1);' rel='tooltip' title='" + JSResource.ActivateRegister + "'><span class='meta'><span class='icon text-danger'><i class='fa fa-close'></i></span></span></a>";
                    }

                    var btnLoad = "<a href='javascript:void(0);' onclick='javascript:fJS_Load(" + dataId + ");' rel='tooltip' title='Editar dados'><span class='meta'><span><i class='fa fa-pencil'></i></span></span></a>";;

                    var tr = $("<tr></tr>");

                    var type = "";

                    if (obj.data[i].recordType == 1) {
                        type = "Debit";
                    } else {
                        type = "Credit";
                    }
                
                    var dataValue = obj.data[i].dateRecords;
                    var d = dataValue.substring(8, 10)
                    var m = dataValue.substring(5, 7)
                    var y = dataValue.substring(0, 4)
                    var dateRecordes = (d + "/" + m + "/" + y);

                    var value = obj.data[i].financialValue.toLocaleString('pt-br', { minimumFractionDigits: 2 });

                    tr.html(('<td class="text-right ' + textType + '" width="50px">' + obj.data[i].financialRecordsId + '</td>')
                        + " " + ('<td class="text-left ' + textType + '" width="350px">' + obj.data[i].description + '</td>')
                        + " " + ('<td class="text-center ' + textType + '" width="150px">' + dateRecordes + '</td>')
                        + " " + ('<td class="text-right ' + textType + '" width="150px">' + value + '</td>')
                        + " " + ('<td class="text-left ' + textType + '" width="80px">' + type + '</td>')
                        + " " + ('<td class="text-center ' + textType + '" width="80px">' + tdStatus + '</td>')
                        + " " + ('<td class="text-center ' + textType + '" width="10px">' + btnAction + '</td>')
                        + " " + ('<td class="text-center ' + textType + '" width="10px">' + btnLoad + '</td>'));
                    tableDefault.append(tr);
                }
            } else if (obj != null && parseInt(obj.code) == NOTFOUND) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
            } else if (obj != null && parseInt(obj.code) == SESSION_EXPIRED) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
                setTimeout(function () { window.location = base_url_session_timeout; }, timeOutError);
            } else if (obj != null && parseInt(obj.code) == -EXCEPTION) {
                $(window).scrollTop(0);
                fJS_ShowError(obj.message);
            } else if (obj != null && parseInt(obj.code) == RECORDALREADYEXISTS) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            } else if (obj != null && parseInt(obj.code) == RECORDSELECT) {
                $(window).scrollTop(0);
                fJS_ShowWarningText(obj.message);
            }
            else {
                $(window).scrollTop(0);
                fJS_ShowError(obj.code + "-" + JSResource.ErrorRequest);
            }
            return false;
        },
        error: function (e) {
            $(window).scrollTop(0);
            fJS_ShowError(JSResource.ErrorRequest + " -- " + e.responseText);
            return false;
        }
    });
}

function fJS_Load(id) {
    if (id != "") {
        $.ajax({
            url: base_url + '/financialrecords/load',
            type: 'Post',
            data: { 'id': parseInt(id) },
            dataType: 'json',
            success: function (response) {
                var obj = response;
                if (obj != null && parseInt(obj.data.financialRecordsId) > 0) {
                    var value = obj.data.financialValue.toLocaleString('pt-br', { minimumFractionDigits: 2 });
                    $("#hdnCode").val(obj.data.financialRecordsId);
                    $("#txtDescription").val(obj.data.description);
                    var dataValue = obj.data.dateRecords;
                    var d = dataValue.substring(8, 10)
                    var m = dataValue.substring(5, 7)
                    var y = dataValue.substring(0, 4)
                    var dateRecordes = (d + "/" + m + "/" + y);
                    $("#txtDate").val(dateRecordes);
                    $("#txtValue").val(value);
                    $("#cboType").val(obj.data.recordType);
                    $("#txtObservation").val(obj.data.observation);
                    $('[href="#tabCadastro"]').tab('show');
                $(window).scrollTop(0);
                } else if (obj != null && parseInt(obj.code) == SESSION_EXPIRED) {
                    $(window).scrollTop(0);
                    fJS_ShowError(obj.message);
                    setTimeout(function () { window.location = base_url_session_timeout; }, timeOutError);
                } else if (obj != null && parseInt(obj.code) == -EXCEPTION) {
                    $(window).scrollTop(0);
                    fJS_ShowError(obj.message);
                } else if (obj != null && parseInt(obj.code) == RECORDALREADYEXISTS) {
                    $(window).scrollTop(0);
                    fJS_ShowWarningText(obj.message);
                } else if (obj != null && parseInt(obj.code) == RECORDSELECT) {
                    $(window).scrollTop(0);
                    fJS_ShowWarningText(obj.message);
                } else {
                    $(window).scrollTop(0);
                    fJS_ShowError(obj.code + "-" + JSResource.ErrorRequest);
                }
                return false;
            },
            error: function (e) {
                $(window).scrollTop(0);
                fJS_ShowError(JSResource.ErrorRequest + " -- " + e.responseText);
                return false;
            }
        });
    }
}

function fJS_Change(id, status) {
    if (id != "") {
        $.ajax({
            url: base_url + '/financialrecords/changestatus',
            type: 'Post',
            data: { 'id': parseInt(id), 'status': parseInt(status) },
            dataType: 'json',
            success: function (response) {
                var obj = response;
                if (obj != null && parseInt(obj.data.financialRecordsId) > 0) {
                    $(window).scrollTop(0);
                    fJS_ShowSucess('Operação Realizada com Sucesso !');
                    fJF_LoadGrid(false, false, nLimits);
                } else if (obj != null && parseInt(obj.code) == -9) {
                    $(window).scrollTop(0);
                    fJS_ShowError(obj.message);
                    setTimeout(function () { window.location = base_url_session_timeout; }, timeOutError);
                } else if (obj != null && parseInt(obj.code) == -99) {
                    $(window).scrollTop(0);
                    fJS_ShowError(obj.message);
                } else if (obj != null && parseInt(obj.code) == -12) {
                    $(window).scrollTop(0);
                    fJS_ShowWarningText(obj.message);
                } else if (obj != null && parseInt(obj.code) == -13) {
                    $(window).scrollTop(0);
                    fJS_ShowWarningText(obj.message);
                } else {
                    $(window).scrollTop(0);
                    fJS_ShowError(obj.code + "-" + JSResource.ErrorRequest);
                }
                return false;
            },
            error: function (e) {
                $(window).scrollTop(0);
                fJS_ShowError(JSResource.ErrorRequest + " -- " + e.responseText);
                return false;
            }
        });
    }
}
