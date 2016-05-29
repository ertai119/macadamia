using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public enum FireMode {Auto, Brust, Single};
    public FireMode fireMode;

	public Transform[] projectileSpawn;
	public Projectile projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
    public int burstCount;
    public int projectilePerMag;
    public float reloadTime = .3f;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

    [Header("Effect")]
    public Transform shell;
    public Transform shellEjection;
    MuzzleFlash muzzleFlash;

	float nextShotTime;
    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    int projectileRemainingInMag;
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
        projectileRemainingInMag = projectilePerMag;
    }

    void LateUpdate()
    {
        // animation recoil
        transform.localPosition =
            Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.right * -recoilAngle;

        if (isReloading == false && projectileRemainingInMag == 0)
        {
            Reload();
        }
    }

	void Shoot()
	{
        if (!isReloading && Time.time >= nextShotTime && projectileRemainingInMag > 0) 
		{
            if (fireMode == FireMode.Brust)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }

                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                if (projectileRemainingInMag == 0)
                    break;
                
                projectileRemainingInMag--;

                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate (projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed (muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();

            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x , recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
		}
	}

    public void Reload()
    {
        if (isReloading == false && projectileRemainingInMag != projectilePerMag)
        {
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = (-Mathf.Pow (percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.right * -reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectileRemainingInMag = projectilePerMag;
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }

    public void Aim(Vector3 aimPoint)
    {
        if (isReloading == false)
        {
            transform.LookAt(aimPoint);
        }
    }
}
