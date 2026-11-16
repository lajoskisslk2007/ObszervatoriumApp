namespace ObszervatoriumApp;

public class Tavcso
{
    public string Azonosito { get; set; }
    public int AtmeroCm { get; set; }
    public int Uzemora { get; set; }

    private static int osszesTavcso = 0;
    public static int OsszesTavcso => osszesTavcso;

    public Tavcso(string azonosito, int atmeroCm, int uzemora)
    {
        Azonosito = azonosito;
        AtmeroCm = atmeroCm;
        Uzemora = uzemora;
        osszesTavcso++;
    }

    public virtual int FenntartasiKoltseg()
    {
        return AtmeroCm * 100 + Uzemora * 15;
    }

    public override string ToString()
    {
        return $"Távcső {Azonosito} | Átmérő: {AtmeroCm} cm | Üzemóra: {Uzemora} óra | Költség: {FenntartasiKoltseg()} EUR";
    }
}
