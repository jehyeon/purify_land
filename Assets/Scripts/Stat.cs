public enum UnitCode
{
    Player,
    WoodBlock,
    Monster
}

public class Stat
{
    // ��� ������Ƽ
    public UnitCode unitCode { get; }
    public string name { get; set; }
    public int hp { get; set; }
    public int maxHp { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public float speed { get; set; }

    public Stat()
    {
        // !!! Temp
        hp = 100;
        maxHp = 100;
    }

    public Stat(UnitCode unitCode, string name, int hp, int maxHp, int attack, int defense, float speed)
    {
        this.unitCode = unitCode;
        this.name = name;
        this.hp = hp;
        this.maxHp = maxHp;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
    }

    public Stat SetUnitStat(UnitCode unitCode)
    {
        Stat stat = null;

        switch (unitCode)
        {
            case UnitCode.Player:
                stat = new Stat(unitCode, "�÷��̾�", 100, 100, 10, 10, 7);
                break;
            case UnitCode.Monster:
                stat = new Stat(unitCode, "����", 30, 30, 5, 0, 5);
                break;
            case UnitCode.WoodBlock:
                stat = new Stat(unitCode, "��������", 4, 4, 0, 65535, 0f);
                break;
        }

        return stat;
    }
}
