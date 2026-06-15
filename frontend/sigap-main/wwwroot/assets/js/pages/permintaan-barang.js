$(function () {
    const { fungsi_pekerja, bagianId_pekerja, bagianNama_pekerja } = window.permintaanbarang;

    function htmlUnescape(str) {
        const txt = document.createElement("textarea");
        txt.innerHTML = str;
        return txt.value;
    }
    // block kode tkjp
    $('#tkjpSection').hide();

    // $('#tkjpCheckbox').on('change', function () {
    //     if ($(this).is(":checked")) {
    //         $('#barang-input-wrapper').removeClass('mt-2');
    //         $('#barang-input-wrapper').prependTo('#barang-tkjp-col-wrapper');
    //         // $('#tkjpSection').fadeIn();
    //         $('#tkjpSection').show();
    //     } else {
    //         // $('#tkjpSection').fadeOut(function () {
    //         $('#tkjpSection').hide();
    //         $('#barang-input-wrapper').addClass('mt-2');
    //         $('#barang-input-wrapper').appendTo('#barang-col-wrapper');
    //         // });
    //     }
    // });

    $('#tkjpCheckbox').on('change', function () {
        const isTKJP = $(this).is(':checked');
        const isMobile = window.matchMedia('(max-width: 767.98px)').matches;

        if (isTKJP) {
            $('#tkjpSection').show();
            const unescapedNama = htmlUnescape(bagianNama_pekerja);
            $('#bagianTKJP')
                .empty()
                .append(new Option(unescapedNama, bagianId_pekerja, true, true))
                .trigger('change');
            $('#bagianTKJP').prop('disabled', true); 
            
            if (isMobile) {
                // move barang input to bottom of TKJP section
                $('#barang-input-wrapper').appendTo('#tkjpSection');
            } else {
                // keep inside TKJP column (like your original code)
                $('#barang-input-wrapper').prependTo('#barang-tkjp-col-wrapper');
            }
        } else {
            $('#tkjpSection').hide();
            $('#bagianTKJP').empty().trigger('change');
            $('#bagianTKJP').prop('disabled', false);
            $('#barang-input-wrapper').appendTo('#barang-col-wrapper');
        }
    });

    // Automatically adjust when resizing screen
    $(window).on('resize', function () {
        if ($('#tkjpCheckbox').is(':checked')) {
            const isMobile = window.matchMedia('(max-width: 767.98px)').matches;

            if (isMobile) {
                $('#barang-input-wrapper').appendTo('#tkjpSection');
            } else {
                $('#barang-input-wrapper').prependTo('#barang-tkjp-col-wrapper');
            }
        }
    });

    $('.select2').select2({ width: '100%' });

    function destroyManualInput() {
        // Replace manual input back to select2
        const select = $('<select>', {
            id: 'id-finger-select',
            class: 'form-control select2',
            required: true
        });

        $('#id-finger-select').replaceWith(select);

        // Reinitialize Select2
        initIdFingerSelect();

        // const bagianSelect = $('#bagianTKJP');

        // if (bagianSelect.data('select2')) {
        //     bagianSelect.select2('destroy');
        // }

        // const bagianInput = $('<input>', {
        //     type: 'text',
        //     id: 'bagianTKJP',
        //     class: 'form-control',
        //     required: true
        // });

        // bagianSelect.replaceWith(bagianInput);

        $('#bagianTKJP').empty().trigger('change');

        // Disable the TKJP text fields again
        $('#tkjpSection input[type="text"]').prop('disabled', true).val('');
        $('#tkjpSection input[type="text"]').prop('placeholder', '');
    }

    // Handle "Isi Manual" checkbox behavior
    $('#isiManual').on('change', function () {
        const isManual = $(this).is(':checked');
        const $fingerSelect = $('#id-finger-select');
        // const $fingerSelectContainer = $fingerSelect.closest('.form-group');
        
        if (isManual) {
            // Destroy Select2 and replace with plain input
            if ($fingerSelect.data('select2')) {
                $fingerSelect.select2('destroy');
            }

            const manualInput = $('<input>', {
                type: 'text',
                id: 'id-finger-select',
                class: 'form-control',
                placeholder: 'masukkan id finger',
                required: true
            });

            $fingerSelect.replaceWith(manualInput);

            // const selectBagian = $('<select>', {
            //     id: 'bagianTKJP',
            //     class: 'form-control select2',
            //     required: true,
            //     disabled: true
            // });

            // $('#bagianTKJP').replaceWith(selectBagian);

            // $.ajax({
            //     url: '/api/select2/bagianuser',
            //     data: { search: '', page: 1, pageSize: 9999 },
            //     dataType: 'json',
            //     cache: true,
            //     success: function (response) {
            //         let matched = response.results.find(x => x.id == bagianId_pekerja);

            //         if (matched) {
            //             // Insert option BEFORE select2 initialization
            //             $('#bagianTKJP')
            //                 .append(new Option(matched.text, matched.id, true, true));
            //         }

            //         // Now safely initialize Select2
            //         initBagianSelect2();
            //     }
            // });

            const unescapedNama = htmlUnescape(bagianNama_pekerja);

            // $('#bagianTKJP')
            //     .append(new Option(unescapedNama, bagianId_pekerja, true, true))
            //     .trigger('change');

            $('#bagianTKJP')
                .empty()
                .append(new Option(unescapedNama, bagianId_pekerja, true, true))
                .trigger('change');
            $('#bagianTKJP').prop('disabled', true);

            // Enable the TKJP text fields
            $('#tkjpSection input[type="text"]').prop('disabled', false).val('');
            $('#namapekerjaTKJP').prop('placeholder', 'masukkan nama pekerja');
            // $('#fungsiTKJP').prop('placeholder', 'masukkan fungsi pekerja');
            $('#fungsiTKJP').prop('disabled', true).val(fungsi_pekerja);
            $('#perusahaanTKJP').prop('placeholder', 'masukkan perusahaan pekerja');
        } else {
            destroyManualInput();
        }
    });

    function initIdFingerSelect() {
        $('#id-finger-select').select2({
            dropdownParent: $('#permintaan-barang-form'),
            placeholder: 'pilih id finger',
            ajax: {
                url: '/api/select2/employee',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page || 1,
                        pageSize: 10,
                        bagian_id: bagianId_pekerja || null
                    };
                },
                processResults: function (data) {
                    // Transform API result so Select2 uses id_finger as both id & text
                    const results = (data.results || []).map(item => ({
                        id: item.id_finger, // return the readable id_finger as value
                        text: item.id_finger, // display id_finger
                        nama_pekerja: item.nama_pekerja,
                        bagian_id: item.bagian_id,
                        bagian_nm: item.bagian_nm,
                        fungsi_pekerja: item.fungsi_pekerja,
                        perusahaan_pekerja: item.perusahaan_pekerja
                    }));

                    return {
                        results: results,
                        pagination: data.pagination
                    };
                },
                cache: true
            }
        });

        $('#id-finger-select').on('select2:select', function (e) {
            const data = e.params.data;
            $('#namapekerjaTKJP').val(data.nama_pekerja);
            $('#fungsiTKJP').val(data.fungsi_pekerja);
            // $('#bagianTKJP').val(data.bagian_id).trigger('change'); // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< GANTI APABILA BACKEND UDAH NGEFIX APINYA
            // if (data.bagian_id) {
            //     $('#bagianTKJP')
            //         .append(new Option(data.bagian_nm, data.bagian_id, true, true))
            //         .trigger('change');
            // } else {
            //     $('#bagianTKJP').empty().trigger('change');
            // }
            $('#perusahaanTKJP').val(data.perusahaan_pekerja);
        });
    }

    initIdFingerSelect();

    // function initBagianSelect2() {
    //     $('#bagianTKJP').select2({
    //         dropdownParent: $('#permintaan-barang-form'),
    //         ajax: {
    //             url: '/api/select2/bagianuser',
    //             dataType: 'json',
    //             delay: 250,
    //             cache: true,
    //             data: function (params) {
    //                 return {
    //                     search: params.term,
    //                     page: params.page || 1,
    //                     pageSize: 9999
    //                 };
    //             },
    //             processResults: function (data) {
    //                 return data;
    //             }
    //         }
    //     });
    // }

    // $.ajax({
    //     url: '/api/select2/bagianuser',
    //     data: { search: '', page: 1, pageSize: 9999 },
    //     dataType: 'json',
    //     cache: true,
    //     success: function (response) {
    //         let matched = response.results.find(x => x.id == bagianId_pekerja);

    //         if (matched) {
    //             // Insert option BEFORE select2 initialization
    //             $('#bagianTKJP')
    //                 .append(new Option(matched.text, matched.id, true, true));
    //         }

    //         // Now safely initialize Select2
    //         initBagianSelect2();
    //     }
    // });

    $('#bagianpekerja').select2({
        dropdownParent: $('#permintaan-barang-form'),
        ajax: {
            url: '/api/select2/bagianuser',
            dataType: 'json',
            delay: 250,
            cache: true,
            data: function (params) {
                return {
                    search: params.term,
                    page: params.page || 1,
                    pageSize: 9999
                };
            },
            processResults: function (data) {
                return data;
            }
        }
    });

    $('#bagianTKJP').select2({
        dropdownParent: $('#permintaan-barang-form'),
        ajax: {
            url: '/api/select2/bagianuser',
            dataType: 'json',
            delay: 250,
            cache: true,
            data: function (params) {
                return {
                    search: params.term,
                    page: params.page || 1,
                    pageSize: 9999
                };
            },
            processResults: function (data) {
                return data;
            }
        }
    });

    // barang select
    let cachedBarangData = [];

    function initBarangSelect(localData) {
        const $select = $('#barang-select');

        // Destroy old Select2 instance (if any)
        if ($select.hasClass("select2-hidden-accessible")) {
            $select.select2('destroy');
        }

        // Clear existing options and add a real placeholder option
        $select.empty().append('<option></option>');

        // Initialize Select2 with local data
        $select.select2({
            dropdownParent: $('#permintaan-barang-form'),
            placeholder: 'pilih barang',
            data: localData,
            templateResult: function (data) {
                return data.children ? $('<strong>' + data.text + '</strong>') : data.text;
            },
            templateSelection: function (data) {
                return data.text || 'Pilih Barang';
            }
        });

        // Make sure nothing is selected
        $select.val(null).trigger('change');
    }

    function loadBarang() {
        // const $select = $('#barang-select');

        // // Destroy old Select2 instance (if any)
        // if ($select.hasClass("select2-hidden-accessible")) {
        //     $select.select2('destroy');
        // }

        // // Clear existing options and add a real placeholder option
        // $select.empty().append('<option></option>');

        // // $('#barang-select').select2({
        // $select.select2({
        //     dropdownParent: $('#permintaan-barang-form'),
        //     placeholder: 'pilih barang',
        //     ajax: {
        //         url: '/api/StockGudang',
        //         dataType: 'json',
        //         delay: 250,
        //         processResults: function (response) {
        //             return {
        //                 results: response.data.map(item => ({
        //                     id: item.barang_id,
        //                     text: item.nama_barang,
        //                     stok: item.jumlah_barang - item.booked_qty,
        //                     img: item.link_gambar_bar
        //                 }))
        //             };
        //         },
        //         cache: true
        //     }
        // });

        // // Make sure nothing is selected
        // $select.val(null).trigger('change');

        $.ajax({
            url: '/api/Dashboard/readiness',
            dataType: 'json',
            success: function (response) {
                if (!response.success || !Array.isArray(response.data)) {
                    return;
                }

                const results = response.data.map(category => {
                    const items = (category.itemDto || []).map(item => ({
                        id: item.barang_id,
                        text: item.nama_barang,
                        // stok: item.jumlah_barang,
                        stok: item.jumlah_barang - item.booked_qty,
                        booked: item.booked_qty,
                        img: item.link_gambar_bar
                    }));

                    return {
                        text: category.namakategoribar,
                        children: items
                    };
                }).filter(group => group.children.length > 0);

                cachedBarangData = results; // cache for reuse

                initBarangSelect(cachedBarangData);
            }
        });
    }

    // First load (fetch once from API)
    if (cachedBarangData.length === 0) {
        loadBarang();
    } else {
        // already cached, just init locally
        initBarangSelect(cachedBarangData);
    }
    // loadBarang(); // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< GANTIIIIIIIIIIIIIIIIIII

    $('#barang-select').on('select2:select', function (e) {
        const data = e.params.data;

        const selectedOption = $(`#barang-select option[value="${data.id}"]`)[0];
        if (!selectedOption) {
            return;
        }

        const id = data.id;

        const existingCard = $(`[data-group-id="${id}"]`)[0];
        if (existingCard) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Barang sudah ditambahkan!',
            });
            $('#barang-select').val(null).trigger('change.select2');
            return;
        }

        const text = data.text;
        const stok = data.stok;
        const img = data.img || '/assets/images/dummy.png';
        // const stok = $(selectedOption).attr('stok');
        // const img = $(selectedOption).attr('data-img');

        // const groupHTML = `
        //     <div class="col d-flex flex-column align-items-center group-card" data-group-id="${id}">
        //         <div class="row w-100 h-100">
        //             <div class="col">    
        //                 <div id="card-permintaan" class="card w-100 h-100 justify-content-center mb-1">
        //                     <div class="card-header">
        //                         <img src="${img}" class="card-img-top-custom" alt="${text}" >
        //                     </div>
        //                     <div class="card-body d-flex flex-column align-items-center justify-content-center text-white text-center">
        //                         <h5 class="card-title text-break">${text}</h5>
        //                         <h5 class="card-title text-break card-title-stok mb-0">Stok: ${stok}</h5>
        //                     </div>
        //                 </div>
        //             </div>
        //             <div class="col d-flex flex-column justify-content-center align-items-center">
        //                 <div class="form-group mb-3 w-100">
		// 					<label for="keterangan">Keterangan</label>
        //                     <input type="text" class="form-control keterangan-input">
        //                 </div>
        //                 <div class="row g-3 align-items-center mb-3 w-100">
        //                     <div class="col ps-0">
        //                         <div class="input-group">
        //                             <input type="number" class="form-control text-center quantity-input rounded-left" value="0" min="1" step="1">
        //                             <button class="btn btn-outline-secondary btn-decrement border-end-0" type="button"><i class="mdi mdi-minus-circle-outline"></i></button>
        //                             <button class="btn btn-outline-secondary btn-increment border-start-0" style="border-radius: 0px 4px 4px 0px !important" type="button"><i class="mdi mdi-plus-circle-outline"></i></button>
        //                         </div>
        //                     </div>
        //                     <div class="col-auto pe-0">
        //                         <a class="btn btn-danger btn-delete d-flex justify-content-center align-items-center">
        //                             <i class="mdi mdi-trash-can"></i>
        //                         </a>
        //                     </div>
        //                 </div>
        //             </div>
        //         </div>
        //     </div>
        // `;

        const groupHTML = `
            <div class="col d-flex flex-column align-items-center group-card" data-group-id="${id}">
                <div class="card w-100 h-100 justify-content-center mb-1">
                    <div class="card-header">
                        <img src="${img}" class="card-img-top-custom" alt="${text}" >
                    </div>
                    <div class="card-body d-flex flex-column align-items-center justify-content-center text-white text-center">
                        <h5 class="card-title text-break">${text}</h5>
                        <h5 class="card-title text-break card-title-stok mb-0">Stok: ${stok}</h5>
                    </div>
                </div>
                <div class="input-group mb-3">
                    <button class="btn btn-outline-secondary btn-decrement" type="button"><i class="mdi mdi-minus-circle-outline"></i></button>
                    <input type="number" class="form-control text-center quantity-input" value="0" min="1" max="${stok}" step="1">
                    <button class="btn btn-outline-secondary btn-increment" type="button"><i class="mdi mdi-plus-circle-outline"></i></button>
                </div>
                <a class="btn btn-danger btn-delete">
                    <i class="mdi mdi-trash-can"></i>
                    <span>Hapus</span>
                </a>
            </div>
        `;

        $('#input-groups-container').append(groupHTML);
        $('#barang-select').val(null).trigger('change.select2');
    });

    // Tombol-tombol dibawah dibuat menggunakan teknik propagation
    $('#input-groups-container').on('click', '.btn-increment, .btn-decrement, .btn-delete', function () {
        const $button = $(this);
        const isIncrement = $button.hasClass('btn-increment');
        const isDecrement = $button.hasClass('btn-decrement');
        const isDelete = $button.hasClass('btn-delete');

        if (isIncrement || isDecrement) {
            const $inputGroup = $button.closest('.input-group');
            const $input = $inputGroup.find('.quantity-input');
            const step = parseInt($input.attr('step')) || 1;
            const max = parseInt($input.attr('max')) || Infinity;
            const min = 0;
            // const min = parseInt($input.attr('min')) || 0;
            const value = parseInt($input.val() || 0);

            if (isIncrement) {
                // $input.val(value + step);
                $input.val(Math.min(max, value + step));
            } else {
                $input.val(Math.max(min, value - step));
            }
        }

        if (isDelete) {
            const $group = $button.closest('.group-card');
            if ($group.length) {
                $group.remove();
            }
        }
    });

    $('#input-groups-container').on('change', '.quantity-input', function () {
        const $input = $(this);
        let value = $input.val().replace(/\D/g, '');

        value = value.replace(/^0+/, '');

        if (value === '') value = '0';

        const max = parseInt($input.attr('max')) || Infinity;
        const min = 0;

        let num = parseInt(value);
        if (num > max) num = max;
        if (num < min) num = min;

        $input.val(num);
    });

    // Input text hanya boleh huruf dan spasi
    // $('#submit-permintaan-barang').on('click', function (e) {
    $('#permintaan-barang-form').on('submit', function (e) {
        e.preventDefault();
        // Validasi input
        if (!$('#namapekerja').val()) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Nama Pekerja harus diisi!',
            });
            return;
        }
        
        if (!$('#nopekerja').val()) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Nomor Pekerja harus diisi!',
            });
            return;
        }

        if (!$('#bagianpekerja').val()) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Bagian harus diisi!',
            });
            return;
        }

        if ($('.group-card').length === 0) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Tidak ada barang yang dipilih!',
                //    html: `
                //        1. Tidak ada barang yang dipilih!<br>
                //        2. Test 2<br>
                //        3. Test 3<br>
                //    `
            });
            return;
        }

        let hasZeroQuantity = false;
        $('#input-groups-container .group-card').each(function () {
            const qty = parseInt($(this).find('.quantity-input').val() || "0");
            if (qty <= 0) {
                hasZeroQuantity = true;
                return false; // break loop
            }
        });

        if (hasZeroQuantity) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Semua barang harus memiliki jumlah lebih dari 0!',
            });
            return;
        }

        if (!$('#keterangan').val().trim()) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Keterangan / Alasan Permintaan harus diisi!',
            });
            return;
        }

        Swal.fire({
            title: 'Menyimpan...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        const formData = new FormData();
        const isTKJP = $('#tkjpCheckbox').is(':checked');

        // TransactionHistory
        formData.append("transactionHistory.kategori_transact_id", "OUT");
        formData.append("transactionHistory.no_miv_safety", $('#no_miv_safety').val() ||"MIV-OUT");
        formData.append("transactionHistory.no_miv_custom", "");
        formData.append("transactionHistory.users_cache_id", $("#userId").val());
        formData.append("transactionHistory.keterangan", $("#keterangan").val());

        // TransactionDetail (repeated JSON strings)
        $('#input-groups-container .group-card').each(function () {
            const id = $(this).data('group-id');
            const qty = parseInt($(this).find('.quantity-input').val() || "1");
            formData.append("transactionDetail", JSON.stringify({ barang_id: id, jumlah_bar: qty.toString() }));
        });

        // EmployeeRequest + Kategori_Pekerja
        if (isTKJP) {
            const file = $("#filependukung")[0]?.files?.[0];
            formData.append("transactionHistory.kategori_pekerja", "KON");
            formData.append("employeeRequest.nama_pekerja", $('#namapekerjaTKJP').val());
            formData.append("employeeRequest.fungsi_pekerja", $('#fungsiTKJP').val());
            formData.append("employeeRequest.id_finger", $('#id-finger-select').val());
            formData.append("employeeRequest.perusahaan_pekerja", $('#perusahaanTKJP').val());
            if (file) formData.append("employeeRequest.link_file_pendukung", file);
            else formData.append("employeeRequest.link_file_pendukung", "");
            formData.append("employeeRequest.bagian_id", bagianId_pekerja); // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  WAJIB GANTI NANTI KALAU UDAH DI FIX BACEND APINYA
            // formData.append("employeeRequest.bagian_id", $('#bagianTKJP').val()); // Harusnya ini yg benar, bukan yg atas
        } else {
            formData.append("transactionHistory.kategori_pekerja", "OWN");
            formData.append("employeeRequest.nama_pekerja", $('#namapekerja').val());
            formData.append("employeeRequest.id_finger", $('#nopekerja').val());
            formData.append("employeeRequest.bagian_id", $('#bagianpekerja').val());
            formData.append("employeeRequest.fungsi_pekerja", "");
            formData.append("employeeRequest.perusahaan_pekerja", "Internal Pertamina");
            formData.append("employeeRequest.link_file_pendukung", "");
        }

        // const file = $("#link_file_pendukung")[0]?.files?.[0];
        // if (file) formData.append("employeeRequest.link_file_pendukung", file);
        // else formData.append("employeeRequest.link_file_pendukung", "");

        // for (const [key, value] of formData.entries()) {
        //     console.log(key, value);
        // }

        $.ajax({
            url: '/api/Transaksi',
            method: 'POST',
            processData: false,
            contentType: false,
            data: formData,
            success: async function (res) {
                await loadBarang();
                await Swal.close();
                await Swal.fire('Berhasil', res.message || 'Transaksi berhasil ditambahkan!', 'success').then(() => {
                    const isTKJP = $('#tkjpCheckbox').is(':checked');
                    const isManual = $('#isiManual').is(':checked');
                    if (isManual) {
                        $('#id-finger-select').val('');
                        destroyManualInput();
                    } else {
                        $('#id-finger-select').val(null).trigger('change.select2');
                    }
                    if (isTKJP) {
                        $('#tkjpSection').hide();
                        $('#barang-input-wrapper').appendTo('#barang-col-wrapper');
                    }

                    $('#barang-select').val(null).trigger('change.select2');
                    $('#permintaan-barang-form')[0].reset();
                    $('#tkjpCheckbox').prop('checked', false);
                    $('#isiManual').prop('checked', false);
                    $('#input-groups-container').empty();
                    $('html, body').animate({ scrollTop: $('#permintaan-barang-form').offset().top }, 'smooth');
                });
            },
            error: function (xhr) {
                Swal.close();
                const res = xhr.responseJSON;
                let msg = res?.message || 'Gagal menambahkan transaksi.';
                if (res?.errors) {
                    const firstKey = Object.keys(res.errors)[0];
                    const firstError = res.errors[firstKey]?.[0];
                    if (firstError) msg = firstError;
                }
                Swal.fire('Error', msg, 'error');
            }
        });
    });
});
