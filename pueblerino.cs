using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zomb = PNJ.Enemigo;

namespace PNJ
{
    namespace Amigo
    {       
        public class Pueblerino : MonoBehaviour
        {
            public Info_Ald info_Ald = new Info_Ald();
            public Estado estadoVillager;
            float tiempo;
            public float distancia;
            public float rapidez_Escape;
            public float años;
            int azar;
            public bool estado_Escape = false;
            bool observacion = false;
            public Vector3 orientacion;
            GameObject objetivo;
            GameObject[] aldeanos;

            public enum Nombres
            {
                Fred, Carlos, Jeremy, Annie, Carmen, Andres, Canela, Robin,
                Arturo, Otoniel, Robert, Fausto, Javier, Hernan, Larry, Gregorio, Zac,
                Victor, Rosa, Daniela
            }
          
            public enum Estado
            {
                quieto, movimiento, rotacion, escape
            }

            IEnumerator localizar_Zomb()
            {
                aldeanos = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (GameObject objeto in aldeanos)
                {
                    Zomb.Zombie componenteZombie = objeto.GetComponent<Zomb.Zombie>();
                    if (componenteZombie != null)
                    {
                        distancia = Mathf.Sqrt(Mathf.Pow((objeto.transform.position.x - transform.position.x), 2) + Mathf.Pow((objeto.transform.position.y - transform.position.y), 2) + Mathf.Pow((objeto.transform.position.z - transform.position.z), 2));
                        if (!estado_Escape)
                        {
                            if (distancia < 5f)
                            {
                                estadoVillager = Estado.escape;
                                objetivo = objeto;
                                estado_Escape = true;
                            }
                        }
                    }
                }

                if (estado_Escape)
                {
                    if (distancia > 5f)
                    {
                        estado_Escape = false;
                    }
                }

                yield return new WaitForSeconds(0.1f);
                StartCoroutine(localizar_Zomb());
            }
           
            void Start()
            {
                Rigidbody aldea;
                gameObject.tag = "Aldeano";
                aldea = gameObject.AddComponent<Rigidbody>();
                aldea.constraints = RigidbodyConstraints.FreezeAll;
                aldea.useGravity = false;
                Nombres apodo;
                apodo = (Nombres)Random.Range(0, 20);
                info_Ald.apodo = apodo.ToString();
                años = Random.Range(15, 101);
                info_Ald.años = (int)años;
                rapidez_Escape = 10 / años;
                gameObject.name = apodo.ToString();
                StartCoroutine(localizar_Zomb());
            }

          
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!estado_Escape)
                {
                    if (tiempo >= 3)
                    {
                        azar = Random.Range(0, 3);
                        observacion = true;
                        tiempo = 0;
                        if (azar == 0)
                        {
                            estadoVillager = Estado.quieto;
                        }
                        else if (azar == 1)
                        {
                            estadoVillager = Estado.movimiento;
                        }
                        else if (azar == 2)
                        {
                            estadoVillager = Estado.rotacion;

                        }
                    }
                }

                switch (estadoVillager)
                {
                    case Estado.quieto:
                        Debug.Log("Aldeano Quieto");
                        break;

                    case Estado.rotacion:
                        gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;

                    case Estado.movimiento:
                        if (observacion)
                        {
                            gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        gameObject.transform.Translate(0, 0, 0.05f);
                        observacion = false;
                        break;

                    case Estado.escape:
                        orientacion = Vector3.Normalize(objetivo.transform.position - transform.position);
                        transform.position -= orientacion * rapidez_Escape;
                        break;
                }
            }
        }
    }
}

