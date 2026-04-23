$(function () {
    // Toggle show/hide password
    $('.toggle-password').on('click', function () {
        const target = $(this).data('target');          // Get target input ID
        const $input = $('#' + target);                 // Target input element
        const $icon = $(this).find('i');                // Find icon inside button
        const type = $input.attr('type') === 'password' ? 'text' : 'password';

        // Toggle the input type
        $input.attr('type', type);

        // Toggle the eye icon
        $icon.toggleClass('mdi-eye mdi-eye-off');
    });

    // Save password button click
    // $('#btnSavePassword').on('click', function (e) {
    $('#formUbahPassword').on('submit', function (e) {
        e.preventDefault(); // prevent page reload

        const oldPassword = $('#oldPassword').val().trim();
        const newPassword = $('#newPassword').val().trim();
        const confirmPassword = $('#confirmPassword').val().trim();
        const userId = $('#userId').val().trim();
        const userNama = $('#userNama').val().trim();

        const inputPasswordLama = $('#oldPassword');
        const feedbackPasswordLama = $('#oldPasswordFeedback');
        const inputPasswordNew = $('#newPassword');
        const feedbackPasswordNew = $('#newPasswordFeedback');
        const inputPasswordConfirm = $('#confirmPassword');
        const feedbackPasswordConfirm = $('#confirmPasswordFeedback');

        // Example API call
        $.ajax({
            url: '/api/user/ubah_password',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                oldPassword: oldPassword,
                newPassword: newPassword,
                confirmPassword: confirmPassword,
                userId: userId,
                userNama: userNama
            }),
            beforeSend: function () {
                Swal.fire({
                    title: 'Memproses...',
                    didOpen: () => {
                        Swal.showLoading();
                    },
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    showConfirmButton: false
                });
            },
            success: function (res) {
                Swal.close();

                let data = res.data || res;

                Swal.fire('Success', data.message || 'Password berhasil diubah!', 'success');
                $('#formUbahPassword')[0].reset();
                $('#ubahPasswordModal').modal('hide');
            },
            error: function (xhr) {
                Swal.close();

                const errors = xhr.responseJSON?.errors || {};

                if(errors.passwordLama) {
                    inputPasswordLama.addClass('is-invalid');
                    feedbackPasswordLama
                        .removeClass('d-none')
                        .html(errors.passwordLama[0]);
                } else {
                    inputPasswordLama.removeClass('is-invalid');
                    feedbackPasswordLama.addClass('d-none');
                }

                if(errors.passwordBaru) {
                    inputPasswordNew.addClass('is-invalid');
                    feedbackPasswordNew
                        .removeClass('d-none')
                        .html(errors.passwordBaru[0]);
                    inputPasswordConfirm.addClass('is-invalid');
                    feedbackPasswordConfirm
                        .removeClass('d-none')
                        .html(errors.passwordBaru[0]);
                } else {
                    inputPasswordNew.removeClass('is-invalid');
                    feedbackPasswordNew.addClass('d-none');
                    inputPasswordConfirm.removeClass('is-invalid');
                    feedbackPasswordConfirm.addClass('d-none');
                }

                if(!errors.passwordLama && !errors.passwordBaru) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: xhr.responseJSON?.message || xhr.responseText || 'Terjadi kesalahan server.'
                    })
                }
            }
        });
    });
});
