using UnityEngine;

public enum ChessPieceType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6
}

public class ChessPiece : MonoBehaviour
{
    public int team;
    public ChessPieceType type;

    public int currentX;
    public int currentY;
    
    private Vector3 desiredPosition;
    private Vector3 desiredScale;

}
