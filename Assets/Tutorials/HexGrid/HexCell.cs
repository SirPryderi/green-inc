using UnityEngine;

public class HexCell : MonoBehaviour
{
    private int _elevation;
    public Color color;

    public HexCoordinates coordinates;
    public HexGrid grid;

    [SerializeField] private HexCell[] neighbors;
    public RectTransform uiRect;

    public int Elevation
    {
        get => _elevation;
        set
        {
            var transform1 = transform;
            _elevation = value;

            var position = transform1.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform1.localPosition = position;

            var uiPosition = uiRect.localPosition;
            uiPosition.z = _elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    public float Latitude => coordinates.Z.Remap(0, grid.height - 1, -90, 90);

    public float Temperature => GameManager.Instance.MapManager.ClimateManager.GetCellTemperature(this);

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int) direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int) direction] = cell;
        cell.neighbors[(int) direction.Opposite()] = this;
    }

    public GameObject Spawn(string resource)
    {
        var tmp = transform;

        return Instantiate(
            Resources.Load(resource) as GameObject,
            tmp.position,
            Quaternion.identity,
            tmp
        );
    }

    public void Clear()
    {
        // Removes all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}