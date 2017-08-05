using System;
using System.Collections.Generic;

namespace MemoryGame
{
    public class Cell
    {
        public enum CellState
        {
            HIDDEN,
            REVEALED,   // Temporarily shown
            SHOWN       // Permanently shown
        };

        private CellState state;

        public int MatchId { get; private set; }
        public event Action<CellState> StateChanged;

        public CellState State
        {
            get { return state; }
            set
            {
                state = value;
                StateChanged?.Invoke( value );
            }
        }

        public Cell( int matchId )
        {
            MatchId = matchId;
            state = CellState.HIDDEN;
        }
    }

    public class GameModel
    {
        private static readonly Random random = new Random( Guid.NewGuid().GetHashCode() );

        private int elapsedTimeSeconds;
        private bool isVictory;

        public List<Cell> Cells { get; private set; }
        public int Dimension { get; private set; }

        public event Action<int> TimeChanged;
        public event Action<bool> IsVictoryChanged;

        public int ElapsedTimeSeconds
        {
            get { return elapsedTimeSeconds; }
            set { elapsedTimeSeconds = value; TimeChanged?.Invoke( value ); }
        }

        public bool IsVictory
        {
            get { return isVictory; }
            set { isVictory = value; IsVictoryChanged?.Invoke( value ); }
        }

        // Warning: Dimension^2 must be even number, otherwise the game is unwinnable.
        public GameModel( int dimension )
        {
            Dimension = dimension;

            InitCells();
        }

        private void InitCells()
        {
            var count = Dimension * Dimension;

            // Generate an array of id's that has two instances of each id
            var idList = new List<int>( count );
            for ( int i = 0; i < count; ++i )   // I miss Kotlin here ;(
                idList.Add( i / 2 );

            Cells = new List<Cell>( count );

            for ( int i = 0; i < count; i++ )
            {
                // Pick a random id. Not the best way performance-wise. TODO: optimize maybe?
                var idIndex = random.Next( idList.Count );
                var id = idList[idIndex];
                idList.RemoveAt( idIndex );

                Cells.Add( new Cell( id ) );
            }
        }
    }
}
