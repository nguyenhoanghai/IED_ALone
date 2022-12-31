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
GPRO.namespace('WorkerBalance');
GPRO.WorkerBalance = function () {
    var Global = {
        UrlAction: {
            GetById: '/TKCInsert/GetLabourDevisionVer',
            Excel: '/TKCInsert/ExportReportWorkerBalance',
            Get: '/TKCInsert/GetReportWorkerBalance',
        },
        Element: {

        },
        Data: {

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
        GetTKCOfLineSelect('worker-balance-line');
        $('#today-date').html(moment().format('D/M/YYYY'));
        $('#btn-insert').hide();
    }


    var RegisterEvent = function () {

        $('#re-worker-balance-line').click(() => {
            ResetInfo();
            GetTKCOfLineSelect('worker-balance-line');
        })

        $('#worker-balance-line').change(() => {
            if ($('#worker-balance-line').val() != undefined) {
                GetLaborDivisionDiagramById();
                //  GetLinePosition($('#worker-balance-line').val());
            }
        });


        $('#btn-worker-balance-excel').click(function () {
            var url = Global.UrlAction.Excel + `?labourId=${$('#worker-balance-line').val()}&product=${$('#worker-balance-product option:selected').text()}`;
            console.log(url)
            window.location.href = url;
        })

        $('#btn-clear').click(function () {
            $('#quantities').val(0);
            $('#command-type').val(1)
        })

    }

    function GetLaborDivisionDiagramById() {
        $.ajax({
            url: Global.UrlAction.Get,
            type: 'post',
            data: JSON.stringify({ 'labourVerId': $('#worker-balance-line').val() }),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                if (result.Result == "OK") {

                    if (result.Records.TechProcess != null) {
                        var techPro = result.Records.TechProcess;
                        $('#create-date').html(moment(techPro.CreatedDate).format('D/M/YYYY hh:mm a'));
                        $('#time-per-commo').html(Math.round(techPro.TimeCompletePerCommo * 1000) / 1000);
                        $('#pro-per-person').html(Math.round(techPro.ProOfPersonPerDay * 1000) / 1000);
                        $('#pro-group-per-hour').html(Math.round(techPro.ProOfGroupPerHour * 1000) / 1000);
                        $('#pro-group-per-day').html(Math.round(techPro.ProOfGroupPerDay * 1000) / 1000);
                        $('#paced-product').html(Math.round(techPro.PacedProduction * 1000) / 1000);
                        $('#workers').html(techPro.NumberOfWorkers);
                        $('#time-per-day').html(techPro.WorkingTimePerDay);
                        $('#note').html(techPro.Note);
                    }

                    var _table = $('#table-worker-balance tbody');
                    _table.empty();
                    if (result.Records.Positions != null && result.Records.Positions.length > 0) {
                        var positions = result.Records.Positions;
                        for (var i = 0; i < positions.length; i++) {
                            var phases = positions[i].Details;
                            var tr = $('<tr></tr>');
                            tr.append(`<td class='text-center'  rowspan='${phases && phases.length > 0 ? phases.length : 1} '>${positions[i].OrderIndex}</td>`);
                            tr.append(`<td class='text-center'  rowspan='${phases && phases.length > 0 ? phases.length : 1} '>${(positions[i].EmployeeId ? positions[i].EmployeeName : '-')}</td>`);
                            if (phases && phases.length > 0) {

                                for (var ii = 0; ii < phases.length; ii++) {
                                    var _tmu = Math.round(phases[ii].TotalTMU);
                                    var _total_15HP = Math.round(_tmu + (_tmu > 0 ? (_tmu * 15) / 100 : 0));
                                    var _dmHour = (_total_15HP > 0 ? (3600 / _total_15HP) : 0);
                                    var _theopc = Math.round(_dmHour > 0 ? (_dmHour * phases[ii].DevisionPercent) / 100 : 0);

                                    var _timeNeed = (_dmHour > 0 ? (_dmHour * (_total_15HP / 3600)) : 0);



                                    if (ii > 0)
                                        tr = $('<tr></tr>');
                                    tr.append(`<td class='text-center' >${phases[ii].PhaseCode}</td>`);
                                    tr.append(`<td>${phases[ii].PhaseName}</td>`);
                                    tr.append(`<td class='text-center' >${_tmu.toFixed(0)}</td >`);
                                    tr.append(`<td class='text-center' >${_total_15HP.toFixed(0)}</td >`);
                                    tr.append(`<td class='text-center' >${_theopc.toFixed(0)}</td >`);
                                    tr.append(`<td class='text-center' >${_timeNeed.toFixed(2)}</td>`);
                                    tr.append(`<td class='text-center' >${_timeNeed > 1 ? 0 : (1 - _timeNeed).toFixed(2)}</td>`);

                                    _table.append(tr);
                                }
                            }
                            else {
                                tr.append(`<td>-</td><td>-</td><td>-</td><td>-</td><td>-</td><td>-</td><td>-</td>`);
                                _table.append(tr);
                            }
                        }
                    }
                    else
                        _table.append(`<tr><td>Không có dữ liệu</td></tr>`);
                }
                else
                    GlobalCommon.ShowMessageDialog('', function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
            }
        });
    }


    ResetInfo = () => {
        $('#worker-balance-workshop').empty().append(` <option value="0">Không có phân xưởng sản xuất</option>`);;
        $('#worker-balance-line').empty().append(` <option value="0">Không có thiết kế chuyền</option>`);;
        $('#worker-balance-line-position').empty().append(` <option value="0">Không có vị trí sản xuất</option>`);;
        $('#worker-balance-line-position-phase').empty().append(` <option value="0">Không có công đoạn sản xuất</option>`);
        $('#btn-insert').hide();
        $('#create-date,#note').html('');
        $('#time-per-commo,#pro-per-person,#pro-group-per-hour,#pro-group-per-day,#paced-product,#workers,#time-per-day,#total').html('0');

        $('#line-box').empty();
    }
}
$(document).ready(function () {
    var obj = new GPRO.WorkerBalance();
    obj.Init();
});