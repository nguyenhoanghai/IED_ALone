if (typeof GPRO == 'undefined' || !GPRO) {
    var GPRO = {};
}

GPRO.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ('' + a[i]).split('.');
        o = GPRO;
        for (j = (d[0] == 'GPRO') ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
}
GPRO.namespace('UsingTech');
GPRO.UsingTech = function () {
    var Global = {
        UrlAction: {
            Export: '/UsingTech/Export',
            GetDetail: '/UsingTech/GetReportDetail',
            ExportDetail: '/UsingTech/ExportDetail',
            Gets: '/UsingTech/Gets'
        },
        Element: {
        },
        Data: {
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        GetUsers("filter-user");
        GetWorkshop ("filter-workshop");
        $('[using-tech-refresh-detail],[using-tech-refresh]').click();
    }

    var RegisterEvent = function () {
        $("#report-from").kendoDatePicker({
            format: "dd/MM/yyyy",
            value: moment().format('DD/MM/YYYY'),
            change: function () {
                var value = this.value();
                if (value != null) {
                    var dp = $("#report-to").data("kendoDatePicker");
                    console.log(value)
                    dp.min(value);
                }
            },
        });

        $("#report-to").kendoDatePicker({
            format: "dd/MM/yyyy",
            value: moment().format('DD/MM/YYYY'),
            min: new Date()
        });


        $('[using-tech-export]').click(function () {
            window.location.href = Global.UrlAction.Export
        });

        $('#filter-user,#filter-view').change(function () {
            GetReportDetail();
        });

        $('[using-tech-refresh-detail]').click(function () {
            GetReportDetail();
        });

        $('[using-tech-refresh]').click(function () {
            GetReport();
        })

        $('[using-tech-export-detail]').click(function () {
            var _from = $("#report-from").data("kendoDatePicker");
            var _to = $("#report-to").data("kendoDatePicker");

            window.location.href = Global.UrlAction.ExportDetail + `?userId=${$('#filter-user').val()}&workshopId=${$('#filter-workshop').val()}&isView=${($('#filter-view').val() == '1' ? true : false)}&from=${ moment(_from.value()).format('DD/MM/YYYY') }&to=${ moment(_to.value()).format('DD/MM/YYYY') }`;
        });
    }

    function GetReportDetail() {
        var _from = $("#report-from").data("kendoDatePicker");
        var _to = $("#report-to").data("kendoDatePicker");

        let userId = ($('#filter-user').val() == undefined || $('#filter-user').val() == null ? 0 : $('#filter-user').val())
        let workshopId = ($('#filter-workshop').val() == undefined || $('#filter-workshop').val() == null ? 0 : $('#filter-workshop').val())
        $.ajax({
            url: Global.UrlAction.GetDetail,
            type: 'post',
            data: JSON.stringify({ 'userId': userId, 'workshopId': workshopId, 'isView': ($('#filter-view').val() == '1' ? true : false), 'from': _from.value(), 'to': _to.value() }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        var records = result.Records;
                        let tb = $('#tb-tech-detail tbody');
                        tb.empty();
                        if (records && records.length > 0) {
                            $.each(records, function (i, item) {
                                tb.append(`<tr>
                                                <td>${item.WorkshopName}</td>
                                                <td>${item.UserName}</td>
                                                <td>${item.Note}</td>
                                                <td>${moment(item.CreatedDate).format('DD/MM/YYYY HH:mm')}</td > 
                                            </tr>`)
                            })
                        }
                        else
                            tb.append(`<tr><td colspan="4">Không có dữ liệu.</td></tr>`)
                    }
                    else
                        GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupModule, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    function GetUsers(controlName) {
        $.ajax({
            url: '/user/GetSelectList',
            type: 'POST',
            data: '',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        if (data.Data.length > 0) {
                            var str = ` <option value="0">Tất cả</option>`;;
                            if (data.Data.length > 0) {
                                $.each(data.Data, function (index, item) {
                                    str += ` <option value="${item.Value}">${item.Code} (${item.Name}) </option>`;
                                });
                            }
                            $('#' + controlName).empty().append(str).change();
                            $('[' + controlName + ']').empty().append(str).change();
                            $('#' + controlName).trigger('liszt:updated');
                        }
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function GetReport() {
        $.ajax({
            url: Global.UrlAction.Gets,
            type: 'post',
            data: '',
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        var record = result.Records;
                        let tb = $('#tb-using-tech tbody');
                        tb.empty();
                        if (record) {
                            tb.append(`<tr>
                                            <td>Tổng</td>
                                            <td>${record.TotalProduct}</td>
                                            <td>0</td>
                                            <td>0</td>
                                            <td>${record.TotalPhase}</td>
                                            <td>${record.TotalViewPhase}</td>
                                            <td>${record.TotalDownloadPhase}</td>
                                            <td>0</td>
                                            <td>${record.TotalNewPhase}</td>
                                            <td>0</td>
                                        </tr >`)
                            $.each(record.Details, function (i, item) {
                                tb.append(`<tr>
                                                <td>${item.Name}</td>
                                                <td>${item.TotalProduct}</td>
                                                <td>0</td>
                                                <td>0</td>
                                                <td>${item.TotalPhase}</td>
                                                <td>${item.TotalViewPhase}</td>
                                                <td>${item.TotalDownloadPhase}</td>
                                                <td>0</td>
                                                <td>${item.TotalNewPhase}</td>
                                                <td>0</td>
                                            </tr >`)
                            })
                        }
                        else
                            tb.append(`<tr><td colspan="10">Không có dữ liệu.</td></tr>`)
                    }
                    else
                        GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupModule, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    function GetWorkshop(controlName) {
        $.ajax({
            url: '/Workshop/GetSelect',
            type: 'POST',
            data: '',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        if (data.Data.length > 0) {
                            var str = ` <option value="0">Tất cả</option>`;;
                            if (data.Data.length > 0) {
                                $.each(data.Data, function (index, item) {
                                    str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                                });
                            }
                            $('#' + controlName).empty().append(str);
                            $('[' + controlName + ']').empty().append(str);
                            $('#' + controlName).trigger('liszt:updated');
                            $('#' + controlName + ',[' + controlName + ']').change();
                        }
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

}
$(document).ready(function () {
    var obj = new GPRO.UsingTech();
    obj.Init();
});