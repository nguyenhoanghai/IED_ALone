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
GPRO.namespace('EmployeeProductionReport');
GPRO.EmployeeProductionReport = function () {
    var Global = {
        UrlAction: {
            GetById: '/TKCInsert/GetLastLabourDevisionVer',
            Gets: '/TKCInsert/GetReportEmployeeProduction',
            Excel: '/TKCInsert/ExportEmployeeProduction'
        },
        Element: {
            Jtable: 'jtable-report-production'
        },
        Data: {

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
        InitJtable();
        // GetProAnaSelect('report-product', 1, 0);
        $('#re-report-line').click();
        $('#today-date').html(moment().format('D/M/YYYY'));
    }


    var RegisterEvent = function () {
        $('#re-report-line').click(() => {
            GetTKCOfLineSelect('report-line');

        }) 

        $('#report-line').change(() => {
            $('#re-report-employee').click();
        });


        $('#re-report-employee').click(() => {
            if ($('#report-line').val() != undefined) {
                GetEmployeeSelect('report-employee', $('#report-line option:selected').attr('lineId'));
            }
        });

        $('#report-employee').change(() => {
            if ($('#report-employee').val() != undefined && $('#report-employee').val() != '0')
                ReloadJtable();
        });

        $('#btn-excel').click(() => {
            window.location.href = Global.UrlAction.Excel + `?labourId=${$('#report-line').val()}&employId=${$('#report-employee').val()}&employee=${$('#report-employee option:selected').text()}`
        })

    }

    function InitJtable() {
        $('#' + Global.Element.Jtable).jtable({
            title: 'Năng xuất công đoạn',
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
                Name: {
                    visibility: 'fixed',
                    title: "Công đoạn",
                    width: "15%",
                },
                Total: {
                    title: "Năng xuất",
                    width: "10%",
                    sorting: false,
                },
                Price: {
                    title: "Đơn giá",
                    width: "10%",
                    sorting: false,
                },
                Coefficient: {
                    title: "Hệ số CĐ",
                    width: "10%",
                    sorting: false,
                },
                Salary: {
                    title: "Tổng lương",
                    width: "10%",
                    sorting: false,
                    display: (data) => {
                        return (data.record.Total * data.record.Price * data.record.Coefficient).toFixed(1);
                    }
                },
            }
        });
    }

    function ReloadJtable() {
        $('#' + Global.Element.Jtable).jtable('load', { 'labourId': $('#report-line').val(), 'employId': $('#report-employee').val() });
    }


    
}
$(document).ready(function () {
    var obj = new GPRO.EmployeeProductionReport();
    obj.Init();
});