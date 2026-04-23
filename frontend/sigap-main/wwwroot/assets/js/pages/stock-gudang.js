// $(function () {
//     $.ajax({
//         url: '/api/StockGudang',
//         method: 'GET',
//         contentType: 'application/json',
//         success: function (res) {
//             let resData = typeof res === 'string' ? JSON.parse(res) : res;
//             initDataTable(resData.data || []);
//         },
//         error: function (xhr) {
//         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat data barang.', 'error');
//         }
//     });

//     function initDataTable(barangData) {
//         //Buttons examples
//         let table = $('#datatable-buttons').DataTable({
//             lengthChange: true,
//             //buttons: ['copy', 'excel', 'pdf', 'colvis'],
//             buttons: [
//                 {
//                     extend: 'colvis',
//                     columns: ':not(.noVis)',
//                     className: 'btn btn-dark',
//                     //columnText: function (dt, idx, title) {
//                     //    if (title != '') {
//                     //        return title;
//                     //    }
//                     //},
//                 },
//             ],
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
//             data: barangData,
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
//                 // { data: "barang_id", title: "ID" },
//                 {
//                     data: null,
//                     title: "No",
//                     render: function (data, type, row, meta) {
//                         // return meta.row + meta.settings._iDisplayStart + 1;
//                         return meta.row + 1;
//                     }
//                 },
//                 {
//                     data: null, // Use null to access multiple fields from the row
//                     title: "Barang",
//                     render: function (data, type, row) {
//                         // const hasImage = row.link_gambar_bar && row.link_gambar_bar.trim() !== "";

//                         // const imageElement = hasImage
//                         //     ? `<img src="${row.link_gambar_bar}" 
//                         //             alt="${row.nama_barang}" 
//                         //             class="img-barang"
//                         //             onerror="this.outerHTML='<div class=&quot;img-barang&quot;></div>';">`
//                         //     : `<div class="img-barang"></div>`;
//                         // <img src="${row.link_gambar_bar}" 
//                         //     alt="${row.nama_barang}" 
//                         //     class="img-barang"
//                         //     onerror="this.outerHTML='<div class=&quot;img-barang&quot;></div>';">
//                         // <span>${row.nama_barang}</span>

//                         return  `
//                             <div class="d-flex img-cell align-items-center gap-3">
//                                 <img src="${row.link_gambar_bar}" 
//                                     alt="${row.nama_barang}" 
//                                     class="img-barang"
//                                     onerror="this.onerror=null; this.src='/assets/images/dummy.png';">
//                                 <span>${row.nama_barang}</span>
//                             </div>
//                         `;
//                     }
//                 },
//                 {
//                     data: null,
//                     title: "Status",
//                     render: function (data, type, row) {
//                         let msl = row.msl_barang > 0 ? row.msl_barang : 1;
//                         let status = Math.round((row.jumlah_barang / msl) * 100);
//                         if (status >= 115) {
//                             return `<span class="badge bg-success">${status}%</span>`;
//                         //} else if (percentage >= 100 && percentage < 115) {
//                         } else if (status >= 100 && status < 115) {
//                             return `<span class="badge bg-warning">${status}%</span>`;
//                         } else {
//                             return `<span class="badge bg-danger">${status}%</span>`;
//                         }
//                     },
//                     orderable: true
//                 },
//                 {
//                     data: "categoryDto.namakategoribar",
//                     title: "Kategori",
//                     render: function (data, type, row) {
//                         return `<span class="dt-wrap">${data}</span>`;
//                     },
//                 },
//                 { 
//                     data: "jumlah_barang",
//                     title: "Jumlah" 
//                 },
//                 {
//                     data: "barang_id",
//                     title: 'Aksi',
//                     render: function (data, type, row) {
//                         return `
//                             <div class="d-flex gap-2">
//                                 <button class="btn btn-secondary btn-detail" data-id="${data}">
//                                     <i class="mdi mdi-file-find-outline"></i>
//                                     <span>Detail</span>
//                                 </button>
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
//             order: [[3, 'asc']],
//             columnDefs: [
//                 { className: 'dt-center align-middle', targets: '_all' },
//                 //{
//                     //targets: '_all',
//                     //targets: [5],
//                     //createdCell: function (td, cellData, rowData, row, col) {
//                     //    td.classList.add('dt-wrap');
//                     //}
//                 //},
//                 {
//                     //targets: '_all',
//                     //createdCell: function (td, cellData, rowData, row, col) {
//                     //    if ([3, 5].includes(col)) {
//                     //        //td.style.textAlign = 'start';
//                     //        td.classList.add('text-start');
//                     //    }
//                     //}
//                     targets: [3,5],
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

