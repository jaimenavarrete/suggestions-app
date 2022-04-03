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


// Modal form validations
const inputsCreate = document.querySelectorAll('.input-create');

// Clear inputs value
const clearInputs = () => {
    inputsCreate.forEach((input) => (input.value = ''));

    // Hide invalid feedback
    const activeFeedback = document.querySelectorAll('.is-invalid');
    activeFeedback.forEach((label) => label.classList.remove('is-invalid'));
}

const clearFeedback = (input, feedback) => {
    input.classList.remove('is-invalid');
    feedback.innerHTML = '';
}

// Validate inputs
const validNameInput = () => {
    const nameInput = document.getElementById('input-name');
    const nameInvalidLabel = document.getElementById('feedback-name');
    let valid = true;

    clearFeedback(nameInput, nameInvalidLabel);

    if (!nameInput.value) {
        nameInvalidLabel.innerHTML += `Debe agregar un nombre.<br/>`;
        valid = false;
    }

    if (nameInput.value.length > 50) {
        nameInvalidLabel.innerHTML += `Debe tener menos de 50 caracteres (Actual: ${nameInput.value.length}).`;
        valid = false;
    }

    if (!valid) nameInput.classList.add('is-invalid');

    return valid;
};

const validTitleInput = () => {
    const titleInput = document.getElementById('input-title');
    const titleInvalidLabel = document.getElementById('feedback-title');
    let valid = true;

    clearFeedback(titleInput, titleInvalidLabel);

    if (!titleInput.value) {
        titleInvalidLabel.innerHTML += `Debe agregar un título a la sugerencia.<br/>`;
        valid = false;
    }

    if (titleInput.value.length > 100) {
        titleInvalidLabel.innerHTML += `Debe tener menos de 100 caracteres (Actual: ${titleInput.value.length}).`;
        valid = false;
    }

    if (!valid) titleInput.classList.add('is-invalid');

    return valid;
};

const validDescriptionInput = () => {
    const descriptionInput = document.getElementById('input-description');
    const descriptionInvalidLabel = document.getElementById('feedback-description');
    let valid = true;

    clearFeedback(descriptionInput, descriptionInvalidLabel);

    if (!descriptionInput.value) {
        descriptionInvalidLabel.innerHTML += 'Debe agregar una descripción.<br/>';
        valid = false;
    }

    if (descriptionInput.value.length > 100) {
        descriptionInvalidLabel.innerHTML += `Debe tener menos de 100 caracteres (Actual: ${descriptionInput.value.length}).`;
        valid = false;
    }

    if (!valid) descriptionInput.classList.add('is-invalid');

    return valid;
};

const validSuggestionDescriptionInput = () => {
    const descriptionInput = document.getElementById('input-suggestion-description');
    const descriptionInvalidLabel = document.getElementById('feedback-suggestion-description');
    let valid = true;

    clearFeedback(descriptionInput, descriptionInvalidLabel);

    if (!descriptionInput.value) {
        descriptionInvalidLabel.innerHTML += 'Debe agregar una descripción a la sugerencia.<br/>';
        valid = false;
    }

    if (descriptionInput.value.length > 1000) {
        descriptionInvalidLabel.innerHTML += `Debe tener menos de 1000 caracteres (Actual: ${descriptionInput.value.length}).`;
        valid = false;
    }

    if (!valid) descriptionInput.classList.add('is-invalid');

    return valid;
};

const validUserNameInput = () => {
    const userNameInput = document.getElementById('input-username');
    const userNameInvalidLabel = document.getElementById('feedback-username');
    let valid = true;

    clearFeedback(userNameInput, userNameInvalidLabel);

    if (!userNameInput.value) {
        userNameInvalidLabel.innerHTML += 'Debe agregar un nombre de usuario.<br/>';
        valid = false;
    }

    if (userNameInput.value.length < 5) {
        userNameInvalidLabel.innerHTML += 'Requiere al menos 5 caracteres.';
        valid = false;
    }

    if (!valid) userNameInput.classList.add('is-invalid');

    return valid;
};

const validEmailInput = () => {
    const emailInput = document.getElementById('input-email');
    const emailInvalidLabel = document.getElementById('feedback-email');
    let valid = true;
    let emailRegex = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,4})+$/;

    clearFeedback(emailInput, emailInvalidLabel);

    if (!emailInput.value) {
        emailInvalidLabel.innerHTML += 'Debe agregar un email.<br/>';
        valid = false;
    }

    if (!emailRegex.test(emailInput.value)) {
        emailInvalidLabel.innerHTML += 'Debe agregar un email válido.<br/>';
        valid = false;
    }

    if (!valid) emailInput.classList.add('is-invalid');

    return valid;
};

const validRoleInput = () => {
    const roleInput = document.getElementById('input-role');
    let valid = true;

    // Clear elements
    roleInput.classList.remove('is-invalid');

    if (!roleInput.value) {
        roleInput.classList.add('is-invalid');
        valid = false;
    }

    return valid;
};

const validPasswordInput = () => {
    const passwordInput = document.getElementById('input-password');
    const passwordInvalidLabel = document.getElementById('feedback-password');
    let valid = true;

    clearFeedback(passwordInput, passwordInvalidLabel);

    if (!passwordInput.value) {
        passwordInvalidLabel.innerHTML += 'Debe agregar una contraseña.<br>';
        valid = false;
    }

    // Digit validation
    if (passwordInput.value.search(/\d/) === -1) {
        passwordInvalidLabel.innerHTML += 'Requiere al menos 1 dígito.<br>';
        valid = false;
    }
    // Lowercase validation
    if (passwordInput.value.search(/[a-z]/) === -1) {
        passwordInvalidLabel.innerHTML += 'Requiere al menos 1 caracter en minúscula.<br>';
        valid = false;
    }
    // Uppercase validation
    if (passwordInput.value.search(/[A-Z]/) === -1) {
        passwordInvalidLabel.innerHTML += 'Requiere al menos 1 caracter en mayúscula.<br>';
        valid = false;
    }
    // Alphanumeric validation
    if (passwordInput.value.search(/\W/) === -1) {
        passwordInvalidLabel.innerHTML += 'Requiere al menos 1 caracter alfanumérico.<br>';
        valid = false;
    }
    // Length validation
    if (passwordInput.value.length < 6) {
        passwordInvalidLabel.innerHTML += 'Requiere al menos 6 caracteres.<br>';
        valid = false;
    }

    if (!valid) passwordInput.classList.add('is-invalid');

    return valid;
};

const validRepeatPasswordInput = () => {
    const passwordInput = document.getElementById('input-password');
    const repeatPasswordInput = document.getElementById('input-repeat-password');
    let valid = true;

    // Clear elements
    repeatPasswordInput.classList.remove('is-invalid');

    if (passwordInput.value !== repeatPasswordInput.value) {
        repeatPasswordInput.classList.add('is-invalid');
        valid = false;
    }

    return valid;
};


const validateInput = (input) => {
    switch (input.id) {
        case 'input-name':
            return validNameInput();
        case 'input-title':
            return validTitleInput();
        case 'input-description':
            return validDescriptionInput();
        case 'input-suggestion-description':
            return validSuggestionDescriptionInput();
        case 'input-username':
            return validUserNameInput();
        case 'input-email':
            return validEmailInput();
        case 'input-role':
            return validRoleInput();
        case 'input-password':
            return validPasswordInput();
        case 'input-repeat-password':
            return validRepeatPasswordInput();
    }
};

const validateForm = () => {
    let valid = true;

    inputsCreate.forEach((input) => {
        if (!validateInput(input)) {
            valid = false;
        }
    });

    return valid;
};