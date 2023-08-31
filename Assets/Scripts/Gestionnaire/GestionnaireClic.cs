using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireClic : MonoBehaviour //script dans le GameObject Gestionnaire clic
{
    [SerializeField] GestionnairePeripherique gestionnairePeripherique; //propri�t� contenant le script GestionnairePeripherique avec l'entieret� de ses fonctions et propri�t�s
    [SerializeField] GameObject plancherTrous; //propri�t� contenant le GameObject PlancherTrous pour �ventuellement l'activer lorsque demand�
    [SerializeField] GameObject plancherPlein; //propri�t� contenant le GameObject PlancherPlein pour �ventuellement le d�sactiver lorsque demand�

    private Camera mainCamera; //propri�t� contenant la mainCamera de la scene

    private GameManager gameManager; //propri�t� du GameManager

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; //instancier la Main Camera dans la propri�t�

        gestionnairePeripherique.clic.AddListener(ProduireClic); //ajout� un �couteur de la propri�t� clic du gestionnairePeripherique et appeler la fonction ProduireClic 

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); //mettre le composant GameManager dans la propri�t�

    }

    private void ProduireClic() //fonction appeler lorsque cliquer
    {
        Vector2 milieuEcran = new Vector2(Screen.width / 2, Screen.height / 2); //Variable Vector2 pour le point milieu de l'�cran

        GameObject objetCollision; //Variable de type GameObject pour avec l'objet en colision


        Ray ray = mainCamera.ScreenPointToRay(milieuEcran); // Variable de type Ray permetant d'avoir une ligne inifini qui par � un point p�cis contenant le milieu de la Main Camera (curseur)
        RaycastHit hit; //variable de type RacastHit contenant l'identit� de l'objet touch� (cliqu�)

        if (Physics.Raycast(ray, out hit) && !gestionnairePeripherique.menuTouches) //si le rayon qui part du point ray touche un point qui a un collider et que le menu objectif n'est pas activer
        {
            if (hit.collider != null) // si le collider de l'objet toucher n'est pas null
            {

                objetCollision = hit.transform.gameObject; // la variable objetCollision = le gameObject du hit


                if(SceneManager.GetActiveScene().name == "LeLaboratoire") //si la scene active est LeLaboratoire
                {


                    if (objetCollision.tag == "Ennemi") //si le tag de l'objetCollision = "Ennemi" 
                    {
                    Destroy(objetCollision); //Dretuire objetCollision
                   
                    gameManager.CalculPoints(); //calcul les points des ennemis tu�s + verifie s'il y a encore des ennemis sur la scene
                    

                    }



                else if(objetCollision.tag == "Objectif" && objetCollision.name == "Disjoncteur_Manette") //si le tag de l'objetCollision = "Objectif" ET son nom = "Disjoncteur_Manette"
                    {

                    if (gameManager.animatorDisjoncteur.GetBool("peutUtiliser")) //si la condition "peutUtiliser" de l'Animator du Disjoncteur dans GameManager est true
                        {
                        
                        objetCollision.GetComponent<Animator>().SetTrigger("persoClic"); //fait un trigger sur la condition "persoClic" de l'Animator de l'objetCollision

                        }
                    
                    }


                else if (objetCollision.tag == "Objectif" && objetCollision.name == "1-Manette") //si le tag de l'objetCollision = "Objectif" et son nom est "1-Manette"
                    {
                    plancherTrous.SetActive(true); //prend la propri�t� plancherTrous et Activer l'objet sur la scene
                    plancherPlein.SetActive(false); //prend la propri�t� plancherPlein et d�sactiver l'objet sur la scene
                        gameManager.bougeDome = true; //prendre la propri�t� bougeDome du GameManager et le mettre a true
                    } 
                
                
                else if (objetCollision.tag == "Robinet" && !gameManager.peutPartir) //si le tag de l'objetCollision = "Robinet" et que la propri�t� peutPartir n'est pas vrai"
                    {

                        gameManager.RobinetToucher(); //appeler la fonction RobinetToucher() du GameManager
                    }



                else if ((objetCollision.tag == "Objectif" && objetCollision.name == "Bouton ON") && gameManager.peutPartir) //si le tag de l'objetCollision = "Objectif" ET son nom est "Bouton ON" ET que la propri�t� peutPartir est vrai
                    {

                        gameManager.YouWin(); //appeler la fonction ChangementScene() dans le GameManager
                    }


                }
                else if(SceneManager.GetActiveScene().name == "RadioactiveCity") //si c'est RadioactiveCity la scene active
                {

                    if(objetCollision.tag == "ObjQuete") //si l'objet en collision est ObjQuete
                    {
                        gameManager.ObjetQueteAcquis();//appeler la fction dans le gameManager ObjetQueteAcquis()
                        Destroy(objetCollision); //Detruit l'objetCollision
                    }
                    else if(objetCollision.tag == "Objectif" && gameManager.objetQuete) //si le tag de l'objetCollis est Objectif ET que le bool objetQuete est vrai
                    {                  
                        Destroy(objetCollision); //Detruit l'objet en collision
                    
                    }
                    else if (objetCollision.tag == "Radio") //si le tag de l'objetCollision est Radio
                    {
                        objetCollision.GetComponent<Radio>().LireProchaineChanson(); //prend le composant radio de l'objetColision et lance la fction LireProchaineChanson()
                    }
                }
                
            }
        }
    }

}
