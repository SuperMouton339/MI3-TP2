using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particules : MonoBehaviour
{
    [SerializeField] private AudioClip[] listeSonsParticules;



    // Start is called before the first frame update
    void Start()
    {
        AudioClip unSon = listeSonsParticules[UnityEngine.Random.Range(0, listeSonsParticules.Length)];
        GetComponent<AudioSource>().PlayOneShot(unSon);
    }

    
}
