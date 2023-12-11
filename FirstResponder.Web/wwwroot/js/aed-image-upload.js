const aedPhotoFormFile = document.getElementById('AedPhotoFormFile')

aedPhotoFormFile.onchange = event => {
    const uploadedImagesPreviewRoot = document.getElementById('uploaded-images-preview-root')
    const file = event.target.files[0]
    if (file) {
        uploadedImagesPreviewRoot.innerHTML = ''
        const card = document.createElement('div')
        card.classList.add('card')

        const cardImage = document.createElement('div')
        cardImage.classList.add('card-image')

        const figure = document.createElement('figure')
        figure.classList.add('image')

        const img = document.createElement('img')
        img.src = URL.createObjectURL(file)

        const footer = document.createElement('footer')
        footer.classList.add('card-footer')

        const footerItem = document.createElement('a')
        footerItem.classList.add('card-footer-item')
        footerItem.innerText = 'Odstrániť'

        figure.appendChild(img)
        cardImage.appendChild(figure)
        card.appendChild(cardImage)
        uploadedImagesPreviewRoot.appendChild(card)
        footer.appendChild(footerItem)
        card.appendChild(footer)

        const uploadPhotoBtn = document.getElementById('upload-photo-btn')
        uploadPhotoBtn.innerText = 'Zmeniť fotografiu'


        footerItem.onclick = () => {
            uploadedImagesPreviewRoot.innerHTML = 'Neboli nahraté žiadne fotografie.'
            aedPhotoFormFile.value = ''
            uploadPhotoBtn.innerText = 'Pridať fotografiu'
        }

    }
}

// Edit AED, uprava existujucich obrazkov
const removePhotoInputs = document.getElementById('remove-photo-inputs')
const removeImageBtns = document.getElementsByClassName('btn-remove-current-image')

for (let i = 0; i < removeImageBtns.length; i++) {
    removeImageBtns[i].onclick = event => {
        const card = event.target.parentElement.parentElement
        card.remove()
        
        const aedPhotoId = event.target.dataset.aedphotoid
        if (aedPhotoId) {
            removePhotoInputs.innerHTML += `<input type="hidden" name="AedPhotosToDelete" value="${aedPhotoId}" />`
        }
    }
}



