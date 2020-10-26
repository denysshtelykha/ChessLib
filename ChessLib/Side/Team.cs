// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ChessLib.Board.Pieces;
using System;
using System.Collections.Generic;

namespace ChessLib.Side
{
    public class Team
    {
        /// <summary>
        /// Team type
        /// </summary>
        TeamType _teamType;
        /// <summary>
        /// Alive pieces
        /// </summary>
        List<Piece> _pieces;
        /// <summary>
        /// Dead pieces
        /// </summary>
        List<Piece> _deadPieaces;

        /// <summary>
        /// Team type
        /// </summary>
        public TeamType TeamType
        {
            get => _teamType;
        }
        /// <summary>
        /// Player alive peaces
        /// </summary>
        public List<Piece> Pieces
        {
            get => _pieces;
        }
        /// <summary>
        /// Player dead peaces
        /// </summary>
        public List<Piece> DeadPieces
        {
            get => _deadPieaces;
        }
        /// <summary>
        /// Create new instance of Team
        /// </summary>
        /// <param name="teamType"></param>
        public Team(TeamType teamType)
        {
            _teamType = teamType;
            Init();
        }
        /// <summary>
        /// Get piece by index
        /// </summary>
        /// <param name="i">Piece index</param>
        /// <returns></returns>
        public Piece this[int i]
        {
            get
            {
                if (i >= 0 && i < _pieces.Count)
                    return _pieces[i];
                throw new IndexOutOfRangeException();
            }
        }
        /// <summary>
        /// Initialize new team
        /// </summary>
        private void Init()
        {
            _pieces = new List<Piece>();
            _deadPieaces = new List<Piece>();

            if (_teamType == TeamType.WHITE)
            {
                _pieces.Add(new King('e', 1, _teamType));
                _pieces.Add(new Queen('d', 1, _teamType));
                _pieces.Add(new Rook('a', 1, _teamType));
                _pieces.Add(new Rook('h', 1, _teamType));
                _pieces.Add(new Knight('b', 1, _teamType));
                _pieces.Add(new Knight('g', 1, _teamType));
                _pieces.Add(new Bishop('c', 1, _teamType));
                _pieces.Add(new Bishop('f', 1, _teamType));
                for (int i = 0; i < 8; i++)
                    _pieces.Add(new Pawn(i, 1, _teamType));
            }
            else
            {
                _pieces.Add(new King('e', 8, _teamType));
                _pieces.Add(new Queen('d', 8, _teamType));
                _pieces.Add(new Rook('a', 8, _teamType));
                _pieces.Add(new Rook('h', 8, _teamType));
                _pieces.Add(new Knight('b', 8, _teamType));
                _pieces.Add(new Knight('g', 8, _teamType));
                _pieces.Add(new Bishop('c', 8, _teamType));
                _pieces.Add(new Bishop('f', 8, _teamType));
                for (int i = 0; i < 8; i++)
                    _pieces.Add(new Pawn(i, 6, _teamType));
            }
        }
    }

    public enum TeamType
    {
        WHITE,
        BLACK
    }
}
