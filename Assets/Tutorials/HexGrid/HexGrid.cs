using System;
using Evaluators;
using Pawns;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Color defaultColor = Color.white;
    public Overlay overlayType = Overlay.NONE;
    public Evaluator evaluator = new CellColourEvaluator();

    internal HexCell[] cells;
    private HexMesh hexMesh;
    private Canvas gridCanvas;

    public int NumberOfCells => height * width;

    private void Awake()
    {
        G.MP.Grid = this;

        cells = new HexCell[height * width];
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        for (int z = 0, i = 0; z < height; z++)
        for (var x = 0; x < width; x++)
            CreateCell(x, z, i++);
    }

    private void Start()
    {
        Refresh();
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        var cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.grid = this;
        cell.color = defaultColor;

        AddNeighbors(x, z, i, cell);

        var label = Instantiate(cellLabelPrefab, gridCanvas.transform, false);
        var rectTransform = label.rectTransform;
        cell.uiRect = rectTransform;

        rectTransform.sizeDelta = new Vector2(HexMetrics.outerRadius, HexMetrics.outerRadius);
        rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        label.text = cell.coordinates.ToString();
    }

    public void RefreshOverlay()
    {
        if (gridCanvas == null) return;

        // Turns off the overlay and quits for performance
        if (overlayType == Overlay.NONE)
        {
            gridCanvas.enabled = false;
            return;
        }

        // Enables the canvas is not already enabled
        if (!gridCanvas.enabled) gridCanvas.enabled = true;

        // Updates each cell according to the overlay status
        foreach (var cell in cells)
        {
            var label = cell.uiRect.GetComponent<Text>();

            switch (overlayType)
            {
                case Overlay.NONE:
                    label.text = "";
                    break;
                case Overlay.COORDINATES:
                    label.text = cell.coordinates.ToString();
                    break;
                case Overlay.TEMPERATURE:
                    label.text = $"{cell.Temperature:0.##}\u00A0°C";
                    break;
                case Overlay.CITY_ATTRACTIVENESS:
                    label.text = $"{evaluator.Evaluate(cell) * 100:0.##}\u00A0%";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void AddNeighbors(int x, int z, int i, HexCell cell)
    {
        if (x > 0) cell.SetNeighbor(HexDirection.W, cells[i - 1]);

        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0) cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1) cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
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
        if (hexMesh != null) hexMesh.Triangulate(cells);
        RefreshOverlay();
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