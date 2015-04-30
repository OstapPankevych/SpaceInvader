using System.Collections.Generic;

namespace SpaceInvaders.GameEngine.Objects
{
    public class Invader : GameObject
    {
        #region Field and Properties

        // Ostap: _recall - зробити byte: 1) ніколи не буде від'ємним 2) економиш пам'ять
        private byte _recall = 1; // count how many times we update obj        
        public int K { get; set; }
        public int EndOfField { get; set; } // use for bullet behavior
        public int Speed { get; set; }

        // Ostap: ти ініціалізуєш _enemyBullet зразу, тому роби readonly!
        private readonly List<Bullet> _enemyBullet = new List<Bullet>();

        public int CanShot 
        { 
            get { return this._enemyBullet.Count; }
        }

        public Bullet EnemyBullet 
        {
            get { return this._enemyBullet[0]; }             
        }

        #endregion


        #region Constructor

        public Invader(int x, int y, int endOfField, int randomShot)
            : base(x, y)
        {
            EndOfField = endOfField;
            K = randomShot;
            Live = true;
            Speed = 1;
        }

        #endregion


        #region Methods
        public void Move()
        {
            this.PosY++;
        }

        public bool Shot(int time)
        {
            if ((time * 0.5) % 52 == 0)
            {
                return true;
            }
            return false;
        }
     
        private bool EnemyCanShot()
         {
            if ((K % 7 == 0) && (this._recall % 7 == 0))
            {
                return true;
            }
            else if ((K % 2 == 0) && (this._recall % 10 == 0))
            {
                return true;
            }
            else if ((K % 4 == 0) && (this._recall % 5 == 0))
            {
                return true;
            }
            else if ((K % 9 == 0) && (this._recall % 2 == 0))
            {
                return true;
            }

            return false;
        }
       
        public void Update(int time)
        {
            _recall++;
            Bullet b = new Bullet(PosX, PosY, false);

                if (Shot(time) && EnemyCanShot() && this._enemyBullet.Count == 0)
                {
                    b.InsertBull(_enemyBullet);
                    Bullet.BulletBehavior(this._enemyBullet, EndOfField);                            
                }
            
                if (this._enemyBullet.Count != 0)
                {
                    Bullet.BulletBehavior(this._enemyBullet, EndOfField);
                }

                if (this._recall % Speed == 0)
                {
                    Move();
                }
            }

        public bool IsFirstShot()
        {
            if (this._enemyBullet.Count != 0)
            {
                return true;
            }
            return false;
        }

        public Bullet GetEnemyBullet()
        {
            return this._enemyBullet[0];
        }

        #endregion


    }
}
