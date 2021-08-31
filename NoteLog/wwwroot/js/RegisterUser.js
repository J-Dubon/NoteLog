$(document).ready(function () {
    // ========== Expresiones regulares ============
    var regExpFirstNameLastName = /^[a-zA-ZÑñÁáÉéÍíÓóÚúÜü\s]+$/;
    var regExpEmail = /^(?:[^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*|"[^\n"]+")@(?:[^<>()[\].,;:\s@"]+\.)+[^<>()[\]\.,;:\s@"]{2,63}$/i;

    // Método que registra un usuario en BD
    $('body').on('click', '#register', function () {
        var firstName = $('#firstName').val();
        var lastName = $('#lastName').val();
        var email = $('#email').val();

        if (regExpFirstNameLastName.test(firstName) && firstName != '' && lastName != '') {
            if (regExpEmail.test(email)) {
                var registerUser = {
                    firstName: firstName,
                    lastName: lastName,
                    userName: $('#userName').val(),
                    email: email,
                    password: $('#password').val()
                }

                $.ajax({
                    url: 'RegisterUser',
                    type: 'POST',
                    dataType: 'json',
                    data: registerUser,
                    success: function (result) {
                        successAlertRegister('Hecho!', "Usuario creado con éxito");
                        //window.location.href = 'Login';
                    },
                    error: function (error) {
                        warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
                    }
                });
            } else {
                warningAlert('Upss...', 'Verifique su email');
            }
        } else {
            warningAlert('Upss...','Verifique que su nombre esté correcto')
        }
    });

    // Alerta personalizada para el registro de usuarios
    function successAlertRegister(title, content) {
        Swal.fire({
            title: title,
            text: content,
            icon: 'success'
        }).then(function () {
            window.location.href = 'Login';
        });
    }
});
