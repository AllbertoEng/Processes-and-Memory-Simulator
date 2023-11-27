using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] public List<Processo> Processos;
    [SerializeField] TMP_Dropdown DropdownQtdProcessos;

    [SerializeField] GameObject TipoEscalonamento;
    [SerializeField] GameObject TipoPaginacao;
    [SerializeField] TMP_Dropdown TipoSimulacao; 
    public void OnQuantidadeProcessosChange()
    {        
        foreach (Processo processo in Processos)
        {
            processo.gameObject.SetActive(true);
            processo.Ativo = true;
        }

        for (int i = Processos.Count-1; i > Convert.ToInt16(DropdownQtdProcessos.value); i--)
        {
            Processos[i].Ativo = false;
            Processos[i].gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnTipoSimulacaoChange()
    {
    }
}
