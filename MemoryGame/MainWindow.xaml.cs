using System.Windows;
using MemoryGame.Game;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            btnNewGame.Click += OnBtnNewgameClicked;

            slBoardSize.ValueChanged += ( sender, args ) => tbBoardSizeText.Text = $"{slBoardSize.Value * 2}x{slBoardSize.Value * 2}";
            slBoardSize.Value = 2;
        }

        private void OnBtnNewgameClicked( object sender, RoutedEventArgs routedEventArgs )
        {
            int size = (int) slBoardSize.Value * 2;

            var game = new GameModel( size );
            var controller = new GameController( game );
            var view = new BoardView( game, controller, ImageLoader.Instance );

            gridBoard.Children.Clear();
            gridBoard.Children.Add( view );

            tbVictory.Visibility = Visibility.Hidden;
            tbTimer.Text = "00:00";

            game.TimeChanged += time => tbTimer.Text = $"{time / 60 :00}:{time % 60 :00}";
            game.IsVictoryChanged += isVictory => { if ( isVictory ) tbVictory.Visibility = Visibility.Visible; };
        }
    }
}
