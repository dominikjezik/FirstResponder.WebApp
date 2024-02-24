<script>
export default {
    data() {
        return {
            filterInput: {
                name: '',
            },
            filterSelect: {
                from: '',
                to: '',
                typeId: '',
            },
            page: 0,
            loading: false,
            hasMore: true,
            optionsTypes: [],
            items: [],
            isMessageVisible: false,
            messageText: '',
            debounceTimer: null,
        }
    },
    mounted() {
        // Load course types
        fetch('/courses/types')
            .then((response) => response.json())
            .then(types => {
                this.optionsTypes = types
            })
            .catch((error) => {
                console.log(error)
            })

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
        onItemClicked(item) {
            window.location = `/courses/${item.id}`
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
                    console.log(items)
                    
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
                        item.startDate = new Date(item.startDate).toLocaleString('sk-SK', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' })
                        item.endDate = new Date(item.endDate).toLocaleString('sk-SK', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' })
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
            let url = new URL('/courses/filtered-table-items', window.location.href)
            url.searchParams.append('pageNumber', this.page)

            if (this.filterSelect.from !== '') {
                url.searchParams.append('from', this.filterSelect.from)
            }

            if (this.filterSelect.to !== '') {
                url.searchParams.append('to', this.filterSelect.to)
            }

            if (this.filterInput.name !== '') {
                url.searchParams.append('name', this.filterInput.name)
            }

            if (this.filterSelect.typeId !== '') {
                url.searchParams.append('typeId', this.filterSelect.typeId)
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
            <label class="label">Podľa&nbsp;názvu</label>
            <div class="control">
                <input v-model="filterInput.name" class="input" type="text">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;typu</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.typeId">
                        <option value="">-</option>
                        <option v-for="option in optionsTypes" :value="option.id">
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
            <th>Od</th>
            <th>Do</th>
            <th>Názov</th>
            <th>Školiteľ</th>
            <th>Typ</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="item in items" :key="item.id" @click="() => onItemClicked(item)">
            <td>{{ item.startDate }}</td>
            <td>{{ item.endDate }}</td>
            <td>{{ item.name }}</td>
            <td>{{ item.trainer }}</td>
            <td>{{ item.courseType?.name }}</td>
        </tr>
        </tbody>
    </table>

    <p v-if="isMessageVisible" class="has-text-centered">{{ messageText }}</p>

</template>
