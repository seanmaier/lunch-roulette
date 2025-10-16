# lunch-roulette

Diese App macht das Mittagessen mit den Kollegen amüsanter, 
indem die Gruppen so durchgemischt werden, 
dass man selten mit derselben Person zu Mittag geht.

# Setup
Das Setup zum entwickeln wird als erstes erläutert. Daraufhin wie die App mit Docker funktioniert.

## Dev environment
Um das Projekt zu starten, müssen erst einmal, 
die Dependencies im `lunch-roulette/` Ordner wiederhergestellt werden.
```bash
dotnet restore
```

Wenn es noch keine Datenbank gibt, 
weil keine Migrationen unter `/lunch-roulette/Migrations/` vorhanden sind,
müssen diese erst einmal erstellt werden.

```bash
dotnet ef migrations add InitCreate
```
Daraufhin, muss die Datenbank aktualisiert werden.

```bash
dotnet ef database update
```

Die Verbindung wird aktuell via der `appsettings.Development.json` Datei im Bereich `ConnectionsStrings:DefaultConnection` gesetzt.
Diese ist standardmäßig auf `Data Source=app.db` gesetzt.

Final muss das Projekt nur noch ausgeführt werden.

```bash
cd ./lunch-roulettte/ # Falls noch nicht im Projekt Verzeichnis
dotnet run
# oder
dotnet watch # Wenn Hot Module Reload aktiviert sein sol 
```

## Docker
Um das Projekt mit Docker aufzusetzen, muss Docker installiert sein.

Das Image muss erst einmal aus dem Projektursprung gebaut werden.

```bash
docker build . -t lunch-roulette
```

Um es nun zu starten:

```bash
docker run -p 8080:8080 lunch-roulette
```

# Architektur
Die Architektur besteht Aus einer Datenbank, einer Service-Schicht für interne Logik und dem Frontend,
welches die Daten von der Service-Schicht erhält. Diese Services werden mithilfe von Dependency Injection geladen.

## Datenbankmodell
Um Wiederholungen in der Datenbank zu vermeiden und die Regeln der Normalisierung einzuhalten,
wurden vier Datenbanktabellen aufgebaut.

- `Persons`
- `GroupMembers`
- `Groups`
- `Lunch`

Der Grund warum es 3 Tabellen nur für die Lunches gibt ist, 
dass mehrere Gruppen an demselben Lunch teilnehmen, währenddessen mehrere Personen einer Gruppe angehören,
aber auch eine Person mehrere Gruppen angehört, da diese jede Woche neu besetzt werden.

### Tabellendefinition
| Tabelle        | Definition                                                                      | Felder                                |
|----------------|---------------------------------------------------------------------------------|---------------------------------------|
| `Persons`      | Stellt die Mitarbeiterwaltung dar.                                              | `Id`, `Name`, `Abteilung`, `Jobtitel` |
| `GroupMembers` | Wird genutzt um zu tracken, welche Personen zu welchen Gruppen bereits gehörten | `Id`, `PersonId`, `GroupId`           |
| `Groups`       | Verweist auf das Lunch, an dem die Gruppe teilgenommen hat.                     | `Id`, `LunchId`,                      |
| `Lunch`         | Das spezifische Lunch an dem sich die Gruppen zum Mittagessen getroffen haben.  | `Id`, `Date`                           |

## Services
Es existieren aktuell Zwei Services:
- PersonService
- LunchService

PersonService stellt simple CRUD Operationen zur Verfügung, um Nutzer bearbeiten, löschen und zu erstellen.

LunchService ist für das Bereitstellen und erstellen der Lunches da.
Ein Lunch wird kalkuliert, sodass dieselben Personenkombinationen vermieden werden.
Ein Dictionary wird genutzt, um die möglichen Personenkombinationen herauszufinden.
Dabei wird ein String, der eine Kombination aus beiden Namen ist, als Key verwendet und die Anzahl,
wie häufig sich diese beiden Personen bereits getroffen haben, als Integerwert gespeichert.
Eine `unassinged` Liste mit Personen, wird zum prüfen genutzt. Solange diese nicht leer ist, werden Personen weiter zugeordnet.


## Gedanken
Vier Tabellen für Lunches zu erstellen scheint mir zu Beginn zwar richtig, jedoch etwas komplex, wenn es um die Queries geht.
Um unnötige Komplexität zu Anfang zu vermeiden, will ich erst einmal testen, die Daten nur mithilfe von Lunch und Group Tabellen zu speichern.
Die Gruppen werden denn als JSON string in der Gruppentabelle gespeichert. Später dann werde ich eine Matrix oder dergleichen
verwenden, um Normalisierungen anzuwenden, mithilfe ich dann die EF Core Queries ausreizen kann.

Wenn Nutzer gelöscht werden, sollen diese dann in der Historie erhalten bleiben? 
Wie soll das funktionieren, wenn der Eintrag mit dem Namen gelöscht wurde?

Der Algorithmus ist aktuell eingeschränkt darauf, die letzten drei Einträge einfach zusammenzufügen.
Man könnte für die ungerade Person ebenfalls noch kalkulieren, welche Personen diese am wenigsten getroffen haben.

## Zeitaufwand

| Schritt           | Zeit | Beschreibung                                                                          |
|-------------------|------|---------------------------------------------------------------------------------------|
| Technik           | 1    | Blazor generell verstehen und spezifisch Blazor Server. Mit SignalR auseinandersetzen |
| Frontend bauen    | 3    | Ein paar wenigste Styles einbauen und UI Library anwenden                             |
| Datenbank         | 2    | Datenbankmodell aufbauen und reevaluieren                                             |
| Lunch Algorithmus | 8    | Lunch Algorithmus entwerfen und implementieren                                        |
| Dockerfile        | 1,5  | Dockerfile schreiben und debuggen                                                     | 
| Serilog           | 0,5  | Serilog einrichten                                                                    |
| Gesamt            | 16   |                                                                                       |