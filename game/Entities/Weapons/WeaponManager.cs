using game.Entities;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using game.Screens;
using Microsoft.Xna.Framework.Content;

namespace game
{
    public class WeaponManager
    {
        public int CurrentWeaponAmmo => ammo[CurrentWeapon.BulletType];

        private IWeapon CurrentWeapon => weaponList[currWeaponIndex];
        private List<IWeapon> weaponList = new List<IWeapon>();
        private int currWeaponIndex = 0;
        private Entity owner;
        private Dictionary<BulletType, int> ammo;
        private ContentManager contentManager;

        public WeaponManager(Entity owner, ContentManager contentManager)
        {
            this.owner = owner;
            this.contentManager = contentManager;

            ammo = new Dictionary<BulletType, int>()
            {
                {BulletType.Pistol, 30 },
                {BulletType.AssaultRifle, 30 }
            };
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

            if (!CurrentWeapon.Shoot())
                return;
            
            ammo[CurrentWeapon.BulletType]--;
            StaticScreenShaker.Instance.Shake(100, 2.5f);
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
