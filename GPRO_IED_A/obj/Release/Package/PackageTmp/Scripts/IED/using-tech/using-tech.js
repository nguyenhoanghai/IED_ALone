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
            ExportDetail: '/UsingTech/ExportDetail'
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
        GetUsers("filter-user")
    }

    var RegisterEvent = function () {
        $('[using-tech-export]').click(function () {
            window.location.href = Global.UrlAction.Export
        });

        $('#filter-user').change(function () {
            GetReportDetail();
        })

        $('[using-tech-export-detail]').click(function () {
            window.location.href = Global.UrlAction.ExportDetail + "?userId=" + $('#filter-user').val();
        });
    }

    function GetReportDetail() {
        $.ajax({
            url: Global.UrlAction.GetDetail,
            type: 'post',
            data: JSON.stringify({ 'userId': $('#filter-user').val() }),
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
                                tb.append(`<tr><td>${item.UserName}</td><td>${item.PhaseName}</td><td>${moment(item.CreatedDate).format('DD/MM/YYYY HH:mm')}</td > </tr >`)
                            })
                        }
                        else
                            tb.append(`<tr><td colspan="3">Không có dữ liệu.</td></tr>`)
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
                            var str =  ` <option value="0">Tất cả</option>`;;
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
}
$(document).ready(function () {
    var obj = new GPRO.UsingTech();
    obj.Init();
});