using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individuo
{

    public int[] genes = new int[5];
    public int evaluacion;
    public bool selected;
    public bool crossed;
    public bool mutated;

    public Individuo(bool inicializar)
    {
        if (inicializar)
        {
            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = Random.Range(0, 5);  //Regresa un numero randon entre 0 - 4
            }
            evaluacion = Evaluar();
        }
        selected = false;
        crossed = false;
        mutated = false;
    }

    public Individuo(int[] genes)
    {
        this.genes = genes;
        evaluacion = Evaluar();
        selected = false;
        crossed = false;
        mutated = false;
    }

    public int Evaluar()
    {
        int ataque = 0;
        for(int i=0; i<genes.Length-1; i++)
        {
            for(int j = i+1; j<genes.Length; j++)
            {
                if ((genes[i] == genes[j]) || (j-i == Mathf.Abs(genes[j]-genes[i])))
                {
                    ataque++;
                }

            }
        }
        return ataque;
    }
}
