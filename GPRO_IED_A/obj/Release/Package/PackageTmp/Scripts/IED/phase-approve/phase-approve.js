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
GPRO.namespace('PhaseApprove');
GPRO.PhaseApprove = function () {
    var Global = {
        UrlAction: {
            Gets: '/PhaseApprove/Gets',
            Approve: '/PhaseApprove/Approve'
        },
        Element: {
            JtablePhase: 'tb-phase-',
            JtablePhaseLib: 'tb-phase-lib-',
            Popup: 'phase-approve-popup-',
            JtableManipulationArr:'jtable_ManipulationArr'
        },
        Data: {
            isPhaseLib: false,
            TimePrepareArray: [],
            PhaseManiVerDetailArray: [],
            Video: '',
            PhaseAutoCode: '',
            phaseLastIndex: 0,
            Commo_Ana_PhaseId: 0,
            TMU: 0,
            PhaseModel: {},
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitTablePhase();
        InitTablePhase_Lib();
        ReloadTablePhase();
        ReloadTablePhase_Lib();

        InitListMani_Arr();

        InitPopup();
        $('[re_workerslevel]').click();
        if ($('#config').attr('tmu') != "");
        Global.Data.TMU = parseFloat($('#config').attr('tmu'));
        if ($('#config').attr('gettmutype') != "")
            Global.Data.IntGetTMUType = parseInt($('#config').attr('gettmutype'));
    }
     
    var RegisterEvent = function () {
        $('[re_workerslevel]').click(function () {
            GetWorkersLevelSelect('workersLevel');
        });
    }

    function InitTablePhase() {
        $('#' + Global.Element.JtablePhase).jtable({
            title: 'Danh sách Công Đoạn',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.Gets,
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
                IsLibrary: {
                    width: "3%",
                    display: function (data) {
                        var txt = '';
                        if (data.record.IsLibrary)
                            var txt = $('<i title="công đoạn mẫu" class="fa fa-flag red"  ></i>');
                        return txt;
                    }
                },
                ProductName: {
                    title: "Mã hàng",
                    width: "7%",
                    sorting: false
                },
                WorkshopName: {
                    title: "Phân xưởng",
                    width: "7%",
                    sorting: false
                },
                PhaseGroupName: {
                    title: "Cụm công đoạn",
                    width: "7%",
                    sorting: false
                },
                Index: {
                    title: "Mã Công Đoạn",
                    width: "3%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.Code + '</span>';
                        return txt;
                    }
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Công Đoạn",
                    width: "15%",
                },
                WorkerLevelId: {
                    title: "Bậc thợ",
                    width: "5%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.WorkerLevelName + '</span>';
                        return txt;
                    }
                },
                TotalTMU: {
                    title: "Thời gian thực hiện(s)",
                    width: "5%",
                    display: function (data) {
                        txt = '<span class="red bold">' + Math.round((data.record.TotalTMU) * 1000) / 1000 + '</span>';
                        return txt;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "5%",
                    sorting: false
                },
                action: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        let div = $('<div class="table-action"></div>');
                        var _edit = $('<i data-toggle="modal" data-target="#' + Global.Element.Popup + '" title="Xem thông tin" class="fa fa-info-circle clickable blue"  ></i>');
                        _edit.click(function () {
                            data.record.TotalTMU = Math.round(data.record.TotalTMU * 1000) / 1000;
                            $('#workersLevel').val(data.record.WorkerLevelId);
                            $('#TotalTMU').html(data.record.TotalTMU);
                            $('#phase-Des').val(data.record.Description);
                            $('#phaseID').val(data.record.Id);
                            $('#phase-name').val(data.record.Name).change();
                            $('#phase-code').html(data.record.Code);
                            $('#phase-index').val(data.record.Index);

                            $('#time-repare-id').val(data.record.TimePrepareId);
                            $('#time-repare-name').val(`${data.record.TimePrepareName} - TMU: ${data.record.TimePrepareTMU}`);
                            $('#time-repare-id').attr('tmu', data.record.TimePrepareTMU);

                            $('#equipmentId').val(data.record.EquipmentId);
                            $('#equipmentName').val(data.record.EquipName);
                            $('#equiptypedefaultId').val(data.record.EquipTypeDefaultId);
                            $('#E_info').val(data.record.EquipDes);
                            $('#ApplyPressure').val(data.record.ApplyPressuresId);
                            $('#islibs').prop('checked', data.record.IsLibrary);
                            $('#chooseApplyPressure').hide();
                            if (data.record.ApplyPressuresId != 0)
                                $('#chooseApplyPressure').show();
                            Global.Data.PhaseManiVerDetailArray.length = 0;

                            if (data.record.actions.length > 0) {
                                $.each(data.record.actions, function (i, item) {
                                    item.OrderIndex = i + 1;
                                });
                                $.each(data.record.actions, function (i, item) {
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
                            $('[percentequipment]').val(data.record.PercentWasteEquipment);
                            $('[percentmanipulation]').val(data.record.PercentWasteManipulation);
                            $('[percentdb]').val(data.record.PercentWasteSpecial);
                            $('[percentnpl]').val(data.record.PercentWasteMaterial);
                            UpdateIntWaste();
                            Global.Data.isInsertPhase = false;
                            Global.Data.Video = data.record.Video;
                            var video = document.getElementsByTagName('video')[0];
                            var sources = video.getElementsByTagName('source');
                            if (data.record.Video) {
                                $('#video-info').html(data.record.Video.split('|')[1] + '  <i onclick="removeVideo()" title="Gỡ video" class="fa fa-trash-o red clickable"></i>')

                                sources[0].src = data.record.Video.split('|')[0];
                                sources[1].src = data.record.Video.split('|')[0];
                                video.load();
                            }
                            else {
                                $('#video-info').html('');
                                sources[0].src = '';
                                video.load();
                            }
                        });
                        div.append(_edit);
                         
                        return div;
                    }
                },
            },
        });
    }

    function ReloadTablePhase() {
        $('#' + Global.Element.JtablePhase).jtable('load', { 'fromLib': false });
    }

    function InitTablePhase_Lib() {
        $('#' + Global.Element.JtablePhaseLib).jtable({
            title: 'Danh sách Công Đoạn',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.Gets,
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
                IsLibrary: {
                    width: "3%",
                    display: function (data) {
                        var txt = '';
                        if (data.record.IsLibrary)
                            var txt = $('<i title="công đoạn mẫu" class="fa fa-flag red"  ></i>');
                        return txt;
                    }
                },
                PhaseGroupName: {
                    visibility: 'fixed',
                    title: "Cụm Công Đoạn",
                    width: "10%",
                },
                Index: {
                    title: "Mã Công Đoạn",
                    width: "5%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.Code + '</span>';
                        return txt;
                    }
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Công Đoạn",
                    width: "15%",
                },
                WorkerLevelId: {
                    title: "Bậc thợ",
                    width: "5%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.WorkerLevelName + '</span>';
                        return txt;
                    }
                },
                TotalTMU: {
                    title: "Thời gian thực hiện(s)",
                    width: "5%",
                    display: function (data) {
                        txt = '<span class="red bold">' + Math.round((data.record.TotalTMU) * 1000) / 1000 + '</span>';
                        return txt;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "5%",
                    sorting: false
                },
                action: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        let div = $('<div class="table-action"></div>');

                        var _edit = $('<i data-toggle="modal" data-target="#' + Global.Element.Popup + '" title="Xem thông tin" class="fa fa-info-circle clickable blue"  ></i>');
                        _edit.click(function () {
                            Global.Data.isPhaseLib = true;
                            data.record.TotalTMU = Math.round(data.record.TotalTMU * 1000) / 1000;
                            $('#workersLevel').val(data.record.WorkerLevelId);
                            $('#TotalTMU').html(data.record.TotalTMU);
                            $('#phase-Des').val(data.record.Description);
                            $('#phaseID').val(data.record.Id);
                            $('#phase-name').val(data.record.Name).change();
                            $('#phase-code').html(data.record.Code);
                            $('#phase-index').val(data.record.Index);

                            $('#time-repare-id').val(data.record.TimePrepareId);
                            $('#time-repare-name').val(`${data.record.TimePrepareName} - TMU: ${data.record.TimePrepareTMU}`);
                            $('#time-repare-id').attr('tmu', data.record.TimePrepareTMU);

                            $('#equipmentId').val(data.record.EquipmentId);
                            $('#equipmentName').val(data.record.EquipName);
                            $('#equiptypedefaultId').val(data.record.EquipTypeDefaultId);
                            $('#E_info').val(data.record.EquipDes);
                            $('#ApplyPressure').val(data.record.ApplyPressuresId);
                            $('#islibs').prop('checked', data.record.IsLibrary);
                            $('#chooseApplyPressure').hide();
                            if (data.record.ApplyPressuresId != 0)
                                $('#chooseApplyPressure').show();
                            Global.Data.PhaseManiVerDetailArray.length = 0;

                            if (data.record.actions.length > 0) {
                                $.each(data.record.actions, function (i, item) {
                                    item.OrderIndex = i + 1;
                                });
                                $.each(data.record.actions, function (i, item) {
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
                            $('[percentequipment]').val(data.record.PercentWasteEquipment);
                            $('[percentmanipulation]').val(data.record.PercentWasteManipulation);
                            $('[percentdb]').val(data.record.PercentWasteSpecial);
                            $('[percentnpl]').val(data.record.PercentWasteMaterial);
                            UpdateIntWaste();
                            Global.Data.isInsertPhase = false;
                            Global.Data.Video = data.record.Video;
                            var video = document.getElementsByTagName('video')[0];
                            var sources = video.getElementsByTagName('source');
                            if (data.record.Video) {
                                $('#video-info').html(data.record.Video.split('|')[1] + '  <i onclick="removeVideo()" title="Gỡ video" class="fa fa-trash-o red clickable"></i>')

                                sources[0].src = data.record.Video.split('|')[0];
                                sources[1].src = data.record.Video.split('|')[0];
                                video.load();
                            }
                            else {
                                $('#video-info').html('');
                                sources[0].src = '';
                                video.load();
                            }
                        });
                        div.append(_edit);
                         
                        return div;
                    }
                },
            },
        });
    }

    function ReloadTablePhase_Lib() {
        $('#' + Global.Element.JtablePhaseLib).jtable('load', { 'fromLib': true });
    }

    function InitPopup() {
        $("#" + Global.Element.Popup).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.Popup + ' button[approve-phase]').click(function () {
            Approve(true);
        });

        $("#" + Global.Element.Popup + ' button[not-approve-phase]').click(function () {
            Approve(false);
        });

        $("#" + Global.Element.Popup + ' button[cancel-create-phase]').click(function () {
            $("#" + Global.Element.Popup).modal("hide"); 
            Global.Data.TimePrepareArray.length = 0; 
            Global.Data.PhaseManiVerDetailArray.length = 0; 
            ReloadListMani_Arr();

            $('div.divParent').attr('currentPoppup', Global.Element.JtablePhase.toUpperCase());
            Global.Data.Video = '';
            $('#video').val('');
            $('#phase-index').val('');
            $('#TotalTMU,#TotalMay').html('0');
            $('#time-repare-name').val('');
            $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + (Global.Data.phaseLastIndex + 1)));

            var video = document.getElementsByTagName('video')[0];
            var sources = video.getElementsByTagName('source');
            sources[0].src = '';
            sources[1].src = '';
            video.load();


           // GetLastPhaseIndex();
            ReloadListPhase_View();

            $('#phaseName_label').html('');
            Global.Data.Commo_Ana_PhaseId = 0;

            $('#phase-name').val('').change();
            $('#lbTenCongDoan').html('');
            $('#phase-index').val('');
            $('#phase-Des').val('');
            $('#phaseID').val('0');
            $('#islibs').prop('checked', false);             
        });
    }

    function Approve(isApprove) {
        $.ajax({
            url: Global.UrlAction.Approve,
            type: 'POST',
            data: JSON.stringify({ 'Id': $('#phaseID').val(), 'isApprove': isApprove, 'phaseLib': Global.Data.isPhaseLib }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        if (Global.Data.isPhaseLib)
                            ReloadTablePhase_Lib();
                        else
                            ReloadTablePhase();
                        $('[cancel-create-phase]').click();
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupCommodityAnalysis, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    //#region Thao tác công đoạn
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
                    width: "15%", 
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
                } 
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
     
    //#endregion

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
    var obj = new GPRO.PhaseApprove();
    obj.Init();
});


