using UnityEngine;

public class throwablesRig : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private GameObject objectToThrow;

    [Header("Stats")]
    [SerializeField]
    private int totalThrows;

    [SerializeField]
    private float throwCoolDown;

    [Header("throwing")]
    [SerializeField]
    private KeyCode throwKey = KeyCode.G;

    [SerializeField]
    private float throwForce;

    [SerializeField]
    private float throwUpwardForce;

    private bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        GameObject projectile = Instantiate<GameObject>(
            objectToThrow,
            attackPoint.position,
            playerCamera.rotation
        );

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceToThrowWith =
            playerCamera.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToThrowWith, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
