namespace ObszervatoriumApp;

public class Radiotavcso : Tavcso
{
    public int FrekvenciaMHz { get; set; }

    public Radiotavcso(string azonosito, int atmeroCm, int uzemora, int frekvenciaMHz)
        : base(azonosito, atmeroCm, uzemora)
    {
        FrekvenciaMHz = frekvenciaMHz;
    }

    public override int FenntartasiKoltseg()
    {
        return base.FenntartasiKoltseg() + FrekvenciaMHz * 2;
    }

    public override string ToString()
    {
        return $"{base.ToString()} | Frekvencia: {FrekvenciaMHz} MHz";
    }
}
