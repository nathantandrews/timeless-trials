using UnityEngine;


[System.Serializable]
public struct EnemyMove
{
    public Vector2 direction;
    public float speed;
    public float duration;
}

public class EnemyController : MonoBehaviour
{
    public EnemyMove[] moves;
    public bool loop = true;
    
    private int currentMoveIndex = 0;
    private float moveTimer = 0f;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (moves.Length == 0) return;
        EnemyMove currentMove = moves[currentMoveIndex];
        moveTimer += Time.deltaTime;

        rb.MovePosition(rb.position + currentMove.direction.normalized * currentMove.speed * Time.deltaTime);
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
