# lunch-roulette

Diese App macht das Mittagessen mit den Kollegen amüsanter, 
indem die Gruppen so durchgemischt werden, 
dass man selten mit derselben Person zu Mittag geht.

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
Dabei wird ein Dictionary verwendet, um die möglichen Personenkombinationen herauszufinden.
Dabei wird ein String, der eine Kombination aus beiden Namen ist, als Key verwendet und die Anzahl,
wie häufig sich diese beiden Personen bereits getroffen haben, als Integerwert gespeichert.


## Gedanken
Ich habe Üsprünglich mit Date, string Kombinationen arbeiten wollen um Komplexität vermeiden zu wollen.

```mermaid
```