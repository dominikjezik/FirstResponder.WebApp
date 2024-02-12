<script>
export default {
    data() {
        return {
            filterInput: {
                fullName: '',
                phoneNumber: '',
            },
            filterSelect: {
                type: '',
                region: '',
            },
            page: 0,
            loading: false,
            hasMore: true,
            optionsType: [
                { id: 0, name: 'Nezaradený' },
                { id: 1, name: 'Responder' },
                { id: 2, name: 'Zamestnanec' }
            ],
            optionsRegion: [
                { id: 0, name: 'Trenčiansky kraj' }
            ],
            users: [],
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
        onItemClicked(user) {
            window.location = `/users/${user.id}`
        },
        filterChanged() {
            this.page = 0
            this.hasMore = true
            this.users = []
            this.loadMore()
        },
        loadMore() {
            if (this.loading || !this.hasMore) {
                return
            }
            
            this.loading = true
            this.isMessageVisible = true
            this.messageText = 'Načítavam ďalších používateľov...'

            fetch(this.getURL())
                .then((response) => response.json())
                .then(users => {
                    if (users.length === 0) {
                        if (this.page === 0) {
                            this.isMessageVisible = true
                            this.messageText = 'Nenašli sa žiadni používatelia.'
                        } else {
                            this.isMessageVisible = false
                        }

                        this.hasMore = false
                        return
                    }

                    this.page++

                    this.users.push(...users)
                    this.isMessageVisible = false
                })
                .catch((error) => {
                    this.isMessageVisible = true
                    this.messageText = 'Nastala chyba pri načítaní ďalších používateľov.'
                    console.log(error)
                })
                .finally(() => {
                    this.loading = false
                })
        },
        getURL() {
            let url = new URL('/api/users/filtered-table-items', window.location.href)
            url.searchParams.append('pageNumber', this.page)

            if (this.filterInput.fullName) {
                url.searchParams.append('fullName', this.filterInput.fullName)
            }

            if (this.filterInput.phoneNumber) {
                url.searchParams.append('phoneNumber', this.filterInput.phoneNumber)
            }

            if (this.filterSelect.type !== '') {
                url.searchParams.append('type', this.filterSelect.type)
            }

            if (this.filterSelect.region !== '') {
                url.searchParams.append('region', this.filterSelect.region)
            }
            
            return url
        },
    }
}
</script>

<template>
    <div class="filter-table-form">
        <div class="field">
            <label class="label">Podľa&nbsp;mena</label>
            <div class="control">
                <input v-model="filterInput.fullName" class="input" type="text">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;telefónu</label>
            <div class="control">
                <input v-model="filterInput.phoneNumber" class="input" type="text">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;stavu</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.type">
                        <option value="">-</option>
                        <option v-for="option in optionsType" :value="option.id">
                            {{ option.name}}
                        </option>
                    </select>
                </div>
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;kraju</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.region">
                        <option value="">-</option>
                        <option v-for="option in optionsRegion" :value="option.id">
                            {{ option.name}}
                        </option>
                    </select>
                </div>
            </div>
        </div>
    </div>

    <table class="table is-striped is-narrow is-hoverable is-fullwidth table-with-clickable-link">
        <thead>
        <tr>
            <th>Meno</th>
            <th>Email</th>
            <th>Telefón</th>
            <th>Evidencia</th>
            <th>Stav</th>
            <th>Kraj</th>
            <th>Adresa</th>
        </tr>
        </thead>
        <tbody>
            <tr v-for="user in users" :key="user.id" @click="() => onItemClicked(user)">
                <td>{{ user.fullName }}</td>
                <td>{{ user.email }}</td>
                <td>{{ user.phoneNumber }}</td>
                <td>{{ user.createdAt }}</td>
                <td>{{ user.type }}</td>
                <td>{{ user.region }}</td>
                <td>{{ user.address }}</td>
            </tr>
        </tbody>
    </table>

    <p v-if="isMessageVisible" class="has-text-centered">{{ messageText }}</p>
</template>
