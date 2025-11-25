# Coffee Survivor - Game Design Document

**WICHTIG**: AM ANFANG: 
```
git config --global http.version HTTP/1.1
git config --global http.postBuffer 524288000
git config --global http.sslVerify true
```
Dadurch kann man große Files auf Github laden

**WICHTIG: IN UNITY IMMER IN EINER SZENE PRO PERSON ARBEITEN.**

**WICHTIG: NACH JEDEM TASK:**
```
git add .
git commit -m "added XYZ"
git push origin branchname
```

---

## High-Level Overview

**Genre:** 3D Casual/Arcade
**Engine:** Unity
**Ziel des Projekts:** Ein simples Spiel entwickeln und dabei Git-Workflows mit Branches praktisch anwenden.

### Spielprinzip

Der Spieler steuert einen Bürocharakter in einer kleinen 3D-Umgebung. Zwei Stationen bestimmen das Gameplay:

**PC-Arbeitsplatz:** Hier erscheinen zufällige Buchstaben auf dem Bildschirm. Der Spieler muss den angezeigten Buchstaben auf der Tastatur drücken, um Punkte zu erhalten. Je mehr korrekte Eingaben, desto höher der Score.

**Kaffeemaschine:** Der Kaffee-Level des Spielers sinkt kontinuierlich. Erreicht er Null, ist das Spiel vorbei. An der Kaffeemaschine kann der Spieler seinen Kaffee wieder auffüllen – verliert dabei aber wertvolle Zeit am PC.

### Core Loop

1. Spieler startet am PC und tippt Buchstaben für Punkte
2. Kaffee-Level sinkt stetig (auch während des Tippens)
3. Spieler muss rechtzeitig zur Kaffeemaschine laufen
4. Kaffee auffüllen und zurück zum PC
5. Wiederholen bis Game Over

### Win/Lose Condition

Es gibt kein klassisches "Gewinnen" – das Ziel ist ein möglichst hoher Highscore. Das Spiel endet, sobald der Kaffee-Level auf Null fällt.

---

## Git-Workflow Übersicht

Jede Person arbeitet auf einem eigenen Feature-Branch und merged nach Fertigstellung direkt in `main`.

### Branches

- `main` – Der Hauptbranch, enthält nur funktionierende, getestete Features
- `feature/player-movement` – Person 1
- `feature/ui-coffee-system` – Person 2
- `feature/typing-minigame` – Person 3

### Arbeitsablauf für jede Person

1. Vom `main`-Branch einen neuen Feature-Branch erstellen
2. Das zugewiesene Feature Schritt für Schritt implementieren
3. Nach jeder abgeschlossenen Teilaufgabe committen und pushen
4. Nach Fertigstellung einen Pull Request erstellen
5. Kurzes Review durch Teammitglieder
6. In `main` mergen

### Wichtig

Bevor ihr mit einem neuen Task beginnt, immer zuerst `main` pullen und in euren Branch mergen. So vermeidet ihr größere Konflikte.

---

## Verbindung zwischen den Features

Da wir keine komplexen Event-Systeme verwenden, nutzen wir eine einfache Methode: Jede Person gibt an den Stellen, wo später andere Features eingebunden werden, eine Debug-Nachricht aus. So wissen alle, wo sie ihren Code einfügen müssen.

**Beispiel:** Person 1 schreibt in ihrem Trigger-Script:
```csharp
Debug.Log("TODO: Hier macht PERSON 2 weiter - Kaffee auffüllen");
```

Nach dem Merge sucht Person 2 nach diesem TODO und ersetzt den Debug.Log durch den eigentlichen Code.

---

## Person 1: Player & Movement

**Branch:** `feature/player-movement`

**Szene:** `Scene_Person1_Player`

Diese Person ist verantwortlich für die grundlegende Szene, den spielbaren Charakter und das Interaktionssystem mit den beiden Stationen. Als Basis dient das Third Person Starter Asset von Unity.

### Task 1: Third Person Starter Asset importieren

Importiere das kostenlose Asset aus dem Unity Asset Store:

- Window → Package Manager → Unity Registry
- "Starter Assets - ThirdPerson" suchen und importieren
- Das Asset enthält einen fertigen Charakter mit Bewegung und Kamera

### Task 2: Grundszene erstellen

Erstelle eine neue Unity-Szene mit den grundlegenden Elementen:

