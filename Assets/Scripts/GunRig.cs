using UnityEngine;

public class GunRig : MonoBehaviour
{
    public int selectedGun;
    public GameObject gunHolder;

    private int previousSelected;

    void Start()
    {
        changeGun();
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
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
}
