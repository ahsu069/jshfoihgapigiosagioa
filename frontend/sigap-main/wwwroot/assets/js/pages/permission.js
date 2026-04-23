$(function () {
    const { hasPermissionCreate, hasPermissionEdit, hasPermissionDelete, hasPermissionAkses } = window.Role
    // let permissionCodeList = [
    //     'approval:approve',
    //     'approval:reject',
    //     'approval:review',
    //     'barang:create',
    //     'barang:delete',
    //     'barang:read',
    //     'barang:update',
    //     'kategori_barang:create',
    //     'kategori_barang:delete',
    //     'kategori_barang:read',
    //     'kategori_barang:update',
    //     'rbac:assign',
    //     'rbac:permission:manage',
    //     'rbac:role:manage',
    //     'satuan:read',
    //     'transaksi:create',
    //     'transaksi:read',
    //     'transaksi:update',
    //     // Tambahan
    //     'dashboard:read',
    //     'user:create',
    //     'user:read',
    //     'user:update',
    //     'user:delete',
    //     'role:create',
    //     'role:read',
    //     'role:update',
    //     'role:delete',
    //     'permission:create',
    //     'permission:read',
    //     'permission:update',
    //     'permission:delete',
    //     'approval:read',
    // ];

    // let permissionCodeList = [
    //     // Currated
    //     { kode: 'approval:approve', nama: 'approval:approve', deskripsi: 'Melakukan approval' },
    //     { kode: 'approval:read', nama: 'approval:read', deskripsi: 'Lihat daftar approval' },
    //     { kode: 'user:read', nama: 'user:read', deskripsi: 'Lihat data pengguna' },
    //     { kode: 'role:read', nama: 'role:read', deskripsi: 'Lihat data role' },
    //     { kode: 'permission:read', nama: 'permission:read', deskripsi: 'Lihat daftar permission' },
    //     { kode: 'user:create', nama: 'user:create', deskripsi: 'Buat pengguna baru' },
    //     { kode: 'user:update', nama: 'user:update', deskripsi: 'Perbarui data pengguna' },
    //     { kode: 'user:delete', nama: 'user:delete', deskripsi: 'Hapus pengguna' },
    //     { kode: 'role:create', nama: 'role:create', deskripsi: 'Buat role baru' },
    //     { kode: 'role:update', nama: 'role:update', deskripsi: 'Perbarui role' },
    //     { kode: 'role:delete', nama: 'role:delete', deskripsi: 'Hapus role' },
    //     { kode: 'permission:create', nama: 'permission:create', deskripsi: 'Buat permission baru' },
    //     { kode: 'permission:update', nama: 'permission:update', deskripsi: 'Perbarui permission' },
    //     { kode: 'permission:delete', nama: 'permission:delete', deskripsi: 'Hapus permission' },
    //     { kode: 'rbac:permission:manage', nama: 'rbac:permission:manage', deskripsi: 'Kelola daftar permission' },
    //     { kode: 'barang:read', nama: 'barang:read', deskripsi: 'Baca data barang/stock gudang' },
    //     { kode: 'barang:create', nama: 'barang:create', deskripsi: 'Tambah barang' },
    //     { kode: 'barang:delete', nama: 'barang:delete', deskripsi: 'Hapus barang' },
    //     { kode: 'barang:update', nama: 'barang:update', deskripsi: 'Ubah barang' },
    //     { kode: 'kategori_barang:read', nama: 'kategori_barang:read', deskripsi: 'Baca kategori barang' },
    //     { kode: 'kategori_barang:create', nama: 'kategori_barang:create', deskripsi: 'Tambah kategori barang' },
    //     { kode: 'kategori_barang:delete', nama: 'kategori_barang:delete', deskripsi: 'Hapus kategori barang' },
    //     { kode: 'kategori_barang:update', nama: 'kategori_barang:update', deskripsi: 'Ubah kategori barang' },
    //     { kode: 'transaksi:pemasukan', nama: 'transaksi:pemasukan', deskripsi: 'Melakukan pemasukan barang' },
    //     { kode: 'transaksi:permintaan', nama: 'transaksi:permintaan', deskripsi: 'Melakukan permintaan barang' },
    //     { kode: 'transaksi:riwayat_transaksi:read', nama: 'transaksi:riwayat_transaksi:read', deskripsi: 'Lihat Riwayat Transaksi' },
    //     { kode: 'transaksi:riwayat_stock:read', nama: 'transaksi:riwayat_stock:read', deskripsi: 'Lihat Riwayat Stock' },
    // ];

    let permissionCodeList = [
        { kode: 'approval:approve', nama: 'approval:approve', deskripsi: 'Melakukan approval' },
        { kode: 'approval:read', nama: 'approval:read', deskripsi: 'Lihat daftar approval' },

        { kode: 'barang:create', nama: 'barang:create', deskripsi: 'Tambah barang' },
        { kode: 'barang:delete', nama: 'barang:delete', deskripsi: 'Hapus barang' },
        { kode: 'barang:read', nama: 'barang:read', deskripsi: 'Baca data barang/stock gudang' },
        { kode: 'barang:update', nama: 'barang:update', deskripsi: 'Ubah barang' },

        { kode: 'kategori_barang:create', nama: 'kategori_barang:create', deskripsi: 'Tambah kategori barang' },
        { kode: 'kategori_barang:delete', nama: 'kategori_barang:delete', deskripsi: 'Hapus kategori barang' },
        { kode: 'kategori_barang:read', nama: 'kategori_barang:read', deskripsi: 'Baca kategori barang' },
        { kode: 'kategori_barang:update', nama: 'kategori_barang:update', deskripsi: 'Ubah kategori barang' },

        { kode: 'permission:create', nama: 'permission:create', deskripsi: 'Buat permission baru' },
        { kode: 'permission:delete', nama: 'permission:delete', deskripsi: 'Hapus permission' },
        { kode: 'permission:read', nama: 'permission:read', deskripsi: 'Lihat daftar permission' },
        { kode: 'permission:update', nama: 'permission:update', deskripsi: 'Perbarui permission' },

        { kode: 'rbac:permission:manage', nama: 'rbac:permission:manage', deskripsi: 'Kelola daftar permission' },

        { kode: 'role:create', nama: 'role:create', deskripsi: 'Buat role baru' },
        { kode: 'role:delete', nama: 'role:delete', deskripsi: 'Hapus role' },
        { kode: 'role:read', nama: 'role:read', deskripsi: 'Lihat data role' },
        { kode: 'role:update', nama: 'role:update', deskripsi: 'Perbarui role' },

        { kode: 'transaksi:pemasukan', nama: 'transaksi:pemasukan', deskripsi: 'Melakukan pemasukan barang' },
        { kode: 'transaksi:permintaan', nama: 'transaksi:permintaan', deskripsi: 'Melakukan permintaan barang' },
        { kode: 'transaksi:riwayat_stock:read', nama: 'transaksi:riwayat_stock:read', deskripsi: 'Lihat Riwayat Stock' },
        { kode: 'transaksi:riwayat_transaksi:read', nama: 'transaksi:riwayat_transaksi:read', deskripsi: 'Lihat Riwayat Transaksi' },

        { kode: 'user:create', nama: 'user:create', deskripsi: 'Buat pengguna baru' },
        { kode: 'user:delete', nama: 'user:delete', deskripsi: 'Hapus pengguna' },
        { kode: 'user:read', nama: 'user:read', deskripsi: 'Lihat data pengguna' },
        { kode: 'user:update', nama: 'user:update', deskripsi: 'Perbarui data pengguna' }
    ];

    let table = $('#datatable-buttons').DataTable({
        serverSide: true,
        processing: true,
        lengthChange: true,
        ajax: {
            url: '/api/Permission/datatable',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                // Custom (Start)
                // const globalSearch = d.search?.value || "";
                // let namakategoribarColumn = d.columns.find(c => c.data === "namakategoribar");

                // d.columns.push({
                //     data: "is_deleted",
                //     name: "",
                //     searchable: true,
                //     orderable: false,
                //     search: {
                //         value: "false",
                //         regex: false,
                //         fixed: []
                //     }
                // });

                // namakategoribarColumn.search.value = globalSearch;

                // Custom (End)
                return JSON.stringify(d);
            },
            dataSrc: function (res) {
                // Handle nested or flat data structure
                const data = res?.data?.data || res?.data || [];
                // console.log(data);
                return Array.isArray(data) ? data : [];
            },
            error: function (xhr) {
                const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal memuat data.';
                Swal.fire('Error', msg, 'error');
            }
        },
        //buttons: ['copy', 'excel', 'pdf', 'colvis'],
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
        //layout: {
        //    bottomEnd: {
        //    }
        //},
        // data: barangData,
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
            // { data: "id", title: "ID" },
            //{ data: "barang", title: "Barang" },
            {
                data: null,
                title: "No",
                render: function (data, type, row, meta) {
                    // return meta.row + 1;
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                orderable: false,
                searchable: false,
            },
            {
                data: "code",
                title: "Kode Permission",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
                searchable: true,
                orderable: true
            },
            {
                data: "name",
                title: "Nama Permission",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
                searchable: true,
                orderable: true
            },
            {
                data: "description",
                title: "Deskripsi",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
                searchable: true,
                orderable: true
            },

            {
                data: "permission_id",
                title: "Aksi",
                render: function (data, type, row) {
                    if (type !== 'display') {
                        return data;  // atau return '';
                    }
                    let html = '<div class="d-flex gap-2">';
                    if (hasPermissionAkses) {
                        html += `<button class="btn btn-secondary btn-akses" data-id="${data}">
                                <i class="mdi mdi-account-cog"></i>
                                <span>Kelola Akses</span>
                                </button>`;
                    }
                    if (hasPermissionEdit) {
                        html += `<button class="btn btn-info btn-edit" data-id="${data}">
                                <i class="mdi mdi-pencil-outline"></i>
                                <span>Edit</span>
                            </button>`;
                    }
                    if (hasPermissionDelete) {
                        html += `<button class="btn btn-danger btn-delete" data-id="${data}">
                                <i class="mdi mdi-trash-can-outline"></i>
                                <span>Delete</span>
                            </button>`;
                    }
                    html += '</div>';
                    return html; 

                    //return `
                    //    <div class="d-flex gap-2">
                    //        <button class="btn btn-secondary btn-akses" data-id="${data}">
                    //            <i class="mdi mdi-account-cog"></i>
                    //            <span>Kelola Akses</span>
                    //        </button>
                    //        <button class="btn btn-info btn-edit" data-id="${data}">
                    //            <i class="mdi mdi-pencil-outline"></i>
                    //            <span>Edit</span>
                    //        </button>
                    //        <button class="btn btn-danger btn-delete" data-id="${data}">
                    //            <i class="mdi mdi-trash-can-outline"></i>
                    //            <span>Delete</span>
                    //        </button>
                    //    </div>
                    //`;
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
        order: [[3, 'asc']],
        columnDefs: [
            { className: 'dt-center align-middle', targets: '_all' },
            //{
            //targets: '_all',
            //targets: [5],
            //createdCell: function (td, cellData, rowData, row, col) {
            //    td.classList.add('dt-wrap');
            //}
            //},
            {
                //targets: '_all',
                //createdCell: function (td, cellData, rowData, row, col) {
                //    if ([3, 5].includes(col)) {
                //        //td.style.textAlign = 'start';
                //        td.classList.add('text-start');
                //    }
                //}
                targets: [3, 4, 5],
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
        initComplete: function () {
            // length change button
            $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
            $(".dt-length select").addClass('form-select form-select-sm');

            $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
                .removeClass('align-items-center')
            //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
            //.addClass('d-flex flex-column col-md-6 gap-2');

            const customButtons = `
                <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
                    <!--button type="button" class="btn btn-success text-white" id="btnTambahPermission">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button-->
                    <!--button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button-->
                </div>
            `;

            $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
            if (hasPermissionDelete) {
                $('#datatable-buttons_wrapper_custom').prepend(`<button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button>`);
            }
            if (hasPermissionCreate) {
                $('#datatable-buttons_wrapper_custom').prepend(`<button type="button" class="btn btn-success text-white" id="btnTambahPermission">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button>`);
            }
            let th = $('th[data-dt-column="1"]');
            let checkbox = th.find('input.dt-select-checkbox');
            checkbox.detach();
            th.empty();
            th.append(checkbox);


            // Kondisi awal tombol hapus terpilih
            $('#delete-selected').prop('disabled', true);

            // Enable/disable button tergantung pada row selection
            table.on('select deselect', function () {
                const selectedCount = table.rows({ selected: true }).count();
                $('#delete-selected').prop('disabled', selectedCount === 0);
            });

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
        }
    });

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

    // Initialize Select2 for Permission Code in Tambah Form (Subtract existing API codes)
    // $('#permissionCode').select2({
    //     dropdownParent: $('#modalTambahPermission'),
    //     placeholder: 'pilih kode permission',
    //     ajax: {
    //         transport: function (params, success, failure) {
    //             // Fetch /api/Permission data, then subtract
    //             $.ajax({
    //                 url: '/api/Permission',
    //                 method: 'GET',
    //                 dataType: 'json',
    //                 success: function (res) {
    //                     const existing = (res?.data || []).map(p => p.code);
    //                     // Difference between permissionCodeList and existing
    //                     const available = permissionCodeList
    //                         .filter(code => !existing.includes(code))
    //                         .map(code => ({ id: code, text: code }));

    //                     // Filter by search term (if any)
    //                     const term = (params.data?.term || '').toLowerCase();
    //                     const filtered = term
    //                         ? available.filter(x => x.text.toLowerCase().includes(term))
    //                         : available;

    //                     success({ results: filtered });
    //                 },
    //                 error: failure
    //             });
    //         },
    //         processResults: function (data) {
    //             return data;
    //         }
    //     }
    // });

    $('#permissionCode').select2({
        dropdownParent: $('#modalTambahPermission'),
        placeholder: 'pilih kode permission',
        ajax: {
            transport: function (params, success, failure) {
                // Fetch existing permissions from API
                $.ajax({
                    url: '/api/Permission',
                    method: 'GET',
                    dataType: 'json',
                    success: function (res) {
                        // extract existing codes
                        const existing = (res?.data || []).map(p => p.code);

                        // permissionCodeList is now an array of objects
                        const available = permissionCodeList
                            .filter(p => !existing.includes(p.kode))       // remove created ones
                            .map(p => ({ id: p.kode, text: p.nama, deskripsi: p.deskripsi }));    // Select2 format

                        // Search term filter
                        const term = (params.data?.term || '').toLowerCase();
                        const filtered = term
                            ? available.filter(x => x.text.toLowerCase().includes(term))
                            : available;

                        success({ results: filtered });
                    },
                    error: failure
                });
            },
            processResults: function (data) {
                return data;
            }
        }
    });

    $('#permissionCode').on('select2:select', function (e) {
        const data = e.params.data;
        $('#permissionName').val(data.text);
        $('#permissionDescription').val(data.deskripsi);
    });

    // Tambah Permission
    $(document).on('click', '#btnTambahPermission', function () {
        const modal = new bootstrap.Modal(document.getElementById('modalTambahPermission'));

        modal.show();
    });

    // fungsi tambah permission 
    $(document).on('submit', '#formTambahPermission', function (e) {
        e.preventDefault();

        const formData = {
            code: $('#permissionCode').val().trim(),
            name: $('#permissionName').val().trim(),
            description: $('#permissionDescription').val().trim(),
        };

        if (!formData.code || !formData.name) {
            Swal.fire('Peringatan', 'Kode dan Nama Permission wajib diisi.', 'warning');
            return;
        }

        Swal.fire({
            title: 'Menyimpan...',
            text: 'Mohon tunggu sebentar',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: '/api/Permission',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (res) {
                Swal.close();

                Swal.fire({
                    icon: 'success',
                    title: 'Berhasil',
                    text: res?.message || 'Permission berhasil ditambahkan!',
                }).then(()=> {
                    $('#modalTambahPermission').modal('hide');
                    $('#formTambahPermission')[0].reset();
                    $('#permissionCode').val(null).trigger('change');
                    // $('#datatable-buttons').DataTable().ajax.reload(null, false);
                    table.ajax.reload(null, false);
                });

            },
            error: function (xhr) {
                Swal.close();
                Swal.fire('Gagal', xhr.responseJSON?.message || 'Gagal menambahkan permission.', 'error');
            }
        });
    });

    // Delete Permission
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');

        Swal.fire({
            title: 'Yakin hapus permission ini?',
            text: "Data yang dihapus tidak bisa dikembalikan!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Ya, hapus!',
            cancelButtonText: 'Batal'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/api/Permission/${id}`,
                    type: 'DELETE',
                    success: function (res) {
                        const msg = res.message || 'Permission berhasil dihapus.';
                        Swal.fire({
                            icon: 'success',
                            title: 'Berhasil',
                            text: msg,
                        }).then(()=>{
                            table.ajax.reload();
                        });
                        // $('#datatable-buttons').DataTable().ajax.reload(null, false); // reload tanpa reset pagination
                    },
                    error: function (xhr) {
                        const msg = xhr.responseJSON?.message || 'Gagal menghapus permission.';
                        Swal.fire('Error', msg, 'error');
                    }
                });
            }
        });
    });

    // Initialize Select2 for Permission Code in Edit Form (Subtract existing API codes)
    // $('#editPermissionCode').select2({
    //     dropdownParent: $('#modalEditPermission'),
    //     placeholder: 'pilih kode permission',
    //     ajax: {
    //         transport: function (params, success, failure) {
    //             const currentId = $('#editPermissionId').val(); // get current editing ID

    //             $.ajax({
    //                 url: '/api/Permission',
    //                 method: 'GET',
    //                 dataType: 'json',
    //                 success: function (res) {
    //                     const existing = (res?.data || []).map(p => ({
    //                         id: p.permission_id,
    //                         code: p.code
    //                     }));

    //                     // Find the current permission code (by ID)
    //                     const currentPermission = existing.find(p => p.id === currentId);
    //                     const currentCode = currentPermission ? currentPermission.code : null;

    //                     // Build available list = all known codes minus existing, but include current one
    //                     const available = permissionCodeList
    //                         .filter(code => !existing.map(e => e.code).includes(code) || code === currentCode)
    //                         .map(code => ({ id: code, text: code }));

    //                     // Filter by search term
    //                     const term = (params.data?.term || '').toLowerCase();
    //                     const filtered = term
    //                         ? available.filter(x => x.text.toLowerCase().includes(term))
    //                         : available;

    //                     success({ results: filtered });
    //                 },
    //                 error: failure
    //             });
    //         },
    //         processResults: function (data) {
    //             return data;
    //         }
    //     }
    // });

    $('#editPermissionCode').select2({
        dropdownParent: $('#modalEditPermission'),
        placeholder: 'pilih kode permission',
        ajax: {
            transport: function (params, success, failure) {

                const currentId = $('#editPermissionId').val(); // id of permission being edited

                $.ajax({
                    url: '/api/Permission',
                    method: 'GET',
                    dataType: 'json',
                    success: function (res) {

                        // list of permissions already created in DB
                        const existing = (res?.data || []).map(p => ({
                            id: p.permission_id,
                            code: p.code
                        }));

                        // find the permission being edited (to allow its own code)
                        const currentPermission = existing.find(p => p.id === currentId);
                        const currentCode = currentPermission ? currentPermission.code : null;

                        // Build options:
                        // - include all codes from permissionCodeList
                        // - exclude already existing codes EXCEPT the current one
                        const available = permissionCodeList
                            .filter(p =>
                                p.kode === currentCode ||          // allow current one
                                !existing.some(e => e.code === p.kode)  // exclude others already in DB
                            )
                            .map(p => ({
                                id: p.kode,   // value of select
                                text: p.nama, // label of select
                                deskripsi: p.deskripsi
                            }));

                        // search filter
                        const term = (params.data?.term || '').toLowerCase();
                        const filtered = term
                            ? available.filter(x => x.text.toLowerCase().includes(term))
                            : available;

                        success({ results: filtered });
                    },
                    error: failure
                });
            },
            processResults: function (data) {
                return data;
            }
        }
    });

    $('#editPermissionCode').on('select2:select', function (e) {
        const data = e.params.data;
        $('#editPermissionName').val(data.text);
        $('#editPermissionDescription').val(data.deskripsi);
    });

    // Edit Permission
    $(document).on('click', '.btn-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');

        Swal.fire({
            title: 'Memuat data...',
            text: 'Mohon tunggu sebentar',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: `/api/Permission/${id}`,
            method: 'GET',
            success: function (res) {
                const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                $('body').css('padding-right', scrollbarWidth + 'px');
                Swal.close();
                let resData = typeof res === 'string' ? JSON.parse(res) : res;
                let data = resData.data;

                $('#editPermissionId').val(id);
                // $('#editPermissionCode').val(data.code);
                $('#editPermissionName').val(data.name);
                $('#editPermissionDescription').val(data.description);

                const option = new Option(
                    data.code,                          // text
                    data.code,                          // value
                    true,                               // defaultSelected
                    true                                // selected
                );
                $('#editPermissionCode').append(option).trigger('change');

                const modal = new bootstrap.Modal(document.getElementById('modalEditPermission'));
                modal.show();
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire('Gagal', xhr.responseJSON?.message || 'Gagal memuat data permission.', 'error');
            }
        });
    });

    // Submit Edit Form
    $(document).on('submit', '#formEditPermission', function (e) {
        e.preventDefault();

        const id = $('#editPermissionId').val();
        const formData = {
            code: $('#editPermissionCode').val().trim(),
            name: $('#editPermissionName').val().trim(),
            description: $('#editPermissionDescription').val().trim(),
        };

        if (!formData.code || !formData.name) {
            Swal.fire('Peringatan', 'Kode dan Nama Permission wajib diisi.', 'warning');
            return;
        }

        Swal.fire({
            title: 'Menyimpan perubahan...',
            text: 'Mohon tunggu sebentar',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: `/api/Permission/${id}`,
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (res) {
                Swal.close();
                Swal.fire({
                    icon: 'success',
                    title: 'Berhasil',
                    text: res?.message || 'Permission berhasil diperbarui!',
                }).then(() => {
                    $('#modalEditPermission').modal('hide');
                    $('#formEditPermission')[0].reset();
                    $('#editPermissionCode').val(null).trigger('change');
                    table.ajax.reload(null, false);
                });
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memperbarui permission.', 'error');
            }
        });
    });

    // Bulk Delete Function
    $(document).on('click', '#delete-selected', function () {
        const selectedData = table.rows({ selected: true }).data().toArray();

        if (selectedData.length === 0) {
            Swal.fire('Info', 'Tidak ada data yang dipilih.', 'info');
            return;
        }

        Swal.fire({
            title: `Hapus ${selectedData.length} permission terpilih?`,
            text: 'Tindakan ini tidak dapat dibatalkan.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Ya, hapus!',
            cancelButtonText: 'Batal',
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
        }).then((result) => {
            if (result.isConfirmed) {
                bulkDelete(selectedData);
            }
        });
    });

    function bulkDelete(items) {
        Swal.fire({
            title: 'Menghapus data...',
            text: 'Mohon tunggu.',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        const deletePromises = items.map(item => {
            return $.ajax({
                url: `/api/Permission/${item.permission_id}`,
                type: 'DELETE'
            });
        });

        Promise.allSettled(deletePromises)
            .then(results => {
                const successCount = results.filter(r => r.status === 'fulfilled').length;
                const failCount = results.length - successCount;

                let msg = `Berhasil menghapus ${successCount} permission.`;
                if (failCount > 0) msg += ` ${failCount} gagal dihapus.`;
                // Swal.fire('Selesai', msg, 'success');
                // table.ajax.reload(null, false);
                Swal.fire('Selesai!', msg, 'success')
                    .then(()=>{
                        table.ajax.reload();
                    });
            })
            .catch(err => {
                Swal.fire('Error', 'Terjadi kesalahan saat menghapus data.', 'error');
                console.error(err);
            });
    }

    // =========================
    // GLOBAL VARIABLES
    // =========================
    let assignedTable, unassignedTable;
    let permissionId;

    // =========================
    // OPEN MODAL
    // =========================
    $(document).on('click', '.btn-akses', async function () {
        permissionId = $(this).data('id');
        const tr = $(this).closest('tr');
        const rowData = table.row(tr).data() || table.row($(this).parents('li')).data();

        // Get details from that row
        // const permissionId = rowData.permission_id;
        const permissionCode = rowData.code;
        // const permissionName = rowData.name;
        $('#modalKelolaAksesLabel').text(`${permissionCode}`);

        Swal.fire({
            title: 'Memuat data role...',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        try {
            // 1️⃣ Fetch all roles
            const allRolesRes = await $.ajax({
                url: '/api/Role',
                method: 'GET',
                dataType: 'json'
            });
            const allRoles = allRolesRes?.data || [];

            // 2️⃣ Fetch assigned roles for this permission
            const assignedRes = await $.ajax({
                url: `/api/RolePermission?permission_id=${permissionId}`,
                method: 'GET',
                dataType: 'json'
            });
            const assignedRoles = assignedRes?.data || [];

            // 3️⃣ Extract assigned role_ids
            const assignedIds = assignedRoles.map(rp => rp.role_id);

            // 4️⃣ Separate unassigned roles
            const unassignedRoles = allRoles.filter(r => !assignedIds.includes(r.role_id));

            Swal.close();

            // 5️⃣ Initialize or reload tables
            if ($.fn.DataTable.isDataTable('#tableAssignedRoles')) {
                assignedTable.clear().rows.add(assignedRoles).draw();
                unassignedTable.clear().rows.add(unassignedRoles).draw();
            } else {
                assignedTable = $('#tableAssignedRoles').DataTable({
                    data: assignedRoles,
                    language: {
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
                        // {
                        //     data: null,
                        //     title: "No",
                        //     render: function (data, type, row, meta) {
                        //         return meta.row + 1;
                        //         // return meta.row + meta.settings._iDisplayStart + 1;
                        //     },
                        //     orderable: 'false'
                        // },
                        { data: 'roleDto.code', title: 'Kode Role', width: '35%' },
                        { data: 'roleDto.name', title: 'Nama Role', width: '35%' },
                        {
                            data: null,
                            title: 'Aksi',
                            render: () => `<button class="btn btn-sm btn-danger btn-remove-role">
                                <i class="mdi mdi-trash-can-outline"></i> Hapus
                            </button>`,
                            orderable: false,
                            className: 'text-center',
                            width: '30%'
                        }
                    ],
                    columnDefs: [
                        { className: 'dt-center align-middle', targets: '_all' },
                        {
                            targets: [0, 1],
                            createdCell: function (td, cellData, rowData, row, col) {
                                td.classList.add('text-start');
                            }
                        },
                    ],
                    order: [[0, 'asc']],
                    initComplete: function() {
                        // length change button
                        $('#tableAssignedRoles_wrapper .dt-length').appendTo('#tableAssignedRoles_wrapper .row:eq(2) .dt-layout-end');
                        $(".dt-length select").addClass('form-select form-select-sm');

                        $('#tableAssignedRoles_wrapper .row:eq(2) .dt-layout-end')
                            .removeClass('align-items-center')

                        $('#tableAssignedRoles thead').addClass('table-dark');

                        $('#tableAssignedRoles_wrapper .row:eq(0) .col-md-auto:eq(1)')
                            .removeClass('align-items-center')
                            .addClass('align-items-end');

                        $('#tableAssignedRoles_wrapper .row:eq(0) .col-md-auto:eq(0)')
                            .removeClass('col-md-auto')
                            .addClass('col-md-6');

                        $('#tableAssignedRoles_wrapper .dt-paging').appendTo('#tableAssignedRoles_wrapper .row:eq(2) .dt-layout-start');
                        $('#tableAssignedRoles_wrapper .dt-info').appendTo('#tableAssignedRoles_wrapper .row:eq(2) .dt-layout-start');
                        $('#tableAssignedRoles_wrapper .row:eq(2) .dt-layout-start')
                            .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
                            //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
                            .addClass('d-flex flex-column col-md-6 gap-2');

                        let jumpToPage = `
                            <div class="dt-jump-to-page d-flex align-items-center align-self-md-start align-self-center">
                                <input type="number" min="1" class="form-control form-control-sm me-2" id="jump-to-page-assigned" placeholder="Lompat ke" style="width: 100px;">
                                <button class="btn btn-sm btn-dark" id="jump-to-btn-assigned">Go</button>
                            </div>
                        `;

                        // Insert it after pagination controls
                        $(jumpToPage).insertAfter('#tableAssignedRoles_wrapper .dt-paging');
                    }
                    // paging: true,
                    // searching: true,
                    // info: true
                });

                function jumpToSpecifiedPageAssignedRoles() {
                    let page = parseInt($('#jump-to-page-assigned').val(), 10) - 1;
                    if (!isNaN(page) && page >= 0 && page < assignedTable.page.info().pages) {
                        assignedTable.page(page).draw('page');
                    }
                }

                // Handle 'Go' button click
                $(document).on('click', '#jump-to-btn-assigned', function () {
                    jumpToSpecifiedPageAssignedRoles();
                });

                // Handle 'Enter' keypress in input
                $(document).on('keypress', '#jump-to-page-assigned', function (e) {
                    if (e.which === 13) {
                        e.preventDefault();
                        jumpToSpecifiedPageAssignedRoles();
                    }
                });

                unassignedTable = $('#tableUnassignedRoles').DataTable({
                    data: unassignedRoles,
                    language: {
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
                        // {
                        //     data: null,
                        //     title: "No",
                        //     render: function (data, type, row, meta) {
                        //         return meta.row + 1;
                        //         // return meta.row + meta.settings._iDisplayStart + 1;
                        //     },
                        //     orderable: 'false'
                        // },
                        { data: 'code', title: 'Kode Role', width: '35%' },
                        { data: 'name', title: 'Nama Role', width: '35%' },
                        {
                            data: null,
                            title: 'Aksi',
                            render: () => `<button class="btn btn-sm btn-info btn-add-role">
                                <i class="mdi mdi-plus-circle-outline"></i> Tambah
                            </button>`,
                            orderable: false,
                            className: 'text-center',
                            width: '30%'
                        }
                    ],
                    columnDefs: [
                        { className: 'dt-center align-middle', targets: '_all' },
                        {
                            targets: [0, 1],
                            createdCell: function (td, cellData, rowData, row, col) {
                                td.classList.add('text-start');
                            }
                        },
                    ],
                    order: [[0, 'asc']],
                    initComplete: function() {
                        // length change button
                        $('#tableUnassignedRoles_wrapper .dt-length').appendTo('#tableUnassignedRoles_wrapper .row:eq(2) .dt-layout-end');
                        $(".dt-length select").addClass('form-select form-select-sm');

                        $('#tableUnassignedRoles_wrapper .row:eq(2) .dt-layout-end')
                            .removeClass('align-items-center')

                        $('#tableUnassignedRoles thead').addClass('table-dark');

                        $('#tableUnassignedRoles_wrapper .row:eq(0) .col-md-auto:eq(1)')
                            .removeClass('align-items-center')
                            .addClass('align-items-end');

                        $('#tableUnassignedRoles_wrapper .row:eq(0) .col-md-auto:eq(0)')
                            .removeClass('col-md-auto')
                            .addClass('col-md-6');

                        $('#tableUnassignedRoles_wrapper .dt-paging').appendTo('#tableUnassignedRoles_wrapper .row:eq(2) .dt-layout-start');
                        $('#tableUnassignedRoles_wrapper .dt-info').appendTo('#tableUnassignedRoles_wrapper .row:eq(2) .dt-layout-start');
                        $('#tableUnassignedRoles_wrapper .row:eq(2) .dt-layout-start')
                            .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
                            //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
                            .addClass('d-flex flex-column col-md-6 gap-2');

                        let jumpToPage = `
                            <div class="dt-jump-to-page d-flex align-items-center align-self-md-start align-self-center">
                                <input type="number" min="1" class="form-control form-control-sm me-2" id="jump-to-page-unassigned" placeholder="Lompat ke" style="width: 100px;">
                                <button class="btn btn-sm btn-dark" id="jump-to-btn-unassigned">Go</button>
                            </div>
                        `;

                        // Insert it after pagination controls
                        $(jumpToPage).insertAfter('#tableUnassignedRoles_wrapper .dt-paging');
                    }
                    // paging: true,
                    // searching: true,
                    // info: false
                });

                function jumpToSpecifiedPageUnassignedRoles() {
                    let page = parseInt($('#jump-to-page-unassigned').val(), 10) - 1;
                    if (!isNaN(page) && page >= 0 && page < unassignedTable.page.info().pages) {
                        unassignedTable.page(page).draw('page');
                    }
                }

                // Handle 'Go' button click
                $(document).on('click', '#jump-to-btn-unassigned', function () {
                    jumpToSpecifiedPageUnassignedRoles();
                });

                // Handle 'Enter' keypress in input
                $(document).on('keypress', '#jump-to-page-unassigned', function (e) {
                    if (e.which === 13) {
                        e.preventDefault();
                        jumpToSpecifiedPageUnassignedRoles();
                    }
                });
            }

            // 6️⃣ Show modal
            const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
            $('body').css('padding-right', scrollbarWidth + 'px');
            const modal = new bootstrap.Modal(document.getElementById('modalKelolaAkses'));
            modal.show();

        } catch (error) {
            Swal.close();
            Swal.fire('Error', 'Gagal memuat data role.', 'error');
            console.error(error);
        }
    });

    // =========================
    // ADD ROLE (move down → up)
    // =========================
    $(document).on('click', '.btn-add-role', async function () {
        const $btn = $(this);
        $btn.prop('disabled', true);

        // Grab role row data from the unassigned table
        const row = unassignedTable.row($btn.closest('tr')).data();

        // Prepare payload
        const payload = {
            role_id: row.role_id,
            permission_id: permissionId
        };

        Swal.fire({
            title: 'Menambahkan role ke permission...',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        try {
            // POST to create RolePermission
            const res = await $.ajax({
                url: '/api/RolePermission',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(payload)
            });

            Swal.close();

            // Normalize response -> data object must contain role_permission_id + roleDto + role_id etc
            const created = (typeof res === 'string') ? JSON.parse(res).data : res.data;
            if (!created) {
                throw new Error('Invalid response from server when creating role permission.');
            }

            // Add the returned object to assignedTable (use the server shape)
            // expected shape: { role_permission_id, role_id, roleDto, permission_id, ... }
            assignedTable.row.add(created).draw();

            // Remove from unassigned table
            unassignedTable.row($btn.closest('tr')).remove().draw();

            Swal.fire('Berhasil', 'Role ditambahkan ke permission.', 'success');
        } catch (err) {
            Swal.close();
            console.error(err);
            const msg = err.responseJSON?.message || err.message || 'Gagal menambahkan role.';
            Swal.fire('Error', msg, 'error');
            $btn.prop('disabled', false);
        }
    });

    // =========================
    // REMOVE ROLE (move up → down)
    // =========================
    $(document).on('click', '.btn-remove-role', async function () {
        const $btn = $(this);
        $btn.prop('disabled', true);

        // Grab role row data from assigned table
        const row = assignedTable.row($btn.closest('tr')).data();

        // If row has role_permission_id => delete on server
        const rpId = row?.role_permission_id;

        if (rpId) {
            Swal.fire({
                title: 'Menghapus akses role...',
                allowOutsideClick: false,
                didOpen: () => Swal.showLoading()
            });

            try {
                await $.ajax({
                    url: `/api/RolePermission/${rpId}`,
                    method: 'DELETE'
                });

                Swal.close();

                // Move row from assignedTable to unassignedTable
                assignedTable.row($btn.closest('tr')).remove().draw();

                // The shape for unassignedTable expects plain role object (role or roleDto)
                // Prefer roleDto if available, else build minimal role object from row
                const roleObj = row.roleDto || { role_id: row.role_id, name: row.name || row.role_name || row.roleDto?.name };
                unassignedTable.row.add(roleObj).draw();

                Swal.fire('Berhasil', 'Akses role dihapus.', 'success');
            } catch (err) {
                Swal.close();
                console.error(err);
                const msg = err.responseJSON?.message || err.message || 'Gagal menghapus akses role.';
                Swal.fire('Error', msg, 'error');
                $btn.prop('disabled', false);
            }
        } else {
            // No server record -> just move back client-side
            const roleObj = row.roleDto || { role_id: row.role_id, name: row.name || row.roleDto?.name };
            assignedTable.row($btn.closest('tr')).remove().draw();
            unassignedTable.row.add(roleObj).draw();
        }
    });

});