<script>
export default {
    props: {
        updateItemFormAction: {
            type: String,
            required: true
        },
        deleteItemFormAction: {
            type: String,
            required: true
        },
        itemName: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            isEditing: false,
            isVisible: true
        }
    },
    methods: {
        focusInput() {
            this.isEditing = true
        },
        blurInput(e) {
            if (e.relatedTarget && e.relatedTarget.classList.contains('save-edit-btn')) {
                return
            }
            
            this.isEditing = false
        },
        submitDeleteForm() {
            if (confirm('Naozaj chcete odstrániť tento záznam?')) {
                this.$refs.deleteForm.submit()
            }
        }
    },
    mounted() {
        window.addEventListener('search-input', (e) => {
            let query = e.detail.query.toLowerCase()
            this.isVisible = this.itemName.toLowerCase().includes(query);
        });
    }
}
</script>

<template>
    <tr class="result-item" v-if="isVisible">
        <td>
            <form :action="updateItemFormAction" method="post" class="in-place-edit-form">
                <slot name="anti-forgery"></slot>
                <input @focus="focusInput" @blur="blurInput" type="text" name="Name" :value="itemName" required class="input input-edit">
                <button type="submit" class="save-edit-btn button button-with-icon is-success is-small" v-show="isEditing">
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
            <form :action="deleteItemFormAction" method="post" @submit.prevent="submitDeleteForm" ref="deleteForm">
                <slot name="anti-forgery"></slot>
                <button type="submit" class="button button-with-icon is-danger is-small">
                        <span class="icon">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3" viewBox="0 0 16 16">
                                <path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5M11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H2.506a.58.58 0 0 0-.01 0H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1h-.995a.59.59 0 0 0-.01 0zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47ZM8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5"/>
                            </svg>
                        </span>
                    <span>Odstrániť</span>
                </button>
            </form>
        </td>
    </tr>
</template>
