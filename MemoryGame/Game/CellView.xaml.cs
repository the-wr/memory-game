using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MemoryGame.Game
{
    public partial class CellView : UserControl
    {
        private const int ANIMATION_DURATION_MS = 200;

        private readonly Cell cell;

        private Cell.CellState prevState = Cell.CellState.HIDDEN;

        public CellView( Cell cell, FrameworkElement content )
        {
            InitializeComponent();

            this.cell = cell;

            gridContent.Children.Add( content );
            cell.StateChanged += (state) => UpdateState();

            UpdateState();
        }

        private void UpdateState()
        {
            if ( cell.State == Cell.CellState.HIDDEN )
            {
                if ( prevState != Cell.CellState.HIDDEN )
                    scale.BeginAnimation( ScaleTransform.ScaleXProperty, new DoubleAnimation { From = scale.ScaleX, To = 0, Duration = TimeSpan.FromMilliseconds( ANIMATION_DURATION_MS ) } );

                rectBackground.Fill = Brushes.LightGoldenrodYellow;
            }

            if ( cell.State == Cell.CellState.REVEALED )
            {
                scale.BeginAnimation( ScaleTransform.ScaleXProperty, new DoubleAnimation { From = scale.ScaleX, To = 1, Duration = TimeSpan.FromMilliseconds( ANIMATION_DURATION_MS ) }  );

                rectBackground.Fill = Brushes.Orange;
            }

            if ( cell.State == Cell.CellState.SHOWN )
            {
                scale.BeginAnimation( ScaleTransform.ScaleXProperty, new DoubleAnimation { From = scale.ScaleX, To = 1, Duration = TimeSpan.FromMilliseconds( ANIMATION_DURATION_MS ) } );

                rectBackground.Fill = Brushes.LimeGreen;
            }

            prevState = cell.State;
        }
    }
}
