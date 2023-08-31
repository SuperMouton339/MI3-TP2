using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieDeplacement : MonoBehaviour //script dans les prefabs des ennemis pour les faire deplacer lorsqu'arriver sur scene
{

    private Vector3 pointA; //propriété servant a mettre un vector3 pour la position initiale de l'enemmi
    private Vector3 pointB; //propriété servant a mettre un vector3 pour la position de destination de l'enemmi

    [SerializeField] public float vitesseEnnemi = 1f; //propriété pouvant etre modifié dans Unity pour la vitesse de deplacement de l'ennemi
    // Start is called before the first frame update
    void Start()
    {
        float positionX = UnityEngine.Random.Range(transform.position.x, transform.position.x *1.5f); //variable position en X généré aléatoirement d'un range entre la position initiale en X et la position en X * 1.5
        float positionZ = UnityEngine.Random.Range(transform.position.z, transform.position.z*1.5f); //variable position en Z généré aléatoirement d'un range entre la position initiale en Z et la position en Z * 1.5


        pointA = new Vector3(transform.position.x, transform.position.y,transform.position.z); //le Vector3 initial ayant les positions initial de l'objet instancier sur tous les axes
        pointB = new Vector3(positionX, transform.position.y, positionZ); // le Vector3 de destination ayant les variables positionX et positionZ
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time * vitesseEnnemi, 1); //determine le temps de tranvers entre les deux point (vitesse)
        transform.position = Vector3.Lerp(pointA, pointB, time); //Vector.Lerp permettant de faire changer la position entre 2 Vector3 (pointA et pointB)
    }
}
