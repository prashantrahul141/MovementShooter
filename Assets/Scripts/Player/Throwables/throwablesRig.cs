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

    [SerializeField]
    private DebugController debugController;

    private bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
        debugController = Component.FindAnyObjectByType<DebugController>();
    }

    private void Update()
    {
        transform.position = playerCamera.position;
        transform.rotation = playerCamera.rotation;
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0 && mainChecks())
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

        Vector3 forceDirection = playerCamera.transform.forward;

        RaycastHit raycastHit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, 500))
        {
            forceDirection = (raycastHit.point - attackPoint.position).normalized;
        }

        Vector3 forceToThrowWith = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToThrowWith, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private bool mainChecks()
    {
        return !debugController.showConsole;
    }
}
