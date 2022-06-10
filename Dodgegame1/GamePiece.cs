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
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Dodgegame1
{
    public class GamePiece
    {
        public double RowPosition,ColPosition;
        public int _width;
        public int _height;
        public Canvas _canvas;
        public Rectangle _rec;

        public GamePiece(Canvas myCanvas, double RowPosition, double ColPosition, int width, int height)
        {
            this._rec = new Rectangle();
            this.RowPosition = RowPosition;
            this.ColPosition = ColPosition;
            this._width = width;
            this._height = height;
        }

    }
    public class Player : GamePiece
    {
        public Player(Canvas myCanvas,double RowPosition, double ColPosition) : base(myCanvas, RowPosition, ColPosition, 80 ,80)
        {
            this._rec = new Rectangle();
            this._canvas = myCanvas;

        }
    }
    public class Enemy : GamePiece
    {
        public Rectangle rec;
        public Enemy(Canvas myCanvas, double RowPosition, double ColPosition) : base(myCanvas,RowPosition, ColPosition, 50 ,50)
        {
            this.rec = new Rectangle();
            this._canvas = myCanvas;
        }

    }
    public class SpeedUp : GamePiece
    {
        public SpeedUp(Canvas myCanvas, double RowPosition, double ColPosition) : base(myCanvas, RowPosition, ColPosition, 30, 30)
        {

        }
    }
}