//         const customButtons = `
//             <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
//                 <button class="btn btn-success text-white" id="btnTambahBarang">
//                     <i class="mdi mdi-plus-circle-outline"></i>
//                     <span>Tambah</span>
//                 </button>
//                 <!-- a href="/StockGudang/TambahBarang" class="btn btn-success text-white">
//                     <i class="mdi mdi-plus-circle-outline"></i>
//                     <span>Tambah</span>
//                 </a -->
//                 <button id="delete-selected" class="btn btn-danger">
//                     <i class="mdi mdi-trash-can-outline"></i>
//                     <span>Hapus Terpilih</span>
//                 </button>
//                 <!-- a href="#" class="btn btn-danger text-white">
//                     <i class="mdi mdi-trash-can-outline"></i>
//                     <span>Hapus Terpilih</span>
//                 </a -->
//                 <!-- a href="/transaksibarang/pemasukanbarang" class="btn btn-success text-white">
//                     <i class="mdi mdi-file-plus-outline"></i>
//                     <span>Pemasukan</span>
//                 </a -->
//                 <a href="/StockGudang/KelolaKategori" class="btn btn-secondary">
//                     <i class="mdi mdi-book-plus-outline"></i>
//                     <span>Kategori Barang</span>
//                 </a>
//         `;

//         $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);

//         // Kondisi awal tombol hapus terpilih
//         $('#delete-selected').prop('disabled', true);

//         // Enable/disable button tergantung pada row selection
//         table.on('select deselect', function () {
//             const selectedCount = table.rows({ selected: true }).count();
//             $('#delete-selected').prop('disabled', selectedCount === 0);
//         });

//         table.buttons().container().addClass('justify-content-center').appendTo('#datatable-buttons_wrapper_custom');

//         $('#datatable-buttons thead').addClass('table-dark');

//         $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(1)')
//             .removeClass('align-items-center')
//             .addClass('align-items-end');

//         $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)')
//             .removeClass('col-md-auto')
//             .addClass('col-md-7');

//         $('#datatable-buttons_wrapper .dt-paging').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//         $('#datatable-buttons_wrapper .dt-info').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start');
//         $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-start')
//             .removeClass('d-md-flex align-items-center col-md-auto justify-content-between')
//             .addClass('d-flex flex-column col-md-6 gap-2');

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

//     // Select2 Initialization (Tambah)
//     $('select[name="kategoribar_id"]').select2({
//         dropdownParent: $('#modalTambahBarang'),
//         placeholder: 'pilih kategori',
//         ajax: {
//             url: '/api/kategori',
//             dataType: 'json',
//             delay: 250,
//             processResults: function (result) {
//                 return {
//                     results: result.data.map(kategori => ({
//                         id: kategori.kategoribar_id,      // option value
//                         text: kategori.namakategoribar    // option label
//                     }))
//                 };
//             }
//         }
//     });

//     // ===============================
//     // Static Select2 (manual options)
//     // ===============================
//     let satuanBarang = [
//         { id: 'BOX', text: 'Box' },
//         { id: 'PCS', text: 'Pcs' },
//         { id: 'UNIT', text: 'Unit' }
//     ];

//     // Append options before initializing Select2
//     let selectSatuan = $('select[name="satuanbar_id"]');
//     selectSatuan.empty(); // optional: clear existing

//     selectSatuan.append(new Option('', '', true, true));

//     satuanBarang.forEach(satuan => {
//         let option = new Option(satuan.text, satuan.id, false, false);
//         selectSatuan.append(option);
//     });

//     // Initialize Select2 AFTER appending options
//     selectSatuan.select2({
//         dropdownParent: $('#modalTambahBarang'),
//         placeholder: 'pilih satuan'
//     });

//     // Tambah Barang button
//     $(document).on('click', '#btnTambahBarang', function () {
//         const modal = new bootstrap.Modal(document.getElementById('modalTambahBarang'));
//         modal.show();
//     });

//     // Image preview
//     $(document).on('change', 'input[name="link_gambar_bar"]', function () {
//         const file = this.files[0];
//         if (!file) return $('#preview_gambar').addClass('d-none');
//         const reader = new FileReader();
//         reader.onload = e => $('#preview_gambar').attr('src', e.target.result).removeClass('d-none');
//         reader.readAsDataURL(file);
//     });

//     $(document).on('change', 'input[name="link_gambar_bar_edit"]', function () {
//         const file = this.files[0];
//         if (!file) return $('#preview_gambar_edit').addClass('d-none');
//         const reader = new FileReader();
//         reader.onload = e => $('#preview_gambar_edit').attr('src', e.target.result).removeClass('d-none');
//         reader.readAsDataURL(file);
//     });

//     // Submit form
//     $(document).on('submit', '#formTambahBarang', function (e) {
//         e.preventDefault();

//         const form = this;
//         const formData = new FormData(form);

//         Swal.fire({
//             title: 'Menyimpan data...',
//             text: 'Mohon tunggu.',
//             allowOutsideClick: false,
//             didOpen: () => Swal.showLoading()
//         });

//         $.ajax({
//             url: '/api/StockGudang',
//             method: 'POST',
//             processData: false,
//             contentType: false,
//             data: formData,
//             success: function (res) {
//                 Swal.fire({
//                     icon: 'success',
//                     title: 'Berhasil',
//                     text: res.message || 'Barang berhasil ditambahkan!',
//                     // timer: 1500,
//                     // showConfirmButton: false
//                 });

