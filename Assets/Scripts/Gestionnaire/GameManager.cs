using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //script pour gerer le UI
using UnityEngine.SceneManagement; //script pour gerer les scenes
using TMPro; //permet d'utiliser le textmeshpro
using System;

public class GameManager : MonoBehaviour //script dans le GameObject GameManager
{

    [SerializeField] private GameObject[] listeEnnemisScene; //propriété contenant un tableau de GameObject des ennemis sur la scene rajouter sur unity (Serializefield)
    
    
    [SerializeField] public Animator animatorDisjoncteur; //propriété accessible sur Unity de type Animator pour le Disjonteur
    [SerializeField] public Animator animatorPanneau; //propriété accessible sur Unity de type Animator pour le Panneau
    [SerializeField] public Animator animatorPanneauTrappe; //propriété accessible sur Unity de type Animator pour la Trappe du Panneau

    [SerializeField] private GameObject domeHaut; //propriété accessible sur Unity pour le GameObject Caisson Haut
    [SerializeField] private GameObject domeBas; //propriété accessible sur Unity pour le GameObject Caisson Bas

    [SerializeField]private GameObject imgWin; //variable de type GameObject pour imgWin
    [SerializeField]private GameObject imgGameOver; //variable de type GameObject pour imgWin
    [SerializeField]private GameObject imgQuete; //variable de type GameObject pour pour l'image de la soudeuse
    [SerializeField]private GameObject panelInfo; //variable de type GameObject pour pour le panel Info UI des menus entre scene

    private GameObject panelMenu; //propriete pour le panelMenu dans le UI


    private GestionnairePeripherique gestionnairePeripherique; //propriété contenant le script GestionnairePeripherique



    [SerializeField] private float vitesseDome = 1f; //propriété accessible sur Unity pour la vitesse de deplacement des domes



    public bool bougeDome = false; //propriété de condition s'il faut bouger le Dome ou pas (public)

    public bool peutPartir = false; //propriété de condition si on peut partir avec le camion ou pas (public)


    public int quantiteEnnemi = 0; //propriété conteur d'ennemi

    public bool objetQuete = false; //propriété bool servant a savoir si l'objet de quete a été ramasser ou non

    private int quantiteObjectifs; //compteur quantiteObjectifs

    public static  int quantitePoints = 0; //propriété des points qui se promene de scene en scene avec le script

    [SerializeField] private int valeurPointsDefaut =5; //valeur de point par défauts des ennemis tué changeable dans unity


    private Text compteurTxt; //propriété du text de compteur d'objectifs du UI
    private Text compteurPointsTxt; //propriété du text compteur points du UI
    private Text menuObjectifUI; //propriété du text dans le menu F1



    private AudioSource jukeboxAudioSource; //propriété pour controler la musique du jukebox dans la class

    [SerializeField] public AudioSource gameManagerAudioSource; //propriété de type AudioSource pour faire jouer des clips audio

    [SerializeField] public AudioClip audioMortDesEnnemis; //propriété audio mis dans Unity pour signaler ennemis tous morts

    [SerializeField] public AudioClip audioRobinet; //propriété audio mis dans Unity pour signaler que le robinet a été tourné
    
    
    [SerializeField] private AudioClip audioWin; //propriété audio mis dans Unity pour signaler que le robinet a été tourné
    [SerializeField] private AudioClip audioGameOver; //propriété audio mis dans Unity pour signaler que le robinet a été tourné

    


    // Start is called before the first frame update
    void Start()
    {
        
        ProprieteCommencementInstance(); //laver la fonction ProprieteCommencementInstance() au commencement de l'instance pour les variables

        
        if(SceneManager.GetActiveScene().name == "LeLaboratoire") //si cest la scene LeLaboratoire qui est actif
        {

            JoueNiv1(); //appel la fonction JoueNiv1()
           
        }
        else if (SceneManager.GetActiveScene().name == "RadioactiveCity") //si cest la scene RadioactiveCity qui est actif
        {

            JoueNiv2(); //appel la fonction JoueNiv2()

        }


        
    }

    

    // Update is called once per frame
    void Update()
    {
       
       

      if (bougeDome && SceneManager.GetActiveScene().name == "LeLaboratoire")BougerDome(); //si bougeDome = true et que le nom de la scene active est LeLaboratoire, appeler la fonction BougerDome()
      
      if (SceneManager.GetActiveScene().name == "LeLaboratoire" || SceneManager.GetActiveScene().name == "RadioactiveCity") VerifMenuObjectif(); //fonction appeler a tous les frames pour verifié si le bouton du menu F1 est enfoncer
        
   
    }


    private void VerifMenuObjectif() //appeler a tout les frame par la fction update()
    {
        if (gestionnairePeripherique.menuTouches) panelMenu.SetActive(true); //le bouton F1 du gestionnairePeripherique est true, met le panelMenu actif
        
        else panelMenu.SetActive(false);//sinon met le panelMenu a false
        
            
        
    }



