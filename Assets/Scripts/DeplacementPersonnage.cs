using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementPersonnage : MonoBehaviour //script a l'int�rieur du GameObject du personnage pour le faire deplaceref
{
    [SerializeField] private GestionnairePeripherique gestionnairePeripherique; // propri�t� accessible sur Unity pour avoir le script GestionnairePeripherique
    [SerializeField] private CharacterController characterController; // propri�t� accessible sur Unity pour avoir le composant CharacterController se trouvant sur le GameObject du personnage 

    [SerializeField] private float vitessePersonnage = 2f; // propri�t� accessible sur Unity pour d�terminer la vitesse de deplacement
    [SerializeField] private float vitesseSprint = 3f; // propri�t� accessible sur Unity pour d�terminer la vitesse du sprint
    [SerializeField] private float hauteurSaut = 1.5f; // propri�t� accessible sur Unity poour d�termin� la hauteur maximal du saut du perso
    [SerializeField] private float gravity = -9.81f; // propri�t� accessible sur Unity d�terminant la gravit� (vitesse de descente)

    [SerializeField] private Transform verifToucheSol; // propri�t� accessible sur Unity ayant les propri�t� Tansform du GameObject verifToucheSol

    [SerializeField] private float distanceAuSol = 0.2f; // propri�t� accessible sur Unity qui d�termine la distance Au Sol pour v�rifier si le perso touche au sol
    [SerializeField] private LayerMask layerSol; // propri�t� accessible sur Unity ayantle layer Sol pour v�rifier si le perso touche au sol

    

    private GestionnaireCamera gestionnaireCamera; //propri�t� contenant le GestionnaireCamera

    private Camera mainCamera; //propri�t� contenant la mainCamera

    private Vector3 velocity; // propri�t� Vector3 pour la velicit�
    private bool toucheSol; // propri�t� bool qui va servir de condition si le perso touche au sol ou non


    private Animator animatorPerso; //propri�t� contenant l'animator de l'instance

    // Start is called before the first frame update
    void Start()
    {
        gestionnairePeripherique.sauter.AddListener(ProduireSaut); //ajouter un ecouteur sur la propri�t� sauter du gestionnairePeripherique et appeler la fonction ProduireSaut() lorsque sauter appel
        animatorPerso = GetComponent<Animator>(); //mettre le composant Animator de l'instance dans la propri�t� animatorPerso
        gestionnaireCamera = GameObject.Find("GestionnaireCamera").GetComponent<GestionnaireCamera>(); //va chercher le GameObject nomm� GestionnaireCamera et prend son composant GestionnaireCamera et me le dans la propri�t�
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); //va chercher le GameObject nomm� GestionnaireCamera et prend son composant GestionnaireCamera et me le dans la propri�t�

    }

   

    private void ProduireSaut() // fonction appeler par l'�couteur
    {
        if (toucheSol) //si toucheSol est vrai
        {
            velocity.y = Mathf.Sqrt(hauteurSaut * -2f * gravity); //changer la valeur de la velocit� en Y en utilisant la formule Mathematique de la gravit� pour faire sauter le personnage 
            animatorPerso.SetTrigger("Saute"); //faire le trigger du paramettre Saute sur l'animator
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gestionnairePeripherique.menuTouches) //si la touche F1 n'est pas enfoncer
        {
        VerifToucheSol(); //appeler la fonction VerifToucheSol

        DeplacementPerso(); //appeler la fonction DeplacementPerso
        
        }
        AppliquerGraviter(); //appeler la fonction AppliquerGraviter
        


    }
    
    
    void VerifToucheSol() //appeler par l'update a chaque frame
    {
        toucheSol = Physics.CheckSphere(verifToucheSol.position, distanceAuSol, layerSol); //regarde la Sphere avec les propri�t� de position de verifToucheSol, la distanceAuSol et s'il touche au layerSol
        

    }



    void DeplacementPerso() //appeler par l'update a chaque frame
    {

        float x = gestionnairePeripherique.deplacementX; //variable temporaire de la fonction avec la valeur en X 
        float y = gestionnairePeripherique.deplacementZ; //variable temporaire de la fonction avec la valeur en Y

        float vitesseDeplacement = vitessePersonnage; //variable temporaire avec la vitesse de deplacement du personnage



        if (gestionnairePeripherique.sprint)//si la condition sprint de gestionnairePeripherique = true
        {
            vitesseDeplacement = vitesseSprint; //mettre la valeur de la vitesseDeplacement = a la vitesseSprint

            animatorPerso.SetBool("onCourt", true); //mettre la condition onCourt a true
        }
        else animatorPerso.SetBool("onCourt", false); //mettre la condition onCourt a false
        
            
            
        
        
        
        
        
        Vector3 move = mainCamera.transform.right * x + mainCamera.transform.forward * y; // Vector3 move = aux propri�t� de transform horizontal * la propri�t� deplacementX du gestionnairePeripherique + les propri�t� du devant * la propri�t� deplacementZ 

        move.y = 0; //le deplacement en Y (Hauteur) ne change pas

        characterController.Move(move * vitesseDeplacement * Time.deltaTime); //fait bouger le personnage avec le characterControler avec la valeur de move * la vitesseDeplacement * Time.deltaTime (temps reel)

        if (gestionnaireCamera.cameraFPSActive) //si la condition cameraFPSActive est vrai
        {
            //rotation joueur premiere personne selon rotation camera
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            if (move.magnitude >= 0.1f) // si perso bouge avance
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 6 * Time.deltaTime);
            }
        }


        if (move != Vector3.zero) //si le perso bouge
        {
            animatorPerso.SetBool("onMarche", true); //mettre la condition onMarche a true

        }
        else
        {
            animatorPerso.SetBool("onMarche", false); //mettre la condition onMarche a false
            animatorPerso.SetBool("onCourt", false);  //mettre la condition onCourt a false
        }
    }


    void AppliquerGraviter()
    {
        velocity.y += gravity * Time.deltaTime; //mise a jour de la propri�t� velocity avec la gravit� * le temps

        characterController.Move(velocity * Time.deltaTime); // la fonction Move du charactController a le Vector3 velocity * le temps
        if (toucheSol && velocity.y < 0)//si touche au sol et que la velocity en y est plus petit que 0
        {
            velocity.y = 0; //remettre la valeur de la velocity en y a 0
        }
    }

}
