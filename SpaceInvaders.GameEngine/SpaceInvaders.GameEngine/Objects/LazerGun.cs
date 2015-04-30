
namespace SpaceInvaders.GameEngine.Objects
{
    public class LazerGun : GameObject
    {
        // Ostap: зробив byte, бо життя ніколи не будуть "-"  і збереже від багів
        private byte _numberOfLive = 3;


        public int NumberOfLives
        {
            get
            {
                return this._numberOfLive;
            }
        }

        #region Constructor
       
        public LazerGun(int x, int y)
            : base(x, y)
        {
            Live = true;
        }
        #endregion

        #region Methods
        public void MoveRight()
        {
            PosX++;
        }
        public void MoveLeft()
        {
            PosX--;
        }

        public void Update(KeyPress key, int endField)
        {
            if (this.Live)
            {
                if ((key == KeyPress.Right) && (PosX < (endField - 4)))
                {
                    MoveRight();
                }
                else if ((key == KeyPress.Left) && (PosX > 2))
                {
                    MoveLeft();
                }
            }
        }                 
        
        // Ostap: всі методи і проперті з великої літери!
        public void IsDie()
        {            
            if (Live)
            {
                this._numberOfLive--;                
            }

            if(this._numberOfLive < 1)
            {
                Live = false;              
            }
        }

        #endregion

    }
}
