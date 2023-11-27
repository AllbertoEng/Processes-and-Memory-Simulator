using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDF : IEscalonamento
{
    public float Resposta { get; set; }
    public float Quantum { get; set; }
    public float Sobrecarga { get; set; }

    private int tempoFinalizacaoProcessos = 0;
    private int totalQuantum = 0;
    private int sobrecargaAtual = 0;

    public void CalcularProximoMovimento(Cursor cursor, List<Processo> processos, int tempo)
    {
        bool inserirBlocoEspera = true;
        int linhaParaMovimentar = -1;

        for (int i = 0; i < processos.Count; i++)
        {
            if (!processos[i].Finalizado && processos[i].TempoExecucao > 0 && processos[i].Ativo)
            {
                if (linhaParaMovimentar == -1)
                    linhaParaMovimentar = i;
                else if (processos[linhaParaMovimentar].TempoChegada >= processos[i].TempoChegada && processos[linhaParaMovimentar].Deadline >= processos[i].Deadline)
                    linhaParaMovimentar = i;

                inserirBlocoEspera = false;
            }
        }

        if (sobrecargaAtual == 0)
            sobrecargaAtual = (int)Sobrecarga;



        if (inserirBlocoEspera)
        {
            cursor.MoverLinha(1, Status.Vazio);
        }
        else if ((int)Sobrecarga > 0 && Quantum > 0 && (int)totalQuantum == Quantum)
        {

            cursor.MoverLinha(linhaParaMovimentar + 1, Status.Sobrecarregando);
            sobrecargaAtual -= 1;

            if (sobrecargaAtual == 0)
            {
                totalQuantum = 0;
            }

        }
        else
        {
            totalQuantum += 1;
            processos[linhaParaMovimentar].TempoExecucao -= 1;

            if (processos[linhaParaMovimentar].Deadline < tempo)
                cursor.MoverLinha(linhaParaMovimentar + 1, Status.Deadline);
            else
                cursor.MoverLinha(linhaParaMovimentar + 1, Status.Executando);

            if (processos[linhaParaMovimentar].TempoExecucao == 0)
            {
                processos[linhaParaMovimentar].Finalizado = true;
                tempoFinalizacaoProcessos += tempo;
                totalQuantum = 0;
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
