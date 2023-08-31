using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiverPanneau : MonoBehaviour   //script a l'intérieur du disjoncteur pour faire animer le disjoncteur a la fin de l'animation
{
    [SerializeField] GameManager gameManager; //propriéter pour mettre le script du GameObject GameManager
   

    public void OuvrirPanneau() // script appeler a la fin de l'animation pour activer le trigger des animations du panneau
    {
        gameManager.animatorPanneau.SetTrigger("disjActiver"); //trigger pour l'animation du panneau
        gameManager.animatorPanneauTrappe.SetTrigger("disjActiver"); //trigger pour l'animation de la trappe du panneau

    }
}
