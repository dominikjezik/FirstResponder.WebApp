<script>
export default {
    data() {
        return {
            filterSelect: {
                from: '',
                to: '',
            },
            items: [],
            events: [],
            centralDate: new Date(),
        }
    },
    mounted() {
        this.dateChanged(new Date())
    },
    watch: {
        filterSelect() {
            this.filterChanged()
        }
    },
    methods: {
        onItemClicked(calendarItem, windowEvent) {
            window.location = `/incidents/${calendarItem.id}`
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
                    this.events = []

                    this.events = this.items
                        .map(item => {
                            let date = new Date(item.createdAt)
                            let dateClass = item.state
                            
                            if (date.getMonth() !== this.centralDate.getMonth()) {
                                dateClass += "-outsideOfMonth"
                            }
                            
                            return {
                                id: item.id,
                                startDate: date,
                                title: item.address,
                                classes: [dateClass]
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

            if (this.filterSelect.from !== '') {
                url.searchParams.append('from', this.filterSelect.from)
            }

            if (this.filterSelect.to !== '') {
                url.searchParams.append('to', this.filterSelect.to)
            }

            return url
        },
        dateChanged(date) {
            let firstDayOfMonth = new Date(date.getFullYear(), date.getMonth(), 1)
            let lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0)

            // Get first day of week (Monday)
            // Monday - 1, ..., Sunday - 7
            let day = firstDayOfMonth.getDay() || 7
            firstDayOfMonth.setDate(firstDayOfMonth.getDate() - day + 1)
            
            // Get last day of week (Sunday)
            // Monday - 1, ..., Sunday - 7
            day = lastDayOfMonth.getDay() || 7
            lastDayOfMonth.setDate(lastDayOfMonth.getDate() + 7 - day)
            
            // lastDayOfMonth + one day
            lastDayOfMonth.setDate(lastDayOfMonth.getDate() + 1)
            let firstDayOfNextMonth = lastDayOfMonth
            
            let firstDayOfMonthIso = new Date(firstDayOfMonth.getTime() - (firstDayOfMonth.getTimezoneOffset() * 60000)).toISOString();
            let firstDayOfNextMonthIso = new Date(firstDayOfNextMonth.getTime() - (firstDayOfNextMonth.getTimezoneOffset() * 60000)).toISOString();

            this.filterSelect.from = firstDayOfMonthIso
            this.filterSelect.to = firstDayOfNextMonthIso
            this.centralDate = date
            
            console.log(this.filterSelect.from)
            console.log(this.filterSelect.to)
            
            this.loadItems()
        }
    }
}
</script>

<template>
    <event-calendar
        style="height: 100%;"
        :events="events"
        @click-item="onItemClicked"
        @date-changed="dateChanged"
        />
</template>
