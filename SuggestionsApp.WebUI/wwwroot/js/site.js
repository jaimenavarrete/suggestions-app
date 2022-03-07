// Delete button confirm dialog
const deleteElementButton = document.querySelectorAll(".delete-element-form");

deleteElementButton.forEach(form => {
    form.addEventListener('submit', (e) => {
        e.preventDefault();

        Swal.fire({
            title: '¿Está seguro?',
            text: "Si borra el elemento, no podrá recuperarlo",
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