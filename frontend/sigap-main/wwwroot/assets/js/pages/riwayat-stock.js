// $(function () {
//     // DataTable initialization
//     const datariwayatstock = [
//         {
//             gambar: "/assets/images/tester/sarung_tangan_kulit.png",
//             barang: "Sarung Tangan Kulit",
//             kategori: "Sarung Tangan",
//             // tanggal: "09/12/23",
//             tanggal: new Date("2023-12-10"), // JS default Date object
//             status: "Masuk",
//             jumlah: 222,
//             stock: 222
//         },
//         {
//             gambar: "/assets/images/tester/tissue_majun_roll.png",
//             barang: "Tissue Majun Roll",
//             kategori: "Maintenance Equipment",
//             // tanggal: "09/12/23",
//             tanggal: new Date("2023-12-10"), // JS default Date object
//             status: "Keluar",
//             jumlah: 30,
//             stock: 222
//         },

//         // Randomly generated entries below...
//     ];
//     // Generate random data for testing
//     for (let i = 0; i < 994; i++) {
//         datariwayatstock.push({
//             gambar: "/assets/images/tester/safety_rubber_boots.png",
//             barang: "Safety Rubber Boots",
//             kategori: "Safety Equipment",
//             // tanggal: `${Math.floor(Math.random() * 28) + 1}/${Math.floor(Math.random() * 12) + 1}/${Math.floor(Math.random() * 24) + 2000}`,
//             tanggal: new Date(                                     // ✅ always Date
//                 2000 + Math.floor(Math.random() * 26),             // year 2000–2025
//                 Math.floor(Math.random() * 12),                    // month 0–11
//                 Math.floor(Math.random() * 28) + 1                 // day 1–28
//             ),
//             status: ["Masuk", "Keluar"][Math.floor(Math.random() * 2)],
//             jumlah: Math.floor(Math.random() * 500) + 1,
//             stock: Math.floor(Math.random() * 500) + 1
//         });
//     }

//     // Initialize DataTable with buttons and customizations
//     let table = $('#datatable-buttons').DataTable({
//         lengthChange: true,
//         //buttons: ['copy', 'excel', 'pdf', 'colvis'],
//         buttons: [ 
//             {
//                 extend: 'colvis',
//                 columns: ':not(.noVis)',
//                 className: 'btn btn-dark',
//                 //columnText: function (dt, idx, title) {
//                 //    if (title != '') {
//                 //        return title;
//                 //    }
//                 //},
//             },
//         ],
//         language: {
//             buttons: {
//                 colvis: 'Tampilkan Kolom',
//             },
//             search: 'Cari:',
//             lengthMenu: '_MENU_ baris barang',
//             info: 'Menampilkan _START_ sampai _END_ dari _TOTAL_ data',
//             select: {
//                 rows: {
//                     _: '%d baris dipilih',
//                     0: '',
//                 }
//             },
//         },
//         //layout: {
//         //    bottomEnd: {
//         //    }
//         //},
//         // data kolom tabel
//         data: datariwayatstock,
//         columns: [
//             {
//                 data: null,
//                 defaultContent: '',
//                 className: 'control noVis',
//                 orderable: false
//             },
//             {
//                 data: null,
//                 render: function (data, type, row, meta) {
//                     return meta.row + 1;
//                 },
//                 title: 'No',
//                 className: 'all noVis dt-center',
//             },

//             //kolom gambar + barang
//             {
//                 data: null,
//                 title: "Barang",
//                 render: function (data, type, row) {
//                     return `
//                         <div class="d-flex img-cell align-items-center gap-3">
//                             <img src="${row.gambar}" alt="${row.barang}" class="img-barang">
//                             <span>${row.barang}</span>
//                         </div>
//                     `;
//                 }


