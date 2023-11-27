using System;
using System.Collections.Generic;
using UnityEngine;

public class SJF : IEscalonamento
{
    public float Resposta { get; set; }
    public float Quantum { get; set; }
    public float Sobrecarga { get; set; }

    private int tempoFinalizacaoProcessos = 0;
    private int menorTempoExecucao = int.MaxValue;

    public void CalcularProximoMovimento(Cursor cursor, List<Processo> processos, int tempo)
    {
        bool inserirBlocoEspera = true;
        int linhaParaMovimentar = -1;


        for (int i = 0; i < processos.Count; i++)
        {
            if (!processos[i].Finalizado && processos[i].TempoExecucao > 0 && processos[i].Ativo)
            {
                if (linhaParaMovimentar == -1)
                {
                    linhaParaMovimentar = i;
                    menorTempoExecucao = processos[i].TempoExecucao;
                }
                else if (processos[i].TempoExecucao <= menorTempoExecucao)
                {
                    linhaParaMovimentar = i;
                    menorTempoExecucao = processos[i].TempoExecucao;
                }

                inserirBlocoEspera = false;
            }

        }

        if (inserirBlocoEspera)
            cursor.MoverLinha(1, Status.Vazio);
        else
        {
            processos[linhaParaMovimentar].TempoExecucao -= 1;
            cursor.MoverLinha(linhaParaMovimentar + 1, Status.Executando);

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

