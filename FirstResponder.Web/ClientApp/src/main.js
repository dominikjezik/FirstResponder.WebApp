import './scss/main.scss'

import { createApp } from 'vue'

import NavigationCategory from "./components/NavigationCategory.vue"
import PersonalAedOwnerField from "./components/PersonalAedOwnerField.vue"
import AedImageUploader from "./components/AedImageUploader.vue"
import EditableTableItem from "./components/EditableTableItem.vue";
import EditableNewTableItem from "./components/EditableNewTableItem.vue";
import UsersListTableFilter from "./components/UsersListTableFilter.vue";
import GroupMembersModal from "./components/GroupMembersModal.vue";

const app = createApp({
    data() {
        return {
            isMobileSidebarOpen: false
        }
    }
})

app.component('navigation-category', NavigationCategory)
app.component('personal-aed-owner-field', PersonalAedOwnerField)
app.component('aed-image-uploader', AedImageUploader)
app.component('editable-table-item', EditableTableItem)
app.component('editable-new-table-item', EditableNewTableItem)
app.component('users-list-table-filter', UsersListTableFilter)
app.component('group-members-modal', GroupMembersModal)

app.mount('#app')
