public class Weapon
{
    public int Damage { get; protected set; }
}

public class Sword : Weapon
{
    // Implement specific properties/methods for swords
}

public class Gun : Weapon
{
    // Implement specific properties/methods for guns
}
