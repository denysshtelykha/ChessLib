// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ChessLib.Board;
using ChessLib.Board.Pieces;
using ChessLib.Side;
using System;
using System.Collections.Generic;
using System.Timers;
using E = ChessLib.Engine.EngineV2;

namespace ChessLib
{
    public class Game : IDisposable
    {
        /// <summary>
        /// Team 1 instance
        /// </summary>
        private Team _team1;
        /// <summary>
        /// Team 2 instance
        /// </summary>
        private Team _team2;
        /// <summary>
        /// Game board instance
        /// </summary>
        private Board.Board _board;
        /// <summary>
        /// Timer for player turn time
        /// </summary>
        private System.Timers.Timer _moveTimer;
        /// <summary>
        /// Move timer last start
        /// </summary>
        private DateTime _moveTimerLastStart;

        public delegate void GameStartedEventHandler(object sender, GameStartedEventArgs e);
        public delegate void BoardChangedEventHandler(object sender, BoardChangedEventArgs e);
        public delegate void PawnPromoteEventHandler(object sender, PawnPromoteEventArgs e);
        public delegate void CheckEventHandler(object sender, CheckEventArgs e);
        public delegate void CheckmateEventHandler(object sender, CheckmateEventArgs e);
        public delegate void DrawEventHandler(object sender, DrawEventArgs e);
        public delegate void GameFinishedEventHandler(object sender, GameFinishedEventArgs e);

        /// <summary>
        /// Occurs when game started
        /// </summary>
        public event GameStartedEventHandler Started;
        /// <summary>
        /// Occurs when any piece on board changing position
        /// </summary>
        public event BoardChangedEventHandler BoardChanged;
        /// <summary>
        /// Occurs when any pawn reached opposite side and can be promoted
        /// </summary>
        public event PawnPromoteEventHandler PawnPromote;
        /// <summary>
        /// Occurs when one of the team received a check
        /// </summary>
        public event CheckEventHandler Check;
        /// <summary>
        /// Occurs when one of the team received a checkmate
        /// </summary>
        public event CheckmateEventHandler Checkmate;
        /// <summary>
        /// Occurs when there is a draw
        /// </summary>
        public event DrawEventHandler Draw;
        /// <summary>
        /// Occurs when game finished
        /// </summary>
        public event GameFinishedEventHandler Finished;

        /// <summary>
        /// Team 1 instance
        /// </summary>
        public Team Team1
        {
            get => _team1;
        }
        /// <summary>
        /// Team 2 instance
        /// </summary>
        public Team Team2
        {
            get => _team2;
        }
        /// <summary>
        /// Game board
        /// </summary>
        public Board.Board Board
        {
            get => _board;
        }
        /// <summary>
        /// Gets value indicating game state
        /// </summary>
        public GameState State
        { get; private set; }
        /// <summary>
        /// Gets value indicating which team turn now
        /// </summary>
        public TeamType Turn
        { get; private set; }
        /// <summary>
        /// Gets value indicating time for player move in miliseconds
        /// </summary>
        public long MoveTime => (long)_moveTimer.Interval;
        /// <summary>
        /// Move time left in miliseconds
        /// </summary>
        public long MoveTimeLeft => (long)(_moveTimer.Interval - (DateTime.Now - _moveTimerLastStart).TotalMilliseconds);

        /// <summary>
        /// Create new game instance
        /// </summary>
        public Game()
        {
            // Initialize board
            _board = new Board.Board();
        }

