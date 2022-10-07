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
GPRO.namespace('ProductGroup');
GPRO.ProductGroup = function () {
    var Global = {
        UrlAction: {
            Gets: '/ProductGroup/Gets',
            Save: '/ProductGroup/Save',
            Delete: '/ProductGroup/Delete',
        },
        Element: {
            Jtable: 'jtableProGroup',
            Popup: 'popup_ProGroup',
        },
        Data: {
            IsInsert: true
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
    }

    var RegisterEvent = function () {
        $("#pro-group-isPrivate").kendoMobileSwitch({
            onLabel: "Tất cả",
            offLabel: "Nội bộ"
        });

        $('#' + Global.Element.Popup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Popup.toUpperCase());
        });

        $('#' + Global.Element.Search).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Search.toUpperCase());
        });

        $('[pro-group-cancel]').click(function () {
            setToDefault();
        });

    }

    setToDefault = () => {
        var switchInstance = $("#pro-group-isPrivate").data("kendoMobileSwitch");
        switchInstance.check(true);
        $('#pro-group-id').val(0);
        $('#pro-group-name').val('');
        $('#pro-group-des').val('');
    }

    function Save() {
        var obj = {
            Id: $('#pro-group-id').val(),
            Name: $('#pro-group-name').val(),
            Description: $('#pro-group-des').val(),
            IsPrivate: $("#pro-group-isPrivate").data("kendoMobileSwitch").check()
        };
        $.ajax({
            url: Global.UrlAction.Save,
            type: 'post',
            data: ko.toJSON(obj),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        ReloadList();
                        setToDefault();

                        //if (!Global.Data.IsInsert) {
                            $("#" + Global.Element.Popup + ' button[pro-group-cancel]').click();
                            $('div.divParent').attr('currentPoppup', '');
                      //  }
                        Global.Data.IsInsert = true;
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupModule, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    function InitList() {
        $('#' + Global.Element.Jtable).jtable({
            title: 'Quản lý chủng loại hàng',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.Gets,
                createAction: Global.Element.Popup,
                // searchAction: Global.Element.Search,
            },
            messages: {
                addNewRecord: 'Thêm mới',
                // searchRecord: 'Tìm kiếm',
                // selectShow: 'Ẩn hiện cột'
            },
            searchInput: {
                id: 'pro-group-keyword',
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
                Name: {
                    visibility: 'fixed',
                    title: "Tên chủng loại hàng",
                    width: "20%",
                },
                Description: {
                    title: "Mô Tả ",
                    width: "20%",
                    sorting: false,
                },
                edit: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var div = $('<div class="table-action"></div>')
                        var text = $('<i data-toggle="modal" data-target="#' + Global.Element.Popup + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {
                            var switchInstance = $("#pro-group-isPrivate").data("kendoMobileSwitch");
                            switchInstance.check(data.record.IsPrivate);
                            $('#pro-group-id').val(data.record.Id);
                            $('#pro-group-name').val(data.record.Name);
                            $('#pro-group-des').val(data.record.Description);
                            Global.Data.IsInsert = false;
                        });
                        div.append(text);

                        var btnDelete = $('<i title="Xóa" class="fa fa-trash-o"></i>');
                        btnDelete.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        div.append(btnDelete);
                        return div;
                    }
                } 
            }
        });
    }

    function ReloadList() {
        var keySearch = $('#pro-group-keyword').val();
        $('#' + Global.Element.Jtable).jtable('load', { 'keyword': keySearch });
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.Delete,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadList();
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.Popup, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InitPopup() {
        $("#" + Global.Element.Popup).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.Popup + ' button[pro-group-save]').click(function () {
            if (CheckValidate()) {
                Save();
            }
        });
        $("#" + Global.Element.Popup + ' button[pro-group-cancel]').click(function () {
            $("#" + Global.Element.Popup).modal("hide");
        });
    }

    function CheckValidate() {
        if ($('#pro-group-name').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên chủng loại hàng.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

}

$(document).ready(function () {
    var ProductGroup = new GPRO.ProductGroup();
    ProductGroup.Init();
});
