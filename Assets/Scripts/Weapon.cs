using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public AudioSource ShotSound;

    public AudioSource ReloadSound;

    public WeaponsEnum WeaponType;

    public int MaxMagazineBulletCount;

    public int CurrentMagazineBulletCount;

    public int MaxAmmoSupply;

    public float TimeBetweenShots;

    public float TimeForReloading;

    bool CanFire = true;

    bool IsReloading = false;

    private IEnumerator LockFire(float Time)
    {
        yield return new WaitForSeconds(Time);
        CanFire = true;
    }
    private IEnumerator LockFireForReloading(float Time)
    {   
        ReloadSound.Play();
        yield return new WaitForSeconds(Time);
        CurrentMagazineBulletCount = MaxMagazineBulletCount;
        CanFire = true;
        IsReloading = false;
        Debug.Log("Перезарядка завершена!");
    }
    public bool IsMagazineEmpty()
    {
        if (CurrentMagazineBulletCount == 0) return true;
        else return false;
    }
    public void Reload()
    {
        if (!IsReloading)
        {
            Debug.Log("Перезарядка!");
            IsReloading = true;
            CanFire = false;
            StartCoroutine(LockFireForReloading(TimeForReloading));
        }
    }
    public void Fire()
    {
        if (CanFire && !IsMagazineEmpty() && IsReloading != true)
        {
            ShotSound.Play();
            Debug.Log(CurrentMagazineBulletCount);
            CurrentMagazineBulletCount--;
            RaycastHit HitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out HitInfo))
            { Debug.Log(HitInfo.transform.name); }
            CanFire = false;
            StartCoroutine(LockFire(TimeBetweenShots));
        }

    }
}

