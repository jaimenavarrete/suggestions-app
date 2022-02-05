// Delete button confirm dialog
const deleteSuggestionForms = document.querySelectorAll(".delete-suggestion-form");

deleteSuggestionForms.forEach(form => {
    form.addEventListener('submit', (e) => {
        e.preventDefault();

        Swal.fire({
            title: '¿Está seguro?',
            text: "Si borra la sugerencia, no podrá recuperarla",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, borrar'
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit();
            }
        });
    });
});