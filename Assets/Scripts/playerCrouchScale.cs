using UnityEngine;

public class playerCrouchScale : MonoBehaviour
{
    public float crouchYScale;
    public float startYScale;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(
                transform.localScale.x,
                crouchYScale,
                transform.localScale.z
            );
        }
        else if (!Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(
                transform.localScale.x,
                startYScale,
                transform.localScale.z
            );
        }
    }
}
