using System.Globalization;

namespace ObszervatoriumApp;

internal class Program
{
    private static void Main()
    {
        // A magyar területi beállítás biztosítja például az 1028,6 formátumot.
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("hu-HU");

        const string bemenetiFajl = "tavcsovek.txt";
        const string kimenetiFajl = "aktiv_tavcsovek.txt";

        if (!File.Exists(bemenetiFajl))
        {
            Console.WriteLine($"A(z) {bemenetiFajl} fájl nem található: {Path.GetFullPath(bemenetiFajl)}");
            return;
        }

        string[] sorok = File.ReadAllLines(bemenetiFajl);
        var tavcsovek = new List<Tavcso>();
        int hibasSorok = 0;

        foreach (string sor in sorok.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(sor))
            {
                hibasSorok++;
                continue;
            }

            string[] mezok = sor.Split('|');
            if (mezok.Length != 5)
            {
                hibasSorok++;
                continue;
            }

            string tipus = mezok[0].Trim();
            string azonosito = mezok[1].Trim();

            if (!int.TryParse(mezok[2], out int atmeroCm) ||
                !int.TryParse(mezok[3], out int uzemora) ||
                !int.TryParse(mezok[4], out int extra))
            {
                hibasSorok++;
                continue;
            }

            switch (tipus)
            {
                case "O":
                    tavcsovek.Add(new Tavcso(azonosito, atmeroCm, uzemora));
                    break;
                case "R":
                    tavcsovek.Add(new Radiotavcso(azonosito, atmeroCm, uzemora, extra));
                    break;
                default:
                    hibasSorok++;
                    break;
            }
        }

        Console.WriteLine($"Sikeresen beolvasott távcsövek: {tavcsovek.Count} db");
        Console.WriteLine($"Kiszűrt hibás sorok száma: {hibasSorok} db");
        Console.WriteLine("Beolvasott távcsövek listája:");

        foreach (Tavcso tavcso in tavcsovek)
        {
            Console.WriteLine(tavcso);
        }

        Console.WriteLine();
        Console.WriteLine("LINQ Statisztikák:");

        int osszesKoltseg = tavcsovek.Sum(t => t.FenntartasiKoltseg());
        Console.WriteLine($"Összesített éves fenntartási költség: {osszesKoltseg} EUR");

        double atlagosUzemora = tavcsovek.Average(t => t.Uzemora);
        Console.WriteLine($"Átlagos üzemóra: {atlagosUzemora:F1} óra");

        Tavcso? legnagyobbAtmero = tavcsovek.MaxBy(t => t.AtmeroCm);
        if (legnagyobbAtmero is not null)
        {
            Console.WriteLine($"Legnagyobb átmérőjű távcső: {legnagyobbAtmero.Azonosito} ({legnagyobbAtmero.AtmeroCm} cm)");
        }

        Tavcso? legalacsonyabbKoltseg = tavcsovek.MinBy(t => t.FenntartasiKoltseg());
        if (legalacsonyabbKoltseg is not null)
        {
            Console.WriteLine($"Legalacsonyabb költségű távcső: {legalacsonyabbKoltseg.Azonosito} ({legalacsonyabbKoltseg.FenntartasiKoltseg()} EUR)");
        }

        int legalabbEzerOra = tavcsovek.Count(t => t.Uzemora >= 1000);
        Console.WriteLine($"Legalább 1000 órát üzemelt távcsövek: {legalabbEzerOra} db");

        bool vanNegyvenezerFeletti = tavcsovek.Any(t => t.FenntartasiKoltseg() > 40000);
        Console.WriteLine($"Van 40 000 EUR feletti költségű távcső: {(vanNegyvenezerFeletti ? "Van" : "Nincs")}");

        var nagyAtmerojuTavcsovek = tavcsovek
            .Where(t => t.AtmeroCm >= 300)
            .OrderByDescending(t => t.AtmeroCm);

        Console.WriteLine("Legalább 300 cm átmérőjű távcsövek (csökkenő sorrendben):");
        foreach (Tavcso tavcso in nagyAtmerojuTavcsovek)
        {
            Console.WriteLine($"- {tavcso.Azonosito} ({tavcso.AtmeroCm} cm)");
        }

        var radiotavcsovek = tavcsovek.OfType<Radiotavcso>().ToList();
        if (radiotavcsovek.Count > 0)
        {
            double atlagosFrekvencia = radiotavcsovek.Average(r => r.FrekvenciaMHz);
            Console.WriteLine($"Rádiótávcsövek átlagos frekvenciája: {atlagosFrekvencia:F0} MHz");
        }
        else
        {
            Console.WriteLine("Rádiótávcsövek átlagos frekvenciája: nincs adat");
        }

        string[] aktivSorok = tavcsovek
            .Where(t => t.Uzemora > 500)
            .OrderBy(t => t.Uzemora)
            .Select(t => $"{t.Azonosito};{t.AtmeroCm};{t.Uzemora};{t.FenntartasiKoltseg()}")
            .ToArray();

        File.WriteAllLines(kimenetiFajl, aktivSorok);

        Console.WriteLine();
        Console.WriteLine($"Az {kimenetiFajl} fájl sikeresen elkészült.");
        Console.WriteLine($"Távcső példányok száma (static): {Tavcso.OsszesTavcso} db");
    }
}
