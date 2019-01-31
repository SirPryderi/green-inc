using UnityEngine;

public class SunControl : MonoBehaviour
{
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        // Updates sun angle
        var angle = CalculateAngle();
        var to = Quaternion.Euler(new Vector3(angle, 90f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 0.5f);

        // Updates sun intensity
        var targetIntensity = angle <= 0f || angle >= 180f ? 0f : 1f;
        _light.intensity = Mathf.Lerp(_light.intensity, targetIntensity, Time.deltaTime);
        
        // TODO Updates light colour
    }

    private float CalculateAngle()
    {
        return Mathf.Repeat(GameManager.Instance.MapManager.Observer.Time * 15f + 90f, 360);
    }
}