//                 // Close modal & reset
//                 bootstrap.Modal.getInstance(document.getElementById('modalTambahBarang')).hide();
//                 form.reset();
//                 $('#preview_gambar').addClass('d-none');

//                 $('select[name="kategoribar_id"]').val(null).trigger('change');
//                 $('select[name="satuanbar_id"]').val(null).trigger('change');

//                 // Reload table
//                 // $('#datatable-buttons').DataTable().ajax.reload(null, false);
//                 // table.ajax.reload(null, false);
//                 $.ajax({
//                     url: '/api/StockGudang',
//                     method: 'GET',
//                     contentType: 'application/json',
//                     success: function (res) {
//                         let resData = typeof res === 'string' ? JSON.parse(res) : res;
//                         let table = $('#datatable-buttons').DataTable();

//                         // redraw datatable
//                         table.clear();
//                         table.rows.add(resData.data || []);
//                         table.draw();
//                     },
//                     error: function (xhr) {
//                         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat data barang.', 'error');
//                     }
//                 });
//             },
//             error: function (xhr) {
//                 Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menambah barang.', 'error');
//             }
//         });
//     });

//     $(document).on('click', '.btn-detail', function () {
//         const id = $(this).data('id');

//         $.ajax({
//             url: `/api/StockGudang/${id}`,
//             method: 'GET',
//             beforeSend: function () {
//                 Swal.fire({
//                     title: 'Menampilkan data...',
//                     text: 'Mohon tunggu.',
//                     allowOutsideClick: false,
//                     didOpen: () => Swal.showLoading()
//                 });
//             },
//             success: function (res) {
//                 const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
//                 $('body').css('padding-right', scrollbarWidth + 'px');
//                 Swal.close();
//                 let resData = typeof res === 'string' ? JSON.parse(res) : res;
//                 let data = resData.data;
//                 let msl = data.msl_barang > 0 ? data.msl_barang : 1;
//                 let status = Math.round((data.jumlah_barang / msl) * 100);

//                 $('img.card-img-top-custom').on('error', function() {
//                     this.src = '/assets/images/dummy.png';
//                 });

//                 // Update modal content
//                 $('#itemDetailModal img').attr('src', data.link_gambar_bar || '/assets/images/dummy.png');
//                 $('#itemDetailModal .nama-barang').text(data.nama_barang);
//                 $('#itemDetailModal .kategori-barang').text(data.categoryDto?.namakategoribar || '-');
//                 $('#itemDetailModal .stock-barang').text(data.jumlah_barang);
//                 $('#itemDetailModal .msl-barang').text(msl);
//                 $('#itemDetailModal .status-barang').text(status + '%');

//                 // Adjust status badge color
//                 const statusBadge = $('#itemDetailModal .status-barang').closest('.badge');
//                 statusBadge
//                     .removeClass('bg-success bg-warning bg-danger')
//                     .addClass(status >= 115 ? 'bg-success' : status >= 100 ? 'bg-warning' : 'bg-danger');

//                 // Show modal
//                 const modal = new bootstrap.Modal(document.getElementById('itemDetailModal'));
//                 modal.show();
//             },
//             error: function (xhr) {
//                 Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat detail barang.', 'error');
//             }
//         });
//     });

//     // Select2 Initialization (Edit)
//     $('select[name="kategoribar_id_edit"]').select2({
//         dropdownParent: $('#modalEditBarang'),
//         placeholder: 'pilih kategori',
//         ajax: {
//             url: '/api/kategori',
//             dataType: 'json',
//             delay: 250,
//             processResults: function (result) {
//                 return {
//                     results: result.data.map(kategori => ({
//                         id: kategori.kategoribar_id,      // option value
//                         text: kategori.namakategoribar    // option label
//                     }))
//                 };
//             }
//         }
//     });

//     // Append options before initializing Select2
//     let selectSatuanEdit = $('select[name="satuanbar_id_edit"]');
//     selectSatuanEdit.empty(); // optional: clear existing

//     selectSatuanEdit.append(new Option('', '', true, true));

//     satuanBarang.forEach(satuan => {
//         let option = new Option(satuan.text, satuan.id, false, false);
//         selectSatuanEdit.append(option);
//     });

//     // Initialize Select2 AFTER appending options
//     selectSatuanEdit.select2({
//         dropdownParent: $('#modalEditBarang'),
//         placeholder: 'pilih satuan'
//     });

//     // Edit Barang
//     $(document).on('click', '.btn-edit', function () {
//         const id = $(this).data('id');

//         $.ajax({
//             url: `/api/StockGudang/${id}`,
//             method: 'GET',
//             beforeSend: function() {
//                 Swal.fire({
//                     title: 'Menampilkan data...',
//                     text: 'Mohon tunggu.',
//                     allowOutsideClick: false,
//                     didOpen: () => Swal.showLoading()
//                 });
//             },
//             success: function (res) {
//                 let resData = typeof res === 'string' ? JSON.parse(res) : res;
//                 let data = resData.data;
//                 const modal = new bootstrap.Modal(document.getElementById('modalEditBarang'));

