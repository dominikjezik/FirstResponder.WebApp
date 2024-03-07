import './scss/main.scss'

import aedHelpers from './js/aed-helpers.js';

import { createApp } from 'vue'

import NavigationCategory from "./components/NavigationCategory.vue"
import StatusMessage from "./components/StatusMessage.vue";
import MapWithMarkers from "./components/MapWithMarkers.vue";
import EventCalendar from "./components/EventCalendar.vue";
import AedsListTableFilter from "./components/Aeds/AedsListTableFilter.vue";
import AedsMapFilter from "./components/Aeds/AedsMapFilter.vue";
import PersonalAedOwnerField from "./components/Aeds/PersonalAedOwnerField.vue"
import AedImageUploader from "./components/Aeds/AedImageUploader.vue"
import EditableTableItem from "./components/EditableTableItem.vue";
import EditableNewTableItem from "./components/EditableNewTableItem.vue";
import UsersListTableFilter from "./components/Users/UsersListTableFilter.vue";
import UserGroupsModal from "./components/Users/UserGroupsModal.vue";
import GroupCreateModal from "./components/Groups/GroupCreateModal.vue";
import GroupEditModal from "./components/Groups/GroupEditModal.vue";
import IncidentsListTableFilter from "./components/Incidents/IncidentsListTableFilter.vue";
import IncidentsMapFilter from "./components/Incidents/IncidentsMapFilter.vue";
import IncidentsCalendar from "./components/Incidents/IncidentsCalendar.vue";
import IncidentSidebarDetails from "./components/Incidents/IncidentSidebarDetails.vue";
import IncidentMessages from "./components/Incidents/IncidentMessages.vue";
import IncidentNewMessageModal from "./components/Incidents/IncidentNewMessageModal.vue";
import IncidentReportModal from "./components/Incidents/IncidentReportModal.vue";
import CoursesListTableFilter from "./components/Courses/CoursesListTableFilter.vue";
import NotificationsListTableFilter from "./components/Notifications/NotificationsListTableFilter.vue";
import NotificationFormModal from "./components/Notifications/NotificationFormModal.vue";
import EntityUserAssociationModal from "./components/EntityUserAssociationModal.vue";
import AddUsersFromGroupToEntityModal from "./components/AddUsersFromGroupToEntityModal.vue";

const app = createApp({
    data() {
        return {
            isMobileSidebarOpen: false
        }
    }
})

// Layout components
app.component('navigation-category', NavigationCategory)
app.component('status-message', StatusMessage)

// Map components
app.component('map-with-markers', MapWithMarkers)

// Event calendar
app.component('event-calendar', EventCalendar)

// Aed index/map page components
app.component('aeds-list-table-filter', AedsListTableFilter)
app.component('aeds-map-filter', AedsMapFilter)

// Aed create/edit page components
app.component('personal-aed-owner-field', PersonalAedOwnerField)
app.component('aed-image-uploader', AedImageUploader)

// Aed language/model/manufacturer page components
app.component('editable-table-item', EditableTableItem)
app.component('editable-new-table-item', EditableNewTableItem)

// User index page components
app.component('users-list-table-filter', UsersListTableFilter)

// User details page components
app.component('user-groups-modal', UserGroupsModal)

// Group page components
app.component('group-create-modal', GroupCreateModal)
app.component('group-edit-modal', GroupEditModal)

// Incidents index/map/calendar page components
app.component('incidents-list-table-filter', IncidentsListTableFilter)
app.component('incidents-map-filter', IncidentsMapFilter)
app.component('incidents-calendar', IncidentsCalendar)
app.component('incident-sidebar-details', IncidentSidebarDetails)
app.component('incident-messages', IncidentMessages)
app.component('incident-new-message-modal', IncidentNewMessageModal)
app.component('incident-report-modal', IncidentReportModal)

// Courses index page components
app.component('courses-list-table-filter', CoursesListTableFilter)

// Notifications index page components
app.component('notifications-list-table-filter', NotificationsListTableFilter)
app.component('notification-form-modal', NotificationFormModal)

// Entity user association modal
app.component('entity-user-association-modal', EntityUserAssociationModal)
app.component('add-users-from-group-to-entity-modal', AddUsersFromGroupToEntityModal)

app.mount('#app')


// Helper functions
window.aedHelpers = aedHelpers

// SignalR
import * as signalR from "@microsoft/signalr";
window.signalR = signalR
