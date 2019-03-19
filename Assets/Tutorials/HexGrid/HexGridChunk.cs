using System;
using UnityEngine;
using UnityEngine.UI;

public class HexGridChunk : MonoBehaviour
{
    HexCell[] cells;

    private HexMesh hexMesh { get; set; }
    private Canvas gridCanvas { get; set; }
    
    [HideInInspector] public HexGrid grid;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }
    
    public void Refresh () {
        hexMesh.Triangulate(cells);
    }
    
    public void RefreshOverlay()
    {
        if (gridCanvas == null) return;

        // Turns off the overlay and quits for performance
        if (grid.overlayType == Overlay.NONE)
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

            switch (grid.overlayType)
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
                    label.text = $"{grid.evaluator.Evaluate(cell) * 100:0.##}\u00A0%";
                    break;
                case Overlay.SOIL_FERTILITY:
                    label.text = $"{grid.evaluator.Evaluate(cell) * 100:0.##}\u00A0%";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}