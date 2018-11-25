using game.Entities;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using game.Screens;

namespace game
{
    public class WeaponManager
    {
        public int CurrentWeaponAmmo => ammo[CurrentWeapon.BulletType];

        private IWeapon CurrentWeapon => weaponList[currWeaponIndex];
        private List<IWeapon> weaponList = new List<IWeapon>();
        private int currWeaponIndex = 0;
        private Entity owner;
        private Dictionary<BulletType, int> ammo = new Dictionary<BulletType, int>()
        {
            {BulletType.Pistol, 30 },
            {BulletType.AssaultRifle, 30 }
        };

        public WeaponManager(Entity owner)
        {
            this.owner = owner;
        }

        public void NextWeapon()
        {
            if (currWeaponIndex + 1 > weaponList.Count - 1)
                currWeaponIndex = 0;
            else
                currWeaponIndex++;
        }

        public void PreviousWeapon()
        {
            if (currWeaponIndex - 1 < 0)
                currWeaponIndex = weaponList.Count - 1;
            else
                currWeaponIndex--;
        }

        public void SwitchWeapon(int index)
        {
            if (index < 0 || index >= weaponList.Count)
                throw new IndexOutOfRangeException("Weapon index out of range");

            currWeaponIndex = index;
        }

        public void AddWeapon(IWeapon weapon)
        {
            weaponList.Add(weapon);
            weapon.Owner = owner;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            CurrentWeapon.Draw(spriteBatch, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            CurrentWeapon.Update(gameTime);
        }

        public void ShootCurrentWeapon()
        {
            if (ammo[CurrentWeapon.BulletType] <= 0)
                return;

            if (CurrentWeapon.Shoot())
            {
                ammo[CurrentWeapon.BulletType]--;
                StaticScreenShaker.Instance.Shake(100, 5);
            }
        }

        public void AddAmmo(BulletType type, int amount)
        {
            if (!ammo.ContainsKey(type))
                throw new Exception("That ammo doesn't exist in our dictionary");
            else
                ammo[type] += amount;
        }
    }
}
