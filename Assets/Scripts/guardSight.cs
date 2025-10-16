using UnityEngine;
using UnityEngine.SceneManagement;

public class guardSight : MonoBehaviour
{
    public GameObject spawner;
    public guardSpawning guardSpawningScript;

    public float visionAngle = 60f;
    public float visionDistance = 10f;

    public Transform player;

    [HideInInspector] public float detectionTime = 0f;

    public float minLookTime = 2f;
    public float maxLookTime = 3f;

    private float lookTimer = 0f;
    private Vector2 initialDirection = Vector2.up;
    private Vector2 currentDirection;
    private Vector2 targetDirection;
    private int directionState = 0;

    public float detectionLimit = 1.5f;

    public string loseScene = "EndGame_LoseState";

    public float rotationSpeed = 2f;

    public SerialController serialController;

    private bool isSeen = false;
    private bool wasSeen = false;

    public int facingDirection = 0;

    private LineRenderer lineRenderer;
    private int segments = 20; // Number of segments to fill the vision cone

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawner = GameObject.FindGameObjectWithTag("Spawner");

        if (spawner == null)
        {
            Debug.LogError("Spawner GameObject not found! Ensure the spawner is tagged correctly.");
            return;
        }

        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        guardSpawningScript = spawner.GetComponent<guardSpawning>();

        if (guardSpawningScript == null)
        {
            Debug.LogError("guardSpawning component not found on spawner GameObject! Ensure the spawner has the guardSpawning script attached.");
            return;
        }

        InitializeDirection();

        lookTimer = Random.Range(minLookTime, maxLookTime);

        // Initialize the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 2; // Center point + edges + segments in between
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true; // Connect the last point to the first to close the shape
    }

    public void InitializeDirection()
    {
        SetInitialDirection(facingDirection);
    }

    private void Update()
    {
        lookTimer -= Time.deltaTime;

        if (lookTimer <= 0f)
        {
            ChangeDirection();
            lookTimer = Random.Range(minLookTime, maxLookTime);
        }

        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * rotationSpeed);

        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        float angleToPlayer = Vector2.Angle(currentDirection, toPlayer);

        if (angleToPlayer < visionAngle / 2f && toPlayer.magnitude < visionDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, visionDistance);
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                isSeen = false;
                detectionTime = 0f;
            }
            else
            {
                isSeen = true;
                detectionTime += Time.deltaTime;
            }
        }
        else
        {
            isSeen = false;
            detectionTime = 0f;
        }

        if (isSeen && !wasSeen)
        {
            serialController.SendSerialMessage("C"); // incompatible with storageLED
        }
        else if (!isSeen && wasSeen)
        {
            serialController.SendSerialMessage("C"); // incompatible with storageLED
        }

        wasSeen = isSeen;

        if (detectionTime >= detectionLimit)
        {
            isSeen = false;
            serialController.SendSerialMessage("F");
            SceneManager.LoadScene(loseScene);
        }

        UpdateLineRenderer();
    }

    private void SetInitialDirection(int facingDirection)
    {
        Debug.Log("facingDirectionNumber" + facingDirection);
        switch (facingDirection)
        {
            case 1: // Up
                initialDirection = Vector2.up;
                break;
            case 2: // Down
                initialDirection = Vector2.down;
                break;
            case 3: // Right
                initialDirection = Vector2.right;
                break;
            case 4: // Left
                initialDirection = Vector2.left;
                break;
            default:
                initialDirection = Vector2.up;
                break;
        }

        currentDirection = initialDirection;
        targetDirection = initialDirection;
    }

    private void ChangeDirection()
    {
        if (directionState == 0)
        {
            targetDirection = Quaternion.Euler(0, 0, -90) * initialDirection;
            directionState = 1;
        }
        else if (directionState == 1)
        {
            targetDirection = initialDirection;
            directionState = 2;
        }
        else if (directionState == 2)
        {
            targetDirection = Quaternion.Euler(0, 0, 90) * initialDirection;
            directionState = 3;
        }
        else if (directionState == 3)
        {
            targetDirection = initialDirection;
            directionState = 0;
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = segments + 2; // Center point + segments + edges

        lineRenderer.SetPosition(0, transform.position); // Center point

        float angleStep = visionAngle / segments;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -visionAngle / 2 + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * currentDirection * visionDistance;
            lineRenderer.SetPosition(i + 1, transform.position + direction);
        }

        // Close the loop
        lineRenderer.SetPosition(segments + 1, transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 leftBoundary = Quaternion.Euler(0f, 0f, -visionAngle / 2) * currentDirection * visionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0f, 0f, visionAngle / 2) * currentDirection * visionDistance;

        Gizmos.DrawRay(transform.position, currentDirection * visionDistance);
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
