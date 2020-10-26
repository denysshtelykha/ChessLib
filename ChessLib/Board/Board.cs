// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChessLib.Board
{
    /// <summary>
    /// Represent game board
    /// </summary>
    public class Board : IEnumerable<Cell>
    {
        /// <summary>
        /// Width of game board
        /// </summary>
        public const int WIDTH = 8;
        /// <summary>
        /// Height of game board
        /// </summary>
        public const int HEIGHT = 8;
        /// <summary>
        /// Horizontal indexes of board
        /// </summary>
        private static readonly char[] _horizontalIndexes = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        /// <summary>
        /// Matrix of cells 
        /// </summary>
        Cell[,] _board;

        /// <summary>
        /// Horizontal indexes of board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')
        /// </summary>
        public static char[] HorizontalIndexes
        {
            get => _horizontalIndexes;
        }

        /// <summary>
        /// Create new game board instance
        /// </summary>
        public Board()
        {
            _board = new Cell[8, 8];

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    _board[i, j] = new Cell(i, j);
                }
            }
        }
        /// <summary>
        /// Create new game board instance based on given
        /// </summary>
        /// <param name="board">Board instance</param>
        public Board(Board board) 
        {
            _board = new Cell[8, 8];

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    _board[i, j] = new Cell(i, j);
                    _board[i, j].SetPiece(board[i, j].Piece);
                }
            }
        }
        /// <summary>
        /// Indexer [0,0] - [7,7]
        /// </summary>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <returns></returns>
        public Cell this[int x, int y]
        {
            get
            {
                if (x >= 0 && y >= 0 && x < WIDTH && y < HEIGHT)
                    return _board[x, y];
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Indexer ['a',1] - ['h',8]
        /// </summary>
        /// <param name="wIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="hIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <returns></returns>
        public Cell this[char wIndex, int hIndex]
        {
            get
            {
                if (hIndex >= 1 && hIndex <= HEIGHT && _horizontalIndexes.Contains(wIndex))
                    return _board[Array.IndexOf(_horizontalIndexes, wIndex), hIndex - 1];
                throw new IndexOutOfRangeException();
            }
        }     

        public IEnumerator<Cell> GetEnumerator()
        {
            foreach (var square in _board)
            {
                yield return square;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
