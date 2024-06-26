<script>
export default {
    data() {
        return {
            filterInput: {
                patient: '',
                address: '',
            },
            filterSelect: {
                from: '',
                to: '',
                state: ''
            },
            page: 0,
            loading: false,
            hasMore: true,
            optionsState: [
                { id: 0, name: 'Vytvorený' },
                { id: 1, name: 'Prebiehajúci' },
                { id: 2, name: 'Ukončený' },
                { id: 3, name: 'Zrušený' }
            ],
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
        }
    },
    methods: {
        onItemClicked(item) {
            window.location = `/incidents/${item.id}`
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
                        item.createdAt = new Date(item.createdAt).toLocaleString('sk-SK', {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        })
                        
                        if (item.resolvedAt !== null) {
                            item.resolvedAt = new Date(item.resolvedAt).toLocaleString('sk-SK', {
                                year: 'numeric',
                                month: '2-digit',
                                day: '2-digit',
                                hour: '2-digit',
                                minute: '2-digit'
                            })
                        }
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
            let url = new URL('/incidents/filtered-table-items', window.location.href)
            url.searchParams.append('pageNumber', this.page)
            url.searchParams.append('pageSize', 30)
            
            if (this.filterInput.patient !== '') {
                url.searchParams.append('patient', this.filterInput.patient)
            }
            
            if (this.filterInput.address !== '') {
                url.searchParams.append('address', this.filterInput.address)
            }

            if (this.filterSelect.from !== '') {
                url.searchParams.append('from', this.filterSelect.from)
            }
            
            if (this.filterSelect.to !== '') {
                url.searchParams.append('to', this.filterSelect.to)
            }
            
            if (this.filterSelect.state !== '') {
                url.searchParams.append('state', this.filterSelect.state)
            }

            return url
        },
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
            <label class="label">Podľa&nbsp;adresy</label>
            <div class="control">
                <input v-model="filterInput.address" class="input" type="text">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;pacienta</label>
            <div class="control">
                <input v-model="filterInput.patient" class="input" type="text">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;stavu</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.state">
                        <option value="">-</option>
                        <option v-for="option in optionsState" :value="option.id">
                            {{ option.name }}
                        </option>
                    </select>
                </div>
            </div>
        </div>
    </div>

    <table class="table is-striped is-narrow is-hoverable is-fullwidth table-with-clickable-link">
        <thead>
        <tr>
            <th>Začiatok</th>
            <th>Koniec</th>
            <th>Adresa</th>
            <th>Pacient</th>
            <th>Diagnóza</th>
            <th>Stav</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="item in items" :key="item.id" @click="() => onItemClicked(item)">
            <td>{{ item.createdAt }}</td>
            <td>{{ item.resolvedAt }}</td>
            <td>{{ item.address }}</td>
            <td>{{ item.patient }}</td>
            <td>{{ item.diagnosis }}</td>
            <td>{{ item.displayState }}</td>
        </tr>
        </tbody>
    </table>

    <p v-if="isMessageVisible" class="has-text-centered">{{ messageText }}</p>

</template>
