$(function () {
    // DataTable initialization
    const datariwayattransaksi = [
        {
            gambar: "/assets/images/users/user-4.jpg",
            nama_pekerja: "SuperUser_1",
            //nama_pekerja: "SuperUser_1 SuperUser_1 SuperUser_1SuperUser_1SuperUser_1SuperUser_1SuperUser_1SuperUser_1SuperUser_1SuperUser_1asdlalsdjalksdjalksjdlaksjdlkajsd",
            no_pegawai: "231121212",
            perusahaan: "Pertamina cabang IV",
            bagian: "HSSE",
            tanggal: "10/12/23",
            inex: "IN",
            status: "Pending"
        },
        {
            gambar: "/assets/images/users/user-6.jpg",
            nama_pekerja: "Evan",
            no_pegawai: "231121211",
            perusahaan: "Pertamina cabang IV",
            bagian: "MA II",
            tanggal: "09/12/23",
            inex: "IN",
            status: "Rejected"
        },
      
        // Randomly generated entries below...
    ];
    // Generate random data for testing
    for (let i = 0; i < 994; i++) {
        datariwayattransaksi.push({
            gambar: "/assets/images/users/user-1.jpg",
            nama_pekerja: "Teguh",
            no_pegawai: "312312312",
            perusahaan: "Pertamina cabang IV",
            bagian: "MA II",
            tanggal: `${Math.floor(Math.random() * 28) + 1}/${Math.floor(Math.random() * 12) + 1}/${Math.floor(Math.random() * 24) + 2000}`,
            inex: ["IN", "EX"][Math.floor(Math.random() * 2)],
            status: ["Pending", "Approved", "Rejected"][Math.floor(Math.random() * 3)],
        });
    }

    // Initialize DataTable with buttons and customizations
    let table = $('#datatable-buttons').DataTable({
        lengthChange: true,
        //buttons: ['copy', 'excel', 'pdf', 'colvis'],
        buttons: [
            'copy','excel','pdf',
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
        data: datariwayattransaksi,
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
            },

            //kolom gambar + nama pekerja
            {
                data: "usersCacheDto.nama_pekerja",
                title: "Nama Pekerja",
                render: function (data, type, row) {
                    const nama =
                        row.employeeDto?.nama_pekerja ??
                        row.usersCacheDto?.nama_pekerja ??
                        "-";

                    const foto = row.gambar || "/assets/images/pngwing.png";

                    return `
                        <div class="d-flex img-cell align-items-center gap-1">
                            <img 
                                src="${foto}"
                                alt="${nama}"
                                class="rounded-circle header-profile-user me-2"
                                onerror="this.onerror=null; this.src='/assets/images/pngwing.png';">
                            <span>${nama}</span>
                        </div>
                    `;
                },
                orderable: true,
                searchable: true
            },
            {
                data: "employeeDto.nama_pekerja",
                visible: false,
                searchable: true,
                orderable: false
            },
            {
                data: "usersCacheDto.bagian_pekerja",
                visible: false,
                searchable: true,
                orderable: false
            },
            {
                data: "employeeDto.bagianUserDto.nama",
                visible: false,
                searchable: true,
                orderable: false
            },

            //kolom pegawai
            {
                data: "no_pegawai",
                title: "No Pegawai",
            },
            //kolom perusahaan
            {
                data: "perusahaan",
                title: "Perusahaan",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
            },
            //kolom tanggal
            { data: "tanggal", title: "Tanggal" },
            //kolom bagian 
            { data: "bagian", title: "Bagian" },
            //kolom in/ex
            { data: "inex", title: "IN/EX" },
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
            //kolom aksi
            {
                data: null,
                title: 'Aksi',
                render: function (data, type, row) {
                    return `
                        <div class="d-flex gap-2">
                            <a href="/RiwayatTransaksi/DetailRiwayatTransaksi" class="btn btn-info">
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
        //select: {
        //    style: 'multi',
        //    selector: 'td:nth-child(2)',
        //    headerCheckbox: 'select-page',
        //},
        order: [[1, 'asc']],
        columnDefs: [
            { className: 'dt-center align-middle', targets: '_all' },
                {
                    //targets: [4,6],
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
        <!--
        <a href="/riwayattransaksi" class="btn btn-success text-white">
                <i class="mdi mdi-file-plus-outline"></i>
                <span>Riwayat Transaksi</span>
            </a>
        <a href="/RiwayatTransaksi/RiwayatStock" class="btn btn-success text-white">
                <i class="mdi mdi-file-plus-outline"></i>
                <span>Riwayat Stock</span>
            </a> -->
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
});