using UnityEngine;

public class GunRig : MonoBehaviour
{
    public int selectedGun;
    public GameObject gunHolder;

    public Transform cameraTransform;

    private int previousSelected;

    private DebugController consoleLogger;
    private HudCanvas hudCanvas;

    void Start()
    {
        hudCanvas = Component.FindAnyObjectByType<HudCanvas>();
        consoleLogger = Component.FindAnyObjectByType<DebugController>();
        changeGun();
    }

    void Update()
    {
        transform.position = cameraTransform.position;
        transform.rotation = cameraTransform.rotation;
        GetInput();
    }

    void GetInput()
    {
        if (mainChecks())
        {
            previousSelected = selectedGun;
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                selectedGun = selectedGun >= transform.childCount ? 0 : selectedGun + 1;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                selectedGun = selectedGun <= 0 ? transform.childCount : selectedGun - 1;
            }

            if (selectedGun != previousSelected)
            {
                changeGun();
            }
        }
    }

    void changeGun()
    {
        int i = 0;
        foreach (Transform weapon in gunHolder.transform)
        {
            if (i == selectedGun)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    bool mainChecks()
    {
        return !consoleLogger.showConsole && !hudCanvas.gameIsPaused;
    }
}
