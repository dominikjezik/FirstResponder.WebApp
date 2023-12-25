<script>
export default {
    props: {
        newItemFormAction: {
            type: String,
            required: true
        },
        itemPlaceholder: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            isEditing: false,
            isVisible: false,
            inputValue: this.itemPlaceholder
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
        removeItem() {
            this.isVisible = false
            this.inputValue = this.itemPlaceholder
        }
    },
    mounted() {
        window.addEventListener('create-new-item', () => {
            this.isVisible = true
            this.$nextTick(() => {
                this.$refs.input.select()
            })
        })
    }
}
</script>

<template>
    <tr class="result-item" v-if="isVisible">
        <td>
            <form :action="newItemFormAction" method="post" class="in-place-edit-form">
                <slot name="anti-forgery"></slot>
                <input @focus="focusInput" @blur="blurInput" ref="input" type="text" name="Name" required class="input input-edit" v-model="inputValue" id="new-item-input">
                <button type="submit" class="save-edit-btn button button-with-icon is-success is-small">
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
            <button @click="removeItem" class="button button-with-icon is-warning is-small">
                    <span class="icon">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                            <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708"/>
                        </svg>
                    </span>
                <span>Zrušiť</span>
            </button>
        </td>
    </tr>
</template>
