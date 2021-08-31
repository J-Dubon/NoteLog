
$('#tableNote').load('/Notes/_NotesList');

$('#BodyNote').load('/Notes/_NotesNew/');

$(document).ready(function () {

    // =========== Vistas parciales =============
    // Carga la vista parcial de nueva nota
    $('body').on('click', '#btnNoteNew', function () {
        $('#BodyNote').load('/Notes/_NotesNew/');
    });

    // Carga la vista parcial para visualizar / editar las notas
    $('body').on('click', '.btnNoteView', function () {
        var btnNoteId = $(this).attr('id');
        var id = btnNoteId.replace('btnNote', '');

        $('#BodyNote').load('/Notes/_NotesEdit/' + id);
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
        SaveOrUpdateNotes(notes);
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
        SaveOrUpdateNotes(notes);
    });

    // Método generalizado para guardar o actualizar una nota
    function SaveOrUpdateNotes(notes) {
        $.ajax({
            url: 'SaveOrUpdateNote',
            type: 'POST',
            dataType: 'json',
            data: notes,
            success: function (result) {
                if (result == true) {
                    successAlert('Hecho!', "Nota guardada con éxito");
                    $('#tableNote').load('/Notes/_NotesList');
                    $('#BodyNote').load('/Notes/_NotesNew/');
                }
                else {
                    warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
                }
            },
            error: function (error) {
                warningAlert('Algo ha salido mal', 'Por favor intentelo más tarde');
            }
        });
    }
});