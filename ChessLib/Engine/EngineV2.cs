using ChessLib.Board;
using ChessLib.Board.Pieces;
using ChessLib.Side;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLib.Engine
{
    public static class EngineV2
    {
        #region Validate move
        /// <summary>
        /// Gets value indicating whether the move is valid or not
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="cell">Cell instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the move is valid or not</returns>
        public static bool IsMoveValid(Piece piece, Cell cell, Board.Board board)
        {
            if (PieceCanMove(piece, board).Contains(cell) || PieceCanCapture(piece, board).Contains(cell))
                return true;
            return false;
        }
        /// <summary>
        /// Gets value indicating whether the move is valid or not
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the move is valid or not</returns>
        public static bool IsMoveValid(Piece piece, int x, int y, Board.Board board) => IsMoveValid(piece, board[x, y], board);
        /// <summary>
        /// Gets value indicating whether the move is valid or not
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="hIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the move is valid or not</returns>
        public static bool IsMoveValid(Piece piece, char hIndex, int vIndex, Board.Board board) => IsMoveValid(piece, board[hIndex, vIndex], board);
        #endregion

        #region Get accessible cells for piece
        /// <summary>
        /// Simulate piece move
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <param name="board">Game board</param>
        /// <returns></returns>
        private static bool simulatePieceMove(Piece piece, int x, int y, Board.Board board)//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            var tmpBoard = new Board.Board(board);
            tmpBoard[piece.X, piece.Y].RemovePiece();
            //Object newPiece = Activator.CreateInstance(piece.GetType(), new Object[] { x, y, piece.Team });
            object newPiece = Activator.CreateInstance(piece.GetType(), new Object[] { x, y, piece.Team });
            
            //if (piece is King)
            //    newPiece = new King(x, y, piece.Team);
            //else
            //    newPiece = new Piece(x, y, piece.Team);
            tmpBoard[x, y].SetPiece((Piece)newPiece);
            var (check, pieces) = IsCheck(piece.Team, tmpBoard);
            return check;
        }
        /// <summary>
        /// Get free(empty) cells on which can be moved given piece
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="board">Game board</param>
        /// <returns>List of cells on which can be moved given piece</returns>
        public static List<Cell> PieceCanMove(Piece piece, Board.Board board)
        {
            List<Cell> freeCells = new List<Cell>();
            switch (piece)
            {
                case Pawn p:
                    if (p.Team == TeamType.WHITE)
                    {
                        // UP 1
                        if (p.Y + 1 < Board.Board.HEIGHT && board[p.X, p.Y + 1].IsEmpty)
                            freeCells.Add(board[p.X, p.Y + 1]);
                        // UP 2
                        if (!p.IsFirstMovementDone)
                        {
                            if (p.Y + 2 < Board.Board.HEIGHT && board[p.X, p.Y + 2].IsEmpty)
                                freeCells.Add(board[p.X, p.Y + 2]);
                        }
                    }
                    else
                    {
                        // DOWN 1
                        if (p.Y - 1 < Board.Board.HEIGHT && board[p.X, p.Y - 1].IsEmpty)
                            freeCells.Add(board[p.X, p.Y - 1]);
                        // DOWN 2
                        if (!p.IsFirstMovementDone)
                        {
                            if (p.Y - 2 < Board.Board.HEIGHT && board[p.X, p.Y - 2].IsEmpty)
                                freeCells.Add(board[p.X, p.Y - 2]);
                        }
                    }
                    break;
                case Rook r:
                    // UP multiple
                    for (int y = r.Y + 1; y < Board.Board.HEIGHT; y++)
                    {
                        if (board[r.X, y].IsEmpty)
                            freeCells.Add(board[r.X, y]);
                        else
                            break;
                    }
                    // DOWN multiple
                    for (int y = r.Y - 1; y >= 0; y--)
                    {
                        if (board[r.X, y].IsEmpty)
                            freeCells.Add(board[r.X, y]);
                        else
                            break;
                    }
                    // RIGHT multiple
                    for (int x = r.X + 1; x < Board.Board.WIDTH; x++)
                    {
                        if (board[x, r.Y].IsEmpty)
                            freeCells.Add(board[x, r.Y]);
                        else
                            break;
                    }
                    // LEFT multiple
                    for (int x = r.X - 1; x >= 0; x--)
                    {
                        if (board[x, r.Y].IsEmpty)
                            freeCells.Add(board[x, r.Y]);
                        else
                            break;
                    }
                    break;
                case Knight k:
                    // Y
                    // ^ . + * .
                    // . . + + . 
                    // . . K + .
                    // . . . . .
                    // . . . . > X
                    if (k.X + 1 < Board.Board.WIDTH && k.Y + 2 < Board.Board.HEIGHT && board[k.X + 1, k.Y + 2].IsEmpty)
                        freeCells.Add(board[k.X + 1, k.Y + 2]);
                    // Y
                    // ^ . . . .
                    // . . + + * 
                    // . . K + +
                    // . . . . .
                    // . . . . > X
                    if (k.X + 2 < Board.Board.WIDTH && k.Y + 1 < Board.Board.HEIGHT && board[k.X + 2, k.Y + 1].IsEmpty)
                        freeCells.Add(board[k.X + 2, k.Y + 1]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // . . K + +
                    // . . + + *
                    // . . . . > X
                    if (k.X + 2 < Board.Board.WIDTH && k.Y - 1 >= 0 && board[k.X + 2, k.Y - 1].IsEmpty)
                        freeCells.Add(board[k.X + 2, k.Y - 1]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // . . K + .
                    // . . + + .
                    // . . + * > X
                    if (k.X + 1 < Board.Board.WIDTH && k.Y - 2 >= 0 && board[k.X + 1, k.Y - 2].IsEmpty)
                        freeCells.Add(board[k.X + 1, k.Y - 2]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // . + K . .
                    // . + + . .
                    // . * + . > X
                    if (k.X - 1 >= 0 && k.Y - 2 >= 0 && board[k.X - 1, k.Y - 2].IsEmpty)
                        freeCells.Add(board[k.X - 1, k.Y - 2]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // + + K . .
                    // * + + . .
                    // . . . . > X
                    if (k.X - 2 >= 0 && k.Y - 1 >= 0 && board[k.X - 2, k.Y - 1].IsEmpty)
                        freeCells.Add(board[k.X - 2, k.Y - 1]);
                    // Y
                    // ^ . . . .
                    // * + + . . 
                    // + + K . .
                    // . . . . .
                    // . . . . > X
                    if (k.X - 2 >= 0 && k.Y + 1 < Board.Board.HEIGHT && board[k.X - 2, k.Y + 1].IsEmpty)
                        freeCells.Add(board[k.X - 2, k.Y + 1]);
                    // Y
                    // ^ * + . .
                    // . + + . . 
                    // . + K . .
                    // . . . . .
                    // . . . . > X
                    if (k.X - 1 >= 0 && k.Y + 2 < Board.Board.HEIGHT && board[k.X - 1, k.Y + 2].IsEmpty)
                        freeCells.Add(board[k.X - 1, k.Y + 2]);
                    break;
                case Bishop b:
                    // UP RIGHT multiple
                    for ((int x, int y) = (b.X + 1, b.Y + 1); x < Board.Board.WIDTH && y < Board.Board.HEIGHT; x++, y++)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // DOWN LEFT multiple
                    for ((int x, int y) = (b.X - 1, b.Y - 1); x >= 0 && y >= 0; x--, y--)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // DOWN RIGHT multiple
                    for ((int x, int y) = (b.X + 1, b.Y - 1); x < Board.Board.WIDTH && y >= 0; x++, y--)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // UP LEFT multiple
                    for ((int x, int y) = (b.X - 1, b.Y + 1); x >= 0 && y < Board.Board.HEIGHT; x--, y++)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    break;
                case Queen q:
                    // UP RIGHT multiple
                    for ((int x, int y) = (q.X + 1, q.Y + 1); x < Board.Board.WIDTH && y < Board.Board.HEIGHT; x++, y++)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // DOWN LEFT multiple
                    for ((int x, int y) = (q.X - 1, q.Y - 1); x >= 0 && y >= 0; x--, y--)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // DONN RIGHT multiple
                    for ((int x, int y) = (q.X + 1, q.Y - 1); x < Board.Board.WIDTH && y >= 0; x++, y--)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // UP LEFT multiple
                    for ((int x, int y) = (q.X - 1, q.Y + 1); x >= 0 && y < Board.Board.HEIGHT; x--, y++)
                    {
                        if (board[x, y].IsEmpty)
                            freeCells.Add(board[x, y]);
                        else
                            break;
                    }
                    // UP multiple
                    for (int y = q.Y + 1; y < Board.Board.HEIGHT; y++)
                    {
                        if (board[q.X, y].IsEmpty)
                            freeCells.Add(board[q.X, y]);
                        else
                            break;
                    }
                    // DOWN multiple
                    for (int y = q.Y - 1; y >= 0; y--)
                    {
                        if (board[q.X, y].IsEmpty)
                            freeCells.Add(board[q.X, y]);
                        else
                            break;
                    }
                    // RIGHT multiple
                    for (int x = q.X + 1; x < Board.Board.WIDTH; x++)
                    {
                        if (board[x, q.Y].IsEmpty)
                            freeCells.Add(board[x, q.Y]);
                        else
                            break;
                    }
                    // LEFT multiple
                    for (int x = q.X - 1; x >= 0; x--)
                    {
                        if (board[x, q.Y].IsEmpty)
                            freeCells.Add(board[x, q.Y]);
                        else
                            break;
                    }
                    break;
                case King kg:
                    //UP 1
                    if (kg.Y + 1 < Board.Board.HEIGHT && board[kg.X, kg.Y + 1].IsEmpty && !simulatePieceMove(kg, kg.X, kg.Y + 1, board))
                        freeCells.Add(board[kg.X, kg.Y + 1]);
                    //DOWN 1
                    if (kg.Y - 1 >= 0 && board[kg.X, kg.Y - 1].IsEmpty && !simulatePieceMove(kg, kg.X, kg.Y - 1, board))
                        freeCells.Add(board[kg.X, kg.Y - 1]);
                    //LEFT 1
                    if (kg.X - 1 >= 0 && board[kg.X - 1, kg.Y].IsEmpty && !simulatePieceMove(kg, kg.X - 1, kg.Y, board))
                        freeCells.Add(board[kg.X - 1, kg.Y]);
                    //RIGHT 1
                    if (kg.X + 1 < Board.Board.WIDTH && board[kg.X + 1, kg.Y].IsEmpty && !simulatePieceMove(kg, kg.X + 1, kg.Y, board))
                        freeCells.Add(board[kg.X + 1, kg.Y]);
                    //UP RIGHT 1
                    if (kg.X + 1 < Board.Board.WIDTH && kg.Y + 1 < Board.Board.HEIGHT && board[kg.X + 1, kg.Y + 1].IsEmpty && !simulatePieceMove(kg, kg.X + 1, kg.Y + 1, board))
                        freeCells.Add(board[kg.X + 1, kg.Y + 1]);
                    //UP LEFT 1
                    if (kg.X - 1 >= 0 && kg.Y + 1 < Board.Board.HEIGHT && board[kg.X - 1, kg.Y + 1].IsEmpty && !simulatePieceMove(kg, kg.X - 1, kg.Y + 1, board))
                        freeCells.Add(board[kg.X - 1, kg.Y + 1]);
                    //DOWN RIGHT 1
                    if (kg.X + 1 < Board.Board.WIDTH && kg.Y - 1 >= 0 && board[kg.X + 1, kg.Y - 1].IsEmpty && !simulatePieceMove(kg, kg.X + 1, kg.Y - 1, board))
                        freeCells.Add(board[kg.X + 1, kg.Y - 1]);
                    //DOWN LEFT 1
                    if (kg.X - 1 >= 0 && kg.Y - 1 >= 0 && board[kg.X - 1, kg.Y - 1].IsEmpty && !simulatePieceMove(kg, kg.X - 1, kg.Y - 1, board))
                        freeCells.Add(board[kg.X - 1, kg.Y - 1]);
                    break;
                default:
                    throw new PieceTypeException("Unknow piece type", piece);
            }

            for (int i = freeCells.Count - 1; i >= 0; i--)
            {
                if (simulatePieceMove(piece, freeCells[i].X, freeCells[i].Y, board))
                    freeCells.RemoveAt(i);
            }

            return freeCells;
        }
        /// <summary>
        /// Get cells which can be captured by current piece
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="board">Game board</param>
        /// <returns>List of cells on which can be moved given piece</returns>
        public static List<Cell> PieceCanCapture(Piece piece, Board.Board board)
        {
            List<Cell> capCells = new List<Cell>();
            switch (piece)
            {
                case Pawn p:
                    if (p.Team == TeamType.WHITE)
                    {
                        // UP LEFT 1
                        if (p.X - 1 >= 0 && p.Y + 1 < Board.Board.HEIGHT && !board[p.X - 1, p.Y + 1].IsEmpty &&
                        board[p.X - 1, p.Y + 1].Piece.Team != p.Team)
                            capCells.Add(board[p.X - 1, p.Y + 1]);
                        // UP RIGHT 1
                        if (p.X + 1 < Board.Board.WIDTH && p.Y + 1 < Board.Board.HEIGHT && !board[p.X + 1, p.Y + 1].IsEmpty &&
                            board[p.X + 1, p.Y + 1].Piece.Team != p.Team)
                            capCells.Add(board[p.X + 1, p.Y + 1]);
                    }
                    else
                    {
                        // DOWN LEFT 1
                        if (p.X - 1 >= 0 && p.Y - 1 < Board.Board.HEIGHT && !board[p.X - 1, p.Y - 1].IsEmpty &&
                        board[p.X - 1, p.Y - 1].Piece.Team != p.Team)
                            capCells.Add(board[p.X - 1, p.Y - 1]);
                        // DOWN RIGHT 1
                        if (p.X + 1 < Board.Board.WIDTH && p.Y - 1 < Board.Board.HEIGHT && !board[p.X + 1, p.Y - 1].IsEmpty &&
                            board[p.X + 1, p.Y - 1].Piece.Team != p.Team)
                            capCells.Add(board[p.X + 1, p.Y - 1]);
                    }
                    break;
                case Rook r:
                    // UP multiple
                    for (int y = r.Y + 1; y < Board.Board.HEIGHT; y++)
                    {
                        if (!board[r.X, y].IsEmpty)
                        {
                            if (board[r.X, y].Piece.Team != r.Team)
                                capCells.Add(board[r.X, y]);
                            break;
                        }
                    }
                    // DOWN multiple
                    for (int y = r.Y - 1; y >= 0; y--)
                    {
                        if (!board[r.X, y].IsEmpty)
                        {
                            if (board[r.X, y].Piece.Team != r.Team)
                                capCells.Add(board[r.X, y]);
                            break;
                        }
                    }
                    // RIGHT multiple
                    for (int x = r.X + 1; x < Board.Board.WIDTH; x++)
                    {
                        if (!board[x, r.Y].IsEmpty)
                        {
                            if (board[x, r.Y].Piece.Team != r.Team)
                                capCells.Add(board[x, r.Y]);
                            break;
                        }
                    }
                    // LEFT multiple
                    for (int x = r.X - 1; x >= 0; x--)
                    {
                        if (!board[x, r.Y].IsEmpty)
                        {
                            if (board[x, r.Y].Piece.Team != r.Team)
                                capCells.Add(board[x, r.Y]);
                            break;
                        }
                    }
                    break;
                case Knight k:
                    // Y
                    // ^ . + * .
                    // . . + + . 
                    // . . K + .
                    // . . . . .
                    // . . . . > X
                    if (k.X + 1 < Board.Board.WIDTH && k.Y + 2 < Board.Board.HEIGHT &&
                        !board[k.X + 1, k.Y + 2].IsEmpty && board[k.X + 1, k.Y + 2].Piece.Team != k.Team)
                        capCells.Add(board[k.X + 1, k.Y + 2]);
                    // Y
                    // ^ . . . .
                    // . . + + * 
                    // . . K + +
                    // . . . . .
                    // . . . . > X
                    if (k.X + 2 < Board.Board.WIDTH && k.Y + 1 < Board.Board.HEIGHT &&
                        !board[k.X + 2, k.Y + 1].IsEmpty && board[k.X + 2, k.Y + 1].Piece.Team != k.Team)
                        capCells.Add(board[k.X + 2, k.Y + 1]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // . . K + +
                    // . . + + *
                    // . . . . > X
                    if (k.X + 2 < Board.Board.WIDTH && k.Y - 1 >= 0 &&
                        !board[k.X + 2, k.Y - 1].IsEmpty && board[k.X + 2, k.Y - 1].Piece.Team != k.Team)
                        capCells.Add(board[k.X + 2, k.Y - 1]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // . . K + .
                    // . . + + .
                    // . . + * > X
                    if (k.X + 1 < Board.Board.WIDTH && k.Y - 2 >= 0 &&
                        !board[k.X + 1, k.Y - 2].IsEmpty && board[k.X + 1, k.Y - 2].Piece.Team != k.Team)
                        capCells.Add(board[k.X + 1, k.Y - 2]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // . + K . .
                    // . + + . .
                    // . * + . > X
                    if (k.X - 1 >= 0 && k.Y - 2 >= 0 &&
                        !board[k.X - 1, k.Y - 2].IsEmpty && board[k.X - 1, k.Y - 2].Piece.Team != k.Team)
                        capCells.Add(board[k.X - 1, k.Y - 2]);
                    // Y
                    // ^ . . . .
                    // . . . . . 
                    // + + K . .
                    // * + + . .
                    // . . . . > X
                    if (k.X - 2 >= 0 && k.Y - 1 >= 0 &&
                        !board[k.X - 2, k.Y - 1].IsEmpty && board[k.X - 2, k.Y - 1].Piece.Team != k.Team)
                        capCells.Add(board[k.X - 2, k.Y - 1]);
                    // Y
                    // ^ . . . .
                    // * + + . . 
                    // + + K . .
                    // . . . . .
                    // . . . . > X
                    if (k.X - 2 >= 0 && k.Y + 1 < Board.Board.HEIGHT &&
                        !board[k.X - 2, k.Y + 1].IsEmpty && board[k.X - 2, k.Y + 1].Piece.Team != k.Team)
                        capCells.Add(board[k.X - 2, k.Y + 1]);
                    // Y
                    // ^ * + . .
                    // . + + . . 
                    // . + K . .
                    // . . . . .
                    // . . . . > X
                    if (k.X - 1 >= 0 && k.Y + 2 < Board.Board.HEIGHT &&
                        !board[k.X - 1, k.Y + 2].IsEmpty && board[k.X - 1, k.Y + 2].Piece.Team != k.Team)
                        capCells.Add(board[k.X - 1, k.Y + 2]);
                    break;
                case Bishop b:
                    // UP RIGHT multiple
                    for ((int x, int y) = (b.X + 1, b.Y + 1); x < Board.Board.WIDTH && y < Board.Board.HEIGHT; x++, y++)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != b.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // DOWN LEFT multiple
                    for ((int x, int y) = (b.X - 1, b.Y - 1); x >= 0 && y >= 0; x--, y--)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != b.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // DOWN RIGHT multiple
                    for ((int x, int y) = (b.X + 1, b.Y - 1); x < Board.Board.WIDTH && y >= 0; x++, y--)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != b.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // UP LEFT multiple
                    for ((int x, int y) = (b.X - 1, b.Y + 1); x >= 0 && y < Board.Board.HEIGHT; x--, y++)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != b.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    break;
                case Queen q:
                    // UP RIGHT multiple
                    for ((int x, int y) = (q.X + 1, q.Y + 1); x < Board.Board.WIDTH && y < Board.Board.HEIGHT; x++, y++)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != q.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // DOWN LEFT multiple
                    for ((int x, int y) = (q.X - 1, q.Y - 1); x >= 0 && y >= 0; x--, y--)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != q.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // DONN RIGHT multiple
                    for ((int x, int y) = (q.X + 1, q.Y - 1); x < Board.Board.WIDTH && y >= 0; x++, y--)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != q.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // UP LEFT multiple
                    for ((int x, int y) = (q.X - 1, q.Y + 1); x >= 0 && y < Board.Board.HEIGHT; x--, y++)
                    {
                        if (!board[x, y].IsEmpty)
                        {
                            if (board[x, y].Piece.Team != q.Team)
                                capCells.Add(board[x, y]);
                            break;
                        }
                    }
                    // UP multiple
                    for (int y = q.Y + 1; y < Board.Board.HEIGHT; y++)
                    {
                        if (!board[q.X, y].IsEmpty)
                        {
                            if (board[q.X, y].Piece.Team != q.Team)
                                capCells.Add(board[q.X, y]);
                            break;
                        }
                    }
                    // DOWN multiple
                    for (int y = q.Y - 1; y >= 0; y--)
                    {
                        if (!board[q.X, y].IsEmpty)
                        {
                            if (board[q.X, y].Piece.Team != q.Team)
                                capCells.Add(board[q.X, y]);
                            break;
                        }
                    }
                    // RIGHT multiple
                    for (int x = q.X + 1; x < Board.Board.WIDTH; x++)
                    {
                        if (!board[x, q.Y].IsEmpty)
                        {
                            if (board[x, q.Y].Piece.Team != q.Team)
                                capCells.Add(board[x, q.Y]);
                            break;
                        }
                    }
                    // LEFT multiple
                    for (int x = q.X - 1; x >= 0; x--)
                    {
                        if (!board[x, q.Y].IsEmpty)
                        {
                            if (board[x, q.Y].Piece.Team != q.Team)
                                capCells.Add(board[x, q.Y]);
                            break;
                        }
                    }
                    break;
                case King kg:
                    //UP 1
                    if (kg.Y + 1 < Board.Board.HEIGHT && !board[kg.X, kg.Y + 1].IsEmpty &&
                        board[kg.X, kg.Y + 1].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X, kg.Y + 1, board))
                            capCells.Add(board[kg.X, kg.Y + 1]);
                    }
                    //DOWN 1
                    if (kg.Y - 1 >= 0 && !board[kg.X, kg.Y - 1].IsEmpty &&
                        board[kg.X, kg.Y - 1].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X, kg.Y - 1, board))
                            capCells.Add(board[kg.X, kg.Y - 1]);
                    }
                    //LEFT 1
                    if (kg.X - 1 >= 0 && !board[kg.X - 1, kg.Y].IsEmpty &&
                        board[kg.X - 1, kg.Y].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X - 1, kg.Y, board))
                            capCells.Add(board[kg.X - 1, kg.Y]);
                    }
                    //RIGHT 1
                    if (kg.X + 1 < Board.Board.WIDTH && !board[kg.X + 1, kg.Y].IsEmpty &&
                        board[kg.X + 1, kg.Y].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X + 1, kg.Y, board))
                            capCells.Add(board[kg.X + 1, kg.Y]);
                    }
                    //UP RIGHT 1
                    if (kg.X + 1 < Board.Board.WIDTH && kg.Y + 1 < Board.Board.HEIGHT && !board[kg.X + 1, kg.Y + 1].IsEmpty &&
                        board[kg.X + 1, kg.Y + 1].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X + 1, kg.Y + 1, board))
                            capCells.Add(board[kg.X + 1, kg.Y + 1]);
                    }
                    //UP LEFT 1
                    if (kg.X - 1 >= 0 && kg.Y + 1 < Board.Board.HEIGHT && !board[kg.X - 1, kg.Y + 1].IsEmpty &&
                        board[kg.X - 1, kg.Y + 1].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X - 1, kg.Y + 1, board))
                            capCells.Add(board[kg.X - 1, kg.Y + 1]);
                    }
                    //DOWN RIGHT 1
                    if (kg.X + 1 < Board.Board.WIDTH && kg.Y - 1 >= 0 && !board[kg.X + 1, kg.Y - 1].IsEmpty &&
                        board[kg.X + 1, kg.Y - 1].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X + 1, kg.Y - 1, board))
                            capCells.Add(board[kg.X + 1, kg.Y - 1]);
                    }
                    //DOWN LEFT 1
                    if (kg.X - 1 >= 0 && kg.Y - 1 >= 0 && !board[kg.X - 1, kg.Y - 1].IsEmpty &&
                        board[kg.X - 1, kg.Y - 1].Piece.Team != kg.Team)
                    {
                        if (!simulatePieceMove(kg, kg.X - 1, kg.Y - 1, board))
                            capCells.Add(board[kg.X - 1, kg.Y - 1]);
                    }
                    break;
                default:
                    throw new PieceTypeException("Unknow piece type", piece);
            }

            //for (int i = capCells.Count - 1; i >= 0; i--)
            //{
            //    if (simulatePieceMove(piece, capCells[i].X, capCells[i].Y, board))
            //        capCells.RemoveAt(i);
            //}

            return capCells;
        }
        #endregion

        #region Check is cell reachable for enemy pieces
        /// <summary>
        /// Gets value indicating whether the cell reachable for enemy, depending on the situation on the board
        /// </summary>
        /// <param name="cell">Cell instance</param>
        /// <param name="team">Player team</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the cell reachable for enemy or not</returns>
        public static bool IsCellReachableForEnemy(Cell cell, TeamType team, Board.Board board)
        {
            if (cell.Piece != null && cell.Piece.Team != team)
                return true;

            List<Cell> cells = new List<Cell>();

            foreach (var c in board)
            {
                if (c.Piece != null && c.Piece.Team != team)
                {
                    cells.AddRange(PieceCanMove(c.Piece, board));
                }
            }

            return cells.Contains(cell) ? true : false;
        }
        /// <summary>
        /// Gets value indicating whether the cell reachable for enemy, depending on the situation on the board
        /// </summary>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <param name="team">Player team</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the cell reachable for enemy or not</returns>
        public static bool IsCellReachableForEnemy(int x, int y, TeamType team, Board.Board board) => IsCellReachableForEnemy(board[x, y], team, board);
        /// <summary>
        /// Gets value indicating whether the cell reachable for enemy, depending on the situation on the board
        /// </summary>
        /// <param name="hIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="team">Player team</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the cell reachable for enemy or not</returns>
        public static bool IsCellReachableForEnemy(char hIndex, int vIndex, TeamType team, Board.Board board) => IsCellReachableForEnemy(board[hIndex, vIndex], team, board);
        /// <summary>
        /// Gets value indicating whether the cell reachable for enemy, depending on the situation on the board
        /// </summary>
        /// <param name="cell">Cell instance</param>
        /// <param name="player">Player instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the cell reachable for enemy or not</returns>
        public static bool IsCellReachableForEnemy(Cell cell, Team team, Board.Board board) => IsCellReachableForEnemy(cell, team.TeamType, board);
        /// <summary>
        /// Gets value indicating whether the cell reachable for enemy, depending on the situation on the board
        /// </summary>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <param name="player">Player instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the cell reachable for enemy or not</returns>
        public static bool IsCellReachableForEnemy(int x, int y, Team team, Board.Board board) => IsCellReachableForEnemy(board[x, y], team.TeamType, board);
        /// <summary>
        /// Gets value indicating whether the cell reachable for enemy, depending on the situation on the board
        /// </summary>
        /// <param name="hIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="player">Player instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the cell reachable for enemy or not</returns>
        public static bool IsCellReachableForEnemy(char hIndex, int vIndex, Team team, Board.Board board) => IsCellReachableForEnemy(board[hIndex, vIndex], team.TeamType, board);
        #endregion

        /// <summary>
        /// Gets value indicating whether the piece can be captured by enemy
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="board">Game board</param>
        /// <returns>(Value indicating whether the piece can be captured by enemy, list of enemy pieces which can capture this piece)</returns>
        public static (bool, List<Piece>) IsPieceCanBeCapturedByEnemy(Piece piece, Board.Board board)
        {
            List<Piece> pieces = new List<Piece>();

            foreach (var cell in board)
            {
                if (cell.Piece != null && cell.Piece.Team != piece.Team && PieceCanCapture(cell.Piece, board).Exists(c => c.X == piece.X && c.Y == piece.Y))
                    pieces.Add(cell.Piece);
            }

            return pieces.Count > 0 ? (true, pieces) : (false, pieces);
        }
        /// <summary>
        /// Gets value indicating whether the piece is covered by friendly piece
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the piece is covered by friendly piece</returns>
        public static bool IsPieceCoveredByFriendlyPiece(Piece piece, Board.Board board)
        {
            Piece tmpPiece = new Piece(piece.X, piece.Y, piece.Team == TeamType.WHITE ? TeamType.BLACK : TeamType.WHITE);
            Board.Board tmpBoard = new Board.Board(board);
            tmpBoard[tmpPiece.X, tmpPiece.Y].RemovePiece();
            tmpBoard[tmpPiece.X, tmpPiece.Y].SetPiece(tmpPiece);
            (bool res, var tmp) = IsPieceCanBeCapturedByEnemy(tmpPiece, tmpBoard);
            return res;
        }

        #region Check if piece can be promoted
        /// <summary>
        /// Gets value indicating whether the piece can be promoted
        /// </summary>
        /// <param name="piece">Piece instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the piece can be promoted or not</returns>
        public static bool PieceCanBePromoted(Piece piece, Board.Board board)
        {
            if (!(piece is Pawn))
                return false;
            if (piece.Team == TeamType.WHITE && piece.vIndex == 8)
                return true;
            if (piece.Team == TeamType.BLACK && piece.vIndex == 1)
                return true;
            return false;
        }
        #endregion

        #region Move piece to new cell
        /// <summary>
        /// Move piece to new position 
        /// </summary>
        /// <param name="activePiece">Piece to move</param>
        /// <param name="destinationCell">Destination cell</param>
        /// <param name="board">Game board</param>
        public static void Move(Piece activePiece, Cell destinationCell, Board.Board board)
        {
            board[destinationCell.X, destinationCell.Y].SetPiece(activePiece);
            board[activePiece.X, activePiece.Y].RemovePiece();
            activePiece.SetNewPosition(destinationCell.X, destinationCell.Y);

        }
        /// <summary>
        /// Move piece to new position 
        /// </summary>
        /// <param name="activePiece">Piece to move</param>
        /// <param name="x">X coordinate of cell on board</param>
        /// <param name="y">Y coordinate of cell on board</param>
        /// <param name="board">Game board</param>
        public static void Move(Piece activePiece, int x, int y, Board.Board board) => Move(activePiece, board[x, y], board);
        /// <summary>
        /// Move piece to new position 
        /// </summary>
        /// <param name="activePiece">Piece to move</param>
        /// <param name="hIndex">Horizontal index of cell on board ('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h')</param>
        /// <param name="vIndex">Vertical index of cell on board (1, 2, 3, 4, 5, 6, 7, 8)</param>
        /// <param name="board">Game board</param>
        public static void Move(Piece activePiece, char hIndex, int vIndex, Board.Board board) => Move(activePiece, board[hIndex, vIndex], board);
        #endregion

        #region Is check
        /// <summary>
        /// Gets value indicating whether the team received a check
        /// </summary>
        /// <param name="king">King instance</param>
        /// <param name="board">Game board</param>
        /// <returns>(Value indicating whether the team received a check or not, list of enemy pieces that made check)</returns>
        public static (bool, List<Piece>) IsCheck(King king, Board.Board board) => IsPieceCanBeCapturedByEnemy(king, board);
        /// <summary>
        /// Gets value indicating whether the team received a check
        /// </summary>
        /// <param name="team">Team</param>
        /// <param name="board">Game board</param>
        /// <returns>(Value indicating whether the team received a check or not, list of enemy pieces that made check)</returns>
        public static (bool, List<Piece>) IsCheck(TeamType team, Board.Board board)
        {
            foreach (var cell in board)
            {
                if (!cell.IsEmpty && cell.Piece is King && cell.Piece.Team == team)
                    return IsCheck((King)cell.Piece, board);
            }
            throw new Exception("No King instance found on board for current team");
        }
        /// <summary>
        /// Gets value indicating whether the team received a check
        /// </summary>
        /// <param name="player">Player instance</param>
        /// <param name="board">Game board</param>
        /// <returns>(Value indicating whether the team received a check or not, list of enemy pieces that made check)</returns>
        public static (bool, List<Piece>) IsCheck(Team team, Board.Board board)
        {
            foreach (var piece in team.Pieces)
            {
                if (piece is King)
                    return IsCheck((King)piece, board);
            }
            throw new Exception("No King instance found in Player piaces collection");
        }
        #endregion

        #region Is checkmate
        /// <summary>
        /// Gets value indicating whether the team received a checkmate
        /// </summary>
        /// <param name="king">King instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the team received a checkmate</returns>
        public static bool IsCheckMate(King king, Board.Board board)
        {
            var (check, pieces) = IsCheck(king, board);

            if (!check) // If no check => no checkmate
                return false;

            if (pieces.Count >= 2) // If 2+ pieces => checkmate 
                return true;

            var (capture, tmp) = IsPieceCanBeCapturedByEnemy(pieces[0], board);
            if (capture) // If we capture piece that made check => no checkmate
                return false;

            var freeCells = PieceCanMove(king, board); // Get safe cells for king
            if (freeCells.Count > 0) // If king can move to safe cell => no checkmate
                return false;

            var capCells = PieceCanCapture(king, board); // Get cells which can be captured by king
            if (capCells.Count > 0) // If king can capture enemy piece => no checkmate
                return false;

            return true;
        }
        /// <summary>
        /// Gets value indicating whether the team received a checkmate
        /// </summary>
        /// <param name="team">Team</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the team received a checkmate</returns>
        public static bool IsCheckMate(TeamType team, Board.Board board)
        {
            foreach (var cell in board)
            {
                if (!cell.IsEmpty && cell.Piece is King && cell.Piece.Team == team)
                    return IsCheckMate((King)cell.Piece, board);
            }
            throw new Exception("No King instance found on board for current team");
        }
        /// <summary>
        /// Gets value indicating whether the team received a checkmate
        /// </summary>
        /// <param name="player">Player instance</param>
        /// <param name="board">Game board</param>
        /// <returns>Value indicating whether the team received a checkmate</returns>
        public static bool IsCheckMate(Team team, Board.Board board)
        {
            foreach (var piece in team.Pieces)
            {
                if (piece is King)
                    return IsCheckMate((King)piece, board);
            }
            throw new Exception("No King instance found in Player piaces collection");
        }
        #endregion
    }
}
