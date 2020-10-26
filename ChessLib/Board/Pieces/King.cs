using ChessLib.Side;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// King
    /// </summary>
    public class King : Piece
    {
        /// <summary>
        /// King first move done
        /// </summary>
        private bool _firstMovementDone = false;
        /// <summary>
        /// Gets value indicating whether the King already made the first move or not
        /// </summary>
        public bool IsFirstMovementDone
        {
            get => _firstMovementDone;
        }
        /// <summary>
        /// Create new King with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of King on board</param>
        /// <param name="y">Y coordinate of King on board</param>
        /// <param name="team">King team</param>
        public King(int x, int y, TeamType team) : base(x, y, team) { }
        /// <summary>
        /// Create new King with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of King on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of King on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">King team</param>
        public King(char hIndex, int vIndex, TeamType team) : base(hIndex, vIndex, team) { }
        /// <summary>
        /// Set new position for this King
        /// </summary>
        /// <param name="x">X coordinate of King on board</param>
        /// <param name="y">Y coordinate of King on board</param>
        internal override void SetNewPosition(int x, int y)
        {
            base.SetNewPosition(x, y);
            _firstMovementDone = true;
        }
    }
}