//             },
//             { data: "kategori", title: "Kategori" },
//             //kolom tanggal
//             // { data: "tanggal", title: "Tanggal" },
//             {
//                 data: 'tanggal',
//                 title: 'Tanggal',
//                 render: function (data, type, row) {
//                     // Sorting/Filtering uses raw Date object
//                     if (type === 'sort' || type === 'type') {
//                         return data;
//                     }
//                     // Display format dd/mm/yyyy
//                     if (data instanceof Date) {
//                         return data.toLocaleDateString("id-ID", {
//                             day: "2-digit",
//                             month: "2-digit",
//                             year: "numeric"
//                         });
//                     }
//                     return data;
//                 }
//             },
//             //kolom in/ex
//             {
//                 data: "status", title: "Status",
//                 render: function (data, type, row) {
//                     switch (data) {
//                         case 'Masuk':
//                             return `<span class="badge bg-success">${data}</span>`;
//                             break;
//                         case 'Keluar':
//                             return `<span class="badge bg-danger">${data}</span>`;
//                             break;
//                     }
//                 }
//             },
//             //kolom jumlah
//             { data: "jumlah", title: "Jumlah" },
//             //stock
//             { data: "stock", title: "Stock" },
//             //kolom aksi
//             {
//                 data: null,
//                 title: 'Aksi',
//                 render: function (data, type, row) {
//                     return `
//                         <div class="d-flex gap-2">
//                             <a href="/riwayattransaksi/detailriwayatstock/" class="btn btn-info">
//                                 <i class="mdi mdi-file-find-outline"></i>
//                                 <span>Detail</span>
//                             </a>
//                         </div>
//                     `;
//                 },
//                 className: 'dt-center noVis',
//                 orderable: false
//             }
//         ],
//         //select: {
//         //    style: 'multi',
//         //    selector: 'td:nth-child(2)',
//         //    headerCheckbox: 'select-page',
//         //},
//         order: [[1, 'asc']],
//         columnDefs: [
//             { className: 'dt-center align-middle', targets: '_all' },
//             {
//                 targets: [2,3],
//                 createdCell: function (td, cellData, rowData, row, col) {
//                     td.classList.add('text-start');


//                 }
//             },
//         ],
//         responsive: {
//             details: {
//                 type: 'column',
//                 target: 0
//             }
//         },
//     });

//     // length change button
//     $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
//     $(".dt-length select").addClass('form-select form-select-sm');

//     $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
//         .removeClass('align-items-center')
//     //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
//     //.addClass('d-flex flex-column col-md-6 gap-2');

//     const customButtons = `
//         <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
//             <a href="/riwayattransaksi" class="btn btn-success text-white">
//                 <i class="mdi mdi-file-plus-outline"></i>
//                 <span>Riwayat Transaksi</span>
//             </a>
//         </div>
//     `;

//     $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
//     table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');

//     $('#datatable-buttons thead').addClass('table-dark');

//     $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
//         .removeClass('align-items-center col-md-auto justify-content-between')
//         .addClass('align-items-end col-xl-6 flex-column justify-content-end');

//     $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
//         .removeClass('col-md-auto align-items-center')
//         .addClass('col-xl-6 align-items-end mb-md-2 mb-xl-0');

//     $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//     $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//     $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
//         .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
//         .addClass('d-flex flex-column col-md-6 gap-2');

//     let jumpToPage = `
//         <div class="dt-jump-to-page d-flex align-items-center align-self-md-start align-self-center">
//             <input type="number" min="1" class="form-control form-control-sm me-2" id="jump-to-page" placeholder="Lompat ke" style="width: 100px;">
//             <button class="btn btn-sm btn-dark" id="jump-to-btn">Go</button>
//         </div>
//     `;

//     // Insert it after pagination controls
//     $(jumpToPage).insertAfter('#datatable-buttons_wrapper .dt-paging');

//     function jumpToSpecifiedPage() {
//         let page = parseInt($('#jump-to-page').val(), 10) - 1;
//         if (!isNaN(page) && page >= 0 && page < table.page.info().pages) {
//             table.page(page).draw('page');
//         }
//     }

//     // Handle 'Go' button click
//     $(document).on('click', '#jump-to-btn', function () {
//         jumpToSpecifiedPage();
//     });

//     // Handle 'Enter' keypress in input
//     $(document).on('keypress', '#jump-to-page', function (e) {
//         if (e.which === 13) {
//             e.preventDefault();
//             jumpToSpecifiedPage();
//         }
//     });

//     // ==== FUNGSI FILTER DATE RANGE + TOGGLE ICONS + CLEAR INPUT (START) ====

//     // Insert Filter Card Above Search Input 
//     const layoutEnd = $('#datatable-buttons_wrapper .dt-layout-end:eq(0)');

//     const filterRow = `
//         <div class="row mb-2">
//             <div class="col-12">
//                 <div class="card shadow-none border-dark border m-0">
//                 <div class="card-body p-3">
//                     <div class="row align-items-center justify-content-between g-2">
//                     <div class="col-md-2 ps-3">
//                         <label class="form-label mb-0">Filter :</label>
//                     </div>
//                     <div class="col-md-10">
//                         <div class="d-flex align-items-center">
//                         <div class="input-group date-input-group">
//                             <input type="text" id="tanggal_awal" class="form-control" placeholder="Tanggal Awal" autocomplete="off">
//                             <span class="input-group-text">
//                             <i id="icon-awal-calendar" class="mdi mdi-calendar"></i>
//                             <i id="icon-awal-clear" class="mdi mdi-close text-muted" style="cursor:pointer; display:none;"></i>
//                             </span>
//                         </div>
//                         <span class="mx-2">-</span>
//                         <div class="input-group date-input-group">
//                             <input type="text" id="tanggal_akhir" class="form-control" placeholder="Tanggal Akhir" autocomplete="off">
//                             <span class="input-group-text">
//                             <i id="icon-akhir-calendar" class="mdi mdi-calendar"></i>
//                             <i id="icon-akhir-clear" class="mdi mdi-close text-muted" style="cursor:pointer; display:none;"></i>
//                             </span>
//                         </div>
//                         </div>
//                     </div>
//                     </div>
//                 </div>
//                 </div>
//             </div>
//         </div>`;

