using System.Runtime.CompilerServices;
using Backend.Data;
using Backend.Data.Enum;
using Backend.Data.Struct;

namespace Backend.Engine;

public static class Evaluation
{
    
    public const int QUEEN = 900;
    public const int ROOK = 500;
    public const int BISHOP_KNIGHT = 300;
    public const int PAWN = 100;

    public static readonly MaterialDevelopmentTable MaterialDevelopmentTable = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RelativeEvaluation(Board board)
    {
        return board.MaterialDevelopmentEvaluation * (-2 * (int)board.ColorToMove + 1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static int InitialMaterialDevelopmentEvaluation(ref BitBoardMap map)
    {
        int whiteScore = 0;
        int blackScore = 0;

        #region White Score Calculation

        Piece piece = Piece.Pawn;
        while (piece != Piece.Empty) {
            BitBoardIterator pieceIterator = map[piece, PieceColor.White].GetEnumerator();
            Square pieceSq = pieceIterator.Current;
            while (pieceIterator.MoveNext()) {
                whiteScore += MaterialDevelopmentTable[piece, (Square)((byte)pieceSq ^ 56)];
                pieceSq = pieceIterator.Current;
            }
            piece++;
        }

        #endregion

        #region Black Score Calculation

        piece = Piece.Pawn;
        while (piece != Piece.Empty) {
            BitBoardIterator pieceIterator = map[piece, PieceColor.Black].GetEnumerator();
            Square pieceSq = pieceIterator.Current;
            while (pieceIterator.MoveNext()) {
                blackScore += MaterialDevelopmentTable[piece, pieceSq];
                pieceSq = pieceIterator.Current;
            }
            piece++;
        }

        #endregion

        return whiteScore - blackScore;
    }

}