- Ein flacher Boden (Plane oder Cube, skaliert)
- Einfache Beleuchtung (Directional Light)
- Den ThirdPersonController aus dem Starter Asset in die Szene ziehen
- Zwei Platzhalter-Objekte für PC und Kaffeemaschine (können zunächst einfache Cubes sein)
- Positioniere die Objekte so, dass der Spieler zwischen ihnen laufen muss (ca. 10-15 Units Abstand)

### Task 3: Interaktionszonen einrichten

Erstelle Trigger-Bereiche an beiden Stationen:

- Je ein leeres GameObject als Child von PC und Kaffeemaschine
- Box Collider hinzufügen und "Is Trigger" aktivieren
- Größe so anpassen, dass der Spieler sie beim Näherkommen betritt
- Eigene Tags erstellen: "PCZone" und "CoffeeZone" und den jeweiligen Zonen zuweisen

### Task 4: Trigger-Script für PC-Zone

Erstelle ein Script `PCZoneTrigger.cs` und hänge es an die PC-Zone:

```csharp
using UnityEngine;

public class PCZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spieler hat PC-Zone betreten");
            Debug.Log("TODO: Hier macht PERSON 3 weiter - Minigame aktivieren");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spieler hat PC-Zone verlassen");
            Debug.Log("TODO: Hier macht PERSON 3 weiter - Minigame deaktivieren");
        }
    }
}
```

### Task 5: Trigger-Script für Kaffee-Zone

Erstelle ein Script `CoffeeZoneTrigger.cs` und hänge es an die Kaffee-Zone:

```csharp
using UnityEngine;

public class CoffeeZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spieler hat Kaffee-Zone betreten");
            Debug.Log("TODO: Hier macht PERSON 2 weiter - Kaffee auffüllen");
        }
    }
}
```

**Tipp:** Stelle sicher, dass der Player-Character den Tag "Player" hat. Das Starter Asset sollte das bereits eingestellt haben.

---

## Person 2: UI & Kaffee-System

**Branch:** `feature/ui-coffee-system`

**Szene:** `Scene_Person2_UI`

Diese Person kümmert sich um die gesamte Benutzeroberfläche und die zentrale Spielmechanik des sinkenden Kaffee-Levels.

### Task 1: Canvas und Punkteanzeige

Erstelle die grundlegende UI-Struktur:

- Neuen Canvas erstellen (Screen Space - Overlay)
- TextMeshPro-Element für die Punkteanzeige in der oberen Ecke
- Gut lesbare Schriftgröße und kontrastreiche Farbe wählen
- Text initial auf "Score: 0" setzen

### Task 2: ScoreManager implementieren

Erstelle ein Script `ScoreManager.cs`:

```csharp
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
        Debug.Log("Score erhöht auf: " + score);
    }
    
    public int GetScore()
    {
        return score;
    }
}
```

### Task 3: Kaffee-Balken UI

Erstelle die visuelle Darstellung des Kaffee-Levels:

- UI-Text oder ein Image mit Fill-Amount
- Positioniere ihn gut sichtbar (z.B. oben mittig oder als vertikaler Balken am Rand)
- Farbverlauf von Braun (voll) zu Rot (fast leer) für besseres Feedback
- Beschriftung oder Icon hinzufügen

### Task 4: CoffeeManager implementieren

Erstelle ein Script `CoffeeManager.cs`:

```csharp
using UnityEngine;
using UnityEngine.UI;

public class CoffeeManager : MonoBehaviour
{
    public static CoffeeManager Instance;
    
    [SerializeField] private Slider coffeeSlider;
    [SerializeField] private float drainRate = 5f;
    private float coffeeLevel = 100f;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        coffeeLevel -= drainRate * Time.deltaTime;
        coffeeSlider.value = coffeeLevel / 100f;
        
        if (coffeeLevel <= 0)
        {
            Debug.Log("TODO: Hier macht PERSON 2 weiter - Game Over auslösen");
        }
    }
    
    public void RefillCoffee()
    {
        coffeeLevel = 100f;
        Debug.Log("Kaffee aufgefüllt!");
    }
}
```

### Task 5: Game Over System

Implementiere das Spielende:

