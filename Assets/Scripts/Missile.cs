using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public static List<Missile> listeMissiles = new List<Missile>(); //propriété liste relié a la class et son instance

    public int points=10; //propriété de listeMissiles pour les points en int

    
    [SerializeField] private GameObject particule; //mettre le prefabs particule dans Unity (SerializeField)


    private GameManager gameManager; //propriete pour le GameManager

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); //on va chercher le GameManager sur la scene
    }


    public void OnEnable() //lorsque l'instance commence
    {
        
       
        listeMissiles.Add(this);  //rajoute cet instance a la listeMissiles
        
    }

    public void OnDisable() //lorsque l'instance arrete
    {
        listeMissiles.Remove(this); //enleve cet instance de la listeMissiles
        
        
        
        
    }
    public void OnDestroy() //lorsque l'objet de l'instance est Detruite
    {
        Instantiate(particule, transform.position, Quaternion.identity); //creer un GameObject particule a la position de l'objet detrui et avec la meme identité de Rotation
        gameManager.ObjObjectifAccumule(this.points, listeMissiles.Count); //appel la fction ObjObectifAccumule du GameManager avec la valeur des point de cet instance et le count de la listeMissiles
    }






}
