# Webová časť systému First responder
Systém First responder je určený na evidenciu vyškolených dobrovoľníkov (First responderov), ktorí môžu byť pomocou tohto systému vyslaní na pomoc. Tento systém je určený ako doplnok k záchranným službám hlavne v prípadoch, kedy dobrovoľník môže prísť na miesto skôr ako k pacientovi dorazí záchranná služba. Dobrovoľníci sú vyškolení v poskytovaní prvej pomoci a sú vybavení AED (Automatický Externý Defibrilátor) zariadením. First responderi sú upozornení na zásah pomocou mobilnej aplikácie.

Tento projekt bol realizovaný ako bakalárska práca na Fakulte riadenia a informatiky na Žilinskej univerzite v Žiline.


## Požiadavky na systém
- Správa používateľských účtov (dobrovoľníci, zamestnanci)
- Evidencia AED zariadení
- Organizácia zásahov
- API pre mobilnú aplikáciu


## Technické detaily
Webová časť je realizovaná v ASP.NET 8 pričom používa viaceré knižnice a frameworky. Aplikácia funguje ako MPA (Multi Page Application) a ako front-end používa Vue.js komponenty. Aplikácia používa Clean archiktektúru a je rozdelená do viacerých projektov.


## Spustenie aplikácie
Pre spustenie systému je potrebné nakonfigurovať viaceré služby. Ukážková štruktúra konfigurácie sa nachádza v súbore `appsettings.json` v projekte `FirstResponder.Web`. Pre vývoj je možné ukladať konfiguráciu pomocou `User Secrets`.


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
- Zobrazenei zásahov na mape
- Vyhľadávanie a filtrovanie zásahov
- Zobrazenie zásahov v kalendári
- Notifikovať responderov v okolí zásahu
- Výmena správ medzi zamestnancom a respondermi
- Zobrazenie polohy responderov v zásahu
- Zobrazenie vytvorených vyhodnotení zásahu
- Potvrdenei a odmietnutie účasti na zásahu
- Získanie informácií o zásahu
- Vytvorenie vyhodnotenia zásahu
