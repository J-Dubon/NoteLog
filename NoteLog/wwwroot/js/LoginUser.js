$(document).ready(function () {

    $('body').on('click', '#login', function () {
        var userName = $('#userName').val();
        var password = $('#password').val();

        $.ajax({
            url: '/Account/LoginUser',
            type: 'POST',
            dataType: 'json',
            data: { userName, password },
            success: function (result) {
                if (result == true) {
                    window.location.href = '/Notes/Notes';
                }
                else {
                    warningAlert('Upss...', 'Usuario o contraseña incorrecta');
                }
            }
        });
    });
});
