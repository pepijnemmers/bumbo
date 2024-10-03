![image](https://github.com/user-attachments/assets/88070fe3-0dc2-409a-8b82-aae87f7e4ab1)

# 🛒 Bumbo applicatie

<br />

## 📄 Inhoud
1. [Uitleg](#-uitleg)
2. [Trello](#-trello)
3. [Coding guidelines](#-coding-guidelines)
4. [Handige linkjes](#-handige-linkjes)

<br />

## 👀 Uitleg
### Branches

#### Feature branche
- Gebaseerd op develop
- waar aan één losse feature wordt gewerkt

#### Develop
- Zitten alle afgeronde features

#### Staging
- Wanneer er getest wordt dan wordt de Develop branch overgezet naar Staging
- Hierin kan getest worden 

#### Hotfix
- Als er een fout gevonden wordt op staging kan deze via een losse Hotfix branch verbeterd worden en toegevoegd aan de staging
- De Staging wordt na een hotfix weer gemerged met develop

#### Master
- Wanneer de testen zijn geslaagd kan de Staging branch gemerged worden naar Master
- Met toegevoegd versie label
- Dit is de productie branch en zou altijd een complete en geteste versie moeten bevatten die werkt.

![image](https://github.com/user-attachments/assets/91a31912-3b84-4d47-aac2-cfb23abca9f9)

<br />

### Feature branch & Pull request
- Geef relevante namen aan je branch en pull request (PR)
- Koppel de branch en/of de pr aan de trello taak

<br />

### Mergen
- Altijd via een pull request
- Pas merge als PR is goedgekeurd
- Feature branches moeten altijd naar develop worden gemerged
- Zorg dat de nieuwste versie van develop in je branch zit (zonder conflicts ;) )

<br />

## 📊 Trello

### Code koppelen aan de trello taak
  - Open taak > Powerups > Github > PR toevoegen <br />
  ![image](https://github.com/user-attachments/assets/9a53f7c5-07c2-413e-a7af-41738bfbebde)

<br />

## 🧾 Coding guidelines

### Best practices
Voor beste performance en betrouwbaarheid
- [Microsoft best practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices?view=aspnetcore-8.0)

<br />

### Naming conventions
Voor leesbaarheid en consistentie
- [Microsoft naming conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [Overzichtelijke versie](https://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Conventions.md)

<br />

### Code conventions
Voor leesbaarheid, consistentie en werkbaar voor team
- [Coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

<br />

## 🧷 Handige linkjes
- [Trello](https://trello.com/b/PY42DRDD/bumbo-project)
- [Brightspace](https://brightspace.avans.nl/d2l/home/204393)
- [Figma](https://www.figma.com/design/UXX8U6dmm77ADFXH7zXqK6/Bumbo-wireframes?node-id=0-1&t=OyejPM8l2YuFlWHM-1)
- [.NETCore docs](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)










