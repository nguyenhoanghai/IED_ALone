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
GPRO.namespace('WorkerBalanceRealtime');
GPRO.WorkerBalanceRealtime = function () {
    var Global = {
        UrlAction: {
            GetById: '/TKCInsert/GetLastLabourDevisionVer',
            Excel: '/TKCInsert/ExportReportWorkerBalance_Realtime',
            Get: '/TKCInsert/GetReportWorkerBalance_Realtime',
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
        $('#re-worker-balance-line').click();
        $('#today-date').html(moment().format('D/M/YYYY'));
        $('#btn-insert').hide();
    }


    var RegisterEvent = function () {
         

        $('#re-worker-balance-line').click(() => { 
                ResetInfo();
                GetTKCOfLineSelect('worker-balance-line' ); 
        })

        $('#worker-balance-line').change(() => {
            if ($('#worker-balance-line').val() != undefined) {
                GetLaborDivisionDiagramById(); 
            }
        });


        $('#btn-worker-balance-excel').click(function () {
            var url = Global.UrlAction.Excel + `?labourVerId=${$('#worker-balance-line').val()}&date=${$('#today-date').html()}`;
            console.log(url)
            window.location.href = url;
        })

        $('#btn-clear').click(function () {
            $('#quantities').val(0);
            $('#command-type').val(1)
        })

    }

    function GetLaborDivisionDiagramById(id) {
        $.ajax({
            url: Global.UrlAction.Get,
            type: 'post',
            data: JSON.stringify({ 'labourVerId': $('#worker-balance-line').val(), 'date': $('#today-date').html() }),
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
                            var phases = positions[i].Phases;
                            var tr = $('<tr></tr>');
                            tr.append(`<td class='text-center' rowspan='${phases && phases.length > 0 ? phases.length : 1} '>${positions[i].OrderIndex}</td>`);
                            tr.append(`<td class='text-center' rowspan='${phases && phases.length > 0 ? phases.length : 1} '>${(positions[i].EmployeeId ? positions[i].EmployeeName : '-')}</td>`);
                            if (phases && phases.length > 0) {

                                for (var ii = 0; ii < phases.length; ii++) { 
                                    if (ii > 0)
                                        tr = $('<tr></tr>');
                                    tr.append(`<td class='text-center'>${phases[ii].PhaseCode}</td>`);
                                    tr.append(`<td>${phases[ii].PhaseName}</td>`); 
                                    tr.append(`<td class='text-center'>${phases[ii].DM}</td >`);
                                    tr.append(`<td class='text-center'>${phases[ii].ProductInDay}</td>`);
                                    _table.append(tr);
                                }
                            }
                            else {
                                tr.append(`<td class='text-center'>-</td><td>-</td><td class='text-center'>-</td><td class='text-center'>-</td>`);
                                _table.append(tr);
                            }
                        }
                    }
                    else
                        _table.append(`<tr><td colspan="6" class="text-center">Không có dữ liệu.</td></tr>`);
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
    var obj = new GPRO.WorkerBalanceRealtime();
    obj.Init();
});