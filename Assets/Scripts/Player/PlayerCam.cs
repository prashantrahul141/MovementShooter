using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform playerOrientation;

    private float sensX;
    private float sensY;
    private float xRotation;
    private float yRotation;
    private DebugController consoleLogger;

    void Start()
    {
        sensX = PlayerPrefs.GetFloat("user_senstivity", 200.0f);
        sensY = PlayerPrefs.GetFloat("user_senstivity", 200.0f);
        consoleLogger = Component.FindAnyObjectByType<DebugController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (mainChecks())
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    bool mainChecks()
    {
        return !consoleLogger.showConsole;
    }
}
