using UnityEngine;

public class HexCell : MonoBehaviour
{
    public RectTransform uiRect;

    public int Elevation
    {
        get => elevation;
        set
        {
            elevation = value;
            var position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;

            var uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    int elevation;
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