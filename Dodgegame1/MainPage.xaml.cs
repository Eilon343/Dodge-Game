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
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.UI.Core;

namespace Dodgegame1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Rect windowRectangle = ApplicationView.GetForCurrentView().VisibleBounds;
        //variables
        private bool is_Pause_pressed;
        private int player_lives = 3;
        private int enemy_counter;
        private double enemyspeed;
        private int score;
        private int numenemies = 10;
        private int PlayerSpeed = 10;

        //calling music manager and creating the images of the explosions
        MusicManager _musicmanager = new MusicManager();
        List<Image> ExplosionList = new List<Image>();
        Image ExplosionImage = new Image();

        //timers
        private DispatcherTimer Timer_Explosion = new DispatcherTimer();
        private DispatcherTimer Timer_Powers = new DispatcherTimer();
        private DispatcherTimer timer = new DispatcherTimer();

        //giving names the the board,player,enemies and the power up
        GameBoard board;
        Rectangle pini;
        Rectangle[] enemies;
        Rectangle _speedUp;

        //command bar buttons
        private AppBarButton newGameBtn;
        private AppBarButton pauseGameBtn;
        private AppBarButton resumeGameBtn;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Random r = new Random();
            //Game timer
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;
            //Explosion Timer
            Timer_Explosion.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            Timer_Explosion.Tick += Timer_Tick_Explosion;
            //speeduppower timer
            Timer_Powers.Interval = new TimeSpan(0, 0, 0, r.Next(3, 10), 0);
            Timer_Powers.Tick += Timer_Powers_Tick;
            //Background picture
            myCanvas.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/space2.png"))
            };
            this.newGameBtn = New;
            this.pauseGameBtn = Pause;
            this.resumeGameBtn = Resume;
        }
        //checking if the game is running with the board (when the board is null the game hasnt started yet).
        //I made this method because when i tried to move the player before the game started the game crashed beacause there was
        //no player to move so i gave the moving method a condition that it will move only when the game is running.
        private bool is_game_running()
        {
            if (board != null)
                return true;
            return false;
        }
        //timer for the power to spawn somewhere between 3-10 seconds from the start of the game
        private void Timer_Powers_Tick(object sender, object e)
        {
            _speedUp = addNewRectangle(board.SpeedUp);
            _speedUp.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/speedboosr.png"))
            };
            //stoping the timer because i want only one boost to spawn
            Timer_Powers.Stop();
        }
        //a method i found online that made moving the player with keyboard possible
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.Left:
                    if (is_game_running() == true)
                    {
                        if (Canvas.GetLeft(pini) <= PlayerSpeed)
                            return;
                        Canvas.SetLeft(pini, Canvas.GetLeft(pini) - PlayerSpeed);
                        board.Pini.RowPosition = (int)Canvas.GetLeft(pini) - PlayerSpeed;
                    }
                    break;

                case VirtualKey.Right:
                    if (is_game_running() == true)
                    {
                        if (Canvas.GetLeft(pini) + PlayerSpeed + pini.Width > myCanvas.ActualWidth)
                            return;
                        Canvas.SetLeft(pini, Canvas.GetLeft(pini) + PlayerSpeed);
                        board.Pini.RowPosition = (int)Canvas.GetLeft(pini) + PlayerSpeed;
                    }
                    break;

                case VirtualKey.Up:
                    if (is_game_running() == true)
                    {

                        if (Canvas.GetTop(pini) <= PlayerSpeed)
                            return;
                        Canvas.SetTop(pini, Canvas.GetTop(pini) - PlayerSpeed);
                        board.Pini.ColPosition = (int)Canvas.GetTop(pini) - PlayerSpeed;
                    }
                    break;

                case VirtualKey.Down:
                    if (is_game_running() == true)
                    {
                        if (Canvas.GetTop(pini) + PlayerSpeed + pini.Height > myCanvas.ActualHeight)
                            return;
                        Canvas.SetTop(pini, Canvas.GetTop(pini) + PlayerSpeed);
                        board.Pini.ColPosition = (int)Canvas.GetTop(pini) + PlayerSpeed;
                    }
                    break;
            }
        }
        // a method to tell enemies to chase after the player
        public void ChasePini()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (board.Pini.ColPosition < board.Enemies[i].ColPosition)
                {
                    Canvas.SetTop(enemies[i], Canvas.GetTop(enemies[i]) - enemyspeed);
                    board.Enemies[i].ColPosition = Canvas.GetTop(enemies[i]) - enemyspeed;
                }
                if (board.Pini.ColPosition > board.Enemies[i].ColPosition)
                {
                    Canvas.SetTop(enemies[i], Canvas.GetTop(enemies[i]) + enemyspeed);
                    board.Enemies[i].ColPosition = Canvas.GetTop(enemies[i]) + enemyspeed;
                }
                if (board.Pini.RowPosition < board.Enemies[i].RowPosition)
                {
                    Canvas.SetLeft(enemies[i], Canvas.GetLeft(enemies[i]) - enemyspeed);
                    board.Enemies[i].RowPosition = Canvas.GetLeft(enemies[i]) - enemyspeed;
                }
                if (board.Pini.RowPosition > board.Enemies[i].RowPosition)
                {
                    Canvas.SetLeft(enemies[i], Canvas.GetLeft(enemies[i]) + enemyspeed);
                    board.Enemies[i].RowPosition = Canvas.GetLeft(enemies[i]) + enemyspeed;
                }
            }
        }
        //method to start the game
        private void StartGame()
        {
            Difficuly();
            //reseting the text's of the score and lives
            HeadLine.Text = "";
            Instructions.Text = "";
            //creating the board, player, enemies and fill them with pictures
            board = new GameBoard(myCanvas, (int)windowRectangle.Width, (int)windowRectangle.Height);
            pini = addNewRectangle(board.Pini);
            pini.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/spaceship.png"))
            };
            enemies = new Rectangle[numenemies];
            for (int i = 0; i < board.Enemies.Length; i++)
            {
                Enemy currentEnemy = board.Enemies[i];
                //making sure that the enemies wont spawn on the player position
                if (board.Pini.ColPosition != board.Enemies[i].ColPosition && board.Pini.RowPosition != board.Enemies[i].RowPosition)
                {
                    enemies[i] = addNewRectangle(currentEnemy);
                    enemies[i].Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/asteroid.png"))
                    };
                }
            }
            enemy_counter = numenemies;
            PlayerSpeed = 10;
            //starting the timers
            timer.Start();
            Timer_Explosion.Start();
            ButtonsWhilePlaying();
        }
        private async void Timer_Tick(object sender, object e)
        {
            //Writing the texts in the timer so they will update in game
            DifficultyTxt.Text = "";
            ScoreTxt.Text = "Score: " + score;
            lifextext.Text = "Lives: " + player_lives;
            if (board.EnemiesCollison() != -1)
            {
                //with every enemy collison enemies getting a bit faster
                enemyspeed += 0.1;
                DisplayExplosion();
                score += 150;
                //playing the explosion sound
                await _musicmanager.PlayExplosionSound();
                enemy_counter--;
                myCanvas.Children.Remove(enemies[board.EnemiesCollison()]);
                //i had a problem with removing the enemies where the program only removed 1 enemy so i found this on the internet and it solved it
                enemies = enemies.Except(new Rectangle[] { enemies[board.EnemiesCollison()] }).ToArray();
                board.Enemies = board.Enemies.Except(new Enemy[] { board.Enemies[board.EnemiesCollison()] }).ToArray();
            }
            if (board.playerCollisonEnemy() == true)
            {
                await _musicmanager.PlayLostLife();
                score = 0;
                player_lives--;
                timer.Stop();
                Timer_Explosion.Stop();
                await new MessageDialog("You Collided with enemy, Lifes Left: " + player_lives).ShowAsync();
                Start_Over();
            }
            if (board.PlayerCollisonPower() == true)
            {
                myCanvas.Children.Remove(_speedUp);
                PlayerSpeed += 5;
            }
            if (PlayerSpeed > 15)
                PlayerSpeed = 15;
            ChasePini();
            Win();
            if (player_lives < 1)
                GameOver();
        }
        //a method to start over the game
        private void Start_Over()
        {
            if (player_lives == 2 || player_lives == 1)
            {
                Timer_Powers.Start();
                enemy_counter = numenemies;
                ClearBoard();
                StartGame();
            }
        }
        //a method that checks if the player lost
        private async void GameOver()
        {
            _musicmanager.StopBackGroundMusic();
            ClearBoard();
            ButtonsWhileGameOver();
            timer.Stop();
            Timer_Explosion.Stop();
            Timer_Powers.Stop();
            await _musicmanager.PlayLosingSound();
            HeadLine.Text = "Game Over, You Lost";
            Instructions.Text = "You can start a new game by Choosing difficulty and pressing new game";
        }
        //method to add new rectangles
        public Rectangle addNewRectangle(GamePiece piece)
        {
            Rectangle rectangle = new Rectangle();
            if (piece is Player)
                rectangle.Fill = new SolidColorBrush(Colors.Green);
            else
                rectangle.Fill = new SolidColorBrush(Colors.Red);
            rectangle.Width = piece._width;
            rectangle.Height = piece._height;
            Canvas.SetLeft(rectangle, piece.RowPosition);
            Canvas.SetTop(rectangle, piece.ColPosition);
            myCanvas.Children.Add(rectangle);
            return rectangle;
        }
        //method to check if the player won 
        private async void Win()
        {
            if (enemy_counter <= 1)
            {
                ButtonsWhileGameOver();
                _musicmanager.StopBackGroundMusic();
                await _musicmanager.PlayVictorySound();
                timer.Stop();
                Timer_Explosion.Stop();
                ClearBoard();
                HeadLine.Text = "Congratulations, You Won";
                Instructions.Text = "You can start a new game by Choosing difficulty and pressing new game";
            }
        }
        private void ButtonsWhileGameOver() //buttons i want to return when the game is over
        {
            DifficultyTxt.Text = "Choose Difficulty:";
            Difficulty.Visibility = Visibility.Visible;
            pauseGameBtn.Visibility = Visibility.Collapsed;
            resumeGameBtn.Visibility = Visibility.Collapsed;
            newGameBtn.Visibility = Visibility.Visible;
        }
        private void ButtonsWhilePlaying() //buttons i want to remove when the game is running
        {
            Difficulty.Visibility = Visibility.Collapsed;
            pauseGameBtn.Visibility = Visibility.Visible;
            resumeGameBtn.Visibility = Visibility.Visible;
            newGameBtn.Visibility = Visibility.Collapsed;
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            is_Pause_pressed = true;
            timer.Stop();
            PlayerSpeed = 0;
            _musicmanager.StopBackGroundMusic();
        }

        private async void Resume_Click(object sender, RoutedEventArgs e)
        {
            //i made this boolean beacause i had a problem when i pressed multiple times at resume the music was played over and over
            //so i solved it with the boolean.
            if (is_Pause_pressed == true)
            {
                await _musicmanager.Playbackgroundmusic();
                is_Pause_pressed = false;
            }
            timer.Start();
            PlayerSpeed = 10;
        }

        private async void New_Click(object sender, RoutedEventArgs e)
        {
            //if the player hasnt chose any difficulty a messege will pop up that tells him to choose one
            if (Difficulty.SelectedItem == null)
            {
                await new MessageDialog("Please select difficulty.").ShowAsync();

            }
            //if the player chose a difficulty then the game is starting
            if (Difficulty.SelectedItem != null)
            {
                SettingForNewGame();
                StartGame();
                await _musicmanager.Playbackgroundmusic();
            }
        }
        private void DisplayExplosion()
        {
            //method to define the explosion
            Image ExplosionImage = new Image();
            Uri uri = new Uri("ms-appx:///Assets/explosion.gif");
            ExplosionImage.Source = new BitmapImage(uri);
            //size of the explosion
            ExplosionImage.Height = 100;
            ExplosionImage.Width = 100;
            //position of the explosion
            Canvas.SetLeft(ExplosionImage, board.EnemyExplosionX);
            Canvas.SetTop(ExplosionImage, board.EnemyExplosionY);
            //add the explosion to the canvas
            myCanvas.Children.Add(ExplosionImage);
            ExplosionList.Add(ExplosionImage);
        }
        //setting the Explosion with timer and removing the explosion so it will not stay forever on the screen
        private void Timer_Tick_Explosion(object sender, object e)
        {
            foreach (var Explosion in ExplosionList)
            {
                //Set the position of the Explosion
                Canvas.SetLeft(Explosion, board.EnemyExplosionX);
                Canvas.SetTop(Explosion, board.EnemyExplosionY);
                myCanvas.Children.Remove(Explosion);
            }
        }
        private void ClearBoard()
        {
            //removing enemies, explosions, player and power up
            for (int i = 0; i < enemies.Length; i++)
                myCanvas.Children.Remove(enemies[i]);
            myCanvas.Children.Remove(pini);
            foreach (var explotion in ExplosionList)
                myCanvas.Children.Remove(explotion);
            RemoveSpeedUpIfNotTaken();
        }
        //reset the basic settings every time a new game begin
        private void SettingForNewGame()
        {
            _musicmanager.StopVictorySound();
            _musicmanager.StopLosingSound();
            player_lives = 3;
            score = 0;
            enemy_counter = 10;
        }
        private void Difficuly()
        {
            //changing the player and enemies speed according to the player difficulty choise.
            if (Difficulty.SelectedIndex == 0)
            {
                //if player speed is normal it means that the player didnt took the speed up power so im removing it
                if (player_lives == 2 && player_lives == 1)
                    enemyspeed = 1.5;
                PlayerSpeed = 10;
                enemyspeed = 1.3;
            }
            if (Difficulty.SelectedIndex == 1)
            {
                //if player loses life im adding a bit to the speed
                if (player_lives == 2 && player_lives == 1)
                    enemyspeed = 2.2;
                PlayerSpeed = 8;
                enemyspeed = 2;
            }
            if (Difficulty.SelectedIndex == 2)
            {
                //if player loses life im adding a bit to the speed
                if (player_lives == 2 && player_lives == 1)
                    enemyspeed = 3.2;
                PlayerSpeed = 7;
                enemyspeed = 3;
            }
        }
        //a method to remove the power up from the board if the player hasnt took it
        private void RemoveSpeedUpIfNotTaken()
        {
            if (player_lives < 3 && PlayerSpeed == PlayerSpeed)
            {
                myCanvas.Children.Remove(_speedUp);
            }
        }
    }
}