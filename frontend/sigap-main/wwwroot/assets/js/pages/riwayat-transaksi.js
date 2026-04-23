$(function () {
    const { userId, hasRiwayatStock } = window.transaksi;

    let table = $('#datatable-buttons').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/api/transaksi/datatable',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                let tanggal = d.columns.find(c => c.data === "created_at");
                let tanggalAwal = $('#tanggal_awal_payload').val();
                let tanggalAkhir = $('#tanggal_akhir_payload').val();
                let tanggalSearchPayload = '';

                d.columns.push({
                    // data: "users_cache_id",
                    data: "usersCacheDto.user_id",
                    searchable: true,
                    orderable: false,
                    search: { value: `${userId}` }
                });

                if(tanggalAwal) {
                    tanggalSearchPayload += tanggalAwal;
                }

                if(tanggalAkhir) {
                    if(tanggalSearchPayload != '') tanggalSearchPayload = tanggalSearchPayload + ',' + tanggalAkhir;
                    else tanggalSearchPayload += tanggalAkhir;
                }

                tanggal.search.value = tanggalSearchPayload;

                return JSON.stringify(d);
            },
            dataSrc: function (res) {
                const data = res?.data?.data || res?.data || [];
                return Array.isArray(data) ? data : [];
            },
            error: function (xhr) {
                const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal memuat data.';
                Swal.fire('Error', msg, 'error');
            }
        },
        lengthChange: true,
        buttons: [
            
            {
                extend: 'colvis',
                columns: ':not(.noVis)',
                className: 'btn btn-dark',
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
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'control noVis',
                orderable: false,
                searchable: false
            },
            {
                data: null,
                title: "No",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                orderable: false,
                searchable: false,
            },

            //kolom gambar + nama pekerja
            {
                data: null,
                title: "Nama Pekerja",
                render: function (data, type, row) {
                    let nama_pekerja = '';

                    if (row.employeeDto)
                        nama_pekerja = row.employeeDto.nama_pekerja;
                    else
                        nama_pekerja = row.usersCacheDto.nama_pekerja;

                    return `
                            <div class="d-flex img-cell align-items-center gap-1">
                                <img 
                                    src="${row.gambar}"
                                    alt="${nama_pekerja}"
                                    class="rounded-circle header-profile-user me-2"
                                    onerror="this.onerror=null; this.src='/assets/images/pngwing.png';">
                                <span>${nama_pekerja}</span>
                            </div>
                        `;
                },
                orderable: true,
                searchable: true
            },

            //kolom pegawai
            {
                data: "categoryEmployeeDto.nama_kategori",
                title: "Jenis Pekerja",
                orderable: true,
                searchable: true
            },
            //kolom perusahaan
            {
                // data: null,
                data: "employeeDto.perusahaan_pekerja",
                title: "Perusahaan",
                render: function (data, type, row) {
                    let perusahaan = '';

                    if (row.employeeDto)
                        perusahaan = row.employeeDto.perusahaan_pekerja;
                    else
                        perusahaan = 'Internal Pertamina'; // Sementara
                    return `<span class="dt-wrap">${perusahaan}</span>`;
                },
                // orderable: false,
                // searchable: false
                orderable: true,
                searchable: true
            },
            //kolom tanggal
            { 
                data: "created_at", 
                title: "Tanggal",
                render: function (data, type, row) {
                    // let tanggal = row.created_at + 'WIB';
                    const tanggal = data.split(' ')[0];

                    return `<span class="dt-wrap">${tanggal}</span>`;
                },
                orderable: true,
                searchable: true
            },
            // { data: "tanggal", title: "Tanggal" },
            // {
            //     data: 'tanggal',
            //     title: 'Tanggal',
            //     render: function (data, type, row) {
            //         // Sorting/Filtering uses raw Date object
            //         if (type === 'sort' || type === 'type') {
            //             return data;
            //         }
            //         // Display format dd/mm/yyyy
            //         if (data instanceof Date) {
            //             return data.toLocaleDateString("id-ID", {
            //                 day: "2-digit",
            //                 month: "2-digit",
            //                 year: "numeric"
            //             });
            //         }
            //         return data;
            //     }
            // },
            // //kolom bagian 
            // { data: "bagian", title: "Bagian" },
            //kolom fungsi 
            { 
                // data: null,
                data: "usersCacheDto.bagian_pekerja",
                title: "Bagian",
                render: function (data, type, row) {
                    let bagian = '';

                    if (row.employeeDto)
                        bagian = row.employeeDto.bagianUserDto.nama;
                    else
                        bagian = row.usersCacheDto.bagian_pekerja;

                    return `<span class="dt-wrap">${bagian}</span>`;
                },
                orderable: true,
                searchable: true
            },
            { 
                // data: null,
                data: "usersCacheDto.fungsi_pekerja",
                title: "Fungsi",
                render: function (data, type, row) {
                    let fungsi = '';

                    if (row.employeeDto)
                        fungsi = row.employeeDto.fungsi_pekerja;
                    else
                        fungsi = row.usersCacheDto.fungsi_pekerja;

                    return `<span class="dt-wrap">${fungsi}</span>`;
                },
                orderable: true,
                searchable: true
            },
            //kolom in/ex
            { 
                data: "kategori_transact_id", 
                // title: "IN/EX",
                title: "IN/OUT",
                // render: function (data, type, row) {
                //     // if (data === 'OUT') data = 'EX';
                //     // return `<span class="dt-wrap">${data}</span>`;
                //     return `<span class="dt-wrap">${data === 'OUT' ? 'EX' : data}</span>`;
                // },
                orderable: true,
                searchable: true
            },
            // //kolom status
            {
                data: "status", title: "Status",
                render: function (data, type, row) {
                    switch (data) {
                        // case 'Pending':
                        //     return `<span class="badge bg-warning">${data}</span>`;
                        //     break;
                        // case 'Approved':
                        //     return `<span class="badge bg-success">${data}</span>`;
                        //     break;
                        // case 'Rejected':
                        //     return `<span class="badge bg-danger">${data}</span>`;
                        //     break;
                        // case 'Done':
                        //     return `<span class="badge bg-info">${data}</span>`;
                        //     break;
                        case 'Approval Section Head Safety Pending':
                            return `<span class="badge bg-warning">Pending</span>`;
                            break;
                        case 'Approval Section Head Pending':
                            return `<span class="badge bg-warning">Pending</span>`;
                            break;
                        case 'Menunggu Konfirmasi Gudang':
                            return `<span class="badge bg-warning">Pending</span>`;
                            break;
                        case 'Approval Section Head Rejected':
                            return `<span class="badge bg-danger">Rejected</span>`;
                            break;
                        case 'Approval Section Head Safety Rejected':
                            return `<span class="badge bg-danger">Rejected</span>`;
                            break;
                        case 'Approval Gudang Rejected':
                            return `<span class="badge bg-danger">Rejected</span>`;
                            break;
                        case 'Done':
                            return `<span class="badge bg-success">Done</span>`;
                            break;
                    }
                },
                // orderable: false,
                // searchable: false
                orderable: true,
                searchable: true
            },
            //kolom aksi
            {
                data: 'transact_id',
                title: 'Aksi',
                render: function (data, type, row) {
                    return `
                        <div class="d-flex gap-2">
                            <a class="btn btn-info btn-detail" data-id="${data}">
                                <i class="mdi mdi-file-find-outline"></i>
                                <span>Detail</span>
                            </a>
                        </div>
                    `;
                },
                className: 'dt-center noVis',
                orderable: false,
                searchable: false
            }
        ],
        order: [[5, 'desc']],
        columnDefs: [
            { className: 'dt-center align-middle', targets: '_all' },
                {
                    //targets: [4,6],
                    targets: [2, 3, 4, 6],
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

    const customButtons = `
        <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
            <!-- a href="/RiwayatTransaksi/RiwayatStock" class="btn btn-success text-white">
                <i class="mdi mdi-file-plus-outline"></i>
                <span>Riwayat Stock</span>
            </a -->
        </div>
    `;

    $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);

    if(hasRiwayatStock){
        $('#datatable-buttons_wrapper_custom').append(`
            <a href="/RiwayatTransaksi/RiwayatStock" class="btn btn-success text-white">
                <i class="mdi mdi-file-plus-outline"></i>
                <span>Riwayat Stock</span>
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

            // table.draw(); // redraw if valid
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

    //     var dateStr = data[5]; // column index of "tanggal"
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

    // detail approval
    $(document).on('click', '.btn-detail', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Memuat data...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        $.ajax({
            url: `/api/Transaksi/${id}`,
            method: 'GET',
            success: function (res) {
                Swal.close();
                const data = res.data || res;
                const tanggal = data.created_at.split(' ')[0];
                const details = data.transactionDetailDto || [];
                const tbody = $('#detailModal tbody');
                tbody.empty(); // clear previous rows

                details.forEach((detail, index) => {
                    const item = detail.itemDto || {};
                    const satuan = item.uomDto?.nama_satuanbar || item.satuanbar_id || '-';
                    const namaBarang = item.nama_barang || '-';
                    const jumlah = detail.jumlah_bar ?? '-';

                    const row = `
                        <tr>
                            <td class="text-center">${index + 1}</td>
                            <td>${namaBarang}</td>
                            <td class="text-center">${jumlah}</td>
                            <td class="text-center">${satuan}</td>
                        </tr>
                    `;
                    tbody.append(row);
                });

                if (data.kategori_transact_id == "OUT") {
                    $('#penanggung-jawab').show();

                    let namapenanggungjawab = '';
                    let fungsipenanggungjawab = '';
                    let statustransaksi = data.status;
                    let badge = '';
                    $('#keterangan').show();
                    $('#statustransaksi').addClass('mb-1');
                    $('#labelstatustransaksi').addClass('mb-1');
                    switch (statustransaksi) {
                        case 'Approval Section Head Pending':
                            $('#keterangan').removeClass('alert-danger');
                            $('#keterangan').addClass('alert-warning');
                            badge = `<span class="badge bg-warning">Pending</span>`;
                            namapenanggungjawab = data.approvalManajemenPekerjaIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalManajemenPekerjaIdDto.usersCacheDto.fungsi_pekerja;
                            break;
                        case 'Approval Section Head Safety Pending':
                            $('#keterangan').removeClass('alert-danger');
                            $('#keterangan').addClass('alert-warning');
                            badge = `<span class="badge bg-warning">Pending</span>`;
                            namapenanggungjawab = data.approvalSectionheadIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalSectionheadIdDto.usersCacheDto.fungsi_pekerja;
                            break;
                        case 'Menunggu Konfirmasi Gudang':
                            $('#keterangan').removeClass('alert-danger');
                            $('#keterangan').addClass('alert-warning');
                            badge = `<span class="badge bg-warning">Pending</span>`;
                            namapenanggungjawab = data.approvalGudangIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalGudangIdDto.usersCacheDto.fungsi_pekerja;
                            break;
                        case 'Approval Section Head Rejected':
                            $('#keterangan').removeClass('alert-warning');
                            $('#keterangan').addClass('alert-danger');
                            badge = `<span class="badge bg-danger">Rejected</span>`;
                            namapenanggungjawab = data.approvalManajemenPekerjaIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalManajemenPekerjaIdDto.usersCacheDto.fungsi_pekerja;
                            statustransaksi = data.approvalManajemenPekerjaIdDto.remark || statustransaksi;
                            break;
                        case 'Approval Section Head Safety Rejected':
                            $('#keterangan').removeClass('alert-warning');
                            $('#keterangan').addClass('alert-danger');
                            badge = `<span class="badge bg-danger">Rejected</span>`;
                            namapenanggungjawab = data.approvalSectionheadIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalSectionheadIdDto.usersCacheDto.fungsi_pekerja;
                            statustransaksi = data.approvalSectionheadIdDto.remark || statustransaksi;
                            break;
                        case 'Approval Gudang Rejected':
                            $('#keterangan').removeClass('alert-warning');
                            $('#keterangan').addClass('alert-danger');
                            badge = `<span class="badge bg-danger">Rejected</span>`;
                            namapenanggungjawab = data.approvalGudangIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalGudangIdDto.usersCacheDto.fungsi_pekerja;
                            statustransaksi = data.approvalGudangIdDto.remark || statustransaksi;
                            break;
                        case 'Done':
                            badge = `<span class="badge bg-success">Done</span>`;
                            namapenanggungjawab = data.approvalGudangIdDto.usersCacheDto.nama_pekerja;
                            fungsipenanggungjawab = data.approvalGudangIdDto.usersCacheDto.fungsi_pekerja;
                            $('#keterangan').hide();
                            $('#statustransaksi').removeClass('mb-1');
                            $('#labelstatustransaksi').removeClass('mb-1');
                            break;
                    }

                    const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                    $('body').css('padding-right', scrollbarWidth + 'px');

                    $('#kategori-transaksi').html(': ' + data.kategori_transact_id);
                    $('#no-miv-safety').html(': ' + data.no_miv_safety);
                    // $('#no-miv-safety').html('Nomor MIV: ' + data.no_miv_safety);

                    if (data.employeeDto) {
                        $('#id-finger-row').show();
                        $('#filependukung-row').show();
                        // $('#id-finger').show();
                        // $('#filependukung').show();

                        let link_file_pendukung = data.employeeDto.link_file_pendukung;

                        if (link_file_pendukung && !/^https?:\/\//i.test(link_file_pendukung)) {
                            // If string does NOT start with http:// or https://, prepend API_URL
                            link_file_pendukung = API_URL.replace(/\/+$/, '') + '/' + link_file_pendukung.replace(/^\/+/, '');
                        }

                        $('#id-finger').html(': ' + data.employeeDto.id_finger);
                        $('#bagian').html(': ' + data.employeeDto.bagianUserDto.nama);
                        $('#fungsi').html(': ' + data.employeeDto.fungsi_pekerja);
                        $('#namapekerja').html(': ' + data.employeeDto.nama_pekerja);
                        $('#filependukung').html(': ' + `<a href="${link_file_pendukung}" target="_blank" rel="noopener noreferrer">Klik untuk membuka file</a>`);
                        // $('#id-finger').html('ID Finger : ' + data.employeeDto.id_finger);
                        // $('#fungsi').html('Fungsi : ' + data.employeeDto.fungsi_pekerja);
                        // $('#namapekerja').html('Nama Pekerja : ' + data.employeeDto.nama_pekerja);
                        // $('#filependukung').html('File Pendukung : ' + `<a href="${link_file_pendukung}" target="_blank" rel="noopener noreferrer">Klik untuk membuka file</a>`);
                    }
                    else {
                        $('#id-finger-row').hide();
                        $('#filependukung-row').hide();
                        // $('#id-finger').hide();
                        // $('#filependukung').hide();

                        $('#bagian').html(': ' + data.usersCacheDto.bagian_pekerja);
                        $('#fungsi').html(': ' + data.usersCacheDto.fungsi_pekerja);
                        $('#namapekerja').html(': ' + data.usersCacheDto.nama_pekerja);
                        // $('#fungsi').html('Fungsi : ' + data.usersCacheDto.fungsi_pekerja);
                        // $('#namapekerja').html('Nama Pekerja : ' + data.usersCacheDto.nama_pekerja);
                        // $('#id-finger').html('ID Finger : -');
                        // $('#filependukung').html('File Pendukung : -');
                    }

                    $('#jenispekerja').html(': ' + data.categoryEmployeeDto.nama_kategori);
                    // $('#jenispekerja').html('Jenis Pekerja : ' + data.categoryEmployeeDto.nama_kategori);
                    // $('#jenispekerja').html('Jenis Pekerja: ' + data.kategori_pekerja);
                    $('#statustransaksi').html(': ' + badge);
                    // $('#statustransaksi').html('Status Transaksi : ' + status);
                    // $('#keterangan').css('white-space', 'pre-wrap').text('  ' + statustransaksi);
                    $('#keterangan').html(statustransaksi);

                    $('#tanggal').html(': ' + tanggal);
                    // $('#tanggal').html('Tanggal : ' + tanggal);
                    // $('#tanggal').html('Tanggal : ' + data.created_at);

                    $('#namapekerjaapprove').html(': ' + namapenanggungjawab);
                    $('#fungsipekerjaapprove').html(': ' + fungsipenanggungjawab);
                    // $('#namapekerjaapprove').html('Nama Pekerja : ' + namapenanggungjawab);
                    // $('#fungsipekerjaapprove').html('Fungsi : ' + fungsipenanggungjawab);
                } else {
                    let statustransaksi = data.status;
                    let badge = '';
                    $('#keterangan').show();
                    $('#statustransaksi').addClass('mb-1');
                    $('#labelstatustransaksi').addClass('mb-1');
                    switch (statustransaksi) {
                        case 'Approval Section Head Pending':
                            $('#keterangan').removeClass('alert-danger');
                            $('#keterangan').addClass('alert-warning');
                            badge = `<span class="badge bg-warning">Pending</span>`;
                            break;
                        case 'Approval Section Head Safety Pending':
                            $('#keterangan').removeClass('alert-danger');
                            $('#keterangan').addClass('alert-warning');
                            badge = `<span class="badge bg-warning">Pending</span>`;
                            break;
                        case 'Menunggu Konfirmasi Gudang':
                            $('#keterangan').removeClass('alert-danger');
                            $('#keterangan').addClass('alert-warning');
                            badge = `<span class="badge bg-warning">Pending</span>`;
                            break;
                        case 'Approval Section Head Rejected':
                            $('#keterangan').removeClass('alert-warning');
                            $('#keterangan').addClass('alert-danger');
                            badge = `<span class="badge bg-danger">Rejected</span>`;
                            statustransaksi = data.approvalManajemenPekerjaIdDto.remark || statustransaksi;
                            break;
                        case 'Approval Section Head Safety Rejected':
                            $('#keterangan').removeClass('alert-warning');
                            $('#keterangan').addClass('alert-danger');
                            badge = `<span class="badge bg-danger">Rejected</span>`;
                            statustransaksi = data.approvalSectionheadIdDto.remark || statustransaksi;
                            break;
                        case 'Approval Gudang Rejected':
                            $('#keterangan').removeClass('alert-warning');
                            $('#keterangan').addClass('alert-danger');
                            badge = `<span class="badge bg-danger">Rejected</span>`;
                            statustransaksi = data.approvalGudangIdDto.remark || statustransaksi;
                            break;
                        case 'Done':
                            badge = `<span class="badge bg-success">Done</span>`;
                            $('#keterangan').hide();
                            $('#statustransaksi').removeClass('mb-1');
                            $('#labelstatustransaksi').removeClass('mb-1');
                            break;
                    }

                    $('#id-finger-row').hide();
                    $('#filependukung-row').hide();
                    $('#penanggung-jawab').hide();

                    $('#kategori-transaksi').html(': ' + data.kategori_transact_id);
                    $('#no-miv-safety').html(': ' + data.no_miv_safety);
                    $('#tanggal').html(': ' + tanggal);
                    $('#namapekerja').html(': ' + data.usersCacheDto.nama_pekerja);
                    $('#jenispekerja').html(': ' + data.categoryEmployeeDto.nama_kategori);
                    $('#bagian').html(': ' + data.usersCacheDto.bagian_pekerja);
                    $('#fungsi').html(': ' + data.usersCacheDto.fungsi_pekerja);
                    $('#statustransaksi').html(': ' + badge);
                    $('#keterangan').html(statustransaksi);
                }

                $('#detailModal').modal('show');
            },
            error: function (xhr) {
                // Swal.close();
                // const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal memuat data transaksi.';
                // Swal.fire('Error', msg, 'error');
                Swal.close();
                let res = xhr.responseJSON;
                let msg = res?.message || 'Gagal memuat data transaksi.';

                if (res?.errors) {
                    // Get first error key and its first message
                    const firstKey = Object.keys(res.errors)[0];
                    const firstError = res.errors[firstKey]?.[0];

                    if (firstError) {
                        msg = firstError; // show only that message
                    }
                }

                Swal.fire('Error', msg, 'error');
            }
        });
        // $('#detailModal').modal('show');
    });
});