using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AG : MonoBehaviour
{

    private int pivote = 0;
    private int generaciones = 25;
    private List<List<Individuo>> poblacion = new List<List<Individuo>>();

    public int seleccion = 80;      //probabilidad de seleccion.                    80%
    public int cruza = 70;          //Probabilidad de que se realice una cruza.     70%
    public int mutacion = 10;       //Probabilidad de que se realice una mutacion.  10%
    public Text texto;
    public Text nGen;
    public GameObject s;

    public GameObject tableroPF;
    private Transform ind;
    private Transform reina;
    private GameObject tablero;
    private SpriteRenderer sR;

    private Individuo indAux;
    private Individuo indAux2;
    private Individuo sel1;
    private Individuo sel2;
    private int isel1, isel2, a;
    

    bool b_solucion = false;
    int solucion;
    int g_solucion;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            /*******************************************************************************************
             *******************************************************************************************
             *                             Inicio algoritmo genetico                                   *
             *******************************************************************************************
             *******************************************************************************************/

            //Generar una poblacion inicial
            //Ejecutar la funcion de evaluacion para cada individuo  
            for (int i = 0; i < 25; i++)
            {
                poblacion.Add(new List<Individuo>());
            }
            for (int i = 0; i < 25; i++)
            {
                poblacion[0].Add(new Individuo(true));
            }

            //Aplicar generaciones
            for (int i = 0; i < generaciones; i++)
            {

                //verifica si se ha encontrado alguna solucion en la generacion con la evaluacion=0; que significa que ninguna reina se ataca
                for (int j = 0; j < 25; j++)
                {
                    if (poblacion[i][j].Evaluar() == 0)
                    {
                        //guarda el individuo y la generacion de la solucion
                        solucion = j;
                        g_solucion = i;
                        b_solucion = true;
                        break;
                    }
                }
                if (b_solucion || i == 24)
                    break;
                //Ordenar la poblacion por su evaluacion de menor a mayor
                for (int j = 0; j < 25; j++)
                {
                    for (int k = 0; k < (24 - j); k++)
                    {
                        if (poblacion[i][k].evaluacion > poblacion[i][k + 1].evaluacion)
                        {
                            indAux = poblacion[i][k];
                            poblacion[i][k] = poblacion[i][k + 1];
                            poblacion[i][k + 1] = indAux;
                        }
                    }
                }
                sel1 = null;
                sel2 = null;
                //Realizar seleccion, cruza y/o mutacion
                for (int j = 0; j < 25; j++)
                {

                    if ((seleccion - (j * 2)) > UnityEngine.Random.Range(0, 101))  //probabilidad de seleccion baja 0.02 con cada elemento siendo el ultimo elemento con
                    {                                                  //una probabilidad de seleccion de 0.3 y el primero de 0.8

                        if (sel1 == null && sel2 == null)                        //Seleccion 1 para la cruza
                        {
                            sel1 = poblacion[i][j];
                            isel1 = j;
                            poblacion[i][j].selected = true;
                            continue;                           //Continuar para que sel2 no sea igual a sel1
                        }
                        if (sel1 != null && sel2 == null)       //Seleccion 2 para la cruza
                        {
                            sel2 = poblacion[i][j];
                            isel2 = j;
                            poblacion[i][j].selected = true;
                        }
                    }
                    else
                    {
                        poblacion[i + 1].Add(new Individuo(poblacion[i][j].genes));
                    }

                    if (sel1 != null && sel2 != null) //Una vez seleccionados dos indivduos se realiza la cruza
                    {
                        if (cruza > UnityEngine.Random.Range(0, 101))
                        {
                            int i1 = UnityEngine.Random.Range(1, 4);
                            int i2 = UnityEngine.Random.Range(1, 4);
                            indAux = new Individuo(false);
                            indAux2 = new Individuo(false);
                            for (int k = 0; k < i1; k++)
                            {
                                indAux.genes[k] = sel1.genes[k];
                                indAux2.genes[k] = sel2.genes[k];
                            }
                            for (int k = i1; k < i2; k++)
                            {
                                indAux.genes[k] = sel2.genes[k];
                                indAux2.genes[k] = sel1.genes[k];
                            }
                            for (int k = i2; k < 5; k++)
                            {
                                indAux.genes[k] = sel1.genes[k];
                                indAux2.genes[k] = sel2.genes[k];
                            }
                            sel1.crossed = true;
                            sel2.crossed = true;
                            poblacion[i + 1].Add(new Individuo(indAux.genes));
                            poblacion[i + 1].Add(new Individuo(indAux2.genes));
                        }
                        else
                        {
                            poblacion[i + 1].Add(indAux);
                            poblacion[i + 1].Add(indAux2);
                        }
                        if (mutacion > UnityEngine.Random.Range(0, 101)) //mutacion de la seleccion 1
                        {
                            sel1.mutated = true;
                            //seleccion de genes a intercambiar
                            int i1 = UnityEngine.Random.Range(0, 5);
                            int i2 = UnityEngine.Random.Range(0, 5);
                            //intercambio de genes
                            a = indAux.genes[i1];
                            indAux.genes[i1] = indAux.genes[i2];
                            indAux.genes[i2] = a;
                        }

                        if (mutacion > UnityEngine.Random.Range(0, 101)) //mutacion de la seleccion 2
                        {
                            sel2.mutated = true;
                            //seleccion de genes a intercambiar
                            int i1 = UnityEngine.Random.Range(0, 5);
                            int i2 = UnityEngine.Random.Range(0, 5);
                            //intercambio de genes
                            a = indAux2.genes[i1];
                            indAux2.genes[i1] = indAux.genes[i2];
                            indAux2.genes[i2] = a;
                        }
                        sel1 = null;
                        sel2 = null;
                    }
                }
                if (sel1 != null)
                {
                    poblacion[i + 1].Add(sel1);
                }
            }

            if (b_solucion)
            {
                texto.text = "Solucion encontrada: \n";
                texto.text += "Generacion = " + g_solucion + "\tIndividuo = " + solucion + "\n";
                texto.text += "R1= " + poblacion[g_solucion][solucion].genes[4] + "\n";
                texto.text += "R2= " + poblacion[g_solucion][solucion].genes[3] + "\n";
                texto.text += "R3= " + poblacion[g_solucion][solucion].genes[2] + "\n";
                texto.text += "R4= " + poblacion[g_solucion][solucion].genes[1] + "\n";
                texto.text += "R5= " + poblacion[g_solucion][solucion].genes[0] + "\n";

                ImprimirTablero(poblacion[g_solucion][solucion], 1.0f, new Vector3(-7.9f, -4f, 0.0f));
            }
            else
            {
                texto.text = "Solucion no encontrada. \n";
            }
            /******************************************************************************************
            *******************************************************************************************
            *                                Fin algoritmo genetico                                   *
            *******************************************************************************************
            *******************************************************************************************/
        }
        catch(NullReferenceException ex){
            Debug.Log("Error");
            Reiniciar();
        }

        //Imprimir lo que contiene poblacion[][]
        nGen.text = "Generacion: "+ pivote;
        for (int i = 0; i < 25; i++)
        {
            ind = s.transform.GetChild(i);
            for (int k = 0; k < 5; k++)
            {
                reina = ind.transform.GetChild(k);
                reina.transform.localPosition = new Vector3(poblacion[pivote][i].genes[k], k, 0.0f);
            }
            sR = ind.GetComponent<SpriteRenderer>();
            if (poblacion[pivote][i].mutated)
                sR.color = Color.red;
            else if (poblacion[pivote][i].crossed)
                sR.color = Color.yellow;
            else if (poblacion[pivote][i].selected)
                sR.color = Color.green;
        }
    }

    void ImprimirTablero(Individuo individuo, float escala, Vector3 posicion)
    {
        tablero = Instantiate(tableroPF, posicion, Quaternion.identity);
        tablero.transform.localScale = new Vector3(escala, escala, 0.0f);
        for (int k = 0; k < 5; k++)
        {
            reina = tablero.transform.GetChild(k);
            reina.transform.localPosition = new Vector3(individuo.genes[k], k, 0.0f);
        }
    }

    public void Next()
    {
        if (pivote < 24)
        {
            pivote++;
            if (poblacion[pivote].Count != 0)
            {
                nGen.text = "Generacion: " + pivote;
                for (int i = 0; i < 25; i++)
                {
                    ind = s.transform.GetChild(i);
                    for (int k = 0; k < 5; k++)
                    {
                        reina = ind.transform.GetChild(k);
                        reina.transform.localPosition = new Vector3(poblacion[pivote][i].genes[k], k, 0.0f);
                    }
                    sR = ind.GetComponent<SpriteRenderer>();
                    if (poblacion[pivote][i].mutated)
                        sR.color = Color.red;
                    else if (poblacion[pivote][i].crossed)
                        sR.color = Color.yellow;
                    else if (poblacion[pivote][i].selected)
                        sR.color = Color.green;
                    else
                        sR.color = Color.white;
                }
            }
            else
            {
                nGen.text = "Generacion: " + pivote + "\n(no generada)";
                for (int i = 0; i < 25; i++)
                {
                    ind = s.transform.GetChild(i);
                    for (int k = 0; k < 5; k++)
                    {
                        reina = ind.transform.GetChild(k);
                        reina.transform.localPosition = new Vector3(0.0f, k, 0.0f);
                    }
                    sR = ind.GetComponent<SpriteRenderer>();
                    sR.color = Color.white;
                }
            }
        }
    }

    public void Previous()
    {
        if (pivote > 0)
        {
            pivote--;
            if (poblacion[pivote].Count != 0)
            {
                nGen.text = "Generacion: " + pivote;
                for (int i = 0; i < 25; i++)
                {
                    ind = s.transform.GetChild(i);
                    for (int k = 0; k < 5; k++)
                    {
                        reina = ind.transform.GetChild(k);
                        reina.transform.localPosition = new Vector3(poblacion[pivote][i].genes[k], k, 0.0f);
                    }
                    sR = ind.GetComponent<SpriteRenderer>();
                    if (poblacion[pivote][i].mutated)
                        sR.color = Color.red;
                    else if (poblacion[pivote][i].crossed)
                        sR.color = Color.yellow;
                    else if (poblacion[pivote][i].selected)
                        sR.color = Color.green;
                    else
                        sR.color = Color.white;
                }
            }
            else
            {
                nGen.text = "Generacion: " + pivote + "\n(no generada)";
                for (int i = 0; i < 25; i++)
                {
                    ind = s.transform.GetChild(i);
                    for (int k = 0; k < 5; k++)
                    {
                        reina = ind.transform.GetChild(k);
                        reina.transform.localPosition = new Vector3(0.0f, k, 0.0f);
                    }
                    sR = ind.GetComponent<SpriteRenderer>();
                    sR.color = Color.white;
                }
            }
        }
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(0);
    }

    public void Cerrar()
    {
        Application.Quit();
    }
}
