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
GPRO.namespace('Product');
GPRO.Product = function () {
    var Global = {
        UrlAction: {
            GetListProduct: '/Product/Gets',
            SaveProduct: '/Product/Save',
            DeleteProduct: '/Product/Delete',
            //GetProDeGroup: '/Product/GetProductDetailGroupByProductId',
            //SaveProDeGroup: '/Product/SaveProductDetailGroup',
            //DeleteProDeGroup: '/Product/DeleteProductDetailGroup',
        },
        Element: {
            JtableProduct: 'jtableProduct',
            PopupProduct: 'popup_Product',
            Search: 'pSearch_Popup',
            ProDeGroupView_Popup: 'ProDeGroupView_Popup',
            ProDeGroupAdd_Popup: 'ProDeGroupAdd_Popup'
        },
        Data: {
            ModelProduct: {},
            ModelProDeGroup: {},
            ProductId: 0,
            ProductName: '',
            ParentID: 0,
            Node: '',
            treeSelectId: 0,
            ProDeGroupList: [],
            ProDeGroupId: 0,
            IsInsert : true
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitList();
        ReloadList();
        InitPopup();
        BindData(null);
    }

    this.reloadListProduct = function () {
        ReloadListProduct();
    }

    this.initViewModel = function (Product) {
        InitViewModel(Product);
    }

    this.bindData = function (Product) {
        BindData(Product);
    }

    var RegisterEvent = function () {
        $("#proIsPrivate").kendoMobileSwitch({
            onLabel: "Tất cả",
            offLabel: "Nội bộ"
        });

        $('#' + Global.Element.PopupProduct).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupProduct.toUpperCase());
        });
        $('#' + Global.Element.Search).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Search.toUpperCase());
        });
        $('[pcancel]').click(function () {
            BindData(null); 
            $('#pdes').val('');
        });

        $('[pclose]').click(function () {
            $('#psearchBy').val('1');
            $('#pkeyword').val('');
            $('div.divParent').attr('currentPoppup', '');
        });

        $('[psearch]').click(function () { 
                ReloadList();
                $('#pkeyword').val('');
                $('[pclose]').click();
        });
         
    }

    //-- product type
    function InitViewModel(Product) {
        var switchInstance = $("#proIsPrivate").data("kendoMobileSwitch");
        var ProductViewModel = {
            Id: 0,
            Name: '',
            Code: '',
            Description: '',
            CTBTP: '',
            IsPrivate: false
        };
        switchInstance.check(true);
        if (Product != null) {
            ProductViewModel = {
                Id: ko.observable(Product.Id),
                Name: ko.observable(Product.Name),
                Code: ko.observable(Product.Code),
                Description: ko.observable(Product.Description),
                CTBTP: ko.observable(Product.CTBTP),
                IsPrivate: ko.observable(Product.IsPrivate),
            };
            switchInstance.check(Product.IsPrivate)
        }
        return ProductViewModel;
    }

    function BindData(Product) {
        Global.Data.ModelProduct = InitViewModel(Product);
        ko.applyBindings(Global.Data.ModelProduct, document.getElementById(Global.Element.PopupProduct));
    }

    function SaveProduct() {
        Global.Data.ModelProduct.IsPrivate = $("#proIsPrivate").data("kendoMobileSwitch").check();
        $.ajax({
            url: Global.UrlAction.SaveProduct,
            type: 'post',
            data: ko.toJSON(Global.Data.ModelProduct),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        ReloadList(); 
                        BindData(null);
                        $('#pdes').val('');

                        if (!Global.Data.IsInsert) {
                            $("#" + Global.Element.PopupProduct + ' button[pcancel]').click();
                            $('div.divParent').attr('currentPoppup', '');
                        }
                        Global.Data.IsInsert = true;
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

    function InitList () {
        $('#' + Global.Element.JtableProduct).jtable({
            title: 'Quản lý sản phẩm',
            paging: true,
            pageSize: 50,
            pageSizeChange: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetListProduct,
                createAction: Global.Element.PopupProduct,
                createObjDefault: InitViewModel(null),
                searchAction: Global.Element.Search,
            },
            messages: {
                addNewRecord: 'Thêm mới',
                searchRecord: 'Tìm kiếm',
                selectShow: 'Ẩn hiện cột'
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
                    title: "Tên Sản Phẩm",
                    width: "20%", 
                },
                Code: {
                    title: "Mã Sản Phẩm",
                    width: "5%",
                },
                //CTBTP: {
                //    title: 'Chi Tiết Bán Thành Phẩm',
                //    width: '10%',
                //    sorting: false,
                //    display: function (data) {
                //        var text = $('<a class="clickable red" data-toggle="modal" data-target="#' + Global.Element.ProDeGroupView_Popup + '" title="Xem Thông Tin Chi Tiết Bán Thành Phẩm">Xem Chi Tiết Bán Thành Phẩm</a>');
                //        text.click(function () {
                //            Global.Data.ProductId = data.record.Id;
                //            Global.Data.ProductName = data.record.Name;
                //            LoadProDeGroupByProTypeId(data.record.Id);
                //        });
                //        return text;
                //    }
                //},
                Description: {
                    title: "Mô Tả ",
                    width: "20%",
                    sorting: false,
                },
                edit: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var text = $('<i data-toggle="modal" data-target="#' + Global.Element.PopupProduct + '" title="Chỉnh sửa thông tin" class="fa fa-pencil-square-o clickable blue"  ></i>');
                        text.click(function () {
                            BindData(data.record); 
                            Global.Data.IsInsert = false;
                        });
                        return text;
                    }
                },
                Delete: {
                    title: '',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                }
            }
        });
    }

    function ReloadList () {
        var keySearch = $('#pkeyword').val();
        var searchBy = $('#psearchBy').val();
        $('#' + Global.Element.JtableProduct).jtable('load', { 'keyword': keySearch, 'searchBy': searchBy });
      }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteProduct,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadList();
                        BindData(null);
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
                }, false, Global.Element.PopupProduct, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }

    function InitPopup() {

        $("#" + Global.Element.PopupProduct).modal({
            keyboard: false,
            show: false
        });

        $("#" + Global.Element.PopupProduct + ' button[psave]').click(function () {
            if (CheckValidate()) {
                SaveProduct();
            }
        });
        $("#" + Global.Element.PopupProduct + ' button[pcancel]').click(function () {
            $("#" + Global.Element.PopupProduct).modal("hide");
        });
    }
     
    function CheckValidate() {
        if ($('#pname').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên Loại Sản Phẩm.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }
     
}

$(document).ready(function () {
    var Product = new GPRO.Product();
    Product.Init(); 
});
 