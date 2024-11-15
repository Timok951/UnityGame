using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaHandler : MonoBehaviour
{
    public int Health;

    private GameManager _Gamemanager;

    private void Start()
    {
        _Gamemanager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _Gamemanager.LavaDamage(Health);
    }
}
