using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaginacaoController : MonoBehaviour
{
    [SerializeField] List<ProcessTag> ProcessTags;
    [SerializeField] CursorPaginacao Cursor;

    private int[] memoriaVirtual;
    public int QuantidadeProcessos;

    public IEscalonamentoPaginacao EscalonamentoPaginacao;

    private void Start()
    {
        ResetVirtualMemory();
    }

    public void AlocarProcessoMemoriaFisica(int process)
    {
        int posicaoAlocada = EscalonamentoPaginacao.SelecionarPosicaoAlocacao(process);
        if (posicaoAlocada == -1)
            return;

        int processoAntigo = Cursor.ReturnProcessOnColumn(posicaoAlocada);

        if (processoAntigo != -1)
        {
            int posicaoVirtual = 0;
            for (int i = 0; i < memoriaVirtual.Length; i++)
            {
                if (memoriaVirtual[i] == -1)
                {
                    posicaoVirtual = i;
                    memoriaVirtual[i] = processoAntigo;
                    break;
                }
            }
            Cursor.PreencherColuna(posicaoVirtual + 6, ProcessTags[processoAntigo].Color, processoAntigo);
        }            

        Cursor.PreencherColuna(posicaoAlocada, ProcessTags[process].Color, process);

        foreach (int number in memoriaVirtual)
        {
            if (number == process)
            {
                memoriaVirtual[number] = -1;
                Cursor.RemoveFillerByCollum(number + 6, true);
                break;
            }                
        }
    }

    public void Reset()
    {
        EscalonamentoPaginacao.Reset();
        Cursor.Reset();
    }

    public void ResetVirtualMemory()
    {
        memoriaVirtual = new int[QuantidadeProcessos];       

        foreach (ProcessTag processo in ProcessTags)        
            processo.gameObject.SetActive(true);       

        for (int i = ProcessTags.Count - 1; i >= QuantidadeProcessos; i--)
        {
            ProcessTags[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < memoriaVirtual.Length; i++)
        {
            memoriaVirtual[i] = i;
            Cursor.PreencherColuna(i + 6, ProcessTags[i].Color, i);
        }
    }
}
