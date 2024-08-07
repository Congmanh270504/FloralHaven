"use strict";
const commentEditor = document.querySelector(".comment-editor");
commentEditor && new Quill(commentEditor, { modules: { toolbar: ".comment-toolbar" }, placeholder: "Write a Comment...", theme: "snow" }),
    $(function () {
        let e, t, a;
        a = (isDarkStyle ? ((e = config.colors_dark.borderColor), (t = config.colors_dark.bodyBg), config.colors_dark) : ((e = config.colors.borderColor), (t = config.colors.bodyBg), config.colors)).headingColor;
        var s = $(".datatables-category-list"),
            o = $(".select2");
        o.length &&
            o.each(function () {
                var e = $(this);
                e.wrap('<div class="position-relative"></div>').select2({ dropdownParent: e.parent(), placeholder: e.data("placeholder") });
            }),
            s.length &&
            (s.DataTable({
                serverSide: true,
                processing: true,
                orderMulti: false,
                ajax: {
                    url: "/Category/GetCategories",
                    type: "POST",
                    dataType: "json",
                },
                columns: [{ data: "" }, { data: "category_name" }, { data: "total_products" }, { data: "" }],
                columnDefs: [
                    {
                        className: "control",
                        searchable: !1,
                        orderable: !1,
                        responsivePriority: 1,
                        targets: 0,
                        render: function (e, t, a, s) {
                            return "";
                        },
                    },
                    {
                        targets: 1,
                        responsivePriority: 2,
                        render: function (e, t, a, s) {
                            var o = a.category_name,
                                r = a.id,
                                n = "",
                                i = a.id;
                            return '<div class="d-flex align-items-center"><div class="avatar-wrapper me-3 rounded-2 bg-label-secondary"><div class="avatar">' + (n ? '<img src="' + n + '" alt="' + i + '" class="rounded-2">' : '<span class="avatar-initial rounded-2 bg-label-' + ["success", "danger", "warning", "info", "dark", "primary", "secondary"][Math.floor(6 * Math.random())] + '">' + (n = (((n = (o = a.category_name).match(/\b\w/g) || []).shift() || "") + (n.pop() || "")).toUpperCase()) + "</span>") + '</div></div><div class="d-flex flex-column justify-content-center"><span class="text-heading text-wrap fw-medium">' + o + '</span><span class="text-truncate mb-0 d-none d-sm-block"><small>' + r + "</small></span></div></div>";
                        },
                    },
                    {
                        targets: 2,
                        responsivePriority: 3,
                        render: function (e, t, a, s) {
                            return '<div class="text-sm-end">' + a.total_products + "</div>";
                        },
                    },
                    {
                        targets: -1,
                        title: "Actions",
                        searchable: !1,
                        orderable: !1,
                        render: function (e, t, a, s) {
                            return '<div class="d-flex align-items-sm-center justify-content-sm-center"><button class="btn btn-icon btn-text-secondary rounded-pill waves-effect waves-light"><i class="ti ti-edit"></i></button><button class="btn btn-icon btn-text-secondary rounded-pill waves-effect waves-light dropdown-toggle hide-arrow" data-bs-toggle="dropdown"><i class="ti ti-dots-vertical ti-md"></i></button><div class="dropdown-menu dropdown-menu-end m-0"><a href="javascript:0;" class="dropdown-item">View</a><a href="javascript:0;" class="dropdown-item">Suspend</a></div></div>';
                        },
                    },
                ],
                order: [2, "desc"],
                dom: '<"card-header d-flex flex-wrap py-0 flex-column flex-sm-row"<f><"d-flex justify-content-center justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex justify-content-center flex-md-row align-items-baseline"lB>>>t<"row mx-1"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                lengthMenu: [7, 10, 20, 50, 70, 100],
                language: { sLengthMenu: "_MENU_", search: "", searchPlaceholder: "Search Category", paginate: { next: '<i class="ti ti-chevron-right ti-sm"></i>', previous: '<i class="ti ti-chevron-left ti-sm"></i>' } },
                buttons: [{ text: '<i class="ti ti-plus ti-xs me-0 me-sm-2"></i><span class="d-none d-sm-inline-block">Add Category</span>', className: "add-new btn btn-primary ms-2 waves-effect waves-light", attr: { "data-bs-toggle": "offcanvas", "data-bs-target": "#offcanvasEcommerceCategoryList" } }],
                responsive: {
                    details: {
                        display: $.fn.dataTable.Responsive.display.modal({
                            header: function (e) {
                                return "Details of " + e.data().categories;
                            },
                        }),
                        type: "column",
                        renderer: function (e, t, a) {
                            a = $.map(a, function (e, t) {
                                return "" !== e.title ? '<tr data-dt-row="' + e.rowIndex + '" data-dt-column="' + e.columnIndex + '"><td> ' + e.title + ':</td> <td class="ps-0">' + e.data + "</td></tr>" : "";
                            }).join("");
                            return !!a && $('<table class="table"/><tbody />').append(a);
                        },
                    },
                },
            }),
                $(".dt-action-buttons").addClass("pt-0"),
                $(".dataTables_filter").addClass("me-3 mb-sm-6 mb-0 ps-0")),
            setTimeout(() => {
                $(".dataTables_filter .form-control").removeClass("form-control-sm"), $(".dataTables_filter .form-control").addClass("ms-0"), $(".dataTables_length .form-select").removeClass("form-select-sm"), $(".dataTables_length .form-select").addClass("ms-0");
            }, 300);
    }),
    (function () {
        var e = document.getElementById("eCommerceCategoryListForm");
        var ff = FormValidation.formValidation(e, {
            fields: { categoryTitle: { validators: { notEmpty: { message: "Please enter category title" } } }, slug: { validators: { notEmpty: { message: "Please enter slug" } } } },
            plugins: {
                trigger: new FormValidation.plugins.Trigger(),
                bootstrap5: new FormValidation.plugins.Bootstrap5({
                    eleValidClass: "is-valid",
                    rowSelector: function (e, t) {
                        return ".mb-6";
                    },
                }),
                submitButton: new FormValidation.plugins.SubmitButton(),
                autoFocus: new FormValidation.plugins.AutoFocus(),
            },
        });
    const submitBtn = e.querySelector('.data-submit[type="submit"]');
        submitBtn.addEventListener("click", function (e) {
        e.preventDefault();
        ff.validate().then(function (e) {
            if ("Valid" == e) {
                var formData = new FormData();
                formData.append("name", document.querySelector("#category-title").value);
                formData.append("slug", document.querySelector("#category-slug").value);                
                $.ajax({
                    url: "/test",
                    type: "POST",
                    data: formData,
                    processData: !1,
                    contentType: !1,
                    success: function (e) {                        
                        var t = document.querySelector("#offcanvasEcommerceCategoryList");
                        t && new bootstrap.Offcanvas(t).hide();
                    },
                });
            }
        });
    });
    })();