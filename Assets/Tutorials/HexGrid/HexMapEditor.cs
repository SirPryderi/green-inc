using JetBrains.Annotations;
using Stat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

    private void Awake()
    {
        SelectColor(0);
        camera = Camera.main;
        Debug.Assert(camera != null, "Camera.main != null");

        var d = (int) (G.CM.Atmosphere.GetGasPercentage("Carbon Dioxide") * 1_000_000);
        co2Text.text = $"{d} ppm";
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        var inputRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out var hit))
        {
            var cell = hexGrid.GetCell(hit.point);
            EditCell(cell);
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
        temperatureText.text = $"{Statistics.AverageTemperature:N} °C";
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int) elevation;
    }

    public void SetCO2Levels(float level)
    {
        var manager = GameManager.Instance.MapManager.ClimateManager;
        co2Text.text = $"{(int) level:D} ppm";

        manager.Atmosphere.SetCO2Concentration(level / 1_000_000f);

        RefreshGrid();
    }

    public void Flatten(int elevation)
    {
        new HexGridGenerator(hexGrid).Flatten(elevation);
    }

    public void Reset()
    {
        ClearAll();
        Flatten(0);
    }

    public void ClearAll()
    {
        foreach (var cell in hexGrid.cells)
        {
            cell.Clear();
        }
    }

    public void Random()
    {
        ClearAll();
        var old = G.CM.Atmosphere.GetGasPercentage("Carbon Dioxide");
        G.GM.ResetMap(hexGrid);
        G.CM.Atmosphere.SetCO2Concentration(old);
        RefreshGrid();
    }

    [UsedImplicitly]
    public void SetLocked(bool value)
    {
        IsLocked = value;
    }

    public void PlayMap()
    {
        DontDestroyOnLoad(hexGrid);
        SceneManager.LoadSceneAsync(1);
    }
}