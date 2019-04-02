using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControls : MonoBehaviour {

    public float m_PanSpeed = 3.0f;
    public float m_ZoomSpeed = 2.0f;

    public Bounds m_CameraBounds = new Bounds(new Vector3(), new Vector3(10, 10));
    public float m_MinCameraSize = 1.0f;
    public float m_MaxCameraSize = 10.0f;

    public string m_HorizontalAxisName = "Horizontal";
    public string m_VerticalAxisName = "Vertical";
    public string m_ZoomAxisName = "Mouse ScrollWheel";

    private float m_HorizontalAxisValue;
    private float m_VerticalAxisValue;
    private float m_ZoomAxisValue;

    private float m_HorizontalMovementValue;
    private float m_VerticalMovementValue;
    private float m_ZoomMovementValue;

    private Camera m_Camera;

    private int m_FingerID = -1;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();

#if !UNITY_EDITOR
        m_FingerID = 0;
#endif
    }

    private void Update()
    {
        // Zoom camera
        if (!EventSystem.current.IsPointerOverGameObject(m_FingerID))
        {
            m_ZoomAxisValue = Input.GetAxis(m_ZoomAxisName) * -1;   // Invert input since zoom in = smaller size and vice versa

            m_ZoomMovementValue = m_ZoomAxisValue * m_ZoomSpeed * Time.deltaTime;

            // Keep camera zoom within the limits
            if (m_Camera.orthographicSize + m_ZoomMovementValue > m_MaxCameraSize)
            {
                // New zoom is zoomed too far out, clamp to max camera size
                m_Camera.orthographicSize = m_MaxCameraSize;
            }
            else if (m_Camera.orthographicSize + m_ZoomMovementValue < m_MinCameraSize)
            {
                // New zoom is zoomed too far in, clamp to min camera size
                m_Camera.orthographicSize = m_MinCameraSize;
            }
            else
            {
                // New zoom is within acceptable boundaries
                m_Camera.orthographicSize += m_ZoomMovementValue;
            }
        }

        // Pan camera
        m_HorizontalAxisValue = Input.GetAxis(m_HorizontalAxisName);
        m_VerticalAxisValue = Input.GetAxis(m_VerticalAxisName);

        m_HorizontalMovementValue = m_HorizontalAxisValue * m_PanSpeed * Time.deltaTime;
        m_VerticalMovementValue = m_VerticalAxisValue * m_PanSpeed * Time.deltaTime;

        float newX, newY;

        if (m_Camera.orthographicSize > m_CameraBounds.size.y / 2)
        {
            // Camera is too tall to fit in the vertical bounds, so just center it vertically
            newY = m_CameraBounds.center.y;
        }
        else if (transform.position.y + m_Camera.orthographicSize + m_VerticalMovementValue > m_CameraBounds.max.y)
        {
            // After moving, the top of the camera will be outside the camera bounds
            newY = m_CameraBounds.max.y - m_Camera.orthographicSize;
        }
        else if (transform.position.y - m_Camera.orthographicSize + m_VerticalMovementValue < m_CameraBounds.min.y)
        {
            // After moving, the bottom of the camera will be outside the camera bounds
            newY = m_CameraBounds.min.y + m_Camera.orthographicSize;
        }
        else
        {
            // The camera remains within the upper and lower bounds after moving
            newY = transform.position.y + m_VerticalMovementValue;
        }

        if (m_Camera.orthographicSize * m_Camera.aspect > m_CameraBounds.size.x / 2)
        {
            // Camera is too wide to fit in the vertical bounds, so just center it horizontally
            newX = m_CameraBounds.center.x;
        }
        else if (transform.position.x + m_Camera.orthographicSize * m_Camera.aspect + m_HorizontalMovementValue > m_CameraBounds.max.x)
        {
            // After moving, the right side of the camera will be outside the camera bounds
            newX = m_CameraBounds.max.x - m_Camera.orthographicSize * m_Camera.aspect;
        }
        else if (transform.position.x - m_Camera.orthographicSize * m_Camera.aspect + m_HorizontalMovementValue < m_CameraBounds.min.x)
        {
            // After moving, the left side of the camera will be outside the camera bounds
            newX = m_CameraBounds.min.x + m_Camera.orthographicSize * m_Camera.aspect;
        }
        else
        {
            // The camera remains within the right and left bounds after moving
            newX = transform.position.x + m_HorizontalMovementValue;
        }

        gameObject.transform.localPosition = new Vector3(newX, newY, gameObject.transform.localPosition.z);

        Debug.DrawLine(m_CameraBounds.max, new Vector3(m_CameraBounds.max.x, m_CameraBounds.min.y));    // right side
        Debug.DrawLine(m_CameraBounds.max, new Vector3(m_CameraBounds.min.x, m_CameraBounds.max.y));    // top side
        Debug.DrawLine(m_CameraBounds.min, new Vector3(m_CameraBounds.max.x, m_CameraBounds.min.y));    // bottom side
        Debug.DrawLine(m_CameraBounds.min, new Vector3(m_CameraBounds.min.x, m_CameraBounds.max.y));    // left side
    }
}
