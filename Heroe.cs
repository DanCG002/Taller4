using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zomb = PNJ.Enemigo;
using Pueblo = PNJ.Amigo;


public class Heroe : MonoBehaviour
{
    float trecho1, trecho2;
    public float time;
    public Text total_Ald;
    public Text total_Zomb;
    GameObject[] aldeanos, zombie;
    Info_Ald info_Ald = new Info_Ald();
    Info_Zomb info_Zomb = new Info_Zomb();

    void Start()
    {
        Rigidbody hero = gameObject.AddComponent<Rigidbody>();
        gameObject.tag = "Hero";
        gameObject.name = "Hero";
        hero.constraints = RigidbodyConstraints.FreezeAll;
        hero.useGravity = false;
        StartCoroutine(BuscaEntidades());
        total_Zomb = GameObject.FindGameObjectWithTag("Total_Zomb").GetComponent<Text>();
        total_Ald = GameObject.FindGameObjectWithTag("Total_Ald").GetComponent<Text>();
    }

    public void Update()
    {
        time += Time.fixedDeltaTime;
    }

    IEnumerator BuscaEntidades()
    {
        zombie = GameObject.FindGameObjectsWithTag("Zombie");
        aldeanos = GameObject.FindGameObjectsWithTag("Aldeano");

        foreach (GameObject objeto in aldeanos)
        {
            yield return new WaitForEndOfFrame();
            Pueblo.Pueblerino componenteAldeano = objeto.GetComponent<Pueblo.Pueblerino>();
            if (componenteAldeano != null)
            {              
                trecho1 = Mathf.Sqrt(Mathf.Pow((objeto.transform.position.x - transform.position.x), 2) + Mathf.Pow((objeto.transform.position.y - transform.position.y), 2) + Mathf.Pow((objeto.transform.position.z - transform.position.z), 2));
                if (trecho1 < 5f)
                {
                    time = 0;
                    info_Ald = objeto.GetComponent<Pueblo.Pueblerino>().info_Ald;
                    total_Ald.text = "Hola soy un " + info_Ald.apodo + " y he cumpido " + info_Ald.años.ToString() + " años";
                }
                if (time > 3)
                {
                    total_Ald.text = " ";
                }
            }
        }

        foreach (GameObject itemZ in zombie)
        {
            yield return new WaitForEndOfFrame();
            Zomb.Zombie componenteZombie = itemZ.GetComponent<Zomb.Zombie>();
            if (componenteZombie != null)
            {              
                trecho2 = Mathf.Sqrt(Mathf.Pow((itemZ.transform.position.x - transform.position.x), 2) + Mathf.Pow((itemZ.transform.position.y - transform.position.y), 2) + Mathf.Pow((itemZ.transform.position.z - transform.position.z), 2));
                if (trecho2 < 5f)
                {
                    time = 0;
                    info_Zomb = itemZ.GetComponent<Zomb.Zombie>().info_Zomb;
                    total_Zomb.text = "Grrrrrrrrrrrr Comida, comidaaaaa Grrr " + info_Zomb.sabor;
                }
                if (time > 3)
                {
                    total_Zomb.text = " ";
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BuscaEntidades());
    }

    public class MoverH : MonoBehaviour
    {

        Rapidez velocidad;

        private void Start()
        {
            velocidad  = new Rapidez(Random.Range(0.25f, 0.5f));
        }

        private void Update()
        {
            
            if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.Translate(0, 0, -velocidad.agil);
            }
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.Translate(0, 0, velocidad.agil);
            }
        }
    }

    public class MirarH : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.Rotate(0, -3, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.Rotate(0, 3, 0);
            }
        }
    }
}

public class Rapidez
{
    public readonly float agil;
    public Rapidez(float vel)
    {
        agil = vel;
    }
}
