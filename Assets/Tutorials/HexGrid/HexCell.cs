using UnityEngine;

public class HexCell : MonoBehaviour
{
    public RectTransform uiRect;
    public HexGrid grid;

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

    private int _elevation;
    public HexCoordinates coordinates;
    public Color color;

    [SerializeField] private HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int) direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int) direction] = cell;
        cell.neighbors[(int) direction.Opposite()] = this;
    }
}