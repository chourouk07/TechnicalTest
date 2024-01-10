public interface IWeapon
{
    public string WeaponName{ get; }
    bool ActivateWeapon(WeaponSwitch weaponSwitch);
}
