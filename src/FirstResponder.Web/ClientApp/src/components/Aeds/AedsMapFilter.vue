<script>
export default {
    data() {
        return {
            filterInput: {
                holder: '',
                serialNumber: '',
            },
            filterSelect: {
                region: '',
                state: '',
            },
            filterSelectTechnicalDetails: {
                manufacturerId: '',
                modelId: '',
            },
            optionsState: [
                { id: 'Registered', name: 'Registrovaný' },
                { id: 'Ready', name: 'Pripravený' },
                { id: 'NotReady', name: 'Nepripravený' },
                { id: 'OutOfService', name: 'Mimo prevádzky' },
                { id: 'ServiceRequired', name: 'Servisný zásah' },
                { id: 'Cancelled', name: 'Zrušený' }
            ],
            optionsRegion: [
                { id: 0, name: 'Bratislavský kraj' },
                { id: 1, name: 'Trnavský kraj' },
                { id: 2, name: 'Trenčiansky kraj' },
                { id: 3, name: 'Nitriansky kraj' },
                { id: 4, name: 'Žilinský kraj' },
                { id: 5, name: 'Banskobystrický kraj' },
                { id: 6, name: 'Prešovský kraj' },
                { id: 7, name: 'Košický kraj' },
            ],
            optionsManufacturer: [],
            optionsModel: [],
            items: [],
            markers: [],
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

        this.loadItems()
    },
    watch: {
        filterSelect: {
            handler() {
                this.filterChanged()
            },
            deep: true
        },
        filterInput: {
            handler() {
                this.filterChanged()
            },
            deep: true
        },
        "filterSelectTechnicalDetails.modelId"() {
            this.filterChanged()
        },
        "filterSelectTechnicalDetails.manufacturerId"() {
            this.filterSelectTechnicalDetails.modelId = ''

            this.filterChanged()

            this.optionsModel = []

            if (this.filterSelectTechnicalDetails.manufacturerId === '') {
                return
            }

            fetch(`/aed/models?manufacturerId=${this.filterSelectTechnicalDetails.manufacturerId}`)
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
            this.markers = this.items
                .filter(item => {
                    if (this.filterInput.holder !== '' && !item.holder.toLowerCase().includes(this.filterInput.holder.toLowerCase())) {
                        return false
                    }
                    if (this.filterInput.serialNumber !== '' && !item.serialNumber.toLowerCase().includes(this.filterInput.serialNumber.toLowerCase())) {
                        return false
                    }
                    if (this.filterSelect.region !== '' && item.region !== this.filterSelect.region) {
                        return false
                    }
                    if (this.filterSelect.state !== '' && item.state !== this.filterSelect.state) {
                        return false
                    }
                    if (this.filterSelectTechnicalDetails.manufacturerId !== '' && item.manufacturerId !== this.filterSelectTechnicalDetails.manufacturerId) {
                        return false
                    }
                    if (this.filterSelectTechnicalDetails.modelId !== '' && item.modelId !== this.filterSelectTechnicalDetails.modelId) {
                        return false
                    }
                    
                    return true
                }).map(item => {
                    return {
                        lat: item.latitude,
                        lon: item.longitude,
                        icon: `aed-icon-${item.state}`,
                        onClickUrl: `/aed/${item.id}`
                    }
                })
        },
        loadItems() {
            fetch('/aed/map-items?type=0')
                .then((response) => response.json())
                .then(items => {
                    if (items.length === 0) {
                        return
                    }
                    
                    this.items = items.filter(item => {
                        return item.latitude !== null && item.longitude !== null
                    })
                    
                    this.markers = this.items
                        .map(item => {
                            return {
                                lat: item.latitude,
                                lon: item.longitude,
                                icon: `aed-icon-${item.state}`,
                                onClickUrl: `/aed/${item.id}`
                            }
                        })
                })
                .catch((error) => {
                    console.log(error)
                })
        },
    }
}
</script>

<template>
    <div class="filter-table-form">
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
                    <select v-model="filterSelectTechnicalDetails.manufacturerId">
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
                    <select v-model="filterSelectTechnicalDetails.modelId">
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
    
    <map-with-markers 
        style="height: 100%;"
        :markers="markers" />
</template>
