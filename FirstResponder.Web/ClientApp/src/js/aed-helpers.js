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
    },
    displayCorrectAvailabilityFields: function() {
        const selectAvailabilityType = document.getElementById('selectAvailabilityType')

        const displayCorrectAvailabilityFieldsCallback = () => {
            const selectedValue = selectAvailabilityType.value
            if (selectedValue === '1') {
                document.getElementById('divAvailabilityTime').style.display = 'block'
                document.getElementById('divAvailabilityDays').style.display = 'block'
                document.getElementById('divAvailabilityDate').style.display = 'none'
            } else if (selectedValue === '2') {
                document.getElementById('divAvailabilityTime').style.display = 'none'
                document.getElementById('divAvailabilityDays').style.display = 'none'
                document.getElementById('divAvailabilityDate').style.display = 'block'
            } else {
                document.getElementById('divAvailabilityTime').style.display = 'none'
                document.getElementById('divAvailabilityDays').style.display = 'none'
                document.getElementById('divAvailabilityDate').style.display = 'none'
            }
        }

        selectAvailabilityType.addEventListener('change', displayCorrectAvailabilityFieldsCallback)
        displayCorrectAvailabilityFieldsCallback()
    }
}
