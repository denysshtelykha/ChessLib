using ChessLib.Side;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// Knight
    /// </summary>
    public class Knight : Piece
    {
        /// <summary>
        /// Create new Knight with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of Knight on board</param>
        /// <param name="y">Y coordinate of Knight on board</param>
        /// <param name="team">Knight team</param>
        public Knight(int x, int y, TeamType team) : base(x, y, team) { }
        /// <summary>
        /// Create new Knight with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of Knight on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of Knight on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Knight team</param>
        public Knight(char hIndex, int vIndex, TeamType team) : base(hIndex, vIndex, team) { }
    }
}