//                 $('#formEditBarang')[0].reset();
//                 // Populate form
//                 $('#idEdit').val(id);
//                 $('input[name="nama_barang_edit"]').val(data.nama_barang);
//                 $('input[name="jumlah_barang_edit"]').val(data.jumlah_barang);
//                 $('input[name="msl_barang_edit"]').val(data.msl_barang);
//                 $('input[name="status_bar_edit"]').val(data.status_bar);
//                 // $('select[name="kategoribar_id_edit"]').val(data.kategoribar_id);
//                 // $('select[name="satuanbar_id_edit"]').val(data.satuanbar_id);
//                 $('#preview_gambar_edit').attr('src', data.link_gambar_bar || '').toggleClass('d-none', !data.link_gambar_bar);

//                 $('select[name="satuanbar_id_edit"]').val(data.satuanbar_id).trigger('change');

//                 if (data.categoryDto && data.categoryDto.namakategoribar) {
//                     const option = new Option(
//                         data.categoryDto.namakategoribar,  // text
//                         data.kategoribar_id,                // value
//                         true,                               // defaultSelected
//                         true                                // selected
//                     );
//                     $('select[name="kategoribar_id_edit"]').append(option).trigger('change');
//                 }

//                 // Add hidden input to mark edit mode
//                 const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
//                 $('body').css('padding-right', scrollbarWidth + 'px');
//                 // modal.show();
//                 // Swal.close();
//                 Swal.close();
//                 // setTimeout(() => modal.show(), 200);
//                 modal.show();
//             },
//             error: function (xhr) {
//                 Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat data barang untuk edit.', 'error');
//             }
//         });
//     });

//     $(document).on('submit', '#formEditBarang', function (e) {
//         e.preventDefault();
//         const id = $('#idEdit').val();
//         const form = this;
//         const originalFormData = new FormData(form);
//         const cleanFormData = new FormData();

//         // Remove `_edit` suffix from all field names
//         for (const [key, value] of originalFormData.entries()) {
//             const cleanKey = key.endsWith('_edit') ? key.slice(0, -5) : key;
//             cleanFormData.append(cleanKey, value);
//         }

//         Swal.fire({
//             title: 'Mengupdate data...',
//             text: 'Mohon tunggu.',
//             allowOutsideClick: false,
//             didOpen: () => Swal.showLoading()
//         });

//         $.ajax({
//             url: `/api/StockGudang/${id}`,
//             method: 'PUT',
//             processData: false,
//             contentType: false,
//             data: cleanFormData,
//             success: function (res) {
//                 Swal.fire({
//                     icon: 'success',
//                     title: 'Berhasil',
//                     text: res.message || 'Barang berhasil diperbarui!'
//                 });

//                 bootstrap.Modal.getInstance(document.getElementById('modalEditBarang')).hide();
//                 form.reset();
//                 $('#preview_gambar_edit').addClass('d-none');

//                 $('select[name="kategoribar_id_edit"]').val(null).trigger('change');
//                 $('select[name="satuanbar_id_edit"]').val(null).trigger('change');

//                 // Reload table
//                 // table.ajax.reload(null, false);
//                 $.ajax({
//                     url: '/api/StockGudang',
//                     method: 'GET',
//                     success: function (res) {
//                         let resData = typeof res === 'string' ? JSON.parse(res) : res;
//                         const table = $('#datatable-buttons').DataTable();
//                         table.clear();
//                         table.rows.add(resData.data || []);
//                         table.draw();
//                     }
//                 });
//             },
//             error: function (xhr) {
//                 Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menyimpan data.', 'error');
//             }
//         });
//     });

//     // DELETE BARANG (SINGLE)
//     $(document).on('click', '.btn-delete', function () {
//         const id = $(this).data('id');

//         Swal.fire({
//             title: 'Yakin ingin menghapus data barang?',
//             text: 'Data barang ini akan dihapus secara permanen.',
//             icon: 'warning',
//             showCancelButton: true,
//             confirmButtonText: 'Ya, hapus!',
//             cancelButtonText: 'Batal',
//             confirmButtonColor: '#d33',
//             cancelButtonColor: '#3085d6'
//         }).then((result) => {
//             if (result.isConfirmed) {
//                 Swal.fire({
//                     title: 'Menghapus data...',
//                     text: 'Mohon tunggu.',
//                     allowOutsideClick: false,
//                     didOpen: () => Swal.showLoading()
//                 });

