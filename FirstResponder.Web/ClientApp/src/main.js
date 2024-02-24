import './scss/main.scss'

import aedHelpers from './js/aed-helpers.js';

import { createApp } from 'vue'

import NavigationCategory from "./components/NavigationCategory.vue"
import StatusMessage from "./components/StatusMessage.vue";
import MapWithMarkers from "./components/MapWithMarkers.vue";
import AedsListTableFilter from "./components/AedsListTableFilter.vue";
import AedsMapFilter from "./components/AedsMapFilter.vue";
import PersonalAedOwnerField from "./components/PersonalAedOwnerField.vue"
import AedImageUploader from "./components/AedImageUploader.vue"
import EditableTableItem from "./components/EditableTableItem.vue";
import EditableNewTableItem from "./components/EditableNewTableItem.vue";
import UsersListTableFilter from "./components/UsersListTableFilter.vue";
import UserGroupsModal from "./components/UserGroupsModal.vue";
import GroupMembersModal from "./components/GroupMembersModal.vue";
import GroupCreateModal from "./components/GroupCreateModal.vue";
import GroupEditModal from "./components/GroupEditModal.vue";
import IncidentsListTableFilter from "./components/IncidentsListTableFilter.vue";
import CoursesListTableFilter from "./components/CoursesListTableFilter.vue";

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
app.component('group-members-modal', GroupMembersModal)
app.component('group-create-modal', GroupCreateModal)
app.component('group-edit-modal', GroupEditModal)

// Incidents index page components
app.component('incidents-list-table-filter', IncidentsListTableFilter)

// Courses index page components
app.component('courses-list-table-filter', CoursesListTableFilter)

app.mount('#app')


// Helper functions
window.aedHelpers = aedHelpers
