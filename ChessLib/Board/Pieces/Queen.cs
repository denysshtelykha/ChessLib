using ChessLib.Side;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// Queen
    /// </summary>
    public class Queen : Piece
    {
        /// <summary>
        /// Create new Queen with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of Queen on board</param>
        /// <param name="y">Y coordinate of Queen on board</param>
        /// <param name="team">Queen team</param>
        public Queen(int x, int y, TeamType team) : base(x, y, team) { }
        /// <summary>
        /// Create new Queen with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of Queen on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of Queen on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Queen team</param>
        public Queen(char hIndex, int vIndex, TeamType team) : base(hIndex, vIndex, team) { }
    }
}
