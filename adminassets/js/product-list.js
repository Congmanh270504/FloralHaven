"use strict";
var table = null;
$(function () {
    let e, s, a;
    a = isDarkStyle ? config.colors_dark.headingColor : config.colors.headingColor;
    e = isDarkStyle ? config.colors_dark.borderColor : config.colors.borderColor;
    s = isDarkStyle ? config.colors_dark.bodyBg : config.colors.bodyBg;
    var t,
        n = $(".datatables-products");
    n.length &&
        ((t = n.DataTable({
            serverSide: true,
            processing: true,
            orderMulti: false,
            ajax: {
                url: "/Product/GetProducts",
                type: "POST",
                dataType: "json",
            },
            columns: [{ data: "id" }, { data: "product_name" }, { data: "category" }, { data: "stock" }, { data: "sku" }, { data: "price" }, { data: "saleprice" }, { data: "" }],
            columnDefs: [
                {
                    className: "control",
                    searchable: !1,
                    orderable: !1,
                    responsivePriority: 2,
                    targets: 0,
                    render: function (t, e, s, a) {
                        return "";
                    },
                },
                {
                    targets: 1,
                    responsivePriority: 1,
                    render: function (t, e, s, a) {
                        var n = s.product_name,
                            i = s.id,
                            o = s.handle,
                            c = s.image;
                        return '<div class="d-flex justify-content-start align-items-center product-name"><div class="avatar-wrapper"><div class="avatar avatar me-4 rounded-2 bg-label-secondary">' + (c ? '<img loading="lazy" src="' + c + '" alt="Product-' + i + '" class="rounded-2">' : '<span class="avatar-initial rounded-2 bg-label-' + ["success", "danger", "warning", "info", "dark", "primary", "secondary"][Math.floor(6 * Math.random())] + '">' + (c = (((c = (n).match(/\b\w/g) || []).shift() || "") + (c.pop() || "")).toUpperCase()) + "</span>") + '</div></div><div class="d-flex flex-column"><h6 class="text-nowrap mb-0">' + n + '</h6><small class="text-truncate d-none d-sm-block">' + o + "</small></div></div>";
                    },
                },
                {
                    targets: 2,
                    responsivePriority: 5,
                    render: function (t, e, s, a) {
                        return "<span>" + s.category + "</span>";
                    },
                },
                {
                    targets: 3,
                    responsivePriority: 3,
                    render: function (t, e, s, a) {
                        return "<span>" + s.stock + "</span>";
                    },
                },
                {
                    targets: 4,
                    render: function (t, e, s, a) {
                        return "<span>" + s.sku + "</span>";
                    },
                },
                {
                    targets: 5,
                    render: function (t, e, s, a) {
                        return "<span>" + s.price + "</span>";
                    },
                },
                {
                    targets: 6,
                    responsivePriority: 4,
                    render: function (t, e, s, a) {
                        return "<span>" + s.saleprice + "</span>";
                    },
                },
                {
                    targets: -1,
                    title: "Actions",
                    searchable: !1,
                    orderable: !1,
                    render: function (t, e, s, a) {
                        return '<div class="d-inline-block text-nowrap"><a href="/Product/Edit/' + s.id + '" class="btn btn-sm btn-icon btn-text-secondary rounded-pill waves-effect waves-light"><i class="ti ti-edit ti-md"></i></a><a href="/Product/AddImages/' + s.id + '" class="btn btn-sm btn-icon btn-text-secondary rounded-pill waves-effect waves-light"><i class="ti ti-photo-edit ti-md"></i></a><button class="btn btn-sm btn-icon btn-text-secondary rounded-pill waves-effect waves-light dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical ti-md"></i></button><div class="dropdown-menu dropdown-menu-end m-0"><a href="/Product/' + s.handle + '" class="dropdown-item">View</a><a href="/Product/Delete/' + s.id + '" class="dropdown-item">Delete</a></div></div>';
                    },
                },
            ],
            order: [1, "asc"],
            dom: '<"card-header d-flex border-top rounded-0 flex-wrap py-0 flex-column flex-md-row align-items-start"<"me-5 ms-n4 pe-5 mb-n6 mb-md-0"f><"d-flex justify-content-start justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex flex-column align-items-start align-items-sm-center justify-content-sm-center pt-0 gap-sm-4 gap-sm-0 flex-sm-row"lB>>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
            lengthMenu: [7, 10, 20, 50, 70, 100],
            language: { sLengthMenu: "_MENU_", search: "", searchPlaceholder: "Search Product", info: "Displaying _START_ to _END_ of _TOTAL_ entries", paginate: { next: '<i class="ti ti-chevron-right ti-sm"></i>', previous: '<i class="ti ti-chevron-left ti-sm"></i>' } },
            buttons: [
                {
                    extend: "collection",
                    className: "btn btn-label-secondary dropdown-toggle me-4 waves-effect waves-light",
                    text: '<i class="ti ti-upload me-1 ti-xs"></i>Export',
                    buttons: [
                        {
                            extend: "print",
                            text: '<i class="ti ti-printer me-2" ></i>Print',
                            className: "dropdown-item",
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("product-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                            customize: function (t) {
                                $(t.document.body).css("color", a).css("border-color", e).css("background-color", s), $(t.document.body).find("table").addClass("compact").css("color", "inherit").css("border-color", "inherit").css("background-color", "inherit");
                            },
                        },
                        {
                            extend: "csv",
                            text: '<i class="ti ti-file me-2" ></i>Csv',
                            className: "dropdown-item",
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("product-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                        },
                        {
                            extend: "excel",
                            text: '<i class="ti ti-file-export me-2"></i>Excel',
                            className: "dropdown-item",
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("product-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                        },
                        {
                            extend: "pdf",
                            text: '<i class="ti ti-file-text me-2"></i>Pdf',
                            className: "dropdown-item",
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("product-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                        },
                        {
                            extend: "copy",
                            text: '<i class="ti ti-copy me-2"></i>Copy',
                            className: "dropdown-item",
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("product-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                        },
                    ],
                },
                {
                    text: '<i class="ti ti-plus me-0 me-sm-1 ti-xs"></i><span class="d-none d-sm-inline-block">Add Product</span>',
                    className: "add-new btn btn-primary ms-2 ms-sm-0 waves-effect waves-light",
                    action: function () {
                        window.location.href = "/Product/Create";
                    },
                },
            ],
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.modal({
                        header: function (t) {
                            return "Details of " + t.data().product_name;
                        },
                    }),
                    type: "column",
                    renderer: function (t, e, s) {
                        s = $.map(s, function (t, e) {
                            return "" !== t.title ? '<tr data-dt-row="' + t.rowIndex + '" data-dt-column="' + t.columnIndex + '"><td>' + t.title + ":</td> <td>" + t.data + "</td></tr>" : "";
                        }).join("");
                        return !!s && $('<table class="table"/><tbody />').append(s);
                    },
                },
            },
            initComplete: function () {

            },
        })),
            $(".dataTables_length").addClass("mx-n2"),
            $(".dt-buttons").addClass("d-flex flex-wrap mb-6 mb-sm-0")),
        $(".datatables-products tbody").on("click", ".delete-record", function () {
            t.row($(this).parents("tr")).remove().draw();
        }),
        setTimeout(() => {
            $(".dataTables_filter .form-control").removeClass("form-control-sm"), $(".dataTables_length .form-select").removeClass("form-select-sm");
        }, 300);
});
