using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Processo : MonoBehaviour
{
    public bool Finalizado { get; set; }
    public bool Ativo { get; set; }
    public int TempoChegada { get; set; }
    public int TempoExecucao { get; set; }
    public int Deadline { get; set; }
    public int Prioridade { get; set; }

    [SerializeField] TMP_InputField TempoChegadaInput;
    [SerializeField] TMP_InputField TempoExecucaoInput;
    [SerializeField] TMP_InputField DeadlineInput;
    [SerializeField] TMP_InputField PrioridadeInput;

    private void Start()
    {
        Ativo = true;
    }

    public void SetProcessInitialValues()
    {
        TempoChegada = string.IsNullOrWhiteSpace(TempoChegadaInput.text) ? 0 : Convert.ToInt16(TempoChegadaInput.text);
        TempoExecucao = string.IsNullOrWhiteSpace(TempoExecucaoInput.text) ? 0 : Convert.ToInt16(TempoExecucaoInput.text);
        Deadline = string.IsNullOrWhiteSpace(DeadlineInput.text) ? 0 : Convert.ToInt16(DeadlineInput.text);
        Prioridade = string.IsNullOrWhiteSpace(PrioridadeInput.text) ? 0 : Convert.ToInt16(PrioridadeInput.text);

        if(TempoExecucao == 0)
            Finalizado = true;
        else
            Finalizado = false;

    }

}
