export default {
    handleManufacturerModelSelects: function (manufacturerSelectId, modelSelectId, baseUrl) {
        const manufacturerSelect = document.getElementById(manufacturerSelectId)
        const modelSelect = document.getElementById(modelSelectId)

        manufacturerSelect.onchange = function () {
            const url = `${baseUrl}?manufacturerId=${manufacturerSelect.value}`

            fetch(url)
                .then(response => response.json())
                .then(models => {
                    modelSelect.innerHTML = ''

                    const defaultOption = document.createElement('option')
                    defaultOption.value = ''
                    defaultOption.textContent = '-'
                    modelSelect.appendChild(defaultOption)

                    models.forEach(model => {
                        const option = document.createElement('option')
                        option.value = model.id
                        option.textContent = model.name
                        modelSelect.appendChild(option)
                    })
                })
        }
    }
}
