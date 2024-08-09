"use strict";
$(function () {
    let e, s, a;
    a = (isDarkStyle ? ((e = config.colors_dark.borderColor), (s = config.colors_dark.bodyBg), config.colors_dark) : ((e = config.colors.borderColor), (s = config.colors.bodyBg), config.colors)).headingColor;
    var t,
        n = $(".datatables-customers"),
        o = $(".select2");
    o.length && (o = o).wrap('<div class="position-relative"></div>').select2({ placeholder: "United States ", dropdownParent: o.parent() }),
        n.length &&
        ((t = n.DataTable({
            serverSide: true,
            processing: true,
            orderMulti: false,
            ajax: {
                url: "/User/GetUserList",
                type: "POST",
                dataType: "json",
            },
            columns: [{ data: "" }, { data: "customer" }, { data: "order" }, { data: "total_spent" }, { data: ""}],
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
                        var n = s.firstName + " " + s.lastName,
                            o = s.email,
                            r = s.avatar;
                        return '<div class="d-flex justify-content-start align-items-center customer-name"><div class="avatar-wrapper"><div class="avatar avatar-sm me-3">' + (r ? '<img src="' + r + '" alt="Avatar" class="rounded-circle">' : '<span class="avatar-initial rounded-circle bg-label-' + ["success", "danger", "warning", "info", "dark", "primary", "secondary"][Math.floor(6 * Math.random())] + '">' + (r = (((r = (n = s.firstName + " " + s.lastName).match(/\b\w/g) || []).shift() || "") + (r.pop() || "")).toUpperCase()) + "</span>") + '</div></div><div class="d-flex flex-column"><a href="/user/details/' + s.id + '" class="text-heading" ><span class="fw-medium">' + n + "</span></a><small>" + o + "</small></div></div>";
                    },
                },
                {
                    targets: 2,
                    render: function (t, e, s, a) {
                        return "<span>" + (s.order || 0) + "</span>";
                    },
                },
                {
                    targets: 3,
                    render: function (t, e, s, a) {
                        return '<span class="fw-medium text-heading">' + (s.totalspent || 0) + "</span>";
                    },
                },
                {
                    targets: -1,
                    title: "Actions",
                    searchable: !1,
                    orderable: !1,
                    render: function (e, t, a, s) {
                        return '<div class="d-flex align-items-sm-center justify-content-sm-center"><a href="/User/Edit/' + a.id + '" class="edit-category btn btn-icon btn-text-secondary rounded-pill waves-effect waves-light"><i class="ti ti-edit"></i></a><button class="btn btn-icon btn-text-secondary rounded-pill waves-effect waves-light dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical ti-md"></i></button><div class="dropdown-menu dropdown-menu-end m-0"><a href="/User/Details/' + a.id+ '" class="dropdown-item">View</a><a href="/User/Delete/' + a.id + '" class="delete-category dropdown-item text-danger">Delete</a></div></div>';
                    },
                },
            ],
            order: [[2, "desc"]],
            dom: '<"card-header d-flex flex-wrap flex-md-row flex-column align-items-start align-items-sm-center py-0"<"d-flex align-items-center me-5"f><"dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-md-end flex-wrap flex-sm-nowrap mb-6 mb-sm-0"lB>>t<"row mx-1"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
            language: { sLengthMenu: "_MENU_", search: "", searchPlaceholder: "Search Order", paginate: { next: '<i class="ti ti-chevron-right ti-sm"></i>', previous: '<i class="ti ti-chevron-left ti-sm"></i>' } },
            buttons: [
                {
                    extend: "collection",
                    className: "btn btn-label-secondary dropdown-toggle me-4 waves-effect waves-light",
                    text: '<i class="ti ti-upload ti-xs me-2"></i>Export',
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
                                                    void 0 !== e.classList && e.classList.contains("customer-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
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
                                columns: [1, 2, 3],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("customer-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
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
                                columns: [1, 2, 3],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("customer-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
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
                                columns: [1, 2, 3],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("customer-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                        },
                        {
                            extend: "copy",
                            text: '<i class="ti ti-copy me-2" ></i>Copy',
                            className: "dropdown-item",
                            exportOptions: {
                                columns: [1, 2, 3],
                                format: {
                                    body: function (t, e, s) {
                                        var a;
                                        return t.length <= 0
                                            ? t
                                            : ((t = $.parseHTML(t)),
                                                (a = ""),
                                                $.each(t, function (t, e) {
                                                    void 0 !== e.classList && e.classList.contains("customer-name") ? (a += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (a += e.textContent) : (a += e.innerText);
                                                }),
                                                a);
                                    },
                                },
                            },
                        },
                    ],
                },
                {
                    text: '<i class="ti ti-plus me-0 me-sm-1 mb-1 ti-xs"></i><span class="d-none d-sm-inline-block">Add Customer</span>',
                    className: "add-new btn btn-primary waves-effect waves-light",
                    action: function () {
                        window.location.href = "/User/Create";
                    },
                },
            ],
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.modal({
                        header: function (t) {
                            return "Details of " + t.data().customer;
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
        })),
            $(".dataTables_length").addClass("ms-n2 me-2"),
            $(".dt-action-buttons").addClass("pt-0"),
            $(".dataTables_filter").addClass("ms-n3 mb-0 mb-md-6"),
            $(".dt-buttons").addClass("d-flex flex-wrap")),
        $(".datatables-customers tbody").on("click", ".delete-record", function () {
            t.row($(this).parents("tr")).remove().draw();
        }),
        setTimeout(() => {
            $(".dataTables_filter .form-control").removeClass("form-control-sm"), $(".dataTables_length .form-select").removeClass("form-select-sm");
        }, 300);
});