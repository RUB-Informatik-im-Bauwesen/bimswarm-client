# bimswarm-hackathon/template-cs-mvc

Der Client "template-cs-mvc" ist eine Beispielimplementierung zum Empfangen und Versenden von Dateien in einer BIMSWARM Toolchain. Toolchains können entsprechend der Dokumentation genutzt werden. Weiterhin ist eine Client-Implementierung der openCDE-API erfolgt, um einer Toolchain im vollem Umfang unterstützen zu können.

## How to get started?
### Was wird benötigt?
- Microsoft Visual Studio / Visual Studio Code mit C#/.NET build tools
- ggf. kann mit Docker gearbeitet werden
- Schnellstart über Konsole: 	```dotnet run --project ./template-cs-mvc/template-cs-mvc.csproj	```



### Famework
- gebaut auf .NET Core 5.0.4
- basiert auf MVC Pattern für Client-Seiten
- serverseitige Programmierung mit C#


### Wie kann ich weitermachen?
- Pakete für clientseitige Entwicklung existieren
- Viewer von BIMSWARM kann über die API eingebunden werden, um z.B. Modelle zu visualisieren
- Serverseitig kann z.B. mit Xbim an IFC Dateien gearbeitet werden https://github.com/xBimTeam/XbimEssentials (oder via nuget https://www.nuget.org/packages/Xbim.Essentials)


### Fragen?

**Philipp Hagedorn**  
*Research Associate, Ruhr University Bochum, Germany*  

Mail: [philipp.hagedorn-n6v@rub.de](mailto:philipp.hagedorn-n6v@rub.de)

## License
This work is licensed under a
[Creative Commons Attribution 4.0 International License][cc-by].

[![CC BY 4.0][cc-by-shield]][cc-by]

[cc-by]: http://creativecommons.org/licenses/by/4.0/
[cc-by-image]: https://i.creativecommons.org/l/by/4.0/88x31.png
[cc-by-shield]: https://img.shields.io/badge/License-CC%20BY%204.0-lightgrey.svg
