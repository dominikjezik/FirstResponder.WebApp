<script>
    export default {
        props: {
            type: {
                type: String,
                default: '',
                validator: value => ['', 'dark', 'primary', 'link', 'info', 'success', 'warning', 'danger'].includes(value)
            }
        },
        mounted() {
            console.log(this.$el)
            
            const deleteButton = this.$el.querySelector('.delete')
            deleteButton.addEventListener('click', () => {
                this.$el.remove()
            });
        }
    }
</script>

<template>
    <div class="message message-closeable" :class="`is-${this.type}`">
        <div class="message-body mb-1">
            <p><slot></slot></p>
            <button class="delete" aria-label="delete"></button>
        </div>
    </div>
</template>
