import './scss/main.scss'

import { createApp } from 'vue'

import NavigationCategory from "./components/NavigationCategory.vue"
import PersonalAedOwnerField from "./components/PersonalAedOwnerField.vue"
import AedImageUploader from "./components/AedImageUploader.vue"
import EditableTableItem from "./components/EditableTableItem.vue";
import EditableNewTableItem from "./components/EditableNewTableItem.vue";
import UsersListTableFilter from "./components/UsersListTableFilter.vue";
import GroupMembersModal from "./components/GroupMembersModal.vue";
import GroupCreateModal from "./components/GroupCreateModal.vue";
import GroupEditModal from "./components/GroupEditModal.vue";
import UserGroupsModal from "./components/UserGroupsModal.vue";

const app = createApp({
    data() {
        return {
            isMobileSidebarOpen: false
        }
    }
})

// Layout components
app.component('navigation-category', NavigationCategory)

// Aed create/edit page components
app.component('personal-aed-owner-field', PersonalAedOwnerField)
app.component('aed-image-uploader', AedImageUploader)

// Aed language/model/manufacturer page components
app.component('editable-table-item', EditableTableItem)
app.component('editable-new-table-item', EditableNewTableItem)

// User index page components
app.component('users-list-table-filter', UsersListTableFilter)

// Group page components
app.component('group-members-modal', GroupMembersModal)
app.component('group-create-modal', GroupCreateModal)
app.component('group-edit-modal', GroupEditModal)

// User details page components
app.component('user-groups-modal', UserGroupsModal)

app.mount('#app')
