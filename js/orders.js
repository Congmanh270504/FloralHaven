"use strict";
$(function () {
	let assetsPath = "/adminassets/";
	let e, a, s;
	//s = (isDarkStyle ? ((e = config.colors_dark.borderColor), (a = config.colors_dark.bodyBg), config.colors_dark) : ((e = config.colors.borderColor), (a = config.colors.bodyBg), config.colors)).headingColor;
	var t,
		n = $(".datatables-order"),
		r = { 1: { title: "Dispatched", class: "bg-label-warning" }, 2: { title: "Delivered", class: "bg-label-success" }, 3: { title: "Out for Delivery", class: "bg-label-primary" }, 4: { title: "Ready to Pickup", class: "bg-label-info" } },
		o = { 1: { title: "Paid", class: "text-success" }, 2: { title: "Pending", class: "text-warning" }, 3: { title: "Failed", class: "text-danger" }, 4: { title: "Cancelled", class: "text-secondary" } };
	n.length &&
		((t = n.DataTable({
			serverSide: true,
			processing: true,
			orderMulti: false,
			ajax: {
				url: "/Account/GetOrders",
				type: "POST",
				dataType: "json",
			},
			columns: [{ data: "id" }, { data: "order" }, { data: "image" }, { data: "date" }, { data: "total" }],
			columnDefs: [
				{
					className: "control",
					searchable: !1,
					orderable: !1,
					responsivePriority: 2,
					targets: 0,
					render: function (t, e, a, s) {
						return "";
					},
				},
				{
					targets: 1,
					render: function (t, e, a, s) {
						return '<a href="/Account/OrderDetails/'+ a.id + '"><span>#' + a.id + "</span></a>";
					},
				},
				{
					targets: 2,
					responsivePriority: 1,
					render: function (t, e, a, s) {
						var n = a.id,
							o = a.image;
						return '<div class="d-flex justify-content-start align-items-center order-name text-nowrap"><div class="avatar-wrapper"><div class="avatar avatar-sm me-3">' + (o ? '<img src="' + o + '" alt="Image" class="rounded-circle">' : '<span class="avatar-initial rounded-circle bg-label-' + ["success", "danger", "warning", "info", "dark", "primary", "secondary"][Math.floor(6 * Math.random())] + '">' + (o = (((o = (n = a.id).match(/\b\w/g) || []).shift() || "") + (o.pop() || "")).toUpperCase()) + "</span>") + '</div></div></div>';
					},
				},
				{
					targets: 3,
					render: function (t, e, a, s) {
						var n = new Date(a.date);
						var time = n.toLocaleTimeString("en-US", { hour: "numeric", minute: "numeric" });
						return '<span class="text-nowrap">' + n.toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric", time: "numeric" }) + ", " + time + "</span>";
					},
				},
				{
					targets: 4,
					render: function (t, e, a, s) {
						return '<div class="text-sm-end">' + a.total + "</div>";
					},
				},
			],
			order: [3, "asc"],
			dom: '<"card-header py-0 d-flex flex-column flex-md-row align-items-center"<f><"d-flex align-items-center justify-content-md-end gap-2 justify-content-center"l<"dt-action-buttons"B>>>t<"row mx-1"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
			lengthMenu: [10, 40, 60, 80, 100],
			language: { sLengthMenu: "_MENU_", search: "", searchPlaceholder: "Search Order", info: "Displaying _START_ to _END_ of _TOTAL_ entries", paginate: { next: '<i class="ti ti-chevron-right ti-sm"></i>', previous: '<i class="ti ti-chevron-left ti-sm"></i>' } },
			buttons: [
				{
					extend: "collection",
					className: "btn btn-label-secondary dropdown-toggle waves-effect waves-light",
					text: '<i class="ti ti-upload ti-xs me-2"></i>Export',
					buttons: [
						{
							extend: "print",
							text: '<i class="ti ti-printer me-2"></i>Print',
							className: "dropdown-item",
							exportOptions: {
								columns: [2, 3, 4, 5, 6, 7],
								format: {
									body: function (t, e, a) {
										var s;
										return t.length <= 0
											? t
											: ((t = $.parseHTML(t)),
												(s = ""),
												$.each(t, function (t, e) {
													void 0 !== e.classList && e.classList.contains("order-name") ? (s += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (s += e.textContent) : (s += e.innerText);
												}),
												s);
									},
								},
							},
							customize: function (t) {
								$(t.document.body).css("color", s).css("border-color", e).css("background-color", a), $(t.document.body).find("table").addClass("compact").css("color", "inherit").css("border-color", "inherit").css("background-color", "inherit");
							},
						},
						{
							extend: "csv",
							text: '<i class="ti ti-file me-2"></i>Csv',
							className: "dropdown-item",
							exportOptions: {
								columns: [1,2, 3, 4],
								format: {
									body: function (t, e, a) {
										var s;
										return t.length <= 0
											? t
											: ((t = $.parseHTML(t)),
												(s = ""),
												$.each(t, function (t, e) {
													void 0 !== e.classList && e.classList.contains("order-name") ? (s += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (s += e.textContent) : (s += e.innerText);
												}),
												s);
									},
								},
							},
						},
						{
							extend: "excel",
							text: '<i class="ti ti-file-export me-2"></i>Excel',
							className: "dropdown-item",
							exportOptions: {
								columns: [1,2, 3, 4],
								format: {
									body: function (t, e, a) {
										var s;
										return t.length <= 0
											? t
											: ((t = $.parseHTML(t)),
												(s = ""),
												$.each(t, function (t, e) {
													void 0 !== e.classList && e.classList.contains("order-name") ? (s += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (s += e.textContent) : (s += e.innerText);
												}),
												s);
									},
								},
							},
						},
						{
							extend: "pdf",
							text: '<i class="ti ti-file-text me-2"></i>Pdf',
							className: "dropdown-item",
							exportOptions: {
								columns: [1,2, 3, 4],
								format: {
									body: function (t, e, a) {
										var s;
										return t.length <= 0
											? t
											: ((t = $.parseHTML(t)),
												(s = ""),
												$.each(t, function (t, e) {
													void 0 !== e.classList && e.classList.contains("order-name") ? (s += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (s += e.textContent) : (s += e.innerText);
												}),
												s);
									},
								},
							},
						},
						{
							extend: "copy",
							text: '<i class="ti ti-copy me-2"></i>Copy',
							className: "dropdown-item",
							exportOptions: {
								columns: [1,2, 3, 4],
								format: {
									body: function (t, e, a) {
										var s;
										return t.length <= 0
											? t
											: ((t = $.parseHTML(t)),
												(s = ""),
												$.each(t, function (t, e) {
													void 0 !== e.classList && e.classList.contains("order-name") ? (s += e.lastChild.firstChild.textContent) : void 0 === e.innerText ? (s += e.textContent) : (s += e.innerText);
												}),
												s);
									},
								},
							},
						},
					],
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
					renderer: function (t, e, a) {
						a = $.map(a, function (t, e) {
							return "" !== t.title ? '<tr data-dt-row="' + t.rowIndex + '" data-dt-column="' + t.columnIndex + '"><td>' + t.title + ":</td> <td>" + t.data + "</td></tr>" : "";
						}).join("");
						return !!a && $('<table class="table"/><tbody />').append(a);
					},
				},
			},
		})),
			$(".dataTables_length").addClass("ms-n2"),
			$(".dt-action-buttons").addClass("pt-0"),
			$(".dataTables_filter").addClass("ms-n3 mb-0 mb-md-6")),
		$(".datatables-order tbody").on("click", ".delete-record", function () {
			t.row($(this).parents("tr")).remove().draw();
		}),
		setTimeout(() => {
			$(".dataTables_filter .form-control").removeClass("form-control-sm"), $(".dataTables_length .form-select").removeClass("form-select-sm");
		}, 300);
});
