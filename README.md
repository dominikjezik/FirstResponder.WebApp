# Web part of First responder system
The First Responder system is designed to keep track of trained volunteers (First Responders) who can be deployed to help using this system. This system is intended as a supplement to emergency services, especially in cases where a volunteer may arrive at the incident before the emergency services arrive at the patient. Volunteers are trained in first aid and are equipped with an AED (Automated External Defibrillator) device. First responders are alerted to the incident using a mobile app.

The text documentation of the bachelor thesis can be found in [The Central Register of Final Theses](https://opac.crzp.sk/?fn=detailBiblioForm&sid=2023538FE3639DEE25592F572DF4).

This project was implemented as a bachelor thesis at the Faculty of Management and Informatics at the University of Žilina.


## Requirements for the system
- User account management (volunteers, staff)
- Evidence of AED devices
- Organisation of the incidents
- API for mobile app


## System use cases
So far, the following system use cases (features) have been implemented.

### 👤 User accounts
- Login to the web part of the system
- CRUD operations over system users
- Modifying the type of user accounts
- Search and filter users
- Edit personal data
- Request to reset a forgotten password
- Registration via mobile app
- Login via the mobile app

### 👥 Responder groups
- CRUD operations over responder groups
- Search for groups
- Edit group users

### 📚 Course management
- CRUD operations over courses
- Search and filter courses
- Edit course users
- Assign users from a group to a course
- CRUD operations over course types
- Sending an email about a course to participants

### 📲 Manage notifications
- CRUD operations over notifications
- Search and filter notifications
- Edit user notifications
- Assign users from a group to a notification
- Sending a push notification to the app

### 🫀 Evidence of AED devices
- CRUD operations over AED devices
- Assign and remove AED photos
- Search and filter AEDs
- View public AEDs on a map
- CRUD operations over AED manufacturers, models and languages
- CRUD operations over owned AEDs

### 🆘 Incident organisation
- CRUD operations over incident
- Incident position selection on the map
- View incidents on the map
- Search and filter incidents
- View incidents on the calendar
- Notify responders near the incident
- Exchange messages between employee and responders
- View the location of responders in an incident
- View created incident reports
- Confirm and deny participation in an incident
- Retrieving information about the incident
- Creating an incident report


## Technical details
The web part is implemented in ASP.NET 8 using several libraries and frameworks. The application works as an MPA (Multi Page Application) and uses Vue.js components as the front-end. The application uses Clean archicture and is split into multiple projects.


## Starting the application
To start the system, you need to configure several services. A sample configuration structure can be found in the `appsettings.json` file in the `FirstResponder.Web` project. For development, the configuration can be saved using `User Secrets`. The entry point of the application is in the `FirstResponder.Web` project, and using npm you need to run the `npm run build` command to compile the front-end part.


### Database configuration
Microsoft SQL Server is used as the database system. For the connection you need to set the ConnectionString under the `ConnectionStrings:DefaultConnection` key. Database tables can be created from migrations using Entity Framework Core with the `dotnet ef database update` command. The command needs to be run in the `FirstResponder.Web` project folder.


### JWT (JSON Web Token)
JWT and refresh tokens are used to authenticate API endpoints used by the mobile application. To work properly, the following must be set up:
- `Jwt:Key` to the secret key to be used when signing and authenticating tokens,
- `Jwt:ExpireMinutes` expressing the JWT token expiration time in minutes,
- `Jwt:RefreshExpireMinutes` expressing the validity period of the refresh token in minutes,
- `Jwt:Issuer` to the URL to be used when signing and verifying tokens.


### Sending emails
You can use SMTP server to send emails. The `MailKit` library is used in the background. As part of the configuration you need to set up:
- `MailSettings:Server` to the SMTP server address,
- `MailSettings:Port` to the port of the SMTP server,
- `MailSettings:SenderName` to the sender name,
- `MailSettings:SenderEmail` to the sender's email,
- `MailSettings:UserName` to the username for the SMTP server,
- `MailSettings:Password` for the password for the SMTP server.


### FCM (Firebase Cloud Messaging)
Real-time notifications for the mobile application are implemented using FCM. At the beginning, you need to create a new project in the Firebase console and generate a private server key in the form of a json file. The individual items from this file need to be set within the configuration:
- `Firebase:type`
- `Firebase:project_id`
- `Firebase:private_key_id`
- `Firebase:private_key`
- `Firebase:client_email`
- `Firebase:client_id`
- `Firebase:auth_uri`
- `Firebase:token_uri`
- `Firebase:auth_provider_x509_cert_url`
- `Firebase:client_x509_cert_url`
- `Firebase:universe_domain`

These values are used by the Firebase Admin SDK library. Note: when storing `Firebase:private_key` using `User Secrets`, you must remove the `\n` characters.


### Seed Database
Seeding data can be used to populate the database with sample data. Two sections of code in the `Program.cs` file must be uncommented before running the application. The code of the first section is:
```csharp
// Database Seeding
builder.Services.AddTransient<DatabaseSeeder>();
```

The code of the second section is:
```csharp
// Seed the database
using var serviceScope = app.Services.CreateScope();
var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
seeder.Seed();
```

Once the application is started and the database is populated, these sections need to be backcommitted, otherwise the database entries would be created every time the application is started.

<br>
<br>



# Webová časť systému First responder
Systém First responder je určený na evidenciu vyškolených dobrovoľníkov (First responderov), ktorí môžu byť pomocou tohto systému vyslaní na pomoc. Tento systém je určený ako doplnok k záchranným službám hlavne v prípadoch, kedy dobrovoľník môže prísť na miesto skôr ako k pacientovi dorazí záchranná služba. Dobrovoľníci sú vyškolení v poskytovaní prvej pomoci a sú vybavení AED (Automatický Externý Defibrilátor) zariadením. First responderi sú upozornení na zásah pomocou mobilnej aplikácie.

Textová dokumentácia bakalárskej práce sa nachádza v [Centrálnom registri záverečných prác](https://opac.crzp.sk/?fn=detailBiblioForm&sid=2023538FE3639DEE25592F572DF4).

Tento projekt bol realizovaný ako bakalárska práca na Fakulte riadenia a informatiky na Žilinskej univerzite v Žiline.


## Požiadavky na systém
- Správa používateľských účtov (dobrovoľníci, zamestnanci)
- Evidencia AED zariadení
- Organizácia zásahov
- API pre mobilnú aplikáciu


## Funkcie systému
Doposiaľ boli implementované nasledujúce funkcie systému.

### 👤 Používateľské účty
- Prihlásenie sa do webovej časti systému
- CRUD operácie nad používateľmi systému
- Úprava typu používateľských účtov
- Vyhľadávanie a filtrovanie používateľov
- Úprava osobných údajov
- Požiadavie o reset zabudnutého hesla
- Registrácia cez mobilnú aplikáciu
- Prihlásenie sa cez mobilnú aplikáciu

### 👥 Skupiny responderov
- CRUD operácie nad skupinami responderov
- Vyhľadávanie skupín
- Úprava používateľov skupiny

### 📚 Správa školení
- CRUD operácie nad školeniami
- Vyhľadávanie a filtrovanie školení
- Úprava používateľov školenia
- Priradenie používateľov zo skupiny do školenia
- CRUD operácie nad typmi školení
- Odoslanie emailu o školení účastníkom

### 📲 Správa notifikácií
- CRUD operácie nad notifikáciami
- Vyhľadávanie a filtrovanie notifikácií
- Úprava používateľov notifikácie
- Priradenie používateľov zo skupiny do notifikácie
- Odoslanie push notifikácie do aplikácie

### 🫀 Evidencia AED zariadení
- CRUD operácie nad AED zariadeniami
- Priradenie a odstránenie fotografií AED
- Vyhľadávanie a filtrovanie AED
- Zobrazenie verejných AED na mape
- CRUD operácie nad výrobcami, modelmi a jazykmi AED
- CRUD operácie nad vlastneným AED

### 🆘 Organizácia zásahov
- CRUD operácie nad zásahom
- Výber pozície zásahu na mape
- Zobrazenie zásahov na mape
- Vyhľadávanie a filtrovanie zásahov
- Zobrazenie zásahov v kalendári
- Notifikovať responderov v okolí zásahu
- Výmena správ medzi zamestnancom a respondermi
- Zobrazenie polohy responderov v zásahu
- Zobrazenie vytvorených vyhodnotení zásahu
- Potvrdenie a odmietnutie účasti na zásahu
- Získanie informácií o zásahu
- Vytvorenie vyhodnotenia zásahu


## Technické detaily
Webová časť je realizovaná v ASP.NET 8 pričom používa viaceré knižnice a frameworky. Aplikácia funguje ako MPA (Multi Page Application) a ako front-end používa Vue.js komponenty. Aplikácia používa Clean archiktektúru a je rozdelená do viacerých projektov.


## Spustenie aplikácie
Pre spustenie systému je potrebné nakonfigurovať viaceré služby. Ukážková štruktúra konfigurácie sa nachádza v súbore `appsettings.json` v projekte `FirstResponder.Web`. Pre vývoj je možné ukladať konfiguráciu pomocou `User Secrets`. Vstupný bod aplikácie je v projekte `FirstResponder.Web` a pomocou npm je potrebné spustiť príkaz `npm run build` pre skompilovanie front-end časti.


### Konfigurácia databázy
Ako databázový systém je použitý Microsoft SQL Server. Pre pripojenie je potrebné nastaviť ConnectionString pod kľúčom `ConnectionStrings:DefaultConnection`. Databázové tabuľky je možné vytvoriť z migrácií pomocou Entity Framework Core príkazom `dotnet ef database update`. Príkaz je potrebné spustiť v priečinku projektu `FirstResponder.Web`.


### JWT (JSON Web Token)
Pre autentifikáciu API endpointov využívaných mobilnou aplikáciou je použitý JWT a refresh tokeny. Pre správne fungovanie je potrebné nastaviť:
- `Jwt:Key` na tajný kľúč, ktorý sa použije pri podpisovaní a overovaní tokenov,
- `Jwt:ExpireMinutes` vyjadrujúci dobu platnosti JWT tokenu v minútach,
- `Jwt:RefreshExpireMinutes` vyjadrujúci dobu platnosti refresh tokenu v minútach,
- `Jwt:Issuer` na URL adresu, ktorá sa použije pri podpisovaní a overovaní tokenov.


### Odosielanie emailov
Pre odosielanie emailov je možné použiť SMTP server. Na pozadí sa používa knižnica `MailKit`. V rámci konfigurácie je potrebné nastaviť:
- `MailSettings:Server` na adresu SMTP servera,
- `MailSettings:Port` na port SMTP servera,
- `MailSettings:SenderName` na meno odosielateľa,
- `MailSettings:SenderEmail` na email odosielateľa,
- `MailSettings:UserName` na používateľské meno pre SMTP server,
- `MailSettings:Password` na heslo pre SMTP server.


### FCM (Firebase Cloud Messaging)
Real-time notifikácie pre mobilnú aplikáciu sú zabezpečené pomocou FCM. Na začiatku je potrebné vytvoriť nový projekt v konzole Firebase a vygenerovať privátny serverový kľúč v podobe json súboru. Jednotlivé položky z tohto súboru je potrebné nastaviť v rámci konfigurácie:
- `Firebase:type`
- `Firebase:project_id`
- `Firebase:private_key_id`
- `Firebase:private_key`
- `Firebase:client_email`
- `Firebase:client_id`
- `Firebase:auth_uri`
- `Firebase:token_uri`
- `Firebase:auth_provider_x509_cert_url`
- `Firebase:client_x509_cert_url`
- `Firebase:universe_domain`

Tieto hodnoty využíva knižnica Firebase Admin SDK. Poznámka: pri ukladaní `Firebase:private_key` pomocou `User Secrets` je potrebné odstrániť `\n` znaky.

### Seed databázy
Pre naplnenie databázy ukážkovými dátami je možné použiť seedovacie dáta. Pred spustením aplikácie je potrebné odkomentovať dve sekcie kódu v súbore `Program.cs`. Kód prvej sekcie je:
```csharp
// Seedovanie databázy
builder.Services.AddTransient<DatabaseSeeder>();
```

Kód druhej sekcie je:
```csharp
// Seedovanie databázy
using var serviceScope = app.Services.CreateScope();
var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
seeder.Seed();
```

Po spustení aplikácie a naplnení databázy je potrebné tieto sekcie spätne zakomentovať, inak by sa záznamy v databáze vytvárali pri každom spustení aplikácie.
