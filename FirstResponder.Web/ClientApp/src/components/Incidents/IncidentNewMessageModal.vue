<script>
export default {
    props: {
        incidentId: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            isShown: false,
            message: '',
            errorMessage: ''
        }
    },
    methods: {
        openModal() {
            this.isShown = true
        },
        closeModal() {
            this.isShown = false
            this.message = ''
        },
        submitForm() {
            fetch(`/incidents/${this.incidentId}/messages`, {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    messageContent: this.message
                })
            })
            .then(response => {
                if (response.ok) {
                    return response.json()
                }
                this.errorMessage = 'Nastala chyba pri odosielaní správy.'
            })
            .then(message => {
                console.log(message)
                this.$emit('message-sent', message)
                this.closeModal()
            })
            .catch(error => {
                console.log(error)
                this.errorMessage = 'Nastala chyba pri odosielaní správy.'
            })
        }
    },
    mounted() {
        document.addEventListener('keydown', (event) => {
            if (event.code === 'Escape') {
                this.closeModal();
            }
        });
    }
}
</script>

<template>
    <div class="modal" :class="{'is-active': isShown}">
        <div class="modal-background" @click="closeModal"></div>
        <div class="modal-card">
            <form method="post" @submit.prevent="submitForm">
                <slot name="anti-forgery"></slot>
                <header class="modal-card-head">
                    <p class="modal-card-title" style="margin-bottom: 0">Odoslať novú správu</p>
                    <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    <div class="message is-danger" v-if="errorMessage !== ''">
                        <div class="message-body mb-1">{{ errorMessage }}</div>
                    </div>
                    <div class="field">
                        <label class="label">Správa</label>
                        <div class="control">
                            <textarea v-model="message" name="Description" class="textarea"></textarea>
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot">
                    <button type="submit" class="button is-link" id="btn-submit-create">Odoslať</button>
                    <button @click="closeModal" type="button" class="button">Zrušiť</button>
                </footer>
            </form>
        </div>
    </div>
</template>