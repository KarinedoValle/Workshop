function formatDate(date) {
    var localDateObject = new Date(date.getTime());
    const day = String(localDateObject.getDate()).padStart(2, '0');
    const month = String(localDateObject.getMonth() + 1).padStart(2, '0');
    const year = localDateObject.getFullYear();
    const hours = String(localDateObject.getHours()).padStart(2, '0');
    const minutes = String(localDateObject.getMinutes()).padStart(2, '0');

     return `${day}/${month}/${year} ${hours}:${minutes}h`;
}


function checkField(field) {
    if (!field.value) {
        field.classList.add('error-border');
    } else {
        field.classList.remove('error-border');
    }
    return field.value;
}

function validaWorkshopCamposPrincipais(formData) {
    if (!formData.Nome || !formData.Descricao || !formData.UsuarioCpf || !formData.Categoria || !formData.Modalidade || !formData.Status)
        return false;
    return true;
}

function validateFormUsuario(formData) {
    if (!formData.Nome || !formData.CPF || !formData.Telefone || !formData.Email || !formData.Login || !formData.Senha)
        return false;
    return true;
}

function addDateField() {
    const date = new Date(document.querySelector('input[name="Datas"]').value);
    if (isNaN(date.getTime())) {
        alert("Selecione uma data para adicionar.");
        return;
    }

    const container = document.getElementById('datasContainer');
    const existingDates = Array.from(container.querySelectorAll('p[name="Datas"]')).map(p => p.id);

    if (existingDates.includes(document.querySelector('input[name="Datas"]').value)) {
        return;
    }

    CriarCampoDeData(document.querySelector('input[name="Datas"]').value);
}

function CriarCampoDeData(dateString) {
    const container = document.getElementById('datasContainer');
    const newDateField = document.createElement('p');
    newDateField.setAttribute('name', 'Datas');
    newDateField.id = dateString;
    newDateField.className = "pDatas";
    newDateField.textContent = formatDate(new Date(dateString));

    const deleteIcon = document.createElement('span');
    deleteIcon.className = "fas fa-trash-alt delete-icon";
    deleteIcon.style.cursor = "pointer";

    deleteIcon.addEventListener('click', function() {
        container.removeChild(newDateField);
    });

    newDateField.appendChild(deleteIcon);
    container.appendChild(newDateField);
}

function Voltar(pagina) {
    window.location.href = `/${pagina}`;
}
