using UnityEngine;

public class AttackPointFollow : MonoBehaviour
{
    public Transform playerOrientation;

    void Update()
    {
        transform.rotation = playerOrientation.rotation;
    }
}