        /// <summary>
        /// Initialize new game with given parameters
        /// </summary>
        /// <param name="player1Name">Player 1 name</param>
        /// <param name="player1Team">Player 1 team</param>
        /// <param name="player2Name">Player 2 name</param>
        /// <param name="player2Team">Player 2 team</param>
        /// <returns></returns>
        public void New(string player1Name, TeamType player1Team, string player2Name, TeamType player2Team, int moveTimeSec)
        {
            // Initialize teams
            _team1 = new Team(TeamType.WHITE);
            _team2 = new Team(TeamType.BLACK);
            //Link pieces to board cells
            for (int i = 0; i < 16; i++)
            {
                _board[_team1[i].X, _team1[i].Y].SetPiece(_team1[i]);
                _board[_team2[i].X, _team2[i].Y].SetPiece(_team2[i]);
            }
            this.BoardChanged?.Invoke(this, new BoardChangedEventArgs(_board));
            // Initialize timer
            _moveTimer = new Timer(moveTimeSec * 1000);
            _moveTimer.Elapsed += moveTimer_Callback;
            // Game state
            State = GameState.NOT_STARTED;
        }
        /// <summary>
        /// Start new game
        /// </summary>
        public void Start()
        {
            if (State == GameState.NOT_STARTED)
            {
                State = GameState.STARTED;
                Turn = TeamType.WHITE;
                _moveTimer.Start();
                _moveTimerLastStart = DateTime.Now;
                this.Started?.Invoke(this, new GameStartedEventArgs());
            }
            else
                throw new Exception("Game already started or finished");
        }
        /// <summary>
        /// Process player turn
        /// </summary>
        /// <param name="activeTeam">Active team</param>
        /// <param name="notActiveTeam">Not active team</param>
        /// <param name="piece">Piece</param>
        /// <param name="cell">Destination cell</param>
        private bool Process(Team activeTeam, Team notActiveTeam, Piece piece, Cell cell)
        {
            if (!E.IsMoveValid(piece, cell, _board))              // Check if move is valide
                return false;                                     // If not => return false
            //------------------------------------------------------------------------------------------------------------------------------
            List<Cell> freeCells = E.PieceCanMove(piece, _board); // Get all free(empty) cells on which can be moved given piece       
            if (freeCells.Contains(cell))                         // If list contain destination cell, then process simple move on empty cell
                E.Move(piece, cell, _board);                      // Move to empty cell
            else                                                  // Else, capture enemy piece 
            {
                notActiveTeam.DeadPieces.Add(cell.Piece);         // Add enemy piece to dead pieces collection
                notActiveTeam.Pieces.Remove(cell.Piece);          // Remove enemy piece from main collection
                E.Move(piece, cell, _board);                      // Set new position for piece  
            }
            //------------------------------------------------------------------------------------------------------------------------------
            if (piece is Pawn && E.PieceCanBePromoted(piece, _board))                              // Check if piece is Pawn and can be promoted
                this.PawnPromote?.Invoke(this, new PawnPromoteEventArgs((Pawn)piece));             // Notify that Pawn can be promoted
            //------------------------------------------------------------------------------------------------------------------------------
            this.BoardChanged?.Invoke(this, new BoardChangedEventArgs(_board));                    // Notify that board is changed 
            //------------------------------------------------------------------------------------------------------------------------------
            var (check, pieces) = E.IsCheck(notActiveTeam, _board);                                // Check if not active team got check
            if (check)
            {
                this.Check?.Invoke(this, new CheckEventArgs(notActiveTeam));                       // Notify that team got check      
                if (E.IsCheckMate(notActiveTeam, _board))                                          // Check if not active team got checkmate
                {
                    this.Checkmate?.Invoke(this, new CheckmateEventArgs(notActiveTeam));           // Notify that team got checkmate
                    this.State = GameState.FINISHED;                                               // Update state
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------
            return true;
        }
        /// <summary>
        /// Process player turn
        /// </summary>
        /// <param name="piece">Piece</param>
        /// <param name="cell">Destination cell</param>
        public bool Process(Piece piece, Cell cell)
        {
            _moveTimer.Stop();
            if (this.State != GameState.STARTED || piece.Team != this.Turn)
                return false;
            if (piece.Team == TeamType.WHITE)
                this.Process(_team1, _team2, piece, cell);
            else
                this.Process(_team2, _team1, piece, cell);

            this.Turn = this.Turn == TeamType.WHITE ? TeamType.BLACK : TeamType.WHITE;
            _moveTimer.Start();
            _moveTimerLastStart = DateTime.Now;
            return true;
        }
        /// <summary>
        /// Get free(empty) cells on which can be moved given piece, depending on the situation on the board
        /// </summary>
        /// <param name="piece">Selected piece</param>
        /// <returns></returns>
        public List<Cell> PeaceCanMove(Piece piece)
        {
            // If game not started or not team turn
            if (this.State != GameState.STARTED || piece == null || piece.Team != this.Turn)
                return new List<Cell>();

            return E.PieceCanMove(piece, _board);
        }
        /// <summary>
        /// Get cells which can be captured by current piece, depending on the situation on the board
        /// </summary>
        /// <param name="piece">Selected piece</param>
        /// <returns></returns>
        public List<Cell> PeaceCanCapture(Piece piece)
        {
            // If game not started or not piece team turn
            if (this.State != GameState.STARTED || piece.Team != this.Turn)
                return new List<Cell>();

            return E.PieceCanCapture(piece, _board);
        }
        /// <summary>
        /// Timer for player turn time callback
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void moveTimer_Callback(object source, ElapsedEventArgs e)
        {
            this.State = GameState.FINISHED;
            this.Finished?.Invoke(this, new GameFinishedEventArgs(this.Turn == TeamType.WHITE ? _team2 : _team1));
        }

        public void Dispose()
        {
            _moveTimer.Dispose();
        }
    }

    public class BoardChangedEventArgs
    {
        public Board.Board board;
        public BoardChangedEventArgs(Board.Board board)
        {
            this.board = board;
        }
    }
    public class GameStartedEventArgs { }
    public class PawnPromoteEventArgs
    {
        public Pawn pawn;
        public PawnPromoteEventArgs(Pawn pawn)
        {
            this.pawn = pawn;
        }

    }
    public class CheckEventArgs
    {
        public Team team;
        public CheckEventArgs(Team team)
        {
            this.team = team;
        }
    }
    public class CheckmateEventArgs
    {
        public Team team;
        public CheckmateEventArgs(Team team)
        {
            this.team = team;
        }
    }
    public class DrawEventArgs { }
    public class GameFinishedEventArgs
    {
        public Team winnerTeam;
        public GameFinishedEventArgs(Team team)
        {
            this.winnerTeam = team;
        }
    }

    /// <summary>
    /// Represent game state
    /// </summary>
    public enum GameState
    {
        NOT_STARTED,
        STARTED,
        FINISHED,
    }
}
