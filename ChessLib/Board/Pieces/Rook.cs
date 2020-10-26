using ChessLib.Side;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// Rook
    /// </summary>
    public class Rook : Piece
    {
        /// <summary>
        /// Rook first move done
        /// </summary>
        private bool _firstMovementDone = false;
        /// <summary>
        /// Gets value indicating whether the Rook already made the first move or not
        /// </summary>
        public bool IsFirstMovementDone
        {
            get => _firstMovementDone;
        }
        /// <summary>
        /// Create new Rook with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of Rook on board</param>
        /// <param name="y">Y coordinate of Rook on board</param>
        /// <param name="team">Rook team</param>
        public Rook(int x, int y, TeamType team) : base(x, y, team) { }
        /// <summary>
        /// Create new Rook with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of Rook on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of Rook on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Rook team</param>
        public Rook(char hIndex, int vIndex, TeamType team) : base(hIndex, vIndex, team) { }
        /// <summary>
        /// Set new position for this Rook
        /// </summary>
        /// <param name="x">X coordinate of Rook on board</param>
        /// <param name="y">Y coordinate of Rook on board</param>
        internal override void SetNewPosition(int x, int y)
        {
            base.SetNewPosition(x, y);
            _firstMovementDone = true;
        }
    }
}
