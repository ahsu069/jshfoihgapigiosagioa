$(function () { 
    const { hasKategoriCreate, hasKategoriEdit, hasKategoriDelete} = window.Permission;

    let table = $('#datatable-buttons').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/api/Kategori/datatable',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                // Custom (Start)
                const globalSearch = d.search?.value || "";
                let namakategoribarColumn = d.columns.find(c => c.data === "namakategoribar");

                d.columns.push({
                    data: "is_deleted",
                    name: "",
                    searchable: true,
                    orderable: false,
                    search: {
                        value: "false",
                        regex: false,
                        fixed: []
                    }
                });

                namakategoribarColumn.search.value = globalSearch;

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
                orderable: false,
                searchable: false
            },
            {
                data: null,
                render: DataTable.render.select(),
                className: 'all noVis',
                orderable: false,
                searchable: false
            },
            // { data: "id", title: "ID" },
            {
                data: null,
                title: "No",
                render: function (data, type, row, meta) {
                    // return meta.row + 1;
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                orderable: false,
                searchable: false,
                width: "15%"
            },
            {
                data: "namakategoribar",
                title: "Kategori",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
                searchable: true,
                orderable: true,
                width: "50%"
            },
            {
                data: "kategoribar_id",
                title: 'Aksi',
                render: function (data, type, row) {
                    let html = '<div class="d-flex gap-2">';
                    if(hasKategoriEdit) {
                        html += `<button class="btn btn-info btn-edit" data-id="${data}">
                                <i class="mdi mdi-pencil-outline"></i>
                                <span>Edit</span>
                            </button>`;
                    }
                    if(hasKategoriDelete) {
                        html += `<button class="btn btn-danger btn-delete" data-id="${data}">
                                <i class="mdi mdi-trash-can-outline"></i>
                                <span>Hapus</span>
                            </button>`;
                    }
                    html += '</div>';
                    return html;
                    // return `
                    //     <div class="d-flex gap-2">
                    //         <button class="btn btn-info btn-edit" data-id="${data}">
                    //             <i class="mdi mdi-pencil-outline"></i>
                    //             <span>Edit</span>
                    //         </button>
                    //         <button class="btn btn-danger btn-delete" data-id="${data}">
                    //             <i class="mdi mdi-trash-can-outline"></i>
                    //             <span>Hapus</span>
                    //         </button>
                    //     </div>
                    // `;
                },
                className: 'dt-center noVis',
                orderable: false,
                searchable: false,
                width: "35%"
            },
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
        initComplete: function () {
            // Buat select checkbox rata tengah
            let th = $('th[data-dt-column="1"]');
            let checkbox = th.find('input.dt-select-checkbox');
            checkbox.detach();
            th.empty();
            th.append(checkbox);

            // length change button
            $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
            $(".dt-length select").addClass('form-select form-select-sm');

            $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
                .removeClass('align-items-center')
            //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
            //.addClass('d-flex flex-column col-md-6 gap-2');

            const customButtons = `
                <div id="datatable-buttons_wrapper_custom" class="d-flex gap-2 align-items-center justify-content-center justify-content-md-start flex-wrap">
                    <!-- button type="button" class="btn btn-success text-white" id="btnTambahKategori">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button -->
                    <!-- button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button -->
                </div>
            `;

            $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);

            if (hasKategoriCreate) {
                $('#datatable-buttons_wrapper_custom').append(`
                    <button type="button" class="btn btn-success text-white" id="btnTambahKategori">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button>`);
            }
            if (hasKategoriDelete) {
                $('#datatable-buttons_wrapper_custom').append(`
                    <button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button>`);
            }

            // Kondisi awal tombol hapus terpilih
            $('#delete-selected').prop('disabled', true);

            // Enable/disable button tergantung pada row selection
            table.on('select deselect', function () {
                const selectedCount = table.rows({ selected: true }).count();
                $('#delete-selected').prop('disabled', selectedCount === 0);
            });
            // Append DataTable buttons to custom container
            table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');
            // Styling
            $('#datatable-buttons thead').addClass('table-dark');
            $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
                .removeClass('align-items-center')
                .addClass('align-items-end');
            // Adjust first row column width
            $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
                .removeClass('col-md-auto d-md-flex')
                .addClass('col-md-6');

            //$('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
            //$('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
            //$('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
            //   .removeClass('col-md-auto justify-content-between')
            //   .addClass('col-md-6 flex-wrap justify-content-end gap-2');

            // Pindahkan pagination dan info ke sebelah kiri bawah
            $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
            $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
            $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
                .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
                //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
                .addClass('d-flex flex-column col-md-6 gap-2');
            // Add jump to page input
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


   /* // Tambah Kategori button
    $(document).on('click', '#submit-kategori', function (e) {
        e.preventDefault();
        let namakategori = $('#namakategori').val().trim();
        if (!namakategori) {
            Swal.fire('Error', 'Nama Kategori tidak boleh kosong', 'error');
            return;
        }
    // Add new category via API
        $.ajax({
            url: '/api/Kategori',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify({ namakategoribar: namakategori, is_deleted: false }),
            success: function (res) {
                let resData = typeof res === "string" ? JSON.parse(res) : res;
                // $.ajax({
                //     url: '/api/Kategori',
                //     type: 'GET',
                //     contentType: 'application/json',
                //     success: function (res) {
                //         let resData = typeof res === "string" ? JSON.parse(res) : res;
                //         let table = $('#datatable-buttons').DataTable();

                //         // redraw datatable
                //         table.clear();
                //         table.rows.add(resData.data || []);
                //         table.draw();
                //     },
                //     error: function (xhr) {
                //         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat ulang data', 'error');
                //     }
                // });

                // reset input field
                $('#namakategori').val('');

                Swal.fire('Berhasil!', resData.message, 'success')
                    .then(()=>{
                        table.ajax.reload(null, false);
                    });
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Update failed', 'error');
            }
        });
    });*/

    // Klik tombol “Tambah Kategori”
    $(document).on('click', '#btnTambahKategori', function () {
        // Kosongkan input form tambah kategori
        $('#AddKategoriId').val('');            // bila ada hidden id, atau bisa di-hapus
        $('#AddNamaKategori').val('');
        // Tampilkan modal tambah kategori
        const modal = new bootstrap.Modal(document.getElementById('AddKategoriModal'));
        modal.show();
    });

    // Handle Form Tambah Kategori
    $('#AddKategoriForm').on('submit', function (e) {
        e.preventDefault();

        // bila ada hidden id, atau bisa di-hapus
        const tambahkategori = $('#AddNamaKategori').val().trim();

        if (!tambahkategori) {
            Swal.fire('Error', 'Nama kategori tidak boleh kosong', 'error');
            return;
        }

        $.ajax({
            url: `/api/Kategori`,        // endpoint untuk tambah (POST)
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ namakategoribar: tambahkategori }),
            success: function (res) {
                let resData = typeof res === "string" ? JSON.parse(res) : res;
                $('#AddKategoriModal').modal('hide');
                Swal.fire('Berhasil!', resData.message, 'success')
                    .then(() => {
                        // reload DataTable
                        const table = $('#datatable-buttons').DataTable();
                        table.ajax.reload(null, false);
                    });
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menambahkan kategori', 'error');
            }
        });
    });

    // Edit button
    $(document).on('click', '.btn-edit', function () {
        const id = $(this).data('id');
        const table = $('#datatable-buttons').DataTable();
        const rowData = table.row($(this).closest('tr')).data();

        // Fill modal fields
        $('#editKategoriId').val(id);
        $('#editNamaKategori').val(rowData.namakategoribar);

        // Show modal
        const modal = new bootstrap.Modal(document.getElementById('editKategoriModal'));
        modal.show();
    });

    // Handle Form Edit Kategori 
    $('#editKategoriForm').on('submit', function (e) {
        e.preventDefault();

        const id = $('#editKategoriId').val();
        const updatedName = $('#editNamaKategori').val().trim();

        if (!updatedName) {
            Swal.fire('Error', 'Nama kategori tidak boleh kosong', 'error');
            return;
        }

        $.ajax({
            url: `/api/Kategori/${id}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify({ namakategoribar: updatedName }),
            success: function (res) {
                let resData = typeof res === "string" ? JSON.parse(res) : res;
                $('#editKategoriModal').modal('hide');
                Swal.fire('Berhasil!', resData.message, 'success')
                    .then(()=>{
                        table.ajax.reload(null, false);
                    });
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memperbarui kategori', 'error');
            }
        });
    });

    // Delete button
    $(document).on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Hapus kategori ini?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Ya, hapus!',
            cancelButtonText: 'Batal',
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/api/Kategori/${id}`,
                    type: 'DELETE',
                    success: function (res) {
                        let resData = typeof res === "string" ? JSON.parse(res) : res;
                        Swal.fire('Berhasil!', resData.message, 'success')
                            .then(()=>{
                                table.ajax.reload();
                            });
                    },
                    error: function (xhr) {
                        Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menghapus kategori', 'error');
                    }
                });
            }
        });
    });

    // Bulk Delete Function
    $(document).on('click', '#delete-selected', function () {
        const table = $('#datatable-buttons').DataTable();
        const selectedData = table.rows({ selected: true }).data().toArray();

        if (selectedData.length === 0) {
            Swal.fire('Info', 'Tidak ada data yang dipilih.', 'info');
            return;
        }

        Swal.fire({
            title: `Hapus ${selectedData.length} kategori terpilih?`,
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
                url: `/api/Kategori/${item.kategoribar_id}`,
                type: 'DELETE'
            });
        });

        Promise.allSettled(deletePromises)
            .then(results => {
                const successCount = results.filter(r => r.status === 'fulfilled').length;
                const failCount = results.length - successCount;

                let msg = `Berhasil menghapus ${successCount} kategori.`;
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




// $(function () { 
//     $.ajax({
//         url: '/api/Kategori',
//         type: 'GET',
//         contentType: 'application/json',
//         success: function (res) {
//             let resData = typeof res === "string" ? JSON.parse(res) : res;
//             initDataTable(resData.data || []);
//         },
//         error: function (xhr) {
//             Swal.fire('Error', xhr.responseJSON?.message || 'Update failed', 'error');
//         }
//     });

//     function initDataTable(kategoriBarang) {
//         let table = $('#datatable-buttons').DataTable({
//             lengthChange: true,
//             //buttons: ['copy', 'excel', 'pdf', 'colvis'],
//             // buttons: [
//             //     {
//             //         extend: 'colvis',
//             //         columns: ':not(.noVis)',
//             //         className: 'btn btn-dark',
//             //         //columnText: function (dt, idx, title) {
//             //         //    if (title != '') {
//             //         //        return title;
//             //         //    }
//             //         //},
//             //     },
//             // ],
//             language: {
//                 buttons: {
//                     colvis: 'Tampilkan Kolom',
//                 },
//                 search: 'Cari:',
//                 lengthMenu: '_MENU_ baris barang',
//                 info: 'Menampilkan _START_ sampai _END_ dari _TOTAL_ data',
//                 select: {
//                     rows: {
//                         _: '%d baris dipilih',
//                         0: '',
//                     }
//                 },
//             },
//             //layout: {
//             //    bottomEnd: {
//             //    }
//             //},
//             data: kategoriBarang,
//             columns: [
//                 {
//                     data: null,
//                     defaultContent: '',
//                     className: 'control noVis',
//                     orderable: false
//                 },
//                 {
//                     data: null,
//                     render: DataTable.render.select(),
//                     className: 'dt-center align-middle all noVis',
//                     orderable: false
//                 },
//                 // { data: "id", title: "ID" },
//                 {
//                     data: null,
//                     title: "No",
//                     render: function (data, type, row, meta) {
//                         return meta.row + 1;
//                     }
//                 },
//                 {
//                     data: "namakategoribar",
//                     title: "Kategori",
//                     render: function (data, type, row) {
//                         return `<span class="dt-wrap">${data}</span>`;
//                     },
//                 },
                
//                 {
//                     data: "kategoribar_id",
//                     title: 'Aksi',
//                     render: function (data, type, row) {
//                         return `
//                             <div class="d-flex gap-2">
//                                 <button class="btn btn-info btn-edit" data-id="${data}">
//                                     <i class="mdi mdi-pencil-outline"></i>
//                                     <span>Edit</span>
//                                 </button>
//                                 <button class="btn btn-danger btn-delete" data-id="${data}">
//                                     <i class="mdi mdi-trash-can-outline"></i>
//                                     <span>Hapus</span>
//                                 </button>
//                             </div>
//                         `;
//                     },
//                     className: 'dt-center noVis',
//                     orderable: false
//                 }
//             ],
//             select: {
//                 style: 'multi',
//                 selector: 'td:nth-child(2)',
//                 headerCheckbox: 'select-page',
//             },
//             order: [[2, 'asc']],
//             columnDefs: [
//                 { className: 'dt-center align-middle', targets: '_all' },
//                 //{
//                 //targets: '_all',
//                 //targets: [5],
//                 //createdCell: function (td, cellData, rowData, row, col) {
//                 //    td.classList.add('dt-wrap');
//                 //}
//                 //},
//                 {
//                     //targets: '_all',
//                     //createdCell: function (td, cellData, rowData, row, col) {
//                     //    if ([3, 5].includes(col)) {
//                     //        //td.style.textAlign = 'start';
//                     //        td.classList.add('text-start');
//                     //    }
//                     //}
//                     targets: [3],
//                     createdCell: function (td, cellData, rowData, row, col) {
//                         td.classList.add('text-start');
//                     }
//                 },
//             ],
//             responsive: {
//                 details: {
//                     type: 'column',
//                     target: 0
//                 }
//             },
//         });
        
//         // Buat select checkbox rata tengah
//         let th = $('th[data-dt-column="1"]');
//         let checkbox = th.find('input.dt-select-checkbox');
//         checkbox.detach();
//         th.empty();
//         th.append(checkbox);

//         // length change button
//         $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
//         $(".dt-length select").addClass('form-select form-select-sm');

//         $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
//             .removeClass('align-items-center')
//         //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
//         //.addClass('d-flex flex-column col-md-6 gap-2');

//         const customButtons = `
//             <div id="datatable-buttons_wrapper_custom" class="d-flex gap-2 align-items-center justify-content-center justify-content-md-start flex-wrap">
//                 <!-- label for="namakategori" class="form-label">Nama Kategori<span class="text-red">*</span></label -->
//                 <!-- div class="d-flex align-items-center gap-2">
//                     <input type="text" name="namakategori" class="form-control" id="namakategori" placeholder="masukkan nama kategori" required>
//                     <button id="submit-kategori" type="submit" class="btn btn-success text-white d-inline-flex align-items-center gap-1">
//                         <i class="mdi mdi-plus-circle-outline"></i>
//                         <span>Tambah</span>
//                     </button>
//                 </div -->
//                 <button id="delete-selected" class="btn btn-danger">
//                     <i class="mdi mdi-trash-can-outline"></i>
//                     <span>Hapus Terpilih</span>
//                 </button>
//             </div>
//         `;

//         $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);

//         // Kondisi awal tombol hapus terpilih
//         $('#delete-selected').prop('disabled', true);

//         // Enable/disable button tergantung pada row selection
//         table.on('select deselect', function () {
//             const selectedCount = table.rows({ selected: true }).count();
//             $('#delete-selected').prop('disabled', selectedCount === 0);
//         });
//         // Append DataTable buttons to custom container
//         table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');
//         // Styling
//         $('#datatable-buttons thead').addClass('table-dark');
//         $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
//             .removeClass('align-items-center')
//             .addClass('align-items-end');
//         // Adjust first row column width
//         $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
//             .removeClass('col-md-auto d-md-flex')
//             .addClass('col-md-6');

//         //$('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
//         //$('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
//         //$('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
//         //   .removeClass('col-md-auto justify-content-between')
//         //   .addClass('col-md-6 flex-wrap justify-content-end gap-2');

//         // Pindahkan pagination dan info ke sebelah kiri bawah
//         $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//         $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//         $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
//             .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
//             //.addClass('col-md-6 flex-wrap justify-content-start gap-2');
//             .addClass('d-flex flex-column col-md-6 gap-2');
//         // Add jump to page input
//         let jumpToPage = `
//             <div class="dt-jump-to-page d-flex align-items-center align-self-md-start align-self-center">
//                 <input type="number" min="1" class="form-control form-control-sm me-2" id="jump-to-page" placeholder="Lompat ke" style="width: 100px;">
//                 <button class="btn btn-sm btn-dark" id="jump-to-btn">Go</button>
//             </div>
//         `;

//         // Insert it after pagination controls
//         $(jumpToPage).insertAfter('#datatable-buttons_wrapper .dt-paging');

//         function jumpToSpecifiedPage() {
//             let page = parseInt($('#jump-to-page').val(), 10) - 1;
//             if (!isNaN(page) && page >= 0 && page < table.page.info().pages) {
//                 table.page(page).draw('page');
//             }
//         }

//         // Handle 'Go' button click
//         $(document).on('click', '#jump-to-btn', function () {
//             jumpToSpecifiedPage();
//         });

//         // Handle 'Enter' keypress in input
//         $(document).on('keypress', '#jump-to-page', function (e) {
//             if (e.which === 13) {
//                 e.preventDefault();
//                 jumpToSpecifiedPage();
//             }
//         });
//     }

//     // Tambah Kategori button
//     $(document).on('click', '#submit-kategori', function (e) {
//         e.preventDefault();
//         let namakategori = $('#namakategori').val().trim();
//         if (!namakategori) {
//             Swal.fire('Error', 'Nama Kategori tidak boleh kosong', 'error');
//             return;
//         }
// // Add new category via API
//         $.ajax({
//             url: '/api/Kategori',
//             type: 'post',
//             contentType: 'application/json',
//             data: JSON.stringify({ namakategoribar: namakategori, is_deleted: false }),
//             success: function (res) {
//                 let resData = typeof res === "string" ? JSON.parse(res) : res;
//                 $.ajax({
//                     url: '/api/Kategori',
//                     type: 'GET',
//                     contentType: 'application/json',
//                     success: function (res) {
//                         let resData = typeof res === "string" ? JSON.parse(res) : res;
//                         let table = $('#datatable-buttons').DataTable();

//                         // redraw datatable
//                         table.clear();
//                         table.rows.add(resData.data || []);
//                         table.draw();
//                     },
//                     error: function (xhr) {
//                         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat ulang data', 'error');
//                     }
//                 });

//                 Swal.fire('Berhasil!', resData.message, 'success');

//                 // reset input field
//                 $('#namakategori').val('');
//             },
//             error: function (xhr) {
//                 Swal.fire('Error', xhr.responseJSON?.message || 'Update failed', 'error');
//             }
//         });
//     });

//     // Edit button
//     $(document).on('click', '.btn-edit', function () {
//         const id = $(this).data('id');
//         const table = $('#datatable-buttons').DataTable();
//         const rowData = table.row($(this).closest('tr')).data();

//         // Fill modal fields
//         $('#editKategoriId').val(id);
//         $('#editNamaKategori').val(rowData.namakategoribar);

//         // Show modal
//         const modal = new bootstrap.Modal(document.getElementById('editKategoriModal'));
//         modal.show();
//     });

//     // Handle Form Edit Kategori 
//     $('#editKategoriForm').on('submit', function (e) {
//         e.preventDefault();

//         const id = $('#editKategoriId').val();
//         const updatedName = $('#editNamaKategori').val().trim();

//         if (!updatedName) {
//             Swal.fire('Error', 'Nama kategori tidak boleh kosong', 'error');
//             return;
//         }

//         $.ajax({
//             url: `/api/Kategori/${id}`,
//             type: 'PUT',
//             contentType: 'application/json',
//             data: JSON.stringify({ namakategoribar: updatedName }),
//             success: function (res) {
//                 let resData = typeof res === "string" ? JSON.parse(res) : res;
//                 Swal.fire('Berhasil!', resData.message, 'success');

//                 // Refresh table
//                 $.ajax({
//                     url: '/api/Kategori',
//                     type: 'GET',
//                     contentType: 'application/json',
//                     success: function (res) {
//                         let resData = typeof res === "string" ? JSON.parse(res) : res;
//                         let table = $('#datatable-buttons').DataTable();
//                         table.clear();
//                         table.rows.add(resData.data || []);
//                         table.draw();

//                         // Close modal
//                         $('#editKategoriModal').modal('hide');
//                     },
//                     error: function (xhr) {
//                         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat ulang data', 'error');
//                     }
//                 });
//             },
//             error: function (xhr) {
//                 Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memperbarui kategori', 'error');
//             }
//         });
//     });

//     // Delete button
//     $(document).on('click', '.btn-delete', function () {
//         const id = $(this).data('id');

//         Swal.fire({
//             title: 'Hapus kategori ini?',
//             icon: 'warning',
//             showCancelButton: true,
//             confirmButtonText: 'Ya, hapus!',
//             cancelButtonText: 'Batal',
//             confirmButtonColor: "#3085d6",
//             cancelButtonColor: "#d33",
//         }).then((result) => {
//             if (result.isConfirmed) {
//                 $.ajax({
//                     url: `/api/Kategori/${id}`,
//                     type: 'DELETE',
//                     success: function (res) {
//                         let resData = typeof res === "string" ? JSON.parse(res) : res;
//                         $.ajax({
//                             url: '/api/Kategori',
//                             type: 'GET',
//                             contentType: 'application/json',
//                             success: function (res) {
//                                 let resData = typeof res === "string" ? JSON.parse(res) : res;
//                                 let table = $('#datatable-buttons').DataTable();

//                                 // redraw datatable
//                                 table.clear();
//                                 table.rows.add(resData.data || []);
//                                 table.draw();
//                             },
//                             error: function (xhr) {
//                                 Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat ulang data', 'error');
//                             }
//                         });

//                         Swal.fire('Berhasil!', resData.message, 'success');
//                     },
//                     error: function (xhr) {
//                         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menghapus kategori', 'error');
//                     }
//                 });
//             }
//         });
//     });

//     // Bulk Delete Function
//     $(document).on('click', '#delete-selected', function () {
//         const table = $('#datatable-buttons').DataTable();
//         const selectedData = table.rows({ selected: true }).data().toArray();

//         if (selectedData.length === 0) {
//             Swal.fire('Info', 'Tidak ada data yang dipilih.', 'info');
//             return;
//         }

//         Swal.fire({
//             title: `Hapus ${selectedData.length} kategori terpilih?`,
//             text: 'Tindakan ini tidak dapat dibatalkan.',
//             icon: 'warning',
//             showCancelButton: true,
//             confirmButtonText: 'Ya, hapus!',
//             cancelButtonText: 'Batal',
//             confirmButtonColor: "#3085d6",
//             cancelButtonColor: "#d33",
//         }).then((result) => {
//             if (result.isConfirmed) {
//                 bulkDelete(selectedData);
//             }
//         });
//     });

//     function bulkDelete(items) {
//         Swal.fire({
//             title: 'Menghapus data...',
//             text: 'Mohon tunggu.',
//             allowOutsideClick: false,
//             didOpen: () => Swal.showLoading()
//         });

//         const deletePromises = items.map(item => {
//             return $.ajax({
//                 url: `/api/Kategori/${item.kategoribar_id}`,
//                 type: 'DELETE'
//             });
//         });

//         Promise.allSettled(deletePromises)
//             .then(results => {
//                 const successCount = results.filter(r => r.status === 'fulfilled').length;
//                 const failCount = results.length - successCount;

//                 let msg = `Berhasil menghapus ${successCount} kategori.`;
//                 if (failCount > 0) msg += ` ${failCount} gagal dihapus.`;
//                 $.ajax({
//                     url: '/api/Kategori',
//                     type: 'GET',
//                     contentType: 'application/json',
//                     success: function (res) {
//                         let resData = typeof res === "string" ? JSON.parse(res) : res;
//                         let table = $('#datatable-buttons').DataTable();

//                         // redraw datatable
//                         table.clear();
//                         table.rows.add(resData.data || []);
//                         table.draw();
//                     },
//                     error: function (xhr) {
//                         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat ulang data', 'error');
//                     }
//                 });

//                 Swal.fire('Selesai', msg, 'success');
//             })
//             .catch(err => {
//                 Swal.fire('Error', 'Terjadi kesalahan saat menghapus data.', 'error');
//                 console.error(err);
//             });
//     }
// });