    private void ProprieteCommencementInstance() //appeler au commencement de l'instance et permet de stocker les propriétés utile de départ
    {   
        
        
        Cursor.lockState = CursorLockMode.Confined;  //confiné le curseur sur la scene


        if(SceneManager.GetActiveScene().name == "LeLaboratoire" || SceneManager.GetActiveScene().name == "RadioactiveCity")
        {
            gestionnairePeripherique = GameObject.Find("GestionnairePeripherique").GetComponent<GestionnairePeripherique>(); //aller chercher le GestionnairePeripherique
            compteurTxt = GameObject.Find("CompteurObjectifUI").GetComponent<Text>(); //aller chercher le text du compteur d'objectif dans l'UI
            compteurPointsTxt = GameObject.Find("CompteurPointsUI").GetComponent<Text>(); //aller chercher le text du compteur de points dans l'UI
            panelMenu = GameObject.Find("PanelMenuUI"); //aller chercher le panel du menu dans l'UI
            menuObjectifUI = GameObject.Find("ObjectifUI").GetComponent<Text>(); //aller chercher le text du menu d'Objectif dans l'UI
            
            Cursor.lockState = CursorLockMode.Locked;  //lock le curseur sur la scene


            if (panelMenu.activeInHierarchy) panelMenu.SetActive(false); //si le UI panelMenu est actif dans la hierarchy, le désactiver
        }
        
        if(SceneManager.GetActiveScene().name == "FinNivLab" || SceneManager.GetActiveScene().name == "Fin") //si cest la scene FinNivLav ou Fin qui est active
        {
            compteurPointsTxt = GameObject.Find("ScoreUI").GetComponent<Text>(); //mettre le ScoreUI dans compteurPointsTxt
            compteurPointsTxt.text = "Votre Score: " + quantitePoints; //mettre le text "Votre Score " + la quantitePoint(static)
            if(SceneManager.GetActiveScene().name == "Fin")
            {
                compteurPointsTxt.text = "Votre Score Final: " + quantitePoints; //mettre le text "Votre Score " + la quantitePoint(static)
            }
        }



    }




    private void JoueNiv1() //appeler dans le start si la condition passe
    {
        for (int i = 0; i < listeEnnemisScene.Length; i++) //boucle de la grosseur du tableau listeEnnemisScene
        {
            float positionX = UnityEngine.Random.Range(-36f, 40f); //variable qui generere une positionen X entre un un range de deux float (-36 et 40)
            float positionZ = UnityEngine.Random.Range(-40f, -11f); //variable qui generere une positionen Z entre un un range de deux float (-40 et -11)

            Instantiate(listeEnnemisScene[i], new Vector3(positionX, -17.62f, positionZ), Quaternion.identity); //permet de faire apparaitre un GameObject dans le tableau de listeEnnemi[i] avec les variable positionX et positionZ
            quantiteEnnemi++; //augmente le compteur de quantiteEnnemi de 1

        }
        compteurTxt.text = "Ennemis à tuer: " + quantiteEnnemi; //mettre le text du compteur de nombre d'ennemi
        menuObjectifUI.text = "Objectifs: " + Environment.NewLine + "Il faut cliquer sur les ennemis pour les éliminers."; //mettre le text du menuObjectif selon le niv
    }
    
    private void JoueNiv2() //appeler dans le start si la condition passe
    {
        jukeboxAudioSource = GameObject.Find("AudioRadio").GetComponent<AudioSource>(); //aller chercher l'AudioSource du GameObject AudioRadio
        quantiteObjectifs = Missile.listeMissiles.Count; //mettre la quantieObjectif a enlever en fonction du count de la liste de missile

        compteurTxt.text = "Missiles Restants: " + quantiteObjectifs; //mettre le text du compteur de nombre de missiles

        menuObjectifUI.text = "Objectifs: " + Environment.NewLine + "Il faut trouver la soudeuse pour enlever les missiles (Lumière Bleue)"; //mettre le text du menuObjectif selon le niv
    }



    

    public void BougerDome() //fonction appeler par update si la condition bougeDome est vrai
    {
        
        Vector3 targetH = new Vector3(domeHaut.transform.position.x, 47.32f, domeHaut.transform.position.z); //variable Vector3 du positionnement a atteindre du domeHaut
        Vector3 targetB = new Vector3(domeBas.transform.position.x, -25.2f, domeBas.transform.position.z); //variable Vector3 du positionnement a atteindre du domeBas
        domeHaut.transform.position = Vector3.MoveTowards(domeHaut.transform.position, targetH, Time.deltaTime * vitesseDome); //changer la position du domeHaut avec la fonction MoveTowards avec la variable targetH
        domeBas.transform.position = Vector3.MoveTowards(domeBas.transform.position, targetB, Time.deltaTime * vitesseDome); //changer la position du domeBas avec la fonction MoveTowards avec la variable targetB
    }

