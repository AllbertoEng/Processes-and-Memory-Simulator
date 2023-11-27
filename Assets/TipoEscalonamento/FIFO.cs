using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FIFO : IEscalonamento
{
    public float Resposta { get; set; }
    public float Quantum { get; set; }
    public float Sobrecarga { get; set; }

    private int tempoFinalizacaoProcessos = 0;
    
    public void CalcularProximoMovimento(Cursor cursor, List<Processo> processos, int tempo)
    {
        bool inserirBlocoEspera = true;
        int linhaParaMovimentar = -1;

        //Algoritmo pra escolher o processo a ser executado
        for (int i = 0; i < processos.Count; i++)
        {
            //Algoritmo pra selecionar dentre os processos válidos o que tem o menor tempo de chegada
            if (!processos[i].Finalizado && processos[i].TempoExecucao > 0 && processos[i].Ativo)
            {
                if (linhaParaMovimentar == -1)
                    linhaParaMovimentar = i;
                else if (processos[linhaParaMovimentar].TempoChegada > processos[i].TempoChegada)
                    linhaParaMovimentar = i;

                inserirBlocoEspera = false;                                  
            }
        }

        //Caso não exista nenhum processo ativo, insere bloco de espera
        if (inserirBlocoEspera)        
            cursor.MoverLinha(1, Status.Vazio);
        else
        {
            //Reduz tempo de execucao restante do processo e escreve uma linha visual
            processos[linhaParaMovimentar].TempoExecucao -= 1;
            cursor.MoverLinha(linhaParaMovimentar + 1, Status.Executando);

            //Se finalizado, fecha o processo e adiciona ao tempo total de execucao
            if (processos[linhaParaMovimentar].TempoExecucao == 0)
            {
                processos[linhaParaMovimentar].Finalizado = true;
                tempoFinalizacaoProcessos += tempo;
            }
        }

        CalcularResposta(processos);
    }

    private void CalcularResposta(List<Processo> processos)
    {
        int tempoEsperaTotal = 0;
        foreach (Processo processo in processos)        
            tempoEsperaTotal += processo.TempoChegada;

        Resposta = (float)(tempoFinalizacaoProcessos - tempoEsperaTotal) / processos.Count;
    }
}
