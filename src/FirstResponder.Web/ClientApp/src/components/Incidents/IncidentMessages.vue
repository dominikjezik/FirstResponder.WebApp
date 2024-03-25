<script>
import { HubConnectionBuilder } from '@microsoft/signalr'

export default {
    props: {
        incidentId: {
            type: String,
            required: true
        },
        displaySendMessageButton: {
            type: Boolean,
            default: true
        }
    },
    data() {
        return {
            messages: [],
            statusMessage: 'Načítavam zoznam správ...'
        }
    },
    methods: {
        onMessageSent(message) {
            if (this.messages.some(m => m.id === message.id)) {
                return
            }
            
            message.createdAt = new Date(message.createdAt).toLocaleString()
            this.messages.unshift(message)
            this.statusMessage = ''
        }
    },
    mounted() {
        fetch(`/incidents/${this.incidentId}/messages`)
            .then(response => response.json())
            .then(data => {
                data.forEach(message => {
                    message.createdAt = new Date(message.createdAt).toLocaleString()
                })
                
                this.messages = data;
                
                if (data.length === 0) {
                    if (!this.displaySendMessageButton) {
                        this.statusMessage = 'Neboli odoslané žiadne správy.'
                    } else {
                        this.statusMessage = 'Doposiaľ nebola odoslaná žiadna správa.'
                    }
                } else {
                    this.statusMessage = ''
                }
            })
            .catch(error => {
                this.statusMessage = 'Nastala chyba pri načítaní zoznamu správ.'
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
        
        window.signalR.on('NewMessage', message => this.onMessageSent(message))
    }
}
</script>

<template>
    <section>
        <header class="section-header">
            <h2 class="heading-with-icon">
                <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" viewBox="0 0 16 16">
                    <path d="M0 4a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V4Zm2-1a1 1 0 0 0-1 1v.217l7 4.2 7-4.2V4a1 1 0 0 0-1-1H2Zm13 2.383-4.708 2.825L15 11.105V5.383Zm-.034 6.876-5.64-3.471L8 9.583l-1.326-.795-5.64 3.47A1 1 0 0 0 2 13h12a1 1 0 0 0 .966-.741ZM1 11.105l4.708-2.897L1 5.383v5.722Z"/>
                </svg>
                Správy
            </h2>

            <button class="button" @click="$refs.newMessageModal.openModal" v-if="displaySendMessageButton">
                Odoslať správu
            </button>
        </header>

        <p v-if="statusMessage !== ''">{{ statusMessage }}</p>

        <div class="incident-messages">
            <div class="message is-link" v-for="message in messages">
                <div class="message-body">
                    <strong>{{ message.senderName }}</strong> {{ message.createdAt }}
                    <p>{{ message.content }}</p>
                </div>
            </div>
        </div>
        
        <incident-new-message-modal
            :incident-id="incidentId"
            ref="newMessageModal"
            @message-sent="onMessageSent"
        />
    </section>
</template>