<script>
export default {
    props: {
        courseIdProp: {
            required: true
        }
    },
    data() {
        return {
            isShown: false,
            groups: [],
            searchedGroups: [],
            selectedGroup: null,
            users: [],
            searchedUsers: [],
            searchQuery: '',
            data: {
                courseId: this.courseIdProp,
                checkedOnUserIds: []
            }
        }
    },
    watch: {
        isShown(val) {
            if (!val) {
                return
            }

            this.groups = []
            this.users = []
            this.data.checkedOnUserIds = []
            this.searchQuery = ''

            this.loadGroups()
        },
        searchQuery() {
            let query = this.searchQuery.trim().toLowerCase()
            
            if (this.selectedGroup != null) {
                this.searchedUsers = this.users.filter(user => {
                    return user.fullName.toLowerCase().includes(query) || user.email.toLowerCase().includes(query)
                })
            } else {
                this.searchedGroups = this.groups.filter(group => {
                    return group.name.toLowerCase().includes(query)
                })
            }
        }
    },
    methods: {
        goBack() {
            if (this.selectedGroup != null) {
                this.selectedGroup = null
                this.users = []
                this.searchedUsers = []
                this.data.checkedOnUserIds = []
                this.searchQuery = ''
            } else {
                this.closeModal()
            }
        },
        closeModal() {
            this.isShown = false
            this.selectedGroup = null
            this.users = []
            this.searchedUsers = []
            this.data.checkedOnUserIds = []
            this.searchQuery = ''
        },
        userCheckboxChanged (user) {
            if (user.isInCourse) {
                this.data.checkedOnUserIds.push(user.userId)
            } else {
                this.data.checkedOnUserIds = this.data.checkedOnUserIds.filter(id => id !== user.userId)
            }
        },
        saveChanges() {
            fetch(`/courses/${this.data.courseId}/users`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.data),
            })
                .then(res => window.location.reload())
                .catch(error => console.log(error))
        },
        loadGroups() {
            fetch(`/courses/groups`)
                .then(response => response.json())
                .then(groups => {
                    this.groups = groups
                    this.searchedGroups = groups
                })
                .catch(error => console.log(error))
        },
        selectGroup(group) {
            this.searchQuery = ''
            this.searchedGroups = this.groups
            this.selectedGroup = group
            
            // Nacitanie uzivatelov zo skupiny
            fetch(`/groups/${group.id}/users`)
                .then(response => response.json())
                .then(users => {
                    users.forEach(user => {
                        // remove attribute isInGroup from user
                        delete user.isInGroup
                        // add attribute isInCourse and set to true
                        user.isInCourse = true
                    })
                    this.users = users
                    this.searchedUsers = users
                    this.data.checkedOnUserIds = users.map(user => user.userId)
                })
                .catch(error => console.log(error))
        }
    },
    mounted() {
        window.addEventListener('display-add-from-group-modal', (e) => {
            this.isShown = true
        })

        document.addEventListener('keydown', (event) => {
            if (event.code === 'Escape') {
                this.goBack()
            }
        });
    }
}
</script>

<template>
    <div class="modal" :class="{'is-active': isShown}">
        <div class="modal-background" @click="closeModal"></div>
        <div class="modal-card" style="height: 100%">
            <header class="modal-card-head">
                <p class="modal-card-title" style="margin-bottom: 0">Pridať účastníkov zo skupiny {{ selectedGroup != null ? selectedGroup.name : '' }}</p>
                <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
            </header>
            <section class="modal-card-body">
                <div class="field">
                    <label class="label">Vyhľadávanie</label>
                    <div class="control">
                        <input v-model="searchQuery" class="input" type="text">
                    </div>
                </div>
                
                <table class="table is-striped is-narrow is-hoverable is-fullwidth" v-if="selectedGroup == null">
                    <thead>
                    <tr>
                        <th>Skupina</th>
                        <th style="width: 0;"></th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="group in searchedGroups">
                        <td>
                            <a @click="() => selectGroup(group)">{{group.name}}</a>
                        </td>
                    </tr>
                    </tbody>
                </table>
                
                <table class="table is-striped is-narrow is-hoverable is-fullwidth" v-if="selectedGroup != null">
                    <thead>
                    <tr>
                        <th>Používateľ</th>
                        <th style="width: 0;"></th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="user in searchedUsers">
                        <td>{{user.fullName}} ({{user.email}})</td>
                        <td>
                            <input v-model="user.isInCourse" @change="() => userCheckboxChanged(user)" type="checkbox" class="user-checkbox">
                        </td>
                    </tr>
                    </tbody>
                </table>
            </section>
            <footer class="modal-card-foot">
                <button @click="saveChanges" :disabled="selectedGroup == null" type="submit" class="button is-success">Pridať účastníkov</button>
                <button @click="goBack" type="button" class="button">{{ this.selectedGroup != null ? 'Späť' : 'Zrušiť' }}</button>
            </footer>
        </div>
    </div>
</template>