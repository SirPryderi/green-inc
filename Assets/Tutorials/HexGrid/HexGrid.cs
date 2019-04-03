using Evaluators;
using Pawns;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    // Chunk Size
    public int chunkCountX = 4, chunkCountZ = 3;

    // Prefabs
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public HexGridChunk chunkPrefab;
    
    // Overlays
    public Color defaultColor = Color.white;
    public Overlay overlayType = Overlay.NONE;
    public Evaluator evaluator = new CellColourEvaluator();

    // Cells and Chunks
    [HideInInspector] public HexCell[] cells;
    [HideInInspector] public HexGridChunk[] chunks;

    #region Properties
    public int CellCountX => chunkCountX * HexMetrics.chunkSizeX;
    public int CellCountZ => chunkCountZ * HexMetrics.chunkSizeZ;
    public int NumberOfCells => CellCountZ * CellCountX;
    #endregion

    private void Awake()
    {
        G.MP.Grid = this;

        CreateChunks();
        CreateCells();
    }
    
    private void CreateChunks () {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++) {
            for (var x = 0; x < chunkCountX; x++) {
                var chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
                chunk.grid = this;
            }
        }
    }

    private void CreateCells()
    {
        cells = new HexCell[CellCountZ * CellCountX];
        for (int z = 0, i = 0; z < CellCountZ; z++)
        for (var x = 0; x < CellCountX; x++)
            CreateCell(x, z, i++);
    }

    private void Start()
    {
        Refresh();
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        // ReSharper disable once PossibleLossOfFraction
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        var cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.grid = this;
        cell.color = defaultColor;

        AddNeighbors(x, z, i, cell);

        var label = Instantiate(cellLabelPrefab);
        var rectTransform = label.rectTransform;
        
        rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        rectTransform.sizeDelta = new Vector2(HexMetrics.outerRadius, HexMetrics.outerRadius);
        rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        cell.uiRect = rectTransform;
        
        AddCellToChunk(x, z, cell);
    }

    private void AddCellToChunk (int x, int z, HexCell cell) {
        var chunkX = x / HexMetrics.chunkSizeX;
        var chunkZ = z / HexMetrics.chunkSizeZ;
        var chunk = chunks[chunkX + chunkZ * chunkCountX];
        
        var localX = x - chunkX * HexMetrics.chunkSizeX;
        var localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }

    private void AddNeighbors(int x, int z, int i, HexCell cell)
    {
        if (x > 0) cell.SetNeighbor(HexDirection.W, cells[i - 1]);

        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - CellCountX]);
                if (x > 0) cell.SetNeighbor(HexDirection.SW, cells[i - CellCountX - 1]);
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - CellCountX]);
                if (x < CellCountX - 1) cell.SetNeighbor(HexDirection.SE, cells[i - CellCountX + 1]);
            }
        }
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        return cells[HexCoordinates.FromPosition(position).toIndex(this)];
    }

    public void ColorCell(Vector3 position, Color color)
    {
        GetCell(position).color = color;
        Refresh();
    }

    public void Refresh()
    {
        EvaluateTemperature();

        foreach (var chunk in chunks)
        {
            chunk.Refresh();
            chunk.RefreshOverlay();
        }
    }

    private void EvaluateTemperature()
    {
        if (cells == null) return;

        foreach (var cell in cells)
        {
            cell.color = evaluator.CellToColour(cell);

            if (cell.transform.childCount > 0)
            {
                var pawn = cell.transform.GetChild(0).GetComponent<Pawn>();

                if (pawn != null) pawn.Evaluate();
            }
        }
    }
}