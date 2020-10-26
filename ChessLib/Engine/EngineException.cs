using ChessLib.Board.Pieces;
using ChessLib.Side;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLib.Engine
{
    class EngineException
    {
    }
    class PieceTypeException : Exception
    {
        public Piece Piece { get; }
        public PieceTypeException(string message, Piece piece)
            : base(message)
        {
            Piece = piece;
        }
    }
    class TeamDoesNotMatchException : Exception
    {
        public TeamDoesNotMatchException(string message)
            : base(message) { }
    }
}
