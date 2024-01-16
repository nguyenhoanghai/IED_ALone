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
GPRO.namespace('UserApprove');
GPRO.UserApprove = function () {
    var Global = {
        UrlAction: {
            Gets: '/UserApprove/Gets',
            Save: '/UserApprove/Save',
            Delete: '/UserApprove/Delete',
        },
        Element: {
            Jtable: 'jtable-user-approve',
            Popup: 'popup-user-approve', 
        },
        Data: { 
            ApproveRoles:''
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
        GetUserSelect('user-approve-userid')
    }

    var RegisterEvent = function () {
        $('#duyet-cd').change(function (evt) {
            if ($(this).is(':checked'))
                Global.Data.ApproveRoles += "Công đoạn,";
            else
                Global.Data.ApproveRoles = Global.Data.ApproveRoles.replace('Công đoạn,', '');
            
        });
    }

    setToDefault = () => { 
        $('#user-approve-id').val(0);
        $('#user-approve-userid').val(0);
        Global.Data.ApproveRoles = '';
    }

    function Save() {
        var obj = {
            Id: $('#user-approve-id').val(),
            UserId: $('#user-approve-userid').val(),
            ApproveRoles: Global.Data.ApproveRoles.trim(), 
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
                        $("#" + Global.Element.Popup + ' button[user-approve-cancel]').click();
                            $('div.divParent').attr('currentPoppup', ''); 
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
            title: 'Quản lý tài khoản',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.Gets,
                createAction: Global.Element.Popup, 
            },
            messages: {
                addNewRecord: 'Thêm mới', 
            },
            searchInput: {
                id: 'user-approve-keyword',
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
                UserId: {
                    visibility: 'fixed',
                    title: "Tài khoản",
                    width: "20%",
                    display: function (data) {
                        return data.record.UserName;
                    }
                },
                ApproveRoles: {
                    title: "Quyền duyệt",
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
                            $('#user-approve-id').val(data.record.Id);
                            $('#user-approve-userid').val(data.record.UserId);
                            Global.Data.ApproveRoles = data.record.ApproveRoles;
                            if (data.record.ApproveRoles.indexOf('Công đoạn,') >= 0) {
                                $('#duyet-cd').prop('checked', true)
                            }
                      
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
        $('#' + Global.Element.Jtable).jtable('load', { 'keyword': $('#user-approve-keyword').val()  });
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

        $('#' + Global.Element.Popup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Popup.toUpperCase());
        });

        $("#" + Global.Element.Popup + ' button[user-approve-save]').click(function () {
            if (CheckValidate()) {
                Save();
            }
        });
        $("#" + Global.Element.Popup + ' button[user-approve-cancel]').click(function () {
            $("#" + Global.Element.Popup).modal("hide");
            $('div.divParent').attr('currentPoppup', '');
            setToDefault();
        });
    }

    function CheckValidate() {
        if ($('#user-approve-userid').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng chọn tài khoản.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        if (Global.Data.ApproveRoles.trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng chọn quyền cho tài khoản.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

}

$(document).ready(function () {
    var obj = new GPRO.UserApprove();
    obj.Init();
});
