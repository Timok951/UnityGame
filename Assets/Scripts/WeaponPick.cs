using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPick : MonoBehaviour
{

    Playercontroller _Playercontroller;

    private void Start()
    {
        _Playercontroller = FindAnyObjectByType<Playercontroller>();

    }

    public GameObject weaponMesh;



    private void OnTriggerEnter(Collider other)
    {
            if (_Playercontroller != null)
            {
                _Playercontroller.PickupWeapon(gameObject, weaponMesh);
            }

    }
}