- Game Over Panel im Canvas erstellen (zunächst deaktiviert)
- Finalen Score anzeigen
- "Restart" Button der die Szene neu lädt
- Time.timeScale = 0 setzen um das Spiel zu pausieren

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    
    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + ScoreManager.Instance.GetScore();
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
```

---

## Person 3: Typing-Minigame

**Branch:** `feature/typing-minigame`

**Szene:** `Scene_Person3_Minigame`

Diese Person entwickelt das Kern-Gameplay: das Buchstaben-Tipp-Minigame am PC.

### Task 1: Buchstaben-Anzeige UI

Erstelle die UI für das Minigame:

- Großes, zentriertes TextMeshPro-Element für den anzuzeigenden Buchstaben
- Gut lesbare, große Schrift (mind. 100pt)
- Optional: Hintergrund-Panel für bessere Sichtbarkeit
- Das gesamte UI-Element sollte initial deaktiviert sein

### Task 2: TypingGame-Script Grundstruktur

Erstelle ein Script `TypingGame.cs`:

```csharp
using UnityEngine;
using TMPro;

public class TypingGame : MonoBehaviour
{
    public static TypingGame Instance;
    
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private TMP_Text letterText;
    
    private char currentLetter;
    private bool isActive = false;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void Activate()
    {
        isActive = true;
        minigameUI.SetActive(true);
        GenerateNewLetter();
        Debug.Log("Minigame aktiviert");
    }
    
    public void Deactivate()
    {
        isActive = false;
        minigameUI.SetActive(false);
        Debug.Log("Minigame deaktiviert");
    }
    
    void GenerateNewLetter()
    {
        currentLetter = (char)Random.Range('A', 'Z' + 1);
        letterText.text = currentLetter.ToString();
    }
    
    void Update()
    {
        if (!isActive) return;
        
        foreach (char c in Input.inputString)
        {
            if (char.ToUpper(c) == currentLetter)
            {
                Debug.Log("Richtig!");
                Debug.Log("TODO: Hier macht PERSON 3 weiter - ScoreManager.Instance.AddScore(10) aufrufen");
                GenerateNewLetter();
            }
        }
    }
}
```

### Task 3: Zufälligen Buchstaben generieren

Die Methode `GenerateNewLetter()` ist bereits im Script oben enthalten:

- Erzeugt einen zufälligen Großbuchstaben A-Z
- Speichert ihn in `currentLetter`
- Zeigt ihn im UI an

### Task 4: Tastatureingabe verarbeiten

Die Eingabeerkennung ist bereits in `Update()` implementiert:

- Prüft nur wenn `isActive == true`
- `Input.inputString` enthält alle in diesem Frame getippten Zeichen
- Vergleicht jeden Buchstaben mit dem Zielbuchstaben
- Ignoriert Groß-/Kleinschreibung

### Task 5: Punkte vergeben

Ersetze das TODO im Script mit dem Aufruf des ScoreManagers:

```csharp
ScoreManager.Instance.AddScore(10);
```

**Hinweis:** Dies funktioniert erst nach dem Merge, wenn der ScoreManager von Person 2 verfügbar ist.

---

## Integration und Testing

Nachdem alle drei Personen ihre Features fertiggestellt und in `main` gemerged haben:

### Schritt 1: Szenen zusammenführen

Eine Person erstellt die finale Szene und kopiert alle Elemente zusammen:

- Player und Level von Person 1
- Canvas und Manager von Person 2
- Minigame-UI von Person 3

### Schritt 2: Alle TODOs auflösen

Sucht im Projekt nach allen verbleibenden `TODO:`-Kommentaren und ersetzt sie durch echten Code:

**In `PCZoneTrigger.cs`:**
```csharp
// Ersetze die Debug.Logs durch:
TypingGame.Instance.Activate();
// bzw.
TypingGame.Instance.Deactivate();
```

**In `CoffeeZoneTrigger.cs`:**
```csharp
// Ersetze den Debug.Log durch:
CoffeeManager.Instance.RefillCoffee();
```

**In `CoffeeManager.cs`:**
```csharp
// Ersetze den Debug.Log durch:
GameOverManager gameOver = FindObjectOfType<GameOverManager>();
gameOver.TriggerGameOver();
```

**In `TypingGame.cs`:**
```csharp
// Ersetze den Debug.Log durch:
ScoreManager.Instance.AddScore(10);
```

### Schritt 3: Gemeinsam testen

1. Alle pullen den finalen `main`-Branch
2. Gemeinsam das Spiel testen
3. Bugs identifizieren und als Team beheben
4. Balancing anpassen (Kaffee-Drain-Rate, Punkte pro Buchstabe, etc.)

### Mögliche Merge-Konflikte

Da jede Person in einer eigenen Szene arbeitet, sollten Konflikte minimal sein. Falls doch Konflikte auftreten, löst sie gemeinsam im Team.

**Tipp:** Die finale Integration macht am besten eine Person, die anderen helfen beim Testen.
