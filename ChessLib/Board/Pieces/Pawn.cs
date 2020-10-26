using ChessLib.Side;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// Pawn
    /// </summary>
    public class Pawn : Piece
    {
        /// <summary>
        /// Pawn first move done
        /// </summary>
        private bool _firstMovementDone = false;
        /// <summary>
        /// Gets value indicating whether the pawn already made the first move or not
        /// </summary>
        public bool IsFirstMovementDone
        {
            get => _firstMovementDone;
        }
        /// <summary>
        /// Create new Pawn with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of Pawn on board</param>
        /// <param name="y">Y coordinate of Pawn on board</param>
        /// <param name="team">Pawn team</param>
        public Pawn(int x, int y, TeamType team) : base(x, y, team) { }
        /// <summary>
        /// Create new Pawn with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of Pawn on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of Pawn on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Pawn team</param>
        public Pawn(char hIndex, int vIndex, TeamType team) : base(hIndex, vIndex, team) { }
        /// <summary>
        /// Set new position for this Pawn
        /// </summary>
        /// <param name="x">X coordinate of Pawn on board</param>
        /// <param name="y">Y coordinate of Pawn on board</param>
        internal override void SetNewPosition(int x, int y)
        {
            base.SetNewPosition(x, y);
            _firstMovementDone = true;
        }
    }
}
