const inputs = document.querySelectorAll('.input-edit');
inputs.forEach(input => {
    input.addEventListener('focus', function () {
        this.parentNode.classList.add('is-editing');
    });

    input.addEventListener('blur', function (ev) {
        if (ev.relatedTarget && ev.relatedTarget.classList.contains('save-edit-btn')) {
            return;
        }

        this.parentNode.classList.remove('is-editing');
    });
});

const btnCreateNewItem = document.getElementById('btn-create-new-item');
const tableBody = document.getElementById('table-body');
let isCreated = false;

btnCreateNewItem.addEventListener('click', function () {
    if (isCreated) {
        const newItemInput = document.getElementById('new-item-input');
        newItemInput.select();
        return;
    }
    isCreated = true;
    const newRow = document.createElement('tr');
    newRow.innerHTML = `<td>
                <form action="${newItemFormAction}" method="post" class="in-place-edit-form is-editing">
                    ${antiForgeryToken}
                    <input type="text" name="Name" required class="input input-edit" value="${defaultNewItemInputValue}" id="new-item-input">
                    <button type="submit" class="save-edit-btn button button-with-icon is-success is-small">
                        <span class="icon">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2" viewBox="0 0 16 16">
                                <path d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z"/>
                            </svg>
                        </span>
                        <span>Uložiť</span>
                    </button>
                </form>
            </td>
            <td style="width: 0; vertical-align: middle;">
                <button class="button button-with-icon is-warning is-small" id="btn-remove-new-item">
                    <span class="icon">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                            <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708"/>
                        </svg>
                    </span>
                    <span>Zrušiť</span>
                </button>
            </td>`

    tableBody.insertBefore(newRow, tableBody.firstChild);
    document.getElementById('new-item-input').select();

    const btnRemoveNewItem = document.getElementById('btn-remove-new-item');
    btnRemoveNewItem.addEventListener('click', function () {
        isCreated = false;
        newRow.remove();
    });

})