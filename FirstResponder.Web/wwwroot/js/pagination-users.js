const tableBody = document.querySelector('#users-table-body')
const loadingText = document.querySelector('#loading-text')

let currentPage = 1;
let loading = false;
let lastPage = false;

function getUsers() {
    if (loading || lastPage) {
        return;
    }
    
    loading = true;
    loadingText.style.display = 'block';
    loadingText.innerHTML = 'Načítavam ďalších používateľov...'
    
    fetch('/api/users/filtered-table-items?pageNumber=' + currentPage)
        .then((response) => response.json())
        .then(data => {
            if (data.length === 0) {
                lastPage = true;
                return;
            }
            
            currentPage++;
            
            data.forEach(user => {
                const tr = document.createElement('tr')
                
                tr.innerHTML = `
                    <td><a href="/users/${user.id}">${user.fullName}</a></td>
                    <td><a href="/users/${user.id}">${user.email}</a></td>
                    <td><a href="/users/${user.id}">${user.phoneNumber}</a></td>
                    <td><a href="/users/${user.id}">${user.createdAt}</a></td>
                    <td><a href="/users/${user.id}">${user.type}</a></td>
                    <td><a href="/users/${user.id}">${user.region}</a></td>
                    <td><a href="/users/${user.id}">${user.address ?? ""}</a></td>
                `
                tableBody.appendChild(tr)
            })

        })
        .catch((error) => {
            loadingText.style.display = 'block';
            loadingText.innerHTML = 'Nastala chyba při načítání dat.'
            console.log(error)
        })
        .finally(() => {
            loading = false
            loadingText.style.display = 'none';
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
