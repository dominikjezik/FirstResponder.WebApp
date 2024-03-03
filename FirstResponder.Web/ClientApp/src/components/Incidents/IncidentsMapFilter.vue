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
            filterSelectTechnicalDetails: {
                manufacturerId: '',
                modelId: '',
            },
            loading: false,
            optionsState: [
                { id: 0, name: 'Vytvorený' },
                { id: 1, name: 'Prebiehajúci' },
                { id: 2, name: 'Ukončený' },
                { id: 3, name: 'Zrušený' }
            ],
            items: [],
            markers: [],
            debounceTimer: null,
        }
    },
    mounted() {
        let now = new Date()
        
        // Get day of the week, 1 is Monday, 2 is Tuesday, ..., 7 is Sunday
        let currentDay = now.getDay() || 7
        
        let firstDayOfWeek = new Date(now.getFullYear(), now.getMonth(), now.getDate() - currentDay + 1)
        let firstDayOfNextWeek = new Date(now.getFullYear(), now.getMonth(), now.getDate() - currentDay + 8)
        
        // .slice is used to remove seconds and milliseconds from the date
        this.filterSelect.from = new Date(firstDayOfWeek.getTime() - (firstDayOfWeek.getTimezoneOffset() * 60000)).toISOString().slice(0, -8)
        this.filterSelect.to = new Date(firstDayOfNextWeek.getTime() - (firstDayOfNextWeek.getTimezoneOffset() * 60000)).toISOString().slice(0, -8)
        
        this.loadItems()
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
            this.loadItems()
        },
        loadItems() {
            if (this.loading) {
                return
            }

            this.loading = true
            
            fetch(this.getURL())
                .then((response) => response.json())
                .then(items => {
                    this.items = items
                    
                    this.markers = []

                    this.markers = this.items
                        .map(item => {
                            return {
                                lat: item.latitude,
                                lon: item.longitude,
                                icon: `incident-icon-${item.state}`,
                                onClickUrl: `/incidents/${item.id}`
                            }
                        })
                })
                .catch((error) => {
                    console.log(error)
                })
                .finally(() => {
                    this.loading = false
                })
        },
        getURL() {
            let url = new URL('/incidents/filtered-table-items', window.location.href)
            url.searchParams.append('pageNumber', 0)

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
    
    <map-with-markers
        @click-item="onItemClicked"
        style="height: 100%;"
        :markers="markers" />
</template>