//                 $.ajax({
//                     url: `/api/StockGudang/${id}`,
//                     method: 'DELETE',
//                     success: function (res) {
//                         Swal.fire({
//                             icon: 'success',
//                             title: 'Berhasil',
//                             text: res.message || 'Barang berhasil dihapus!'
//                         }).then(()=>{
//                             // Refresh table
//                             // table.ajax.reload(null, false);
//                             $.ajax({
//                                 url: '/api/StockGudang',
//                                 method: 'GET',
//                                 success: function (res) {
//                                     const resData = typeof res === 'string' ? JSON.parse(res) : res;
//                                     const table = $('#datatable-buttons').DataTable();
//                                     table.clear();
//                                     table.rows.add(resData.data || []);
//                                     table.draw();
//                                 }
//                             });
//                         });

//                     },
//                     error: function (xhr) {
//                         Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menghapus barang.', 'error');
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
//             title: `Hapus ${selectedData.length} barang terpilih?`,
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
//                 url: `/api/StockGudang/${item.barang_id}`,
//                 type: 'DELETE'
//             });
//         });

//         Promise.allSettled(deletePromises)
//             .then(results => {
//                 const successCount = results.filter(r => r.status === 'fulfilled').length;
//                 const failCount = results.length - successCount;

//                 let msg = `Berhasil menghapus ${successCount} barang.`;
//                 if (failCount > 0) msg += ` ${failCount} gagal dihapus.`;
//                 // Swal.fire('Selesai', msg, 'success');
//                 // table.ajax.reload(null, false);
//                 Swal.fire('Selesai!', msg, 'success')
//                     .then(()=>{
//                         // table.ajax.reload(null, false);
//                         $.ajax({
//                             url: '/api/StockGudang',
//                             method: 'GET',
//                             success: function (res) {
//                                 const resData = typeof res === 'string' ? JSON.parse(res) : res;
//                                 const table = $('#datatable-buttons').DataTable();
//                                 table.clear();
//                                 table.rows.add(resData.data || []);
//                                 table.draw();
//                             }
//                         });
//                     });
//             })
//             .catch(err => {
//                 Swal.fire('Error', 'Terjadi kesalahan saat menghapus data.', 'error');
//                 console.error(err);
//             });
//     }
// });

