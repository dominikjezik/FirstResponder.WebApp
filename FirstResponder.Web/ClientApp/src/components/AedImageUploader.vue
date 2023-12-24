<script>
export default {
    props: {
        inputElementId: {
            type: String,
            required: true
        },
        currentPhotosProp: {
            type: Array,
            required: false,
            default: []
        },
        removePhotoInputsId: {
            type: String,
            required: false
        }
    },
    data() {
        return {
            uploadedPhotos: [],
            currentPhotos: this.currentPhotosProp
        }
    },
    methods: {
        openFileExplorer() {
            document.getElementById(this.inputElementId).click();
        },
        removeUploadedPhoto(index) {
            this.uploadedPhotos.splice(index, 1);

            const input = document.getElementById(this.inputElementId);
            let newFiles = new DataTransfer();
            
            for (let i = 0; i < input.files.length; i++) {
                if (i !== index) {
                    newFiles.items.add(input.files[i]);
                }
            }
            
            input.files = newFiles.files;
        },
        removeCurrentPhoto(index) {
            if (this.removePhotoInputsId) {
                const div = document.getElementById(this.removePhotoInputsId);
                const input = document.createElement('input');
                input.type = 'hidden';
                input.name = 'AedPhotosToDelete';
                input.value = this.currentPhotos[index].id;
                div.appendChild(input);

                this.currentPhotos.splice(index, 1);
            }
        }
    },
    mounted() {
        // inputElementId nastav že keď nastane onchange potom ziskaš všetky nahraté fotky a zobrazíš ich cez uploadedPhotos
        document.getElementById(this.inputElementId).onchange = (e) => {
            const files = e.target.files;
            if (files.length > 0) {
                for (let i = 0; i < files.length; i++) {
                    const file = files[i]
                    this.uploadedPhotos.push(URL.createObjectURL(file));
                }
            }
        }
    }
}
</script>

<template>
    <header class="section-header">
        <h2 class="heading-with-icon">
            <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" class="bi bi-images" viewBox="0 0 16 16">
                <path d="M4.502 9a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3z"/>
                <path d="M14.002 13a2 2 0 0 1-2 2h-10a2 2 0 0 1-2-2V5A2 2 0 0 1 2 3a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v8a2 2 0 0 1-1.998 2zM14 2H4a1 1 0 0 0-1 1h9.002a2 2 0 0 1 2 2v7A1 1 0 0 0 15 11V3a1 1 0 0 0-1-1zM2.002 4a1 1 0 0 0-1 1v8l2.646-2.354a.5.5 0 0 1 .63-.062l2.66 1.773 3.71-3.71a.5.5 0 0 1 .577-.094l1.777 1.947V5a1 1 0 0 0-1-1h-10z"/>
            </svg>
            Fotografie
        </h2>

        <button class="button" @click="openFileExplorer">
            Pridať fotografiu
        </button>
    </header>

    <div class="mb-5">
        <div v-if="uploadedPhotos.length === 0 && currentPhotos.length === 0">
            Neboli nahraté žiadne fotografie.
        </div>
        <div class="photos-preview">
            <div v-for="(photo, index) in uploadedPhotos" class="card">
                <div class="card-image">
                    <figure class="image">
                        <img :src="photo" alt="Fotografia AED">
                    </figure>
                </div>
                <div class="card-footer">
                    <a @click="() => removeUploadedPhoto(index)" class="card-footer-item btn-remove-current-image">Odstrániť</a>
                </div>
            </div>
        </div>
        <div class="photos-preview">
            <div v-for="(photo, index) in currentPhotos" class="card">
                <div class="card-image">
                    <figure class="image">
                        <img :src="`/uploads/${photo.photoName}`" alt="Fotografia AED">
                    </figure>
                </div>
                <div class="card-footer">
                    <a @click="() => removeCurrentPhoto(index)" class="card-footer-item btn-remove-current-image">Odstrániť</a>
                </div>
            </div>
        </div>
    </div>
</template>
