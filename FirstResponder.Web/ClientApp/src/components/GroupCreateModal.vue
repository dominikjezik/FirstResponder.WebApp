<script>
export default {
    data() {
        return {
            isShown: false
        }
    },
    methods: {
        closeModal() {
            this.isShown = false
        }
    },
    mounted() {
        window.addEventListener('display-create-modal', (e) => {
            this.isShown = true
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
            <form action="/groups/create" method="post">
                <slot name="anti-forgery"></slot>
                <header class="modal-card-head">
                    <p class="modal-card-title" style="margin-bottom: 0">Pridať skupinu</p>
                    <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    <div class="field">
                        <label class="label">Skupina *</label>
                        <div class="control">
                            <input name="Name" class="input" type="text" required>
                        </div>
                    </div>
                    <div class="field">
                        <label class="label">Popis</label>
                        <div class="control">
                            <textarea name="Description" class="textarea"></textarea>
                        </div>
                    </div>
                </section>
                <footer class="modal-card-foot">
                    <button type="submit" class="button is-link" id="btn-submit-create">Vytvoriť</button>
                    <button @click="closeModal" type="button" class="button">Zrušiť</button>
                </footer>
            </form>
        </div>
    </div>
</template>