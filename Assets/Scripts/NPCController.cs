using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct Move
{
    public Vector2 direction;
    public float speed;
    public float duration;
}

public class NPCController : MonoBehaviour
{
    public Move[] moves;
    public bool loop = true;
    
    private int currentMoveIndex = 0;
    private float moveTimer = 0f;
    private Vector3Int lastCellPos;

    [SerializeField] private Rigidbody2D rb;
    private Tilemap groundTilemap;
    private GameTimer gameTimer;

    void Start()
    {
        BoxCollider2D NPCcollider = GetComponent<BoxCollider2D>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(NPCcollider, playerCollider, true);
            }
        }
        gameTimer = FindFirstObjectByType<GameTimer>();
        groundTilemap = FindFirstObjectByType<Tilemap>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        UpdateTilePosition();
    }
    void UpdateTilePosition()
    {
        Vector3Int currentCell = groundTilemap.WorldToCell(transform.position);
        if (currentCell != lastCellPos)
        {
            TileOccupancyManager.Instance.LeaveTile(lastCellPos, gameObject);
            TileOccupancyManager.Instance.EnterTile(currentCell, gameObject);
            lastCellPos = currentCell;
        }
    }

    void FixedUpdate()
    {
        if (moves.Length == 0) return;
        Move currentMove = moves[currentMoveIndex];
        moveTimer += Time.deltaTime;

        rb.MovePosition(rb.position + currentMove.direction.normalized * currentMove.speed * Time.deltaTime * gameTimer.currentSpeed);
        UpdateTilePosition();

        if (moveTimer >= currentMove.duration)
        {
            moveTimer = 0f;
            currentMoveIndex++;
            if (currentMoveIndex >= moves.Length)
            {
                if (loop)
                {
                    currentMoveIndex = 0;
                }
                else
                {
                    enabled = false;
                }
            }
        }
    }
}
