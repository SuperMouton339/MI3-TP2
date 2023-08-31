using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] private AudioClip[] listeMusiques; //propriete AudioClip tableau listeMusiques

    private GestionnairePeripherique gestionnairePeripherique; //propriete pour le GestionnairePeripherique

    private AudioSource audioRadio; //propriete pour l'AudioSour de la radio

    private int indexListe = 0; //compteur pour le compteur du tableau et les chansons

    // Start is called before the first frame update
    void Start()
    {
        //aller chercher les composants des GameObjects et les mettre dans leur propriete respectifs en partant la premiere chanson de la listeMusiques
        audioRadio = GameObject.Find("AudioRadio").GetComponent<AudioSource>();
        gestionnairePeripherique = GameObject.Find("GestionnairePeripherique").GetComponent<GestionnairePeripherique>();
        audioRadio.PlayOneShot(listeMusiques[indexListe]);
    }

    public void Update()//appeler a chaque frame
    {
        VerifJoue(); //fction pour verifié si le bouton menu est enfoncer (F1)
    }

    private void VerifJoue() //appeler par l'update
    {
        if (gestionnairePeripherique.menuTouches) //si la condition menTouches est vrai du gestionnairePeripherique
        {
            audioRadio.Pause(); //mettre la radio a pause

        }
        else //sinon
        {
            audioRadio.UnPause(); //repartir la radio
        }
    }

    public void LireProchaineChanson() //appeler par le GestionnaireClic
    {
        indexListe++; //Augment le competeur indexListe de 1

       if(indexListe >= listeMusiques.Length) //si l'indexListe est plus grand ou egal a la grandeur du tableau listeMusiques
        {
            audioRadio.Stop(); //arrete la chanson actuel
            indexListe = -1;//remet le compteur a -1
        }
        else//sinon
        {
            audioRadio.Stop();//arrete la chanson en cours
            audioRadio.PlayOneShot(listeMusiques[indexListe]);//fait jouer la chanson suivante dans le tableau listeMusiques
        }


    }


}
