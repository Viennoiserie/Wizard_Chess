using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [Header("Objects")] 
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject physicalBoard;

    [Header("Values")]
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float boundsOffset = 0f;

    [Header("Prefabs && Materials")]
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private Material[] teamMaterials;
    [SerializeField] private Material hovered;
    [SerializeField] private Material transparent;


    // Variables
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;

    private GameObject[,] tiles;
    private ChessPiece[,] activePieces;

    private Vector2Int current;


    // Setup
    private void Awake()
    {
        generateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);

        float bx = physicalBoard.transform.position.x + TILE_COUNT_X / 2 * tileSize - boundsOffset;
        float bz = physicalBoard.transform.position.z + TILE_COUNT_Y / 2 * tileSize - boundsOffset;

        // We are here making simple operations to change the coordinates of both the board and the camera 
        physicalBoard.transform.position = new Vector3(bx, 0, bz);
        cam.transform.position += new Vector3(bx, 0, bz);
    }


    // Gameplay
    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit info, 1000, LayerMask.GetMask("Tiles")))
        {
            Vector2Int hitPosition = getTile(info.transform.gameObject);

            if (current == -Vector2Int.one) // If we start hovering tiles
            {
                current = hitPosition;
                tiles[current.x, current.y].GetComponent<MeshRenderer>().material = hovered;
            }

            else if (current != hitPosition) // If we change tiles while hovering
            {
                tiles[current.x, current.y].GetComponent<MeshRenderer>().material = transparent;
                current = hitPosition;
                tiles[current.x, current.y].GetComponent<MeshRenderer>().material = hovered;
            }
        }

        else
        {
            if (current != -Vector2Int.one)
            {
                tiles[current.x, current.y].GetComponent<MeshRenderer>().material = transparent;
                current = -Vector2Int.one;
            }
        }
    }


    // Board generation
    private GameObject generateSingleTile(float size, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0} , Y:{1}", x, y));

        Mesh mesh = new Mesh();

        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>();
        tileObject.AddComponent<BoxCollider>();

        // For some reason the box colliders all spawned in 0,0,0 at the beginning of the board : so I set them by hand
        tileObject.GetComponent<BoxCollider>().size = new Vector3(size, size/100, size);
        tileObject.GetComponent<BoxCollider>().center = new Vector3(x*size + size/2, yOffset, y*size + size/2);

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(x * size, yOffset, y * size);
        vertices[1] = new Vector3(x * size, yOffset, (y + 1) * size);
        vertices[2] = new Vector3((x + 1) * size, yOffset, y * size); ;
        vertices[3] = new Vector3((x + 1) * size, yOffset, (y + 1) * size);

        int[] triangles = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        tileObject.GetComponent<MeshRenderer>().material = transparent;
        tileObject.layer = LayerMask.NameToLayer("Tiles");

        tileObject.transform.parent = transform;

        return tileObject;
    }

    private void generateAllTiles(float size, int xNb, int yNb)
    {
        tiles = new GameObject[xNb, yNb];

        for(int i = 0; i < xNb; i++)
        {
            for(int j = 0; j < yNb; j++)
            {
                tiles[i,j] = generateSingleTile(size, i, j);
            }
        }
    }


    // Spawning Pieces

    private ChessPiece spawnSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece cp = Instantiate(pieces[(int)type - 1], transform).GetComponent<ChessPiece>();

        cp.team = team;
        cp.type = type;

        cp.GetComponent<MeshRenderer>().material = teamMaterials[team];
        return(cp);
    }

    private void spawnAllPiece()
    {
        activePieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];

        int white = 0;
        int black = 1;

        // Spawning white :

        activePieces[0, 0] = spawnSinglePiece(ChessPieceType.Rook, white);
        activePieces[0, 1] = spawnSinglePiece(ChessPieceType.Knight, white);
        activePieces[0, 2] = spawnSinglePiece(ChessPieceType.Bishop, white);
        activePieces[0, 3] = spawnSinglePiece(ChessPieceType.Queen, white);
        activePieces[0, 4] = spawnSinglePiece(ChessPieceType.King, white);
        activePieces[0, 5] = spawnSinglePiece(ChessPieceType.Bishop, white);
        activePieces[0, 6] = spawnSinglePiece(ChessPieceType.Knight, white);
        activePieces[0, 7] = spawnSinglePiece(ChessPieceType.Rook, white);

        for(int i=0; i < TILE_COUNT_X; i++)
        {
            activePieces[1, i] = spawnSinglePiece(ChessPieceType.Pawn, white);
        }

        // Spawning black :

        activePieces[7, 0] = spawnSinglePiece(ChessPieceType.Rook, black);
        activePieces[7, 1] = spawnSinglePiece(ChessPieceType.Knight, black);
        activePieces[7, 2] = spawnSinglePiece(ChessPieceType.Bishop, black);
        activePieces[7, 3] = spawnSinglePiece(ChessPieceType.Queen, black);
        activePieces[7, 4] = spawnSinglePiece(ChessPieceType.King, black);
        activePieces[7, 5] = spawnSinglePiece(ChessPieceType.Bishop, black);
        activePieces[7, 6] = spawnSinglePiece(ChessPieceType.Knight, black);
        activePieces[7, 7] = spawnSinglePiece(ChessPieceType.Rook, black);

        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            activePieces[6, i] = spawnSinglePiece(ChessPieceType.Pawn, black);
        }
    }

    private void positionSinglePiece()
    {

    }

    private void positionAllPieces()
    {

    }


    // Useful functions
    private Vector2Int getTile(GameObject hitInfo)
    {
        // Iterate through our list of tile (only 64 of them) to find which one we hit
        for(int i = 0; i < TILE_COUNT_X; i++)
        {
            for(int j = 0; j < TILE_COUNT_Y; j++)
            {
                if (tiles[i,j] == hitInfo) 
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        // Returning useless value to assure unity this function isn't obsolete
        return -Vector2Int.one;
    }
}
