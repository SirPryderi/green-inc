using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const double SENSITIVITY = 0.01;
    private Transform swivel, stick;

    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMin = 50f;
    public float moveSpeedMax = 100f;

    public float zoom = 1f;

    void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);

        SetZoom();
    }

    void Update()
    {
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(zoomDelta) > SENSITIVITY)
        {
            AdjustZoom(zoomDelta);
        }

        var xDelta = Input.GetAxis("Horizontal");
        var zDelta = Input.GetAxis("Vertical");

        if (Mathf.Abs(xDelta) > SENSITIVITY || Mathf.Abs(zDelta) > SENSITIVITY)
        {
            AdjustPosition(xDelta, zDelta);
        }
    }

    void AdjustPosition(float xDelta, float zDelta)
    {
        Vector3 direction = new Vector3(xDelta, 0f, zDelta).normalized;
        var moveSpeed = Mathf.Lerp(moveSpeedMax, moveSpeedMin, zoom);
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = moveSpeed * damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;

        transform.localPosition = position;
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
}