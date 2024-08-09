"use strict";
$(function () {
	var e = $(".datatables-order-details");
	e.length &&
		e.DataTable({
			serverSide: true,
			processing: true,
			orderMulti: false,
			ajax: {
				url: "/Admin/GetOrderDetails/" + vbid,
				type: "POST",
				dataType: "json",
			},
			columns: [{ data: "id" }, { data: "name" }, { data: "price" }, { data: "qty" }, { data: "total" }, {data:""}],
			columnDefs: [
				{
					className: "control",
					searchable: !1,
					orderable: !1,
					responsivePriority: 2,
					targets: 0,
					render: function (e, t, a, r) {
						return "";
					},
				},
				{
					targets: 1,
					responsivePriority: 1,
					searchable: !1,
					orderable: !1,
					render: function (e, t, a, r) {
						var s = a.name,
							n = a.handle,
							o = a.image;
						return '<div class="d-flex justify-content-start align-items-center text-nowrap"><div class="avatar-wrapper"><div class="avatar avatar-sm me-3">' + (o ? '<img src="' + o + '" alt="product-' + s + '" class="rounded-2">' : '<span class="avatar-initial rounded-2 bg-label-' + ["success", "danger", "warning", "info", "dark", "primary", "secondary"][Math.floor(6 * Math.random())] + '">' + (o = (((o = (s = a.name).match(/\b\w/g) || []).shift() || "") + (o.pop() || "")).toUpperCase()) + "</span>") + '</div></div><div class="d-flex flex-column"><h6 class="text-heading mb-0">' + s + "</h6><small>" + n + "</small></div></div>";
					},
				},
				{
					targets: 2,
					searchable: !1,
					orderable: !1,
					render: function (e, t, a, r) {
						return "<span>$" + a.price + "</span>";
					},
				},
				{
					targets: 3,
					searchable: !1,
					orderable: !1,
					render: function (e, t, a, r) {
						return '<span class="text-body">' + a.qty + "</span>";
					},
				},
				{
					targets: 4,
					searchable: !1,
					orderable: !1,
					render: function (e, t, a, r) {
						return '<span class="text-body">' + a.qty * a.price + "</span>";
					},
				},
				{
					targets: -1,
					title: "Actions",
					searchable: !1,
					orderable: !1,
					render: function (e, t, a, s) {
						return '<div class="d-flex align-items-sm-center justify-content-sm-center"><button data-id="' + a.id + '" class="edit-billdetails btn btn-icon btn-text-secondary rounded-pill waves-effect waves-light"><i class="ti ti-edit"></i></button></div>';
					},
				},
			],
			order: [1, ""],
			dom: "t",
			responsive: {
				details: {
					display: $.fn.dataTable.Responsive.display.modal({
						header: function (e) {
							return "Details of " + e.data().full_name;
						},
					}),
					type: "column",
					renderer: function (e, t, a) {
						a = $.map(a, function (e, t) {
							return "" !== e.title ? '<tr data-dt-row="' + e.rowIndex + '" data-dt-column="' + e.columnIndex + '"><td>' + e.title + ":</td> <td>" + e.data + "</td></tr>" : "";
						}).join("");
						return !!a && $('<table class="table"/><tbody />').append(a);
					},
				},
			},
		}),
		setTimeout(() => {
			$(".dataTables_filter .form-control").removeClass("form-control-sm"), $(".dataTables_length .form-select").removeClass("form-select-sm");
		}, 300);
});