$(function () {
    const { hasUserCreate, hasUserEdit, hasUserDelete, hasUserRole, hasUserPermission } = window.User
    let table = $('#datatable-buttons').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/api/User/datatable',
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

                // console.log(d);
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
                    // return meta.row + 1;
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                orderable: false,
                searchable: false,
            },
            {
                data: "nama",
                title: "Nama Pekerja",
                render: function (data, type, row) {
                    // const hasImage = row.link_gambar_user && row.link_gambar_user.trim() !== "";

                    // const imageElement = hasImage
                    //     ? `<img src="${row.link_gambar_user}" 
                    //             alt="${row.nama}" 
                    //             class="rounded-circle header-profile-user me-2"
                    //             onerror="this.outerHTML='<div class=&quot;rounded-circle header-profile-user me-2&quot;></div>';">`
                    //     : `<div class="rounded-circle header-profile-user me-2"></div>`;

                    return `
                        <div class="d-flex img-cell align-items-center gap-1">
                            <img src="${row.link_gambar_user}" 
                                alt="${row.nama}" 
                                class="rounded-circle header-profile-user me-2"
                                onerror="this.onerror=null; this.src='/assets/images/pngwing.png';">
                            <span>${row.nama}</span>
                        </div>
                    `;
                }
            },
            // { data: "no_pegawai", title: "No Pegawai" },
            // { data: "bagian", title: "Bagian" }, 
            // { data: "status", title: "Status Pekerja" },
            // { data: "Jabatan", title: "Jabatan" },
            // { data: "instansi", title: "Instansi" },
            { data: "bagianUserDto.nama", title: "Bagian", class: "text-wrap text-break" }, 
            { data: "userRoleDto.roleDto.name", title: "Role", class: "text-wrap text-break" }, 
            // { data: "username", title: "Username" }, 
            // { data: "password", title: "Password" }, 
            {
                data: "user_id",
                title: 'Aksi',
                render: function (data, type, row) {

                    if (type !== 'display') {
                        return data;  // atau return '';
                    }
                    let html = '<div class="d-flex gap-2">';
                    if (hasUserEdit) {
                        html += `<button class="btn btn-info btn-edit" data-id="${data}">
                                <i class="mdi mdi-pencil-outline"></i>
                                <span>Edit</span>
                            </button>`;
                    }
                    if (hasUserEdit) {
                        html += `<button class="btn btn-info btn-edit-pass" data-id="${data}">
                                <i class="mdi mdi-key-outline"></i>
                                <span>Ubah Password</span>
                            </button>`;
                    }
                    if (hasUserDelete) {
                        html += `<button class="btn btn-danger btn-delete" data-id="${data}">
                                <i class="mdi mdi-trash-can-outline"></i>
                                <span>Hapus</span>
                            </button>`;
                    }
                    html += '</div>';
                    return html;
                    
                    //return `
                    //    <div class="d-flex gap-2">
                    //        <button class="btn btn-info btn-edit" data-id="${data}">
                    //            <i class="mdi mdi-pencil-outline"></i>
                    //            <span>Edit</span>
                    //        </button>
                    //        <button class="btn btn-info btn-edit-pass" data-id="${data}">
                    //            <i class="mdi mdi-key-outline"></i>
                    //            <span>Ubah Password</span>
                    //        </button>
                    //        <button class="btn btn-danger btn-delete" data-id="${data}">
                    //            <i class="mdi mdi-trash-can-outline"></i>
                    //            <span>Hapus</span>
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
                targets: [3, 4, 5,  6],
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
        initComplete: function() {
            $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
            $(".dt-length select").addClass('form-select form-select-sm');

            $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
                .removeClass('align-items-center')

            const customButtons = `
                <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
                    <!--button class="btn btn-success text-white" id="btnAddUser">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button-->
                    <!--button class="btn btn-danger text-white" id="delete-selected">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button-->
                    <!--a href="/user/role" class="btn btn-secondary waves-effect waves-light text-white">
                        <i class="mdi mdi-account"></i>
                        <span>Kelola Role</span>
                    </a-->
                    <!--a href="/user/permission" class="btn btn-secondary waves-effect waves-light text-white">
                        <i class="mdi mdi-account-lock"></i>
                        <span>Kelola Permission</span>
                    </a-->
                </div>
            `;

            $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
            if (hasUserPermission) {
                $('#datatable-buttons_wrapper_custom').prepend(`<a href="/user/permission" class="btn btn-secondary waves-effect waves-light text-white">
                        <i class="mdi mdi-account-lock"></i>
                        <span>Kelola Permission</span>
                    </a>`);
            }
            if (hasUserRole) {
                $('#datatable-buttons_wrapper_custom').prepend(`<a href="/user/role" class="btn btn-secondary waves-effect waves-light text-white">
                        <i class="mdi mdi-account"></i>
                        <span>Kelola Role</span>
                    </a>`);
            }
            if (hasUserDelete) {
                $('#datatable-buttons_wrapper_custom').prepend(`<button class="btn btn-danger text-white" id="delete-selected">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button>`);
            }
            if (hasUserCreate) {
                $('#datatable-buttons_wrapper_custom').prepend(` <button class="btn btn-success text-white" id="btnAddUser">
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

    // Tambah User
    $(document).on('click', '#btnAddUser', function () {
        $('#addUserModal').modal('show');
    });

    // Inisialisasi Select2 bagian untuk tambah user
    $('#bagian_id').select2({
        dropdownParent: $('#addUserModal'),
        placeholder: 'pilih bagian',
        ajax: {
            url: '/api/select2/bagianuser',
            dataType: 'json',
            data: function(params) {
                var query = {
                    search: params.term,
                    page: params.page || 1,
                    pageSize: 10
                }

                return query;
            },
            delay: 250,
            cache: true
        }
    });

    // Inisialisasi Select2 role untuk tambah user
    $('#role_id').select2({
        dropdownParent: $('#addUserModal'),
        placeholder: 'pilih role',
        ajax: {
            url: '/api/select2/role',
            dataType: 'json',
            data: function(params) {
                var query = {
                    search: params.term,
                    page: params.page || 1,
                    pageSize: 10
                }

                return query;
            },
            delay: 250,
            cache: true
        }
    });

    // Submit Tambah User
    $(document).on('submit', '#addUserForm', function (e) {
        e.preventDefault();

        let userData = {
            nama: $('#nama').val().trim(),
            bagian_id: parseInt($('#bagian_id').val()),
            username: $('#username').val().trim(),
            //password: $('#password').val()
            password: '',
            role_id: $('#role_id').val().trim()
        };

        Swal.fire({
            title: 'Menyimpan...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        $.ajax({
            url: '/api/User',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(userData),
            success: function (res) {
                Swal.close();
                Swal.fire('Berhasil', 'User berhasil ditambahkan!', 'success').then(()=>{
                    $('#addUserModal').modal('hide');
                    $('#addUserForm')[0].reset();
                    $('#bagian_id').val(null).trigger('change');
                    $('#role_id').val(null).trigger('change');
                    table.ajax.reload(null, false);
                });
            },
            error: function (xhr) {
                Swal.close();
                let res = xhr.responseJSON;
                let msg = res?.message || 'Gagal menambahkan user.';

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
    });

    // Edit User
    let bagian_id_edit = null;

    // Inisialisasi select2 bagian di edit user
    $('#bagian_idEdit').select2({
        dropdownParent: $('#editUserModal'),
        placeholder: 'pilih bagian',
        ajax: {
            url: '/api/select2/bagianuser',
            dataType: 'json',
            data: function(params) {
                var query = {
                    search: params.term,
                    page: params.page || 1,
                    pageSize: 10
                };
                return query;
            },
            delay: 250,
            cache: true
        }
    });

    // Inisialisasi Select2 role untuk edit user
    $('#role_idEdit').select2({
        dropdownParent: $('#editUserModal'),
        placeholder: 'pilih role',
        ajax: {
            url: '/api/select2/role',
            dataType: 'json',
            data: function(params) {
                var query = {
                    search: params.term,
                    page: params.page || 1,
                    pageSize: 10
                }

                return query;
            },
            delay: 250,
            cache: true
        }
    });

    $(document).on('click', '.btn-edit', function () {
        const id = $(this).data('id');
        // bagian_id_edit = null;

        Swal.fire({
            title: 'Memuat data...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        $.ajax({
            url: `/api/User/${id}`,
            method: 'GET',
            success: function (res) {
                Swal.close();
                const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                $('body').css('padding-right', scrollbarWidth + 'px');

                const user = res.data || res;
                $('#namaEdit').val(user.nama);
                $('#usernameEdit').val(user.username);
                bagian_id_edit = user.bagian_id;

                // Load bagian into select2
                if (user.bagianUserDto && user.bagianUserDto.bagian_id && user.bagianUserDto.nama) {
                    const option = new Option(user.bagianUserDto.nama, user.bagianUserDto.bagian_id, true, true);
                    $('#bagian_idEdit').append(option).trigger('change');
                } else {
                    $('#bagian_idEdit').val(null).trigger('change');
                }

                // Load role into select2
                if(user.userRoleDto.roleDto.role_id && user.userRoleDto.roleDto.name) {
                    const option = new Option(user.userRoleDto.roleDto.name, user.userRoleDto.roleDto.role_id, true, true);
                    $('#role_idEdit').append(option).trigger('change');
                } else {
                    $('#role_idEdit').val(null).trigger('change');
                }

                // Store id in modal (hidden)
                $('#idEdit').val(id);
                $('#editUserModal').modal('show');
            },
            error: function (xhr) {
                // Swal.close();
                // const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal memuat data user.';
                // Swal.fire('Error', msg, 'error');
                Swal.close();
                let res = xhr.responseJSON;
                let msg = res?.message || 'Gagal memuat data user.';

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
    });

    // Submit Edit User
    $(document).on('submit', '#editUserForm', function (e) {
        e.preventDefault();

        const id = $('#idEdit').val();
        const userData = {
            nama: $('#namaEdit').val().trim(),
            bagian_id: parseInt($('#bagian_idEdit').val()) || bagian_id_edit,
            username: $('#usernameEdit').val().trim(),
            password: '',
            role_id: $('#role_idEdit').val().trim()
        };

        Swal.fire({
            title: 'Menyimpan perubahan...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        $.ajax({
            url: `/api/User/${id}`,
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(userData),
            success: function () {
                Swal.close();
                Swal.fire('Berhasil', 'Data user berhasil diperbarui.', 'success').then(()=>{
                    $('#editUserModal').modal('hide');
                    $('#editUserForm')[0].reset();
                    $('#bagian_idEdit').val(null).trigger('change');
                    $('#role_idEdit').val(null).trigger('change');
                    // bagian_id_edit = null;
                    table.ajax.reload(null, false);
                });
            },
            error: function (xhr) {
                // Swal.close();
                // const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal memuat data user.';
                // Swal.fire('Error', msg, 'error');
                Swal.close();
                let res = xhr.responseJSON;
                let msg = res?.message || 'Gagal menyimpan perubahan data user.';

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
    });


    // Ubah Password
    $(document).on('click', '.btn-edit-pass', function () {
        const id = $(this).data('id');
        // bagian_id_edit = null;

        Swal.fire({
            title: 'Memuat data...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        $.ajax({
            url: `/api/User/${id}`,
            method: 'GET',
            success: function (res) {
                Swal.close();
                const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                $('body').css('padding-right', scrollbarWidth + 'px');

                const user = res.data || res;
                $('#namaPass').val(user.nama);
                $('#usernamePass').val(user.username);
                $('#passwordEdit').val('');
                $('#bagian_idPass').val(user.bagian_id);
                $('#role_idPass').val(user.userRoleDto.role_id);
                bagian_id_edit = user.bagian_id;

                // Store id in modal (hidden)
                $('#idPass').val(id);
                $('#editPasswordModal').modal('show');
            },
            error: function (xhr) {
                // Swal.close();
                // const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal memuat data user.';
                // Swal.fire('Error', msg, 'error');
                Swal.close();
                let res = xhr.responseJSON;
                let msg = res?.message || 'Gagal memuat data user.';

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
    });

    // Submit Edit Password
    $(document).on('submit', '#editPasswordForm', function (e) {
        e.preventDefault();

        const id = $('#idPass').val();
        const userData = {
            nama: $('#namaPass').val().trim(),
            bagian_id: parseInt($('#bagian_idPass').val()) || bagian_id_edit,
            username: $('#usernamePass').val().trim(),
            password: $('#passwordEdit').val().trim(),
            role_id: $('#role_idPass').val().trim(),
        };

        Swal.fire({
            title: 'Menyimpan perubahan...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        $.ajax({
            url: `/api/User/${id}`,
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(userData),
            success: function () {
                Swal.close();
                Swal.fire('Berhasil', 'Data user berhasil diperbarui.', 'success').then(() => {
                    $('#editPasswordModal').modal('hide');
                    $('#editPasswordForm')[0].reset();
                    bagian_id_edit = null;
                    table.ajax.reload(null, false);
                });
            },
            error: function (xhr) {
                // Swal.close();
                // const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal menyimpan perubahan.';
                // Swal.fire('Error', msg, 'error');
                Swal.close();
                let res = xhr.responseJSON;
                let msg = res?.message || 'Gagal menyimpan perubahan.';

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
    });

    // Delete User
    $(document).on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Yakin ingin menghapus user ini?',
            text: 'Tindakan ini tidak bisa dibatalkan!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Ya, hapus!',
            cancelButtonText: 'Batal',
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6'
        }).then(result => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Menghapus...',
                    didOpen: () => Swal.showLoading(),
                    allowOutsideClick: false,
                    showConfirmButton: false
                });

                $.ajax({
                    url: `/api/User/${id}`,
                    method: 'DELETE',
                    success: function () {
                        Swal.close();
                        Swal.fire('Dihapus!', 'User berhasil dihapus.', 'success').then(()=>{
                            table.ajax.reload();
                        });
                    },
                    error: function (xhr) {
                        // Swal.close();
                        // const msg = xhr.responseJSON?.message || xhr.responseText || 'Gagal menghapus user.';
                        // Swal.fire('Error', msg, 'error');
                        Swal.close();
                        let res = xhr.responseJSON;
                        let msg = res?.message || 'Gagal menghapus user.';

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
            }
        });
    });


    //️ Bulk Delete User
    $(document).on('click', '#delete-selected', function (e) {
        e.preventDefault();
        const selectedRows = table.rows({ selected: true }).data().toArray();

        if (selectedRows.length === 0) {
            Swal.fire('Info', 'Tidak ada user yang dipilih.', 'info');
            return;
        }

        const ids = selectedRows.map(row => row.user_id);

        Swal.fire({
            title: `Hapus ${ids.length} user terpilih?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Ya, hapus semua',
            cancelButtonText: 'Batal',
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6'
        }).then(result => {
            if (result.isConfirmed) {
                bulkDeleteUser(selectedRows);
            }
        });
    });

    function bulkDeleteUser(users) {
        Swal.fire({
            title: 'Menghapus data...',
            text: 'Mohon tunggu.',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        const deletePromises = users.map(user => {
            return $.ajax({
                url: `/api/User/${user.user_id}`,
                type: 'DELETE'
            });
        });

        Promise.allSettled(deletePromises)
            .then(results => {
                const successCount = results.filter(r => r.status === 'fulfilled').length;
                const failCount = results.length - successCount;

                let msg = `Berhasil menghapus ${successCount} user.`;
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
            });
    }
});