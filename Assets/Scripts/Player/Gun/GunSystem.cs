using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting,
        spread,
        range,
        reloadTime,
        timeBetweenShots;
    public int magazineSize,
        bulletsPerTap;
    public bool allowButtonHold;
    public float recoilX,
        recoilY;
    private int bulletsLeft,
        bulletsShot;
    private bool shooting,
        readyToShoot,
        reloading;

    [Header("References")]
    public Transform playerOrientation;
    public Camera fpsCamera;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemyLayer;
    public RecoilScript recoilScript;
    public DecalManager decalManager;
    public HudCanvas hudCanvasScript;
    public DebugController debugController;

    [Header("Graphics")]
    public ParticleSystem muzzleFlash_GunCamera;
    public ParticleSystem muzzleFlash_MainCamera;
    public Transform gunModel;

    private void Start()
    {
        hudCanvasScript = Component.FindAnyObjectByType<HudCanvas>();
        debugController = Component.FindAnyObjectByType<DebugController>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        if (mainChecks())
        {
            TakeInput();
        }
    }

    private void TakeInput()
    {
        shooting = allowButtonHold
            ? Input.GetKey(KeyCode.Mouse0)
            : Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKey(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        // when to shoot
        if (bulletsLeft <= 0)
        {
            BulletsOver();
        }
        else
        {
            if (readyToShoot && shooting && !reloading)
            {
                bulletsShot = bulletsPerTap;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        recoilScript.FireRecoil(ref recoilX, ref recoilY);
        readyToShoot = false;
        playerOrientation.transform.rotation = playerOrientation.transform.rotation.normalized;

        // spread
        float xRandSpread = Random.Range(-spread, spread);
        float yRandSpread = Random.Range(-spread, spread);

        Vector3 direction = fpsCamera.transform.forward + new Vector3(xRandSpread, yRandSpread, 0);

        // RayCast
        if (Physics.Raycast(fpsCamera.transform.position, direction, out rayHit, range, enemyLayer))
        {
            Quaternion decalDirection = Quaternion.FromToRotation(Vector3.forward, rayHit.normal);
            decalManager.CreateDecal(Enums.DecalTypes.METAL, rayHit.point, decalDirection);
            Debug.Log("Hit : " + rayHit.transform.gameObject.name.ToString());
        }

        // bullet hole, muzzle flash
        muzzleFlash_GunCamera.Play();
        muzzleFlash_MainCamera.Play();

        bulletsLeft--;
        bulletsShot--;
        Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        Debug.Log("Started Reloading.");
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        Debug.Log("Finished Reloading.");
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void BulletsOver()
    {
        Debug.Log("Bullets Over.");
    }

    public bool IsShooting()
    {
        return (!readyToShoot && !reloading && bulletsLeft > 0);
    }

    private bool mainChecks()
    {
        return !hudCanvasScript.gameIsPaused && !debugController.showConsole;
    }
}
