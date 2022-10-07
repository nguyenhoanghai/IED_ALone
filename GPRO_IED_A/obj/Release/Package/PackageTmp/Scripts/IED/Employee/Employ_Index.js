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
GPRO.namespace('EmployeeList');
GPRO.EmployeeList = function () {
    var Global = {
        UrlAction: {
            GetList: '/Employee/Gets',
            SaveEmployee: '/Employee/Save',
            DeleteEmployee: '/Employee/Delete',

        },
        Element: {
            Jtable: 'jtableEmployee',
            PopupEmployee: 'popup_Employee',
            PopupSearch: 'eSearch_Popup',
        },
        Data: {
            ModelEmployee: {},
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

        GetWorkshopSelect('eWorkshopId');
    }

    this.reloadListEmployeeList = function () {
        ReloadListEmployeeList();
    }

    this.initViewModel = function (employeeList) {
        InitViewModel(employeeList);
    }

    var RegisterEvent = function () {
        $("#eGender").kendoMobileSwitch({
            onLabel: "Nam",
            offLabel: "Nữ"
        });
        $("#EBirthday").kendoDatePicker({
            format: "dd/MM/yyyy",
        });
        $('#' + Global.Element.PopupEmployee).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupEmployee.toUpperCase());
        });
        $('#' + Global.Element.PopupSearch).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupSearch.toUpperCase());
        });

        $('#eWorkshopId').change(() => {
            GetLineSelect('eLineId', $('#eWorkshopId').val());
        })
    }

    function InitViewModel(Employee) {
        var switchInstance = $("#eGender").data("kendoMobileSwitch");
        var EmployeeViewModel = {
            Id: 0,
            FirstName: '',
            LastName: '',
            Gender: true,
            Birthday: new Date(),
            Mobile: '',
            Code: '',
            Email: ''
        };
        switchInstance.check(false);
        if (Employee != null) {
            EmployeeViewModel = {
                Id: ko.observable(Employee.Id),
                FirstName: ko.observable(Employee.FirstName),
                LastName: ko.observable(Employee.LastName),
                Gender: ko.observable(Employee.Gender),
                Birthday: ko.observable(Employee.Birthday),
                Mobile: ko.observable(Employee.Mobile),
                Code: ko.observable(Employee.Code),
                Email: ko.observable(Employee.Email),
            };
            date = new Date(parseJsonDateToDate(Employee.Birthday));
            switchInstance.check(Employee.Gender);
        }
        return EmployeeViewModel;
    }

    function BindData(Employee) {
        Global.Data.ModelEmployee = InitViewModel(Employee);
        ko.applyBindings(Global.Data.ModelEmployee, document.getElementById(Global.Element.PopupEmployee));
    }

    function InitList() {
        $('#' + Global.Element.Jtable).jtable({
            title: 'Quản Lý Nhân Viên',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.GetList,
                // searchAction: Global.Element.PopupSearch,
                createAction: Global.Element.PopupEmployee
            },
            messages: {
                addNewRecord: 'Thêm Nhân Viên',
                // searchRecord: 'Tìm kiếm',
                //selectShow: 'Ẩn hiện cột'
            },
            searchInput: {
                id: 'ekeyword',
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
                    title: 'Mã Nhân Viên',
                    width: '10%'
                },
                FullName: {
                    visibility: 'fixed',
                    title: "Tên Nhân Viên",
                    width: "20%",
                },
                Image: {
                    title: 'Hình',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var text = $('<img style = "width:40px" src="' + data.record.Image + '"  />');
                        if (data.record.Image != null) {
                            return text;
                        }
                    }
                },
                Birthday: {
                    title: 'Ngày Sinh',
                    width: '10%',
                    display: function (data) {
                        date = new Date(parseJsonDateToDate(data.record.Birthday));
                        txt = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
                        return txt;
                    }
                },
                Gender: {
                    title: 'Giới Tính',
                    width: '5%',
                    display: function (data) {
                        var text = '';
                        if (data.record.Gender)
                            text = $('<i class="fa fa-male" style="font-size:26px"></i> ');
                        else
                            text = $('<i class="fa fa-female blue"  style="font-size:26px"></i> ');
                        return text;
                    }
                },
                Email: {
                    title: "Email",
                    width: "20%",
                },
                Mobile: {
                    title: "Điện thoại",
                    width: "20%",
                },
                edit: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var div = $('<div class="table-action"></div>')
                        var text = $('<i data-toggle="modal" data-target="#' + Global.Element.PopupEmployee + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {
                            //  BindData(data.record);
                            $('#eId').val(data.record.Id);
                            $('#ecode').val(data.record.Code);
                            $('#efirst').val(data.record.FirstName);
                            $('#elast').val(data.record.LastName);
                            $('#eEmail').val(data.record.Email);
                            $('#eMobile').val(data.record.Mobile);
                            $('#eWorkshopId').val(data.record.WorkshopId);
                            $('#eLineId').val(data.record.LineId);
                            var switchInstance = $("#eGender").data("kendoMobileSwitch");
                            switchInstance.check(data.record.Gender);
                            var date = new Date(parseJsonDateToDate(data.record.Birthday));
                            var datepicker = $("#EBirthday").data("kendoDatePicker");
                            datepicker.value(new Date(date.getFullYear(), date.getMonth(), date.getDate()));
                            datepicker.trigger("change");
                            Global.Data.IsInsert = false;
                        });
                        div.append(text)

                        var _text = $('<i title="Xóa" class="fa fa-trash-o"></i>');
                        _text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        div.append(_text);
                        return div;
                    }
                } 
            }
        });
    }

    function ReloadList() {
        $('#' + Global.Element.Jtable).jtable('load', { 'keyword': $('#ekeyword').val() });
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteEmployee,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadList();
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupEmployeeList, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InitPopup() {
        $("#" + Global.Element.PopupEmployee).modal({
            keyboard: false,
            show: false
        });
        $("#" + Global.Element.PopupEmployee + ' button[esave]').click(function () {
            if (CheckValidate())
                SaveEmployee();
        });
        $("#" + Global.Element.PopupEmployee + ' button[ecancel]').click(function () {
            $("#" + Global.Element.PopupEmployee).modal("hide");
            BindData(null);
            $('div.divParent').attr('currentPoppup', '');
        });
    }

    function CheckValidate() {
        if ($('#ecode').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Bạn chưa nhập mã nhân viên.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#efirst').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Bạn chưa nhập họ.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#elast').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Bạn chưa nhập tên.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#EBirthday').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Bạn chưa chọn ngày sinh.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#eWorkshopId').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Vui lòng chọn phân xưởng.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#eLineId').val().trim() == '') {
            GlobalCommon.ShowMessageDialog("Vui lòng chọn chuyền.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

    function SaveEmployee() {
        var obj = {
            Id: $('#eId').val(),
            Code: $('#ecode').val(),
            FirstName: $('#efirst').val(),
            LastName: $('#elast').val(),
            Email: $('#eEmail').val(),
            Mobile: $('#eMobile').val(),
            WorkshopId: $('#eWorkshopId').val(),
            LineId: $('#eLineId').val(),
            Gender: $("#eGender").data("kendoMobileSwitch").check(),
            Birthday: $("#EBirthday").data("kendoDatePicker").value(),
        }

        $.ajax({
            url: Global.UrlAction.SaveEmployee,
            type: 'post',
            data: ko.toJSON(obj),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        ReloadList();
                       // if (!Global.Data.IsInsert)
                            $("#" + Global.Element.PopupEmployee + ' button[ecancel]').click();
                        Global.Data.IsInsert = true;
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupModule, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }

}
$(document).ready(function () {
    var EmployeeList = new GPRO.EmployeeList();
    EmployeeList.Init();
});