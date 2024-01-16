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
GPRO.namespace('TKCInsertPhaseGroup');
GPRO.TKCInsertPhaseGroup = function () {
    var Global = {
        UrlAction: {
            Insert: '/TKCInsert/InsertPhaseGroupProduction',
            Gets: '/TKCInsert/GetPhaseGroupProductionInDay',
        },
        Element: {
            Jtable: 'jtable-phase-group-production'
        },
        Data: {
            CommoId: 0,
            PhaseGroupId: 0,
            disabled: true,
            isReset: false
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitJtable();
        ResetInfo();
        GetProAnaSelect('tkc-insert-product-pg', 1, 0);
        $('#today-date-pg').html(moment().format('D/M/YYYY'));
        $('#btn-insert-pg').hide();

    }


    var RegisterEvent = function () {

        $('#re-tkc-insert-product-pg').click(() => {
            GetProAnaSelect('tkc-insert-product-pg', 1, 0);
        })
        $('#tkc-insert-product-pg').change(() => {
            GetProAnaSelect('tkc-insert-workshop-pg', 2, $('#tkc-insert-product-pg').val());
            ResetInfo();
        })

        $('#re-tkc-insert-workshop-pg').click(() => {
            GetProAnaSelect('tkc-insert-workshop-pg', 2, $('#tkc-insert-product-pg').val());
        });

        $('#tkc-insert-workshop-pg').change(() => {
            $('#re-tkc-insert-phase-group').click();
        })

        $('#re-tkc-insert-phase-group').click(() => {
            if ($('#tkc-insert-workshop-pg').val() != undefined) {
                Global.Data.isReset = true;
                GetProAnaSelect('tkc-insert-phase-group', 6, $('#tkc-insert-workshop-pg').val());
            }
        })


        $('#tkc-insert-phase-group').change(() => {
            if (Global.Data.isReset) {
                Global.Data.isReset = false;
                $('#tkc-insert-phase-group').val(Global.Data.CommoId);
            }
            if ($('#tkc-insert-phase-group').val() != undefined) {
                $('#btn-insert-pg').show();
                $('#total-quantities-pg').html($('#tkc-insert-phase-group option:selected').attr('quantities'));
                Global.Data.PhaseGroupId = parseInt($('#tkc-insert-phase-group option:selected').attr('objId'));
                Global.Data.CommoId = parseInt($('#tkc-insert-phase-group').val());
                ReloadJtable();
            }
        });



        $('#btn-insert-pg').click(function () {
            InsertProduction();
        })

        $('#btn-clear-pg').click(function () {
            $('#quantities-pg').val(0);
            $('#command-type-pg').val(1)
        })

    }

    function InsertProduction() {

        var obj = {
            Date: $('#today-date-pg').html(),
            ComAnaId: Global.Data.CommoId,
            PhaseGroupId: Global.Data.PhaseGroupId,
            ComandType: $('#command-type-pg').val(),
            Quantities: $('#quantities-pg').val()
        }

        $.ajax({
            url: Global.UrlAction.Insert,
            type: 'POST',
            data: JSON.stringify({ 'model': obj }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        $('#re-tkc-insert-phase-group').click();
                        $('#quantities-pg').val(0);
                        $('#command-type-pg').val(1);
                    }
                }, false, '', true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    ResetInfo = () => {
        $('#tkc-insert-workshop-pg').empty().append(` <option value="0">Không có phân xưởng sản xuất</option>`);;
        $('#tkc-insert-line-phase-group').empty().append(` <option value="0">Không có cụm công đoạn sản xuất</option>`);
        $('#btn-insert-pg').hide();
        $('#quantities-pg').val(0);
        $('#command-type-pg').val(1);
    }

    function InitJtable() {
        $('#' + Global.Element.Jtable).jtable({
            title: 'Năng xuất trong ngày',
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
                CreatedDate: {
                    visibility: 'fixed',
                    title: "Ngày",
                    width: "15%",
                    display: (data) => {
                        return moment(data.record.CreatedDate).format('D/M/YYYY HH:mm a');
                    }
                },
                CreatedUser: {
                    title: "Người thực hiện",
                    width: "15%",
                    display: (data) => {
                        return data.record.UserName;
                    }
                },

                ComandType: {
                    title: "Hình thức",
                    width: "10%",
                    display: (data) => {
                        if (data.record.ComandType == 1)
                            return 'Tăng';
                        return '<span class="red">Giảm</span>';
                    }
                },
                Quantities: {
                    title: "Năng xuất",
                    width: "10%",
                    sorting: false,
                },
            }
        });
    }

    function ReloadJtable() {
        $('#' + Global.Element.Jtable).jtable('load', { 'date': $('#today-date-pg').html(), 'commoId': Global.Data.CommoId, phaseGroupId: Global.Data.PhaseGroupId });
    }

}
$(document).ready(function () {
    var obj = new GPRO.TKCInsertPhaseGroup();
    obj.Init();
});