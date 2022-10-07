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
GPRO.namespace('Phantich4');
GPRO.Phantich4 = function () {
    var Global = {
        UrlAction: {
            GetProducts: '/Product/GetSelectList',
            FindCommo: '/phantich/GetSampleProAnaByProductId ',
            GetListPhase: '/ProAna/GetPhases',
            GetPhaseById: '/ProAna/GetPhaseById',
            GetLastIndex: '/ProAna/GetPhaseLastIndex',
            SavePhase: '/ProAna/SavePhase',
            DeletePhase: '/ProAna/DeletePhase',
            RemovePhaseVideo: '/ProAna/RemovePhaseVideo',
            CopyPhase: '/ProAna/CopyPhase',

            GetTech: '/ProAna/GetTech',
            SaveTech: '/ProAna/SaveTech',
            ExportToExcel_QTCN: '/ProAna/ExportToExcel',

            TinhLaiCode: '/ProAna/TinhLaiCode',




            Export_CommoAnaPhaseGroup: '/ProAna/Export_CommoAnaPhaseGroup',
            Copy_CommoAnaPhaseGroup: '/ProAna/Copy_CommoAnaPhaseGroup',


            CreateLaborDivisionVersion: '/LaborDivision/Create/' + $('#order').attr('option') + '/' + $('#order').val() + '/' + $('#order').attr('line') + '/' + $('#order').attr('cus') + '/' + $('#order').attr('commo'),
            ActiveVersion: '/LaborDivision/ActiveLaborDivisionVersion',
            DrawImage: '/LaborDivision/Draw/',
            GetListTechProcessVerDetail: '/TechnologyProcess/GetTechProcessVersionDetail',
            RefreshTKCById: '/ProAna/RefreshTKCById',



            GetListEquipment: '/Equipment/Gets',
            GetListTimePrepare: '/TimePrepare/GetLists',
            GetAllManipulation: '/MType/GetAllManipulation',
            GetPhasesForSugest: '/ProAna/GetPhasesForSuggest',
            GetManipulationEquipmentInfoByCode: '/MType/GetManipulationEquipmentInfoByCode',

            //TKC
            Save_TKC: '/ProAna/SaveTKC',
            GetById_TKC: '/ProAna/GetTKCById',
            RefreshTKCById: '/ProAna/RefreshTKCById',
            Gets_TKC: '/ProAna/Gets_TKC',
            Gets_TKC_Ver: '/ProAna/Gets_TKC_Ver',
            Delete_TKC: '/ProAna/DeleteTKC',
            ExportExcel_TKC: '/ProAna/ExportDiagramToExcel',
        },
        Element: {
            JtablePhaseSample: 'jtable-phase-sample',
            JtablePhase: 'jtable-phase',

            PopupLine: 'popup_Line',
            JtableTech_Cycle: 'jtable-qtcn-sample',
            popupExportQTCN: 'popup-export-qtcn',


            JtableTKC: 'jtable-tkc',
            JtableSampleTKC: 'jtable-tkc-sample',
            PopupTKC: 'popup_tkc',
            Popup_positionTKC: 'tkc_popup_position',
            JtablePhase_ArrTKC: 'Jtable_tkc_Phase',

            CreateCommodityPopup: 'capopup_Commodity',

            CreateWorkShopPopup: 'Create-Workshop-popup',

            CreatePhaseGroupPopup: 'Create-PhaseGroup-Popup',

            CreatePhasePopup: 'Create-Phase-Popup',
            JtableManipulationArr: 'jtable_ManipulationArr',
            //THIET BI
            JtableEquipment: 'jtable-chooseequipment',
            PopupChooseEquipment: 'ChooseEquipment_Popup',
            //TGCB
            jtable_Timeprepare_Chooise: 'jtable-timeprepare',
            jtable_timeprepare_arr: 'jtable-timeprepare-arr',
            timeprepare_Popup: 'timeprepare-Popup',

            //TKC
            Popup_position: 'tkc_popup_position',
            JtablePhase_Arr: 'Jtable_tkc_Phase',
        },
        Data: {
            Products: [],
            Node: '',
            ParentID: 0,
            TechCycle_Arr: [],
            warningChuaCoQTCN: false,
            TimeProductPerCommodity: 0,
            SelectedProductId: 0,
            CommoAnaItems: [],
            CurrentObj: {
                Id: 0,
                Node: '',
                ParentId: 0,
                ObjectType: 1
            },
            SampleObj: {
                Node: '',
                ParentId: 0,
                Copy_CommoAnaPhaseGroupId: 0,
                tempCopy_CommoAnaPhaseGroupId: 0,
            },
            techExportUrl: '',
            AfterSave: false,
            isScrollBottom: false,
            TechProcessVersion: {},

            EquipTypeDefaultId: { SE: 1, C: 2 },
            PhaseManiVerDetailArray: [],
            TimePrepareArray: [],
            PhaseModel: {},
            PhasesSuggest: [],
            SuggestPhaseId: 0,
            ManipulationList: [],
            PhaseAutoCode: '',
            phaseLastIndex: 1,
            IntGetTMUType: 0,
            TMU: 0,
            Commo_Ana_PhaseId: 0,
            ManipulationVersionModel: {},
            isInsertPhase: true,
            Video: '',
            SelectedWorkshopId: 0,

            // TKC
            LineEmployeeArr: [],
            TechProcessVerDetailArray: [],
            Position_Arr: [],
            EmployeeArr: [],
            Phase_Arr: [],
            PhaseList: [],
            TechProVerId: 0,
            CodeOption: '',
            NameOption: '',
            Code: [],
            Name: [],
            BasePhases: [],
            Index: 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    //#region out events
    this.SetNode_ParentIdSample = function (node, parentId, groupName) {
        $('#title-popup').html(`phân tích cụm công đoạn: <span class ="red">${$('#pt-sample-product').val()}</span>`);
        $('#qtcn-action,#jtable-tkc,#current-phase-group-action').hide();
        $('#phase-group-action').show();
        SetNode_ParentIdSample(node, parentId, groupName);
        Global.Data.SampleObj.tempCopy_CommoAnaPhaseGroupId = parentId;
    }

    this.ReloadQTCN_Sample = function (node, parentId) {
        $('#title-popup').html(`quy trình công nghệ: <span class ="red">${$('#pt-sample-product').val()}</span>`);
        $('#jtable-phase-sample,#jtable-tkc,#phase-group-action,[techsave] ,#current-phase-group-action,#jtable-phase').hide();
        $('#techprocess,#qtcn-action').show();
        Global.Data.AfterSave = true;
        Global.Data.SampleObj.Node = node;
        Global.Data.SampleObj.ParentId = parentId;
        GetTechProcess(node, parentId);
    }

    this.ReloadTKC_Sample = function (node, parentId) {
        $('#title-popup').html(`thiết kế chuyền: <span class ="red">${$('#pt-sample-product').val()}</span>`)
        Global.Data.SampleObj.Node = node;
        Global.Data.SampleObj.ParentId = parentId;
        $('#jtable-phase-sample,#techprocess,#qtcn-action,#phase-group-action,#jtable-phase,#jtable-tkc').hide();
        $('#jtable-tkc-sample').show();
        ReloadList_TKCSample();
    }

    this.ResetTime_Percent = function (index) {
        ResetTime_Percent(index);
    }

    this.removeVideo = function () { removeVideo(); }

    this.bindData = function (Id, ObjectType, wkId) {
        BindData(Id, ObjectType, wkId);
    }

    this.addWorkshop = function (node, parentId) {
        Global.Data.CurrentObj.Id = 0;
        Global.Data.CurrentObj.ObjectType = 2;
        Global.Data.CurrentObj.Node = node;
        Global.Data.CurrentObj.ParentId = parentId;
        $('#' + Global.Element.CreateWorkShopPopup).modal('show');
    }

    this.delete = function (_id, oType) {
        GlobalCommon.ShowConfirmDialog('Dữ liệu đã xóa sẽ không thể phục hồi. Bạn có chắc chắn muốn xóa?', function () {
            Delete(_id, oType);
        }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
    }

    this.addNew = function (node, parentId, oType) {
        Global.Data.CurrentObj.Id = 0;
        Global.Data.CurrentObj.ObjectType = oType;
        Global.Data.CurrentObj.Node = node;
        Global.Data.CurrentObj.ParentId = parentId;
        if (oType == 2)
            $('#' + Global.Element.CreateWorkShopPopup).modal('show');
        if (oType == 6)
            $('#' + Global.Element.CreatePhaseGroupPopup).modal('show');
    }

    this.pastePhaseGroup = function (_id) {
        Copy_CommoAnaPhaseGroup(_id);
    }

    this.TitleChange = function () {
        var value = $('#tile-parent').val();
        var $selectedRows = $('#' + Global.Element.JtableTech_Cycle).find('tbody tr')
        if ($selectedRows.length > 0) {
            var time_product = 0;
            $selectedRows.each(function (i, item) {
                $(this).find('[percent]').val(value);
                var time = parseFloat(Global.Data.TechCycle_Arr[i].StandardTMU);
                var time_per = Math.round(((time * 100) / parseFloat(value)) * 1000) / 1000;

                $($($(this).find('td'))[6]).html(ParseStringToCurrency(time_per));
                Global.Data.TechCycle_Arr[i].TimeByPercent = time_per;
                time_product += time_per;
            });
            $('#time-product').html(Math.round(time_product * 1000) / 1000);
            Global.Data.TimeProductPerCommodity = time_product;
            ResetWorkingBox(0);
        }
    };

    this.insert_Exit = function (id) {
        insert_Exit(id);
    }

    this.insert_BTP = function (id) {
        insert_BTP(id);
    }

    this.delete_Po = function (id) {
        delete_Po(parseInt(id));
    }

    this.ChooseEmployee = function (id) {
        ChooseEmployee(id);
    }
    this.ChangeIndex = function (id) {
        ChangePositionIndex(id);
    }
    //#endregion

    this.Init = function () {
        RegisterEvent();

        InitPhaseGroupEvents();
        InitPhaseEvents();
        InitQTCNEvents();
        InitPopupExportQTCN();

        GetProducts();
        GetAllManipulation();
        GetPhasesForSuggest();

        InitListPhase_Sample();
        InitListPhase_View();
        InitListEquipment();
        InitListMani_Arr();
        AddEmptyObject();
        ReloadListMani_Arr();
        UpdateIntWaste();
        InitListTimePrepare_chooise();
        InitListPhase_Arr();

        InitPopupCreateProduct();
        InitPopupCreateWorkshop();
        InitPopupCreatePhaseGroup();
        InitPopupCreatePhase();
        InitPopupEquipment();
        InitPopupTimeRepare();
        InitPopupCreateTKC();

        InitListTech_Cycle();
        InitList_TKC();
        InitList_TKCSample();

        if ($('#config').attr('tmu') != "");
        Global.Data.TMU = parseFloat($('#config').attr('tmu'));
        if ($('#config').attr('gettmutype') != "")
            Global.Data.IntGetTMUType = parseInt($('#config').attr('gettmutype'));

        $('[re_caproduct],[re_phasegroup-name],[re_wk-name],[re_workerslevel],[re-apply-pressure]').click();

        $('#jtable-phase-sample,#techprocess,#qtcn-action,#phase-group-action,#jtable-tkc,#jtable-tkc-sample,#current-phase-group-action,#jtable-phase').hide();
    }


    var RegisterEvent = function () {
        $('#enable-sample-product').click(() => {
            $('#pt-sample-product').prop('disabled', false);
            $('#pt-sample-product').val('').focus();
        });

        $('#pt-sample-product').change(function () {
            $('#tree-sample').empty();
            var _filter = Global.Data.Products.filter(x => x.Name == $(this).val())[0];
            if (_filter) {
                FindCommo(_filter.Value, true);
            }
            else
                GlobalCommon.ShowMessageDialog(`Không tìm thấy mã hàng có tên :<span class"red bold">'${$(this).val()}' trong danh mục mã hàng. Vui lòng kiểm tra lại.!`, function () { }, "Lỗi nhập liệu");
        });


        $('#enable-pt-product').click(() => {
            $('#pt-product').prop('disabled', false);
            $('#pt-product').val('').focus();
        });

        $('#pt-product').change(function () {
            $('#tree-pt').empty();
            Global.Data.CommoAnaItems.length = 0;
            var _filter = Global.Data.Products.filter(x => x.Name == $(this).val())[0];
            if (_filter) {
                Global.Data.SelectedProductId = _filter.Value;
                FindCommo(_filter.Value, false);
            }
            else
                GlobalCommon.ShowMessageDialog(`Không tìm thấy mã hàng có tên :<span class"red bold">'${$(this).val()}' trong danh mục mã hàng. Vui lòng kiểm tra lại.!`, function () { $('#tree-pt').html('Không có dữ liệu ....') }, "Lỗi nhập liệu");
        });



    }

    function InitPhaseGroupEvents() {
        $('#phase-group-copy').click(() => {
            Global.Data.SampleObj.Copy_CommoAnaPhaseGroupId = Global.Data.SampleObj.tempCopy_CommoAnaPhaseGroupId;
            $('.fa-paste').show();
        })

        $('#phase-group-export').click(() => {
            window.location.href = Global.UrlAction.Export_CommoAnaPhaseGroup + "?id=" + Global.Data.SampleObj.tempCopy_CommoAnaPhaseGroupId;
        });

        $('#pt-phase-group-export').click(() => {
            window.location.href = Global.UrlAction.Export_CommoAnaPhaseGroup + "?id=" + Global.Data.CurrentObj.Id;
        });

        $('#pt-phase-group-info').click(() => {
            $('#phaseGroup-name').val(Global.Data.CurrentObj.ObjectId);
            $('#phaseGroup-name').prop('disabled', true);
            $('#phaseGroup-Description').val(Global.Data.CurrentObj.Note);
            $('#' + Global.Element.CreatePhaseGroupPopup).modal('show');
        });

        $('#pt-phase-group-delete').click(() => {
            GlobalCommon.ShowConfirmDialog('Dữ liệu đã xóa sẽ không thể phục hồi. Bạn có chắc chắn muốn xóa?', function () {
                Delete(Global.Data.CurrentObj.Id, 6);
            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
        });
    }

    function InitQTCNEvents() {
        $('.but_new_box').click(function () {
            if ($('.new_box').css('display') == 'none') {
                $('.new_box').delay(10).fadeIn();
                $('.but_new_box i').removeClass('fa-angle-double-right').addClass('fa-angle-double-left');
            }
            else {
                $('.new_box').hide();
                $('.but_new_box i').removeClass('fa-angle-double-left').addClass('fa-angle-double-right');
            }
        });

        $('.tech-info-box-but').click(function () {
            if ($('.tech-info-box-main').css('display') == 'none') {
                $('.tech-info-box-main').delay(10).fadeIn();
                $('.tech-info-box-but i').removeClass('fa-angle-double-right').addClass('fa-angle-double-left');
            }
            else {
                $('.tech-info-box-main').hide();
                $('.tech-info-box-but i').removeClass('fa-angle-double-left').addClass('fa-angle-double-right');
            }
        });

        $('[techexport]').click(function () {
            $('#' + Global.Element.popupExportQTCN).show();
        });

        $('[techsave]').click(function () {
            GetData();
            SaveTechVersion();
        });

        $('#tile-parent').change(function () {
            value = $(this).val();
            var $selectedRows = $('#' + Global.Element.JtableTech_Cycle).find('tbody tr')
            if ($selectedRows.length > 0) {
                time_product = 0;
                $selectedRows.each(function (i, item) {
                    $(this).find('[percent]').val(value);
                    time = parseFloat(Global.Data.TechCycle_Arr[i].StandardTMU);
                    time_per = Math.round(((time * 100) / parseFloat(value)) * 1000) / 1000;

                    $($($(this).find('td'))[6]).html(ParseStringToCurrency(time_per));
                    Global.Data.TechCycle_Arr[i].TimeByPercent = time_per;
                    time_product += time_per;

                });
                $('#time-product').html(Math.round(time_product * 1000) / 1000);
                Global.Data.TimeProductPerCommodity = time_product;
                ResetWorkingBox(0);
            }
        });

        $('#total-worker').change(function () {
            ResetWorkingBox(0);
        });

        $('#work-time').change(function () {
            ResetWorkingBox(0);

        });

        $('#percent,#pricePerSecond').change(function () {
            ResetThanhTien()
        });
    }

    function InitPhaseEvents() {

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
            if (Check_Phase_Validate()) {
                if ($('#video').val() != '')
                    UploadVideo();
                else
                    SavePhase();
            }
        });

        $('[cancel-create-phase]').click(function () {
            Global.Data.isInsertPhase = true;
            Global.Data.TimePrepareArray.length = 0;
            //ReloadListTimePrepare();
            Global.Data.PhaseManiVerDetailArray.length = 0;
            AddEmptyObject();
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
            $('[save-phase]').show();
            // $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + (Global.Data.phaseLastIndex + 1)));

        });

        $('#btn-browse-file').click(function () {
            $('#video').click();
        })
    }

    //#region 
    function GetProducts() {
        $.ajax({
            url: Global.UrlAction.GetProducts,
            type: 'POST',
            data: '',
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        Global.Data.Products.length = 0;
                        var option = '';
                        if (data.Data != null && data.Data.length > 0) {
                            $.each(data.Data, function (i, item) {
                                if (item.Value != 0) {
                                    Global.Data.Products.push(item);
                                    option += '<option value="' + item.Name + '" /> ';
                                }
                            });
                        }
                        $('#datalist-product').empty().append(option);
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function FindCommo(_productId, isSample) {
        if (isSample)
            $('#tree-sample').empty();
        else {
            $('#tree-pt').empty();
            Global.Data.CommoAnaItems.length = 0;
        }

        $.ajax({
            url: Global.UrlAction.FindCommo,
            type: 'POST',
            data: JSON.stringify({ 'productId': _productId }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (res) {
                $('#loading').hide();
                if (res.Result == "OK") {
                    if (isSample) {
                        DrawSampleCommo(JSON.parse(res.Data));
                        $('#pt-sample-product').prop('disabled', true);
                    }
                    else {
                        DrawCommoAna(JSON.parse(res.Data));
                        $('#pt-product').prop('disabled', true);
                    }
                }
                else
                    GlobalCommon.ShowMessageDialog(`Mã hàng chưa có bài phân tích. Vui lòng kiểm tra lại.!`, function () {
                        if (!isSample)
                            $('#tree-pt').html('Không có dữ liệu ....')
                    }, "Thông báo");
            }
        });
    }

    function DrawSampleCommo(obj) {
        var root = $('#tree-sample');
        root.append(`<div class="div-lv1"> <i class="fa fa-shirtsinbulk"></i>${obj.Name}   </div>`);
        if (obj.Details && obj.Details.length > 0) {
            $.each(obj.Details, (key, _item) => {
                root.append(`<div class="div-lv2"> <i class="fa fa-industry"></i>${_item.Name}   </div>`);
                if (_item.Details && _item.Details.length > 0) {
                    $.each(_item.Details, (key, item1) => {
                        if (item1.ObjectType == 3)
                            root.append(`<div class="div-lv3" onclick="ReloadQTCN_Sample('${item1.Node}',${item1.Id})"><i class="fa fa-diamond"></i>${item1.Name}</div>`);
                        if (item1.ObjectType == 8)
                            root.append(`<div class="div-lv3" onclick="ReloadTKC_Sample('${item1.Node}',${item1.ParentId})"><i class="fa fa-area-chart"></i>${item1.Name}</div>`);
                        if (item1.ObjectType == 5)
                            root.append(`<div class="div-lv3"> <i class="fa fa-object-group"></i>Cụm công đoạn  </div>`);
                        if (item1.Details && item1.Details.length > 0) {
                            $.each(item1.Details, (key, item2) => {
                                root.append(` <div class="div-lv4" onclick="SetNode_ParentIdSample('${item2.Node}',${item2.Id},'${item2.Name}')"><i class="fa fa-caret-right"></i>${item2.Name}</div>`);
                            });
                        }
                    });
                }
            });
        }
    }

    function SetNode_ParentIdSample(node, parentId, groupName) {
        Global.Data.SampleObj.Node = node;
        Global.Data.SampleObj.ParentId = parentId;
        $('#' + Global.Element.JtablePhaseSample).find('.jtable-title-text').html('Danh Sách Công Đoạn của : <span class="red"> ' + groupName + '</span>');
        $('#' + Global.Element.JtablePhaseSample).show();
        $('#techprocess,#jtable-phase,#jtable-tkc-sample').hide();
        ReloadListPhase_Sample();
    }

    function InitListPhase_Sample() {
        $('#' + Global.Element.JtablePhaseSample).jtable({
            title: 'Danh sách Công Đoạn',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.GetListPhase,
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
                    title: "T.Gian (s)",
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
                actions: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        let div = $('<div class="table-action"></div>');
                        if (data.record.actions.length > 0) {
                            var excel = $('<i title="Xuất Danh Sách Thao Tác của Công đoạn" class="fa fa-file-excel-o"></i>');
                            excel.click(function () {
                                window.location.href = '/ProAna/export_PhaseManiVersion?Id=' + data.record.Id;
                            });
                            div.append(excel);
                        }
                        var edit = $('<i data-toggle="modal" data-target="#' + Global.Element.CreatePhasePopup + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o "  ></i>');
                        edit.click(function () {
                            $('[save-phase]').hide();
                            data.record.TotalTMU = Math.round(data.record.TotalTMU * 1000) / 1000;
                            $('#workersLevel').val(data.record.WorkerLevelId);
                            $('#TotalTMU').html(data.record.TotalTMU);
                            $('#phase-Des').val(data.record.Description);
                            $('#phaseID').val(data.record.Id);
                            $('#phase-name').val(data.record.Name).change();
                            $('#phase-code').html(data.record.Code);
                            $('#phase-index').val(data.record.Index);

                            if (data.record.timePrepares.length > 0) {
                                $.each(data.record.timePrepares, function (i, item) {
                                    Global.Data.TimePrepareArray.push(item);
                                });
                                // ReloadListTimePrepare();  
                                $('#time-repare-name').val(`${Global.Data.TimePrepareArray[0].Name} - TMU: ${Global.Data.TimePrepareArray[0].TMUNumber}`);
                            }
                            if (Global.Data.TimePrepareArray && Global.Data.TimePrepareArray.length > 0)
                               
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
                            //  AddEmptyObject();
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
                        div.append(edit);
                        return div;
                    }
                }
            },
        });
    }

    function ReloadListPhase_Sample() {
        $('#' + Global.Element.JtablePhaseSample).jtable('load', { 'node': (Global.Data.SampleObj.Node + Global.Data.SampleObj.ParentId) });
    }



    function DrawCommoAna(obj) {
        Global.Data.CommoAnaItems.push(obj);

        var root = $('#tree-pt');
        root.append(`<div class="div-lv1">
                        <div onclick="BindData(${obj.Id},${obj.ObjectType})"><i class="fa fa-shirtsinbulk"></i>${obj.Name}</div>
                        <i onclick="AddNew('${obj.Node}',${obj.Id},2)" class="fa fa-plus-circle" title="Thêm phân xưởng"></i>
                    </div>`);

        if (obj.Details && obj.Details.length > 0) {

            $.each(obj.Details, (key, _item) => {
                Global.Data.CommoAnaItems.push(_item);
                root.append(`<div class="div-lv2">
                                <div onclick="BindData(${_item.Id},${_item.ObjectType})"><i class="fa fa-industry"></i>${_item.Name}</div>
                                <i onclick="Delete(${_item.Id},2)" class="fa fa-trash-o" title="Xóa phân xưởng"></i>
                            </div>`);

                if (_item.Details && _item.Details.length > 0) {
                    root.append(``)
                    $.each(_item.Details, (key, item1) => {
                        Global.Data.CommoAnaItems.push(item1);
                        if (item1.ObjectType == 3)
                            root.append(`<div class="div-lv3" onclick="BindData(${item1.Id},${item1.ObjectType})"><i class="fa fa-diamond"></i>${item1.Name}</div>`);
                        if (item1.ObjectType == 8)
                            root.append(`<div class="div-lv3" onclick="BindData(${item1.Id},${item1.ObjectType},${_item.ObjectId})"><i class="fa fa-area-chart"></i>${item1.Name}</div>`);
                        if (item1.ObjectType == 5)
                            root.append(`<div class="div-lv3">
                                            <div >
                                                <i class="fa fa-object-group"></i>Cụm công đoạn
                                            </div>
                                            <i onclick="PastePhaseGroup( ${item1.Id} )" class="fa fa-paste" title="Dán cụm công đoạn"></i>
                                            <i onclick="AddNew('${item1.Node}',${item1.Id},6)" class="fa fa-plus-circle" title="Thêm cụm công đoạn"></i>
                                        </div>`);
                        if (item1.Details && item1.Details.length > 0) {
                            $.each(item1.Details, (key, item2) => {
                                Global.Data.CommoAnaItems.push(item2);
                                root.append(` <div class="div-lv4" onclick="BindData('${item2.Id}',${item2.ObjectType})"><i class="fa fa-caret-right"></i>${item2.Name}</div>`);
                            });
                        }
                    });
                }
            });
        }

        if (Global.Data.isScrollBottom) {
            $('#tree-pt').animate({ scrollTop: $('#tree-pt').height() + $('#tree-pt').height() });
            Global.Data.isScrollBottom = false;
        }

        if (Global.Data.SampleObj.Copy_CommoAnaPhaseGroupId == 0) {
            $('.fa-paste').hide();
        }
        else {
            $('.fa-paste').show();
        }
    }

    function InitListPhase_View() {
        $('#' + Global.Element.JtablePhase).jtable({
            title: 'Danh sách Công Đoạn',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.GetListPhase,
                createAction: Global.Element.CreatePhasePopup,
            },
            messages: {
                addNewRecord: 'Thêm mới Công Đoạn',
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                flag: {
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
                action: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var text = $('<i   title="Copy công đoạn" class="fa fa-files-o clickable blue"  ></i>');
                        text.click(function () {
                            CopyPhase(data.record.Id);
                        });
                        return text;
                    }
                },
                Excel: {
                    title: '',
                    width: '2%',
                    sorting: false,
                    display: function (data) {
                        if (data.record.actions.length > 0) {
                            var txt = $('<i title="Xuất Danh Sách Thao Tác của Công đoạn" class="fa fa-file-excel-o"></i>');
                            txt.click(function () {
                                window.location.href = '/ProAna/export_PhaseManiVersion?Id=' + data.record.Id;
                            });
                            return txt;
                        }
                    }
                },
                edit: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var text = $('<i data-toggle="modal" data-target="#' + Global.Element.CreatePhasePopup + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {
                            data.record.TotalTMU = Math.round(data.record.TotalTMU * 1000) / 1000;
                            $('#workersLevel').val(data.record.WorkerLevelId);
                            $('#TotalTMU').html(data.record.TotalTMU);
                            $('#phase-Des').val(data.record.Description);
                            $('#phaseID').val(data.record.Id);
                            $('#phase-name').val(data.record.Name).change();
                            $('#phase-code').html(data.record.Code);
                            $('#phase-index').val(data.record.Index);

                            if (data.record.timePrepares.length > 0) {
                                $.each(data.record.timePrepares, function (i, item) {
                                    Global.Data.TimePrepareArray.push(item);
                                });
                                // ReloadListTimePrepare();
                                $('#time-repare-name').val(`${Global.Data.TimePrepareArray[0].Name} - TMU: ${Global.Data.TimePrepareArray[0].TMUNumber}`);
                            }
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
                        });
                        return text;
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
                                DeletePhase(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;
                    }
                }
            },
        });
    }

    function ReloadListPhase_View() {
        $('#' + Global.Element.JtablePhase).jtable('load', { 'node': (Global.Data.CurrentObj.Node + Global.Data.CurrentObj.Id) });
    }

    //#endregion

    //#region
    function BindData(Id, oType, wkId) {
        var _filter = Global.Data.CommoAnaItems.filter(x => x.Id == Id && x.ObjectType == oType)[0];
        if (_filter) {
            Global.Data.CurrentObj = _filter;
            $('#jtable-phase-sample,#techprocess,#qtcn-action,#phase-group-action,#jtable-tkc,#jtable-tkc-sample,#current-phase-group-action,#jtable-phase').hide();
            // product
            if (oType == 1) {
                $('#caproduct').val(_filter.ObjectId);
                $('#Description').val(_filter.Note);
                $('#caproduct').prop('disabled', true);
                $('#' + Global.Element.CreateCommodityPopup).modal('show');
            }

            // workshop
            if (oType == 2) {
                $('#wk-name').val(_filter.ObjectId);
                $('#wk-name').prop('disabled', true);
                $('#workshop-Description').val(_filter.Note);
                $('#' + Global.Element.CreateWorkShopPopup).modal('show');
            }

            //QTCN
            if (oType == 3) {
                $('#title-popup').html(`quy trình công nghệ: <span class ="red">${$('#pt-product').val()}</span>`);
                $('#techprocess,#qtcn-action,[techsave]').show();
                Global.Data.AfterSave = false;
                Global.Data.SampleObj.Node = _filter.Node;
                Global.Data.SampleObj.ParentId = _filter.Id;
                GetTechProcess(_filter.Node, _filter.Id);
            }

            //phase gruop
            if (oType == 6) {
                if (Global.Data.SampleObj.Copy_CommoAnaPhaseGroupId != 0)
                    $('#pt-phase-group-paste').show();
                else {
                    $('#pt-phase-group-paste').hide();
                }

                $('#current-phase-group-action').show();
                $('#title-popup').html(`phân tích cụm công đoạn : <span class ="red">${$('#pt-product').val()}</span>`);
                $('#' + Global.Element.JtablePhase).find('.jtable-title-text').html('Danh Sách Công Đoạn của : <span class="red"> ' + _filter.Name + '</span>');
                $('#' + Global.Element.JtablePhase).show();

                ReloadListPhase_View();
                GetLastPhaseIndex();

                Global.Data.PhaseAutoCode = _filter.Note;
                Global.Data.phaseLastIndex = 0;
            }

            if (oType == 8) {
                Global.Data.SelectedWorkshopId = wkId;
                $('#title-popup').html(`thiết kế chuyền : <span class ="red">${$('#pt-product').val()}</span>`);
                $('#jtable-tkc').show();
                ReloadList_TKC();
                $('[re_line_tkc]').click();


                $('#jtable-tkc').attr('parentid', _filter.Id);
                $('#jtable-tkc').attr('node', _filter.Node);
                $('#jtable-tkc').attr('pId', _filter.ParentId);

                let _found = Global.Data.CommoAnaItems.filter(x => x.ObjectId == wkId && x.ObjectType == 2)[0];
                if (_found) {
                    $('#jtable-tkc').attr('wkId', _found.ObjectId);
                    $('#jtable-tkc').attr('wkName', _found.Name);
                    $('#workshop_').html(_found.Name);
                }

                $('#jtable-tkc').change();
                // Global.Data.warningChuaCoQTCN = true;
                // var _parentId = parseInt(Global.Data.ParentID) - 1;
                //GetTechProcess(findObj.Node, _parentId);
            }
        }

    }

    function InitPopupCreateProduct() {
        $("#" + Global.Element.CreateCommodityPopup).modal({
            keyboard: false,
            show: false
        });

        $('#' + Global.Element.CreateCommodityPopup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.CreateCommodityPopup.toUpperCase());
        });

        $('[re_caproduct]').click(function () {
            GetProductSelect('caproduct');
        });

        $('[save-commo]').click(function () {
            if ($('#caproduct option:selected').val() == '0') {
                GlobalCommon.ShowMessageDialog('Bạn chưa chọn Mã hàng.', function () { }, "Lỗi nhập liệu");
            }
            else {
                Global.Data.SelectedProductId = parseInt($('#caproduct').val());
                var _newObj = {
                    Id: Global.Data.CurrentObj.Id,
                    Name: $('#caproduct option:selected').text(),
                    ObjectType: Global.Data.CurrentObj.ObjectType,
                    ObjectId: $('#caproduct').val(),
                    ParentId: Global.Data.CurrentObj.ParentId,
                    Node: Global.Data.CurrentObj.Node,
                    Description: $('#Description').val()
                };
                SaveCommoAnalysis(_newObj);
                $('[cancel-commo]').click();
            }
        });

        $('[cancel-commo]').click(function () {
            Global.Data.CurrentObj = {
                Id: 0,
                Node: '',
                ParentId: 0,
                ObjectType: 1
            };
            $('#caproduct').val(0);
            $('#Description').val('');
            $('#caproduct').prop('disabled', false);
        });

        $('#add-new-product').click(() => {
            $('#' + Global.Element.CreateCommodityPopup).modal('show');
        })
    }

    function InitPopupCreateWorkshop() {
        $("#" + Global.Element.CreateWorkShopPopup).modal({
            keyboard: false,
            show: false
        });

        $('#' + Global.Element.CreateWorkShopPopup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.CreateWorkShopPopup.toUpperCase());
        });

        $('[re_wk-name]').click(function () {
            GetWorkshopSelect('wk-name');
        });

        $('[save-workshop]').click(function () {
            if ($('#wk-name option:selected').val() == '' || $('#wk-name option:selected').val() == '0') {
                GlobalCommon.ShowMessageDialog('Bạn chưa chọn Phân xưởng.', function () { }, "Lỗi nhập liệu");
            }
            else {
                var _newObj = {
                    Id: Global.Data.CurrentObj.Id,
                    Name: $('#wk-name option:selected').text(),
                    ObjectType: Global.Data.CurrentObj.ObjectType,
                    ObjectId: $('#wk-name option:selected').val(),
                    ParentId: Global.Data.CurrentObj.ParentId,
                    Node: Global.Data.CurrentObj.Node,
                    Description: $('#workshop-Description').val()
                };
                SaveCommoAnalysis(_newObj);
                $('[cancel-workshop]').click();
            }
        });

        $('[cancel-workshop]').click(function () {
            Global.Data.CurrentObj = {
                Id: 0,
                Node: '',
                ParentId: 0,
                ObjectType: 1
            };
            $('#wk-name').val('');
            $('#workshop-Description').val('');
            $('#wk-name').prop('disabled', false);
        });

    }

    function InitPopupCreatePhaseGroup() {
        $("#" + Global.Element.CreatePhaseGroupPopup).modal({
            keyboard: false,
            show: false
        });

        $('#' + Global.Element.CreatePhaseGroupPopup).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.CreatePhaseGroupPopup.toUpperCase());
        });

        $('[re_phasegroup-name]').click(function () {
            GetPhaseGroupSelect('phaseGroup-name');
        });

        $('[save-phasegroup]').click(function () {
            if ($('#phaseGroup-name option:selected').val() == '' || $('#phaseGroup-name option:selected').val() == '0') {
                GlobalCommon.ShowMessageDialog('Bạn chưa chọn cụm công đoạn.', function () { }, "Lỗi nhập liệu");
            }
            else {
                var _newObj = {
                    Id: Global.Data.CurrentObj.Id,
                    Name: $('#phaseGroup-name option:selected').text(),
                    ObjectType: Global.Data.CurrentObj.ObjectType,
                    ObjectId: $('#phaseGroup-name option:selected').val(),
                    ParentId: Global.Data.CurrentObj.ParentId,
                    Node: Global.Data.CurrentObj.Node,
                    Description: $('#phaseGroup-Description').val()
                };
                SaveCommoAnalysis(_newObj);
            }
        });

        $('[cancel-phasegroup]').click(function () {
            Global.Data.CurrentObj = {
                Id: 0,
                Node: '',
                ParentId: 0,
                ObjectType: 1
            };
            $('#phaseGroup-name').val('');
            $('#phaseGroup-Description').val('');
            $('#phaseGroup-name').prop('disabled', false);
        });
    }

    function Copy_CommoAnaPhaseGroup(_id) {
        $.ajax({
            url: Global.UrlAction.Copy_CommoAnaPhaseGroup,
            type: 'POST',
            data: JSON.stringify({ 'CopyObjectId': Global.Data.SampleObj.Copy_CommoAnaPhaseGroupId, 'ObjectId': _id, 'copyFull': true }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        FindCommo(Global.Data.SelectedProductId, false);
                        Global.Data.SampleObj.Copy_CommoAnaPhaseGroupId = 0;
                        $('.fa-paste').hide();
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupProAna, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    //#endregion

    /********************************************************************************************************************        
                                                       NO NAME
         ********************************************************************************************************************/

    function SaveCommoAnalysis(_obj) {
        Global.Data.lastLeftScroll = $("#div-left-panel").scrollTop();
        console.log('last tree expand : ' + Global.Data.lastLeftScroll)

        var _url = '';
        switch (_obj.ObjectType) {
            case 1: _url = '/ProAna/SaveProduct'; break;
            case 2:
            case 3: _url = '/ProAna/SaveWorkshop'; break;
            case 6:
            case 7: _url = '/ProAna/SavePhaseGroup'; break;
        }
        $.ajax({
            url: _url,
            type: 'post',
            data: ko.toJSON(_obj),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        if (_obj.ObjectType > 1)
                            Global.Data.isScrollBottom = true;
                        FindCommo(Global.Data.SelectedProductId, false);

                        $('#' + Global.Element.CreateCommodityPopup).modal('hide');
                        $('#' + Global.Element.CreateWorkShopPopup).modal('hide');
                        $('#' + Global.Element.CreatePhaseVersionPopup).modal('hide');
                        $('#' + Global.Element.CreatePhaseGroupPopup).modal('hide');

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

    function Delete(Id, oType) {
        var _url = '';
        switch (oType) {
            case 1: _url = '/ProAna/DeleteProduct'; break;
            case 2: _url = '/ProAna/DeleteWorkshop'; break;
            case 6: _url = '/ProAna/DeletePhaseGroup'; break;
        }
        $.ajax({
            url: _url,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        FindCommo(Global.Data.SelectedProductId, false);
                        $('#left').scrollTop(Global.Data.position);

                        if (oType == 6) {
                            $('#jtable-phase-sample,#phase-group-action,#current-phase-group-action,#jtable-phase').hide();
                        }
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupProAna, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    /********************************************************************************************************************        
                                          TECHPROCESS 
    ********************************************************************************************************************/

    //#region                                       Quy trình công nghệ
    function InitListTech_Cycle() {
        $('#' + Global.Element.JtableTech_Cycle).jtable({
            title: 'Danh sách Công Đoạn',
            selectShow: false,
            actions: {
                //listAction: Global.UrlAction.GetTechCycleOfCommodity,
            },
            messages: {
                // selectShow: 'Ẩn hiện cột'
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
                    width: "3%",
                },
                PhaseCode: {
                    title: "Mã Công Đoạn",
                    width: "3%",
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Công Đoạn",
                    width: "20%",
                    display: function (data) {
                        var txt = '<span class="blue">' + data.record.Name + '</span>';
                        return txt;
                    }
                },
                EquipmentId: {
                    title: "Thiết Bị",
                    width: "5%",
                    display: function (data) {
                        return data.record.EquipmentName;
                    }
                },
                TotalTMU: {
                    title: "TGian Chuẩn(s)",
                    width: "7%",
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.TotalTMU + '</span>';
                        return txt;
                    }
                },
                ti_le: {
                    title: 'Tỉ Lệ <br/><input class="form-control" style="width:55px" value="100" type="text" id="tile-parent" onChange="Change()" /> %',
                    width: "5%",
                },
                Time_Percent: {
                    title: 'TGian Theo %',
                    width: "10%",
                    sorting: false,
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.Time_Percent + '</span>';
                        return txt;
                    }
                },
                NumberOfWorker: {
                    title: "Lao Động",
                    width: "5%",
                    sorting: false,
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.NumberOfWorker + '</span>';
                        return txt;
                    }
                },
                Description: {
                    title: "Ghi chú",
                    width: "20%",
                    sorting: false,
                    display: function (data) {
                        txt = '<input type="text" value="' + data.record.Description + '" des />';
                        return txt;
                    }
                }
            }
        });
    }

    function GetTechProcess(node, parentID) {
        $.ajax({
            url: Global.UrlAction.GetTech,
            type: 'post',
            data: JSON.stringify({ 'parentId': parentID, 'node': node }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        Global.Data.TechCycle_Arr.length = 0;
                        if (result.Data.Id == 0) {
                            $('#techId').val(0);
                            $('#total-worker').val(0);
                            var str = '';
                            if (result.Data.details.length > 0) {
                                var total_time_Product = 0;
                                $.each(result.Data.details, function (index, item) {
                                    var ii = index + 1;
                                    item.EquipmentName = item.EquipmentName == null ? '' : item.EquipmentName;
                                    str += '<tr>';
                                    str += '<td >' + ii + '</td>';
                                    str += '<td >' + item.PhaseCode + '</td>';
                                    str += '<td class="red">' + item.PhaseName + '</td>';
                                    str += '<td class="blue">' + item.EquipmentName + '</td>';
                                    str += '<td>' + Math.round((item.StandardTMU) * 1000) / 1000 + '</td>';
                                    str += '<td><input class="form-control" percent type="text" id="percent' + ii + '" value="100" onchange ="ResetTime_Percent(\'' + ii + '\')" /></td>';
                                    str += '<td>' + Math.round(((item.StandardTMU)) * 1000) / 1000 + '</td>';
                                    str += '<td>0</td>';
                                    str += '<td><input class="form-control" description type="text"  value="' + item.Description + '"  /></td>';
                                    str += '</tr>';
                                    let _obj = {
                                        Id: 0,
                                        TechProcessVersionId: 0,
                                        CA_PhaseId: item.CA_PhaseId,
                                        OrderIndex: ii,
                                        EquipmentId: null,
                                        StandardTMU: item.StandardTMU,
                                        Percent: 100,
                                        TimeByPercent: item.StandardTMU,
                                        Worker: 0,
                                        Description: item.Description
                                    };
                                    Global.Data.TechCycle_Arr.push(_obj);
                                    total_time_Product += (_obj.TimeByPercent);
                                });
                                $('#' + Global.Element.JtableTech_Cycle).find('tbody').empty().append(str);

                                // alert(total_time_Product);
                                //tong gio sx 1 san pham
                                $('#time-product').html(Math.round((total_time_Product) * 1000) / 1000);
                                Global.Data.TimeProductPerCommodity = Math.round((total_time_Product) * 1000) / 1000;
                                ResetWorkingBox(0);
                            }
                            else
                                GlobalCommon.ShowMessageDialog("Mã hàng này chưa có công đoạn nào được phân tích. Vui lòng kiểm tra lại.", function () { }, 'Thông báo');
                            if (Global.Data.warningChuaCoQTCN)
                                GlobalCommon.ShowMessageDialog('Quy trình công nghệ chưa được tạo. Bạn cần phải lưu quy trình công nghệ trước rồi mới có thể tạo thiết kế chuyền được !.', function () { }, "Lỗi thao tác");

                        }
                        else {
                            $('#techId').val(result.Data.Id);
                            $('#total-worker').val(result.Data.NumberOfWorkers);
                            $('#pricePerSecond').val(result.Data.PricePerSecond);
                            $('#allowance').val(result.Data.Allowance);
                            $('#work-time').val(result.Data.WorkingTimePerDay);
                            $('#paced_production').html(Math.round((result.Data.PacedProduction) * 1000) / 1000);
                            $('#time-product').html(Math.round((result.Data.TimeCompletePerCommo) * 1000) / 1000);
                            Global.Data.TimeProductPerCommodity = result.Data.TimeCompletePerCommo;
                            $('#pro_group_day').html(Math.round(result.Data.ProOfGroupPerDay * 1000) / 1000);
                            $('#productivity').html(Math.round(result.Data.ProOfGroupPerHour * 1000) / 1000);
                            $('#pro_person_day').html(Math.round(result.Data.ProOfPersonPerDay * 1000) / 1000);

                            var _thanhtien = Global.Data.TimeProductPerCommodity * result.Data.PricePerSecond;
                            var _percentTotal = ((Global.Data.TimeProductPerCommodity * 100) / (parseFloat($('#percent').val()))) * result.Data.PricePerSecond;
                            $('#total-price-percent').html(_percentTotal.toFixed(1));
                            $('#total-price').html(_thanhtien.toFixed(1));

                            $('#des').val(result.Data.Note);

                            if (result.Data.details.length > 0) {
                                str = '';
                                var tog = 0
                                $.each(result.Data.details, function (index, item) {
                                    var ii = index + 1;
                                    str += '<tr>';
                                    str += '<td>' + ii + '</td>';
                                    str += '<td>' + item.PhaseCode + '</td>';
                                    str += '<td>' + item.PhaseName + '</td>';
                                    str += '<td><a class="blue" title="' + item.EquipmentName + '">' + item.EquipmentName + '</a></td>';
                                    str += '<td>' + Math.round((item.StandardTMU) * 1000) / 1000 + '</td>';
                                    str += '<td><input class="form-control" percent type="text" id="percent' + ii + '" value="' + item.Percent + '" onchange ="ResetTime_Percent(\'' + ii + '\')" /></td>';
                                    str += '<td>' + Math.round((item.TimeByPercent) * 1000) / 1000 + '</td>';
                                    str += '<td>' + Math.round(item.Worker * 1000) / 1000 + '</td>';
                                    str += '<td><input class="form-control" description type="text"  value="' + item.Description + '"  /></td>';
                                    str += '</tr>';
                                    let obj = {
                                        Id: item.Id,
                                        TechProcessVersionId: item.TechProcessVersionId,
                                        CA_PhaseId: item.CA_PhaseId,
                                        OrderIndex: ii,
                                        EquipmentId: null,
                                        StandardTMU: Math.round((item.StandardTMU) * 1000) / 1000,
                                        Percent: item.Percent,
                                        TimeByPercent: Math.round((item.TimeByPercent) * 1000) / 1000,
                                        Worker: Math.round(item.Worker * 1000) / 1000,
                                        Description: item.Description

                                    };
                                    Global.Data.TechCycle_Arr.push(obj);
                                    tog += item.TimeByPercent;
                                });
                                Global.Data.TimeProductPerCommodity = Math.round((tog) * 1000) / 1000;
                                $('#' + Global.Element.JtableTech_Cycle).find('tbody').empty().append(str);
                                ResetWorkingBox(0);

                                $('#tile-parent').val(result.Data.PercentWorker)//.change(); 

                            }


                        }

                        if (!Global.Data.warningChuaCoQTCN)
                            Global.Data.warningChuaCoQTCN = true;
                    }
                    else
                        GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupModule, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                });
            }
        });
    }

    function SaveTechVersion() {
        var details = JSON.stringify(Global.Data.TechProcessVersion.details);
        var ver = Global.Data.TechProcessVersion;
        ver.details = [];

        $.ajax({
            url: Global.UrlAction.SaveTech,
            type: 'post',
            data: JSON.stringify({ 'version': ver, 'details': details }),//ko.toJSON(Global.Data.TechProcessVersion),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result != "ERROR") {
                        $('#techId').val(result.Result)
                        if (Global.Data.AfterSave) {
                            window.location.href = Global.Data.techExportUrl;
                            Global.Data.AfterSave = false;
                        }
                        else
                            GlobalCommon.ShowMessageDialog('Lưu thành công.', function () { }, "Thông báo");
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

    function GetData() {
        var version = {
            Id: $('#techId').val(),
            ParentId: Global.Data.CurrentObj.Id,
            PricePerSecond: $('#pricePerSecond').val(),
            Allowance: $('#allowance').val(),
            PercentWorker: $('#tile-parent').val(),
            NumberOfWorkers: $('#total-worker').val(),
            WorkingTimePerDay: parseFloat($('#work-time').val()),
            PacedProduction: parseFloat($('#paced_production').html()),
            TimeCompletePerCommo: parseFloat($('#time-product').html()),
            ProOfGroupPerDay: parseFloat($('#pro_group_day').html()),
            ProOfGroupPerHour: parseFloat($('#productivity').html()),
            ProOfPersonPerDay: parseFloat($('#pro_person_day').html()),
            Note: $('#des').val(),
            details: null,
            ProductId: Global.Data.SelectedProductId
        }
        var $selectedRows = $('#' + Global.Element.JtableTech_Cycle).find('tbody tr')
        if ($selectedRows.length > 0) {
            $selectedRows.each(function (i, item) {
                Global.Data.TechCycle_Arr[i].Description = $($(this).find('[description]')).val();
                Global.Data.TechCycle_Arr[i].Percent = $($(this).find('[percent]')).val();
                // Global.Data.TechCycle_Arr[i].Worker = $($($($selectedRows[i]).find('td'))[5]).html();
            });
        }
        Global.Data.TechProcessVersion = version;
        Global.Data.TechProcessVersion.details = Global.Data.TechCycle_Arr;
    }

    function ResetWorkingBox(index) {
        total_worker = parseFloat($('#total-worker').val());
        total_worktime = parseFloat($('#work-time').val());
        total_timePerProduct = Global.Data.TimeProductPerCommodity;

        paced_production = Math.round(((total_timePerProduct / total_worker)) * 1000) / 1000;
        pro_group_per_hour = Math.round(((3600 / total_timePerProduct) * total_worker) * 1000) / 1000;
        pro_group_per_day = Math.round((pro_group_per_hour * total_worktime) * 1000) / 1000;
        pro_person_per_day = Math.round((pro_group_per_day / total_worker) * 1000) / 1000;

        // nhip do sx
        $('#paced_production').html(paced_production);
        //nang xuat to trong 1 gio
        $('#productivity').html(pro_group_per_hour);
        // nag xuat to trong 1 ngay  
        $('#pro_group_day').html(pro_group_per_day);
        // nag xuat 1 nguoi trong 1 ngay 
        $('#pro_person_day').html(pro_person_per_day);

        var tong = 0;
        var $selectedRows = $('#' + Global.Element.JtableTech_Cycle).find('tbody tr')
        if ($selectedRows.length > 0) {
            $selectedRows.each(function (i, item) {
                time_per = Global.Data.TechCycle_Arr[i].TimeByPercent;
                laodong = Math.round(((time_per / paced_production)) * 1000) / 1000;
                $($($(this).find('td'))[7]).html(laodong);
                Global.Data.TechCycle_Arr[i].Worker = laodong;
                tong += laodong;
            });
            // alert(tong);
        }

        ResetThanhTien();
    }

    function ResetTime_Percent(index) {
        i = parseInt(index);
        value = $('#percent' + i).val();
        var $selectedRows = $('#' + Global.Element.JtableTech_Cycle).find('tbody tr');
        time = parseFloat(Global.Data.TechCycle_Arr[i - 1].StandardTMU);
        timeByPercent = Math.round(((time * 100) / parseFloat(value)) * 1000) / 1000;

        $($($($selectedRows[i - 1]).find('td'))[6]).html(ParseStringToCurrency(timeByPercent));

        oldValue = parseFloat($('#time-product').html());
        oldValue = oldValue - Global.Data.TechCycle_Arr[i - 1].TimeByPercent;
        new_Value = Math.round((oldValue + timeByPercent) * 1000) / 1000;
        $('#time-product').html(Math.round(new_Value * 1000) / 1000);
        Global.Data.TechCycle_Arr[i - 1].TimeByPercent = timeByPercent;
        Global.Data.TimeProductPerCommodity = new_Value;
        ResetWorkingBox(index);
    }


    function InitPopupExportQTCN() {
        $("#" + Global.Element.popupExportQTCN).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.popupExportQTCN + ' button[close]').click(function () {
            $("#" + Global.Element.popupExportQTCN).modal("hide");
        });

        $('#' + Global.Element.popupExportQTCN).on('shown.bs.modal', function () {
            //    $('#' + Global.Element.PopupChooseEquipment).css('z-index', 0);
            $('div.divParent').attr('currentPoppup', Global.Element.popupExportQTCN.toUpperCase());
        });

        $('[texch-export1]').click(function () {
            Global.Data.techExportUrl = Global.UrlAction.ExportToExcel_QTCN + "?parentId=" + Global.Data.SampleObj.ParentId + "&fileType=" + 1;
            if (Global.Data.AfterSave) {
                window.location.href = Global.Data.techExportUrl;
                Global.Data.AfterSave = false;
            }
            else {
                Global.Data.AfterSave = true;
                $('[techsave]').click();
            }
        });
        $('[texch-export2]').click(function () {
            Global.Data.techExportUrl = Global.UrlAction.ExportToExcel_QTCN + "?parentId=" + Global.Data.SampleObj.ParentId + "&fileType=" + 2;
            if (Global.Data.AfterSave) {
                window.location.href = Global.Data.techExportUrl;
                Global.Data.AfterSave = false;
            }
            else {
                Global.Data.AfterSave = true;

                $('[techsave]').click();
            }
        });
        $('[texch-export3]').click(function () {
            Global.Data.techExportUrl = Global.UrlAction.ExportToExcel_QTCN + "?parentId=" + Global.Data.SampleObj.ParentId + "&fileType=" + 3;
            if (Global.Data.AfterSave) {
                window.location.href = Global.Data.techExportUrl;
                Global.Data.AfterSave = false;
            }
            else {
                Global.Data.AfterSave = true;

                $('[techsave]').click();
            }
        });
        $('[texch-export4]').click(function () {
            Global.Data.techExportUrl = Global.UrlAction.ExportToExcel_QTCN + "?parentId=" + Global.Data.SampleObj.ParentId + "&fileType=" + 4;
            if (Global.Data.AfterSave) {
                window.location.href = Global.Data.techExportUrl;
                Global.Data.AfterSave = false;
            }
            else {
                Global.Data.AfterSave = true;
                $('[techsave]').click();
            }
        });

        $('[texch-export5]').click(function () {
            Global.Data.techExportUrl = "/ProAna/ExportToExcel_5?parentId=" + Global.Data.SampleObj.ParentId  ;
            if (Global.Data.AfterSave) {
                window.location.href = Global.Data.techExportUrl;
                Global.Data.AfterSave = false;
            }
            else {
                Global.Data.AfterSave = true;
                $('[techsave]').click();
            }
        });
    }

    function ResetThanhTien() {
        var _thanhtien = Global.Data.TimeProductPerCommodity * (parseFloat($('#pricePerSecond').val()));
        var _percentTotal = ((Global.Data.TimeProductPerCommodity * 100) / (parseFloat($('#percent').val()))) * (parseFloat($('#pricePerSecond').val()));
        $('#total-price-percent').html(_percentTotal.toFixed(1));
        $('#total-price').html(_thanhtien.toFixed(1));
    }
    //#endregion


    //#region                                           Thiết kế chuyền
    function InitList_TKCSample() {
        $('#' + Global.Element.JtableSampleTKC).jtable({
            title: 'Danh sách thiết kế chuyền',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.Gets_TKC,
            },
            messages: {
            },
            datas: {
                jtableId: Global.Element.JtableSampleTKC
            },
            rowInserted: function (event, data) {
                if (data.record.Id == Global.Data.ParentId) {
                    var $a = $('#' + Global.Element.JtableSampleTKC).jtable('getRowByKey', data.record.Id);
                    $($a.children().find('.aaa')).click();
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                LineName: {
                    title: 'Chuyền',
                    width: '7%',
                    edit: false,
                },
                TotalPosition: {
                    title: 'Tổng Vị Trí',
                    width: '7%',
                    edit: false,
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.TotalPosition + '</span> Vị Trí.';
                        return txt;
                    }
                },
                LastEditer: {
                    title: 'Cập nhật cuối',
                    width: '7%',
                    edit: false,
                },
                LastEditTime: {
                    title: 'Giờ cập nhật',
                    width: '7%',
                    edit: false,
                    display: function (data) {
                        if (data.record.LastEditTime != null && typeof (data.record.LastEditTime)) {
                            txt = '<span class="bold red">' + ParseDateToString_cl(parseJsonDateToDate(data.record.LastEditTime)) + '</span>';
                            return txt;
                        }
                    }
                },
                actions: {
                    title: '',
                    width: "7%",
                    sorting: false,
                    columnClass: 'text-center',
                    display: function (parent) {
                        var div = $('<div></div>');
                        var $img = $('<i style="margin-right:10px" class="fa fa-list-ol clickable blue aaa" title="Danh sách thiết kế chuyền "></i>');
                        $img.click(function () {
                            Global.Data.ParentId = parent.record.Id;
                            $('#' + Global.Element.JtableSampleTKC).jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: `<div>Danh sách thiết kế chuyền</div>`,
                                    paging: true,
                                    pageSize: 20,
                                    pageSizeChange: true,
                                    sorting: true,
                                    selectShow: false,
                                    actions: {
                                        listAction: Global.UrlAction.Gets_TKC_Ver + '?labourId=' + parent.record.Id,
                                    },
                                    messages: {
                                    },
                                    fields: {
                                        OrderId: {
                                            type: 'hidden',
                                            defaultValue: parent.record.Id
                                        },
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        TotalPosition: {
                                            title: 'Tổng Vị Trí',
                                            width: '7%',
                                            edit: false,
                                            display: function (data) {
                                                var txt = '<span class="red bold">' + data.record.TotalPosition + '</span> Vị Trí.';
                                                return txt;
                                            }
                                        },
                                        LastEditer: {
                                            title: 'Người tạo',
                                            width: '7%',
                                            edit: false,
                                        },
                                        LastEditTime: {
                                            title: 'Ngày tạo',
                                            width: '7%',
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.LastEditTime != null && typeof (data.record.LastEditTime)) {
                                                    txt = '<span class="bold red">' + ParseDateToString_cl(parseJsonDateToDate(data.record.LastEditTime)) + '</span>';
                                                    return txt;
                                                }
                                            }
                                        },
                                        actions: {
                                            title: '',
                                            width: '5%',
                                            sorting: false,
                                            columnClass: 'text-center',
                                            display: function (data) {
                                                var div = $('<div></div>');
                                                var btnEdit = $('<i data-toggle="modal" data-target="#' + Global.Element.PopupTKC + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                                                btnEdit.click(function () {
                                                    GetLaborDivisionDiagramById(data.record.Id, false);
                                                });
                                                div.append(btnEdit)

                                                var btnExcel = $('<i title="Xuất file Excel" class="fa fa-file-excel-o clickable red" style="padding-left:10px"  ></i>');
                                                btnExcel.click(function () {
                                                    window.location.href = Global.UrlAction.ExportExcel_TKC + '/' + data.record.Id;
                                                });
                                                div.append(btnExcel);
                                                return div;
                                            }
                                        },
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                        });
                        div.append($img);

                        return div;
                    }
                }
            }
        });
    }

    function ReloadList_TKCSample() {
        $('#' + Global.Element.JtableSampleTKC).jtable('load', { 'parentId': Global.Data.SampleObj.ParentId });
    }


    function InitList_TKC() {
        $('#' + Global.Element.JtableTKC).jtable({
            title: 'Danh sách thiết kế chuyền',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: Global.UrlAction.Gets_TKC,
                createAction: Global.Element.PopupTKC,
            },
            messages: {
                // selectShow: 'Ẩn hiện cột',
                addNewRecord: 'Thêm mới',
            },
            datas: {
                jtableId: Global.Element.JtableTKC
            },
            rowInserted: function (event, data) {
                if (data.record.Id == Global.Data.ParentId) {
                    var $a = $('#' + Global.Element.JtableTKC).jtable('getRowByKey', data.record.Id);
                    $($a.children().find('.aaa')).click();
                }
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                LineName: {
                    title: 'Chuyền',
                    width: '7%',
                    edit: false,
                },
                TotalPosition: {
                    title: 'Tổng Vị Trí',
                    width: '7%',
                    edit: false,
                    display: function (data) {
                        var txt = '<span class="red bold">' + data.record.TotalPosition + '</span> Vị Trí.';
                        return txt;
                    }
                },
                LastEditer: {
                    title: 'Cập nhật cuối',
                    width: '7%',
                    edit: false,
                },
                LastEditTime: {
                    title: 'Giờ cập nhật',
                    width: '7%',
                    edit: false,
                    display: function (data) {
                        if (data.record.LastEditTime != null && typeof (data.record.LastEditTime)) {
                            txt = '<span class="bold red">' + ParseDateToString_cl(parseJsonDateToDate(data.record.LastEditTime)) + '</span>';
                            return txt;
                        }
                    }
                },
                actions: {
                    title: '',
                    width: "7%",
                    sorting: false,
                    columnClass: 'text-center',
                    display: function (parent) {
                        var div = $('<div></div>');
                        var $img = $('<i style="margin-right:10px" class="fa fa-list-ol clickable blue aaa" title="Danh sách thiết kế chuyền "></i>');
                        $img.click(function () {
                            Global.Data.ParentId = parent.record.Id;
                            $('#' + Global.Element.JtableTKC).jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: `<div>Danh sách thiết kế chuyền</div>`,
                                    paging: true,
                                    pageSize: 20,
                                    pageSizeChange: true,
                                    sorting: true,
                                    selectShow: false,
                                    actions: {
                                        listAction: Global.UrlAction.Gets_TKC_Ver + '?labourId=' + parent.record.Id,
                                    },
                                    messages: {
                                    },
                                    fields: {
                                        OrderId: {
                                            type: 'hidden',
                                            defaultValue: parent.record.Id
                                        },
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        TotalPosition: {
                                            title: 'Tổng Vị Trí',
                                            width: '7%',
                                            edit: false,
                                            display: function (data) {
                                                var txt = '<span class="red bold">' + data.record.TotalPosition + '</span> Vị Trí.';
                                                return txt;
                                            }
                                        },
                                        LastEditer: {
                                            title: 'Người tạo',
                                            width: '7%',
                                            edit: false,
                                        },
                                        LastEditTime: {
                                            title: 'Ngày tạo',
                                            width: '7%',
                                            edit: false,
                                            display: function (data) {
                                                if (data.record.LastEditTime != null && typeof (data.record.LastEditTime)) {
                                                    txt = '<span class="bold red">' + ParseDateToString_cl(parseJsonDateToDate(data.record.LastEditTime)) + '</span>';
                                                    return txt;
                                                }
                                            }
                                        },
                                        actions: {
                                            title: '',
                                            width: '5%',
                                            sorting: false,
                                            columnClass: 'text-center',
                                            display: function (data) {
                                                var div = $('<div></div>');
                                                var btnEdit = $('<i data-toggle="modal" data-target="#' + Global.Element.PopupTKC + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                                                btnEdit.click(function () {
                                                    GetLaborDivisionDiagramById(data.record.Id, true);
                                                });
                                                div.append(btnEdit)

                                                var btnExcel = $('<i title="Xuất file Excel" class="fa fa-file-excel-o clickable red" style="padding-left:10px"  ></i>');
                                                btnExcel.click(function () {
                                                    window.location.href = Global.UrlAction.ExportExcel_TKC + '/' + data.record.Id;
                                                });
                                                div.append(btnExcel);
                                                return div;
                                            }
                                        },
                                    }
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                        });
                        div.append($img);

                        var btnDelete = $('<i title="Xóa" class="fa fa-trash red i-delete clickable"></i>');
                        btnDelete.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete_TKC(parent.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        div.append(btnDelete);
                        return div;
                    }
                }
            }
        });
    }

    function ReloadList_TKC() {
        $('#' + Global.Element.JtableTKC).jtable('load', { 'parentId': Global.Data.CurrentObj.ParentId }); //$('#jtable_tkc').attr('pId')
        //  if (Global.Data.TechProVerId == null || Global.Data.TechProVerId == 0)
        //      GlobalCommon.ShowMessageDialog('Quy trình công nghệ chưa được tạo. Bạn cần phải lưu quy trình công nghệ trước rồi mới có thể tạo thiết kế chuyền được !.', function () { }, "Lỗi thao tác");

    }

    function Delete_TKC(Id) {
        $.ajax({
            url: Global.UrlAction.Delete_TKC,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadList_TKC();
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupSize, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }


    function GetTech(node) {
        Global.Data.TechProcessVerDetailArray.length = 0;
        Global.Data.PhaseList.length = 0;
        Global.Data.BasePhases.length = 0;
        $.ajax({
            url: Global.UrlAction.GetTech,
            type: 'post',
            data: JSON.stringify({ 'parentId': Global.Data.CurrentObj.Id - 1, 'node': Global.Data.CurrentObj.Node }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        Global.Data.TechProVerId = result.Data.Id;

                        if (result.Data.details != null && result.Data.details.length > 0) {
                            $('#time-per-commo').html(Math.round(result.Data.TimeCompletePerCommo));
                            $('#pro-per-person').html(Math.round(result.Data.ProOfPersonPerDay));
                            $('#pro-group-per-hour').html(Math.round(result.Data.ProOfGroupPerHour));
                            $('#pro-group-per-day').html(Math.round(result.Data.ProOfGroupPerDay));
                            $('#paced-product').html(Math.round(result.Data.PacedProduction));
                            $('#workers').html(result.Data.NumberOfWorkers);
                            $('#time-per-day').html(result.Data.WorkingTimePerDay);
                            $('#note').html(result.Data.Note);

                            var option = '';
                            var code_option = '';
                            $.each(result.Data.details, function (i, item) {
                                var obj = {
                                    Id: item.Id,
                                    TechProcessVersionId: item.TechProcessVersionId,
                                    OrderIndex: item.OrderIndex,
                                    PhaseCode: item.PhaseCode,
                                    PhaseName: item.PhaseName,
                                    StandardTMU: item.StandardTMU,
                                    Percent: item.Percent,
                                    TimeByPercent: item.TimeByPercent,
                                    Worker: item.Worker,
                                    Description: item.Description,
                                    EquipmentCode: item.EquipmentCode,
                                    TotalTMU: item.TotalTMU,
                                    SkillRequired: item.SkillRequired,
                                    PhaseGroupId: item.PhaseGroupId,
                                    Index: item.Index,
                                    De_Percent: 0
                                }
                                Global.Data.TechProcessVerDetailArray.push(obj);
                                Global.Data.PhaseList.push(obj);
                                Global.Data.BasePhases.push(obj);
                                option += '<option value="' + item.PhaseName + '" /> ';
                                code_option += '<option value="' + (item.PhaseCode.trim() + ' (' + item.PhaseName + ')') + '" /> ';
                            });
                            $('#autoCompleteSource').append(option);
                            $('#autoCompleteSource_Code').append(code_option);
                        }
                        else {
                            //GlobalCommon.ShowMessageDialog("Mã hàng này chưa có quy trình công nghệ không thể thiết kế chuyền được. Vui lòng tạo quy trình công nghệ trước.", function () { }, 'Thông báo');
                            $('#' + Global.Element.Jtable).hide();
                        }
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

    //#endregion


    //#region  công đoạn

    function SavePhase() {
        var obj = {
            Id: $('#phaseID').val() == '' ? 0 : $('#phaseID').val(),
            Name: $('#phase-name').val(),
            Code: $('#phase-code').html(),
            Description: $('#phase-Des').val(),
            EquipmentId: $('#equipmentId').val(),
            WorkerLevelId: $('#workersLevel').val(),
            PhaseGroupId: Global.Data.CurrentObj.ObjectId,
            ParentId: Global.Data.CurrentObj.Id,
            TotalTMU: parseFloat($('#TotalTMU').html()),
            ApplyPressuresId: $('#ApplyPressure').val(),
            PercentWasteEquipment: $('[percentEquipment]').val(),
            PercentWasteManipulation: $('[percentManipulation]').val(),
            PercentWasteSpecial: $('[percentDB]').val(),
            PercentWasteMaterial: $('[percentNPL]').val(),
            Node: Global.Data.CurrentObj.Node,
            ManiVerTMU: 0,
            IsDetailChange: Global.Data.ManipulationVersionModel.IsDetailChange,
            actions: Global.Data.PhaseManiVerDetailArray,
            Index: ($('#phase-index').val() == '' ? (Global.Data.phaseLastIndex + 1) : parseInt($('#phase-index').val())),
            Video: Global.Data.Video,
            IsLibrary: $('#islibs').prop('checked')
        }
        $.ajax({
            url: Global.UrlAction.SavePhase,
            type: 'post',
            data: JSON.stringify({ 'phase': obj, 'accessories': Global.Data.AccessoriesArray, 'timePrepares': Global.Data.TimePrepareArray }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        if (obj.Id == 0) {
                            $('#phaseID').val(result.Data);
                        }

                        /*
                        GetLastPhaseIndex();
                        ReloadListPhase_View();
                        Global.Data.TimePrepareArray.length = 0;
                        ReloadListTimePrepare();
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

    function Check_Phase_Validate() {
        if ($('#phase-name').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui Nhập Tên Công Đoạn .", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('#workersLevel').val() == "" || $('#workersLevel').val() == "0") {
            GlobalCommon.ShowMessageDialog("Vui Nhập chọn bậc thợ .", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

    function GetLastPhaseIndex() {
        $.ajax({
            url: Global.UrlAction.GetLastIndex,
            type: 'POST',
            data: JSON.stringify({ 'Id': Global.Data.CurrentObj.Id }),
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
    //#endregion

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
                        //txt.click(function () { txt.select(); })
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

    //#region thiết bị & tính TMU code may va cat
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
                addNewRecord: 'Thêm mới',
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
                Code: {
                    title: "Mã Thiết Bị",
                    width: "10%",
                },
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


    function split_Arr() {
        $.each(Global.Data.PhaseManiVerDetailArray, function (i, item) {
            if (i < Global.Data.PhaseManiVerDetailArray.length) {
                if (item.ManipulationCode.substring(0, 2) == 'SE' || item.ManipulationCode.substring(0, 1) == 'C') {
                    Global.Data.PhaseManiVerDetailArray.splice(i, 1);
                    split_Arr();
                    return false;
                }
            }
        });
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
            }
        });
    }


    //#region gợi y cong đoạn
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

                        Global.Data.TimePrepareArray.length = 0;
                        if (data.Records.timePrepares.length > 0) {
                            $.each(data.Records.timePrepares, function (i, item) {
                                Global.Data.TimePrepareArray.push(item);
                            });
                            // ReloadListTimePrepare();
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
    //#endregion

    //#endregion

    function InitPopupEquipment() {
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
            ReloadListEquipment();
            $('#' + Global.Element.CreatePhasePopup).hide();
            $('div.divParent').attr('currentPoppup', Global.Element.PopupChooseEquipment.toUpperCase());
        });

        $('[chooseequipment_popupclose]').click(function () {
            $('#' + Global.Element.CreatePhasePopup).show();
            $('div.divParent').attr('currentPoppup', Global.Element.CreatePhasePopup.toUpperCase());
        });
    }

    //#region *******************************            time prepare choose            *********************************************/

    function InitPopupTimeRepare() {
        $('#' + Global.Element.timeprepare_Popup).on('shown.bs.modal', function () {
            $('#' + Global.Element.CreatePhasePopup).hide();
            ReloadListTimePrepare_Chooise();
            $('div.divParent').attr('currentPoppup', Global.Element.timeprepare_Popup.toUpperCase());
        });
        $('#choose-time').click(function () {
            var $selectedRows = $('#' + Global.Element.jtable_Timeprepare_Chooise).jtable('selectedRows');
            if ($selectedRows.length > 0) {
                successCount = 0;
                flag = false;
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    flag = false;
                    //push record into array 
                    $.each(Global.Data.TimePrepareArray, function (i, item) {
                        if (item.TimePrepareId == record.Id) {
                            GlobalCommon.ShowMessageDialog("Thời Gian Chuẩn Bị này đã được chọn. Vui lòng kiểm tra lại.!", function () { }, "Thông báo Thời Gian Chuẩn Bị Tồn Tại");
                            flag = true;
                            return false;
                        }
                    });
                    if (!flag) {
                        obj = {
                            Id: record.Id,
                            TimePrepareId: record.Id,
                            Name: record.Name,
                            Code: record.Code,
                            TimeTypePrepareName: record.TimeTypePrepareName,
                            Description: record.Description,
                            TMUNumber: record.TMUNumber
                        }
                        Global.Data.TimePrepareArray.push(obj);
                        successCount++;
                    }
                });
                if (successCount > 0) {
                    $('#' + Global.Element.timeprepare_Popup).modal('hide');
                    $('#' + Global.Element.CreatePhasePopup).show();
                    // GlobalCommon.ShowMessageDialog("Đã thêm " + successCount + " Thời Gian Chuẩn Bị thành công.!", function () { }, "Thông báo Thêm Thành Công");
                    // ReloadListTimePrepare();
                    // $('#' + Global.Element.jtable_timeprepare_arr).jtable('deselectRows');
                    $('.jtable-row-selected').removeClass('jtable-row-selected');

                    Global.Data.PhaseModel.IsTimePrepareChange = true;
                    UpdateTotalTimeVersion();
                }


                if (Global.Data.TimePrepareArray && Global.Data.TimePrepareArray.length > 0)
                    $('#time-repare-name').val(`${Global.Data.TimePrepareArray[0].Name} - TMU: ${Global.Data.TimePrepareArray[0].TMUNumber}`);
                else
                    $('#time-repare-name').val('');
                $('div.divParent').attr('currentPoppup', Global.Element.JtablePhase.toUpperCase());
            }
            else {
                GlobalCommon.ShowMessageDialog("Không có Thời Gian Chuẩn Bị nào được chọn. Vui lòng kiểm tra lại.!", function () { }, "Thông báo Chưa chọn Thời Gian Chuẩn Bị");
            }
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
                            Global.Data.TimePrepareArray.length = 0;
                            var obj = {
                                Id: record.Id,
                                TimePrepareId: record.Id,
                                Name: record.Name,
                                Code: record.Code,
                                TimeTypePrepareName: record.TimeTypePrepareName,
                                Description: record.Description,
                                TMUNumber: record.TMUNumber
                            }
                            Global.Data.TimePrepareArray.push(obj);

                            $('#' + Global.Element.timeprepare_Popup).modal('hide');
                            $('#' + Global.Element.CreatePhasePopup).show();
                            // ReloadListTimePrepare();

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
    //#endregion

    //#region Thu vien thao tác
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

    //#endregion

    function InitPopupCreatePhase() {
        $('#' + Global.Element.CreatePhasePopup).on('shown.bs.modal', function () {
            if (Global.Data.isInsertPhase) {
                $('#phase-code').html(((Global.Data.PhaseAutoCode == null || Global.Data.PhaseAutoCode == '' ? '' : (Global.Data.PhaseAutoCode + '-')) + (Global.Data.phaseLastIndex + 1)));
                $('#phase-index').val((Global.Data.phaseLastIndex + 1));
                $('[percentequipment],[percentdb],[percentnpl').val(0);
                $('[percentmanipulation]').val($('#config').attr('maniexpenddefault'));
            }
        });
    }


    //#region Thiet ke chuyen
    function InitPopupCreateTKC() {
        $('[re_line_tkc]').click(function () {
            GetLineSelect('tkc_lineName', Global.Data.SelectedWorkshopId);
            $('#tkc_lineName').change();
        });

        $('#tkc_lineName').change(function () {
            var _option = $('#tkc_lineName').children('option:selected');
            $('#workers').html(_option.attr('labours'));

            $('[re_employ_tkc]').click();
        });

        $('#tkc_sl_line').change(function () {
            if ($('#tkc_sl_line').val() != '') {
                Draw_LinePosition(parseInt($('#tkc_sl_line').val()));
                $('#tkc_sl_line').prop('disabled', true);
                if (parseInt($('#tkc_sl_line').val()) % 2 != 0)
                    $('#tkc_sl_line').val((parseInt($('#tkc_sl_line').val()) + 1))
            }
        });

        $('[add_row]').click(function () {
            if (Global.Data.Position_Arr.length == 0) {
                Draw_Position(1, 2);
                $('#tkc_sl_line').val(2);
            }
            else {
                var last = Global.Data.Position_Arr[Global.Data.Position_Arr.length - 1].OrderIndex;
                Draw_Position(last + 1, last + 2);
                $('#tkc_sl_line').val(parseInt($('#tkc_sl_line').val()) + 2);
            }
            $('#tkc_sl_line').prop('disabled', true);
        });

        $('[tkc-save-draft]').click(function () {
            if ($('#tkc_sl_line').val().trim() == "" || parseInt($('#tkc_sl_line').val()) == 0) {
                GlobalCommon.ShowMessageDialog("Vui lòng nhập số lượng vị trí phân công.", function () { }, "Lỗi Nhập liệu");
                return false;
            }
            var obj = {
                LabourDivisionVerId: ($('#tkc-LabourDivisionVerId').val() == '' ? 0 : $('#tkc-LabourDivisionVerId').val()),
                Id: ($('#tkc_id').val() == '' ? 0 : $('#tkc_id').val()),
                ParentId: $('#jtable-tkc').attr('pId'),
                TechProVer_Id: Global.Data.TechProVerId,
                LineId: $('#tkc_lineName').val(),
                TotalPosition: $('#tkc_sl_line').val(),
                Positions: Global.Data.Position_Arr,
            }
            SaveDiagram(obj, false);
        });

        $('[tkc_save]').click(function () {
            if ($('#tkc_sl_line').val().trim() == "" || parseInt($('#tkc_sl_line').val()) == 0) {
                GlobalCommon.ShowMessageDialog("Vui lòng nhập số lượng vị trí phân công.", function () { }, "Lỗi Nhập liệu");
                return false;
            }
            var obj = {
                Id: ($('#tkc_id').val() == '' ? 0 : $('#tkc_id').val()),
                LabourDivisionVerId: ($('#tkc-LabourDivisionVerId').val() == '' ? 0 : $('#tkc-LabourDivisionVerId').val()),
                ParentId: $('#jtable-tkc').attr('pId'),
                TechProVer_Id: Global.Data.TechProVerId,
                LineId: $('#tkc_lineName').val(),
                TotalPosition: $('#tkc_sl_line').val(),
                Positions: Global.Data.Position_Arr,
            }
            SaveDiagram(obj, true);
        });

        $('[tkc_cancel]').click(function () {
            $('#tkc_id').val(0);
            $('#tkc-LabourDivisionVerId').val(0)
            $('#tkc_sl_line').val(0);
            $('#tkc_sl_line').prop('disabled', false);
            $('#tkc_des').val(0);
            Global.Data.Position_Arr.length = 0;
            $('#line-box').empty();
            $('#tkc_lineName').removeAttr('disabled');

            Global.Data.PhaseList.length = 0;
            $.each(Global.Data.BasePhases, function (i, item) {
                var obj = {
                    Id: item.Id,
                    TechProcessVersionId: item.TechProcessVersionId,
                    OrderIndex: item.OrderIndex,
                    PhaseCode: item.PhaseCode,
                    PhaseName: item.PhaseName,
                    StandardTMU: item.StandardTMU,
                    Percent: item.Percent,
                    TimeByPercent: item.TimeByPercent,
                    Worker: item.Worker,
                    Description: item.Description,
                    EquipmentCode: item.EquipmentCode,
                    TotalTMU: item.TotalTMU,
                    SkillRequired: item.SkillRequired,
                    PhaseGroupId: item.PhaseGroupId,
                    De_Percent: 0
                }
                Global.Data.PhaseList.push(obj);
            });
            $('div.divParent').attr('currentPoppup', '');
        });

        $('#jtable-tkc').change(function () {
            GetTech();
            //GetLineSelect('tkc_lineName', parseInt($('#jtable_tkc').attr('wkId')));
            ReloadList_TKC();
        });

        $('[re_employ_tkc]').click(function () {
            GetEmployeeWithSkill('tkc_Employee');
        });

        $('#' + Global.Element.Popup_position).on('shown.bs.modal', function () {
            $('#' + Global.Element.PopupTKC).hide();
            $('div.divParent').attr('currentPoppup', Global.Element.Popup_position.toUpperCase());

        });

        $('[cancel_tkc_po]').click(function () {
            $('#' + Global.Element.PopupTKC).show();

            Global.Data.Phase_Arr.length = 0;
            $('#' + Global.Element.PopupChoose).modal('hide');
            $('#tkc_Employee').val(0);
            $('div.divParent').attr('currentPoppup', Global.Element.PopupTKC.toUpperCase());
        });

        $('[save_tkc_po]').click(function () {
            // neu co cai cũ thi trả lai gia tri cũ truoc khi them mới
            if (Global.Data.Position_Arr[Global.Data.Index - 1].Details.length > 0) {
                Global.Data.Position_Arr[Global.Data.Index - 1].Details.forEach(function (item, i) {
                    Global.Data.PhaseList.forEach(function (phase, ii) {
                        if (phase.Id == item.TechProVerDe_Id) {
                            var _per = item.DevisionPercent > phase.De_Percent ? item.DevisionPercent - phase.De_Percent : phase.De_Percent - item.DevisionPercent;
                            phase.De_Percent = _per;
                            console.log('per:', _per);
                            console.log('De_Percent:', phase.De_Percent);
                            return false;
                        }
                    });
                });
            }

            Global.Data.Position_Arr[Global.Data.Index - 1].Details.length = 0;
            $.each(Global.Data.Phase_Arr, function (i, item) {
                item.DevisionPercent = item.DevisionPercent_Temp;
                Global.Data.Position_Arr[Global.Data.Index - 1].Details.push(item);
                $.each(Global.Data.PhaseList, function (ii, phase) {
                    if (phase.Id == item.TechProVerDe_Id) {
                        phase.De_Percent += item.DevisionPercent_Temp;
                        phase.De_Percent = phase.De_Percent > 100 ? 100 : phase.De_Percent;
                        return false;
                    }
                });
            });

            if ($('#tkc_Employee').val() != 0) {
                Global.Data.Position_Arr[Global.Data.Index - 1].EmployeeId = parseInt($('#tkc_Employee').val());
                Global.Data.Position_Arr[Global.Data.Index - 1].EmployeeName = $('#tkc_Employee option:selected').text();
            }
            $('[cancel_tkc_po]').click();
            Add_PhaseIntoPosition();
        });

    }
    /************************************************************************************************************/
    function Draw_LinePosition(positions) {
        if (positions != 0) {
            positions = positions % 2 == 0 ? positions : positions + 1;
            if (positions > 0) {
                for (var i = 1, y = i + 1; i <= positions; i += 2, y += 2) {
                    Draw_Position(i, y);
                }
            }
        }
    }

    function Draw_Position(i, y) {
        var str = '';
        str += '<div id="row_' + i + '" class="line-box" >';
        str += '  <div id="' + i + '" class="line-left">';
        str += '      <div class="child">';
        str += '          <div title="Click Tạo Lối Đi ở Vị Trí này." style="width: 22px;float: left;"><div class="insert-exit" onclick="insert_Exit(\'' + i + '\')"></div>';
        str += '          <div title="Click Tạo đường vào BTP ở Vị Trí này." class="insert-btp" onclick="insert_BTP(\'' + i + '\')"></div> ';
        str += '          <div title="Click Xóa Vị Trí." class="delete" onclick="delete_Po(\'' + i + '\')"></div></div>';
        str += '          <div class="main-info c-left" >';
        str += '              <div  class="line-child-box">';
        str += '              <div class="top"> ';
        str += '                  <div id="phase"></div>';
        str += '                  <div style="clear: both"></div>';
        str += '              </div>';
        str += '             <div><div class="col-md-1"><input type="text" linePo_' + i + ' onchange="ChangeIndex(' + i + ')" value="' + i + '"/></div><div class="col-md-11 bottom bold red" style="background-image: linear-gradient( rgb(247, 234, 236),rgb(255, 236, 239),rgb(255, 236, 239)) !important" onclick="ChooseEmployee(\'' + i + '\')" data-toggle="modal" list="autoCompleteSource" data-target="#' + Global.Element.Popup_position + '">Chọn Nhân Viên ...</div><div class="clearfix"></div></div>';
        str += '          </div>';
        str += '      </div>';
        str += '      <div class="equipment" ><div ></div></div>';
        str += '      <div style="clear: left"></div>';
        str += '      <div class="exit e-left" style="background-image: linear-gradient( #0000FF,#0000FF,#0000FF) !important;">Lối Đi</div>';
        str += '  </div>';
        str += '  </div>';
        str += '  <div class="line-center" style ="background-image: linear-gradient( #FFD700,#FFD700,#FFD700) !important;' + (i == 0 ? "border-top:1px solid #ccc" : "") + '' + (i == 8 ? "border-bottom:1px solid #ccc" : "") + '"></div>';
        str += '      <div id="' + y + '" class="line-right">';
        str += '          <div class="child">';
        str += '              <div class="equipment"><div style="transform:rotate(-90deg); -webkit-transform:rotate(-90deg); -ms-transform:rotate(-90deg); max-width:50px"></div></div>';
        str += '              <div class="main-info c-right"  >';
        str += '                  <div id="' + y + '" class="line-child-box">';
        str += '                  <div class="top"> ';
        str += '                      <div id="phase"></div>';
        str += '                      <div style="clear: both"></div>';
        str += '                  </div>';
        str += '                  <div><div class="col-md-1"><input type="text" linePo_' + y + ' onchange="ChangeIndex(' + y + ')" value="' + y + '"/></div><div class="col-md-11 bottom bold red" style="background-image: linear-gradient( rgb(247, 234, 236),rgb(255, 236, 239),rgb(255, 236, 239)) !important ;" onclick="ChooseEmployee(\'' + y + '\')" data-toggle="modal" data-target="#' + Global.Element.Popup_position + '">Chọn Nhân Viên ...</div><div class="clearfix"></div></div>';
        str += '              </div>';
        str += '          </div>';
        str += '          <div title="Click Tạo đường vào BTP ở Vị Trí này." class="insert-btp" onclick="insert_BTP(\'' + y + '\')"></div>';
        str += '          <div style="clear: left"></div>';
        str += '          <div class="exit e-right" style="background-image: linear-gradient( #0000FF,#0000FF,#0000FF) !important;">Lối Đi</div>';
        str += '      </div>';
        str += '  </div>';
        str += '  <div style="clear: left"></div>';
        str += '</div>';

        var obj = {
            Id: 0,
            TechProVer_Id: parseInt($('#url').attr('TechProVerId')),
            OrderIndex: i,
            EmployeeId: null,
            EmployeeName: '',
            IsHasExitLine: false,
            IsHasBTP: false,
            Details: []
        }
        Global.Data.Position_Arr.push(obj);
        obj = {
            Id: 0,
            TechProVer_Id: parseInt($('#url').attr('TechProVerId')),
            OrderIndex: y,
            EmployeeId: null,
            EmployeeName: '',
            IsHasExitLine: false,
            IsHasBTP: false,
            Details: []
        }
        Global.Data.Position_Arr.push(obj);

        $('#line-box').html(str + $('#line-box').html());

    }

    //draw with detail
    function DrawLinePositionWidthDetail(positions, details) {
        Global.Data.Position_Arr.length = 0;
        $('#line-box').empty();
        if (positions.length > 0) {
            rows = (positions.length - 1) % 2 == 0 ? (positions.length - 1) / 2 : parseInt((positions.length - 1) / 2) + 1;
            str_left = '';
            str_right = '';
            str = '';

            if (rows > 0) {
                for (var i = 1, y = i + 1; i <= positions.length; i += 2, y += 2) {
                    var ul = '';
                    var equip = '';
                    var tongtmu = 0;
                    var numOfLabor = 0;
                    var tb2 = '';
                    var BTP = false;
                    str = '';
                    $.each(positions, function (index, LinePosition) {
                        if (LinePosition.OrderIndex == i) {
                            ul += '<table>';
                            tb2 += '<table style="width:100%"><tr>';
                            var flag = false;
                            var EQUIP = [];
                            $.each(LinePosition.Details, function (iii, detail) {
                                $.each(Global.Data.PhaseList, function (ii, phase) {
                                    if (detail.TechProVerDe_Id == phase.Id) {
                                        phase.De_Percent += detail.DevisionPercent;

                                        ul += '<tr>';
                                        ul += '<td class="code">' + phase.PhaseCode + '</td>';
                                        ul += '<td class="pname">' + phase.PhaseName + '</td>';
                                        ul += '<td class="tmu">' + Math.round(phase.TimeByPercent) + '</td>';
                                        ul += '<td class="per blue">' + detail.DevisionPercent + '</td>';
                                        ul += '<td class="labor red">' + Math.round(detail.NumberOfLabor * 10) / 10 + '</td>';
                                        ul += '</tr>';
                                        if (EQUIP.length > 0) {
                                            $.each(EQUIP, function (e, E) {
                                                if (E == phase.EquipmentCode) {
                                                    flag = true;
                                                    return false;
                                                }
                                            });
                                            if (!flag && phase.EquipmentCode != null) {
                                                equip += phase.EquipmentCode + '<br/>';
                                                EQUIP.push(phase.EquipmentCode);
                                            }
                                        }
                                        else {
                                            if (phase.EquipmentCode != '') {
                                                EQUIP.push(phase.EquipmentCode);
                                                equip += phase.EquipmentCode + '<br/>';
                                            }
                                        }
                                        tongtmu += phase.TimeByPercent;
                                        numOfLabor += detail.NumberOfLabor;
                                        return false;
                                    }
                                });
                            });
                            // add empty object detail
                            var obj = {
                                Id: 0,
                                Line_PositionId: LinePosition.Id,
                                IsPass: false,
                                TechProVerDe_Id: 0,
                                PhaseCode: '',
                                PhaseName: '',
                                EquipmentId: 0,
                                EquipmentCode: '',
                                TotalTMU: 0,
                                PhaseGroupId: 0,
                                SkillRequired: 0,
                                TotalLabor: 0,
                                DevisionPercent: 0,
                                DevisionPercent_Temp: 0,
                                NumberOfLabor: 0,
                                Note: '',
                                OrderIndex: LinePosition.Details.length + 1,
                            }
                            LinePosition.Details.push(obj);
                            var name = LinePosition.EmployeeName + '' == 'null' ? '...  Chọn  Nhân Viên  ...' : LinePosition.EmployeeName;
                            ul += '</table>';
                            tb2 += '<td class="pname" style="width:75%">' + name + '</td>';
                            tb2 += '<td class="tmu" style="width:12%;">' + Math.round(tongtmu) + '</td>';
                            tb2 += '<td class="labor" style="width:12%; border-right:none">' + Math.round(numOfLabor * 10) / 10 + '</td>';
                            tb2 += '</tr></table>';

                            if (LinePosition.IsHasBTP)
                                BTP = true;
                            Global.Data.Position_Arr.push({
                                Id: LinePosition.Id,
                                TechProVer_Id: LinePosition.TechProVer_Id,
                                OrderIndex: LinePosition.OrderIndex,
                                EmployeeId: LinePosition.EmployeeId,
                                EmployeeName: LinePosition.EmployeeName,
                                IsHasExitLine: LinePosition.IsHasExitLine,
                                IsHasBTP: LinePosition.IsHasBTP,
                                Details: LinePosition.Details
                            });
                            return false;
                        }
                    });
                    if (ul == '' && tb2 == '') {
                        tb2 = 'Chọn Nhân Viên ...';
                    }
                    str += '<div id="row_' + i + '" class="line-box" style ="' + (i == 0 ? "margin-top:20px" : "") + '">';
                    str += '  <div id="' + i + '" class="line-left">';
                    str += '      <div class="child">';
                    str += '         <div title="Click Tạo Lối Đi ở Vị Trí này." style="width: 22px;float: left;"> <div class="insert-exit" onclick=insert_Exit(\'' + i + '\')></div>';
                    str += '          <div title="Click Tạo đường vào BTP ở Vị Trí này." class="insert-btp ' + (BTP ? "insert-hover" : "") + '" onclick=insert_BTP(\'' + i + '\')></div>';
                    str += '          <div title="Click Xóa Vị Trí." class="delete" onclick="delete_Po(\'' + i + '\')"></div></div>';
                    str += '          <div class="main-info c-left" >';
                    str += '              <div  class="line-child-box">';
                    str += '              <div class="top"> ';
                    str += '                  <div id="phase">' + ul + '</div>';
                    str += '                  <div style="clear: both"></div>';
                    str += '              </div>';
                    str += '              <div><div class="col-md-1"><input type="text" linePo_' + i + ' onchange="ChangeIndex(' + i + ')" value="' + i + '"/></div><div class="col-md-11 bottom bold red" style="background-image: linear-gradient( rgb(247, 234, 236),rgb(255, 236, 239),rgb(255, 236, 239)) !important" onclick="ChooseEmployee(\'' + i + '\')" data-toggle="modal" list="autoCompleteSource" data-target="#' + Global.Element.Popup_position + '">' + tb2 + '</div><div class="clearfix"></div></div>';
                    str += '          </div>';
                    str += '      </div>';
                    str += '      <div class="equipment" ><div style="transform:rotate(-90deg); -webkit-transform:rotate(-90deg); -ms-transform:rotate(-90deg); max-width:50px ">' + equip + '</div></div>';
                    str += '      <div style="clear: left"></div>';
                    str += '      <div class="exit e-left" style="background-image: linear-gradient( #0000FF,#0000FF,#0000FF) !important;">Lối Đi</div>';
                    str += '  </div>';
                    str += '  </div>';
                    var ul = '';
                    var equip = '';
                    var tongtmu = 0;
                    var numOfLabor = 0;
                    var tb2 = '';
                    var BTP = false;
                    $.each(positions, function (index, LinePosition) {
                        if (LinePosition.OrderIndex == y) {
                            ul += '<table>';
                            tb2 += '<table style="width:100%"><tr>';
                            var flag = false;
                            var EQUIP = [];
                            $.each(LinePosition.Details, function (iii, detail) {
                                $.each(Global.Data.PhaseList, function (ii, phase) {
                                    if (detail.TechProVerDe_Id == phase.Id) {
                                        phase.De_Percent += detail.DevisionPercent;

                                        ul += '<tr>';
                                        ul += '<td class="code">' + phase.PhaseCode + '</td>';
                                        ul += '<td class="pname">' + phase.PhaseName + '</td>';
                                        ul += '<td class="tmu">' + Math.round(phase.TimeByPercent) + '</td>';
                                        ul += '<td class="per blue">' + detail.DevisionPercent + '</td>';
                                        ul += '<td class="labor red">' + Math.round(detail.NumberOfLabor * 10) / 10 + '</td>';
                                        ul += '</tr>';

                                        if (EQUIP.length > 0) {
                                            $.each(EQUIP, function (e, E) {
                                                if (E == phase.EquipmentCode) {
                                                    flag = true;
                                                    return false;
                                                }
                                            });
                                            if (!flag && phase.EquipmentCode != null) {
                                                equip += phase.EquipmentCode + '<br/>';
                                                EQUIP.push(phase.EquipmentCode);
                                            }
                                        }
                                        else {
                                            EQUIP.push(phase.EquipmentCode);
                                            equip += phase.EquipmentCode + '<br/>';
                                        }
                                        tongtmu += phase.TimeByPercent;
                                        numOfLabor += detail.NumberOfLabor;
                                        return false;
                                    }
                                });
                            });
                            // add empty object detail
                            var obj = {
                                Id: 0,
                                Line_PositionId: LinePosition.Id,
                                IsPass: false,
                                TechProVerDe_Id: 0,
                                PhaseCode: '',
                                PhaseName: '',
                                EquipmentId: 0,
                                EquipmentCode: '',
                                TotalTMU: 0,
                                PhaseGroupId: 0,
                                SkillRequired: 0,
                                TotalLabor: 0,
                                DevisionPercent: 0,
                                DevisionPercent_Temp: 0,
                                NumberOfLabor: 0,
                                Note: '',
                                OrderIndex: LinePosition.Details.length + 1,
                            }
                            LinePosition.Details.push(obj);
                            var name = LinePosition.EmployeeName + '' == 'null' ? '...  Chọn  Nhân Viên  ...' : LinePosition.EmployeeName;
                            ul += '</table>';
                            tb2 += '<td class="pname" style="width:75%">' + name + '</td>';
                            tb2 += '<td class="tmu" style="width:12%;">' + Math.round(tongtmu) + '</td>';
                            tb2 += '<td class="labor" style="width:12%; border-right:none">' + Math.round(numOfLabor * 10) / 10 + '</td>';
                            tb2 += '</tr></table>';
                            if (LinePosition.IsHasBTP)
                                BTP = true;
                            Global.Data.Position_Arr.push({
                                Id: LinePosition.Id,
                                TechProVer_Id: LinePosition.TechProVer_Id,
                                OrderIndex: LinePosition.OrderIndex,
                                EmployeeId: LinePosition.EmployeeId,
                                EmployeeName: LinePosition.EmployeeName,
                                IsHasExitLine: LinePosition.IsHasExitLine,
                                IsHasBTP: LinePosition.IsHasBTP,
                                Details: LinePosition.Details
                            });
                            return false;
                        }
                    });
                    if (ul == '' && tb2 == '') {
                        tb2 = 'Chọn Nhân Viên ...';
                    }
                    str += '  <div class="line-center" style ="background-image: linear-gradient( #FFD700,#FFD700,#FFD700) !important;' + (i == 0 ? "border-top:1px solid #ccc" : "") + '' + (i == 8 ? "border-bottom:1px solid #ccc" : "") + '"></div>';
                    str += '      <div id="' + y + '" class="line-right">';
                    str += '          <div class="child">';
                    str += '              <div class="equipment"><div style="transform:rotate(-90deg); -webkit-transform:rotate(-90deg); -ms-transform:rotate(-90deg); max-width:50px ">' + equip + '</div></div>';
                    str += '              <div class="main-info c-right"  >';
                    str += '                  <div id="' + y + '" class="line-child-box">';
                    str += '                  <div class="top"> ';
                    str += '                      <div id="phase">' + ul + '</div>';
                    str += '                      <div style="clear: both"></div>';
                    str += '                  </div>';
                    str += '                  <div><div class="col-md-1"><input type="text" linePo_' + y + ' onchange="ChangeIndex(' + y + ')" value="' + y + '"/></div><div class="col-md-11 bottom bold red" style="background-image: linear-gradient( rgb(247, 234, 236),rgb(255, 236, 239),rgb(255, 236, 239)) !important ;" onclick="ChooseEmployee(\'' + y + '\')" data-toggle="modal" data-target="#' + Global.Element.Popup_position + '">' + tb2 + '</div><div class="clearfix"></div></div>';
                    str += '              </div>';
                    str += '          </div>';
                    str += '          <div title="Click Tạo đường vào BTP ở Vị Trí này." class="insert-btp ' + (BTP ? "insert-hover" : "") + '" onclick=insert_BTP(\'' + y + '\')></div>';
                    str += '          <div style="clear: left"></div>';
                    str += '          <div class="exit e-right" style="background-image: linear-gradient( #0000FF,#0000FF,#0000FF) !important;">Lối Đi</div>';
                    str += '      </div>';
                    str += '  </div>';
                    str += '  <div style="clear: left"></div>';
                    str += '</div>';
                    $('#line-box').html(str + $('#line-box').html());
                }
                Global.Data.Index = 0;
                ResetSize();
            }
        }
    }


    /*************************************************************************************************************/
    function ResetSize() {
        if (Global.Data.Index == 0) {
            for (var i = 1, y = i + 1; i <= (Global.Data.Position_Arr.length); i += 2, y += 2) {
                var left = $($('#' + i).parent().find('.line-left')).find('.main-info #phase').height();
                var right = $($('#' + y).parent().find('.line-right')).find('.main-info #phase').height();
                if (left > right) {
                    var height_b = $($('#' + i).parent().find('.line-left')).find('.main-info').height();
                    $($('#' + i).parent().find('.line-left')).find('.main-info #phase').css('min-height', left);
                    $($('#' + y).parent().find('.line-right')).find('.main-info #phase').css('min-height', left);
                    $('#' + i).parent().find('.equipment').css('height', height_b);
                    $('#' + i).parent().find('.equipment div').css('margin', (height_b / 3) + 'px 0 0 0');
                    $('#row_' + i).find('.line-center').css('min-height', $('#' + i).parent().find('.line-left').css('height'));
                }
                else {
                    var height_b = $($('#' + y).parent().find('.line-right')).find('.main-info').height();
                    $($('#' + i).parent().find('.line-left')).find('.main-info #phase').css('min-height', right);
                    $($('#' + y).parent().find('.line-right')).find('.main-info #phase').css('min-height', right);
                    $('#' + i).parent().find('.equipment').css('height', height_b);
                    $('#' + i).parent().find('.equipment div').css('margin', (height_b / 3) + 'px 0 0 0');
                    $('#row_' + i).find('.line-center').css('min-height', $('#' + i).parent().find('.line-right').css('height'));
                }
                $.each(Global.Data.Position_Arr, function (ii, item) {
                    if (item.OrderIndex == i && item.IsHasExitLine) {
                        insert_Exit(i);
                        return false;
                    }
                });
            }
        }
        else {
            var left = $($('#' + Global.Data.Index).parent().find('.line-left')).find('.main-info #phase').height();
            var right = $($('#' + Global.Data.Index).parent().find('.line-right')).find('.main-info #phase').height();
            if (left > right) {
                var height_b = $($('#' + Global.Data.Index).parent().find('.line-left')).find('.main-info').height();
                $($('#' + Global.Data.Index).parent().find('.line-left')).find('.main-info #phase').css('min-height', left);
                $($('#' + Global.Data.Index).parent().find('.line-right')).find('.main-info #phase').css('min-height', left);
                $('#' + Global.Data.Index).parent().find('.equipment').css('height', height_b);
                $('#' + Global.Data.Index).parent().find('.equipment div').css('margin', (height_b / 3) + 'px 0 0 0');
                $('#row_' + Global.Data.Index).find('.line-center').css('min-height', $('#' + Global.Data.Index).parent().find('.line-left').css('height'));
            }
            else {
                var height_b = $($('#' + Global.Data.Index).parent().find('.line-right')).find('.main-info').height();
                $($('#' + Global.Data.Index).parent().find('.line-left')).find('.main-info #phase').css('min-height', right);
                $($('#' + Global.Data.Index).parent().find('.line-right')).find('.main-info #phase').css('min-height', right);
                $('#' + Global.Data.Index).parent().find('.equipment').css('height', height_b);
                $('#' + Global.Data.Index).parent().find('.equipment div').css('margin', (height_b / 3) + 'px 0 0 0');
                $('#row_' + Global.Data.Index).find('.line-center').css('min-height', $('#' + Global.Data.Index).parent().find('.line-right').css('height'));
            }
        }
    }

    function insert_Exit(id) {
        if ($($('#' + id).find('.exit')).css('display') == 'block') {
            $($('#' + id).parent().find('.exit')).hide();
            $.each(Global.Data.Position_Arr, function (ii, diagram) {
                if (parseInt(id) == diagram.OrderIndex) {
                    diagram.IsHasExitLine = false;
                    $($('#' + id).parent().find('.insert-exit')).removeClass('insert-hover');
                    return false;
                }
            });
        }
        else {
            $($('#' + id).parent().find('.exit')).show();
            $.each(Global.Data.Position_Arr, function (ii, diagram) {
                if (parseInt(id) == diagram.OrderIndex) {
                    diagram.IsHasExitLine = true;
                    $($('#' + id).parent().find('.insert-exit')).addClass('insert-hover');
                    return false;
                }
            });
        }
    }

    function insert_BTP(id) {
        var obj = $('#' + id).find('.insert-btp');
        if (obj.hasClass('insert-hover')) {
            obj.removeClass('insert-hover');
            $.each(Global.Data.Position_Arr, function (ii, diagram) {
                if (parseInt(id) == diagram.OrderIndex) {
                    diagram.IsHasBTP = false;
                    return false;
                }
            });
        }
        else {
            obj.addClass('insert-hover');
            $.each(Global.Data.Position_Arr, function (ii, diagram) {
                if (parseInt(id) == diagram.OrderIndex) {
                    diagram.IsHasBTP = true;
                    return false;
                }
            });
        }
    }

    function delete_Po(id) {
        $('#line-box').find('#row_' + id).remove();
        Global.Data.Position_Arr.splice(id - 1, 2);
        $('#tkc_sl_line').val(parseInt($('#tkc_sl_line').val()) - 2);
    }

    function ChooseEmployee(id) {
        var idd = parseInt(id);
        Global.Data.Index = idd;
        var tong = 0;

        if (Global.Data.Position_Arr[idd - 1].Details.length == 0)
            Add_Empty_Object();
        else {
            $.each(Global.Data.Position_Arr[idd - 1].Details, function (i, item) {
                var obj = {
                    Id: item.Id,
                    Line_PositionId: item.Line_PositionId,
                    IsPass: item.IsPass,
                    TechProVerDe_Id: item.TechProVerDe_Id,
                    PhaseCode: item.PhaseCode,
                    PhaseName: item.PhaseName,
                    EquipmentId: item.EquipmentId,
                    EquipmentName: item.EquipmentName,
                    TotalTMU: item.TotalTMU,
                    PhaseGroupId: item.PhaseGroupId,
                    SkillRequired: item.SkillRequired,
                    TotalLabor: item.TotalLabor,
                    DevisionPercent: item.DevisionPercent,
                    DevisionPercent_Temp: item.DevisionPercent_Temp,
                    NumberOfLabor: item.NumberOfLabor,
                    Note: item.Note,
                    OrderIndex: item.OrderIndex,
                    Index: item.Index
                }
                Global.Data.Phase_Arr.push(obj);
                tong += item.NumberOfLabor;
            });
        }
        $('#total_worker').val(Math.round(tong * 100) / 100);
        $('#tkc_Employee').val(Global.Data.Position_Arr[idd - 1].EmployeeId == null ? 0 : Global.Data.Position_Arr[idd - 1].EmployeeId);
        ReloadListPhase_Arr();
    }

    function ChangePositionIndex(index) {
        var Newindex = parseInt($('[linePo_' + index + ']').val());
        var OldIndex = parseInt(index);
        if (Newindex <= 0 || Newindex >= (Global.Data.Position_Arr.length + 1)) {
            GlobalCommon.ShowMessageDialog('Số Thứ Tự vị trí phải lớn hơn 0 và nhỏ hơn ' + Global.Data.Position_Arr.length + '.',
                function () { $('[linePo_' + index + ']').val(index); }, "Số Thứ Tự vị trí không hợp lệ");
        }
        else {
            var objTemp = Global.Data.Position_Arr[OldIndex - 1];
            objTemp.OrderIndex = Newindex;
            Global.Data.Position_Arr.splice(OldIndex - 1, 1);
            Global.Data.Position_Arr.splice(Newindex - 1, 0, objTemp);
            if (Newindex < OldIndex) {
                for (var i = Newindex; i < Global.Data.Position_Arr.length; i++) {
                    Global.Data.Position_Arr[i].OrderIndex = i + 1;
                }
            }
            else {
                for (var i = 0; i < Global.Data.Position_Arr.length; i++) {
                    if (i + 1 != Newindex)
                        Global.Data.Position_Arr[i].OrderIndex = i + 1;
                }
            }

            //sorting array
            Global.Data.Position_Arr.sort(function (a, b) {
                var nameA = a.OrderIndex, nameB = b.OrderIndex;
                if (nameA < nameB)
                    return -1;
                if (nameA > nameB)
                    return 1;
                return 0;
            });

            var arr = [];
            $.each(Global.Data.Position_Arr, function (i, item) {
                arr.push(item);
            });


            DrawLinePositionWidthDetail(arr, null);
        }
    }

    /***************************************************************************************************************** 
                                                    Lưu thiết kế chuyền
     *****************************************************************************************************************/
    function SaveDiagram(obj, isSubmit) {
        $.ajax({
            url: Global.UrlAction.Save_TKC,
            type: 'post',
            data: JSON.stringify({ 'model': JSON.stringify(obj), 'isSubmit': isSubmit }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        //  GlobalCommon.ShowMessageDialog('Lưu Thành Công.', function () { }, "Thông Báo");
                        //   window.location.href = Global.UrlAction.Back;
                        $('[tkc_cancel]').click();
                        ReloadList_TKC();
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

    function GetLaborDivisionDiagramById(id, checkQTCNChange) {
        if (checkQTCNChange)
            $('[add_row],[tkc-save-draft],[tkc_save]').show();
        else
            $('[add_row],[tkc-save-draft],[tkc_save]').hide();

        $.ajax({
            url: Global.UrlAction.GetById_TKC,
            type: 'post',
            data: JSON.stringify({ 'labourId': id }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                if (result.Result == "OK") {
                    $('#tkc_id').val(result.Records.Id);
                    $('#tkc-LabourDivisionVerId').val(result.Records.LabourDivisionVerId);

                    $('#tkc_lineName').val(result.Records.LineId);
                    $('#tkc_lineName').prop('disabled', true);

                    Global.Data.TechProVerId = result.Records.TechProVer_Id;

                    //TODO  18/1/2022
                    Global.Data.CodeOption = '<option value="0" >Chọn ...</option>';
                    Global.Data.NameOption = '<option value="0" >Chọn ...</option>';
                    Global.Data.Code.length = 0;
                    Global.Data.Name.length = 0;
                    Global.Data.PhaseList.length = 0;
                    if (result.Records.TechProcess != null) {
                        $.each(result.Records.TechProcess.details, function (i, item) {
                            Global.Data.PhaseList.push(item);
                            Global.Data.CodeOption += '<option value="' + item.Id + '" >' + item.PhaseCode + '</option>';
                            Global.Data.NameOption += '<option value="' + item.Id + '" >' + item.PhaseName + '</option>';
                            Global.Data.Code.push(item.PhaseCode.trim());
                            Global.Data.Name.push(item.PhaseName.trim());
                        });
                    }

                    if (result.Records.Positions != null && result.Records.Positions.length > 0) {
                        DrawLinePositionWidthDetail(result.Records.Positions, null);
                        $('#tkc_sl_line').val(result.Records.Positions.length);
                    }

                    $('#loading').hide();
                    if (result.Records.IsTechVersionChange && checkQTCNChange)
                        GlobalCommon.ShowConfirmDialog("Thông tin quy trình công nghệ đã thay đổi. Bạn có muốn cập nhật lại thông tin cho thiết kế chuyền không ?",
                            () => {
                                //cập nhật lại thông tin công đoạn cho thiết kế chuyền
                                RefreshLaborDivisionDiagramById(id);
                            },
                            () => { }, "Cập nhật", "Không cập nhật", "Thông báo");
                }
                else
                    GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");

            }
        });
    }

    function RefreshLaborDivisionDiagramById(id) {
        $.ajax({
            url: Global.UrlAction.RefreshTKCById,
            type: 'post',
            data: JSON.stringify({ 'labourId': id }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                if (result.Result == "OK") {
                    GetLaborDivisionDiagramById(id);
                }
                else
                    GlobalCommon.ShowMessageDialog('Cập nhật thất bại', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
            }
        });
    }
    /*************************************************************************************************************/
    /*                                  DANH SACH CONG DOAN PHAN CONG TUNG VI TRI                                */
    /*************************************************************************************************************/
    function InitListPhase_Arr() {
        $('#' + Global.Element.JtablePhase_Arr).jtable({
            title: 'Danh Sách Công Đoạn Được Phân',
            pageSizeChangeSize: true,
            selectShow: true,
            actions: {
                listAction: Global.Data.Phase_Arr,
            },
            messages: {
                selectShow: 'Ẩn hiện cột',
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                //IsPass: {
                //    title: ' ',
                //    width: '3%',
                //    display: function (data) {
                //        var txt = $('<button pPass title="" class="jtable-pass-command-button jtable-command-button"><span></span></button>');
                //        if (!data.record.IsPass)
                //            txt = $('<button pPass title="" class="jtable-fail-command-button jtable-command-button"><span></span></button>');
                //        return txt;
                //    }
                //},
                PhaseCode: {
                    visibility: 'fixed',
                    title: "Mã Công Đoạn",
                    width: "20%",
                    display: function (data) {
                        var vl = data.record.PhaseCode != '' ? (data.record.PhaseCode + ' (' + data.record.PhaseName + ')') : '';
                        var txt = $('<input type="text" code_' + data.record.OrderIndex + ' list="autoCompleteSource_Code" class="form-control" techProVerId="' + data.record.TechProVerDe_Id + '" value="' + vl + '" />');
                        txt.keypress(function (evt) {
                            var charCode = (evt.which) ? evt.which : event.keyCode;
                            if (charCode == 13)
                                txt.change();
                        });
                        txt.change(function () {
                            if (txt.val() != '' && txt.val() != vl) {
                                var isExists = false;
                                var isOutOfRange = true;
                                // kt trùng
                                $.each(Global.Data.Phase_Arr, function (i, item) {
                                    if ((item.PhaseCode.trim() + ' (' + item.PhaseName + ')') == txt.val().trim()) {
                                        isExists = true;
                                        GlobalCommon.ShowMessageDialog(`Công Đoạn: <span class="red bold">${item.PhaseCode} (${item.PhaseName})</span> đã được Phân Công. Vui lòng chọn Công Đoạn khác.`, function () { }, "Thông Báo");
                                        txt.val(data.record.PhaseCode);
                                        return false;
                                    }
                                });

                                if (!isExists) {
                                    $.each(Global.Data.PhaseList, function (i, item) {
                                        if ((item.PhaseCode.trim() + ' (' + item.PhaseName + ')') == txt.val()) {
                                            if (item.De_Percent == 100) {
                                                GlobalCommon.ShowMessageDialog(`Số Lao Động của Công Đoạn : <span class="red bold">${item.PhaseCode} (${item.PhaseName})</span> đã được Phân Công hết.Vui lòng chọn Công Đoạn khác.`, function () { }, "Thông Báo");
                                                txt.val(data.record.PhaseCode);
                                                isOutOfRange = false;
                                            }
                                            else {
                                                data.record.TechProVerDe_Id = item.Id;
                                                data.record.PhaseCode = item.PhaseCode;
                                                data.record.PhaseName = item.PhaseName;
                                                data.record.SkillRequired = item.SkillRequired;
                                                data.record.PhaseGroupId = item.PhaseGroupId;
                                                data.record.TotalTMU = item.TimeByPercent;
                                                data.record.TotalLabor = item.Worker;
                                                data.record.NumberOfLabor = 0;
                                                data.record.DevisionPercent = 0;
                                                data.record.EquipmentId = item.EquipmentId;
                                                data.record.EquipmentCode = item.EquipmentCode;
                                                data.record.Index = item.Index;
                                                if (item.De_Percent == 0 || typeof (item.De_Percent) == 'undefined') {
                                                    data.record.DevisionPercent_Temp = 100;
                                                    data.record.NumberOfLabor = item.Worker;
                                                    // item.De_Percent = 100; 
                                                    // data.record.DevisionPercent = 100;
                                                }
                                                else if (((item.Worker * (100 - item.De_Percent)) / 100) < 1.2) {
                                                    data.record.DevisionPercent_Temp = 100 - item.De_Percent;
                                                    data.record.NumberOfLabor = ((item.Worker * (100 - item.De_Percent)) / 100);
                                                }

                                                if (data.record.OrderIndex == Global.Data.Phase_Arr.length)
                                                    Add_Empty_Object();
                                                ReloadListPhase_Arr();
                                                isOutOfRange = false;
                                                CalculatorWorker();
                                                $('[code_' + Global.Data.Phase_Arr.length + ']').focus();
                                            }
                                            return false;
                                        }
                                    });

                                    // thong bao loi neu ko tim thay thong tin cong doan
                                    if (isOutOfRange) {
                                        GlobalCommon.ShowMessageDialog('Không tìm thấy Thông tin của Công Đoạn "<span class="red bold">' + txt.val() + '</span>".\nVui lòng chọn Công Đoạn khác.', function () { }, "Thông Báo");
                                        txt.val(data.record.PhaseCode);
                                    }
                                }
                            }
                        });
                        // txt.click(function () { txt.select(); });
                        txt.focusout(function () {
                            txt.change();
                        });
                        txt.autocomplete({
                            source: Global.Data.Code
                        });
                        return txt;
                    }
                },
                //PhaseName: {
                //    title: 'Tên Công Đoạn',
                //    width: '15%'
                //},
                EquipmentName: {
                    title: 'Thiết Bị',
                    width: '3%',
                    display: function (data) {
                        if (data.record.EquipmentCode != null) {
                            var txt = '<span class="red  ">' + data.record.EquipmentName + '</span>';
                            return txt;
                        }
                    }
                },
                TotalTMU: {
                    title: 'Tổng TG (s)',
                    width: '5%',
                    display: function (data) {
                        var txt = '<span class="blue  ">' + Math.round(data.record.TotalTMU) + '</span>';
                        return txt;
                    }
                },
                //WorkerLevelId: {
                //    title: "Bậc thợ",
                //    width: "5%",
                //    display: function (data) {
                //        var txt = '<span class="red bold">' + data.record.WorkerLevelName + '</span>';
                //        return txt;
                //    }
                //},
                TotalLabor: {
                    title: 'Tổng LĐ có thể PC',
                    width: '5%',
                    display: function (data) {
                        var txt = '<span class="red bold">' + Math.round(data.record.TotalLabor * 1000) / 1000 + '</span>';
                        return txt;
                    }
                },
                DevisionPercent: {
                    title: 'Tỷ Lệ',
                    width: '5%',
                    display: function (data) {
                        var txt = $('<input type="text" class="form-control center" value="' + data.record.DevisionPercent_Temp + '" onkeypress=" return isNumberKey(event)"/>');
                        //txt.click(function () { txt.select(); });
                        txt.change(function () {
                            $.each(Global.Data.PhaseList, function (i, item) {
                                if (item.Id == data.record.TechProVerDe_Id) {
                                    if (parseFloat(txt.val()) > 0 && parseFloat(txt.val()) <= 100) {
                                        if ((item.De_Percent - data.record.DevisionPercent) + parseFloat(txt.val()) > 100) {
                                            GlobalCommon.ShowMessageDialog('Số Lao Động của Công Đoạn : <span class="red bold">' + item.PhaseCode + '</span> chỉ còn lại <span class="red bold">' + (100 - item.De_Percent) + '%</span>.\nVui lòng Phân Công trong khoảng cho phép.', function () { }, "Thông Báo");
                                            txt.val(data.record.DevisionPercent_Temp);
                                        }
                                        var labor = (item.Worker * parseFloat(txt.val())) / 100;
                                        data.record.NumberOfLabor = labor;
                                        data.record.DevisionPercent_Temp = parseFloat(txt.val());
                                        ReloadListPhase_Arr();
                                        CalculatorWorker();
                                    }
                                    else {
                                        GlobalCommon.ShowMessageDialog('Tỷ Lệ Phân Công phải lớn hơn <span class="red bold">0</span> và nhỏ hơn <span class="red bold">100%</span>.', function () { }, "Thông Báo");
                                        txt.val(data.record.DevisionPercent_Temp);
                                    }
                                    return false;
                                }
                            });
                        });
                        return txt;
                    }
                },
                NumberOfLabor: {
                    title: 'Số LĐ PC',
                    width: '5%',
                    display: function (data) {
                        var txt = '<span class="red bold">' + Math.round(data.record.NumberOfLabor * 1000) / 1000 + '</span>';
                        return txt;
                    }
                },
                Note: {
                    title: 'Ghi Chú',
                    width: '5%',
                    display: function (data) {
                        var txt = $('<input pNote class="form-control" type="text" value="' + data.record.Note + '"></input>');
                        txt.change(function () { data.record.Note = txt.val(); });
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
                                $.each(Global.Data.Phase_Arr, function (i, item) {
                                    if (item.TechProVerDe_Id == data.record.TechProVerDe_Id) {
                                        // tra lai data cu
                                        //$.each(Global.Data.PhaseList, function (iii, phase) {
                                        //    if (item.TechProVerDe_Id == phase.Id) {
                                        //        phase.De_Percent -= item.DevisionPercent; 
                                        //        return false;
                                        //    }
                                        //});
                                        // xoa 
                                        Global.Data.Phase_Arr.splice(i, 1);

                                        // sap xiep lai
                                        for (var y = i; y < Global.Data.Phase_Arr.length; y++) {
                                            Global.Data.Phase_Arr[y].OrderIndex = y + 1;
                                        }
                                        ReloadListPhase_Arr();
                                        $('[code_' + Global.Data.Phase_Arr.length + ']').focus();
                                        return false;
                                    }
                                });
                                CalculatorWorker();
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;
                    }
                }
            }
        });
    }

    function ReloadListPhase_Arr() {
        $('#' + Global.Element.JtablePhase_Arr).jtable('load');
    }

    function Add_Empty_Object() {
        var obj = {
            Id: 0,
            Line_PositionId: 0,
            IsPass: false,
            TechProVerDe_Id: 0,
            PhaseCode: '',
            PhaseName: '',
            EquipmentId: 0,
            EquipmentName: '',
            TotalTMU: 0,
            PhaseGroupId: 0,
            SkillRequired: 0,
            TotalLabor: 0,
            DevisionPercent: 0,
            DevisionPercent_Temp: 0,
            NumberOfLabor: 0,
            Note: '',
            OrderIndex: Global.Data.Phase_Arr.length + 1,
        }
        Global.Data.Phase_Arr.push(obj);
    }

    function CalculatorWorker() {
        var tong = 0;
        $.each(Global.Data.Phase_Arr, function (i, item) {
            tong += item.NumberOfLabor;
        });
        $('#total_worker').val(Math.round(tong * 10) / 10);
        if (tong > 1.2) {
            GlobalCommon.ShowMessageDialog('Tổng Lao Động Phân công cho Vị Trí này đã lớn hơn 1.2 Lao Động.', function () { }, "Thông Báo");
        }
    }

    function Add_PhaseIntoPosition() {
        var ul = '<table>';
        var equip = '';
        var tongtmu = 0;
        var numOfLabor = 0;
        var EQUIP = [];
        $.each(Global.Data.Position_Arr[Global.Data.Index - 1].Details, function (i, item) {
            if (i < (Global.Data.Position_Arr[Global.Data.Index - 1].Details.length - 1)) {
                ul += '<tr>';
                ul += '<td class="code">' + item.PhaseCode + '</td>';
                ul += '<td class="pname">' + item.PhaseName + '</td>';
                ul += '<td class="tmu ">' + Math.round((item.TotalTMU)) + '</td>';
                ul += '<td class="per blue">' + Math.round((item.DevisionPercent)) + '</td>';
                ul += '<td class="labor red">' + Math.round(item.NumberOfLabor * 100) / 100 + '</td>';
                ul += '</tr>';
                if (EQUIP.length > 0 && item.EquipmentName != '') {
                    var flag = false;
                    $.each(EQUIP, function (e, E) {
                        if (E == item.EquipmentName.trim()) {
                            flag = true;
                            return false;
                        }
                    });
                    if (!flag) {
                        equip += item.EquipmentName.trim() + '<br/>';
                        EQUIP.push(item.EquipmentName.trim());
                    }
                }
                else {
                    if (item.EquipCode != '') {
                        equip += item.EquipmentName + '<br/>';
                        EQUIP.push(item.EquipmentName);
                    }
                }
                tongtmu += Math.round(item.TotalTMU);
                numOfLabor += item.NumberOfLabor;
            }
        });
        ul += '</table>';
        var tb2 = '<table style="width:100%"><tr>';
        tb2 += '<td class="pname" style="width:75%">' + Global.Data.Position_Arr[Global.Data.Index - 1].EmployeeName + '</td>';
        tb2 += '<td class="tmu" style="width:12%">' + Math.round((tongtmu)) + '</td>';
        tb2 += '<td class="labor" style="width:12%; border-right:none">' + Math.round(numOfLabor * 100) / 100 + '</td>';
        tb2 += '</tr><table>';
        $('#' + Global.Data.Index).find('#phase').html(ul);
        $('#' + Global.Data.Index).find('.equipment div').html(equip);
        $('#' + Global.Data.Index).find('.bottom').html(tb2);
        ResetSize();
    }

    function GetEmployeeWithSkill(controlName) {
        $('#' + controlName).empty();
        if ($('#tkc_lineName').val() != null) {
            $.ajax({
                url: '/Employee/GetEmployWithSkill',
                type: 'post',
                data: JSON.stringify({ 'lineId': $('#tkc_lineName').val() }),
                contentType: 'application/json',
                beforeSend: function () { $('#loading').show(); },
                success: function (result) {
                    $('#loading').hide();
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result.Result == "OK") {
                            var option = '<option value="0" >Không Có Dữ Liệu Nhân Viên</option>';
                            Global.Data.EmployeeArr.length = 0;
                            if (result.Data.length > 0) {
                                option = '';
                                $.each(result.Data, function (i, item) {
                                    Global.Data.EmployeeArr.push(item);
                                    option += '<option value="' + item.EmployeeId + '" >' + item.EmployeeName + ' (' + item.EmployeeCode + ')</option>';
                                });
                                $('#' + controlName).append(option);

                            }
                        }
                        else
                            GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                    }, false, '', true, true, function () {
                        var msg = GlobalCommon.GetErrorMessage(result);
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                    });
                }
            });
        }

    }

    //#endregion
}
