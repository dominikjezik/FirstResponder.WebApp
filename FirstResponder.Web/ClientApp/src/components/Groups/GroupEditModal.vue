<script>
export default {
    data() {
        return {
            isShown: false,
            group: {
                id: null,
                name: '',
                description: ''
            }
        }
    },
    methods: {
        closeModal() {
            this.isShown = false
        },
        deleteGroup() {
            if (confirm('Naozaj chcete odstrániť túto skupinu?')) {
                document.getElementById('formDelete').submit();
            }
        }
    },
    mounted() {
        window.addEventListener('display-edit-modal', (e) => {
            this.isShown = true
            this.group.id = e.detail.groupId
            this.group.name = e.detail.name
            this.group.description = e.detail.description
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
            <form action="/groups/update" method="post">
                <slot name="anti-forgery"></slot>
                <input type="hidden" name="GroupId" v-model="group.id">
                <header class="modal-card-head">
                    <p class="modal-card-title" style="margin-bottom: 0">Upraviť skupinu</p>
                    <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    <div class="field">
                        <label class="label">Skupina *</label>
                        <div class="control">
                            <input name="Name" v-model="group.name" class="input" type="text" required>
                        </div>
                    </div>
                    <div class="field">
                        <label class="label">Popis</label>
                        <div class="control">
                            <textarea name="Description" v-model="group.description" class="textarea"></textarea>
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot flex-space-between">
                    <div class="control">
                        <button type="submit" class="button is-link" id="btn-submit-update">Aktualizovať</button>
                        <button @click="closeModal" type="button" class="button">Zrušiť</button>
                    </div>

                    <button @click.prevent="deleteGroup" type="submit" class="button is-danger is-light">Odstrániť skupinu</button>
                </footer>
            </form>
            <form action="/groups/delete" method="post" id="formDelete">
                <slot name="anti-forgery"></slot>
                <input type="hidden" name="GroupId" v-model="group.id" />
            </form>
        </div>
    </div>
</template>
