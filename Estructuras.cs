
public struct Info_Zomb
{
    public string sabor;
    public string apodo;
    public int años;
}
public struct Info_Ald
{
    public string apodo;
    public int años;

    static public implicit operator Info_Zomb(Info_Ald Ald)
    {
        Info_Zomb undead = new Info_Zomb();
        undead.sabor = "Cerebros";
        undead.años = Ald.años;
        undead.apodo = "Zombie " + Ald.apodo;
        return undead;
    }
}
