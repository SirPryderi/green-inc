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
        EvaluateTemperature();
        hexMesh.Triangulate(cells);
    }

    private void EvaluateTemperature()
    {
        foreach (var cell in cells)
        {
            var latitude = Latitude(cell);

            var seaColor = new Color(0, 0.3117442f, 1);
            var sandColor = new Color(0.94f, 0.73f, 0.38f);

            // TODO improve code

            var temp = GameManager.Instance.ClimateManager.GetTemperature(latitude, cell.Elevation * 100);

            var desert = 35;
            var temperate = 25;
            var tundra = 10;
            var ice = 0;

            if (temp > desert)
            {
                cell.color = sandColor;
            }
            else if (temp > temperate)
            {
                var f = Remap((float) temp, temperate, desert, 0, 1);
                cell.color = Color.Lerp(new Color(0.074f, 0.427f, 0.082f), sandColor, f);
            }
            else if (temp > tundra)
            {
                var f = Remap((float) temp, tundra, temperate, 0, 1);
                cell.color = Color.Lerp(new Color(0.254f, 0.596f, 0.039f), new Color(0.074f, 0.427f, 0.082f), f);
            }
            else if (temp > ice)
            {
                var f = Remap((float) temp, ice, tundra, 0, 1);
                cell.color = Color.Lerp(Color.white, new Color(0.254f, 0.596f, 0.039f), f);
            }
            else
            {
                cell.color = Color.white;
            }

            if (cell.Elevation == 0 && temp > 0)
            {
                cell.color = seaColor;
            }
        }
    }

    public float Latitude(HexCell cell)
    {
        var latitude = Remap(cell.coordinates.Z, 0, height - 1, -90, 90);
        return latitude;
    }

    public static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}