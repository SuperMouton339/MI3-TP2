using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class GestionnaireCamera : MonoBehaviour
{
    private GestionnairePeripherique gestionnairePeripherique; //propriété contenant le script de GestionnairePeripherique
    private CinemachineVirtualCamera cameraFPS; //propriete contenant la CineMachineVirtualCamera de la cameraFPS
    private CinemachineFreeLook cameraTPS; //propriété contenant la CineMachineFreeLook de la cameraTPS

    public bool cameraFPSActive = true; //propriété condiditon si la cameraFPS est active

    // Start is called before the first frame update
    void Start()
    {
        //commande de depart d'instance pour aller chercher les composants de GameObject dans les propriété voulu et rajoute un ecouteur a une fction

        gestionnairePeripherique = GameObject.Find("GestionnairePeripherique").GetComponent<GestionnairePeripherique>();
        cameraFPS = GameObject.Find("Camera1rePersonne").GetComponent<CinemachineVirtualCamera>();
        cameraTPS = GameObject.Find("Camera3ePersonne").GetComponent<CinemachineFreeLook>();
        gestionnairePeripherique.changementCamera.AddListener(ProduireChangementCamera);
        
    }

    private void ProduireChangementCamera() //lorsque l'écouteur entend le cris
    {
        if (!gestionnairePeripherique.menuTouches) //si le bool menuTouches est faux
        {
            cameraFPSActive = !cameraFPSActive; //inverser le bool cameraFPSAcive

            if (cameraFPSActive) //si vrai
            {
            cameraTPS.Priority = 0; //mettre la priorité de la cameraTPS a 0
            }
            else //sinon
            {
            cameraTPS.Priority = 20; //mettre la prorité a 20
            }
        }
        
    }
}
