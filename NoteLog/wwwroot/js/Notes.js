
$('#tableNote').load('/Notes/NotesList');

$('#BodyNote').load('/Notes/NotesNew/');

$(document).ready(function () {

    // =========== Vistas parciales =============
    // Carga la vista parcial de nueva nota
    $('body').on('click', '#btnNoteNew', function () {
        $('#BodyNote').load('/Notes/NotesNew/');
    });

    // Carga la vista parcial para visualizar / editar las notas
    $('body').on('click', '.btnNoteView', function () {
        var btnNoteId = $(this).attr('id');
        var id = btnNoteId.replace('btnNote', '');

        $('#BodyNote').load('/Notes/NotesEdit/' + id);
    });

    // ========== Funcionalidad ============
    // Evento para guardar una nueva nota
    $('body').on('click', '#btnSaveNoteNew', function () {
        var notes = {
            id: 0,
            title: $('#txtTitleNoteNew').val(),
            subject: $('#txtSubjectNoteNew').val(),
            body: $('#txtBodyNoteNew').val(),
        }

        $.ajax({
            url: 'SaveNote',
            type: 'POST',
            dataType: 'json',
            data: notes,
            success: function (result) {
                if (result == true) {
                    successAlert('Hecho!', "Nota guardada con éxito");
                    $('#tableNote').load('/Notes/NotesList');
                    $('#BodyNote').load('/Notes/NotesNew/');
                }
                else {
                    warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
                }
            },
            error: function (error) {
                warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
            }
        });
    });

    // Evento para modificar una nota existente
    $('body').on('click', '#btnSaveNoteEdit', function () {
        var notes = {
            id: $(this).data('id'),
            title: $('#txtTitleNoteEdit').val(),
            subject: $('#txtSubjectNoteEdit').val(),
            body: $('#txtBodyNoteEdit').val(),
            createdDate: $(this).data('date')
        }

        $.ajax({
            url: 'UpdateNote',
            type: 'POST',
            dataType: 'json',
            data: notes,
            success: function (result) {
                if (result == true) {
                    successAlert('Hecho!', "Nota guardada con éxito");
                    $('#tableNote').load('/Notes/NotesList');
                    $('#BodyNote').load('/Notes/NotesNew/');
                }
                else {
                    warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
                }
            },
            error: function (error) {
                warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
            }
        });
    });

});