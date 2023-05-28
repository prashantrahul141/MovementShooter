using UnityEngine;

public class throwableObject : MonoBehaviour
{
    private Collider throwableCollider;

    private void Start()
    {
        throwableCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.ToString() != "player")
        {
            print("bombed with : " + collision.gameObject.name.ToString());
            Destroy(gameObject);
        }
    }
}
