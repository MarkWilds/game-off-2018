using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class WeaponManager
    {
        public IWeapon CurrentWeapon => weaponList[currWeaponIndex];

        private List<IWeapon> weaponList = new List<IWeapon>();
        private int currWeaponIndex = 0;
        private Entity owner;

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
            if (InputManager.IsKeyPressed(Keys.E))
                NextWeapon();
            if (InputManager.IsKeyPressed(Keys.Q))
                PreviousWeapon();

            CurrentWeapon.Update(gameTime);
        }
    }
}
