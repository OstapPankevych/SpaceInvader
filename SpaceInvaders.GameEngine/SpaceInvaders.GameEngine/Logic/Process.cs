using SpaceInvaders.GameEngine.Logic;
using SpaceInvaders.GameEngine.Objects;
using System;
using System.Collections.Generic;
using System.Timers;
using SpaceInvaders.GameEngine.Logic.Exceptions;


namespace SpaceInvaders.GameEngine
{
    public enum KeyPress
    {
        Left,
        Right,
        Shot,
        Wait,
        Pause,
        Restore
    }

    public enum Status
    {
        IsNeedInitialize,        
        IsRuning,
        IsPaused,
        IsExit
    }
    public class Process
    {
        #region Fields
        
        // Ostap: добавив модифікатори доступу "private", зробив поля readonly
            private const int _pause = 100;
            private readonly IDistanceStrategy _distanceStrategy;   

            private int _count=0;
            private int _enemy_posx;
            private int _enemy_posy;
            private readonly Timer _timer;                           
                         
            private readonly List<Bullet> _gunBulletList = new List<Bullet>();
            private Invader[,] _invadersArray;
            private readonly Score _score = new Score();
            private LazerGun _gun;
            private Field _playground;

        #endregion

        #region Properties

        public bool IsExit { get; set; }     
        public bool Win { get; protected set; }   // flag for ending with player's win            
        public int Score 
        { 
            get 
            { 
                return this._score.score;
            }
        }
        public Status GameStatus { get; private set; }
        public KeyPress Key { get; set; } 
              
        #endregion
        
        #region Events

        // Ostap: можна івенти Action зразу ініціалізувати, щоб потім на null не провіряти постійно
        public event Action<GameObject> Draw = delegate { };
        public event Action<Score> Show = delegate { };
        public event Action Clear = delegate { };
        public event Func<KeyPress> InputKey = delegate { return KeyPress.Wait; };

        #endregion
        
        public Process(IDistanceStrategy distanceStrategy)
        {
            IsExit = false;
            this._distanceStrategy = distanceStrategy;
            GameStatus = Status.IsNeedInitialize;
            this._timer = new Timer(100);
            this._timer.Elapsed += GameUpdate; 
        }

        // Ostap: заведи свій Exception: ApplicationException, так буде легше дебажити
        public void Init(int x, int y, int pos_x, int pos_y)
        {
            if ((x <= 0) || (y <= 0) || (pos_x <= 0) || (pos_y <= 0) || (pos_x >= x/6) || (pos_y >= y/6) || (y < (pos_y + 20)))
            {
                throw new BadInitProcessException("Game cann't be initialize with this parametrs!");
            }

            Field Playground = new Field(x, y);  //Define Size of playground
            LazerGun gun = new LazerGun(x/2, y - 1);
            this._gun = gun;
            this._playground = Playground;

            this._enemy_posx = pos_x;
            this._enemy_posy = pos_y;
            CreateEnemyArray(x, y);
            SetEnemy(this._invadersArray, y - 1, 50, pos_x, pos_y);

            GameStatus = Status.IsRuning;
            this._timer.Start();
        }

        #region Helpers
        
        public void OnDraw(GameObject gameObj)
        {
            if (Draw != null)
            {
                Draw(gameObj);
            }
        }

        public void OnShow(Score sc)
        {
            Show(sc);
        }

        public void OnClear() 
        {

            Clear();
        }

        public KeyPress OnInputKey()
        {

            return this.InputKey();                
        }

        public void UpdScore(int number)
        {
            this._score.AddScore(number);
            
        }

        public void CreateEnemyArray(int x, int y)
        {

            int i = x / 10;
            int j;
            if ((y / 8) < 3)
            {
                j = y / 8;
            }
            else
            {
                j = 3;
            }

            #region Validation

            if ((i <= 0) && (j <= 0))
            {
                throw new BadCreateEnemyException("Array can't be initialize with this parametrs!");
            }

            #endregion

            this._invadersArray = new Invader[i, j];         
        }     

        public void TryExitGame()
        {
            if (!(this._gun.Live) || (this._count > 5))
            {
                IsExit = true;
                GameStatus = Status.IsExit;
            }
        }

        public void TryChangeLevel()
        {
            if (Level())
            {
                this._count++;
                NextLevel(_count);
            }
        }
              
        public void Pause()
        {
            #region Validation

            if ((GameStatus != Status.IsRuning) && (GameStatus != Status.IsPaused))
            {
                throw new PauseException();
            }

            #endregion Validation

            GameStatus = Status.IsPaused;         
        }

        public void Restore()
        {
            #region Validation

            if ((GameStatus != Status.IsPaused) && (GameStatus != Status.IsRuning))
            {
                throw new RestoreException();
            }

            #endregion Validation

            GameStatus = Status.IsRuning;                      
        }

        public void UpdatePlayer(KeyPress key)
        {
            Bullet bull = new Bullet(this._gun.PosX, this._gun.PosY, true);

            if (key == KeyPress.Shot) 
            {
                bull.InsertBull(_gunBulletList);
            }

            this._gun.Update(key, this._playground.PosX);                                                              
            Bullet.BulletBehavior(this._gunBulletList,0);  
        }

        public void UpdateEnemy(int time)
        {
            for (var i = 0; i < this._invadersArray.GetLength(0); i++)
            {
                for (var j = 0; j < this._invadersArray.GetLength(1); j++)
                {
                    if (this._invadersArray[i, j].Live)
                    {
                        this._invadersArray[i, j].Update(time);
                    }
                }
            }
        }

