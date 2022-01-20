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
            Save : '/ProductGroup/Save',
            Delete : '/ProductGroup/Delete', 
        },
        Element: {
            Jtable: 'jtableProGroup',
            Popup: 'popup_ProGroup', 
        },
        Data: { 
            IsInsert : true
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
        $("#proIsPrivate").kendoMobileSwitch({
            onLabel: "Tất cả",
            offLabel: "Nội bộ"
        });

        $('#' + Global.Element.Popup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Popup.toUpperCase());
        });

        $('#' + Global.Element.Search).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Search.toUpperCase());
        });

        $('[pcancel]').click(function () {
            setToDefault();
        });
                 
    }

    setToDefault = () => {
        var switchInstance = $("#proIsPrivate").data("kendoMobileSwitch");
        switchInstance.check(true);
        $('#pid').val(0);
        $('#pname').val('');
        $('#pdes').val(''); 
    }
     
    function Save() { 
        var obj = {
            Id: $('#pid').val(),
            Name: $('#pname').val(), 
            Description: $('#pdes').val(), 
            IsPrivate: $("#proIsPrivate").data("kendoMobileSwitch").check()
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

                        if (!Global.Data.IsInsert) {
                            $("#" + Global.Element.Popup + ' button[pcancel]').click();
                            $('div.divParent').attr('currentPoppup', '');
                        }
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

    function InitList () {
        $('#' + Global.Element.Jtable).jtable({
            title: 'Quản lý nhóm mã hàng',
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
                    title: "Tên nhóm mã hàng",
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
                        var text = $('<i data-toggle="modal" data-target="#' + Global.Element.Popup + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {  
                            var switchInstance = $("#proIsPrivate").data("kendoMobileSwitch");
                            switchInstance.check(data.record.IsPrivate);
                            $('#pid').val(data.record.Id);
                            $('#pname').val(data.record.Name);
                            $('#pdes').val(data.record.Description); 
                            Global.Data.IsInsert = false;
                        });
                        return text;
                    }
                },
                Delete: {
                    title: '',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                }
            }
        });
    }

    function ReloadList () {
        var keySearch = $('#pro-group-keyword').val(); 
        $('#' + Global.Element.Jtable).jtable('load', { 'keyword': keySearch  });
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

        $("#" + Global.Element.Popup + ' button[psave]').click(function () {
            if (CheckValidate()) {
                Save();
            }
        });
        $("#" + Global.Element.Popup + ' button[pcancel]').click(function () {
            $("#" + Global.Element.Popup).modal("hide");
        });
    }
     
    function CheckValidate() {
        if ($('#pname').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên nhóm mã hàng.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }
     
}

$(document).ready(function () {
    var ProductGroup = new GPRO.ProductGroup();
    ProductGroup.Init(); 
});
 