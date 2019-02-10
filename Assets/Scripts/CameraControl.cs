using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float SENSITIVITY = 0.01f;
    private Transform swivel, stick;
    private float rotationAngle;

    [Header("Stick")] public float stickMinZoom;
    public float stickMaxZoom;
    [Header("Swivel")] public float swivelMinZoom;
    public float swivelMaxZoom;
    [Header("Speed")] public float moveSpeedMin = 50f;
    public float moveSpeedMax = 100f;
    public float rotationSpeed = 180f;
    [Header("Zoom")] public float zoom = 1f;

    void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);

        SetZoom();
    }

    private void Start()
    {
        SetZoom(0.5f);
        transform.localPosition = MaxPosition() / 2f;
    }

    void Update()
    {
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(zoomDelta) > SENSITIVITY)
        {
            AdjustZoom(zoomDelta);
        }

        var rotationDelta = Input.GetAxis("Rotation");
        if (Math.Abs(rotationDelta) > SENSITIVITY)
        {
            AdjustRotation(rotationDelta);
        }

        var xDelta = Input.GetAxis("Horizontal");
        var zDelta = Input.GetAxis("Vertical");

        if (Mathf.Abs(xDelta) > SENSITIVITY || Mathf.Abs(zDelta) > SENSITIVITY)
        {
            AdjustPosition(xDelta, zDelta);
        }
    }

    private void AdjustRotation(float delta)
    {
        rotationAngle += delta * rotationSpeed * Time.deltaTime;

        if (rotationAngle < 0f)
        {
            rotationAngle += 360f;
        }

        else if (rotationAngle >= 360f)
        {
            rotationAngle -= 360f;
        }

        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    void AdjustPosition(float xDelta, float zDelta)
    {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        var moveSpeed = Mathf.Lerp(moveSpeedMax, moveSpeedMin, zoom);

        if (Input.GetKey(KeyCode.LeftShift)) moveSpeed *= 2f;

        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = moveSpeed * damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;

        transform.localPosition = ClampPosition(position);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        var max = MaxPosition();

        position.x = Mathf.Clamp(position.x, 0f, max.x);
        position.z = Mathf.Clamp(position.z, 0f, max.z);

        return position;
    }

    private Vector3 MaxPosition()
    {
        var grid = G.MP.Grid;

        var xMax = (grid.width - 0.5f) * (2f * HexMetrics.innerRadius);
        var zMax = (grid.height - 1) * (1.5f * HexMetrics.outerRadius);

        return new Vector3(xMax, 0, zMax);
    }

    void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);

        SetZoom();
    }

    private void SetZoom()
    {
        var distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);

        var angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    private void SetZoom(float newZoom)
    {
        zoom = newZoom;
        SetZoom();
    }
}