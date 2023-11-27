using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoundRobin : IEscalonamento
{
    public float Resposta { get; set; }
    public float Quantum { get; set; }
    public float Sobrecarga { get; set; }

    private int tempoFinalizacaoProcessos = 0;
    private int tempoQuantum = 0;
    private int indiceProcessoAtual = 0;

    public void CalcularProximoMovimento(Cursor cursor, List<Processo> processos, int tempo)
    {
        bool inserirBlocoEspera = true;
        int linhaParaMovimentar = -1;
        bool todosConcluidos = true;

        for (int i = 0; i < processos.Count; i++)
        {
            if (!processos[i].Finalizado && processos[i].TempoExecucao > 0 && processos[i].Ativo)
            {
                todosConcluidos = false;
                if (linhaParaMovimentar == -1)
                    linhaParaMovimentar = i;
                inserirBlocoEspera = false;
            }
        }

        if (todosConcluidos)
            return;

        if (inserirBlocoEspera)
        {
            cursor.MoverLinha(1, Status.Vazio);
            indiceProcessoAtual = 0;
        }
        else
        {
            if (tempoQuantum >= Quantum || processos[indiceProcessoAtual].TempoExecucao == 0)
            {
                if (Sobrecarga != 0 && processos[indiceProcessoAtual].TempoExecucao > 0)
                {
                    for (int j = 0; j < (int)Sobrecarga; j++)
                    {
                        cursor.MoverLinha(indiceProcessoAtual + 1, Status.Sobrecarregando);
                    }
                }

                if (processos[indiceProcessoAtual].TempoExecucao == 0)
                {
                    processos[indiceProcessoAtual].Finalizado = true;
                    tempoFinalizacaoProcessos += tempo;
                }

                tempoQuantum = 0;
                indiceProcessoAtual = (indiceProcessoAtual + 1) % processos.Count;
            }
            else
            {
                tempoQuantum++;
                processos[indiceProcessoAtual].TempoExecucao--;
                cursor.MoverLinha(indiceProcessoAtual + 1, Status.Executando);
            }

        }
        if (processos[indiceProcessoAtual].TempoExecucao == 0)
            processos[indiceProcessoAtual].Finalizado = true;

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
