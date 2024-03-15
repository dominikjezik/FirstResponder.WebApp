<script>
export default {
    props: {
        modalTitle: {
            type: String,
            required: true
        },
        saveChangesBaseUrl: {
            type: String,
            required: true
        },
        searchBaseUrl: {
            type: String,
            required: true
        },
        onSaveReloadPage: {
            type: Boolean,
            required: false,
            default: false
        }
    },
    data() {
        return {
            isShown: false,
            users: [],
            newSelectedUsers: [],
            searchQuery: '',
            debounceTimer: null,
            data: {
                entityId: null,
                checkedOnUserIds: [],
                checkedOffUserIds: []
            }
        }
    },
    watch: {
        isShown(val) {
            if (!val) {
                return
            }

            this.users = []
            this.newSelectedUsers = []
            this.data.checkedOnUserIds = []
            this.data.checkedOffUserIds = []
            this.searchQuery = ''

            this.searchForResults()
        },
        searchQuery() {
            clearTimeout(this.debounceTimer)
            this.debounceTimer = setTimeout(this.searchForResults, 300)
        }
    },
    methods: {
        closeModal() {
            this.isShown = false
        },
        userCheckboxChanged(user) {
            if (user.isAssociated) {
                this.data.checkedOnUserIds.push(user.userId)
                this.data.checkedOffUserIds = this.data.checkedOffUserIds.filter(id => id !== user.userId)

                if (!this.newSelectedUsers.some(u => u.userId === user.userId)) {
                    this.newSelectedUsers.push(user)
                }
            } else {
                this.data.checkedOffUserIds.push(user.userId)
                this.data.checkedOnUserIds = this.data.checkedOnUserIds.filter(id => id !== user.userId)
                this.newSelectedUsers = this.newSelectedUsers.filter(u => u.userId !== user.userId)
            }
        },
        saveChanges() {
            fetch(`${this.saveChangesBaseUrl}/${this.data.entityId}/users`, {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(this.data),
            })
                .then(res => this.onSaveReloadPage ? location.reload() : this.closeModal())
                .catch(error => console.log(error))

        },
        searchForResults() {
            let query = this.searchQuery.trim().toLowerCase()

            fetch(`${this.searchBaseUrl}/${this.data.entityId}/users?query=${query}`)
                .then(response => response.json())
                .then(users => {
                    // Zobrazenie správneho stavu checkboxov po uprave
                    users.forEach(user => {
                        if (this.data.checkedOnUserIds.includes(user.userId)) {
                            user.isAssociated = true
                        }
                        if (this.data.checkedOffUserIds.includes(user.userId)) {
                            user.isAssociated = false
                        }
                    })

                    // Zobrazenie zaradenych uzivatelov po uprave
                    if (query === '') {
                        users = users.filter(user => user.isAssociated)

                        this.newSelectedUsers.forEach(user => {
                            if (!users.some(u => u.userId === user.userId)) {
                                users.push(user)
                            }
                        })
                    }

                    this.users = users.sort((a, b) => a.fullName.localeCompare(b.fullName))
                })
                .catch(error => console.log(error))
        }
    },
    mounted() {
        window.addEventListener('display-users-list-modal', (e) => {
            this.data.entityId = e.detail.entityId
            this.isShown = true
        })

        document.addEventListener('keydown', (event) => {
            if (event.code === 'Escape') {
                this.closeModal();
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
                <p class="modal-card-title" style="margin-bottom: 0">{{ modalTitle }}</p>
                <button @click="closeModal" type="button" class="delete" aria-label="close"></button>
            </header>
            <section class="modal-card-body">
                <div class="field">
                    <label class="label">Vyhľadávanie</label>
                    <div class="control">
                        <input v-model="searchQuery" class="input" type="text">
                    </div>
                </div>
                <table class="table is-striped is-narrow is-hoverable is-fullwidth">
                    <thead>
                    <tr>
                        <th>Používateľ</th>
                        <th style="width: 0;"></th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="user in users">
                        <td>{{ user.fullName }} ({{ user.email }})</td>
                        <td>
                            <input v-model="user.isAssociated" @change="() => userCheckboxChanged(user)" type="checkbox"
                                   class="user-checkbox">
                        </td>
                    </tr>
                    </tbody>
                </table>
            </section>
            <footer class="modal-card-foot">
                <button @click="saveChanges" type="submit" class="button is-link" id="btn-submit-users-update">Uložiť zmeny</button>
                <button @click="closeModal" type="button" class="button">Zrušiť</button>
            </footer>
        </div>
    </div>
</template>