//     // Wrap the existing dt-search in its own row
//     const searchDiv = layoutEnd.find('.dt-search').detach();
//     const searchRow = $('<div class="row w-100"></div>').append(searchDiv);

//     // Clear layout-end and append both rows
//     layoutEnd.empty().append(filterRow).append(searchRow);

//     // --- Custom Date Range Filter ---
//     $('#tanggal_awal, #tanggal_akhir').datepicker({
//         format: "dd/mm/yyyy",
//         autoclose: true,
//         todayHighlight: true,
//         orientation: "bottom auto"
//     });

//     // --- Helper: parse dd/mm/yyyy to Date ---
//     function parseDate(str) {
//         if (!str) return null;
//         var parts = str.split('/');
//         return new Date(parts[2], parts[1] - 1, parts[0]);
//     }

//     // --- Helper: toggle icons ---
//     function toggleIcons(input, calendarIcon, clearIcon) {
//         if ($(input).val()) {
//             $(calendarIcon).hide();
//             $(clearIcon).show();
//         } else {
//             $(calendarIcon).show();
//             $(clearIcon).hide();
//         }
//     }

//     // --- Bind date input behavior ---
//     function bindDateInput(inputId, calIconId, clearIconId) {
//         $(inputId).on('input change', function () {
//             toggleIcons(this, calIconId, clearIconId);

//             var startDate = parseDate($('#tanggal_awal').val());
//             var endDate = parseDate($('#tanggal_akhir').val());

//             // Validate date order
//             if (startDate && endDate && endDate < startDate) {
//                 Swal.fire({
//                     icon: 'error',
//                     title: 'Tanggal tidak valid',
//                     text: 'Tanggal akhir tidak boleh lebih kecil dari tanggal awal',
//                     confirmButtonText: 'OK'
//                 });
//                 $(this).val('').datepicker('update', null);
//                 toggleIcons(this, calIconId, clearIconId);
//                 return;
//             }

//             table.draw(); // redraw if valid
//         });

//         // Clear with ❌
//         $(clearIconId).on('click', function () {
//             $(inputId).val('').datepicker('update', null).trigger('change');
//         });

//         // Initial state
//         toggleIcons(inputId, calIconId, clearIconId);
//     }

//     // --- Attach to inputs ---
//     bindDateInput('#tanggal_awal', '#icon-awal-calendar', '#icon-awal-clear');
//     bindDateInput('#tanggal_akhir', '#icon-akhir-calendar', '#icon-akhir-clear');

//     // --- DataTable custom date filter ---
//     $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
//         var min = parseDate($('#tanggal_awal').val());
//         var max = parseDate($('#tanggal_akhir').val());

//         var dateStr = data[4]; // column index of "tanggal"
//         if (!dateStr) return false; // skip rows without date

//         var parts = dateStr.split('/');
//         var rowDate = new Date(parts[2], parts[1] - 1, parts[0]);

//         if (
//             (min === null || rowDate >= min) &&
//             (max === null || rowDate <= max)
//         ) {
//             return true;
//         }
//         return false;
//     });
// });

