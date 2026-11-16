# ObszervatoriumApp

.NET 8 C# konzolalkalmazás távcsőadatok beolvasására, öröklődéses tárolására,
LINQ-alapú statisztikák készítésére és szűrt adatok fájlba mentésére.

## Futtatás

A projekt mappájában:

```bash
dotnet run
```

A `tavcsovek.txt` fájl automatikusan a kimeneti könyvtárba másolódik, ha újabb
a korábban másoltnál. A program az `aktiv_tavcsovek.txt` fájlt a futtatási
könyvtárban hozza létre.
