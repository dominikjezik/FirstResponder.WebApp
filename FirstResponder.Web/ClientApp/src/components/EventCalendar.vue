<script>
import { CalendarView, CalendarViewHeader } from "vue-simple-calendar"

import "vue-simple-calendar/dist/style.css"
import "vue-simple-calendar/dist/css/gcal.css"

export default {
    props: {
        events: {
            type: Array,
            required: true
        }
    },
    data() {
        return {
            showDate: new Date(),
            items: this.events
        }
    },
    components: {
        CalendarView,
        CalendarViewHeader,
    },
    methods: {
        setShowDate(d) {
            this.showDate = d
            this.$emit("date-changed", d)
        },
        thisMonth(d, h, m) {
            const t = new Date()
            return new Date(t.getFullYear(), t.getMonth(), d, h || 0, m || 0)
        },
        onItemClicked(calendarItem, windowEvent) {
            this.$emit("click-item", calendarItem, windowEvent)
        }
    }
}
</script>

<template>
    <div>
        <calendar-view
            :items="events"
            :startingDayOfWeek="1"
            :show-date="showDate"
            item-top="2.6em"
            item-content-height="1.8em"
            current-period-label="Dnes"
            locale="sk"
            @click-item="(calendarItem, windowEvent) => $emit('click-item', calendarItem, windowEvent)"
            style="width: 100%; height: 100%;"
            class="theme-gcal">
            <template #header="{ headerProps }">
                <calendar-view-header
                    slot="header" :header-props="headerProps"
                    previous-year-label=""
                    previous-period-label=""
                    next-period-label=""
                    next-year-label=""
                    @input="setShowDate"
                />
            </template>
        </calendar-view>
    </div>
</template>

<style scoped>
@import "https://fonts.googleapis.com/icon?family=Material+Icons";
</style>