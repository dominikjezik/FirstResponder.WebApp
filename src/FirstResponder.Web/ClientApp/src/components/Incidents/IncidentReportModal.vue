<script>
export default {
    props: {
        incidentId: {
            type: String,
            required: true
        },
    },
    data() {
        return {
            isShown: false,
            responderId: '',
            data: {
                responder: '',
                submittedAt: '',
                incidentReportForm: {
                    aedUsed: false,
                    aedShocks: 0,
                    details: ''
                }
            }
        }
    },
    methods: {
        openModal(responderId) {
            this.responderId = responderId
            this.loadReport()
            this.isShown = true
        },
        closeModal() {
            this.isShown = false
        },
        loadReport() {
            fetch(`/incidents/${this.incidentId}/reports/${this.responderId}`)
                .then(response => response.json())
                .then(data => {
                    this.data = data
                })
                .catch(error => {
                    console.log(error)
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
            <header class="modal-card-head">
                <p class="modal-card-title" style="margin-bottom: 0">Vyhodnotenie zásahu</p>
                <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
            </header>
            <section class="modal-card-body">
                <div class="field">
                    <label class="label">Responder</label>
                    <div class="control">
                        <input v-model="data.responder" name="Name" class="input" type="text" readonly>
                    </div>
                </div>
                <div class="field">
                    <label class="label">Vyhodnotené</label>
                    <div class="control">
                        <input v-model="data.submittedAt" name="Name" class="input" type="text" readonly>
                    </div>
                </div>
                <div class="field">
                    <div class="control">
                        <label class="checkbox">
                            <input v-model="data.incidentReportForm.aedUsed" type="checkbox" onclick="return false;">
                            Bolo použité AED?
                        </label>
                    </div>
                </div>
                <div class="field">
                    <label class="label">Počet výbojov</label>
                    <div class="control">
                        <input v-model="data.incidentReportForm.aedShocks" name="Name" class="input" type="text" readonly>
                    </div>
                </div>
                <div class="field">
                    <label class="label">Poznámka</label>
                    <div class="control">
                        <textarea v-model="data.incidentReportForm.details" name="Details" class="textarea" readonly></textarea>
                    </div>
                </div>
            </section>
            <footer class="modal-card-foot">
                <button @click="closeModal" type="button" class="button">Zavrieť</button>
            </footer>
        </div>
    </div>
</template>