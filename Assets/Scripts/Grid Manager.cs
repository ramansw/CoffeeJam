using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public Transform floorTransform;
    public int rows = 6;
    public int columns = 6;
    private float cellSize;
    private Vector3 originPosition;

    private Dictionary<Vector2Int, Tray> occupiedCells = new Dictionary<Vector2Int, Tray>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (floorTransform == null)
        {
            Debug.LogError("GridManager: Floor Transform is not assigned!");
            return;
        }

        Renderer floorRenderer = floorTransform.GetComponent<Renderer>();
        if (floorRenderer == null)
        {
            Debug.LogError("GridManager: Floor must have a Renderer component!");
            return;
        }

        Vector3 floorSize = Vector3.Scale(floorRenderer.bounds.size, Vector3.one);
        cellSize = floorSize.x / columns; 

        originPosition = new Vector3(
            floorRenderer.bounds.min.x + (cellSize / 2f),
            floorRenderer.bounds.max.y,
            floorRenderer.bounds.min.z + (cellSize / 2f)
        );

        SnapAllTraysToGrid();
    }

    public Vector3 GetNearestGridPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - originPosition.x) / cellSize);
        int z = Mathf.RoundToInt((worldPosition.z - originPosition.z) / cellSize);

        x = Mathf.Clamp(x, 0, columns - 1);
        z = Mathf.Clamp(z, 0, rows - 1);

        return new Vector3(originPosition.x + x * cellSize, originPosition.y, originPosition.z + z * cellSize);
    }

    public Vector2Int GetGridCoordinates(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - originPosition.x) / cellSize);
        int z = Mathf.RoundToInt((worldPosition.z - originPosition.z) / cellSize);
        return new Vector2Int(x, z);
    }

    public bool IsCellOccupied(Vector2Int cell)
    {
        return occupiedCells.ContainsKey(cell);
    }

    public void OccupyCell(Vector2Int cell, Tray tray)
    {
        occupiedCells[cell] = tray;
    }

    public void VacateCell(Vector2Int cell)
    {
        if (occupiedCells.ContainsKey(cell))
            occupiedCells.Remove(cell);
    }

    public void SnapAllTraysToGrid()
    {
        Tray[] allTrays = FindObjectsOfType<Tray>();
        foreach (var tray in allTrays)
        {
            tray.ForceSnap();
        }
    }
}
