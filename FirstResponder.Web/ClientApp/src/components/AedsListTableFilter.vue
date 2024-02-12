<script>
export default {
    data() {
        return {
            filterInput: {
                holder: '',
                serialNumber: '',
            },
            filterSelect: {
                type: 0,
                region: '',
                state: '',
                manufacturerId: '',
                modelId: '',
            },
            page: 0,
            loading: false,
            hasMore: true,
            optionsState: [
                { id: 0, name: 'Registrovaný' },
                { id: 1, name: 'Pripravený' },
                { id: 2, name: 'Nepripravený' },
                { id: 3, name: 'Mimo prevádzky' },
                { id: 4, name: 'Servisný zásah' },
                { id: 5, name: 'Zrušený' }
            ],
            optionsRegion: [
                { id: 0, name: 'Trenčiansky kraj' }
            ],
            optionsManufacturer: [],
            optionsModel: [],
            items: [],
            isMessageVisible: false,
            messageText: '',
            debounceTimer: null,
        }
    },
    mounted() {
        // Load manufacturers
        fetch('/aed/manufacturers')
            .then((response) => response.json())
            .then(manufacturers => {
                this.optionsManufacturer = manufacturers
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
        "filterSelect.manufacturerId"() {
            this.filterSelect.modelId = ''
            this.optionsModel = []

            if (this.filterSelect.manufacturerId === '') {
                return
            }

            fetch(`/aed/models?manufacturerId=${this.filterSelect.manufacturerId}`)
                .then((response) => response.json())
                .then(models => {
                    this.optionsModel = models
                })
                .catch((error) => {
                    console.log(error)
                })
        }
    },
    methods: {
        onItemClicked(item) {
            window.location = `/aed/${item.id}`
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
            let url = new URL('/aed/filtered-table-items', window.location.href)
            url.searchParams.append('pageNumber', this.page)
            url.searchParams.append('type', this.filterSelect.type)
            
            if (this.filterInput.holder !== '') {
                url.searchParams.append('holder', this.filterInput.holder)
            }
            
            if (this.filterSelect.region !== '') {
                url.searchParams.append('region', this.filterSelect.region)
            }
            
            if (this.filterSelect.state !== '') {
                url.searchParams.append('state', this.filterSelect.state)
            }
            
            if (this.filterSelect.manufacturerId !== '') {
                url.searchParams.append('manufacturerId', this.filterSelect.manufacturerId)
            }
            
            if (this.filterSelect.modelId !== '') {
                url.searchParams.append('modelId', this.filterSelect.modelId)
            }
            
            if (this.filterInput.serialNumber !== '') {
                url.searchParams.append('serialNumber', this.filterInput.serialNumber)
            }

            return url
        },
    }
}
</script>

<template>
    <div class="filter-table-form">
        <div class="field">
            <label class="label">Typ</label>
            <div class="control">
                <label style="margin-right: .5rem;">
                    <input v-model="filterSelect.type" value="0" name="type" type="radio"> Verejné
                </label>
                <label>
                    <input v-model="filterSelect.type" value="1" name="type" type="radio"> Osobné
                </label>
            </div>
        </div>
        
        <div class="field">
            <label class="label">Podľa&nbsp;držateľa</label>
            <div class="control">
                <input v-model="filterInput.holder" class="input" type="text">
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;kraju</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.region">
                        <option value="">-</option>
                        <option v-for="option in optionsRegion" :value="option.id">
                            {{ option.name }}
                        </option>
                    </select>
                </div>
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

        <div class="field">
            <label class="label">Podľa&nbsp;výrobcu</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.manufacturerId">
                        <option value="">-</option>
                        <option v-for="option in optionsManufacturer" :value="option.id">
                            {{ option.name }}
                        </option>
                    </select>
                </div>
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;modelu</label>
            <div class="control">
                <div class="select">
                    <select v-model="filterSelect.modelId">
                        <option value="">-</option>
                        <option v-for="option in optionsModel" :value="option.id">
                            {{ option.name }}
                        </option>
                    </select>
                </div>
            </div>
        </div>

        <div class="field">
            <label class="label">Podľa&nbsp;sériového&nbsp;čísla</label>
            <div class="control">
                <input v-model="filterInput.serialNumber" class="input" type="text">
            </div>
        </div>
    </div>

    <table class="table is-striped is-narrow is-hoverable is-fullwidth table-with-clickable-link">
        <thead>
        <tr>
            <th></th>
            <th>Registrácia</th>
            <th>Držateľ</th>
            <th>Sériové číslo</th>
            <th>Výrobca</th>
            <th>Stav</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="item in items" :key="item.id" @click="() => onItemClicked(item)">
            <td class="table-icon">
                <svg :class="'aed-icon-' + item.state" width="16" height="16" viewBox="0 0 500 500" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" clip-rule="evenodd" d="M3.79832 201.333C-17.2805 98.4708 52.7101 20.734 131.257 20.0131C171.625 19.3941 222.121 40.7359 249.035 92.2912C275.948 40.7359 326.444 19.3941 366.813 20.0131C445.36 20.734 515.35 98.4708 494.271 201.333C478.203 279.744 394.702 381.713 249.035 480.552C103.368 381.713 19.8668 279.744 3.79832 201.333ZM164.031 331.283L190.768 48.4749C209.15 59.0772 226.112 75.261 238.016 98.0647L244.335 110.173L214.174 250.673L327.999 168.283L277.857 395.135H307.363L248.319 460.766L213.116 378.72L242.623 389.081L258.995 262.813L164.031 331.283Z" fill="currentColor"/>
                </svg>
            </td>
            <td>{{ item.createdAt }}</td>
            <td>{{ item.holder }}</td>
            <td>{{ item.serialNumber }}</td>
            <td>{{ item.manufacturer }}</td>
            <td>{{ item.displayState }}</td>
        </tr>
        </tbody>
    </table>

    <p v-if="isMessageVisible" class="has-text-centered">{{ messageText }}</p>
    
</template>
