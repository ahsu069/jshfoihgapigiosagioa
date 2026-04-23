$(function () {
    $('#userphoto').on('change', function (event) {
        var output = document.getElementById('viewer');
        var file = event.target.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                output.src = e.target.result; // Set the image source to the file's data URL
            };
            reader.readAsDataURL(file); // Read the file as a data URL
            $('#viewer').removeClass('d-none');
        }
    });

    $('#submit-edit-button').on('click', function (e) {
        e.preventDefault();

        const namabarang = $('#namabarang').val().trim();
        const jumlahbarang = $('#jumlahbarang').val().trim();
        const kategori = $('#kategori').val();
        const msl = $('#msl').val().trim();
        const userphoto = $('#userphoto')[0].files.length;

        let errorMessages = [];

        if (!namabarang) errorMessages.push("Nama Barang wajib diisi!");
        if (!jumlahbarang) errorMessages.push("Jumlah Awal wajib diisi!");
        if (!kategori) errorMessages.push("Kategori wajib dipilih!");
        if (!msl) errorMessages.push("MSL wajib diisi!");
        if (userphoto === 0) errorMessages.push("Gambar wajib diunggah!");

        console.log(errorMessages);

        if (errorMessages.length > 0) {
            Swal.fire({
                icon: 'error',
                title: 'Peringatan',
                html: errorMessages.join('<br>'),
            });
        } else {
            Swal.fire({
                icon: 'success',
                title: 'Berhasil!',
                text: 'Data berhasil di edit!',
            }).then(() => {
                // Optional: submit the form
                // this.submit();
            });
        }
    });
});