    public void RobinetToucher() //fonction appeler par le script GestionnaireClic apres la condition du Robinet
    {
        
        gameManagerAudioSource.PlayOneShot(audioRobinet); //faire jouer audioRobinet une fois 

        peutPartir = true; //changer la condition peutPartir a true
        
    }
    public void ChangementScene() //fonction appeler par le script GestionnaireClic
    {
        if (SceneManager.GetActiveScene().name == "Intro") SceneManager.LoadScene("LeLaboratoire"); //si la scene active a le nom Niveau1, change la scene pour FinNiveau1
        else if (SceneManager.GetActiveScene().name == "LeLaboratoire") SceneManager.LoadScene("FinNivLab"); //si la scene active a le nom Niveau1, change la scene pour FinNiveau1
        else if (SceneManager.GetActiveScene().name == "FinNivLab") SceneManager.LoadScene("RadioactiveCity"); //si la scene active a le nom Niveau1, change la scene pour FinNiveau1
        else if (SceneManager.GetActiveScene().name == "RadioactiveCity") SceneManager.LoadScene("Fin"); //si la scene active a le nom Niveau1, change la scene pour FinNiveau1
        else if (SceneManager.GetActiveScene().name == "Fin") SceneManager.LoadScene("Intro"); //si la scene active a le nom Niveau1, change la scene pour FinNiveau1

    }
    public void ObjetQueteAcquis() //appeler par une fction de clic du GestionnaireClic
    {
        gameManagerAudioSource.PlayOneShot(audioMortDesEnnemis); //joue un audio
        imgQuete.SetActive(true); //metre l'imgQuete du UI actif
        objetQuete = true; //et mettre le bool objetQuete a true
    }

    public void ObjObjectifAccumule(int pointObj, int objRestants) //appeler par le script Missile OnDestroy
    {
        quantitePoints += pointObj; //addition de quantite point selon la valeur donné dans la fction
        quantiteObjectifs = objRestants; //mise a jour quantite Objectifs selon la valeur donné dans la fction

        if (quantiteObjectifs<=1)compteurTxt.text = "Missile Restant: " + quantiteObjectifs; //si la valeur de la quantie objectifs est plus petit ou egal a 1

        else compteurTxt.text = "Missiles Restants: " + quantiteObjectifs;

        compteurPointsTxt.text = "Points: " + quantitePoints; // mettre a jours le text compteurPointTxt a la nouvelle valeur

        if (quantiteObjectifs <= 0) // si la quantiteObjectif est plus petit ou egal a 0
        {
            jukeboxAudioSource.Stop(); //arreter l'audio du jukebox
            YouWin(); //appeler la fonction YouWin()
        }
        
    }

    public void CalculPoints() //fonction accessible externe pour calculer les points
    {
        quantitePoints += valeurPointsDefaut; //augmente la quantitPoints avec la valeurPointsDefaut
        compteurPointsTxt.text = "Points: " + quantitePoints; //met a jour le montant additionner sur l'UI
        quantiteEnnemi--; //enleve un ennemi de la quantite
        VerifEnnemi(); //appel la fonction pour verifier le nombre d'ennemi restant
    }
    
    public void VerifEnnemi() //fonction public appeler par CalculPoint()
    {
        compteurTxt.text = "Ennemis à tuer: " + quantiteEnnemi; //met a jour le nombre d'ennemi a tuer restant sur la map
        if(quantiteEnnemi<=1) compteurTxt.text = "Ennemi à tuer: " + quantiteEnnemi; //si la quantite est plus petit 
        if (quantiteEnnemi == 0) //si le compteur quantiteEnnemi = 0
        {
            gameManagerAudioSource.PlayOneShot(audioMortDesEnnemis); //joue le son une seul fois audioMortDesEnnemis
            animatorDisjoncteur.SetBool("peutUtiliser", true); //met la condition peutUtiliser a true du animatorDisjoncteur
        }
    }


    public void YouWin() //fonction accessible externe pour lorsqu'on gagne
    {
        imgWin.SetActive(true); //mettre l'imgWin du UI a actif
        gameManagerAudioSource.PlayOneShot(audioWin); //faire jouer un son audioWin
        Invoke("ChangementScene",5f); //lance la fction ChangementScene apres 5 seconde
    }

    public void BoutonInfo()
    {
        panelInfo.SetActive(true);
    }

    public void FermerInfo()
    {
        panelInfo.SetActive(false);
    }

    public void SkipNiv1()
    {
        SceneManager.LoadScene("FinNivLab");
    }


}
