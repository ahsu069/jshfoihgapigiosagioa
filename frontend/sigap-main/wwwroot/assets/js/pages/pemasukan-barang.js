$(function () {
    // $('#barang-select').select2({
    //     dropdownParent: $('#pemasukan-barang-form'), // <-- fix missing #
    //     placeholder: 'Pilih Barang',
    //     // allowClear: true,
    //     ajax: {
    //         url: '/api/Dashboard/readiness',
    //         dataType: 'json',
    //         processResults: function (response) {
    //             if (!response.success || !Array.isArray(response.data)) {
    //                 return { results: [] };
    //             }

    //             // Map API data to Select2 optgroup structure
    //             const results = response.data.map(category => {
    //                 const items = (category.itemDto || []).map(item => ({
    //                     id: item.barang_id,          // value
    //                     text: item.nama_barang,      // label
    //                     stok: item.jumlah_barang,
    //                     img: item.link_gambar_bar
    //                 }));

    //                 return {
    //                     text: category.namakategoribar, // optgroup label
    //                     children: items                // options inside group
    //                 };
    //             }).filter(group => group.children.length > 0); // ignore empty groups

    //             return { results };
    //         }
    //     },
    //     templateResult: function (data) {
    //         // Optional: customize display with category/item distinction
    //         return data.children ? $('<strong>' + data.text + '</strong>') : data.text;
    //     },
    //     templateSelection: function (data) {
    //         return data.text || 'Pilih Barang';
    //     }
    // });

    let cachedBarangData = [];

    function initBarangSelect(localData) {
        // $('#barang-select').select2({
        //     dropdownParent: $('#pemasukan-barang-form'),
        //     placeholder: 'Pilih Barang',
        //     // allowClear: true,
        //     data: localData, // Local data for client-side search
        //     templateResult: function (data) {
        //         return data.children ? $('<strong>' + data.text + '</strong>') : data.text;
        //     },
        //     templateSelection: function (data) {
        //         return data.text || 'Pilih Barang';
        //     }
        // });
        const $select = $('#barang-select');

        // Destroy old Select2 instance (if any)
        if ($select.hasClass("select2-hidden-accessible")) {
            $select.select2('destroy');
        }

        // Clear existing options and add a real placeholder option
        $select.empty().append('<option></option>');

        // Initialize Select2 with local data
        $select.select2({
            dropdownParent: $('#pemasukan-barang-form'),
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
        // $('#barang-select').select2({
        //     dropdownParent: $('#pemasukan-barang-form'),
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
        //                     booked: item.booked_qty,
        //                     img: item.link_gambar_bar
        //                 }))
        //             };
        //         },
        //         cache: true
        //     }
        // });
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
        const booked = data.booked;
        const img = data.img || '/assets/images/dummy.png';
        // const stok = $(selectedOption).attr('stok');
        // const img = $(selectedOption).attr('img');

        const groupHTML = `
            <div class="col d-flex flex-column align-items-center group-card" data-group-id="${id}">
                <div class="card w-100 h-100 justify-content-center mb-1">
                    <div class="card-header">
                        <img src="${img}" class="card-img-top-custom" alt="${text}" >
                    </div>
                    <div class="card-body d-flex flex-column align-items-center justify-content-center text-white text-center">
                        <h5 class="card-title text-break">${text}</h5>
                        <h5 class="card-title text-break card-title-stok">Stok: ${stok}</h5>
                        <h5 class="card-title text-break card-title-stok mb-0">Booked: ${booked}</h5>
                    </div>
                </div>
                <div class="input-group mb-3">
                    <button class="btn btn-outline-secondary btn-decrement" type="button"><i class="mdi mdi-minus-circle-outline"></i></button>
                    <input type="number" class="form-control text-center quantity-input" value="0" min="1" step="1">
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
            const min = 0;
            // const min = parseInt($input.attr('min')) || 0;
            const value = parseInt($input.val() || 0);

            if (isIncrement) {
                $input.val(value + step);
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
        // let value = $input.val();
        // value = value.replace(/\D/g, '');
        let value = $input.val().replace(/\D/g, '');

        // if (value.length > 1 && value.startsWith('0')) {
        //     value = value.replace(/^0+/, '');
        // }
        value = value.replace(/^0+/, '');

        if (value === '') {
            value = '0';
        }

        $input.val(value);
    });

    // $('#submit-pemasukan-barang').on('click', function (e) {
    $('#pemasukan-barang-form').on('submit', function (e) {
        e.preventDefault();

        // if (!$('#namapekerja').val()) {
        //     Swal.fire({
        //         icon: 'error',
        //         title: 'Peringatan',
        //         text: 'Nama Pekerja harus diisi!',
        //     });
        //     return;
        // }

        // if (!$('#nopeg').val()) {
        //     Swal.fire({
        //         icon: 'error',
        //         title: 'Peringatan',
        //         text: 'Nomor Pegawai harus diisi!',
        //     });
        //     return;
        // }

        // if (!$('#jabatan').val()) {
        //     Swal.fire({
        //         icon: 'error',
        //         title: 'Peringatan',
        //         text: 'Bagian/Fungsi harus diisi!',
        //     });
        //     return;
        // }

        if ($('.group-card').length === 0) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                text: 'Tidak ada barang yang dipilih!',
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

        Swal.fire({
            title: 'Menyimpan...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            showConfirmButton: false
        });

        const formData = new FormData();

        // TransactionHistory
        formData.append("transactionHistory.kategori_transact_id", "IN");
        formData.append("transactionHistory.kategori_pekerja", "ORG");
        // formData.append("transactionHistory.no_miv_safety", "MIV-IN");
        formData.append("transactionHistory.no_miv_safety", "-");
        formData.append("transactionHistory.no_miv_custom", "");
        formData.append("transactionHistory.users_cache_id", $("#userId").val());

        // TransactionDetail (repeated JSON strings)
        $('#input-groups-container .group-card').each(function () {
            const id = $(this).data('group-id');
            const qty = parseInt($(this).find('.quantity-input').val() || "1");
            formData.append("transactionDetail", JSON.stringify({ barang_id: id, jumlah_bar: qty.toString() }));
        });

        // EmployeeRequest
        formData.append("employeeRequest.nama_pekerja", "");
        formData.append("employeeRequest.fungsi_pekerja", "");
        formData.append("employeeRequest.id_finger", "");
        formData.append("employeeRequest.perusahaan_pekerja", "");
        formData.append("employeeRequest.link_file_pendukung", "");
        formData.append("employeeRequest.bagian_id", "");

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
                    $('#pemasukan-barang-form')[0].reset();
                    $('#barang-select').val(null).trigger('change.select2');
                    $('#input-groups-container').empty();
                    $('html, body').animate({ scrollTop: $('#pemasukan-barang-form').offset().top }, 'smooth');
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
