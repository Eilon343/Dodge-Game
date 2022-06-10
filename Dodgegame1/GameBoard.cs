using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.System.Threading;
using Windows.System;
using Windows.UI.Xaml.Core;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace Dodgegame1
{
    public class GameBoard
    {
        public int EnemyExplosionX, EnemyExplosionY;
        public int enemy_size = 20;
        public int _boardWidth;
        public int _boardHeight;
        private int numEnemies = 10;

        Random r = new Random();
        Canvas myCanvas;

        private Player pini;
        private Enemy[] enemies;
        private SpeedUp _speedUp;
        private DispatcherTimer Timer_Explosion = new DispatcherTimer();

        //building the board, powerup, player and enemies
        public GameBoard(Canvas myCanvas, int boardWidth, int boardHeight)
        {
            this.myCanvas = myCanvas;
            this._boardWidth = boardWidth;
            this._boardHeight = boardHeight;
            _speedUp = new SpeedUp(myCanvas, r.Next(30, _boardWidth - 30), r.Next(30, boardHeight - 30));
            pini = new Player(myCanvas, _boardWidth / 2 - 25, _boardHeight / 2 - 25);
            enemies = new Enemy[numEnemies];
            for (int i = 0; i < numEnemies; i++)
            {
                enemies[i] = new Enemy(myCanvas, r.Next(30, 1500), r.Next(30, 200));
            }
        }
        public Enemy[] Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }
        public Player Pini
            { get { return pini; } }
        public SpeedUp SpeedUp
            { get { return _speedUp; } }
        //method that checks if the player collided with one of the enemies
        public bool playerCollisonEnemy()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (pini.ColPosition < enemies[i].ColPosition + enemy_size && pini.ColPosition > enemies[i].ColPosition - enemy_size &&
                    pini.RowPosition < enemies[i].RowPosition + enemy_size && pini.RowPosition > enemies[i].RowPosition - enemy_size)
                //collison happend
                {
                    return true;
                }
            }
            return false;
        }
        //method to check if the player collided with the power up
        public bool PlayerCollisonPower()
        {
            if (pini.ColPosition < _speedUp.ColPosition + 50 && pini.ColPosition > _speedUp.ColPosition - 50 &&
                pini.RowPosition < _speedUp.RowPosition + 50 && pini.RowPosition > _speedUp.RowPosition - 50)
            //collisio happened
            {
                return true;
            }
            return false;
        }
        //method to check if enemy collided with other enemy
        public int EnemiesCollison()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                for (int j = 0; j < enemies.Length; j++)
                {
                    if (i != j && enemies[i].ColPosition < enemies[j].ColPosition + enemy_size
                    && enemies[i].ColPosition > enemies[j].ColPosition - enemy_size
                    && enemies[i].RowPosition < enemies[j].RowPosition + enemy_size
                   && enemies[i].RowPosition > enemies[j].RowPosition - enemy_size)
                        //collision happened
                    {
                        //saving the explosion cordinates so i can use this to display the explosion
                        EnemyExplosionX = (int)enemies[i].RowPosition;
                        EnemyExplosionY = (int)enemies[i].ColPosition;
                        return j;
                    }
                }
            }
            return -1;
        }
    }
}
