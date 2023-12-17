const tableBody = document.querySelector('#users-table-body')
const loadingText = document.querySelector('#loading-text')

const filterFieldName = document.querySelector('#filter-field-name')
const filterFieldPhone = document.querySelector('#filter-field-phone')
const filterSelectType = document.querySelector('#filter-select-type')
const filterSelectRegion = document.querySelector('#filter-select-region')

let currentPage = 1;
let loading = false;
let lastPage = false;

filterFieldName.oninput = filterChanged
filterFieldPhone.oninput = filterChanged
filterSelectType.onchange = filterChanged
filterSelectRegion.onchange = filterChanged

function filterChanged() {
    currentPage = 0;
    lastPage = false;
    tableBody.innerHTML = '';
    getUsers();
}

function getUsers() {
    if (loading || lastPage) {
        return;
    }
    
    loading = true;
    loadingText.style.display = 'block';
    loadingText.innerHTML = 'Načítavam ďalších používateľov...'
    
    let url = new URL('/api/users/filtered-table-items', window.location.href)
    url.searchParams.append('pageNumber', currentPage)
    
    if (filterFieldName.value) {
        url.searchParams.append('fullName', filterFieldName.value)
    }
    
    if (filterFieldPhone.value) {
        url.searchParams.append('phoneNumber', filterFieldPhone.value)
    }
    
    if (filterSelectType.value) {
        url.searchParams.append('type', filterSelectType.value)
    }
    
    if (filterSelectRegion.value) {
        url.searchParams.append('region', filterSelectRegion.value)
    }
    
    fetch(url)
        .then((response) => response.json())
        .then(data => {
            if (data.length === 0) {
                if (currentPage === 0) {
                    loadingText.style.display = 'block';
                    loadingText.innerHTML = 'Nenašli sa žiadni používatelia.'
                } else {
                    loadingText.style.display = 'none';
                }
                
                lastPage = true;
                return;
            }
            
            currentPage++;
            
            data.forEach(user => {
                const tr = document.createElement('tr')
                
                tr.onclick = () => window.location = `/users/${user.id}`
                
                tr.innerHTML = `
                    <td>${user.fullName}</td>
                    <td>${user.email}</td>
                    <td>${user.phoneNumber}</td>
                    <td>${user.createdAt}</td>
                    <td>${user.type}</td>
                    <td>${user.region}</td>
                    <td>${user.address ?? ""}</td>
                `
                tableBody.appendChild(tr)
            })

            loadingText.style.display = 'none';
        })
        .catch((error) => {
            loadingText.style.display = 'block';
            loadingText.innerHTML = 'Nastala chyba při načítání dat.'
            console.log(error)
        })
        .finally(() => {
            loading = false
        })
}

// On scroll, check if we’re at the bottom of the page
window.onscroll = () => {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
        if (!lastPage) {
            getUsers();
        }
    }
}
