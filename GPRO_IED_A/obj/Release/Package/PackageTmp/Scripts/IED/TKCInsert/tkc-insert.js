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
GPRO.namespace('TKCInsert');
GPRO.TKCInsert = function () {
    var Global = {
        UrlAction: {
            GetById: '/TKCInsert/GetLastLabourDevisionVer',
        },
        Element: {

        },
        Data: {
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

            LinePositions: [],
            LabourDevision_VerId: 0,
            LinePoId: 0,
            LinePo_DetailId: 0,
            disabled: true,
            PhaseId: 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        //InitList();
        //ReloadList(); 
        //InitPopup();  
        ResetInfo();
        GetProAnaSelect('tkc-insert-product', 1, 0);
        $('#today-date').html(moment().format('D/M/YYYY'));
        $('#btn-insert').hide();
    }


    var RegisterEvent = function () {

        $('#re-tkc-insert-product').click(() => {
            GetProAnaSelect('tkc-insert-product', 1, 0);
        })

        $('#tkc-insert-product').change(() => {
            GetProAnaSelect('tkc-insert-workshop', 2, $('#tkc-insert-product').val());
            ResetInfo();
        })

        $('#re-tkc-insert-workshop').click(() => {
            GetProAnaSelect('tkc-insert-workshop', 2, $('#tkc-insert-product').val());
        });

        $('#tkc-insert-workshop').change(() => {
            $('#re-tkc-insert-line').click();
        })

        $('#re-tkc-insert-line').click(() => {
            if ($('#tkc-insert-workshop').val() != undefined)
                GetTKCLineSelect('tkc-insert-line', $('#tkc-insert-workshop').val());
        })

        $('#tkc-insert-line').change(() => {
            $('#re-tkc-insert-line-position').click();
        });

        $('#re-tkc-insert-line-position').click(() => {
            if ($('#tkc-insert-line').val() != undefined) {
                GetLaborDivisionDiagramById($('#tkc-insert-line').val());
                GetLinePosition($('#tkc-insert-line').val());
            }
        });

        $('#tkc-insert-line-position').change(() => {
            $('#tkc-insert-line-position-phase').empty();
            var _id = parseInt($('#tkc-insert-line-position').val());
            var positionObj = Global.Data.LinePositions.filter(x => x.Id === _id)[0];
            if (positionObj && positionObj.Phases && positionObj.Phases.length > 0) {
                var str = '';
                if (positionObj.Phases.length > 0) {
                    $.each(positionObj.Phases, function (index, item) {
                        str += ` <option phaseId ="${item.PhaseId}" total ="${item.Quantities}" value="${item.Id}">${item.PhaseName}</option>`;
                    });
                }
                $('#tkc-insert-line-position-phase').append(str);
                if (Global.Data.LinePo_DetailId)
                    $('#tkc-insert-line-position-phase').val(Global.Data.LinePo_DetailId).change();
                else
                    $('#tkc-insert-line-position-phase').change();
                $('#btn-insert').show();
            }
            else {
                $('#tkc-insert-line-position-phase').append(` <option value="0">Không có công đoạn</option>`);
                $('#btn-insert').hide();
            }
        });

        $('#tkc-insert-line-position-phase').change(function () {
            $('#total').html($('#tkc-insert-line-position-phase option:selected').attr('total'));
            Global.Data.PhaseId = parseInt($('#tkc-insert-line-position-phase option:selected').attr('phaseId'));
        })

        $('#btn-insert').click(function () {
            InsertProduction();
        })

        $('#btn-clear').click(function () {
            $('#quantities').val(0);
            $('#command-type').val(1)
        })

    }

    function GetLaborDivisionDiagramById(id) {
        $.ajax({
            url: Global.UrlAction.GetById,
            type: 'post',
            data: JSON.stringify({ 'labourId': id }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {

                if (result.Result == "OK") {

                    //$('#tkc_id').val(result.Records.Id);
                    //$('#tkc_lineName').val(result.Records.LineId);
                    //$('#tkc_lineName').prop('disabled', true);

                    //Global.Data.TechProVerId = result.Records.TechProVer_Id;

                    ////TODO  18/1/2022
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
                    //if (result.Records.IsTechVersionChange)
                    //    GlobalCommon.ShowConfirmDialog("Thông tin quy trình công nghệ đã thay đổi. Bạn có muốn cập nhật lại thông tin cho thiết kế chuyền không ?",
                    //        () => {
                    //            //cập nhật lại thông tin công đoạn cho thiết kế chuyền
                    //            RefreshLaborDivisionDiagramById(id);
                    //        },
                    //        () => { }, "Cập nhật", "Không cập nhật", "Thông báo");
                }
                else
                    GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");

            }
        });
    }

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
                                        ul += '<td class="psanluong"><input class="number-input" type="text" value="0" /></td>';
                                        //ul += '<td class="tmu">' + Math.round(phase.TimeByPercent) + '</td>';
                                        //ul += '<td class="per blue">' + detail.DevisionPercent + '</td>';
                                        //ul += '<td class="labor red">' + Math.round(detail.NumberOfLabor * 10) / 10 + '</td>';
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
                    //str += '         <div title="Click Tạo Lối Đi ở Vị Trí này." style="width: 22px;float: left;"> <div class="insert-exit" onclick=insert_Exit(\'' + i + '\')></div>';
                    str += '         <div title="Click Tạo Lối Đi ở Vị Trí này." style="width: 22px;float: left;"> ';
                    if (BTP)
                        str += '          <div title="Đường vào BTP ở Vị Trí này." class="insert-btp insert-hover "  ></div>';
                    else
                        str += '          <div class="none-btp"  ></div>';
                    str += '           </div>';
                    str += '          <div class="main-info c-left" >';
                    str += '              <div  class="line-child-box">';
                    str += '              <div class="top"> ';
                    str += '                  <div id="phase">' + ul + '</div>';
                    str += '                  <div style="clear: both"></div>';
                    str += '              </div>';
                    str += '              <div><div class="col-md-1 po-index">' + i + '</div><div class="col-md-11 bottom bold red" style="background-image: linear-gradient( rgb(247, 234, 236),rgb(255, 236, 239),rgb(255, 236, 239)) !important" onclick="ChooseEmployee(\'' + i + '\')" data-toggle="modal" list="autoCompleteSource" data-target="#' + Global.Element.Popup_position + '">' + tb2 + '</div><div class="clearfix"></div></div>';
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
                                        ul += '<td class="psanluong"><input type="text" /></td>';
                                        //ul += '<td class="tmu">' + Math.round(phase.TimeByPercent) + '</td>';
                                        //ul += '<td class="per blue">' + detail.DevisionPercent + '</td>';
                                        //ul += '<td class="labor red">' + Math.round(detail.NumberOfLabor * 10) / 10 + '</td>';
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
                            var name = LinePosition.EmployeeName + '' == 'null' ? '---' : LinePosition.EmployeeName;
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
                        tb2 = '---';
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
                    str += '                  <div><div class="col-md-1 po-index"> ' + y + ' </div><div class="col-md-11 bottom bold red" style="background-image: linear-gradient( rgb(247, 234, 236),rgb(255, 236, 239),rgb(255, 236, 239)) !important ;" onclick="ChooseEmployee(\'' + y + '\')" data-toggle="modal" data-target="#' + Global.Element.Popup_position + '">' + tb2 + '</div><div class="clearfix"></div></div>';
                    str += '              </div>';
                    str += '          </div>';
                    if (BTP)
                        str += '          <div title="Đường vào BTP ở Vị Trí này." class="insert-btp insert-hover " ></div>';
                    else
                        str += '          <div  class="none-btp " ></div>';
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


    function GetLinePosition(laDevisionId) {
        $('#tkc-insert-line-position').empty();
        Global.Data.LinePositions.length = 0;
        $.ajax({
            url: '/TKCInsert/GetLinePositions',
            type: 'POST',
            data: JSON.stringify({ 'laDevisionId': laDevisionId, 'date': moment().format('D/M/YYYY') }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        if (data.Data && data.Data.Positions && data.Data.Positions.length > 0) {
                            var _positions = data.Data.Positions;
                            var str = '';
                            if (_positions.length > 0) {
                                $.each(_positions, function (index, item) {
                                    Global.Data.LinePositions.push(item);
                                    str += ` <option value="${item.Id}">Vị trí ${item.Index} ${item.EmployName}</option>`;
                                });
                            }

                            $('#tkc-insert-line-position').append(str);
                            if (Global.Data.LinePoId)
                                $('#tkc-insert-line-position').val(Global.Data.LinePoId).change();
                            else
                                $('#tkc-insert-line-position').change();
                        }
                        else
                            $('#tkc-insert-line-position-phase').append(` <option value="0">Không có vị trí sản xuất</option>`);

                        if (data.Data) {
                            $('#create-date').html(moment(data.Data.CreatedDate).format('D/M/YYYY hh:mm a'));
                            $('#time-per-commo').html(Math.round(data.Data.TimeCompletePerCommo * 1000) / 1000);
                            $('#pro-per-person').html(Math.round(data.Data.ProOfPersonPerDay * 1000) / 1000);
                            $('#pro-group-per-hour').html(Math.round(data.Data.ProOfGroupPerHour * 1000) / 1000);
                            $('#pro-group-per-day').html(Math.round(data.Data.ProOfGroupPerDay * 1000) / 1000);
                            $('#paced-product').html(Math.round(data.Data.PacedProduction * 1000) / 1000);
                            $('#workers').html(data.Data.NumberOfWorkers);
                            $('#time-per-day').html(data.Data.WorkingTimePerDay);
                            $('#note').html(data.Data.Note);
                            Global.Data.LabourDevision_VerId = data.Data.Id;
                        }
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InsertProduction() {
        Global.Data.LinePoId = $('#tkc-insert-line-position').val();
        Global.Data.LinePo_DetailId = $('#tkc-insert-line-position-phase').val()
        var obj = {
            Date: $('#today-date').html(),
            LabourDevisionId: $('#tkc-insert-line').val(),
            LabourDevision_VerId: Global.Data.LabourDevision_VerId,
            PhaseId: Global.Data.PhaseId,
            LinePo_DetailId: $('#tkc-insert-line-position-phase').val(),
            ComandType: $('#command-type').val(),
            Quantities: $('#quantities').val()
        }

        $.ajax({
            url: '/TKCInsert/InsertProduction',
            type: 'POST',
            data: JSON.stringify({ 'model': obj }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        $('#re-tkc-insert-line-position').click();
                        $('#quantities').val(0);
                        $('#command-type').val(1)
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    ResetInfo = () => {
        $('#tkc-insert-workshop').empty().append(` <option value="0">Không có phân xưởng sản xuất</option>`);;
        $('#tkc-insert-line').empty().append(` <option value="0">Không có thiết kế chuyền</option>`);;
        $('#tkc-insert-line-position').empty().append(` <option value="0">Không có vị trí sản xuất</option>`);;
        $('#tkc-insert-line-position-phase').empty().append(` <option value="0">Không có công đoạn sản xuất</option>`);
        $('#btn-insert').hide();
        $('#create-date,#note').html('');
        $('#time-per-commo,#pro-per-person,#pro-group-per-hour,#pro-group-per-day,#paced-product,#workers,#time-per-day,#total').html('0');
        Global.Data.Position_Arr.length = 0;
        $('#line-box').empty();
        Global.Data.LabourDevision_VerId = 0;
    }
}
$(document).ready(function () {
    var obj = new GPRO.TKCInsert();
    obj.Init();
});