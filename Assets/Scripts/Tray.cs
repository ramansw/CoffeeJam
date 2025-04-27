using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Tray : MonoBehaviour
{
    public Transform selectablePart;
    public float moveSpeed = 15f;
    public float snapDistance = 0.1f;

    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    private Vector2Int originalCell;
    private float groundYPosition;
    private LayerMask trayLayerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ConfigurePhysics();
        InitializeLayerMask();
    }

    private void Start()
    {
        groundYPosition = transform.position.y;
        ForceSnap();
    }

    private void ConfigurePhysics()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | 
                        RigidbodyConstraints.FreezeRotationZ |
                        RigidbodyConstraints.FreezeRotationY;
        rb.isKinematic = true;
        rb.mass = 3f;
    }

    private void InitializeLayerMask()
    {
        trayLayerMask = ~LayerMask.GetMask("Tray");
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            SmoothMovement();
            MaintainHeight();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDragging();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }

    private void TryStartDragging()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, trayLayerMask))
        {
            if (IsValidSelection(hit))
            {
                StartDragging(hit);
            }
        }
    }

    private bool IsValidSelection(RaycastHit hit)
    {
        if (selectablePart != null)
        {
            return hit.collider.transform == selectablePart || 
                   hit.collider.transform.IsChildOf(selectablePart);
        }
        return hit.collider.transform == transform || 
               hit.collider.transform.IsChildOf(transform);
    }

    private void StartDragging(RaycastHit hit)
    {
        isDragging = true;
        originalPosition = transform.position;
        originalCell = GridManager.Instance.GetGridCoordinates(originalPosition);
        offset = originalPosition - hit.point;
        offset.y = 0;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
    }

    private void SmoothMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, trayLayerMask))
        {
            Vector3 targetPosition = hit.point + offset;
            targetPosition.y = groundYPosition;
            
            Vector3 moveDirection = targetPosition - rb.position;
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    private void MaintainHeight()
    {
        if (Mathf.Abs(rb.position.y - groundYPosition) > 0.01f)
        {
            rb.position = new Vector3(rb.position.x, groundYPosition, rb.position.z);
        }
    }

    private void StopDragging()
    {
        isDragging = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        TrySnap();
        rb.isKinematic = true;
    }

    private void TrySnap()
    {
        Vector3 targetSnapPos = GridManager.Instance.GetNearestGridPoint(transform.position);
        Vector2Int targetCell = GridManager.Instance.GetGridCoordinates(targetSnapPos);

        if (Vector3.Distance(transform.position, targetSnapPos) < snapDistance && 
            !GridManager.Instance.IsCellOccupied(targetCell))
        {
            CompleteSnap(targetSnapPos, targetCell);
        }
        else
        {
            RevertPosition();
        }
    }

    private void CompleteSnap(Vector3 position, Vector2Int cell)
    {
        GridManager.Instance.VacateCell(originalCell);
        GridManager.Instance.OccupyCell(cell, this);
        transform.position = position;
    }

    private void RevertPosition()
    {
        transform.position = originalPosition;
        GridManager.Instance.OccupyCell(originalCell, this);
    }

    public void ForceSnap()
    {
        Vector3 snapPos = GridManager.Instance.GetNearestGridPoint(transform.position);
        Vector2Int snapCell = GridManager.Instance.GetGridCoordinates(snapPos);

        if (!GridManager.Instance.IsCellOccupied(snapCell))
        {
            CompleteSnap(snapPos, snapCell);
        }
    }
}