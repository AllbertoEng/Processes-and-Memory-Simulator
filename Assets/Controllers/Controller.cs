using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Cursor Cursor;
    [SerializeField] TMP_Dropdown DropdownTipoEscalonamento;
    [SerializeField] TMP_Dropdown DropdownTipoEscalonamentoPaginacao;
    [SerializeField] TMP_Text RespostaText;
    [SerializeField] TMP_InputField QuantumInput;
    [SerializeField] TMP_InputField SobrecargaInput;
    IEscalonamento escalonamento;

    [SerializeField] GameObject Menu;
    [SerializeField] GameObject InterfaceEscalonamento;
    [SerializeField] GameObject MenuEscalonamento;

    private List<Processo> processos;

    [SerializeField] PaginacaoController PaginacaoController;
    public float Resposta { get; set; }

    [SerializeField] float Delay = 1f, ContadorDelay = 0f;
    private int tempo = 0;

    private void Start()
    {
        processos = new List<Processo>();
    }

    private void Update()
    {
        if (escalonamento != null)
        {
            bool existeProcessoNaoFinalizado = false;
            foreach (Processo processo in processos)            
                if (!processo.Finalizado)                
                    existeProcessoNaoFinalizado = true;               

            if (!existeProcessoNaoFinalizado)
            {
                //Concluido o escalonamento
                RespostaText.text = $"{escalonamento.Resposta.ToString()} Segundos";
                escalonamento = null;
                return;
            }                     

            foreach (Processo processo in processos)
            {
                if (processo.TempoChegada > tempo)
                    processo.Ativo = false;
                else
                    processo.Ativo = true;
            }

            if (ContadorDelay >= Delay)
            {
                ContadorDelay = 0f;
                tempo++;

                escalonamento.CalcularProximoMovimento(Cursor, processos, tempo);                
            }
            ContadorDelay += Time.deltaTime;
        }
    }

    public void StartEscalonamento()
    {        
        switch (DropdownTipoEscalonamento.value)
        {
            case 0:
                //FIFO
                escalonamento = new FIFO();
                break;

            case 1:
                //SFJ
                escalonamento = new SJF();
                break;

            case 2:
                //RoundRobin
                escalonamento = new RoundRobin();
                break;

            case 3:
                //EDF
                escalonamento = new EDF();
                break;
        }

        switch (DropdownTipoEscalonamentoPaginacao.value)
        {
            case 0:
                PaginacaoController.EscalonamentoPaginacao = new FIFOPaginacao();
                break;

            case 1:
                PaginacaoController.EscalonamentoPaginacao = new LRUPaginacao();
                break;
        }

        if (escalonamento != null)
        {
            CloseMenu();
            Cursor.Reset();
            tempo = 0;
            processos.Clear();
            RespostaText.text = "";

            escalonamento.Quantum = string.IsNullOrWhiteSpace(QuantumInput.text) ? 0 : Convert.ToInt16(QuantumInput.text);
            escalonamento.Sobrecarga = string.IsNullOrWhiteSpace(SobrecargaInput.text) ? 0 : Convert.ToInt16(SobrecargaInput.text);

            foreach (Processo processo in Menu.GetComponent<MenuController>().Processos)
            {
                if (processo.Ativo)
                {
                    processos.Add(processo);
                    processo.SetProcessInitialValues();
                }                
            }

            PaginacaoController.QuantidadeProcessos = processos.Count;
            PaginacaoController.Reset();
        }
    }

    private void ShowMenu(bool show)
    {
        Menu.SetActive(show);
        InterfaceEscalonamento.SetActive(!show);
        MenuEscalonamento.SetActive(!show);
    }

    public void OpenMenu()
    {
        ShowMenu(true);
    }
    public void CloseMenu()
    {
        ShowMenu(false);
    }
}
