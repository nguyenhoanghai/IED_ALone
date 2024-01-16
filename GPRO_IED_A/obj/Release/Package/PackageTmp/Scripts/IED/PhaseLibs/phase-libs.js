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
GPRO.namespace('PhaseLibs');
GPRO.PhaseLibs = function () {
    var Global = {
        UrlAction: {
            GetWhichNotLibs: '/PhaseLibs/GetWhichNotLibs',
            GetWhichIsLibs: '/PhaseLibs/GetWhichIsLibs',
            Save: '/PhaseLibs/Update',
            GetPhaseById: '/ProAna/GetPhaseSusgestById',
        },
        Element: {
            tableNotLibs: 'tb-phase',
            tableIsLibs: 'tb-phase-lib',
            CreatePhasePopup: 'Create-Phase-Popup',
            JtableManipulationArr: 'jtable_ManipulationArr',
        },
        Data: {
            IntGetTMUType: 0,
            TMU: 0,
            PhaseManiVerDetailArray: [],
            TimePrepareArray: [],
            Video: '',
            PhaseModel: {},
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitListLibs();
        ReloadListIsLibs();

        InitListNotLibs();
        ReloadListNotLibs();

        InitListMani_Arr();

        if ($('#config').attr('tmu') != "");
        Global.Data.TMU = parseFloat($('#config').attr('tmu'));
        if ($('#config').attr('gettmutype') != "")
            Global.Data.IntGetTMUType = parseInt($('#config').attr('gettmutype'));

        $('[re_workerslevel],[re-apply-pressure]').click();
    }

    var RegisterEvent = function () {
        $('[re_workerslevel]').click(function () {
            GetWorkersLevelSelect('workersLevel');
        });

        $('[re-apply-pressure]').click(function () {
            GetApplyPressureSelect('ApplyPressure');
        });


        $('.bt-save-1').click(() => {
            var ids = "";
            var $selectedRows = $('#' + Global.Element.tableNotLibs).jtable('selectedRows');
            if ($selectedRows.length > 0) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    ids += record.Id + ",";
                });
            }
            else {
                GlobalCommon.ShowMessageDialog("Không có công đoạn nào được chọn. Vui lòng chọn ít nhất 1 công đoạn để thực hiện hành động này.!", () => {
                    // no function
                }, "Thông báo chưa chọn công đoạn");
            }
            if (ids != '')
                Save(ids, false);
        });

        $('.bt-save-2').click(() => {
            var ids = "";
            var $selectedRows = $('#' + Global.Element.tableIsLibs).jtable('selectedRows');
            if ($selectedRows.length > 0) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    ids += record.Id + ",";
                });
            }
            else {
                GlobalCommon.ShowMessageDialog("Không có công đoạn nào được chọn. Vui lòng chọn ít nhất 1 công đoạn để thực hiện hành động này.!", () => {
                    // no function
                }, "Thông báo chưa chọn công đoạn");
            }
            if (ids != '')
                Save(ids, true);
        });

        $('[pgsearch]').click(() => {
            ReloadListIsLibs();
            $('[pgclose]').click();
        });

        $('[pgsearch-not]').click(() => {
            ReloadListNotLibs();
            $('[pgclose-not]').click();
        });

        $('[pgclose]').click(() => { $('#p-l-keyword').val('') });
        $('[pgclose-not]').click(() => { $('#p-n-l-keyword').val('') });
    }

    Save = (ids, isremove) => {
        $.ajax({
            url: Global.UrlAction.Save,
            type: 'post',
            data: JSON.stringify({ 'ids': ids, 'isRemove': isremove }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        ReloadListIsLibs();
                        ReloadListNotLibs();
                    }
                    else
                        GlobalCommon.ShowMessageDialog("Lưu thông tin bị lỗi. Vui lòng thao tác lại.!", function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupLine, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    InitListNotLibs = () => {
        $('#' + Global.Element.tableNotLibs).jtable({
            title: 'Danh sách công đoạn không phải mẫu',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            sorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: Global.UrlAction.GetWhichNotLibs,
            },
            messages: {
            },
            searchInput: {
                id: 'phase-not-lib-keyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadListNotLibs();
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Product: {
                    visibility: 'fixed',
                    title: "Mã hàng",
                    width: "20%",
                },
                GroupPhase: {
                    visibility: 'fixed',
                    title: "Cụm công đoạn",
                    width: "20%",
                },
                Code: {
                    title: "Mã công đoạn",
                    width: "6%",
                },
                Name: {
                    title: "Tên công đoạn",
                    width: "20%",
                },
                EquipName: {
                    title: "Thiết bị",
                    width: "20%",
                },

                TotalTMU: {
                    title: "Tổng TMU",
                    sorting: false,
                    width: "10%",
                    display: function (data) {
                        txt = '<span class="red bold">' + data.record.TotalTMU + '</span>';
                        return txt;
                    }
                },
                actions: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        let div = $('<div class="table-action"></div>');
                        var edit = $('<i data-toggle="modal" data-target="#' + Global.Element.CreatePhasePopup + '" title="Xem thông tin" class="fa fa-info-circle "  ></i>');
                        edit.click(function () {
                            GetPhasesById(data.record.Id);
                        });
                        div.append(edit);
                        return div;
                    }
                }
            }
        });
    }
    ReloadListNotLibs = () => {
        $('#' + Global.Element.tableNotLibs).jtable('load', { 'keyword': $('#phase-not-lib-keyword').val() });
    }

    InitListLibs = () => {
        $('#' + Global.Element.tableIsLibs).jtable({
            title: 'Danh sách công đoạn mẫu',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            sorting: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: Global.UrlAction.GetWhichIsLibs,
            },
            messages: {
            },
            searchInput: {
                id: 'phase-lib-keyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadListIsLibs();
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Product: {
                    visibility: 'fixed',
                    title: "Mã hàng",
                    width: "20%",
                },
                GroupPhase: {
                    visibility: 'fixed',
                    title: "Cụm công đoạn",
                    width: "20%",
                },
                Code: {
                    title: "Mã công đoạn",
                    width: "6%",
                },
                Name: {
                    title: "Tên công đoạn",
                    width: "20%",
                },
                EquipName: {
                    title: "Thiết bị",
                    width: "20%",
                },
                TotalTMU: {
                    sorting: false,
                    title: "Tổng TMU",
                    width: "10%",
                    display: function (data) {
                        txt = '<span class="red bold">' + data.record.TotalTMU + '</span>';
                        return txt;
                    }
                },
                actions: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        let div = $('<div class="table-action"></div>');
                        var edit = $('<i data-toggle="modal" data-target="#' + Global.Element.CreatePhasePopup + '" title="Xem thông tin" class="fa fa-info-circle "  ></i>');
                        edit.click(function () {
                            GetPhasesById(data.record.Id);                        
                        });
                        div.append(edit);
                        return div;
                    }
                }
            }
        });
    }
    ReloadListIsLibs = () => {
        $('#' + Global.Element.tableIsLibs).jtable('load', { 'keyword': $('#phase-lib-keyword').val() });
    }

    function InitListMani_Arr() {
        $('#' + Global.Element.JtableManipulationArr).jtable({
            title: 'Danh Sách Thao Tác',
            pageSize: 100,
            pageSizeChange: true,
            selectShow: false,
            sorting: false,
            actions: {
                listAction: Global.Data.PhaseManiVerDetailArray,
            },
            messages: {

            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                OrderIndex: {
                    title: "STT",
                    width: "5%", 
                },
                ManipulationCode: {
                    title: "Mã",
                    width: "5%", 
                },
                ManipulationName: {
                    title: "Mô Tả",
                    width: "35%", 
                },
                Loop: {
                    title: "Tần Suất",
                    width: "5%", 
                },
                TMUEquipment: {
                    title: "TMU Thiết Bị (chuẩn)",
                    width: "5%",
                    display: function (data) {
                        var txt = '<span class="blue bold">' + data.record.TMUEquipment + '</span>';
                        return txt;
                    }
                },
                TMUManipulation: {
                    title: "TMU Thao Tác (chuẩn)",
                    width: "5%",
                    display: function (data) {
                        var txt = '<span class="blue bold">' + data.record.TMUManipulation + '</span>';
                        return txt;
                    }
                },
                TotalTMU: {
                    title: "Tổng TMU",
                    width: "5%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.TotalTMU + '</span>';
                        return txt;
                    }
                }, 
            }
        });
    }

    function ReloadListMani_Arr() {
        var _totalmay = 0;
        if (Global.Data.PhaseManiVerDetailArray.length > 0) {
            $.each(Global.Data.PhaseManiVerDetailArray, function (i, item) {
                if (item.ManipulationCode) {
                    var _code = item.ManipulationCode.trim();
                    if (_code.indexOf('SE') >= 0) {
                        var cd = parseInt(_code.substring(2, _code.length - 1));
                        if (!isNaN(cd)) {
                            _totalmay += (cd * item.Loop);
                        }
                    }
                }
            });
        }
        $('#TotalMay').html(_totalmay);

        $('#' + Global.Element.JtableManipulationArr).jtable('load');
    }

    function GetPhasesById(_id) {
        $.ajax({
            url: Global.UrlAction.GetPhaseById,
            type: 'POST',
            data: JSON.stringify({ 'phaseId': _id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    $('#phase-suggest').val('');
                    if (data.Result == "OK") {
                        data.Records.TotalTMU = Math.round(data.Records.TotalTMU * 1000) / 1000;
                        $('#workersLevel').val(data.Records.WorkerLevelId);
                        $('#TotalTMU').html(data.Records.TotalTMU);
                        $('#phase-Des').val(data.Records.Description); 
                        $('#phase-name').val(data.Records.Name).change();
                        $('#phase-index').val(data.Records.Index); 
                        $('#phase-code').html(data.Records.Code);

                        Global.Data.TimePrepareArray.length = 0;
                        if (data.Records.timePrepares.length > 0) {
                            $.each(data.Records.timePrepares, function (i, item) {
                                Global.Data.TimePrepareArray.push(item);
                            }); 
                            $('#time-repare-id').val(data.Records.timePrepares[0].TimePrepareId);
                            $('#time-repare-id').attr('tmu', data.Records.timePrepares[0].TMUNumber);
                            $('#time-repare-name').val(`${data.Records.timePrepares[0].Name} - TMU: ${data.Records.timePrepares[0].TMUNumber}`);
                        }
                        $('#equipmentId').val(data.Records.EquipmentId);
                        $('#equipmentName').val(data.Records.EquipName);
                        $('#equiptypedefaultId').val(data.Records.EquipTypeDefaultId);
                        $('#E_info').val(data.Records.EquipDes);
                        $('#ApplyPressure').val(data.Records.ApplyPressuresId);
                        $('#chooseApplyPressure').hide();
                        if (data.Records.ApplyPressuresId != 0)
                            $('#chooseApplyPressure').show();
                        Global.Data.PhaseManiVerDetailArray.length = 0;
                        if (data.Records.actions.length > 0) {
                            $.each(data.Records.actions, function (i, item) {
                                item.OrderIndex = i + 1;
                            });
                            $.each(data.Records.actions, function (i, item) {
                                var obj = {
                                    Id: item.Id,
                                    CA_PhaseId: item.CA_PhaseId,
                                    OrderIndex: item.OrderIndex,
                                    ManipulationId: item.ManipulationId,
                                    ManipulationCode: item.ManipulationCode.trim(),
                                    EquipmentId: item.EquipmentId,
                                    TMUEquipment: item.TMUEquipment,
                                    TMUManipulation: item.TMUManipulation,
                                    Loop: item.Loop,
                                    TotalTMU: item.TotalTMU,
                                    ManipulationName: item.ManipulationName == null ? '' : item.ManipulationName.trim()
                                }
                                Global.Data.PhaseManiVerDetailArray.push(obj);
                            });
                        } 
                        ReloadListMani_Arr();
                        $('[percentequipment]').val(data.Records.PercentWasteEquipment);
                        $('[percentmanipulation]').val(data.Records.PercentWasteManipulation);
                        $('[percentdb]').val(data.Records.PercentWasteSpecial);
                        $('[percentnpl]').val(data.Records.PercentWasteMaterial);
                        UpdateIntWaste(); 
                        Global.Data.Video = data.Records.Video;
                        var video = document.getElementsByTagName('video')[0];
                        var sources = video.getElementsByTagName('source');
                        if (data.Records.Video != null) {
                            sources[0].src = data.Records.Video.split('|')[0];
                            sources[1].src = data.Records.Video.split('|')[0];
                            video.load();
                        }
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupProductType, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    //#region hao phí
    function LoadChooseManipulationPopup(code, callback) {
        code = code.toUpperCase();
        if (code.length >= 4) {
            if (code.substring(0, 1) == "C" || code.substring(0, 2) == "SE") {
                GetManipulationEquipmentInfoByCode(code, function (dataReturn) {
                    if (callback)
                        callback(dataReturn);
                });
            }
            else {
                ReloadListManipulation_Choose(code, 2, function () {
                    AddManipulationInfoToListChoose();
                });
            }
        }
        else {
            ReloadListManipulation_Choose(code, 2, function () {
                AddManipulationInfoToListChoose();
            });
        }
    }
    function GetTotalPercentWaste() {
        var totalEquipment = 0;
        if ($('input[percentEquipment]').val() != "")
            totalEquipment = parseFloat($('input[percentEquipment]').val());
        var totalManipulation = 0;
        if ($('input[percentManipulation]').val() != "")
            totalManipulation = parseFloat($('input[percentManipulation]').val());
        var totalDB = 0;
        if ($('input[percentDB]').val() != "")
            totalDB = parseFloat($('input[percentDB]').val());
        var totalNPL = 0;
        if ($('input[percentNPL]').val() != "")
            totalNPL = parseFloat($('input[percentNPL]').val());
        $('input[totalPercentWaste]').val(totalEquipment + totalManipulation + totalDB + totalNPL);
    }
    function GetTotalTimeWaste() {
        var totalEquipment = 0;
        if ($('input[totalTimeWasteEquipment]').val() != "")
            totalEquipment = parseFloat($('input[totalTimeWasteEquipment]').val());
        var totalManipulation = 0;
        if ($('input[totalTimeWasteManipulation]').val() != "")
            totalManipulation = parseFloat($('input[totalTimeWasteManipulation]').val());
        var totalDB = 0;
        if ($('input[totalTimeWasteDB]').val() != "")
            totalDB = parseFloat($('input[totalTimeWasteDB]').val());
        var totalNPL = 0;
        if ($('input[totalTimeWasteNPL]').val() != "")
            totalNPL = parseFloat($('input[totalTimeWasteNPL]').val());
        $('input[totalTimeWaste]').val(totalEquipment + totalManipulation + totalDB + totalNPL);
    }
    function GetTotalTMUAndTimeVersion() {
        var totalTimeWaste = 0;
        if ($('input[totalTimeWaste]').val() != "")
            totalTimeWaste = parseFloat($('input[totalTimeWaste]').val());
        var totalTime = 0;
        if ($('input[totalTime]').val() != "")
            totalTime = parseFloat($('input[totalTime]').val());
        $('#mani-ver-TotalTMU').val(Math.round((totalTime + totalTimeWaste) * 1000) / 1000);
    }
    function UpdateIntWaste() {
        if (Global.Data.PhaseManiVerDetailArray != null && Global.Data.PhaseManiVerDetailArray.length > 0) {
            var totalTMUEquipment = 0;
            var totalTMUManipulation = 0;

            $.each(Global.Data.PhaseManiVerDetailArray, function (i, obj) {
                totalTMUEquipment += parseFloat(obj.TMUEquipment) * obj.Loop;
                totalTMUManipulation += parseFloat(obj.TMUManipulation) * obj.Loop;
            });

            $('input[totalTMUEquipment]').val(totalTMUEquipment);
            $('input[totalTMUManipulation]').val(totalTMUManipulation);
            var totalTimeEquipment = totalTMUEquipment / Global.Data.TMU;
            var totalTimeManipulation = totalTMUManipulation / Global.Data.TMU;
            $('input[totalTimeEquipment]').val(totalTimeEquipment);
            $('input[totalTimeManipulation]').val(totalTimeManipulation);
            $('input[totalTMU]').val(totalTMUEquipment + totalTMUManipulation);
            $('input[totalTime]').val(totalTimeEquipment + totalTimeManipulation);

            if ($('input[percentEquipment]').val() != "") {
                var percent = parseInt($('input[percentEquipment]').val());
                var totalTimeEquipment = 0;
                if ($('input[totalTimeEquipment]').val() != "")
                    totalTimeEquipment = parseFloat($('input[totalTimeEquipment]').val());
                $('input[totalTimeWasteEquipment]').val((percent * totalTimeEquipment) / 100);
            }

            if ($('input[percentManipulation]').val() != "") {
                var percent = parseInt($('input[percentManipulation]').val());
                var totalTimeManipulation = 0;
                if ($('input[totalTimeManipulation]').val() != "")
                    totalTimeManipulation = parseFloat($('input[totalTimeManipulation]').val());
                $('input[totalTimeWasteManipulation]').val((percent * totalTimeManipulation) / 100);
            }

            if ($('input[percentDB]').val() != "") {
                var percent = parseInt($('input[percentDB]').val());
                var totalTime = 0;
                if ($('input[totalTime]').val() != "")
                    totalTime = parseFloat($('input[totalTime]').val());
                $('input[totalTimeWasteDB]').val((percent * totalTime) / 100);
            }

            if ($('input[percentNPL]').val() != "") {
                var percent = parseInt($('input[percentNPL]').val());
                var totalTime = 0;
                if ($('input[totalTime]').val() != "")
                    totalTime = parseFloat($('input[totalTime]').val());
                $('input[totalTimeWasteNPL]').val((percent * totalTime) / 100);
            }
            GetTotalPercentWaste();
            GetTotalTimeWaste();
            GetTotalTMUAndTimeVersion();
            UpdateTotalTimeVersion();
        }
    }
    function UpdateTotalTimeVersion() {
        var totalTMUPrepare = 0;
        $.each(Global.Data.TimePrepareArray, function (i, item) {
            totalTMUPrepare += parseFloat(item.TMUNumber);
        });
        var totalTimePrepare = totalTMUPrepare / Global.Data.TMU;
        var totalTimeManiVerTMU = ($('[totaltime]').val() != "" ? parseFloat($('[totaltime]').val()) : 0) + ($('[totaltimewaste]').val() != '' ? parseFloat($('[totaltimewaste]').val()) : 0);
        Global.Data.PhaseModel.TotalTMU = totalTimePrepare + totalTimeManiVerTMU;
        $('#TotalTMU').html(Math.round(Global.Data.PhaseModel.TotalTMU * 1000) / 1000);
        $('#lbTenCongDoan').empty().append(`${$('#phase-name').val()} <i class="fa fa-share-square-o red"></i> TG CĐ: <span class="red">${$('#TotalTMU').html()}</span> (s)`);
    }
    //#endregion
}
$(document).ready(function () {
    var obj = new GPRO.PhaseLibs();
    obj.Init();
});