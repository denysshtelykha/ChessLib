// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ChessLib.Side;
using System;

namespace ChessLib.Board.Pieces
{
    /// <summary>
    /// Game piece
    /// </summary>
    public class Piece : IEquatable<Piece>
    {
        /// <summary>
        /// X coordinate of piece on board
        /// </summary>
        int _x;
        /// <summary>
        /// Y coordinate of piece on board
        /// </summary>
        int _y;
        /// <summary>
        /// Piece team
        /// </summary>
        TeamType _team;

        /// <summary>
        /// X coordinate of piece on board
        /// </summary>
        public int X
        {
            get => _x;
        }
        /// <summary>
        /// Y coordinate of piece on board
        /// </summary>
        public int Y
        {
            get => _y;
        }
        /// <summary>
        /// Horizontal index of piece on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')
        /// </summary>
        public char hIndex
        {
            get => Board.HorizontalIndexes[_x];
        }
        /// <summary>
        /// Vertical index of piece on board (1, 2, 3, 4, 5, 6, 7, 8)
        /// </summary>
        public int vIndex
        {
            get => _y + 1;
        }
        /// <summary>
        /// Piece team
        /// </summary>
        public TeamType Team
        {
            get => _team;
        }

        /// <summary>
        /// Create new piece with the specified coordinates and team
        /// </summary>
        /// <param name="x">X coordinate of piece on board</param>
        /// <param name="y">Y coordinate of piece on board</param>
        /// <param name="team">Piece team</param>
        public Piece(int x, int y, TeamType team)
        {
            _x = x;
            _y = y;
            _team = team;
        }
        /// <summary>
        /// Create new piece with the specified coordinates and team
        /// </summary>
        /// <param name="hIndex">Horizontal index of piece on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of piece on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Piece team</param>
        public Piece(char hIndex, int vIndex, TeamType team)
        {
            _x = Array.IndexOf(Board.HorizontalIndexes, hIndex);
            _y = vIndex - 1;
            _team = team;
        }

        /// <summary>
        /// Set new position for this piece
        /// </summary>
        /// <param name="x">X coordinate of piece on board</param>
        /// <param name="y">Y coordinate of piece on board</param>
        internal virtual void SetNewPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }
        /// <summary>
        /// Check if two pieces are equal
        /// </summary>
        /// <param name="piece">Piece instance for comparison</param>
        /// <returns></returns>
        public bool Equals(Piece piece)
        {
            if (piece == null || (_x, _y, _team) != (piece.X, piece.Y, piece.Team))
                return false;

            return true;
        }
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
}