        private void SetEnemy(Invader[,] arr, int x, int speed, int pos_x, int pos_y)  // set an army of invaders
        {
            int posx = pos_x;

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                int posy = pos_y;

                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Invader invader = new Invader(posx, posy, x, posx + posy);
                    invader.Speed = speed;
                    arr[i, j] = invader;
                    posy += 6;
                }
                posx += 8;
            }
        }

        private bool Level()
        {
            int count = 0;
            for (var i = 0; i < this._invadersArray.GetLength(0); i++)
            {
                for (var j = 0; j < this._invadersArray.GetLength(1); j++)
                {
                    if (this._invadersArray[i, j].Live)
                    {
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return true;
            }

            return false;
        }

        private void NextLevel(int levelNumber)
        {
            switch (levelNumber)
            {
                case (1):
                    Level(35, 3);
                    break;

                case (2):
                    Level(25, 7);
                    break;

                case (3):
                    Level(20, 9);
                    break;

                case (4):
                    Level(10, 12);
                    break;

                case (5):
                    Level(5, 15);
                    break;

                default:
                {
                    IsExit = true;
                    Win = true;
                    GameStatus = Status.IsExit;
                    break;
                }
            }
        }

        // Ostap: створи приватну функцію для твоїх levelNumber
        private void Level(int speed, int addToPos_y)
        {
            HidePlayerBullet();
            SetEnemy(this._invadersArray, this._gun.PosY, speed, this._enemy_posx, this._enemy_posy + addToPos_y); 
        }

        private void HidePlayerBullet()
        {
            this._gunBulletList.RemoveAll(b => b.Live == true);
        }

        private static long GetCurrentTime()
        {
            DateTime currentDate = DateTime.Now;
            return currentDate.Millisecond;
        }                  
                                
        #endregion
        
        public void GameUpdate(object source, ElapsedEventArgs e)
        {
            Key = OnInputKey();

            if (Key == KeyPress.Pause)
            {
                Pause();
            }
            else if (Key == KeyPress.Restore)
            {
                Restore();               
            }
            else
            {
                Update(Key);
            }

            this.Render();
        }
             
        public void Update(KeyPress key)  
        {
            if (GameStatus!= Status.IsPaused)
            {                
                TryExitGame();
                TryChangeLevel();
                                
                long Current = GetCurrentTime();

                #region Update objects

                UpdatePlayer(key);
                UpdateEnemy((int)Current);

                #endregion

                FindCollision();               
            }
        }
                               
        #region Graphic part

        public void Render()  
        {
            OnClear();
            OnDraw(this._playground);
            OnDraw(this._gun);              
                                                
            for (var i = 0; i < this._gunBulletList.Count; i++)
            {
                OnDraw(this._gunBulletList[i]);   //draw LazerGun`s bullet   
            }

            for (var i = 0; i < this._invadersArray.GetLength(0); i++)
            {
                for (var j = 0; j < this._invadersArray.GetLength(1); j++)
                {
                    if (this._invadersArray[i, j].Live)
                    {
                        OnDraw(this._invadersArray[i, j]);

                        if (this._invadersArray[i, j].IsFirstShot())
                        {
                            OnDraw(this._invadersArray[i, j].GetEnemyBullet());  // draw Invader`s bullet                             
                        }
                    }
                }
            }
            OnShow(this._score);               
    }

        #endregion

        #region ColliosionsMethod

        public void FindCollision()
        {
            for (var i = 0; i < this._invadersArray.GetLength(0); i++)
            {
                for (var j = this._invadersArray.GetLength(1) - 1; j >= 0; j--)
                {
                    if (this._invadersArray[i, j].Live)
                    {
                        CollisionLazerGun(this._invadersArray, i, j, this._gunBulletList);
                        CollisionInvader(this._invadersArray, i, j, this._gun);
                    }
                }
            }
        }

        public bool isCollision(GameObject striker, GameObject receiver)
        {
            return ((this._distanceStrategy.GetDistance(striker, receiver)) <= 0);
        }


        public bool InvaderWin(GameObject obj1, GameObject obj2)
        {
            int k = obj2.PosY - obj1.PosY;

            if (k <= 1)
            {
                return true;
            }

            return false;
        }

        public void CollisionInvader(Invader[,] enemy, int i, int j, LazerGun gun)
        {
            if (InvaderWin(enemy[i, j], gun))  // when Invader win
            {
                gun.Live = false;
            }

            if (enemy[i, j].CanShot != 0)
            {

                if (isCollision(enemy[i, j].EnemyBullet, gun))
                {
                    gun.IsDie();
                }
            }
        }


        public void CollisionLazerGun( Invader[,] enemy, int i, int j, List<Bullet> gun_bullet)
        {
            for (int b = 0; b < gun_bullet.Count; b++)
            {
                if (isCollision(enemy[i, j], gun_bullet[b]))  // when LazerGun kill an Invader
                {
                    _gunBulletList.Remove(gun_bullet[b]);
                    enemy[i, j].Live = false;
                    if (j == 2)
                    {
                        UpdScore(50);
                    }
                    else if (j == 1)
                    {
                        UpdScore(70);
                    }
                    else
                    {
                        UpdScore(100);
                    }
                }
            }
        }

        #endregion
    }
}
