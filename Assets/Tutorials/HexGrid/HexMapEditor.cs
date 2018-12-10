using JetBrains.Annotations;
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
    private Camera camera;

    void Awake()
    {
        SelectColor(0);
        camera = Camera.main;
        Debug.Assert(camera != null, "Camera.main != null");
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
        var inputRay = camera.ScreenPointToRay(Input.mousePosition);
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

        RefreshGrid();
    }

    public void RefreshGrid()
    {
        hexGrid.Refresh();
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int) elevation;
    }

    public void setCO2Levels(float level)
    {
        var manager = GameManager.Instance.ClimateManager;
        co2Text.text = $"{(int) level} ppm";
        temperatureText.text = $"{manager.GetTemperature(0)} °C";

        manager.Atmosphere.SetCO2Concentration(level / 1_000_000f);

        RefreshGrid();
    }

    [UsedImplicitly]
    public void Flatten(int elevation)
    {
        new HexGridGenerator(hexGrid).Flatten(elevation);
    }

    public void Random()
    {
        new HexGridGenerator(hexGrid).GenerateFromPerlin();
    }

    [UsedImplicitly]
    public void SetLocked(bool value)
    {
        IsLocked = value;
    }
}