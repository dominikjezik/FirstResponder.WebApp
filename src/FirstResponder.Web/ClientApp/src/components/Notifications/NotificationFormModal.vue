<script>
export default {
    data() {
        return {
            isShown: false,
            notificationId: null,
            content: ''
        }
    },
    methods: {
        closeModal() {
            this.isShown = false
        },
        deleteNotification() {
            if (confirm('Naozaj chcete odstrániť túto notifikáciu?')) {
                document.getElementById('formDelete').submit();
            }
        }
    },
    mounted() {
        window.addEventListener('display-edit-modal', (e) => {
            this.isShown = true
            if (e.detail.notificationId) {
                this.notificationId = e.detail.notificationId
                this.content = e.detail.content
            } else {
                this.notificationId = null
                this.content = ''
            }
        })

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
            <form :action="`/users/notifications/${this.notificationId === null ? 'create' : 'update'}`" method="post">
                <slot name="anti-forgery"></slot>
                <header class="modal-card-head">
                    <p class="modal-card-title" style="margin-bottom: 0">{{this.notificationId === null ? 'Vytvoriť' : 'Upraviť'}} notifikáciu</p>
                    <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    <input type="hidden" name="NotificationId" :value="notificationId" />
                    <div class="field">
                        <label class="label">Text</label>
                        <div class="control">
                            <textarea name="Content" class="textarea" required v-model="content"></textarea>
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot flex-space-between">
                    <div class="control">
                        <button type="submit" class="button is-link" id="btn-submit-create">
                            {{this.notificationId === null ? 'Vytvoriť' : 'Upraviť'}}
                        </button>
                        <button @click="closeModal" type="button" class="button">Zrušiť</button>
                    </div>
                    
                    <button @click.prevent="deleteNotification" v-if="notificationId !== null" type="submit" class="button is-danger is-light">Odstrániť notifikáciu</button>
                </footer>
            </form>
            <form action="/users/notifications/delete" method="post" id="formDelete">
                <slot name="anti-forgery"></slot>
                <input type="hidden" name="NotificationId" :value="notificationId" />
            </form>
        </div>
    </div>
</template>