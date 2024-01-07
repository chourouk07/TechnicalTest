using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class AttackController : MonoBehaviour
{
    public class PlayerCombatController : MonoBehaviour
    {
        private Weapon currentWeapon;  // Reference to the currently equipped weapon

        public void EquipWeapon(Weapon newWeapon)
        {
            currentWeapon = newWeapon;
            // Implement logic for updating player animations or other UI elements
        }

        public void Attack()
        {
            if (currentWeapon is Sword)
            {
                // Implement melee attack logic
            }
            else if (currentWeapon is Gun)
            {
                // Implement ranged attack logic
            }
        }

        public void SwitchWeapon(Weapon newWeapon)
        {
            // Implement logic for switching between melee and ranged weapons
            EquipWeapon(newWeapon);
        }
    }
}
