$(function () {
    const { hasRoleCreate, hasRoleEdit, hasRoleDelete } = window.Role
    let table = $('#datatable-buttons2').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/api/Role/datatable',
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
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                orderable: false,
                searchable: false,
            },
            {
                data: "code",
                title: "Kode Role",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
            },
            {
                data: "name",
                title: "Nama Role",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
            },
            {
                data: "description",
                title: "Deskripsi",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                }
            },

            {
                data: "role_id",
                title: "Aksi",
                render: function (data, type, row) {
                    if (type !== 'display') {
                        return data;  // atau return '';
                    }
                    let html = '<div class="d-flex gap-2">';
                    if (hasRoleEdit) {
                        html += `<button class="btn btn-info btn-edit" data-id="${data}">
                                <i class="mdi mdi-pencil-outline"></i>
                                <span>Edit</span>
                                </button>`;
                    }
                    if (hasRoleDelete) {
                        html += `<button class="btn btn-danger btn-delete" data-id="${data}">
                                <i class="mdi mdi-trash-can-outline"></i>
                                <span>Hapus</span>
                                </button>`;
                            }
                    html += '</div>';
                    return html; 
                //    <div class="d-flex gap-2">
                    //<button class="btn btn-info btn-edit" data-id="${data}">
                    //    <i class="mdi mdi-pencil-outline"></i>
                    //    <span>Edit</span>
                    //</button>
                    //<button class="btn btn-danger btn-delete" data-id="${data}">
                    //    <i class="mdi mdi-trash-can-outline"></i>
                    //    <span>Hapus</span>
                    //</button>
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

                // <!--Jika data lebih dari 1 -->

                //targets: [3, 5],
                //createdCell: function (td, cellData, rowData, row, col) {
                //   td.classList.add('text-start');
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
            $('#datatable-buttons2_wrapper .dt-length').appendTo('#datatable-buttons2_wrapper .row:eq(2) .dt-layout-end');
            $(".dt-length select").addClass('form-select form-select-sm');

            $('#datatable-buttons2_wrapper .row:eq(2) .dt-layout-end')
                .removeClass('align-items-center')

            const customButtons = `
                <div id="datatable-buttons2_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
                    <!--button type="button" class="btn btn-success text-white" id="btnTambahRole">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button-->
                    <!--button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button-->
                </div>
            `;

            $('#datatable-buttons2_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
            // Append Buttons conditionally
            if (hasRoleDelete) {
                $('#datatable-buttons2_wrapper_custom').prepend(`<button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button>`);
            }
            if (hasRoleCreate) {
                $('#datatable-buttons2_wrapper_custom').prepend(`<button type="button" class="btn btn-success text-white" id="btnTambahRole">
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
            
            table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons2_wrapper_custom');

            $('#datatable-buttons2 thead').addClass('table-dark');

            $('#datatable-buttons2_wrapper .row:eq(0) .col-md-auto:eq(1)')
                .removeClass('align-items-center')
                .addClass('align-items-end');

            $('#datatable-buttons2_wrapper .row:eq(0) .col-md-auto:eq(0)')
                .removeClass('col-md-auto')
                .addClass('col-md-6');

            $('#datatable-buttons2_wrapper .dt-paging').appendTo('#datatable-buttons2_wrapper .row:eq(2) .dt-layout-start');
            $('#datatable-buttons2_wrapper .dt-info').appendTo('#datatable-buttons2_wrapper .row:eq(2) .dt-layout-start');
            $('#datatable-buttons2_wrapper .row:eq(2) .dt-layout-start')
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
            $(jumpToPage).insertAfter('#datatable-buttons2_wrapper .dt-paging');
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

    // Tambah Role
    $(document).on('click', '#btnTambahRole', function () {
        const modal = new bootstrap.Modal(document.getElementById('modalTambahRoleJabatan'));
        modal.show();
    });

    $(document).on('submit', '#formTambahRole', function (e) {
        e.preventDefault();
        // const formData = Object.fromEntries(new FormData(this));
        const formData = {
            code: $('#roleCode').val().trim(),
            name: $('#roleName').val().trim(),
            description: $('#roleDescription').val().trim(),
            is_active: $('#roleHidden').val() === 'true'
        };

        if (!formData.code || !formData.name) {
            Swal.fire('Peringatan', 'Kode dan Nama Role wajib diisi.', 'warning');
            return;
        }

        Swal.fire({
            title: 'Menyimpan...',
            text: 'Mohon tunggu sebentar',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: '/api/Role',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: (res) => {
                Swal.close();

                Swal.fire('Berhasil', 'Role berhasil ditambahkan', 'success').then(()=>{
                    $('#modalTambahRoleJabatan').modal('hide');
                    $('#formTambahRole')[0].reset();
                    table.ajax.reload(null, false);
                });
            },
            error: xhr => {
                Swal.fire('Gagal', xhr.responseJSON?.message || 'Gagal menambah role', 'error');
            }
        });
    });

    // Edit Role
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
            url: `/api/Role/${id}`,
            method: 'GET',
            success: function (res) {
                const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                $('body').css('padding-right', scrollbarWidth + 'px');
                Swal.close();
                let resData = typeof res === 'string' ? JSON.parse(res) : res;
                let data = resData.data;
                console.log(data);

                $('#roleEditId').val(id);
                $('#roleCodeEdit').val(data.code);
                $('#roleNameEdit').val(data.name);
                $('#roleDescriptionEdit').val(data.description);

                const modal = new bootstrap.Modal(document.getElementById('modalEditRoleJabatan'));
                modal.show();
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire('Gagal', xhr.responseJSON?.message || 'Gagal memuat data role.', 'error');
            }
        });
    });

    $(document).on('submit', '#formEditRole', function (e) {
        e.preventDefault();
        const id = $('#roleEditId').val();
        const formData = {
            code: $('#roleCodeEdit').val().trim(),
            name: $('#roleNameEdit').val().trim(),
            description: $('#roleDescriptionEdit').val().trim(),
            is_active: $('#roleHiddenEdit').val() === 'true'
        };

        if (!formData.code || !formData.name) {
            Swal.fire('Peringatan', 'Kode dan Nama Role wajib diisi.', 'warning');
            return;
        }

        Swal.fire({
            title: 'Menyimpan perubahan...',
            text: 'Mohon tunggu sebentar',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: `/api/Role/${id}`,
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (res) {
                Swal.close();
                Swal.fire({
                    icon: 'success',
                    title: 'Berhasil',
                    text: res?.message || 'Role berhasil diperbarui!',
                }).then(() => {
                    $('#modalEditRoleJabatan').modal('hide');
                    $('#formEditRole')[0].reset();
                    table.ajax.reload(null, false);
                });
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memperbarui role.', 'error');
            }
        });
    });

    // Delete Role (Single Delete)
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        Swal.fire({
            title: 'Yakin hapus role ini?',
            text: "Data yang dihapus tidak bisa dikembalikan!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Ya, hapus!',
            cancelButtonText: 'Batal'
        }).then(result => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/api/Role/${id}`,
                    method: 'DELETE',
                    success: () => {
                        Swal.fire('Berhasil', 'Role dihapus', 'success');
                        table.ajax.reload();
                    },
                    error: xhr => {
                        Swal.fire('Gagal', xhr.responseJSON?.message || 'Tidak dapat menghapus role', 'error');
                    }
                });
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
            title: `Hapus ${selectedData.length} role terpilih?`,
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
                url: `/api/Role/${item.role_id}`,
                type: 'DELETE'
            });
        });

        Promise.allSettled(deletePromises)
            .then(results => {
                const successCount = results.filter(r => r.status === 'fulfilled').length;
                const failCount = results.length - successCount;

                let msg = `Berhasil menghapus ${successCount} role.`;
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
});