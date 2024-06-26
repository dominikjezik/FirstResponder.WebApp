<script>
export default {
    data() {
        return {
            filterInput: {
                content: '',
            },
            filterSelect: {
                from: '',
                to: ''
            },
            page: 0,
            loading: false,
            hasMore: true,
            items: [],
            isMessageVisible: false,
            messageText: '',
            debounceTimer: null,
        }
    },
    mounted() {
        // Load first page
        this.loadMore()

        // On scroll, check if we’re at the bottom of the page
        window.onscroll = () => {
            if (this.hasMore && window.innerHeight + window.scrollY >= document.body.offsetHeight) {
                this.loadMore()
            }
        }
    },
    watch: {
        filterSelect: {
            handler() {
                clearTimeout(this.debounceTimer)
                this.filterChanged()
            },
            deep: true
        },
        filterInput: {
            handler() {
                clearTimeout(this.debounceTimer)
                this.debounceTimer = setTimeout(this.filterChanged, 300)
            },
            deep: true
        },
    },
    methods: {
        onItemClicked(item, e) {
            if (e.target.classList.contains('btn-send-notification')) {
                if (confirm('Naozaj chcete odoslať túto notifikáciu zadaným príjemcom?')) {
                    this.$refs.notificationId.value = item.id
                    this.$refs.sendNotificationForm.submit()
                }
                
                return
            }
            
            if (e.target.classList.contains('btn-users-list')) {
                window.dispatchEvent(new CustomEvent('display-users-list-modal', { detail: {
                    entityId: item.id
                }}))
                
                return
            }
            
            if (e.target.classList.contains('btn-add-group')) {
                window.dispatchEvent(new CustomEvent('display-add-from-group-modal', { detail: {
                    entityId: item.id
                }}))
                
                return
            }
            
            window.dispatchEvent(new CustomEvent('display-edit-modal', { detail: {
                notificationId: item.id,
                content: item.content
            }}))
        },
        filterChanged() {
            this.page = 0
            this.hasMore = true
            this.items = []
            this.loadMore()
        },
        loadMore() {
            if (this.loading || !this.hasMore) {
                return
            }

            this.loading = true
            this.isMessageVisible = true
            this.messageText = 'Načítavam ďalšie položky...'

            fetch(this.getURL())
                .then((response) => response.json())
                .then(items => {
                    if (items.length === 0) {
                        if (this.page === 0) {
                            this.isMessageVisible = true
                            this.messageText = 'Nenašli sa žiadne položky.'
                        } else {
                            this.isMessageVisible = false
                        }

                        this.hasMore = false
                        return
                    }
                    
                    items.forEach(item => {
                        item.createdAt = new Date(item.createdAt).toLocaleString('sk-SK', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' })
                    })

                    this.page++

                    this.items.push(...items)
                    this.isMessageVisible = false
                })
                .catch((error) => {
                    this.isMessageVisible = true
                    this.messageText = 'Nastala chyba pri načítaní ďalších položiek.'
                    console.log(error)
                })
                .finally(() => {
                    this.loading = false
                })
        },
        getURL() {
            let url = new URL('/users/notifications/filtered-table-items', window.location.href)
            url.searchParams.append('pageNumber', this.page)

            if (this.filterSelect.from !== '') {
                url.searchParams.append('from', this.filterSelect.from)
            }

            if (this.filterSelect.to !== '') {
                url.searchParams.append('to', this.filterSelect.to)
            }

            if (this.filterInput.content !== '') {
                url.searchParams.append('content', this.filterInput.content)
            }

            return url
        },
        maxLength(value, max) {
            return value.length > max ? value.substring(0, max) + '...' : value
        }
    }
}
</script>

<template>
    <div class="filter-table-form">
        <div class="field">
            <label class="label">Od</label>
            <div class="control">
                <input v-model="filterSelect.from" class="input" type="datetime-local">
            </div>
        </div>

        <div class="field">
            <label class="label">Do</label>
            <div class="control">
                <input v-model="filterSelect.to" class="input" type="datetime-local">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa obsahu</label>
            <div class="control">
                <input v-model="filterInput.content" class="input" type="text">
            </div>
        </div>
    </div>

    <table class="table is-striped is-narrow is-hoverable is-fullwidth table-with-clickable-link">
        <thead>
        <tr>
            <th>Vytvorená</th>
            <th>Odosielateľ</th>
            <th>Obsah</th>
            <th style="width: 0"></th>
        </tr>
        </thead>
        <tbody id="table-body">
        <tr v-for="item in items" :key="item.id" @click="(e) => onItemClicked(item, e)">
            <td>{{ item.createdAt }}</td>
            <td>{{ item.senderName }}</td>
            <td>{{ maxLength(item.content, 70) }}</td>
            <td>
                <a class="btn-add-group" style="margin-right: 1rem;">Pridať&nbsp;skupinu</a>
                <a class="btn-users-list" style="margin-right: 1rem;">Príjemcovia</a>
                <a class="btn-send-notification has-text-success">Odoslať</a>
            </td>
        </tr>
        </tbody>
    </table>

    <form action="/users/notifications/send" method="post" ref="sendNotificationForm">
        <slot name="anti-forgery"></slot>
        <input type="hidden" name="NotificationId" ref="notificationId">
    </form>

    <p v-if="isMessageVisible" class="has-text-centered">{{ messageText }}</p>
</template>
