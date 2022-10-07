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
GPRO.namespace('Customer');
GPRO.Customer = function () {
    var Global = {
        UrlAction: {
            Gets: '/Customer/Gets',
            Save: '/Customer/Save',
            Delete: '/Customer/Delete',
        },
        Element: {
            Jtable: 'jtableCustomer',
            Popup: 'popup_Customer',
            Search: 'pSearch_Popup'
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
        $("#cust-isPrivate").kendoMobileSwitch({
            onLabel: "Tất cả",
            offLabel: "Nội bộ"
        });

        $('#' + Global.Element.Popup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Popup.toUpperCase());
        });

        $('#' + Global.Element.Search).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Search.toUpperCase());
        });

        $('[cust-cancel]').click(function () {
            setToDefault();
        });

        $('[cust-close]').click(function () {
            $('div.divParent').attr('currentPoppup', '');
        });

    }

    setToDefault = () => {
        var switchInstance = $("#cust-isPrivate").data("kendoMobileSwitch");
        switchInstance.check(true);
        $('#cust-id').val(0);
        $('#cust-name').val('');
        $('#cust-des').val('');
    }

    function Save() {
        var obj = {
            Id: $('#cust-id').val(),
            Name: $('#cust-name').val(),
            Description: $('#cust-des').val(),
            IsPrivate: $("#cust-isPrivate").data("kendoMobileSwitch").check()
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

                       // if (!Global.Data.IsInsert) {
                            $("#" + Global.Element.Popup + ' button[cust-cancel]').click();
                            $('div.divParent').attr('currentPoppup', '');
                       // }
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
            title: 'Quản lý khách hàng',
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
                //selectShow: 'Ẩn hiện cột'
            },
            searchInput: {
                id: 'cust-keyword',
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
                    title: "Tên khách hàng",
                    width: "20%",
                },
                Description: {
                    title: "Mô Tả ",
                    width: "20%",
                    sorting: false,
                },
                edit: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var div = $('<div class="table-action"></div>')

                        var btnEdit = $('<i data-toggle="modal" data-target="#' + Global.Element.Popup + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        btnEdit.click(function () {
                            var switchInstance = $("#cust-isPrivate").data("kendoMobileSwitch");
                            switchInstance.check(data.record.IsPrivate);
                            $('#cust-id').val(data.record.Id);
                            $('#cust-name').val(data.record.Name);
                            $('#cust-des').val(data.record.Description);
                            Global.Data.IsInsert = false;
                        });
                        div.append(btnEdit);

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
        var keySearch = $('#cust-keyword').val();
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
                        BindData(null);
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

        $("#" + Global.Element.Popup + ' button[cust-save]').click(function () {
            if (CheckValidate()) {
                Save();
            }
        });
        $("#" + Global.Element.Popup + ' button[cust-cancel]').click(function () {
            $("#" + Global.Element.Popup).modal("hide");
        });
    }

    function CheckValidate() {
        if ($('#cust-name').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên khách hàng.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

}

$(document).ready(function () {
    var Customer = new GPRO.Customer();
    Customer.Init();
});
