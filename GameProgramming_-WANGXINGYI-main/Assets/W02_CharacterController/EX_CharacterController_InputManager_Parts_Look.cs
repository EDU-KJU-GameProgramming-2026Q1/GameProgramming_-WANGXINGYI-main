using UnityEngine;

public class EX_CharacterController_InputManager_Parts_Look : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraPivot;
    public float mouseSensitivity = 3f;

    float yaw;
    float pitch;

    void Start()
    {
        if (cameraPivot == null) cameraPivot = transform.Find("CameraRig/CameraPivot");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity;
        pitch -= mouseY * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -90f, 50f); // Look Up, Look Down

        transform.rotation = Quaternion.Euler(0, yaw, 0);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}