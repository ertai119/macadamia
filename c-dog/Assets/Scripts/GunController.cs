using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun[] allGuns;
	Gun equippedGun;

	void Start()
    {
	}

	public void EquipGun(Gun gunToEquip)
    {
		if (equippedGun != null)
        {
			Destroy(equippedGun.gameObject);
		}

		equippedGun = Instantiate (gunToEquip, weaponHold.position,weaponHold.rotation) as Gun;
		equippedGun.transform.parent = weaponHold;
	}

	public void EquipGun(int weaponIndex)
    {
		EquipGun (allGuns [weaponIndex]);
	}

    public Gun GetEquippedGun()
    {
        return equippedGun;
    }

	public void OnTriggerHold() {
		if (equippedGun != null) {
			equippedGun.OnTriggerHold();
		}
	}

	public void OnTriggerRelease() {
		if (equippedGun != null) {
			equippedGun.OnTriggerRelease();
		}
	}

	public float GunHeight {
		get {
			return weaponHold.position.y;
		}
	}

	public void Aim(Vector3 aimPoint) {
		if (equippedGun != null)
        {
            Vector3 heightCorrectedPoint = new Vector3 (aimPoint.x, transform.position.y, aimPoint.z);
            equippedGun.Aim(heightCorrectedPoint);
		}
	}

	public void Reload() {
		if (equippedGun != null) {
			equippedGun.Reload();
		}
	}

}