$(function () {
    const { hasItemCreate, hasItemEdit, hasItemDelete, hasItemKategori, hasItemRead} = window.User
    //Buttons examples
    let table = $('#datatable-buttons').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/api/StockGudang/datatable',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                // Custom (Start)
                // const globalSearch = d.search?.value || "";
                // let namakategoribarColumn = d.columns.find(c => c.data === "namakategoribar");

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
                orderable: false,
                searchable: false
            },
            {
                data: null,
                render: DataTable.render.select(),
                className: 'dt-center align-middle all noVis',
                orderable: false,
                searchable: false
            },
            // { data: "barang_id", title: "ID" },
            {
                data: null,
                title: "No",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                orderable: false,
                searchable: false
            },
            {
                data: "nama_barang", // Use null to access multiple fields from the row
                title: "Barang",
                render: function (data, type, row) {
                    return  `
                        <div class="d-flex img-cell align-items-center gap-3">
                            <img src="${row.link_gambar_bar}" 
                                alt="${row.nama_barang}" 
                                class="img-barang"
                                onerror="this.onerror=null; this.src='/assets/images/dummy.png';">
                            <span>${row.nama_barang}</span>
                        </div>
                    `;
                },
                orderable: true,
                searchable: true
            },
            {
                // data: null,
                data: 'jumlah_barang',
                title: "Status",
                render: function (data, type, row) {
                    let msl = row.msl_barang > 0 ? row.msl_barang : 1;
                    let status = Math.round((row.jumlah_barang / msl) * 100);
                    if (status >= 115) {
                        return `<span class="badge bg-success">${status}%</span>`;
                    //} else if (percentage >= 100 && percentage < 115) {
                    } else if (status >= 100 && status < 115) {
                        return `<span class="badge bg-warning">${status}%</span>`;
                    } else {
                        return `<span class="badge bg-danger">${status}%</span>`;
                    }
                },
                // orderable: false,
                // searchable: false
                orderable: true,
                searchable: true
            },
            {
                data: "categoryDto.namakategoribar",
                title: "Kategori",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${data}</span>`;
                },
                orderable: true,
                searchable: true
            },
            { 
                // data: null,
                data: "jumlah_barang",
                title: "Stok",
                render: function (data, type, row) {
                    return `<span class="dt-wrap">${row.jumlah_barang - row.booked_qty}</span>`;
                },
                // orderable: false,
                // searchable: false
                orderable: true,
                searchable: true
            },
            { 
                data: "booked_qty",
                title: "Booked",
                orderable: true,
                searchable: true
            },
            { 
                data: "jumlah_barang",
                title: "Jumlah",
                orderable: true,
                searchable: true
            },
            { 
                data: "msl_barang",
                title: "MSL",
                orderable: true,
                searchable: true
            },
            { 
                data: "uomDto.nama_satuanbar",
                title: "satuan",
                orderable: true,
                searchable: true
            },
            {
                data: "barang_id",
                title: 'Aksi',
                render: function (data, type, row) {
                    if (type !== 'display') {
                        return data;  // atau return '';
                    }
                    let html = '<div class="d-flex gap-2">';
                    // if (hasItemRead) {
                        html += `<button class="btn btn-secondary btn-detail" data-id="${data}">
                                <i class="mdi mdi-file-find-outline"></i>
                                <span>Detail</span>
                            </button>`;
                    // }
                    if (hasItemEdit) {
                        html += `<button class="btn btn-info btn-edit" data-id="${data}">
                                <i class="mdi mdi-pencil-outline"></i>
                                <span>Edit</span>
                            </button>`;
                    }
                    if (hasItemDelete) {
                        html += `<button class="btn btn-danger btn-delete" data-id="${data}">
                                <i class="mdi mdi-trash-can-outline"></i>
                                <span>Hapus</span>
                            </button>`;
                    }
                    html += '</div>';
                    return html;
                    //return `
                    //    <div class="d-flex gap-2">
                    //        <button class="btn btn-secondary btn-detail" data-id="${data}">
                    //            <i class="mdi mdi-file-find-outline"></i>
                    //            <span>Detail</span>
                    //        </button>
                    //        <button class="btn btn-info btn-edit" data-id="${data}">
                    //            <i class="mdi mdi-pencil-outline"></i>
                    //            <span>Edit</span>
                    //        </button>
                    //        <button class="btn btn-danger btn-delete" data-id="${data}">
                    //            <i class="mdi mdi-trash-can-outline"></i>
                    //            <span>Hapus</span>
                    //        </button>
                    //    </div>
                    //`;
                },
                className: 'dt-center noVis',
                orderable: false,
                searchable: false
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
                targets: [3,5],
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
            // length change button
            $('#datatable-buttons_wrapper .dt-length').appendTo('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end');
            $(".dt-length select").addClass('form-select form-select-sm');

            $('#datatable-buttons_wrapper .row:eq(2) .dt-layout-end')
                .removeClass('align-items-center')

            const customButtons = `
                <div id="datatable-buttons_wrapper_custom" class="d-flex flex-wrap gap-2 align-items-center justify-content-center justify-content-md-start">
                    <!--button class="btn btn-success text-white" id="btnTambahBarang">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </button-->
                    <!--button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button-->
                    <!--a href="/stockgudang/kelolakategori" class="btn btn-secondary">
                        <i class="mdi mdi-book-plus-outline"></i>
                        <span>Kategori Barang</span>
                    </a-->
                    <!-- a href="/StockGudang/TambahBarang" class="btn btn-success text-white">
                        <i class="mdi mdi-plus-circle-outline"></i>
                        <span>Tambah</span>
                    </a -->
                    <!-- a href="#" class="btn btn-danger text-white">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </a -->
                    <!-- a href="/transaksibarang/pemasukanbarang" class="btn btn-success text-white">
                        <i class="mdi mdi-file-plus-outline"></i>
                        <span>Pemasukan</span>
                    </a -->
            `;

            $('#datatable-buttons_wrapper .row:eq(0) .col-md-auto:eq(0)').html(customButtons);
            if (hasItemKategori) {
                $('#datatable-buttons_wrapper_custom').prepend(`<a href="/stockgudang/kelolakategori" class="btn btn-secondary">
                        <i class="mdi mdi-book-plus-outline"></i>
                        <span>Kategori Barang</span>
                    </a>`);
            }
            if (hasItemDelete) {
                $('#datatable-buttons_wrapper_custom').prepend(`<button id="delete-selected" class="btn btn-danger">
                        <i class="mdi mdi-trash-can-outline"></i>
                        <span>Hapus Terpilih</span>
                    </button>`);
            }
            if (hasItemCreate) {
                $('#datatable-buttons_wrapper_custom').prepend(`<button class="btn btn-success text-white" id="btnTambahBarang">
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
                .addClass('col-md-7');

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

    // Select2 Initialization (Tambah)
    $('select[name="kategoribar_id"]').select2({
        dropdownParent: $('#modalTambahBarang'),
        placeholder: 'pilih kategori',
        ajax: {
            url: '/api/kategori',
            dataType: 'json',
            delay: 250,
            processResults: function (result) {
                return {
                    results: result.data.map(kategori => ({
                        id: kategori.kategoribar_id,      // option value
                        text: kategori.namakategoribar    // option label
                    }))
                };
            }
        }
    });

    // ===============================
    // Static Select2 (manual options)
    // ===============================
    let satuanBarang = [
        { id: 'BOX', text: 'Box' },
        { id: 'PCS', text: 'Pcs' },
        { id: 'UNIT', text: 'Unit' }
    ];

    // Append options before initializing Select2
    let selectSatuan = $('select[name="satuanbar_id"]');
    selectSatuan.empty(); // optional: clear existing

    selectSatuan.append(new Option('', '', true, true));

    satuanBarang.forEach(satuan => {
        let option = new Option(satuan.text, satuan.id, false, false);
        selectSatuan.append(option);
    });

    // Initialize Select2 AFTER appending options
    selectSatuan.select2({
        dropdownParent: $('#modalTambahBarang'),
        placeholder: 'pilih satuan'
    });

    // Tambah Barang button
    $(document).on('click', '#btnTambahBarang', function () {
        const modal = new bootstrap.Modal(document.getElementById('modalTambahBarang'));
        modal.show();
    });

    // Image preview
    $(document).on('change', 'input[name="link_gambar_bar"]', function () {
        const file = this.files[0];
        if (!file) return $('#preview_gambar').addClass('d-none');
        const reader = new FileReader();
        reader.onload = e => $('#preview_gambar').attr('src', e.target.result).removeClass('d-none');
        reader.readAsDataURL(file);
    });

    $(document).on('change', 'input[name="link_gambar_bar_edit"]', function () {
        const file = this.files[0];
        if (!file) return $('#preview_gambar_edit').addClass('d-none');
        const reader = new FileReader();
        reader.onload = e => $('#preview_gambar_edit').attr('src', e.target.result).removeClass('d-none');
        reader.readAsDataURL(file);
    });

    // Submit form
    $(document).on('submit', '#formTambahBarang', function (e) {
        e.preventDefault();

        const form = this;
        const formData = new FormData(form);

        Swal.fire({
            title: 'Menyimpan data...',
            text: 'Mohon tunggu.',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: '/api/StockGudang',
            method: 'POST',
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                Swal.fire({
                    icon: 'success',
                    title: 'Berhasil',
                    text: res.message || 'Barang berhasil ditambahkan!',
                    // timer: 1500,
                    // showConfirmButton: false
                });

                // Close modal & reset
                bootstrap.Modal.getInstance(document.getElementById('modalTambahBarang')).hide();
                form.reset();
                $('#preview_gambar').addClass('d-none');

                $('select[name="kategoribar_id"]').val(null).trigger('change');
                $('select[name="satuanbar_id"]').val(null).trigger('change');

                // Reload table
                // $('#datatable-buttons').DataTable().ajax.reload(null, false);
                table.ajax.reload(null, false);
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menambah barang.', 'error');
            }
        });
    });

    $(document).on('click', '.btn-detail', function () {
        const id = $(this).data('id');

        $.ajax({
            url: `/api/StockGudang/${id}`,
            method: 'GET',
            beforeSend: function () {
                Swal.fire({
                    title: 'Menampilkan data...',
                    text: 'Mohon tunggu.',
                    allowOutsideClick: false,
                    didOpen: () => Swal.showLoading()
                });
            },
            success: function (res) {
                const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                $('body').css('padding-right', scrollbarWidth + 'px');
                Swal.close();
                let resData = typeof res === 'string' ? JSON.parse(res) : res;
                let data = resData.data;
                let msl = data.msl_barang > 0 ? data.msl_barang : 1;
                let status = Math.round((data.jumlah_barang / msl) * 100);

                $('img.card-img-top-custom').on('error', function() {
                    this.src = '/assets/images/dummy.png';
                });

                // Update modal content
                $('#itemDetailModal img').attr('src', data.link_gambar_bar || '/assets/images/dummy.png');
                $('#itemDetailModal .nama-barang').text(data.nama_barang);
                $('#itemDetailModal .kategori-barang').text(data.categoryDto?.namakategoribar || '-');
                $('#itemDetailModal .satuan-barang').text(data.uomDto?.nama_satuanbar || '-');
                $('#itemDetailModal .stok-barang').text(data.jumlah_barang - data.booked_qty);
                $('#itemDetailModal .booked-barang').text(data.booked_qty);
                $('#itemDetailModal .jumlah-barang').text(data.jumlah_barang);
                $('#itemDetailModal .msl-barang').text(data.msl_barang);
                $('#itemDetailModal .status-barang').text(status + '%');

                // Adjust status badge color
                const statusBadge = $('#itemDetailModal .status-barang').closest('.badge');
                statusBadge
                    .removeClass('bg-success bg-warning bg-danger')
                    .addClass(status >= 115 ? 'bg-success' : status >= 100 ? 'bg-warning' : 'bg-danger');

                // Show modal
                const modal = new bootstrap.Modal(document.getElementById('itemDetailModal'));
                modal.show();
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat detail barang.', 'error');
            }
        });
    });

    // Select2 Initialization (Edit)
    $('select[name="kategoribar_id_edit"]').select2({
        dropdownParent: $('#modalEditBarang'),
        placeholder: 'pilih kategori',
        ajax: {
            url: '/api/kategori',
            dataType: 'json',
            delay: 250,
            processResults: function (result) {
                return {
                    results: result.data.map(kategori => ({
                        id: kategori.kategoribar_id,      // option value
                        text: kategori.namakategoribar    // option label
                    }))
                };
            }
        }
    });

    // Append options before initializing Select2
    let selectSatuanEdit = $('select[name="satuanbar_id_edit"]');
    selectSatuanEdit.empty(); // optional: clear existing

    selectSatuanEdit.append(new Option('', '', true, true));

    satuanBarang.forEach(satuan => {
        let option = new Option(satuan.text, satuan.id, false, false);
        selectSatuanEdit.append(option);
    });

    // Initialize Select2 AFTER appending options
    selectSatuanEdit.select2({
        dropdownParent: $('#modalEditBarang'),
        placeholder: 'pilih satuan'
    });

    // Edit Barang
    $(document).on('click', '.btn-edit', function () {
        const id = $(this).data('id');

        $.ajax({
            url: `/api/StockGudang/${id}`,
            method: 'GET',
            beforeSend: function() {
                Swal.fire({
                    title: 'Menampilkan data...',
                    text: 'Mohon tunggu.',
                    allowOutsideClick: false,
                    didOpen: () => Swal.showLoading()
                });
            },
            success: function (res) {
                let resData = typeof res === 'string' ? JSON.parse(res) : res;
                let data = resData.data;
                const modal = new bootstrap.Modal(document.getElementById('modalEditBarang'));

                $('#formEditBarang')[0].reset();
                // Populate form
                $('#idEdit').val(id);
                $('input[name="nama_barang_edit"]').val(data.nama_barang);
                $('input[name="jumlah_barang_edit"]').val(data.jumlah_barang);
                $('input[name="msl_barang_edit"]').val(data.msl_barang);
                $('input[name="status_bar_edit"]').val(data.status_bar);
                // $('select[name="kategoribar_id_edit"]').val(data.kategoribar_id);
                // $('select[name="satuanbar_id_edit"]').val(data.satuanbar_id);
                $('#preview_gambar_edit').attr('src', data.link_gambar_bar || '').toggleClass('d-none', !data.link_gambar_bar);

                $('select[name="satuanbar_id_edit"]').val(data.satuanbar_id).trigger('change');

                if (data.categoryDto && data.categoryDto.namakategoribar) {
                    const option = new Option(
                        data.categoryDto.namakategoribar,  // text
                        data.kategoribar_id,                // value
                        true,                               // defaultSelected
                        true                                // selected
                    );
                    $('select[name="kategoribar_id_edit"]').append(option).trigger('change');
                }

                // Add hidden input to mark edit mode
                const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
                $('body').css('padding-right', scrollbarWidth + 'px');
                // modal.show();
                // Swal.close();
                Swal.close();
                // setTimeout(() => modal.show(), 200);
                modal.show();
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal memuat data barang untuk edit.', 'error');
            }
        });
    });

    $(document).on('submit', '#formEditBarang', function (e) {
        e.preventDefault();
        const id = $('#idEdit').val();
        const form = this;
        const originalFormData = new FormData(form);
        const cleanFormData = new FormData();

        // Remove `_edit` suffix from all field names
        for (const [key, value] of originalFormData.entries()) {
            const cleanKey = key.endsWith('_edit') ? key.slice(0, -5) : key;
            cleanFormData.append(cleanKey, value);
        }

        Swal.fire({
            title: 'Mengupdate data...',
            text: 'Mohon tunggu.',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url: `/api/StockGudang/${id}`,
            method: 'PUT',
            processData: false,
            contentType: false,
            data: cleanFormData,
            success: function (res) {
                Swal.fire({
                    icon: 'success',
                    title: 'Berhasil',
                    text: res.message || 'Barang berhasil diperbarui!'
                });

                bootstrap.Modal.getInstance(document.getElementById('modalEditBarang')).hide();
                form.reset();
                $('#preview_gambar_edit').addClass('d-none');

                $('select[name="kategoribar_id_edit"]').val(null).trigger('change');
                $('select[name="satuanbar_id_edit"]').val(null).trigger('change');

                // Reload table
                table.ajax.reload(null, false);
            },
            error: function (xhr) {
                Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menyimpan data.', 'error');
            }
        });
    });

    // DELETE BARANG (SINGLE)
    $(document).on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Yakin ingin menghapus data barang?',
            text: 'Data barang ini akan dihapus secara permanen.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Ya, hapus!',
            cancelButtonText: 'Batal',
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Menghapus data...',
                    text: 'Mohon tunggu.',
                    allowOutsideClick: false,
                    didOpen: () => Swal.showLoading()
                });

                $.ajax({
                    url: `/api/StockGudang/${id}`,
                    method: 'DELETE',
                    success: function (res) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Berhasil',
                            text: res.message || 'Barang berhasil dihapus!'
                        }).then(()=>{
                            // Refresh table
                            table.ajax.reload();
                        });

                    },
                    error: function (xhr) {
                        Swal.fire('Error', xhr.responseJSON?.message || 'Gagal menghapus barang.', 'error');
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
            title: `Hapus ${selectedData.length} barang terpilih?`,
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
                url: `/api/StockGudang/${item.barang_id}`,
                type: 'DELETE'
            });
        });

        Promise.allSettled(deletePromises)
            .then(results => {
                const successCount = results.filter(r => r.status === 'fulfilled').length;
                const failCount = results.length - successCount;

                let msg = `Berhasil menghapus ${successCount} barang.`;
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