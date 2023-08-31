using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegardPerso : MonoBehaviour //script �tant dans le GameObject de la Main Camera
{
    [SerializeField] GestionnairePeripherique gestionnairePeripherique; //propri�t� accessible sur unity pour mettre le script GestionnairePeripherique

    [SerializeField] private Transform personnage; //propri�t� de type Transform accessible sur unity pour mettre le personnage a l'int�rieur

    [SerializeField] private float vitesseCamHorizontal = 100f; //propri�t� accessible sur unity pour la vitesse Horizontal de la cam�ra
    [SerializeField] private float vitesseCamVertical = 100f; //propri�t� accessible sur unity pour la vitesse Vertical de la cam�ra

    private float xRotation = 0f; //propri�t� servant a stocker la rotation de la cam�ra de Haut en Bas

    // Update is called once per frame
    void Update()
    {
        float mouseX = gestionnairePeripherique.regardHorizontal * Time.deltaTime * vitesseCamHorizontal; //variable de contenant le deplacement de la souris sur l'axe des X fois la vitesseHorizontal et le temps
        float mouseY = gestionnairePeripherique.regardVertical * Time.deltaTime * vitesseCamVertical; //variable de contenant le deplacement de la souris sur l'axe des Y fois la vitesseVertical et le temps

        xRotation -= mouseY; //changement de la valeur de la propri�t� xRotation en fonction de la nouvelle valeur de mouseY 
        xRotation = Mathf.Clamp(xRotation, -90f, 50f); //changement de la valeur de la propri�t� xRotation qui force la valeur de ne pas d�passer deux float (-90 et 50) avec Mathf.Clamp

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //faire une rotation local de l'objet dans lequel le script est dedans avec la valeur xRotation pour faire tourner la camera verticalement

        personnage.Rotate(Vector3.up * mouseX); //faire une rotation en X du personnage pour faire tourn� le personnage horizontalement lorsque qu'on bouge la souris de gauche a droite
        // personnage.Rotate(Vector3.(0,1,0) * mouseX);
    }
}
