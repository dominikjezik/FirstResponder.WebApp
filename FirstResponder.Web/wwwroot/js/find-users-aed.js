const radioPersonalAed = document.getElementById('radio-personal-aed')

if (radioPersonalAed.checked) {
    setupSearchBox()
}

radioPersonalAed.onchange = (ev) => {
    if (ev.target.checked) {
        setTimeout(() => setupSearchBox(), 0) // obicyklovanie bugu
    }
}

function setupSearchBox() {
    const searchBox = document.getElementById('search-box')
    const searchResults = document.getElementById('search-results')
    const removeSelectedOwner = document.getElementById('remove-selected-owner')

    searchBox.addEventListener('keyup', searchForResults)
    searchBox.addEventListener('focusin', searchForResults)
    searchBox.addEventListener('focusout', () => searchResults.innerHTML = '')

    removeSelectedOwner.onclick = (ev) => {
        ev.preventDefault()
        const searchField = document.getElementById('search-field')
        const selectedOwner = document.getElementById('selected-owner')
        
        document.getElementById('owner-id').value = ''
        searchField.style.display = 'block'
        selectedOwner.style.display = 'none'
    }
}

function searchForResults() {
    const searchResults = document.getElementById('search-results')
    const searchBox = document.getElementById('search-box')
    let query = searchBox.value.trim()

    if (query === '') {
        searchResults.innerHTML = ''
        document.getElementById('owner-id').value = ''
        return
    }

    fetch('/api/users/search?query=' + query)
        .then((response) => response.json())
        .then(data => {
            // clear previeous results
            searchResults.innerHTML = ''
            
            // map search results to html
            data.forEach(user => addSearchItemIntoHtml(user))

            // add event listeners to search results
            document.querySelectorAll('.search-result').forEach((link) => {
                link.onmousedown = (ev) => ev.preventDefault() /* obicyklovanie bugu */
                link.onclick = (ev) => selectOwner(ev)
            })

        })
        .catch((error) => console.log(error))
}

function selectOwner(ev) {
    const searchField = document.getElementById('search-field')
    const searchBox = document.getElementById('search-box')
    const searchResults = document.getElementById('search-results')
    
    const selectedOwner = document.getElementById('selected-owner')
    const ownerName = document.getElementById('owner-name')
    
    selectedOwner.style.display = 'block'
    ownerName.innerHTML = `<b>${ev.target.dataset.fullname}</b> (${ev.target.dataset.email})`
    document.getElementById('owner-id').value = ev.target.dataset.userid
    
    searchBox.blur()
    searchBox.value = ''
    searchResults.innerHTML = ''
    searchField.style.display = 'none'
}

function addSearchItemIntoHtml(user) {
    const searchResults = document.getElementById('search-results')
    searchResults.innerHTML += 
        `<li>
            <a class="panel-block search-result" data-userid="${user.userId}" data-fullname="${user.fullName}" data-email="${user.email}">
                <b>${user.fullName}</b> &nbsp;(${user.email})
            </a>
        </li>`
}

