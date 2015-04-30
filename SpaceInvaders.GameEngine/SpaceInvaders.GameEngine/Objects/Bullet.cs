using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.GameEngine.Objects
{
    public class Bullet : GameObject
    {
        // Ostap: зробив поле readonly (ти напрямок ніколи не міняєш, а задаєш зразу в конструкторі)
        private readonly bool _direction; // if it`s true, bullet moves up

        // Ostap: добавив "this" до приватних полів, і забрав з пропертів
        public bool Direction
        {
            get 
            {
                return this._direction;
            }
        }
               
        public Bullet(int x, int y, bool direction)
            : base(x, y)
        {
            this._direction = direction;
        }
                        
        public void InsertBull(List<Bullet> blist)
        {
            blist.Add(this);
            Live = true;
        }

        public void RemoveBull(List<Bullet> blist,int end)
        {
            if (this._direction)
            {
                if (PosY < 5)
                {
                    blist.RemoveAt(blist.IndexOf(this));
                    Live = false;
                }
            }
            else 
            {
                if (PosY > (end - 1))
                {
                    blist.RemoveAt(blist.IndexOf(this));
                    Live = false;
                }
            } 
        }

        public static void BulletBehavior(List <Bullet> blist, int end)  // insert and remove bull from list
        {
            for (var i = 0; i < blist.Count; i++)
            {
                blist[i].RemoveBull(blist, end);
            }
                                                                                                                 
            for (var i = 0; i < blist.Count; i++)
            {
                blist[i].Move();                                    
            }                    
        }

        public void Move()
        {
            if (this._direction)
            {
                PosY--;  // LazerGun shot                
            }
            else
            {
                PosY++; // Invader shot                
            }
        }
       
    }
}