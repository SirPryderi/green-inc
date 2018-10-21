using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Color defaultColor = Color.white;

    private HexCell[] cells;
    private HexMesh hexMesh;
    private Canvas gridCanvas;

    void Awake()
    {
        cells = new HexCell[height * width];
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        for (int z = 0, i = 0; z < height; z++)
        {
            for (var x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start()
    {
        Refresh();
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        var cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        AddNeighbors(x, z, i, cell);

        var label = Instantiate(cellLabelPrefab, gridCanvas.transform, false);
        var rectTransform = label.rectTransform;
        cell.uiRect = rectTransform;

        rectTransform.sizeDelta = new Vector2(HexMetrics.outerRadius, HexMetrics.outerRadius);
        rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        label.text = cell.coordinates.ToString();
    }

    private void AddNeighbors(int x, int z, int i, HexCell cell)
    {
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }

        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
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
        hexMesh.Triangulate(cells);
    }
}