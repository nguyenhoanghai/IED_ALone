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
GPRO.namespace('EmployeeProductionInDay');
GPRO.EmployeeProductionInDay = function () {
    var Global = {
        UrlAction: { 
            Gets: '/TKCInsert/GetReportEmployeeProductionDetail',
            Excel: '/TKCInsert/ExportReportEmployeeProductionDetail'
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
            window.location.href = Global.UrlAction.Excel + `?labourVerId=${$('#report-line').val()}&employId=${$('#report-employee').val()}&date=${$('#today-date').html()}&ename=${$('#report-employee option:selected').text()}`
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
                Date: {
                    title: "Ngày",
                    width: "10%",
                    sorting: false,
                },
                Name: {
                    visibility: 'fixed',
                    title: "Công đoạn",
                    width: "15%",
                },
                Total: {
                    title: "Sản lượng",
                    width: "10%",
                    sorting: false,
                } 
            }
        });
    }

    function ReloadJtable() {
        $('#' + Global.Element.Jtable).jtable('load', { 'labourVerId': $('#report-line').val(), 'employId': $('#report-employee').val(), 'date': $('#today-date').html() });
    }


    
}
$(document).ready(function () {
    var obj = new GPRO.EmployeeProductionInDay();
    obj.Init();
});