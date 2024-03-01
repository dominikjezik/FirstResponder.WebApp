<script>
import { HubConnectionBuilder } from '@microsoft/signalr'

export default {
    props: {
        incidentId: {
            type: String,
            required: true
        },
        incidentLat: {
            type: String,
            required: true
        },
        incidentLon: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            responders: [],
            respondersListMessage: 'Načítavam zoznam Responderov...',
            markers: [{
                type: 'incident',
                lat: this.incidentLat,
                lon: this.incidentLon,
                icon: 'incident-icon'
            }],
        }
    },
    mounted() {
        // Load responders
        fetch(`/incidents/${this.incidentId}/responders`)
            .then(response => response.json())
            .then(data => {
                this.responders = data.map(responder => {
                    responder.acceptedAt = new Date(responder.acceptedAt).toLocaleString()
                    return responder
                })
                
                if (data.length === 0) {
                    this.respondersListMessage = 'Doposiaľ žiadny Responder nepotvrdil účasť na tomto zásahu.'
                } else {
                    this.respondersListMessage = ''
                }
            })
            .catch(error => {
                this.respondersListMessage = 'Nastala chyba pri načítaní zoznamu Responderov.'
                console.log(error)
            })

        // Setup SignalR
        if (window.signalR === undefined) {
            const connection = new HubConnectionBuilder()
                .withUrl('/hubs/incidents')
                .build()

            connection.start()
                .then(() => {
                    connection.invoke('JoinIncidentGroup', this.incidentId)
                })
            
            window.signalR = connection
        }
        
        window.signalR.on('ResponderAccepted', responder => {
            if (this.responders.some(r => r.responderId === responder.responderId)) {
                return
            }
            
            responder.acceptedAt = new Date(responder.acceptedAt).toLocaleString()

            this.responders.push(responder)
            this.respondersListMessage = ''
            
            if (responder.latitude === null || responder.longitude === null) {
                return
            }
            
            this.markers.push({
                type: 'responder',
                responderId: responder.responderId,
                lat: responder.latitude,
                lon: responder.longitude,
                icon: `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`
            })
            
            this.$refs.mapWithMarkers.refreshMarkers()
        })
        
        window.signalR.on('ResponderDeclined', responderId => {
            this.responders = this.responders.filter(responder => responder.responderId !== responderId)
            if (this.responders.length === 0) {
                this.respondersListMessage = 'Doposiaľ žiadny Responder nepotvrdil účasť na tomto zásahu.'
            }
            
            this.markers = this.markers.filter(marker => marker.type !== 'responder' || marker.responderId !== responderId)
        })
        
        window.signalR.on('ResponderLocationChanged', responder => {
            // TODO
            
            const markers = this.markers
            
            const responderMarkerIndex = markers.findIndex(marker => marker.type === 'responder' && marker.responderId === responder.responderId)
            
            if (responderMarkerIndex !== -1) {
                markers[responderMarkerIndex].lat = responder.latitude
                markers[responderMarkerIndex].lon = responder.longitude
                markers[responderMarkerIndex].icon = `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`
            } else {
                markers.push({
                    type: 'responder',
                    responderId: responder.responderId,
                    lat: responder.latitude,
                    lon: responder.longitude,
                    icon: `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`
                })
            }
            
            this.markers = markers

            this.$refs.mapWithMarkers.refreshMarkers()
        })
    },
}
</script>

<template>
    <section>
        <h2 class="heading-with-icon">
            <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" class="bi bi-geo-alt" viewBox="0 0 16 16">
                <path d="M12.166 8.94c-.524 1.062-1.234 2.12-1.96 3.07A31.493 31.493 0 0 1 8 14.58a31.481 31.481 0 0 1-2.206-2.57c-.726-.95-1.436-2.008-1.96-3.07C3.304 7.867 3 6.862 3 6a5 5 0 0 1 10 0c0 .862-.305 1.867-.834 2.94zM8 16s6-5.686 6-10A6 6 0 0 0 2 6c0 4.314 6 10 6 10z"/>
                <path d="M8 8a2 2 0 1 1 0-4 2 2 0 0 1 0 4zm0 1a3 3 0 1 0 0-6 3 3 0 0 0 0 6z"/>
            </svg>
            Umiestnenie
        </h2>
        <map-with-markers
            style="height: 300px; margin-bottom: 1rem;"
            :markers="markers"
            ref="mapWithMarkers"
        />
    </section>
    
    <h2 class="heading-with-icon">
        <svg class="nav-icon" xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" viewBox="0 0 16 16">
            <path d="M7 14s-1 0-1-1 1-4 5-4 5 3 5 4-1 1-1 1H7Zm4-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm-5.784 6A2.238 2.238 0 0 1 5 13c0-1.355.68-2.75 1.936-3.72A6.325 6.325 0 0 0 5 9c-4 0-5 3-5 4s1 1 1 1h4.216ZM4.5 8a2.5 2.5 0 1 0 0-5 2.5 2.5 0 0 0 0 5Z"/>
        </svg>
        Priradený Responderi
    </h2>
    
    <table class="table is-striped is-narrow is-hoverable is-fullwidth" v-if="responders.length > 0">
        <thead>
        <tr>
            <th>Responder</th>
            <th>Dátum a čas potvrdenia</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="responder in responders">
            <td>
                <a :href="`/users/${responder.responderId}/edit`">{{ responder.fullName }}</a>
            </td>
            <td>{{ responder.acceptedAt }}</td>
        </tr>
        </tbody>
    </table>
    
    <p v-if="respondersListMessage !== ''">{{ respondersListMessage }}</p>

    <header class="section-header" style="opacity: 0.2;">
        <h2 class="heading-with-icon">
            <svg width="28" height="28" viewBox="0 0 500 500" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path fill-rule="evenodd" clip-rule="evenodd" d="M3.79832 201.333C-17.2805 98.4708 52.7101 20.734 131.257 20.0131C171.625 19.3941 222.121 40.7359 249.035 92.2912C275.948 40.7359 326.444 19.3941 366.813 20.0131C445.36 20.734 515.35 98.4708 494.271 201.333C478.203 279.744 394.702 381.713 249.035 480.552C103.368 381.713 19.8668 279.744 3.79832 201.333ZM164.031 331.283L190.768 48.4749C209.15 59.0772 226.112 75.261 238.016 98.0647L244.335 110.173L214.174 250.673L327.999 168.283L277.857 395.135H307.363L248.319 460.766L213.116 378.72L242.623 389.081L258.995 262.813L164.031 331.283Z" fill="currentColor"/>
            </svg>
            AED
        </h2>
    </header>

    <table class="table is-striped is-narrow is-hoverable is-fullwidth" style="opacity: 0.2;">
        <thead>
        <tr>
            <th>Umiestnenie</th>
        </tr>
        </thead>
        <tbody>
        <tr>
            <td>-</td>
        </tr>
        </tbody>
    </table>
</template>
