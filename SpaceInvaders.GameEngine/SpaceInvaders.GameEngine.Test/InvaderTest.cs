﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceInvaders.GameEngine.Objects;

namespace SpaceInvaders.GameEngine.Test
{
    [TestClass]
    public class InvaderTest
    {
        Invader inv = new Invader(2,3,20,9);
        [TestMethod]
        public void ConstructorTest()
        {
            Assert.AreEqual(2,inv.PosX);

        }

        [TestMethod]
        public void MoveTest()
        {
            inv.Move();
            Assert.AreEqual(4, inv.PosY);

        }

        [TestMethod]
        public void ShotTest()
        {
            Assert.IsFalse(inv.Shot(10));
            Assert.IsFalse(!inv.Shot(208));
        }


        [TestMethod]
        public void UpdateTest()
        {
            inv.Speed = 1;
            inv.Update(10000);
            Assert.IsTrue(inv.enem_bullet.Count==0);
        }

        [TestMethod]
        public void firstShotTest()
        {            
            Assert.IsFalse(inv.firstShot());
        }

         [TestMethod]
       public void UpdateTrueTest()
        {
            inv.Speed = 1;
            inv.Update(208);
            inv.Update(208);
            Assert.IsTrue(inv.enem_bullet.Count != 0);              
         } 

         [TestMethod]
         public void firstShotTrueTest()
         {
             Bullet b = new Bullet(1,2,false);
             inv.Speed = 1;
             inv.enem_bullet.Add(b);
             Assert.IsTrue(inv.firstShot());
         }


         [TestMethod]
         public void canShotTrueTest()
         {
             inv.Speed = 1;

             for (var i = 0; i <= 9; i++)
             {

                 if (i == 3)
                 {
                     inv.K = 4;
                     inv.Update(416);                     
                     Assert.IsTrue(inv.enem_bullet.Count != 0);
                 }
                 else if(i==5)
                 {
                     inv.K = 7;
                     inv.Update(416);                   
                     Assert.IsTrue(inv.enem_bullet.Count != 0);
                 }
                 else if (i == 8)
                 {
                     inv.K = 2;
                     inv.Update(416);                     
                     Assert.IsTrue(inv.enem_bullet.Count != 0);
                 }
                 else 
                 {
                     inv.Update(208);
                 }

             }
         }

         [TestMethod]
         public void GetBulletTest()
         {
             Bullet b = new Bullet(inv.PosX, inv.PosY, false);
             inv.Speed = 1;
             inv.enem_bullet.Add(b);
             int x;
             int y;
             string s;

             inv.GetBullet(out s,out x, out y);

             Assert.AreEqual("Bullet",s );
             Assert.AreEqual(inv.PosX, x );
             Assert.AreEqual(inv.PosY, y);
         }

    }
}