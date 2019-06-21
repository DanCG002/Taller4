using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pueblo = PNJ.Amigo;
namespace PNJ
{
    namespace Enemigo
    {     
        public class Zombie : MonoBehaviour
        {
            public Info_Zomb info_Zomb;
            bool infectado = false;
            int color_Zomb;
            public string deseo;
            public int azar = 0;
            public float años = 0;
            public float tiempo = 0;
            public bool observacion = false;
            public float vel_Seguir;
            public Estado estado_Zomb;
            public Vector3 orientacion;
            float trecho1;
            float trecho2;
            public bool persecucion = false;
            GameObject heroe, objetivo;
            GameObject[] aldeanos;
     
            IEnumerator localizar_Ald()
            {
                aldeanos = GameObject.FindGameObjectsWithTag("Aldeano");
                heroe = GameObject.FindGameObjectWithTag("Hero");
                foreach (GameObject objeto in aldeanos)
                {
                    yield return new WaitForEndOfFrame();
                    Pueblo.Pueblerino componenteAldeano = objeto.GetComponent<Pueblo.Pueblerino>();
                    if (componenteAldeano != null)
                    {
                        trecho2 = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        trecho1 = Mathf.Sqrt(Mathf.Pow((objeto.transform.position.x - transform.position.x), 2) + Mathf.Pow((objeto.transform.position.y - transform.position.y), 2) + Mathf.Pow((objeto.transform.position.z - transform.position.z), 2));
                        if (!persecucion)
                        {

                            if(trecho1 < 5f)
                            {
                                estado_Zomb = Estado.Seguir;
                                objetivo = objeto;
                                persecucion = true;
                            }
                            else if (trecho2 < 5f)
                            {
                                estado_Zomb = Estado.Seguir;
                                objetivo = heroe;
                                persecucion = true;
                            }
                        }
                        if (trecho1 < 5f && trecho2 < 5f)
                        {
                            objetivo = objeto;
                        }
                    }
                }

                if (persecucion)
                {
                    if (trecho1 > 5f && trecho2 > 5f)
                    {
                        persecucion = false;
                    }
                }
                
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(localizar_Ald());
            }

            public enum Estado
            {
            movimiento, quieto, rotacion, Seguir
            }
        
            public enum Deseo
            {
                piernas, cabeza, cuello, brazos, torso
            }

            void Start()
            {
                if (!infectado)
                {
                    años =Random.Range(15, 101);
                    info_Zomb = new Info_Zomb();
                    color_Zomb = Random.Range(0, 3);
                    Rigidbody infectados;
                    infectados = gameObject.AddComponent<Rigidbody>();
                    infectados.constraints = RigidbodyConstraints.FreezeAll;
                    infectados.useGravity = false;
                    gameObject.name = "Zombie";
                }
                else
                {
                    años = info_Zomb.años;
                    gameObject.name = info_Zomb.apodo;
                }
                StartCoroutine(localizar_Ald());
                vel_Seguir = 10 / años;
                gameObject.tag = "Zombie";
                Deseo Deseo;
                Deseo = (Deseo)Random.Range(0, 5);
                deseo = Deseo.ToString();
                info_Zomb.sabor = deseo;

                if (color_Zomb == 0)
                {
                     gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                }
                if (color_Zomb == 1)
                {
                   gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                if (color_Zomb == 2)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                }
            }

            void Update()
            {
                tiempo += Time.deltaTime;
                if (!persecucion)
                {
                    if (tiempo >= 3)
                    {
                        azar = Random.Range(0, 3);
                        observacion = true;
                        tiempo = 0;
                        if (azar == 0)
                        {
                            estado_Zomb = Estado.quieto;
                        }
                        else if (azar == 1)
                        {
                            estado_Zomb = Estado.rotacion;
                        }
                        else if (azar == 2)
                        {
                            estado_Zomb = Estado.movimiento;
                        }
                    }
                }
                

                switch (estado_Zomb)
                {
                    case Estado.quieto:
                        Debug.Log("Quietud");
                        break;

                    case Estado.Seguir:
                        orientacion = Vector3.Normalize(objetivo.transform.position - transform.position);
                        transform.position += orientacion * vel_Seguir;
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
                }
            }
      
            void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Aldeano")
                {
                    collision.gameObject.AddComponent<Zombie>().info_Zomb = collision.gameObject.GetComponent<PNJ.Amigo.Pueblerino>().info_Ald;
                    collision.gameObject.GetComponent<Zombie>().infectado = true;
                    Destroy(collision.gameObject.GetComponent<PNJ.Amigo.Pueblerino>());
                }

                if (collision.gameObject.tag == "Hero")
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