$(function () {
    const { hasRiwayatTransaksi } = window.Permission;

    // Initialize DataTable with buttons and customizations
    let table = $('#datatable-buttons').DataTable({
        serverSide: true,
        processing: true,
        lengthChange: true,
        //buttons: ['copy', 'excel', 'pdf', 'colvis'],
        ajax: {
            // url: "/api/transaksi",
            // type: "GET",
            url: '/api/transaksi/datatable',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                let tanggal = d.columns.find(c => c.data === "created_at");
                let tanggalAwal = $('#tanggal_awal_payload').val();
                let tanggalAkhir = $('#tanggal_akhir_payload').val();
                let tanggalSearchPayload = '';

                d.columns.push({
                    data: "status",
                    searchable: true,
                    orderable: true,
                    search: { value: "Done" }
                });

                if(tanggalAwal) {
                    tanggalSearchPayload += tanggalAwal;
                }

                if(tanggalAkhir) {
                    if(tanggalSearchPayload != '') tanggalSearchPayload = tanggalSearchPayload + ',' + tanggalAkhir;
                    else tanggalSearchPayload += tanggalAkhir;
                }

                tanggal.search.value = tanggalSearchPayload;

                return JSON.stringify(d);   // << send DataTables parameters
            },
            dataSrc: function (json) {

                let rows = [];

                json.data.forEach(parent => {

                    parent.transactionDetailDto.forEach(detail => {

                        rows.push({
                            transactionDetailDto: {
                                jumlah_bar: detail.jumlah_bar,
                                itemDto: {
                                    nama_barang: detail.itemDto.nama_barang,
                                    satuanbar_id: detail.itemDto.satuanbar_id,
                                    link_gambar_bar: detail.itemDto.link_gambar_bar,
                                    jumlah_barang: detail.itemDto.jumlah_barang,
                                    booked_qty: detail.itemDto.booked_qty,
                                }
                            },
                            // kategori: detail.itemDto.categoryDto.namakategoribar,
                            kategori_transact_id: parent.kategori_transact_id,
                            created_at: parent.created_at,
                            // status: parent.status,
                        });

                    });

                });

                return rows;
            }
        },
        buttons: [ 
            {
                extend: 'colvis',
                columns: ':not(.noVis)',
                className: 'btn btn-dark',
                //columnText: function (dt, idx, title) {
                //    if (title != '') {
                //        return title;
                //    }
                //},
            },
        ],
        language: {
            buttons: {
                colvis: 'Tampilkan Kolom',
            },
            search: 'Cari:',
            lengthMenu: '_MENU_ baris barang',
            info: 'Menampilkan _START_ sampai _END_ dari _TOTAL_ data',
            select: {
                rows: {
                    _: '%d baris dipilih',
                    0: '',
                }
            },
        },
        //layout: {
        //    bottomEnd: {
        //    }
        //},
        // data kolom tabel
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'control noVis',
                orderable: false
            },
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                },
                title: 'No',
                className: 'all noVis dt-center',
                orderable: false,
                sortable: false
            },
            // { 
            //     data: "barang",
            //     title: "Barang",
            //     render: data => `<span>${data}</span>`
            // },
            {
                data: "transactionDetailDto.itemDto.nama_barang",
                title: "Barang",
                render: function (data, type, row) {
                    return  `
                        <div class="d-flex img-cell align-items-center gap-3">
                            <img src="${row.transactionDetailDto.itemDto.link_gambar_bar}" 
                                alt="${row.transactionDetailDto.itemDto.nama_barang}" 
                                class="img-barang"
                                onerror="this.onerror=null; this.src='/assets/images/dummy.png';">
                            <span>${row.transactionDetailDto.itemDto.nama_barang}</span>
                        </div>
                    `;
                },
                // orderable: false,
                // sortable: false
                orderable: true,
                sortable: true
            },
            // { 
            //     data: "kategori",
            //     title: "Kategori",
            //     render: data => `<span>${data}</span>`
            // },
            {
                data: "created_at",
                title: "Tanggal",
                render: function (data, type, row) {
                    const tanggal = row.created_at.split(' ')[0];
                    return `<span class="dt-wrap">${tanggal}</span>`;
                },
                orderable: true,
                sortable: true
            },
            {
                data: "kategori_transact_id",
                title: "Status",
                render: data => {
                    if(data == 'IN') return `<span class="badge bg-success">Masuk</span>`;
                    else return `<span class="badge bg-danger">Keluar</span>`;
                },
                orderable: true,
                sortable: true
            },
            // {
            //     data: "status",
            //     title: "Status",
            //     render: data => `<span class="badge bg-info">${data}</span>`,
            //     orderable: true,
            //     sortable: true
            // },
            // { data: "jumlah", title: "Jumlah" },
            { 
                data: "transactionDetailDto.jumlah_bar",
                title: "Jumlah",
                orderable: true,
                sortable: true
            },
            { 
                // data: null,  
                data: "transactionDetailDto.itemDto.jumlah_barang",
                title: "Stok",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${row.transactionDetailDto.itemDto.jumlah_barang - row.transactionDetailDto.itemDto.booked_qty}</span>`;
                },
                orderable: true,
                sortable: true
            },
            { 
                data: "satuanbar_id",  
                title: "Satuan",
                render: function (data, type, row) {
                    let satuan = '';
                    switch(row.transactionDetailDto.itemDto.satuanbar_id) {
                        case 'PCS':
                            satuan = 'Pieces';
                            break;
                        case 'BOX':
                            satuan = 'Box';
                            break;
                        case 'UNIT':
                            satuan = 'Unit';
                            break;
                    }
                    return `<span class="dt-wrap">${satuan}</span>`;
                },
                orderable: true,
                sortable: true
            },
        ],
        //select: {
        //    style: 'multi',
        //    selector: 'td:nth-child(2)',
        //    headerCheckbox: 'select-page',
        //},
        order: [[3, 'desc']],
        columnDefs: [
            { className: 'dt-center align-middle', targets: '_all' },
            {
                targets: [2],
                createdCell: function (td, cellData, rowData, row, col) {
                    td.classList.add('text-start');


                }
            },
        ],
        responsive: {
            details: {
                type: 'column',
                target: 0
            }
        },
    });

    // length change button
    $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
    $(".dt-length select").addClass('form-select form-select-sm');

    $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
        .removeClass('align-items-center')
    //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
    //.addClass('d-flex flex-column col-md-6 gap-2');

    const customButtons = `
        <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
            <!-- a href="/riwayattransaksi" class="btn btn-success text-white">
                <i class="mdi mdi-file-plus-outline"></i>
                <span>Riwayat Transaksi</span>
            </a -->
        </div>
    `;

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);

    if(hasRiwayatTransaksi) {
        $('#datatable-buttons_wrapper_custom').append(`
            <a href="/riwayattransaksi" class="btn btn-success text-white">
                <i class="mdi mdi-file-plus-outline"></i>
                <span>Riwayat Transaksi</span>
            </a>
        `);
    }

    table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');

    $('#datatable-buttons thead').addClass('table-dark');

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
        .removeClass('align-items-center col-md-auto justify-content-between')
        .addClass('align-items-end col-xl-6 flex-column justify-content-end');

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
        .removeClass('col-md-auto align-items-center')
        .addClass('col-xl-6 align-items-end mb-md-2 mb-xl-0');

    $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
    $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
    $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
        .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
        .addClass('d-flex flex-column col-md-6 gap-2');

    let jumpToPage = `
        <div class="dt-jump-to-page d-flex align-items-center align-self-md-start align-self-center">
            <input type="number" min="1" class="form-control form-control-sm me-2" id="jump-to-page" placeholder="Lompat ke" style="width: 100px;">
            <button class="btn btn-sm btn-dark" id="jump-to-btn">Go</button>
        </div>
    `;

    // Insert it after pagination controls
    $(jumpToPage).insertAfter('#datatable-buttons_wrapper .dt-paging');

    function jumpToSpecifiedPage() {
        let page = parseInt($('#jump-to-page').val(), 10) - 1;
        if (!isNaN(page) && page >= 0 && page < table.page.info().pages) {
            table.page(page).draw('page');
        }
    }

    // Handle 'Go' button click
    $(document).on('click', '#jump-to-btn', function () {
        jumpToSpecifiedPage();
    });

    // Handle 'Enter' keypress in input
    $(document).on('keypress', '#jump-to-page', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            jumpToSpecifiedPage();
        }
    });

    // ==== FUNGSI FILTER DATE RANGE + TOGGLE ICONS + CLEAR INPUT (START) ====

    // Insert Filter Card Above Search Input 
    const layoutEnd = $('#datatable-buttons_wrapper .dt-layout-end:eq(0)');

    const filterRow = `
        <div class="row mb-2">
            <div class="col-12">
                <div class="card shadow-none border-dark border m-0">
                <div class="card-body p-3">
                    <div class="row align-items-center justify-content-between g-2">
                    <div class="col-md-2 ps-3">
                        <label class="form-label mb-0">Filter :</label>
                    </div>
                    <div class="col-md-10">
                        <div class="d-flex align-items-center">
                        <div class="input-group date-input-group">
                            <input type="text" id="tanggal_awal" class="form-control" placeholder="Tanggal Awal" autocomplete="off">
                            <input type="hidden" id="tanggal_awal_payload">
                            <span class="input-group-text">
                            <i id="icon-awal-calendar" class="mdi mdi-calendar"></i>
                            <i id="icon-awal-clear" class="mdi mdi-close text-muted" style="cursor:pointer; display:none;"></i>
                            </span>
                        </div>
                        <span class="mx-2">-</span>
                        <div class="input-group date-input-group">
                            <input type="text" id="tanggal_akhir" class="form-control" placeholder="Tanggal Akhir" autocomplete="off">
                            <input type="hidden" id="tanggal_akhir_payload">
                            <span class="input-group-text">
                            <i id="icon-akhir-calendar" class="mdi mdi-calendar"></i>
                            <i id="icon-akhir-clear" class="mdi mdi-close text-muted" style="cursor:pointer; display:none;"></i>
                            </span>
                        </div>
                        </div>
                    </div>
                    </div>
                </div>
                </div>
            </div>
        </div>`;

    // Wrap the existing dt-search in its own row
    const searchDiv = layoutEnd.find('.dt-search').detach();
    const searchRow = $('<div class="row w-100"></div>').append(searchDiv);

    // Clear layout-end and append both rows
    layoutEnd.empty().append(filterRow).append(searchRow);

    // --- Custom Date Range Filter ---
    $('#tanggal_awal, #tanggal_akhir').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        // todayHighlight: true,
        orientation: "bottom auto"
    });

    // --- Helper: parse dd/mm/yyyy to Date ---
    function parseDate(str) {
        if (!str) return null;
        var parts = str.split('/');
        return new Date(parts[2], parts[1] - 1, parts[0]);
    }

    // --- Helper: toggle icons ---
    function toggleIcons(input, calendarIcon, clearIcon) {
        if ($(input).val()) {
            $(calendarIcon).hide();
            $(clearIcon).show();
        } else {
            $(calendarIcon).show();
            $(clearIcon).hide();
        }
    }

    // --- Helper: change string format dd/mm/yyyy to string yyyy-mm-dd
    function formatToPayloadDate(str) {
        if (!str) return '';

        const parts = str.split('/');
        if (parts.length !== 3) return '';

        const [dd, mm, yyyy] = parts;

        // Validate numeric + pad if needed
        if (!dd || !mm || !yyyy) return '';

        return `${yyyy}-${mm.padStart(2, '0')}-${dd.padStart(2, '0')}`;
    }

    // --- Bind date input behavior ---
    function bindDateInput(inputId, calIconId, clearIconId, saveTo) {
        $(inputId).on('input change', function () {
            toggleIcons(this, calIconId, clearIconId);

            var startDate = parseDate($('#tanggal_awal').val());
            var endDate   = parseDate($('#tanggal_akhir').val());
            $(saveTo).val(formatToPayloadDate($(inputId).val()));

            // Validate date order
            if (startDate && endDate && endDate < startDate) {
                Swal.fire({
                    icon: 'error',
                    title: 'Tanggal tidak valid',
                    text: 'Tanggal akhir tidak boleh lebih kecil dari tanggal awal',
                    confirmButtonText: 'OK'
                });
                $(this).val('').datepicker('update', null);
                toggleIcons(this, calIconId, clearIconId);
                return;
            }

            table.ajax.reload(null, false);
        });

        // Clear with ❌
        $(clearIconId).on('click', function () {
            $(inputId).val('').datepicker('update', null).trigger('change');
        });

        // Initial state
        toggleIcons(inputId, calIconId, clearIconId);
    }

    // --- Attach to inputs ---
    bindDateInput('#tanggal_awal', '#icon-awal-calendar', '#icon-awal-clear', '#tanggal_awal_payload');
    bindDateInput('#tanggal_akhir', '#icon-akhir-calendar', '#icon-akhir-clear', '#tanggal_akhir_payload');

    // --- DataTable custom date filter ---
    // $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
    //     var min = parseDate($('#tanggal_awal').val());
    //     var max = parseDate($('#tanggal_akhir').val());

    //     var dateStr = data[4]; // column index of "tanggal"
    //     if (!dateStr) return false; // skip rows without date

    //     var parts = dateStr.split('/');
    //     var rowDate = new Date(parts[2], parts[1] - 1, parts[0]);

    //     if (
    //         (min === null || rowDate >= min) &&
    //         (max === null || rowDate <= max)
    //     ) {
    //         return true;
    //     }
    //     return false;
    // });
});

// $(function () {
//     // Initialize DataTable with buttons and customizations
//     let table = $('#datatable-buttons').DataTable({
//         serverSide: true,
//         processing: true,
//         lengthChange: true,
//         //buttons: ['copy', 'excel', 'pdf', 'colvis'],
//         ajax: {
//             // url: "/api/transaksi",
//             // type: "GET",
//             url: '/api/transaksi/datatable',
//             type: 'POST',
//             contentType: 'application/json',
//             data: function (d) {
//                 return JSON.stringify(d);   // << send DataTables parameters
//             },
//             dataSrc: function (json) {
//                 return json.data.map((parent, i) => ({
//                     no: i + 1,
//                     tanggal: parent.created_at,
//                     status: parent.status,
//                     details: parent.transactionDetailDto.map(d => ({
//                         barang: d.itemDto.nama_barang,
//                         kategori: d.itemDto.categoryDto.namakategoribar,
//                         jumlah: d.jumlah_bar,
//                         stock: d.itemDto.jumlah_barang
//                     }))
//                 }));
//             }
//         },
//         buttons: [ 
//             {
//                 extend: 'colvis',
//                 columns: ':not(.noVis)',
//                 className: 'btn btn-dark',
//                 //columnText: function (dt, idx, title) {
//                 //    if (title != '') {
//                 //        return title;
//                 //    }
//                 //},
//             },
//         ],
//         language: {
//             buttons: {
//                 colvis: 'Tampilkan Kolom',
//             },
//             search: 'Cari:',
//             lengthMenu: '_MENU_ baris barang',
//             info: 'Menampilkan _START_ sampai _END_ dari _TOTAL_ data',
//             select: {
//                 rows: {
//                     _: '%d baris dipilih',
//                     0: '',
//                 }
//             },
//         },
//         //layout: {
//         //    bottomEnd: {
//         //    }
//         //},
//         // data kolom tabel
//         columns: [
//             { 
//                 className: 'dt-control',
//                 orderable: false,
//                 data: null,
//                 defaultContent: ''
//             },
//             { data: "no", title: "No" },
//             { 
//                 data: "tanggal",
//                 title: "Tanggal",
//                 render: data => data.split(" ")[0]
//             },
//             { 
//                 data: "status",
//                 title: "Status",
//                 render: data => `<span class="badge bg-info">${data}</span>`
//             }
//         ],
//         //select: {
//         //    style: 'multi',
//         //    selector: 'td:nth-child(2)',
//         //    headerCheckbox: 'select-page',
//         //},
//         order: [[1, 'asc']],
//         columnDefs: [
//             { className: 'dt-center align-middle', targets: '_all' },
//             {
//                 targets: [2,3],
//                 createdCell: function (td, cellData, rowData, row, col) {
//                     td.classList.add('text-start');


//                 }
//             },
//         ],
//         responsive: {
//             details: {
//                 type: 'column',
//                 target: 0
//             }
//         },
//     });

//     $('#datatable-buttons tbody').on('click', 'td.dt-control', function () {
//         let tr  = $(this).closest('tr');
//         let row = table.row(tr);

//         if (row.child.isShown()) {
//             row.child.hide();
//             tr.removeClass('shown');
//         } else {
//             row.child(formatDetails(row.data())).show();
//             tr.addClass('shown');
//         }
//     });

//     function formatDetails(row) {

//         let html = `
//             <table class="table table-sm mb-0">
//                 <thead>
//                     <tr>
//                         <th>Barang</th>
//                         <th>Kategori</th>
//                         <th>Jumlah</th>
//                         <th>Stock</th>
//                     </tr>
//                 </thead>
//                 <tbody>
//         `;

//         row.details.forEach(d => {
//             html += `
//                 <tr>
//                     <td>${d.barang}</td>
//                     <td>${d.kategori}</td>
//                     <td>${d.jumlah}</td>
//                     <td>${d.stock}</td>
//                 </tr>
//             `;
//         });

//         html += `</tbody></table>`;

//         return html;
//     }

//     // length change button
//     $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
//     $(".dt-length select").addClass('form-select form-select-sm');

//     $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
//         .removeClass('align-items-center')
//     //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
//     //.addClass('d-flex flex-column col-md-6 gap-2');

//     const customButtons = `
//         <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
//             <a href="/riwayattransaksi" class="btn btn-success text-white">
//                 <i class="mdi mdi-file-plus-outline"></i>
//                 <span>Riwayat Transaksi</span>
//             </a>
//         </div>
//     `;

//     $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
//     table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');

//     $('#datatable-buttons thead').addClass('table-dark');

//     $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
//         .removeClass('align-items-center col-md-auto justify-content-between')
//         .addClass('align-items-end col-xl-6 flex-column justify-content-end');

//     $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
//         .removeClass('col-md-auto align-items-center')
//         .addClass('col-xl-6 align-items-end mb-md-2 mb-xl-0');

//     $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//     $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//     $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
//         .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
//         .addClass('d-flex flex-column col-md-6 gap-2');

//     let jumpToPage = `
//         <div class="dt-jump-to-page d-flex align-items-center align-self-md-start align-self-center">
//             <input type="number" min="1" class="form-control form-control-sm me-2" id="jump-to-page" placeholder="Lompat ke" style="width: 100px;">
//             <button class="btn btn-sm btn-dark" id="jump-to-btn">Go</button>
//         </div>
//     `;

//     // Insert it after pagination controls
//     $(jumpToPage).insertAfter('#datatable-buttons_wrapper .dt-paging');

//     function jumpToSpecifiedPage() {
//         let page = parseInt($('#jump-to-page').val(), 10) - 1;
//         if (!isNaN(page) && page >= 0 && page < table.page.info().pages) {
//             table.page(page).draw('page');
//         }
//     }

//     // Handle 'Go' button click
//     $(document).on('click', '#jump-to-btn', function () {
//         jumpToSpecifiedPage();
//     });

//     // Handle 'Enter' keypress in input
//     $(document).on('keypress', '#jump-to-page', function (e) {
//         if (e.which === 13) {
//             e.preventDefault();
//             jumpToSpecifiedPage();
//         }
//     });

//     // ==== FUNGSI FILTER DATE RANGE + TOGGLE ICONS + CLEAR INPUT (START) ====

//     // Insert Filter Card Above Search Input 
//     const layoutEnd = $('#datatable-buttons_wrapper .dt-layout-end:eq(0)');

//     const filterRow = `
//         <div class="row mb-2">
//             <div class="col-12">
//                 <div class="card shadow-none border-dark border m-0">
//                 <div class="card-body p-3">
//                     <div class="row align-items-center justify-content-between g-2">
//                     <div class="col-md-2 ps-3">
//                         <label class="form-label mb-0">Filter :</label>
//                     </div>
//                     <div class="col-md-10">
//                         <div class="d-flex align-items-center">
//                         <div class="input-group date-input-group">
//                             <input type="text" id="tanggal_awal" class="form-control" placeholder="Tanggal Awal" autocomplete="off">
//                             <span class="input-group-text">
//                             <i id="icon-awal-calendar" class="mdi mdi-calendar"></i>
//                             <i id="icon-awal-clear" class="mdi mdi-close text-muted" style="cursor:pointer; display:none;"></i>
//                             </span>
//                         </div>
//                         <span class="mx-2">-</span>
//                         <div class="input-group date-input-group">
//                             <input type="text" id="tanggal_akhir" class="form-control" placeholder="Tanggal Akhir" autocomplete="off">
//                             <span class="input-group-text">
//                             <i id="icon-akhir-calendar" class="mdi mdi-calendar"></i>
//                             <i id="icon-akhir-clear" class="mdi mdi-close text-muted" style="cursor:pointer; display:none;"></i>
//                             </span>
//                         </div>
//                         </div>
//                     </div>
//                     </div>
//                 </div>
//                 </div>
//             </div>
//         </div>`;

//     // Wrap the existing dt-search in its own row
//     const searchDiv = layoutEnd.find('.dt-search').detach();
//     const searchRow = $('<div class="row w-100"></div>').append(searchDiv);

//     // Clear layout-end and append both rows
//     layoutEnd.empty().append(filterRow).append(searchRow);

//     // --- Custom Date Range Filter ---
//     $('#tanggal_awal, #tanggal_akhir').datepicker({
//         format: "dd/mm/yyyy",
//         autoclose: true,
//         todayHighlight: true,
//         orientation: "bottom auto"
//     });

//     // --- Helper: parse dd/mm/yyyy to Date ---
//     function parseDate(str) {
//         if (!str) return null;
//         var parts = str.split('/');
//         return new Date(parts[2], parts[1] - 1, parts[0]);
//     }

//     // --- Helper: toggle icons ---
//     function toggleIcons(input, calendarIcon, clearIcon) {
//         if ($(input).val()) {
//             $(calendarIcon).hide();
//             $(clearIcon).show();
//         } else {
//             $(calendarIcon).show();
//             $(clearIcon).hide();
//         }
//     }

//     // --- Bind date input behavior ---
//     function bindDateInput(inputId, calIconId, clearIconId) {
//         $(inputId).on('input change', function () {
//             toggleIcons(this, calIconId, clearIconId);

//             var startDate = parseDate($('#tanggal_awal').val());
//             var endDate = parseDate($('#tanggal_akhir').val());

//             // Validate date order
//             if (startDate && endDate && endDate < startDate) {
//                 Swal.fire({
//                     icon: 'error',
//                     title: 'Tanggal tidak valid',
//                     text: 'Tanggal akhir tidak boleh lebih kecil dari tanggal awal',
//                     confirmButtonText: 'OK'
//                 });
//                 $(this).val('').datepicker('update', null);
//                 toggleIcons(this, calIconId, clearIconId);
//                 return;
//             }

//             table.draw(); // redraw if valid
//         });

//         // Clear with ❌
//         $(clearIconId).on('click', function () {
//             $(inputId).val('').datepicker('update', null).trigger('change');
//         });

//         // Initial state
//         toggleIcons(inputId, calIconId, clearIconId);
//     }

//     // --- Attach to inputs ---
//     bindDateInput('#tanggal_awal', '#icon-awal-calendar', '#icon-awal-clear');
//     bindDateInput('#tanggal_akhir', '#icon-akhir-calendar', '#icon-akhir-clear');

//     // --- DataTable custom date filter ---
//     $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
//         var min = parseDate($('#tanggal_awal').val());
//         var max = parseDate($('#tanggal_akhir').val());

//         var dateStr = data[4]; // column index of "tanggal"
//         if (!dateStr) return false; // skip rows without date

//         var parts = dateStr.split('/');
//         var rowDate = new Date(parts[2], parts[1] - 1, parts[0]);

//         if (
//             (min === null || rowDate >= min) &&
//             (max === null || rowDate <= max)
//         ) {
//             return true;
//         }
//         return false;
//     });
// });