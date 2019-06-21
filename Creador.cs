using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zomb = PNJ.Enemigo;
using Pueblo = PNJ.Amigo;

public class Creador : MonoBehaviour
{
    public Text cantidadZombies;
    public Text cantidadVillagers;
    public int varZombies;
    public int cant_Ald;
    public GameObject[] infectados,aldea;
    void Start()
    {
        new CrearInstancias();
    }

    private void Update()
    {
        infectados = GameObject.FindGameObjectsWithTag("Zombie");
        aldea = GameObject.FindGameObjectsWithTag("Aldeano");
        foreach (GameObject item in infectados)
        {
            varZombies = infectados.Length;
        }
        foreach (GameObject item in aldea)
        {
            cant_Ald = aldea.Length;
        }

        if(aldea.Length == 0)
        {
            cantidadVillagers.text = 0.ToString();
        }
        else
        {
            cantidadVillagers.text = cant_Ald.ToString();
        }

        cantidadZombies.text = varZombies.ToString();
    }
}

 class CrearInstancias 
{
    public GameObject cube;
    public readonly int minInstancias = Random.Range(5, 16);
    int escoger = 0;
    const int MAX = 26;
    public CrearInstancias()
    {
        for (int i = 0; i < Random.Range(minInstancias,MAX); i++)
        {
            
            if (escoger == 0)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<Camera>();
                cube.AddComponent<Heroe>();
                cube.AddComponent<Heroe.MirarH>();
                cube.AddComponent<Heroe.MoverH>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                escoger += 1;
            }

            int selec = Random.Range(escoger, 3);

            if (selec == 1)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<Pueblo.Pueblerino>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if (selec == 2)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<Zomb.Zombie>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
        }
    }
}