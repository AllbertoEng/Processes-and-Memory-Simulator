using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRUPaginacao : IEscalonamentoPaginacao
{
    private int[] memoriaFisica;
    private int[] memoriaFisicaTimer;
    private int timer = 1;
    public int SelecionarPosicaoAlocacao(int process)
    {
        //Se ja existe o processo alocado, incrementa o timer
        for (int i = 0; i < memoriaFisica.Length; i++)
        {
            if (memoriaFisica[i] == process)
            {
                memoriaFisicaTimer[i] = timer;
                timer++;
                return -1;
            }
        }

        //Seleciona o processo com menos recentemente utilizado
        int escolhido = 0;
        for (int i = 1; i < memoriaFisica.Length; i++)
        {
            if (memoriaFisicaTimer[i] < memoriaFisicaTimer[i - 1])
            {
                escolhido = i;
            }
        }

        memoriaFisica[escolhido] = process;
        memoriaFisicaTimer[escolhido] = timer;
        timer++;
        return escolhido;
    }

    public void Reset()
    {
        memoriaFisica = new int[5];
        memoriaFisicaTimer = new int[5];
        for (int i = 0; i < memoriaFisica.Length; i++)
        {
            memoriaFisica[i] = -1;
            memoriaFisicaTimer[i] = 0;
        }
        timer = 1;
    }
}
