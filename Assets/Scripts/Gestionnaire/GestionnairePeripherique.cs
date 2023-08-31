using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //permet d'utilis� les methode venant du composant InputSystem
using UnityEngine.Events; //permet d'utiliser les ecouteurs et les cris (invoke) de Unity

public class GestionnairePeripherique : MonoBehaviour
{

    [SerializeField] private Vector2 deplacement; //propri�t� contenant les valeurs de Vector2 WASD 
    [SerializeField] private Vector2 regarder;  //propri�t� contenant les valeurs de Vector2 Xet Y du deplacement de la souris



    private PeripheriqueEntree peripheriqueEntree; //propri�t� de type PeripheriqueEntree pour les Inputs configur�s


    public float deplacementX; //propri�t� pour stocker la valeur en X pour le deplacement Horizontal (Accessible par les scripts externes)
    public float deplacementZ; //propri�t� pour stocker la valeur en y pour le deplacement devant et arriere (Accessible par les scripts externes)

    public float regardVertical; //propri�t� stocker la valeur en Y pour la rotation de la camera (Accessible par les scripts externes)
    public float regardHorizontal; //propri�t� stocker la valeur en X pour la rotation de la camera (Accessible par les scripts externes)

    public UnityEvent sauter; //propri�t� permettant de faire un Event (cris) pour indiquer un saut (Accessible par les scripts externes)
    public UnityEvent clic; //propri�t� permettant de faire un Event (cris) pour indiquer un clic (Accessible par les scripts externes)
    public UnityEvent changementCamera; //propri�t� permettant de faire un Event (cris) pour indiquer un changement de camera (Accessible par les scripts externes)


    public bool sprint;//propri�t� de condition si le personnage sprint ou non


    public bool menuTouches; //propri�t� de condidisiont si on appuie sur la touche menu ou non (F1)
  

    private void Awake()
    {
        peripheriqueEntree = new PeripheriqueEntree(); //initialisation de la propri�t� peripheriqueEntree
        peripheriqueEntree.PersoSurSol.Deplacer.performed += LireMouvementDeplacement; //lorsque le peripheriqueEntree performe un deplacement (Clavier WASD) , fait la fonction LireMouvementDeplacement avec le context d'appel
        peripheriqueEntree.PersoSurSol.Deplacer.canceled += LireMouvementDeplacement; //lorsque le peripheriqueEntree arrete un deplacement (Clavier WASD), fait la fonction LireMouvementDeplacement avec le context d'appel

        peripheriqueEntree.PersoSurSol.Regarder.performed += LireMouvementRegard; //lorsque le peripheriqueEntree performe un deplacement de souris, fait la fonction LireMouvementRegard avec le context d'appel
        peripheriqueEntree.PersoSurSol.Regarder.canceled += LireMouvementRegard; //lorsque le peripheriqueEntree arrete un deplacement, fait la fonction LireMouvementRegard avec le context d'appel

        peripheriqueEntree.PersoSurSol.Sauter.started += LireSaut; //lorsque le peripheriqueEntree Saute (spacebar) est activ�, fait la fonction LireSaut avec le context d'appel

        peripheriqueEntree.PersoSurSol.Cliquer.started += LireClic; //lorsque le peripheriqueEntree Clique (Clic gauche) est activ�, fait la fonction LireClic avec le context d'appel

        peripheriqueEntree.PersoSurSol.Sprint.started += LireSprint; //lorsque le peripheriqueEntree performe un commencement de Sprint (left shift), fait la fonction LireSprintOui avec le context d'appel
        peripheriqueEntree.PersoSurSol.Sprint.canceled += LireSprint; //lorsque le peripheriqueEntree performe un arret de Sprint (left shift), fait la fonction LireSprintNon avec le context d'appel

        peripheriqueEntree.PersoSurSol.ChangementCamera.started += LireChangementCamera; //lorsque le peripheriqueEntree d�tecte l'appuie sur le bouton ChangementCamera lance la fction LireChangementCamera

        peripheriqueEntree.PersoSurSol.MenuTouche.started += LireTouche; //lorsque le peripheriqueEntree d�tecte l'appuie sur le bouton MenuTouche lance la fction LireTouche
        peripheriqueEntree.PersoSurSol.MenuTouche.canceled += LireTouche; //lorsque le peripheriqueEntree d�tecte le relache du bouton MenuTouche lance la fction LireTouche

    }

    private void LireTouche(InputAction.CallbackContext context)
    {
        
        menuTouches = context.ReadValueAsButton(); //retourne vrai ou faux sur le context du bouton enfonc� et le met dans la propri�t� menuTouches
    }

    private void LireChangementCamera(InputAction.CallbackContext context)
    {
        changementCamera.Invoke(); //envois un Event(cris) lorsque le bouton ChangementCamera a ete peser
    }

    private void LireSprint(InputAction.CallbackContext context) //appeler si on pese le bouton left shift
    {
        sprint = context.ReadValueAsButton(); //change le sprint a true ou false selon le constext du boutton
    }

    private void LireClic(InputAction.CallbackContext context) //appeler lorsqu'on clique avec le bouton de gauche de la souris
    {
        clic.Invoke(); //envoie un Event (cris) de la propri�t� clic pour un �couteur 
    }

    private void LireSaut(InputAction.CallbackContext context) //appeler lorsqu'on pese sur la spacebar
    {
        sauter.Invoke(); //envoie un Event (cris) de la propri�t� sauter pour un �couteur 
    }

    private void LireMouvementRegard(InputAction.CallbackContext context) //appeler lorsqu'on bouge la souris
    {
        regarder = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1); //met les vector2 du deplacement de souris (context) qui CampMagnitude pour limiter sa valeur maximale de 1

        regardHorizontal = regarder.x; //mettre la propri�t� de regarder.x dans regardHorizontal
        regardVertical = regarder.y; //mettre la propri�t� de regarder.y dans regardVertical
    }

    private void LireMouvementDeplacement(InputAction.CallbackContext context) //appeler lorsqu'on pese sur les touches du clavier WASD
    {
        
        deplacement = context.ReadValue<Vector2>(); //lis les valeur Vector2 du context et le met dans la propri�t� deplacement

        deplacementX = deplacement.x; //la propri�t� deplacement.x dans deplacementX
        deplacementZ = deplacement.y; //la propri�t� deplacement.y  dans deplacementZ
       
    }
    private void OnEnable()
    {
        peripheriqueEntree.PersoSurSol.Enable();
    }

    private void OnDisable()
    {
        peripheriqueEntree.PersoSurSol.Disable();
    }
   
}
