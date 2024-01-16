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
GPRO.namespace('WorkShop');
GPRO.WorkShop = function () {
    var Global = {
        UrlAction: {
            GetListWorkShop: '/WorkShop/Gets',
            SaveWorkShop: '/WorkShop/Save',
            DeleteWorkShop: '/WorkShop/Delete',
        },
        Element: {
            JtableWorkShop: 'jtableWorkShop',
            PopupWorkShop: 'popup_WorkShop',
            PopupSearch: 'popup_SearchWorkShop'
        },
        Data: {
            ModelWorkShop: {},
            isInsert: true
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitListWorkShop();
        ReloadListWorkShop();
        InitPopupWorkShop();
        InitPopupSearch();
        BindData(null);
    }
     
    var RegisterEvent = function () {
        $('#' + Global.Element.PopupWorkShop).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupWorkShop.toUpperCase());
        });
        $('#' + Global.Element.PopupSearch).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupSearch.toUpperCase());
        });
    }

    function BindData(obj) {
        if (obj) {
            $('#wkId').val(obj.Id);
            $('#wkcode').val(obj.Code);
            $('#wkName').val(obj.Name);
            $('#wkDescription').val(obj.Description);
        }
        else {
            $('#wkId').val(0);
            $('#wkcode,#wkName,#wkDescription').val('');
        }
    }


    function SaveWorkShop() {
        var obj = {
            Id: $('#wkId').val(),
            Code: $('#wkcode').val(),
            Name: $('#wkName').val(),
            Description: $('#wkDescription').val(),
        }
        $.ajax({
            url: Global.UrlAction.SaveWorkShop,
            type: 'post',
            data: ko.toJSON(obj),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        BindData(null);
                        ReloadListWorkShop();
                        // if (!Global.Data.isInsert)
                        $("#" + Global.Element.PopupWorkShop + ' button[wkcancel]').click();
                        Global.Data.isInsert = true;
                    }
                }, false, Global.Element.PopupWorkShop, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    function InitListWorkShop() {
        $('#' + Global.Element.JtableWorkShop).jtable({
            title: 'Danh sách Phân Xưởng',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.GetListWorkShop,
                createAction: Global.Element.PopupWorkShop,
                createObjDefault: BindData(null),
                //searchAction: Global.Element.PopupSearch,
            },
            messages: {
                addNewRecord: 'Thêm mới',
                // searchRecord: 'Tìm kiếm',
                // selectShow: 'Ẩn hiện cột'
            },
            searchInput: {
                id: 'wkkeyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadListWorkShop();
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
                    title: "Mã ",
                    width: "5%",
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Phân Xưởng",
                    width: "20%",
                },

                Description: {
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
                        var text = $('<i data-toggle="modal" data-target="#' + Global.Element.PopupWorkShop + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {
                            BindData(data.record);
                            Global.Data.isInsert = false;
                        });
                        div.append(text);

                        var btnDelete = $('<i title="Xóa" class="fa fa-trash-o"></i>');
                        btnDelete.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        div.append(btnDelete)
                        return div;
                    }
                }
            }
        });
    }

    function ReloadListWorkShop() {
        $('#' + Global.Element.JtableWorkShop).jtable('load', { 'keyword': $('#wkkeyword').val() });
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteWorkShop,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8', beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListWorkShop();
                    }
                }, false, Global.Element.PopupWorkShop, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InitPopupWorkShop() {
        $("#" + Global.Element.PopupWorkShop).modal({
            keyboard: false,
            show: false
        });
        $("#" + Global.Element.PopupWorkShop + ' button[wksave]').click(function () {
            if (CheckValidate()) {
                SaveWorkShop();
            }
        });
        $("#" + Global.Element.PopupWorkShop + ' button[wkcancel]').click(function () {
            $("#" + Global.Element.PopupWorkShop).modal("hide");
            BindData(null);
            $('div.divParent').attr('currentPoppup', '');
        });
    }

    function InitPopupSearch() {
        $("#" + Global.Element.PopupSearch).modal({
            keyboard: false,
            show: false
        });
        $("#" + Global.Element.PopupSearch + ' button[wksearch]').click(function () {
            ReloadListWorkShop();
            $("#" + Global.Element.PopupSearch + ' button[wkclose]').click();
        });

        $("#" + Global.Element.PopupSearch + ' button[wkclose]').click(function () {
            $("#" + Global.Element.PopupSearch).modal("hide");
            $('#wktxtSearch').val('');
            $('div.divParent').attr('currentPoppup', '');
        });
    }

    function CheckValidate() {
        if ($('#wkName').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên Phân Xưởng.", function () { }, "Lỗi Nhập liệu");
            $('#wkName').focus();
            return false;
        }
        return true;
    }
}
$(document).ready(function () {
    var WorkShop = new GPRO.WorkShop();
    WorkShop.Init();
})
