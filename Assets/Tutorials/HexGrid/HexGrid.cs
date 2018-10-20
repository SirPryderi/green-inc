using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public Text cellLabelPrefab;

    private HexCell[] cells;

    private Canvas gridCanvas;
    private HexMesh hexMesh;

    public Color defaultColor = Color.white;

    void Awake()
    {
        cells = new HexCell[height * width];
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start()
    {
        TriangulateMesh();
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

        var label = Instantiate(cellLabelPrefab, gridCanvas.transform, false);
        var rectTransform = label.rectTransform;

        rectTransform.sizeDelta = new Vector2(HexMetrics.outerRadius, HexMetrics.outerRadius);
        rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        label.text = cell.coordinates.ToString();
    }

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        var cell = cells[HexCoordinates.FromPosition(position).toIndex(this)];
        cell.color = color;
        TriangulateMesh();
    }

    private void TriangulateMesh()
    {
        hexMesh.Triangulate(cells);
    }
}