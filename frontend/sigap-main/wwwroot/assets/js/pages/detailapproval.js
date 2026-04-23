/*$(function () {
    // DataTable initialization
    const detailapproval = [
        {
            nama_Barang: "Helm Pertamina (merah)",
            Jumlah: "1",
            Satuan: "pcs",
            keterangan: "-",
        },

        // Randomly generated entries below...
    ];
    // Generate random data for testing
    for (let i = 0; i < 10; i++) {
        detailapproval.push({
            nama_Barang: "Helm Pertamina (merah)",
            jumlah: Math.floor(Math.random() * 500) + 1,
            status: ["Pending", "Approved", "Rejected"][Math.floor(Math.random() * 3)],
            no_pegawai: "312312312",
            bagian: "MA II",
            tanggal: `${Math.floor(Math.random() * 28) + 1}/${Math.floor(Math.random() * 12) + 1}/${Math.floor(Math.random() * 24) + 2000}`,
            nama_barang: `Barang Dummy ${i + 1}`,

        });
    }

    // Initialize DataTable with buttons and customizations
    let table = $('#datatable-buttons').DataTable({
        lengthChange: true,
        //buttons: ['copy', 'excel', 'pdf', 'colvis'],
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
        data: detailapproval,
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'control noVis',
                orderable: false
            },
            {
                data: null,
                render: DataTable.render.select(),
                className: 'all noVis',
                orderable: false
            },
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                },
                title: 'No',
                className: 'all noVis dt-center',
            },
             {
                 data: null,
                 render: function (data, type, row, meta) {
                     return meta.row + 1;
                 },
                 title: 'No',
                 className: 'all noVis dt-center',
             },

            //kolom gambar + nama peminjam
            {
                data: null,
                title: "Nama Peminjam",
                render: function (data, type, row) {
                    return `
                        <div class="d-flex img-cell align-items-center gap-1">
                            <img src="${row.gambar}" alt="${row.nama_peminjam}" class="rounded-circle header-profile-user me-2">
                            <span>${row.nama_peminjam}</span>
                        </div>
                    `;
                }
            },
            //kolom status
            {
                data: "status", title: "Status",
                render: function (data, type, row) {
                    switch (data) {
                        case 'Pending':
                            return `<span class="badge bg-warning">${data}</span>`;
                            break;
                        case 'Approved':
                            return `<span class="badge bg-success">${data}</span>`;
                            break;
                        case 'Rejected':
                            return `<span class="badge bg-danger">${data}</span>`;
                            break;
                        case 'Done':
                            return `<span class="badge bg-info">${data}</span>`;
                            break;

                    }
                }
            },
            //kolom pegawai
            {
                data: "no_pegawai",
                title: "No Pegawai",
            },
            //kolom bagian 
            { data: "bagian", title: "Bagian" },
            //kolom tanggal
            { data: "tanggal", title: "Tanggal" },
            //kolom nama barang
            //{ data: "nama_barang", title: "Nama Barang" },
            {
                data: "nama_barang", title: "Nama Barang",
                render: function (data, type, row) {
                    return `<span class="dt-wrap line-clamp-4">${data}</span>`;
                },
            },
            //kolom jumlah
            { data: "jumlah", title: "Jumlah" },
            //kolom aksi
            {
                data: null,
                title: 'Aksi',
                render: function (data, type, row) {
                    return `
                        <div class="d-flex gap-2">
                            <a href="/Approval/DetailDataPermintaanBarang" class="btn btn-info">
                                <i class="mdi mdi-file-find-outline"></i>
                                <span>Detail</span>
                            </a>
                        </div>
                    `;
                },
                className: 'dt-center noVis',
                orderable: false
            }
        ],
        select: {
            style: 'multi',
            selector: 'td:nth-child(2)',
            headerCheckbox: 'select-page',
        },
        order: [[2, 'asc']],
        columnDefs: [
            { className: 'dt-center align-middle', targets: '_all' },
            {
                targets: [3],
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
               <a href="#" class="btn btn-success text-white">
                   <i class="mdi mdi-check-circle-outline"></i>
                   <span>Approve Pengawas Gudang</span>
               </a>
               <a href="#" class="btn btn-danger text-white">
                   <i class="mdi mdi-close-octagon-outline"></i>
                   <span>Reject Pengawas Gudang</span>
               </a>
               <a href="#" class="btn btn-success text-white">
                   <i class="mdi mdi-check-circle-outline"></i>
                   <span>Approve Health</span>
               </a>
               <a href="#" class="btn btn-danger text-white">
                   <i class="mdi mdi-close-octagon-outline"></i>
                   <span>Reject Health</span>
               </a>
               <a href="#" class="btn btn-success text-white">
                   <i class="mdi mdi-check-circle-outline"></i>
                   <span>Approve Safety</span>
               </a>
               <a href="#" class="btn btn-danger text-white">
                   <i class="mdi mdi-close-octagon-outline"></i>
                   <span>Reject Safety</span>
               </a>
       </div>
    `;

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
    table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');

    $('#datatable-buttons thead').addClass('table-dark');

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
        .removeClass('align-items-center')
        .addClass('align-items-end');

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
        .removeClass('col-md-auto')
        .addClass('col-md-6');

    //$('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
    //$('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
    //$('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
    //   .removeClass('col-md-auto justify-content-between')
    //   .addClass('col-md-6 flex-wrap justify-content-end gap-2');

    $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
    $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
    $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
        .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
        //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
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
});*/