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
        },
        isOpened: {
            type: Boolean,
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
    methods: {
        openReportModal(responderId) {
            this.$refs.incidentReportModal.openModal(responderId)
        },
        loadResponder() {
            fetch(`/incidents/${this.incidentId}/responders`)
                .then(response => response.json())
                .then(data => {
                    this.responders = data.map(responder => {
                        responder.acceptedAt = new Date(responder.acceptedAt).toLocaleString()
                        
                        if (responder.latitude === null || responder.longitude === null) {
                            return responder
                        }

                        this.markers.push({
                            type: 'responder',
                            responderId: responder.responderId,
                            lat: responder.latitude,
                            lon: responder.longitude,
                            icon: `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`,
                            popup: `${responder.fullName}`
                        })
                        
                        return responder
                    })
                    
                    this.$refs.mapWithMarkers.refreshMarkers()

                    if (data.length === 0) {
                        if (this.isOpened) {
                            this.respondersListMessage = 'Doposiaľ žiadny Responder nepotvrdil účasť na tomto zásahu.'
                        } else {
                            this.respondersListMessage = 'V zásahu sa nezúčastnil žiadny Responder.'
                        }
                    } else {
                        this.respondersListMessage = ''
                    }
                })
                .catch(error => {
                    this.respondersListMessage = 'Nastala chyba pri načítaní zoznamu Responderov.'
                    console.log(error)
                })
        },
        newResponderAccepted(responder) {
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
                icon: `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`,
                popup: `${responder.fullName}`
            })

            this.$refs.mapWithMarkers.refreshMarkers()
        },
        updateResponderLocation(responder) {
            const responderMarkerIndex = this.markers.findIndex(marker => marker.type === 'responder' && marker.responderId === responder.responderId)

            if (responderMarkerIndex !== -1) {
                this.markers[responderMarkerIndex].lat = responder.latitude
                this.markers[responderMarkerIndex].lon = responder.longitude
                this.markers[responderMarkerIndex].icon = `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`
            } else {
                this.markers.push({
                    type: 'responder',
                    responderId: responder.responderId,
                    lat: responder.latitude,
                    lon: responder.longitude,
                    icon: `responder-${responder.typeOfTransport === null ? 'walking' : responder.typeOfTransport.toLowerCase()}`,
                    popup: `${responder.fullName}`
                })
            }

            this.$refs.mapWithMarkers.refreshMarkers()
        },
        updateFormLabels() {
            const incidentStateInput = document.getElementById('incidentStateInput')
            const cancelIncidentBtn = document.getElementById('cancelIncidentBtn')
            
            if (this.responders.length === 0) {
                incidentStateInput.value = 'Vytvorený'
                cancelIncidentBtn.innerText = 'Zrušiť zásah'
            } else {
                incidentStateInput.value = 'Prebiehajúci'
                cancelIncidentBtn.innerText = 'Ukončiť zásah'
            }
        }
    },
    mounted() {
        this.loadResponder()
        
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
        
        // New responder accepted incident
        window.signalR.on('ResponderAccepted', responder => {
            if (this.responders.some(r => r.responderId === responder.responderId)) {
                this.updateResponderLocation(responder)
                return
            }
            
            this.newResponderAccepted(responder)
            
            if (this.responders.length === 1) {
                this.updateFormLabels()
            }
        })
        
        // Responder that was already accepted declined incident
        window.signalR.on('ResponderDeclined', responderId => {
            this.responders = this.responders.filter(responder => responder.responderId !== responderId)
            if (this.responders.length === 0) {
                this.respondersListMessage = 'Doposiaľ žiadny Responder nepotvrdil účasť na tomto zásahu.'
                this.updateFormLabels()
            }
            
            this.markers = this.markers.filter(marker => marker.type !== 'responder' || marker.responderId !== responderId)
        })
        
        // Responder submitted report
        window.signalR.on('ResponderReportSubmitted', responderId => {
            const responder = this.responders.find(responder => responder.responderId === responderId)
            if (responder !== undefined) {
                responder.reportSubmitted = true
            }
        })
        
        // Responder location changed
        window.signalR.on('ResponderLocationChanged', responder => {
            this.updateResponderLocation(responder)
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
            <th style="width: 0;"></th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="responder in responders">
            <td>
                <a :href="`/users/${responder.responderId}/edit`">{{ responder.fullName }}</a>
            </td>
            <td>{{ responder.acceptedAt }}</td>
            <td>
                <a v-if="responder.reportSubmitted" @click="() => openReportModal(responder.responderId)" title="Zobraziť vyhodnotenie">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-flag-fill" viewBox="0 0 16 16">
                        <path d="M14.778.085A.5.5 0 0 1 15 .5V8a.5.5 0 0 1-.314.464L14.5 8l.186.464-.003.001-.006.003-.023.009a12 12 0 0 1-.397.15c-.264.095-.631.223-1.047.35-.816.252-1.879.523-2.71.523-.847 0-1.548-.28-2.158-.525l-.028-.01C7.68 8.71 7.14 8.5 6.5 8.5c-.7 0-1.638.23-2.437.477A20 20 0 0 0 3 9.342V15.5a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 1 0v.282c.226-.079.496-.17.79-.26C4.606.272 5.67 0 6.5 0c.84 0 1.524.277 2.121.519l.043.018C9.286.788 9.828 1 10.5 1c.7 0 1.638-.23 2.437-.477a20 20 0 0 0 1.349-.476l.019-.007.004-.002h.001"/>
                    </svg>
                </a>
            </td>
        </tr>
        </tbody>
    </table>
    
    <p v-if="respondersListMessage !== ''">{{ respondersListMessage }}</p>
    
    <incident-report-modal 
        :incident-id="incidentId" 
        ref="incidentReportModal" />
</template>
