using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    // rotations
    private Vector3 currentRotations;
    private Vector3 targetRotation;

    [SerializeField]
    private float snappiness;

    [SerializeField]
    private float returnSpeed;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotations = Vector3.Slerp(
            currentRotations,
            targetRotation,
            snappiness * Time.deltaTime
        );
        transform.localRotation = Quaternion.Euler(currentRotations);
    }

    public void FireRecoil(ref float recoilX, ref float recoilY)
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), 0);
    }
}
