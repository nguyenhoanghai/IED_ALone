﻿if (typeof GPRO == 'undefined' || !GPRO) {
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
            DeleteFile: '/Product/DeleteFile'
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
            IsInsert: true
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
        GetCustomerSelect('pcustomer');
        GetProductGroupSelect('pro-group-customer');
    }

    this.deleteFile = function (fileId) {
        DeleteFile(fileId);
    }

    var RegisterEvent = function () {
        $("#proIsPrivate").kendoMobileSwitch({
            onLabel: "Nội bộ",
            offLabel: "Tất cả"
        });

        $('#' + Global.Element.PopupProduct).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.PopupProduct.toUpperCase());
        });
        $('#' + Global.Element.Search).on('shown.bs.modal', function () {
            $('div.divParent').attr('currentPoppup', Global.Element.Search.toUpperCase());
        });
        $('[pcancel]').click(function () {
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

    function SaveProduct() {
        var obj = {
            Id: $('#pid').val(),
            IsPrivate: $("#proIsPrivate").data("kendoMobileSwitch").check(),
            CustomerId: $("#pcustomer").val(),
            Description: $("#pdes").val(),
            Name: $("#pname").val(),
            Code: $("#pcustomer option:selected").text(),
            ProductGroupId: $("#pro-group-customer").val(),
            Img: $('#p-file-upload').attr("newUrl")
        }
        $.ajax({
            url: Global.UrlAction.SaveProduct,
            type: 'post',
            data: ko.toJSON(obj),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                if (result.Result == "OK") {
                    ReloadList();
                    $('#pid').val(0);
                    $('#pdes').val('');
                    $('#pname').val('');
                    $('#p-file-upload').attr("newUrl", '');
                    $('#p-file-upload').val('');
                    if (!Global.Data.IsInsert) {
                        $("#" + Global.Element.PopupProduct + ' button[pcancel]').click();
                        $('div.divParent').attr('currentPoppup', '');
                    }
                    Global.Data.IsInsert = true;
                }
                else
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
            }
        });
    }

    function InitList() {
        $('#' + Global.Element.JtableProduct).jtable({
            title: 'Quản lý mã hàng',
            paging: true,
            pageSize: 1000,
            pageSizeChange: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetListProduct,
                createAction: Global.Element.PopupProduct,
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
                    title: "Tên mã hàng",
                    width: "20%",
                },
                Code: {
                    title: "Khách hàng",
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
                            $("#pcustomer").val(data.record.CustomerId);
                            $('#pro-group-customer').val(data.record.ProductGroupId);
                            $('#pid').val(data.record.Id);
                            $("#pcustomer").val(data.record.CustomerId);
                            $("#pdes").val(data.record.Description);
                            $("#pname").val(data.record.Name);

                            var switchInstance = $("#proIsPrivate").data("kendoMobileSwitch");
                            switchInstance.check(data.record.IsPrivate);
                            var imgBox = $('.img-box');
                            imgBox.empty();
                            if (data.record.Files && data.record.Files.length > 0) {
                                var files = data.record.Files;
                                for (var i = 0; i < files.length; i++) {
                                    imgBox.append(`<div class="img-item">
                                    <img src="${files[i].Code}" class="img-avatar" id="img-avatar" />
                                    <div class="delete" onClick='DeleteFile(${files[i].Value})'>Xóa ảnh</div>
                                </div> `);
                                }
                            }
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

    function ReloadList() {
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
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                if (data.Result == "OK") {
                    ReloadList();
                }
                else
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
            }
        });
    }

    function DeleteFile(fileId) {
        $.ajax({
            url: Global.UrlAction.DeleteFile,
            type: 'POST',
            data: JSON.stringify({ 'Id': fileId }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                if (data.Result == "OK") {
                    $("#" + Global.Element.PopupProduct + ' button[pcancel]').click();
                    ReloadList();
                }
                else
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xử lý.");
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
                if ($('#p-file-upload').val() != '')
                    UploadPicture("p-file-upload", 'p-file-upload');
                else
                    SaveProduct();
            }
        });
        $("#" + Global.Element.PopupProduct + ' button[pcancel]').click(function () {
            $("#" + Global.Element.PopupProduct).modal("hide");
        });

        $('#p-file-upload').select(function () {
            SaveProduct();
        });

        $('#p-btn-file-upload').click(function () {
            $('#p-file-upload').click();
        });
    }

    function CheckValidate() {
        if ($('#pname').val().trim() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên mã hàng.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

    UploadPicture = (controlId, returnId) => {
        if (window.FormData !== undefined) {
            var fileUpload = $('#' + controlId).get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            $.ajax({
                url: '/Upload/ProductFile',
                type: "POST",
                data: fileData,
                beforeSend: function () { $('#loading').show(); },
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                success: function (result) {
                    $('#loading').hide();
                    $('#' + returnId).attr("newUrl", result);
                    $('#' + returnId).select();
                },
                error: function (err) {
                    alert("Lỗi up hình : " + err.statusText);
                }
            });
        }
        else {
            alert("FormData is not supported.");
        }
    }
}


