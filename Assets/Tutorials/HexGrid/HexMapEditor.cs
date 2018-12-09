using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    private Color activeColor;
    private int activeElevation;

    public bool IsLocked { get; set; }

    public Text co2Text;
    public Text temperatureText;

    void Awake()
    {
        SelectColor(0);
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Debug.Assert(Camera.main != null, "Camera.main != null");

        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out var hit))
        {
            var cell = hexGrid.GetCell(hit.point);
            EditCell(cell);
            Debug.Log(hexGrid.Latitude(cell));
        }
    }

    private void EditCell(HexCell c)
    {
        if (IsLocked)
        {
            return;
        }

        c.color = activeColor;
        c.Elevation = activeElevation;
        
        EvaluateTemperature();
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int) elevation;
    }

    public void EvaluateTemperature()
    {
        hexGrid.EvaluateTemperature();
        hexGrid.Refresh();
    }

    public void setCO2Levels(float level)
    {
        var manager = GameManager.Instance.ClimateManager;
        co2Text.text = $"{(int) level} ppm";
        temperatureText.text = $"{manager.GetTemperature(0)} °C";

        manager.Atmosphere.SetCO2Concentration(level / 1_000_000f);

        EvaluateTemperature();
    }

    public void SetLocked(bool value)
    {
        this.IsLocked = value;
    }
}