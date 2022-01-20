function GetCategoriesSelect(moduleId, controlId, value) {
    if (moduleId != '0') {
        $.ajax({
            url: '/Category/GetSelect',
            type: 'POST',
            data: JSON.stringify({ 'ModuleId': moduleId }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                if (data.Result == "OK") {
                    var str = '';
                    if (data.Data != null && data.Data.length > 0) {
                        $.each(data.Data, function (index, item) {
                            str += '<option value="' + item.Value + '" ' + (item.Value == value ? "selected" : "") + '>' + item.Name + '</option>';
                        });
                    }
                    $('[' + controlId + ']').empty().append(str);
                    $('#' + controlId   ).empty().append(str);
                    $('[' + controlId + ']').trigger('liszt:updated');
                }
                else
                    GlobalCommon.ShowMessageDialog('Lỗi', function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
            }
        });
    }
}

function GetTimeTypePrepareSelect(controlId) {
    $.ajax({
        url: '/TimePrepare/GetTimeTypePreparesSelectList',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            if (data.Result == "OK") {
                var str = ' <option value="0"> Không có dữ liệu </option>';
                if (data.Data.length > 0) {
                    str = ' <option value="0">- Chọn Loại Thời Gian -</option>';
                    $.each(data.Data, function (index, item) {
                        str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                    });
                }
                $('#' + controlId).empty().append(str);
                $('#' + controlId).trigger('liszt:updated');
            }
            else
                GlobalCommon.ShowMessageDialog('Lỗi', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
        }
    });
}

function GetWorkersLevelSelect(controlName) {
    $.ajax({
        url: '/WorkerLevel/GetSelectList',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = ' <option value="0"> Không có Bậc thợ </option>';
                        if (data.Data.length > 0) {
                            str = ' <option value="0">- - Chọn Bậc thợ - -</option>';
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
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

function GetProductSelect(controlName) {
    $.ajax({
        url: '/Product/GetSelectList',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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
function GetProductGroupSelect(controlName) {
    $.ajax({
        url: '/Productgroup/GetSelectList',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = ' <option value="0">-- Nhóm mã hàng --</option>';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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

function GetCustomerSelect(controlName) {
    $.ajax({
        url: '/Customer/GetSelectList',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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


function GetWorkshopSelect(controlName) {
    $.ajax({
        url: '/Workshop/GetSelect',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
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

function GetPhaseGroupSelect(controlName) {
    $.ajax({
        url: '/PhaseGroup/GetSelect',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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

function GetApplyPressureSelect(controlName) {
    $.ajax({
        url: '/MType/GetApplyPressures',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            if (data.Result == "OK") {
                var str = '<option val="0" value="0"> --Chọn số lớp cắt-- </option>';
                if (data.Data.length > 0) {
                    $.each(data.Data, function (index, item) {
                        str += ' <option value="' + item.Id + '" val="' + item.Value + '">' + item.Level + '</option>';
                    });
                }
                $('#' + controlName).empty().append(str);
                $('[' + controlName + ']').empty().append(str);
            }
            else
                GlobalCommon.ShowMessageDialog('Lỗi lấy thư viện lớp cắt', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
        }
    });
}

function GetETypeSelect(controlName) {
    $.ajax({
        url: '/EquipmentType/GetSelects',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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

function GetEGroupSelect(controlName) {
    $.ajax({
        url: '/EquipmentGroup/GetSelects',
        type: 'POST',
        data: '',
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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

function GetLineSelect(controlName, id) {
    $.ajax({
        url: '/Line/GetSelect',
        type: 'POST',
        data: JSON.stringify({ 'workshopId': id }),
        contentType: 'application/json charset=utf-8',
        success: function (data) {
            GlobalCommon.CallbackProcess(data, function () {
                if (data.Result == "OK") {
                    if (data.Data.length > 0) {
                        var str = '';
                        if (data.Data.length > 0) {
                            $.each(data.Data, function (index, item) {
                                str += ' <option labours = "' + item.Data + '" value="' + item.Value + '">' + item.Name + '</option>';
                            });
                        }
                        $('#' + controlName).empty().append(str);
                        $('[' + controlName + ']').empty().append(str);
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
