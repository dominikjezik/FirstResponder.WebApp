<script>
export default {
    props: {
        userId: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            isShown: false,
            groups: [],
            newSelectedGroups: [],
            searchQuery: '',
            debounceTimer: null,
            data: {
                userId: this.userId,
                checkedOnGroupIds: [],
                checkedOffGroupIds: []
            }
        }
    },
    watch: {
        isShown(val) {
            if (!val) {
                return
            }

            this.groups = []
            this.newSelectedGroups = []
            this.data.checkedOnGroupIds = []
            this.data.checkedOffGroupIds = []
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
        groupCheckboxChanged(group) {
            if (group.isUserInGroup) {
                this.data.checkedOnGroupIds.push(group.groupId)
                this.data.checkedOffGroupIds = this.data.checkedOffGroupIds.filter(id => id !== group.groupId)

                if (!this.newSelectedGroups.some(g => g.groupId === group.groupId)) {
                    this.newSelectedGroups.push(group)
                }
            } else {
                this.data.checkedOffGroupIds.push(group.groupId)
                this.data.checkedOnGroupIds = this.data.checkedOnGroupIds.filter(id => id !== group.groupId)
                this.newSelectedGroups = this.newSelectedGroups.filter(g => g.groupId !== group.groupId)
            }
        },
        saveChanges() {
            fetch(`/users/${this.userId}/groups`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.data),
            })
                .then(res => {
                    if (res.ok) {
                        location.reload()
                    }
                })
                .catch(error => console.log(error))
        },
        searchForResults() {
            let query = this.searchQuery.trim().toLowerCase()
            
            fetch(`/users/${this.userId}/groups?query=${query}`)
                .then(response => response.json())
                .then(groups => {
                    // Zobrazenie správneho stavu checkboxov po uprave
                    groups.forEach(group => {
                        if (this.data.checkedOnGroupIds.includes(group.groupId)) {
                            group.isUserInGroup = true
                        }
                        if (this.data.checkedOffGroupIds.includes(group.groupId)) {
                            group.isUserInGroup = false
                        }
                    })

                    // Zobrazenie priradenych skupin po uprave
                    if (query === '') {
                        groups = groups.filter(group => group.isUserInGroup)

                        this.newSelectedGroups.forEach(group => {
                            if (!groups.some(g => g.groupId === group.groupId)) {
                                groups.push(group)
                            }
                        })
                    }

                    this.groups = groups.sort((a, b) => a.name.localeCompare(b.name))
                     
                    console.log(groups)
                })
                .catch(error => console.log(error))
        }
    },
    mounted() {
        window.addEventListener('display-groups-list-modal', () => {
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
                <p class="modal-card-title" style="margin-bottom: 0">Zaradenia</p>
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
                        <th>Skupina</th>
                        <th style="width: 0;"></th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="group in groups">
                        <td>{{group.name}}</td>
                        <td>
                            <input v-model="group.isUserInGroup" @change="() => groupCheckboxChanged(group)" type="checkbox" class="user-checkbox">
                        </td>
                    </tr>
                    </tbody>
                </table>
            </section>
            <footer class="modal-card-foot">
                <button @click="saveChanges" type="submit" class="button is-link">Uložiť zmeny</button>
                <button @click="closeModal" type="button" class="button">Zrušiť</button>
            </footer>
        </div>
    </div>
</template>