using System;
using System.Linq;
using System.Windows.Threading;

namespace MemoryGame.Game
{
    public class GameController
    {
        private const int REVEAL_DURATION_MS = 1000;

        enum State
        {
            WAITING_FOR_FIRST,
            WAITING_FOR_SECOND,
            REVEALING_WRONG_PAIR,
            TERMINAL
        }

        private readonly DispatcherTimer revealTimer = new DispatcherTimer();
        private readonly DispatcherTimer gameTimer = new DispatcherTimer();

        private readonly DateTime startTime;

        private GameModel game;

        private State state = State.WAITING_FOR_FIRST;
        private Cell revealedCell1;
        private Cell revealedCell2;

        public GameController( GameModel game )
        {
            this.game = game;

            revealTimer.Interval = TimeSpan.FromMilliseconds( REVEAL_DURATION_MS );
            revealTimer.Tick += OnRevealTimerTick;

            gameTimer.Interval = TimeSpan.FromSeconds( 1 );
            gameTimer.Tick += OnGameTimerTick;
            gameTimer.Start();

            startTime = DateTime.Now;
        }

        public void ClickCell( Cell cell )
        {
            if ( state == State.WAITING_FOR_FIRST )
            {
                if ( cell.State == Cell.CellState.HIDDEN )
                {
                    cell.State = Cell.CellState.REVEALED;
                    revealedCell1 = cell;
                    state = State.WAITING_FOR_SECOND;
                }
            }

            if ( state == State.WAITING_FOR_SECOND )
            {
                if ( cell.State == Cell.CellState.HIDDEN && revealedCell1 != null )
                {
                    // Match?
                    if ( cell.MatchId == revealedCell1.MatchId )
                    {
                        revealedCell1.State = Cell.CellState.SHOWN;
                        cell.State = Cell.CellState.SHOWN;
                        CheckForVictory();

                        revealedCell1 = null;
                        revealedCell2 = null;
                        state = State.WAITING_FOR_FIRST;
                    }
                    else
                    {
                        cell.State = Cell.CellState.REVEALED;

                        revealedCell2 = cell;
                        revealTimer.Start();

                        state = State.REVEALING_WRONG_PAIR;
                    }
                }
            }
        }

        private void CheckForVictory()
        {
            if ( game.Cells.All( c => c.State != Cell.CellState.HIDDEN ) )
            {
                gameTimer.Stop();
                game.IsVictory = true;
                state = State.TERMINAL;
            }
        }

        private void OnRevealTimerTick( object sender, EventArgs e )
        {
            revealTimer.Stop();

            if ( revealedCell1 == null || revealedCell2 == null )
                return;

            revealedCell1.State = Cell.CellState.HIDDEN;
            revealedCell2.State = Cell.CellState.HIDDEN;

            revealedCell1 = null;
            revealedCell2 = null;
            state = State.WAITING_FOR_FIRST;
        }

        private void OnGameTimerTick( object sender, EventArgs e )
        {
            var timeElapsed = (int)( DateTime.Now - startTime ).TotalSeconds;
            game.ElapsedTimeSeconds = timeElapsed;
        }
    }
}
