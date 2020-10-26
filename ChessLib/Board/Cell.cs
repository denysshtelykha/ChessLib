// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ChessLib.Board.Pieces;
using System;

namespace ChessLib.Board
{
    /// <summary>
    /// Represent board cell
    /// </summary>
    public class Cell : IEquatable<Cell>
    {
        /// <summary>
        /// X coordinate of cell on board
        /// </summary>
        readonly int _x;
        /// <summary>
        /// Y coordinate of cell on board
        /// </summary>
        readonly int _y;
        /// <summary>
        /// Piece on this cell
        /// </summary>
        Piece _piece;

        /// <summary>
        /// X coordinate of cell on board
        /// </summary>
        public int X
        {
            get => _x;
        }
        /// <summary>
        /// Y coordinate of cell on board
        /// </summary>
        public int Y
        {
            get => _y;
        }
        /// <summary>
        /// Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')
        /// </summary>
        public char hIndex
        {
            get => Board.HorizontalIndexes[_x];
        }
        /// <summary>
        /// Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)
        /// </summary>
        public int vIndex
        {
            get => _y + 1;
        }
        /// <summary>
        /// Gets Piece instance which is linked to this cell
        /// </summary>
        public Piece Piece
        {
            get => _piece;
        }
        /// <summary>
        /// Gets value indicating whether the cell is empty (no piece on this cell) 
        /// </summary>
        public bool IsEmpty
        {
            get => _piece == null ? true : false;
        }
        /// <summary>
        /// Gets value indicating cell color (Light/Dark)
        /// </summary>
        public CellColor Color
        {
            get => (_x + _y) % 2 != 0 ? CellColor.LIGHT : CellColor.DARK;
        }

        /// <summary>
        /// Create new board cell with the specified coordinates and linked piece
        /// </summary>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <param name="piece">Piece for linked to this cell</param>
        public Cell(int x, int y, Piece piece)
        {
            _x = x;
            _y = y;
            _piece = piece;
        }
        /// <summary>
        /// Create new board cell with the specified coordinates
        /// </summary>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        public Cell(int x, int y) : this(x, y, null) { }
        /// <summary>
        /// Create new board cell with the specified coordinates and linked piece
        /// </summary>
        /// <param name="wIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="hIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="piece">Piece for linked to this cell</param>
        public Cell(char wIndex, int hIndex, Piece piece)
        {
            _x = Array.IndexOf(Board.HorizontalIndexes, wIndex);
            _y = hIndex - 1;
            _piece = piece;
        }
        /// <summary>
        /// Create new board cell with the specified coordinates
        /// </summary>
        /// <param name="wIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="hIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        public Cell(char wIndex, int hIndex) : this(wIndex, hIndex, null) { }
        /// <summary>
        /// Link new piece to this cell
        /// </summary>
        /// <param name="piece">Piece for linked to this cell</param>
        internal void SetPiece(Piece piece)
        {
            _piece = piece;
        }
        /// <summary>
        /// Remove piece from this cell
        /// </summary>
        internal void RemovePiece()
        {
            _piece = null;
        }
        /// <summary>
        /// Check if two cell are equal
        /// </summary>
        /// <param name="cell">Cell instance for comparison</param>
        /// <returns></returns>
        public bool Equals(Cell cell)
        {
            if (cell == null || (_x, _y, _piece) != (cell.X, cell.Y, cell.Piece))
                return false;

            return true;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    /// <summary>
    /// Represent cell color
    /// </summary>
    public enum CellColor
    {
        LIGHT,
        DARK
    }
}
