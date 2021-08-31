$(document).ready(function () {

    // Evento para iniciar sesión
    $('body').on('click', '#login', function () {
        var userName = $('#userName').val();
        var password = $('#password').val();

        if (userName != '' && password != '') {
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
        } else {
            warningAlert('Upss...', 'Verifique sus credenciales');
        }
    });
});
