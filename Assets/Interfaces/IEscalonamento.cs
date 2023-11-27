using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEscalonamento
{
    public float Resposta { get; set; }
    public float Quantum { get; set; }
    public float Sobrecarga { get; set; }
    public void CalcularProximoMovimento(Cursor cursor, List<Processo> processos, int tempo);
}
