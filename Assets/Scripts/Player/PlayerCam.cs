using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Senstivity")]
    public float sensX;
    public float sensY;

    public Transform playerOrientation;

    private float xRotation;
    private float yRotation;
    private Logger consoleLogger;

    void Start()
    {
        consoleLogger = Component.FindAnyObjectByType<Logger>();
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

            xRotation = Mathf.Clamp(xRotation, -90f, 70f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    bool mainChecks()
    {
        return !consoleLogger.showConsole;
    }
}
