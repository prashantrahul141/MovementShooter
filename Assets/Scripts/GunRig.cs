using UnityEngine;

public class GunRig : MonoBehaviour
{
    public int selectedGun;
    public GameObject gunHolder;

    void Start()
    {
        selectGun();
    }

    void Update()
    {
        GetInput();
    }

    void GetInput() { }

    void selectGun()
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
