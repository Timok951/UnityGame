using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MedKitHandler : MonoBehaviour
{
    public ParticleSystem DestroyParticle;
    public int Health;
    public AudioSource DestroyAudio;

    private GameManager _GameManager;

    private void Start()
    {
        _GameManager= FindObjectOfType<GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyParticle.transform.parent = null;
        DestroyAudio.transform.parent=null;
        DestroyAudio.Play();
        DestroyParticle.Play();
        Destroy(gameObject);

        _GameManager.Healing(Health);
    }



}
