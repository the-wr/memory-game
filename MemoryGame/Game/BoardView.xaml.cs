using System.Windows.Controls;

namespace MemoryGame.Game
{
    public partial class BoardView : UserControl
    {
        public BoardView( GameModel game, GameController controller, IAssetProvider assetProvider )
        {
            InitializeComponent();

            for ( int i = 0; i < game.Dimension; i++ )
            {
                gridCells.ColumnDefinitions.Add( new ColumnDefinition() );
                gridCells.RowDefinitions.Add( new RowDefinition() );
            }

            for ( int i = 0; i < game.Dimension; ++i )
            {
                for ( int k = 0; k < game.Dimension; k++ )
                {
                    int cellIndex = i * game.Dimension + k;
                    var cell = game.Cells[cellIndex];

                    var cellView = new CellView( cell, assetProvider.GetAssetForId( cell.MatchId ) );
                    Grid.SetRow( cellView, i );
                    Grid.SetColumn( cellView, k );

                    cellView.MouseDown += delegate { controller.ClickCell( cell ); };

                    gridCells.Children.Add( cellView );
                }
            }
        }
    }
}
