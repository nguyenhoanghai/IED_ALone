﻿if (typeof GPRO == 'undefined' || !GPRO) {
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
GPRO.namespace('Line');
GPRO.Line = function () {
    var Global = {
        UrlAction: {
            GetListLine: '/Line/Gets',
            SaveLine: '/Line/Save ',
            DeleteLine: '/Line/Delete ',
        },
        Element: {
            JtableLine: 'jtableLine',
            PopupLine: 'popup_Line',
            PopupSearch: 'popup_SearchLine'
        },
        Data: {
            ModelLine: {},
            isCreate: true
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitList();
        ReloadList();
        InitPopup();
        InitPopupSearch();
        BindData(null);
        GetWorkshopSelect('line_workshop');
    }


    var RegisterEvent = function () {
        $('[re_line_workshop]').click(function () {
            GetWorkshopSelect('line_workshop');
        });
        $('#' + Global.Element.PopupLine).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupLine.toUpperCase());
        });
        $('#' + Global.Element.PopupSearch).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupSearch.toUpperCase());
        });
    }

    function InitViewModel(Line) {
        var LineViewModel = {
            Id: 0,
            Code: '',
            Name: '',
            Description: '',
            CountOfLabours: 0,
            WorkShopId: 0,
        };
        if (Line != null) {
            LineViewModel = {
                Id: ko.observable(Line.Id),
                Code: ko.observable(Line.Code),
                Name: ko.observable(Line.Name),
                Description: ko.observable(Line.Description),
                CountOfLabours: ko.observable(Line.CountOfLabours),
                WorkShopId: ko.observable(Line.WorkShopId),
            };
        }
        return LineViewModel;
    }

    function BindData(Line) {
        Global.Data.ModelLine = InitViewModel(Line);
        ko.applyBindings(Global.Data.ModelLine,document.getElementById(Global.Element.PopupLine));
    }

    function SaveLine() {
        Global.Data.ModelLine.WorkShopId = $('#line_workshop').val();
        $.ajax({
            url: Global.UrlAction.SaveLine,
            type: 'post',
            data: ko.toJSON(Global.Data.ModelLine),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                       // if (!Global.Data.isCreate) {
                            $("#" + Global.Element.PopupLine + ' button[cancel_line]').click();
                            Global.Data.isCreate = true;
                       // }
                        BindData(null);
                        ReloadList();
                    }
                }, false, Global.Element.PopupLine, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    function InitList() {
        $('#' + Global.Element.JtableLine).jtable({
            title: 'Danh sách Chuyền',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.GetListLine,
                createAction: Global.Element.PopupLine,
                createObjDefault: InitViewModel(null),
               // searchAction: Global.Element.PopupSearch,
            },
            messages: {
                addNewRecord: 'Thêm mới',
                //searchRecord: 'Tìm kiếm',
                //selectShow: 'Ẩn hiện cột'
            },
            searchInput: {
                id: 'line-keyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadList();
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Code: {
                    title: "Mã Chuyền",
                    width: "6%",
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Chuyền",
                    width: "20%",
                },
                CountOfLabours: {
                    title: "LĐ Định Biên",
                    width: "5%",
                    display: function (data) {
                        txt = '<span class="red bold">' + data.record.CountOfLabours + ' (LĐ)</span>';
                        return txt;
                    }
                },
                WorkShopName: {
                    title: "Phân Xưởng",
                    width: "10%",
                    display: function (data) {
                        txt = '<span class=" bold">' + data.record.WorkShopName + '</span>';
                        return txt;
                    }
                }, Description: {
                    title: "Mô Tả",
                    width: "20%",
                    sorting: false
                },
                edit: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var div = $('<div class="table-action"></div>')
                          var text = $('<i data-toggle="modal" data-target="#' + Global.Element.PopupLine + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {
                            $('#line_workshop').val(data.record.WorkShopId);
                            BindData(data.record);
                            Global.Data.isCreate = false;
                        });
                        div.append(text)

                        var btnDelete = $('<i title="Xóa" class="fa fa-trash-o"></i>');
                        btnDelete.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        div.append(btnDelete)
                        return  div;
                    }
                }  
            }
        });
    }

    function ReloadList() {
        $('#' + Global.Element.JtableLine).jtable('load', { 'keyword': $('#line-keyword').val() });
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteLine,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
     $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadList();                   
                    }
                }, false, Global.Element.PopupLine, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InitPopup() {
        $("#" + Global.Element.PopupLine).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.PopupLine + ' button[save_line]').click(function () {
            if (CheckValidate())  
                SaveLine(); 
        });

        $("#" + Global.Element.PopupLine + ' button[cancel_line]').click(function () {
            $("#" + Global.Element.PopupLine).modal("hide");
            BindData(null);
            $('div.divParent').attr('currentPoppup', '');
        });
    }

    function InitPopupSearch() {
        $("#" + Global.Element.PopupSearch).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.PopupSearch + ' button[search_line]').click(function () {
            ReloadList();
            $("#" + Global.Element.PopupSearch + ' button[close_search_line]').click();
        });

        $("#" + Global.Element.PopupSearch + ' button[close_search_line]').click(function () {
            $("#" + Global.Element.PopupSearch).modal("hide");
            $('#linetxtSearch').val('');
            $('#linesearchBy').val('1');
            $('div.divParent').attr('currentPoppup', '');
        });
    }
    
    function CheckValidate() {
        if ($('#lineName').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên.", function () { }, "Lỗi Nhập liệu");
            $('#lineName').focus();
            return false;
        }
        else if ($('#lineCountOfLabours').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng Nhập số Lao Động Định Biên.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if (parseInt($('#lineCountOfLabours').val()) <= 0) {
            GlobalCommon.ShowMessageDialog("Vui lòng Nhập số Lao Động Định Biên lớn hơn 0.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#line_workshop').val() == "0") {
            GlobalCommon.ShowMessageDialog("Vui lòng Chọn Phân Xưởng.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }
}
$(document).ready(function () {
    var Line = new GPRO.Line();
    Line.Init();
});