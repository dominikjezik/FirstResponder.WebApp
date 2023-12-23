<script>
export default {
    props: {
        selectedOwner: {
            type: Object,
            default: null
        }
    },
    data() {
        return {
            debounceTimer: null,
            currentOwner: this.selectedOwner,
            searchQuery: "",
            isLoading: false,
            searchResults: []
        }
    },
    watch: {
        searchQuery() {
            clearTimeout(this.debounceTimer)
            this.debounceTimer = setTimeout(this.searchForResults, 300)
        }
    },
    methods: {
        searchForResults() {
            let query = this.searchQuery.trim()

            if (query === '') {
                this.searchResults = []
                return
            }

            this.isLoading = true

            fetch('/api/users/search?query=' + query)
                .then((response) => response.json())
                .then(data => {
                    this.isLoading = false
                    this.searchResults = data
                })
                .catch((error) => console.log(error))
        },
        selectOwner(userId, fullName, email) {
            this.currentOwner = { userId: userId, fullName: fullName, email: email }
            this.searchQuery = ""
        },
        removeSelectedOwner() {
            this.currentOwner = null
        },
        focusOutSearchInput() {
            // if mouse is over results, do not hide them
            if (document.querySelector('.aed-responder-search-field .results:hover')) {
                return
            }

            this.searchResults = []
        }
    }
};
</script>

<template>
    <section class="form-section">
        <h2>Vlastník AED (First responder)</h2>
        <div class="field aed-responder-search-field" v-show="currentOwner == null">
            <label class="label">Používateľ *</label>
            <div class="control" :class="{ 'is-loading' : isLoading }">
                <input v-model="searchQuery" @focusin="searchForResults" @focusout="focusOutSearchInput" class="input" placeholder="Vyhľadávanie podľa mena alebo emailu" autocomplete="off">
            </div>
            <ul class="results panel">
                <li v-for="user in searchResults">
                    <a @click="() => selectOwner(user.userId, user.fullName, user.email)" class="panel-block search-result">
                        <b>{{ user.fullName }}</b> &nbsp;({{ user.email }})
                    </a>
                </li>
            </ul>
        </div>

        <div class="card" v-if="currentOwner != null">
            <footer class="card-content aed-responder-selected-owner">
                <p class="selected-owner-p"><b>{{ currentOwner.fullName }}</b> ({{ currentOwner.email }})</p>
                <button @click="removeSelectedOwner" type="button" class="delete" aria-label="delete"></button>
            </footer>
            <input type="hidden" name="AedFormDTO.OwnerId" :value="currentOwner.userId">
        </div>
    </section>
</template>
