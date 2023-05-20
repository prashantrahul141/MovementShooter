using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform playerPosition;

    void Update()
    {
        transform.position = playerPosition.position;
    }
}
