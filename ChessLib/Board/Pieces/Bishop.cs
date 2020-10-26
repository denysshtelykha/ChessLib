using ChessLib.Side;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// Bishop
    /// </summary>
    public class Bishop : Piece
    {
        /// <summary>
        /// Create new Bishop with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of Bishop on board</param>
        /// <param name="y">Y coordinate of Bishop on board</param>
        /// <param name="team">Bishop team</param>
        public Bishop(int x, int y, TeamType team) : base(x, y, team) { }
        /// <summary>
        /// Create new Bishop with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of Bishop on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of Bishop on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Bishop team</param>
        public Bishop(char hIndex, int vIndex, TeamType team) : base(hIndex, vIndex, team) { }
    }
}
