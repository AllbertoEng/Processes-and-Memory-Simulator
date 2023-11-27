using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIFOPaginacao : IEscalonamentoPaginacao
{
    private int[] memoriaFisica;
    private int pointer;    
    public int SelecionarPosicaoAlocacao(int process)
    {
        foreach (int p in memoriaFisica)
        {
            if (p == process)
                return -1;
        }

        memoriaFisica[pointer] = process;
        int selecionado = pointer;
        pointer++;
        if (pointer >= memoriaFisica.Length)
            pointer = 0;
        return selecionado;
    }
    public void Reset()
    {
        memoriaFisica = new int[5];
        for (int i = 0; i < memoriaFisica.Length; i++)
            memoriaFisica[i] = -1;
        pointer = 0;        
    }
}
