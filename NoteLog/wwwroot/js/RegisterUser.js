$(document).ready(function () {

    $('body').on('click', '#register', function () {
        var registerUser = {
            firstName: $('#firstName').val(),
            lastName: $('#lastName').val(),
            userName: $('#userName').val(),
            email: $('#email').val(),
            password: $('#password').val()
        }

        $.ajax({
            url: 'RegisterUser',
            type: 'POST',
            dataType: 'json',
            data: registerUser,
            success: function (result) {
                console.log(result.Url);
                //successAlert('Hecho!', "Usuario creado con éxito");
                window.location.href = 'Login';
            },
            error: function (error) {
                console.log(error);
                warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
            }
        });
    });
});
