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
GPRO.namespace('PhaseGroupAna');
GPRO.PhaseGroupAna = function () {
    var Global = {
        UrlAction: {
            GetAllManipulation: '/MType/GetAllManipulation',
            GetPhasesForSugest: '/ProAna/GetPhasesForSuggest',
            GetPhaseById: '/ProAna/GetPhaseSusgestById',

            SavePhase: '/PhaseGroupAna/Save',
            DeletePhase: '/PhaseGroupAna/Delete',
            RemovePhaseVideo: '/PhaseGroupAna/RemoveVideo',
            CopyPhase: '/PhaseGroupAna/Copy',
            GetLastIndex: '/PhaseGroupAna/GetLastIndex',
            GetListPhase: '/PhaseGroupAna/Gets',
            Approve: '/PhaseApprove/Approve',
            ExportGroup: '/PhaseGroupAna/Export_CommoAnaPhaseGroup',
            Export: '/PhaseGroupAna/export_PhaseManiVersion',

            TinhLaiCode: '/PhaseGroupAna/TinhLaiCode',

            GetListEquipment: '/Equipment/Gets',
            GetManipulationEquipmentInfoByCode: '/MType/GetManipulationEquipmentInfoByCode',
            GetListTimePrepare: '/TimePrepare/GetLists',
            GetTimeTypePrepare: '/TimePrepare/GetTimeTypePreparesByWorkShopId',
        },
        Element: {
            CreatePhasePopup: 'Create-Phase-Popup',

            jtablePhase: 'jtable-phase',
            jtablePhaseVersion: 'techprocess',

            JtableManipulationArr: 'jtable_ManipulationArr',
            PopupSearchEquipment: 'POPUP_SEARCHEQUIP',
            JtableEquipment: 'jtable-chooseequipment',
            PopupChooseEquipment: 'ChooseEquipment_Popup',

            jtable_Timeprepare_Chooise: 'jtable-timeprepare',
            jtable_timeprepare_arr: 'jtable-timeprepare-arr',
            timeprepare_Popup: 'timeprepare-Popup',
            timeprepare_Popup_Search: 'timePrepare_PopupSearch',

        },
        Data: {
            Approver: false,
            PhaseStatus: 'Soạn thảo',
            PhaseGroupId: 0,

            PhaseNode: '',

            ManipulationList: [],
            PhasesSuggest: [],
            SuggestPhaseId: 0,

            PhaseManiVerDetailArray: [],
            isInsertPhase: true,
            PhaseModel: {},
            isUserMachine: false,
            EquipTypeDefaultId: { SE: 1, C: 2 },
            ManipulationVersionModel: {},
            PhaseAutoCode: '',
            phaseLastIndex: 1,
            IntGetTMUType: 0,
            TMU: 0,
            Video: '',
            AccessoriesArray: [],
            Commo_Ana_PhaseId: 0,
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Export = function (_id) {
        window.location.href = Global.UrlAction.ExportGroup + "?id=" + _id;
    }

    this.Init = function () {
        RegisterEvent();
        GetPhaseGroup();

        GetWorkersLevelSelect('workersLevel');
        GetApplyPressureSelect('ApplyPressure');
        GetAllManipulation();
        GetPhasesForSuggest();

        if ($('#config').attr('tmu') != "");
        Global.Data.TMU = parseFloat($('#config').attr('tmu'));
        if ($('#config').attr('gettmutype') != "")
            Global.Data.IntGetTMUType = parseInt($('#config').attr('gettmutype'));
        InitListPhase_View();
        InitPopupChooseEquipment();
        InitPopupTimeRepare();
        InitListTimePrepare_chooise();

        InitListEquipment();
        InitListMani_Arr();
        AddEmptyObject();
        ReloadListMani_Arr();
        UpdateIntWaste();
        $('.box-jtable').hide();
    }


    var RegisterEvent = function () {

        $('#filter-phasegroup').keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 13)
                GetPhaseGroup();
            return true;

        });

        $('[re_workerslevel]').click(function () {
            GetWorkersLevelSelect('workersLevel');
        });

        $('[re-apply-pressure]').click(function () {
            GetApplyPressureSelect('ApplyPressure');
        });

        $('#phase-suggest').click(() => {
            $('#phase-suggest').select();
        })

        $('#phase-suggest').change(() => {
            var selectValue = $('#phase-suggest').val();
            if (selectValue != '') {
                var found = Global.Data.PhasesSuggest.filter((item, i) => {
                    return (item.Name == selectValue || item.Code == selectValue);
                })[0];
                if (found != null) {
                    // GlobalCommon.ShowMessageDialog("Thông tin Công Đoạn : " + found.Name + " (<b class='red'>" + found.Code + "</b>) tổng TMU : " + found.Double, function () { }, "Thông báo");

                    $('#phase-suggest').val(found.Name + " ( " + found.Code + " ) tổng TMU : " + found.Double);
                    Global.Data.SuggestPhaseId = found.Value;
                }
                else {
                    GlobalCommon.ShowMessageDialog("Không tìm thấy thông tin Công Đoạn : <b class='red'>" + selectValue + "</b> ", function () { }, "Thông báo");
                }
            }
        });

        $('[save-sugguest-phase]').click(() => {
            var selectValue = $('#phase-suggest').val();
            if (selectValue != '') {
                GetPhasesById();
                // alert(selectValue)
            }
            else {
                GlobalCommon.ShowMessageDialog("Vui lòng nhập tên hoặc mã Công Đoạn vào ô <b class='red'>Tìm Công Đoạn</b>", function () { }, "Thông báo");
            }
        });

        $('#phase-name').change(() => {
            $('#lbTenCongDoan').empty().append(`${$('#phase-name').val()} <i class="fa fa-share-square-o red"></i> TG CĐ: <span class="red">${$('#TotalTMU').html()}</span> (s)`);
        });

        $('#phase-name').keyup(() => {
            $('#phase-name').change();
        });

        $('#phase-index').change(function () {
            $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + $('#phase-index').val()));
        });

        $('[filelist]').select(function () {
            SavePhase();
        });

        $('#hid_video').change(function () { SavePhase(); });

        $('#remove-video').click(() => {
            removeVideo();
        })

        $('[save-phase]').click(function () {
            Global.Data.PhaseStatus = 'Soạn thảo';
            if (Check_Phase_Validate()) {
                if ($('#video').val() != '')
                    UploadVideo();
                else
                    SavePhase();
            }
        });

        $('[submit-phase]').click(function () {
            if (Check_Phase_Validate()) {
                Global.Data.PhaseStatus = 'Chờ duyệt';
                if ($('#video').val() != '')
                    UploadVideo();
                else
                    SavePhase();
            }
        });


        $('[approve-phase]').click(function () {
            ApprovePhase(true);
        });

        $('[not-approve-phase]').click(function () {
            Approve( );
        });

        $('[cancel-create-phase]').click(function () {
            Global.Data.isInsertPhase = true;
            Global.Data.PhaseManiVerDetailArray.length = 0;
            AddEmptyObject();
            ReloadListMani_Arr();

            // $('div.divParent').attr('currentPoppup', Global.Element.JtablePhase.toUpperCase());
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
            $('#video-info').html('');

            GetLastPhaseIndex();
            ReloadListPhase_View();

            $('#phaseName_label').html('');
            Global.Data.Commo_Ana_PhaseId = 0;

            $('#phase-name').val('').change();
            $('#lbTenCongDoan').html('');
            $('#phase-index').val('');
            $('#phase-Des').val('');
            $('#phaseID').val('0');
            $('#islibs').prop('checked', false);
            $('[save-phase],[submit-phase]').show();
            $('[not-approve-phase]').hide();
            // $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + (Global.Data.phaseLastIndex + 1)));

        });

        $('#btn-browse-file').click(function () {
            $('#video').click();
        })
    }
    
    function GetPhaseGroup() {
        $.ajax({
            url: '/PhaseGroup/GetSelectByKey',
            type: 'POST',
            data: JSON.stringify({ 'keyword': $('#filter-phasegroup').val() }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        if (data.Data.length > 0) {
                            let _box = $('.phasegroup-box');
                            _box.empty();

                            if (data.Data.length > 0) {
                                $.each(data.Data, function (index, item) {
                                    if (item.Value != 0) {
                                        let _item = $(`<div class="group-item"><div> <i class="fa fa-caret-right red"></i> <span>${item.Name}</span></div> <i onclick="Export(${item.Value})" class="fa fa-file-excel-o red" title="Xuất tệp phân tích cụm công đoạn"></i></div>`);
                                        _item.click(function () {
                                            $('.box-jtable').show();
                                            Global.Data.PhaseGroupId = item.Value;
                                            GetLastPhaseIndex();
                                            $('.title-name').html(`DANH SÁCH CÔNG ĐOẠN : <span class="red bold">${item.Name}</span>`);
                                            ReloadListPhase_View()
                                        });
                                        _box.append(_item);
                                    }
                                });
                            }
                        }
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    /*********************************************** LIBRARY ***********************************************************/
    function GetPhasesForSuggest() {
        $.ajax({
            url: Global.UrlAction.GetPhasesForSugest,
            type: 'POST',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        Global.Data.PhasesSuggest.length = 0;
                        var option = '';
                        if (data.Records != null && data.Records.length > 0) {
                            $.each(data.Records, function (i, item) {
                                Global.Data.PhasesSuggest.push(item);
                                option += '<option value="' + item.Code + '" /> ';
                                option += '<option value="' + item.Name + '" /> ';
                            });
                        }
                        $('#suggestPhases').empty().append(option);
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

    function GetAllManipulation() {
        $.ajax({
            url: Global.UrlAction.GetAllManipulation,
            type: 'POST',
            data: '',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        var option = '';
                        if (data.Data != null && data.Data.length > 0) {
                            $.each(data.Data, function (i, item) {
                                Global.Data.ManipulationList.push(item);
                                option += '<option value="' + item.Code + '" /> ';
                                option += '<option value="' + item.Name + '" /> ';
                            });
                        }
                        $('#manipulations').append(option);
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

    /*********************************************** END LIBRARY ***********************************************************/

    /*********************************************** PHASE ***********************************************************/

    function InitListPhase_View() {
        $('#' + Global.Element.jtablePhase).jtable({
            title: 'Danh sách Công Đoạn',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            rowInserted: function (event, data) {
                if (data.record.Id == Global.Data.Commo_Ana_PhaseId) {
                    var $a = $('#' + Global.Element.jtablePhase).jtable('getRowByKey', data.record.Id);
                    $($a.children().find('.aaa')).click();
                }
            },
            actions: {
                listAction: Global.UrlAction.GetListPhase,
                createAction: Global.Element.CreatePhasePopup,
            },
            messages: {
                addNewRecord: 'Thêm mới ',
                //selectShow: 'Ẩn hiện cột'
            },
            searchInput: {
                id: 'phasegroup-phase-keyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadListPhase_View();
                }
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
                Index: {
                    title: "Mã Công Đoạn",
                    width: "7%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.Code + '</span>';
                        return txt;
                    }
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Công Đoạn",
                    width: "25%",
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
                Status: {
                    title: 'Trạng thái',
                    width: '5%',
                    display: function (data) {
                        let cls = 'normal-text';
                        switch (data.record.Status) {
                            case 'Chờ duyệt': cls = 'danger-text'; break;
                            case 'Đã duyệt': cls = 'primary-text'; break;
                            default: break;
                        }
                        return `<span class='${cls}'>${data.record.Status}</span>`;
                    }
                },
                action: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        let div = $('<div class="table-action"></div>');

                        var _excel = $('<i   title="Copy công đoạn" class="fa fa-file-excel-o clickable red"  ></i>');
                        _excel.click(function () {
                            window.location.href = Global.UrlAction.Export + "?Id=" + data.record.Id;
                        });
                        div.append(_excel);

                        var _copy = $('<i   title="Copy công đoạn" class="fa fa-files-o clickable blue"  ></i>');
                        _copy.click(function () {
                            CopyPhase(data.record.Id);
                        });
                        div.append(_copy);

                        var _edit = $('<i data-toggle="modal" data-target="#' + Global.Element.CreatePhasePopup + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        if (data.record.Status !== "Soạn thảo")
                            _edit = $('<i data-toggle="modal" data-target="#' + Global.Element.CreatePhasePopup + '" title="Xem thông tin" class="fa fa-info-circle clickable blue"  ></i>');
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
                            AddEmptyObject();
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

                            $('#phase-status').html(data.record.Status);
                            $('#phase-status').attr('class', '');
                            let cls = 'normal-text';
                            switch (data.record.Status) {
                                case 'Chờ duyệt': cls = 'danger-text'; break;
                                case 'Đã duyệt': cls = 'primary-text'; break;
                                default: break;
                            }
                            $('#phase-status').addClass(cls);

                            if (data.record.Status == "Chờ duyệt" || data.record.Status == "Đã duyệt") {
                                $('[save-phase],[submit-phase]').hide();
                                $('[not-approve-phase]').show();
                            }
                            else
                                $('[not-approve-phase]').hide();
                        });
                        div.append(_edit);

                        if (data.record.Status == "Soạn thảo" || (Global.Data.Approver && data.record.Status != 'Đã duyệt')) {
                            var _delete = $('<i title="Xóa" class="fa fa-trash-o"></i>');
                            _delete.click(function () {
                                GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                    DeletePhase(data.record.Id);
                                }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                            });
                            div.append(_delete);
                        }
                        return div;
                    }
                },  
            },
        });
    }

    function ReloadListPhase_View() {
        $('#' + Global.Element.jtablePhase).jtable('load', { 'phaseGroupId': Global.Data.PhaseGroupId, 'keyword': $('#phasegroup-phase-keyword').val() });
    }

    function GetLastPhaseIndex() {
        $.ajax({
            url: Global.UrlAction.GetLastIndex,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.PhaseGroupId }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                if (data.Result == "OK") {
                    Global.Data.phaseLastIndex = data.Records;
                    $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + (Global.Data.phaseLastIndex + 1)));
                    $('#phase-index').val((Global.Data.phaseLastIndex + 1));
                }
                else
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
            }
        });
    }

    function Check_Phase_Validate() {
        if ($('#phase-name').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui Nhập Tên Công Đoạn .", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#workersLevel').val() == "" || $('#workersLevel').val() == "0") {
            GlobalCommon.ShowMessageDialog("Vui Nhập chọn bậc thợ .", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#time-repare-id').val() == "" || $('#time-repare-id').val() == "0") {
            GlobalCommon.ShowMessageDialog("Vui chọn thời gian chuẩn bị.", function () { $('#time-repare-id').focus() }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

    function SavePhase() {
        var obj = {
            Id: $('#phaseID').val() == '' ? 0 : $('#phaseID').val(),
            Name: $('#phase-name').val(),
            Code: $('#phase-code').html(),
            Description: $('#phase-Des').val(),
            EquipmentId: $('#equipmentId').val(),
            WorkerLevelId: $('#workersLevel').val(),
            PhaseGroupId: Global.Data.PhaseGroupId,
            TotalTMU: parseFloat($('#TotalTMU').html()),
            ApplyPressuresId: $('#ApplyPressure').val(),
            PercentWasteEquipment: $('[percentEquipment]').val(),
            PercentWasteManipulation: $('[percentManipulation]').val(),
            PercentWasteSpecial: $('[percentDB]').val(),
            PercentWasteMaterial: $('[percentNPL]').val(),
            Node: Global.Data.Node,
            ManiVerTMU: 0,
            IsDetailChange: Global.Data.ManipulationVersionModel.IsDetailChange,
            actions: Global.Data.PhaseManiVerDetailArray,
            Index: ($('#phase-index').val() == '' ? (Global.Data.phaseLastIndex + 1) : parseInt($('#phase-index').val())),
            Video: Global.Data.Video,
            TimePrepareId: $('#time-repare-id').val(),
            IsLibrary: $('#islibs').prop('checked'),
            Status: Global.Data.PhaseStatus
        }
        $.ajax({
            url: Global.UrlAction.SavePhase,
            type: 'post',
            data: JSON.stringify({ 'phase': obj, 'accessories': Global.Data.AccessoriesArray }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        if (Global.Data.PhaseStatus == 'Chờ duyệt') {
                            GetLastPhaseIndex();
                            ReloadListPhase_View();
                            $('[cancel-create-phase]').click();
                        }
                        else {
                            if (obj.Id == 0) {
                                $('#phaseID').val(result.Data);
                            }
                        }

                        /*
                        GetLastPhaseIndex();
                        ReloadListPhase_View();
                        $('#phaseName_label').html('');
                        Global.Data.Commo_Ana_PhaseId = 0;
                        if (!Global.Data.isInsertPhase) {
                            $('#' + Global.Element.CreatePhasePopup).modal('hide');
                            Global.Data.isInsertPhase = true;
                        }
                        $('#phase-name').val('').change();
                        $('#lbTenCongDoan').html('');
                        $('#phase-index').val('');
                        $('#TotalTMU').html('0');
                        $('#phase-Des').val('');
                        $('#phaseID').val('0');
                        $('#islibs').prop('checked', false);
                        Global.Data.PhaseManiVerDetailArray.length = 0;
                        AddEmptyObject();
                        ReloadListMani_Arr();
                        $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + (Global.Data.phaseLastIndex + 1)));
                        */
                        UpdateIntWaste();
                        GetPhasesForSuggest();
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


    function DeletePhase(Id) {
        $.ajax({
            url: Global.UrlAction.DeletePhase,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListPhase_View();
                        GetLastPhaseIndex();
                        GetPhasesForSuggest();
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

    function CopyPhase(Id) {
        $.ajax({
            url: Global.UrlAction.CopyPhase,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListPhase_View();
                        GetLastPhaseIndex();
                        GetPhasesForSuggest();
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

    function GetPhasesById() {
        $.ajax({
            url: Global.UrlAction.GetPhaseById,
            type: 'POST',
            data: JSON.stringify({ 'phaseId': Global.Data.SuggestPhaseId }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    $('#phase-suggest').val('');
                    if (data.Result == "OK") {
                        data.Records.TotalTMU = Math.round(data.Records.TotalTMU * 1000) / 1000;
                        $('#workersLevel').val(data.Records.WorkerLevelId);
                        $('#TotalTMU').html(data.Records.TotalTMU);
                        $('#phase-Des').val(data.Records.Description);
                        // $('#phaseID').val(data.Records.Id);
                        $('#phase-name').val(data.Records.Name).change();
                        $('#phase-index').html(data.Records.Index);

                        if (data.Records.timePrepares && data.Records.timePrepares.length > 0) {
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
                        AddEmptyObject();
                        ReloadListMani_Arr();
                        $('[percentequipment]').val(data.Records.PercentWasteEquipment);
                        $('[percentmanipulation]').val(data.Records.PercentWasteManipulation);
                        $('[percentdb]').val(data.Records.PercentWasteSpecial);
                        $('[percentnpl]').val(data.Records.PercentWasteMaterial);
                        UpdateIntWaste();
                        Global.Data.isInsertPhase = false;
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

    function UploadVideo() {
        if (window.FormData !== undefined) {
            var fileUpload = $('#video').get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            $.ajax({
                url: '/ProAna/UploadVideo',
                type: "POST",
                data: fileData,
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                beforeSend: function () { $('#loading').show(); },
                success: function (result) {
                    Global.Data.Video = result;
                    $('#hid_video').val(result).change();
                    $('#loading').hide();
                },
                error: function (err) {
                    alert("Lỗi up hình : " + err.statusText);
                    $('#loading').hide();
                }
            });
        }
        else
            alert("FormData is not supported.");
    }

    removeVideo = () => {
        $.ajax({
            url: Global.UrlAction.RemovePhaseVideo,
            type: 'POST',
            data: JSON.stringify({ 'Id': $('#phaseID').val() }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        $('[cancel-create-phase]').click();
                        $('#' + Global.Element.CreatePhasePopup).modal('hide');
                        Global.Data.isInsertPhase = true;
                        ReloadListPhase_View();
                        GetLastPhaseIndex();
                        GetPhasesForSuggest();
                        $('#video-info').html('');
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

    function Approve() {
        $.ajax({
            url: Global.UrlAction.Approve,
            type: 'POST',
            data: JSON.stringify({ 'Id': $('#phaseID').val(), 'isApprove': false, 'phaseLib': true }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        GetLastPhaseIndex();
                        ReloadListPhase_View();
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

    /*********************************************** END PHASE ***********************************************************/

    /*********************************************** TIME PREPARE LIBRARY ***********************************************************/
    function InitPopupTimeRepare() {
        $('#' + Global.Element.timeprepare_Popup).on('shown.bs.modal', function () {
            $('#' + Global.Element.CreatePhasePopup).hide();
            ReloadListTimePrepare_Chooise();
            $('div.divParent').attr('currentPoppup', Global.Element.timeprepare_Popup.toUpperCase());
        });

        $('[close-time]').click(function () {
            $('#' + Global.Element.CreatePhasePopup).show();
            $('div.divParent').attr('currentPoppup', Global.Element.CreatePhasePopup.toUpperCase());
        });
    }

    function InitListTimePrepare_chooise() {
        $('#' + Global.Element.jtable_Timeprepare_Chooise).jtable({
            title: 'Danh Sách Thời Gian Chuẩn Bị',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            selecting: false, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: false, //Show checkboxes on first column
            actions: {
                listAction: Global.UrlAction.GetListTimePrepare,
            },
            messages: {
            },
            searchInput: {
                id: 'time-prepare-keyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadListTimePrepare_Chooise();
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
                    title: "Tên ",
                    width: "20%",
                    display: function (data) {
                        var txt = $('<a class="clickable"  data-target="#' + Global.Element.timeprepare_Popup + '" >' + data.record.Name + '</a>');
                        txt.click(function () {
                            var record = data.record;
                            var obj = {
                                Id: record.Id,
                                TimePrepareId: record.Id,
                                Name: record.Name,
                                Code: record.Code,
                                TimeTypePrepareName: record.TimeTypePrepareName,
                                Description: record.Description,
                                TMUNumber: record.TMUNumber
                            }

                            $('#' + Global.Element.timeprepare_Popup).modal('hide');
                            $('#' + Global.Element.CreatePhasePopup).show();

                            $('#time-repare-id').val(obj.Id);
                            $('#time-repare-id').attr('tmu', obj.TMUNumber);
                            Global.Data.PhaseModel.IsTimePrepareChange = true;
                            UpdateTotalTimeVersion();
                            $('#time-repare-name').val(`${obj.Name} - TMU: ${obj.TMUNumber}`);

                            $('div.divParent').attr('currentPoppup', Global.Element.JtablePhase.toUpperCase());
                        })
                        return txt;
                    }
                },
                Code: {
                    title: "Mã",
                    width: "5%",
                },
                TimeTypePrepareName: {
                    title: "Loại Thời Gian Chuẩn Bị",
                    width: "20%",
                },
                TMUNumber: {
                    title: "Chỉ số TMU",
                    width: "20%",
                    display: function (data) {
                        txt = '<span class="red bold">' + ParseStringToCurrency(data.record.TMUNumber) + '</span>';
                        return txt;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%",
                    sorting: false,
                }
            }
        });
    }

    function ReloadListTimePrepare_Chooise() {
        $('#' + Global.Element.jtable_Timeprepare_Chooise).jtable('load', { 'keyword': $('#time-prepare-keyword').val() });
    }
    /*********************************************** END TIME PREPARE LIBRARY ***********************************************************/

    /*********************************************** EQUIPMENT ***********************************************************/
    function InitListEquipment() {
        $('#' + Global.Element.JtableEquipment).jtable({
            title: 'Danh sách Thiết Bị',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.GetListEquipment,
            },
            messages: {
            },
            searchInput: {
                id: 'equip-keyword',
                className: 'search-input',
                placeHolder: 'Nhập từ khóa ...',
                keyup: function (evt) {
                    if (evt.keyCode == 13)
                        ReloadListEquipment();
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },

                //Code: {
                //    title: "Mã Thiết Bị",
                //    width: "10%",
                //},
                Name: {
                    visibility: 'fixed',
                    title: "Tên Thiết Bị",
                    width: "20%",
                    display: function (data) {
                        var text = $('<a class="clickable"  data-target="#popup_Equipment" title="Chọn thiết bị cho phiên bản công đoạn.">' + data.record.Name + '</a>');
                        text.click(function () {
                            if (($('#equipmentId').val().trim() == '' || $('#equipmentId').val().trim() == '0') || Global.Data.PhaseManiVerDetailArray.length == 0) {
                                $('#equipmentId').val(data.record.Id);
                                $('#equipmentName').val(data.record.Name);
                                $('#equiptypedefaultId').val(data.record.EquipTypeDefaultId);
                                $('#E_info').val(data.record.Description);

                                $('[percentequipment]').val(data.record.Expend);
                                if (data.record.EquipTypeDefaultId == Global.Data.EquipTypeDefaultId.C)
                                    $('#chooseApplyPressure').show();
                                else
                                    $('#chooseApplyPressure').hide();

                                $('#' + Global.Element.PopupChooseEquipment).modal('hide');
                                $('#' + Global.Element.CreatePhasePopup).show();
                            }
                            else {
                                if (Global.Data.PhaseManiVerDetailArray.length > 0) {
                                    var arr = [];
                                    $.each(Global.Data.PhaseManiVerDetailArray, function (i, item) {
                                        item.ManipulationCode = item.ManipulationCode.trim().toUpperCase();
                                        if (item.ManipulationCode.substring(0, 2) == 'SE' || item.ManipulationCode.substring(0, 1) == 'C')
                                            arr.push(item.OrderIndex);
                                    });
                                    if (arr.length > 0) {
                                        GlobalCommon.ShowConfirmDialog('Khi Bạn thay đổi Thiết Bị, chỉ số TMU các Mã May và Mã Cắt \nđã phân tích trước đó sẽ được tính lại.\nBạn có muốn thay đổi Thiết Bị không ?',
                                            function () {
                                                $('#equipmentId').val(data.record.Id);
                                                $('#equipmentName').val(data.record.Name);
                                                $('#equiptypedefaultId').val(data.record.EquipTypeDefaultId);
                                                $('#E_info').val(data.record.Description);
                                                $('[percentequipment]').val(data.record.Expend);
                                                if (data.record.EquipTypeDefaultId == Global.Data.EquipTypeDefaultId.C) {
                                                    $('#chooseApplyPressure').show();
                                                }
                                                else {
                                                    $('#chooseApplyPressure').hide();
                                                }

                                                TinhLaiCodeMayCat();
                                                $('#' + Global.Element.PopupChooseEquipment).modal('hide');
                                                $('#' + Global.Element.CreatePhasePopup).show();
                                            },
                                            function () {
                                                $('#' + Global.Element.PopupChooseEquipment).modal('hide');
                                                $('#' + Global.Element.CreatePhasePopup).show();
                                            },
                                            'Đồng ý', 'Hủy bỏ', 'Thông báo');
                                    }
                                    else {
                                        $('#equipmentId').val(data.record.Id);
                                        $('#equipmentName').val(data.record.Name);
                                        $('#equiptypedefaultId').val(data.record.EquipTypeDefaultId);
                                        $('#E_info').val(data.record.Description);

                                        $('[percentequipment]').val(data.record.Expend);
                                        if (data.record.EquipTypeDefaultId == Global.Data.EquipTypeDefaultId.C)
                                            $('#chooseApplyPressure').show();
                                        else
                                            $('#chooseApplyPressure').hide();

                                        $('#' + Global.Element.PopupChooseEquipment).modal('hide');
                                        $('#' + Global.Element.CreatePhasePopup).show();
                                    }
                                }
                            }
                        });
                        return text;
                    }
                },
                EquipmentTypeName: {
                    title: "Loại Thiết Bị",
                    width: "20%",
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%",
                },
            }
        });
    }

    function ReloadListEquipment() {
        $('#' + Global.Element.JtableEquipment).jtable('load', { 'keyword': $('#equip-keyword').val() });
    }

    function InitPopupChooseEquipment() {
        $("#" + Global.Element.PopupChooseEquipment).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.PopupChooseEquipment + ' button[close]').click(function () {
            $("#" + Global.Element.PopupChooseEquipment).modal("hide");
        });

        $('#' + Global.Element.PopupSearchEquipment).on('shown.bs.modal', function () {
            $('#' + Global.Element.PopupChooseEquipment).css('z-index', 0);
            $('div.divParent').attr('currentPoppup', Global.Element.PopupSearchEquipment.toUpperCase());
        });

        $('[searchEquipment]').click(function () {
            ReloadListEquipment();
            $('[cancel_search_equip]').click();
        });

        $('[cancel_search_equip]').click(function () {
            $('#keywordequipment').val('');
            $("#" + Global.Element.PopupSearchEquipment).modal("hide");
            $('#' + Global.Element.PopupChooseEquipment).css('z-index', 1040);
            $('div.divParent').attr('currentPoppup', Global.Element.PopupChooseEquipment.toUpperCase());
        });

        $('#equipmentName').click(function () {
            ReloadListEquipment(); $('#' + Global.Element.CreatePhasePopup).hide();
            $('div.divParent').attr('currentPoppup', Global.Element.PopupChooseEquipment.toUpperCase());
        });

        $('[chooseequipment_popupclose]').click(function () {
            $('#' + Global.Element.CreatePhasePopup).show();
            $('div.divParent').attr('currentPoppup', Global.Element.CreatePhasePopup.toUpperCase());
        });

    }
    /*********************************************** END EQUIPMENT ***********************************************************/

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
                    display: function (data) {
                        var txt = $('<input class="form-control text-center" stt type="text" value =\'' + data.record.OrderIndex + '\' />');
                        txt.change(function () {
                            if (data.record.OrderIndex == Global.Data.PhaseManiVerDetailArray.length) {
                                GlobalCommon.ShowMessageDialog('Đây là Thao Tác rỗng không thể thay đỗi Chèn vào vị trí khác được.',
                                    function () { }, "Thao Tác Rỗng");
                                txt.val(data.record.OrderIndex);
                            }
                            else {
                                var Newindex = parseInt(txt.val());
                                var OldIndex = data.record.OrderIndex;
                                if (Newindex <= 0 || Newindex >= Global.Data.PhaseManiVerDetailArray.length) {
                                    GlobalCommon.ShowMessageDialog('Số Thứ Tự Thao Tác phải lớn hơn 0 và nhỏ hơn ' + Global.Data.PhaseManiVerDetailArray.length + '.',
                                        function () { }, "Số Thứ Tự Thao Tác không hợp lệ");
                                    txt.val(data.record.OrderIndex);
                                }
                                else {
                                    var objTemp = Global.Data.PhaseManiVerDetailArray[OldIndex - 1];
                                    objTemp.OrderIndex = Newindex;
                                    Global.Data.PhaseManiVerDetailArray.splice(OldIndex - 1, 1);
                                    Global.Data.PhaseManiVerDetailArray.splice(Newindex - 1, 0, objTemp);
                                    if (Newindex < OldIndex) {
                                        for (var i = Newindex; i < Global.Data.PhaseManiVerDetailArray.length; i++) {
                                            Global.Data.PhaseManiVerDetailArray[i].OrderIndex = i + 1;
                                        }
                                    }
                                    else {
                                        for (var i = 0; i < Global.Data.PhaseManiVerDetailArray.length; i++) {
                                            if (i + 1 != Newindex)
                                                Global.Data.PhaseManiVerDetailArray[i].OrderIndex = i + 1;
                                        }
                                    }

                                    //sorting array
                                    Global.Data.PhaseManiVerDetailArray.sort(function (a, b) {
                                        var nameA = a.OrderIndex, nameB = b.OrderIndex;
                                        if (nameA < nameB)
                                            return -1;
                                        if (nameA > nameB)
                                            return 1;
                                        return 0;
                                    });
                                    ReloadListMani_Arr();
                                }
                            }
                        });
                        //txt.click(function () { txt.select(); })
                        return txt;
                    }
                },
                ManipulationCode: {
                    title: "Mã",
                    width: "15%",
                    display: function (data) {
                        var txt = $('<input class="form-control" code_' + data.record.OrderIndex + ' list="manipulations" type="text" value="' + data.record.ManipulationCode + '" />');
                        txt.change(function () {
                            var code = txt.val().trim().toUpperCase();
                            if (Global.Data.ManipulationList.length > 0 && code != '') {
                                code = code.toUpperCase();
                                var IsNormalCode = true;
                                var flag = false;
                                if (code.length >= 4) {
                                    if (code.substring(0, 1) == "C" || code.substring(0, 2) == "SE") {
                                        LoadChooseManipulationPopup(code, function (dataReturn) {
                                            flag = false;
                                            if (typeof (dataReturn) != 'undefined' && dataReturn != null) {
                                                Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].ManipulationName = dataReturn.Name;
                                                Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].ManipulationCode = dataReturn.Code;
                                                Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TMUEquipment = dataReturn.StandardTMU;
                                                Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TMUManipulation = 0;
                                                Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TotalTMU = Math.round((Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].Loop * dataReturn.StandardTMU) * 1000) / 1000;
                                                if (Global.Data.PhaseManiVerDetailArray.length == data.record.OrderIndex) {
                                                    AddEmptyObject();
                                                    $('[code_' + Global.Data.PhaseManiVerDetailArray.length + ']').focus();
                                                    $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').height());
                                                }
                                                else
                                                    $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').scrollTop());
                                                ReloadListMani_Arr();
                                                UpdateIntWaste();
                                                $('[code_' + Global.Data.PhaseManiVerDetailArray.length + ']').focus();
                                                $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').height());
                                            }
                                        });
                                        IsNormalCode = false;
                                    }
                                    else if (code.substring(0, 3) == "SPT") {
                                        if (isNaN(code.substring(3, code.length))) //ko phai số
                                            GlobalCommon.ShowMessageDialog('Đuôi sau SPT phải là số. Vui lòng nhập lại.', function () { }, "Thông báo");
                                        else {
                                            var _tmu = parseInt(code.substring(3, code.length));
                                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].ManipulationName = "";
                                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].ManipulationCode = code;
                                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TMUEquipment = _tmu;
                                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TMUManipulation = 0;
                                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TotalTMU = Math.round((Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].Loop * _tmu) * 1000) / 1000;
                                            if (Global.Data.PhaseManiVerDetailArray.length == data.record.OrderIndex) {
                                                AddEmptyObject();
                                                $('[code_' + Global.Data.PhaseManiVerDetailArray.length + ']').focus();
                                                $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').height());
                                            }
                                            else
                                                $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').scrollTop());
                                            ReloadListMani_Arr();
                                            UpdateIntWaste();
                                            $('[code_' + Global.Data.PhaseManiVerDetailArray.length + ']').focus();
                                            $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').height());
                                        }
                                        IsNormalCode = false;
                                    }

                                }
                                if (IsNormalCode) {
                                    $.each(Global.Data.ManipulationList, function (i, item) {
                                        if (item.Code.trim() == code || (item.Name != '' && item.Name.trim().toUpperCase() == code)) {
                                            $.each(Global.Data.PhaseManiVerDetailArray, function (ii, mani) {
                                                if (mani.OrderIndex == data.record.OrderIndex) {
                                                    mani.ManipulationId = item.Id;
                                                    mani.ManipulationCode = item.Code;
                                                    mani.ManipulationName = item.Name;
                                                    mani.TMUManipulation = item.isUseMachine ? 0 : item.StandardTMU;
                                                    mani.TMUEquipment = item.isUseMachine ? item.StandardTMU : 0;
                                                    mani.TotalTMU = Math.round((mani.Loop * item.StandardTMU) * 1000) / 1000;
                                                    flag = true;
                                                    return false;
                                                }
                                            });
                                            return false;
                                        }
                                    });
                                    if (!flag) {
                                        GlobalCommon.ShowMessageDialog('Không tìm thấy thông tin của Thao Tác này trong Thư Viện.\nVui lòng kiểm tra lại Thư Viện thao Tác.', function () { }, "Không Tìm Thấy Thao Tác");
                                    }
                                }
                                if (flag) {
                                    if (Global.Data.PhaseManiVerDetailArray.length == data.record.OrderIndex)
                                        AddEmptyObject();
                                    ReloadListMani_Arr();
                                    UpdateIntWaste();
                                    if (Global.Data.PhaseManiVerDetailArray.length - 1 == data.record.OrderIndex) {
                                        $('[code_' + Global.Data.PhaseManiVerDetailArray.length + ']').focus();
                                        $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').height());
                                    }
                                    else
                                        $('#Create-ManipulationVersion-Popup .modal-body').scrollTop($('#Create-ManipulationVersion-Popup .modal-body').scrollTop());
                                }
                            }
                        });
                        txt.keypress(function (e) {
                            var charCode = (e.which) ? e.which : event.keyCode;
                            if (charCode == 13) {
                                txt.change();
                            }
                        });
                        //txt.click(function () { txt.select(); })
                        return txt;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "35%",
                    display: function (data) {
                        var txt = $('<input class="form-control" des type="text" value="' + data.record.ManipulationName + '" />');
                        txt.change(function () {
                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].ManipulationName = txt.val();
                        });
                        txt.keypress(function (e) {
                            var charCode = (e.which) ? e.which : event.keyCode;
                            if (charCode == 13) {
                                txt.change();
                            }
                        });
                        // txt.click(function () { txt.select(); })
                        return txt;
                    }
                },
                Loop: {
                    title: "Tần Suất",
                    width: "5%",
                    display: function (data) {
                        var txt = $('<input class="form-control text-center" loop type="text" value="' + data.record.Loop + '"  onkeypress="return isNumberKey(event)"/>');
                        txt.change(function () {
                            var loop = parseFloat(txt.val());
                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].Loop = txt.val();
                            Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TotalTMU = Math.round((Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TMUEquipment * loop + Global.Data.PhaseManiVerDetailArray[data.record.OrderIndex - 1].TMUManipulation * loop) * 1000) / 1000;
                            ReloadListMani_Arr();
                            UpdateIntWaste();
                        });
                        return txt;
                    }
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
                Delete: {
                    title: '',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<i title="Xóa" class="fa fa-trash-o"></i>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                var oldIndex = data.record.OrderIndex - 1;
                                Global.Data.PhaseManiVerDetailArray.splice(oldIndex, 1);
                                for (var i = oldIndex; i < Global.Data.PhaseManiVerDetailArray.length; i++) {
                                    Global.Data.PhaseManiVerDetailArray[i].OrderIndex = i + 1;
                                }
                                ReloadListMani_Arr();
                                UpdateIntWaste();
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;
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

    function AddEmptyObject() {
        var obj = {
            Id: 0,
            CA_PhaseId: 0,
            OrderIndex: Global.Data.PhaseManiVerDetailArray.length + 1,
            ManipulationId: 0,
            ManipulationCode: '',
            EquipmentId: 0,
            TMUEquipment: 0,
            TMUManipulation: 0,
            Loop: 1,
            TotalTMU: 0,
            ManipulationName: ''
        }
        Global.Data.PhaseManiVerDetailArray.push(obj);
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
        var totalTMUPrepare = parseFloat($('#time-repare-id').attr('tmu'));
        var totalTimePrepare = totalTMUPrepare / Global.Data.TMU;
        var totalTimeManiVerTMU = ($('[totaltime]').val() != "" ? parseFloat($('[totaltime]').val()) : 0) + ($('[totaltimewaste]').val() != '' ? parseFloat($('[totaltimewaste]').val()) : 0);
        Global.Data.PhaseModel.TotalTMU = totalTimePrepare + totalTimeManiVerTMU;
        $('#TotalTMU').html(Math.round(Global.Data.PhaseModel.TotalTMU * 1000) / 1000);
        $('#lbTenCongDoan').empty().append(`${$('#phase-name').val()} <i class="fa fa-share-square-o red"></i> TG CĐ: <span class="red">${$('#TotalTMU').html()}</span> (s)`);
    }

    function GetManipulationEquipmentInfoByCode(code, callback) {
        var isGetTMU = false;
        var codeType = "";
        if (code.indexOf("C") > -1) {
            codeType = code.substring(0, 1);
            if (codeType == "C")
                isGetTMU = true;
        }
        if (isGetTMU == false && code.indexOf("SE") > -1) {
            codeType = code.substring(0, 2);
            if (codeType == "SE")
                isGetTMU = true;
        }
        if (isGetTMU == true) {
            if ($('#equipmentId').val() == "" || $('#equiptypedefaultId').val() == "") {
                isGetTMU = false;
                GlobalCommon.ShowMessageDialog("Bạn chưa chọn thiết bị. Vui lòng chọn thiết bị trước", function () { }, "Lỗi thao tác.");
            }
            else {
                if (code.substring(0, 1) == "C") {
                    if ($('#equiptypedefaultId').val() != Global.Data.EquipTypeDefaultId.C) {
                        isGetTMU = false;
                        GlobalCommon.ShowMessageDialog("Thiết bị bạn chọn không phải là thiết bị cắt. Vui lòng kiểm tra lại", function () { }, "Lỗi thao tác.");
                    }
                    else {
                        if ($('#ApplyPressure').val() == 0) {
                            isGetTMU = false;
                            GlobalCommon.ShowMessageDialog("Bạn chưa chọn số lớp cắt. Vui lòng kiểm tra lại", function () { }, "Lỗi thao tác.");
                        }
                    }
                }
                else if (code.substring(0, 2) == "SE") {
                    if ($('#equiptypedefaultId').val() != Global.Data.EquipTypeDefaultId.SE) {
                        isGetTMU = false;
                        GlobalCommon.ShowMessageDialog("Thiết bị bạn chọn không phải là thiết bị may. Vui lòng kiểm tra lại", function () { }, "Lỗi thao tác.");
                    }
                }
            }

        }
        if (isGetTMU == true) {
            var equipmentId = $('#equipmentId').val();
            var equiptypedefaultId = $('#equiptypedefaultId').val();
            var applyPressure = $('#ApplyPressure option:selected').attr('val');
            $.ajax({
                url: Global.UrlAction.GetManipulationEquipmentInfoByCode,
                type: 'POST',
                data: JSON.stringify({ 'equipmentId': equipmentId, 'equiptypedefaultId': equiptypedefaultId, 'applyPressure': applyPressure, 'code': code }),
                contentType: 'application/json charset=utf-8',
                beforeSend: function () { $('#loading').show(); },
                success: function (data) {
                    $('#loading').hide();
                    GlobalCommon.CallbackProcess(data, function () {
                        if (data.Result == "OK") {
                            if (callback)
                                callback(data.Data);
                        } else
                            GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                    }, false, Global.Element.PopupModule, true, true, function () {
                        var msg = GlobalCommon.GetErrorMessage(data);
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                    });
                }
            });
        }
    }

    function TinhLaiCodeMayCat() {
        var equipmentId = $('#equipmentId').val();
        var equiptypedefaultId = $('#equiptypedefaultId').val();
        var applyPressure = $('#ApplyPressure option:selected').attr('val');
        var actions = Global.Data.PhaseManiVerDetailArray;
        actions = actions.splice(0, actions.length - 1);

        $.ajax({
            url: Global.UrlAction.TinhLaiCode,
            type: 'POST',
            data: JSON.stringify({ 'actions': actions, 'equipmentId': equipmentId, 'equiptypedefaultId': equiptypedefaultId, 'applyPressure': applyPressure }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                Global.Data.PhaseManiVerDetailArray.length = 0;
                $.each(data.Records, function (i, item) {
                    Global.Data.PhaseManiVerDetailArray.push(item);
                });
                AddEmptyObject();
                //split_Arr();
                ReloadListMani_Arr();
                UpdateIntWaste();
            }
        });
    }

    //#endregion
}
