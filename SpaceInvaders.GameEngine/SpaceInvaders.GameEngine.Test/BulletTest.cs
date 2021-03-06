﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceInvaders.GameEngine.Objects;
using System.Collections.Generic;

namespace SpaceInvaders.GameEngine.Test
{
    [TestClass]
    public class BulletTest
    {
             
        [TestMethod]
        public void RemoveBullTest()
        {
            Bullet b = new Bullet(1, 0, true);
            Bullet c = new Bullet(1, 5, false);
            List<Bullet> b_l = new List<Bullet>();
            b_l.Add(b);
            b.RemoveBull(b_l,1);
            Assert.IsTrue(b_l.Count==0);

        }

        [TestMethod]
        public void RemoveEnemyBullTest()
        {
            Bullet b = new Bullet(1, 0, true);
            Bullet c = new Bullet(1, 5, false);
            List<Bullet> b_l = new List<Bullet>();
            b_l.Add(c);
            c.RemoveBull(b_l, 5);
            Assert.IsTrue(b_l.Count == 0);
        }

        [TestMethod]
        public void MoveTest()
        {
            Bullet d = new Bullet(5,5,true);
            d.Move();
            Assert.AreEqual(4,d.PosY);          
        }

        [TestMethod]
        public void DirectionTest()
        {
            Bullet d = new Bullet(5, 5, true);
        
            Assert.IsTrue(d.Direction);
        }
       

    }
}
