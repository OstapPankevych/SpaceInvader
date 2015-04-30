using SpaceInvaders.GameEngine.Objects.Base;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.GameEngine.Objects
{
    public abstract class GameObject : IGameObject
    {

        #region Field and Properties

        private int _posx;   // position of objects
        private int _posy;      
       
           
        public int PosX 
        {
            get { return this._posx; }
            protected set { this._posx = value; }
        } 
        public int PosY 
        {
            get { return this._posy; }
            protected set { this._posy = value; }
        }
      
        public bool Live { get; set; }
                                     
        #endregion
        
        #region Constructor
        public GameObject(int x, int y)
        {
            PosX = x;
            PosY = y;            
            Live =true;
        }
        #endregion              
          
    }
}
