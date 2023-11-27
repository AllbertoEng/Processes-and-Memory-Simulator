using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filler : MonoBehaviour
{
    [SerializeField] int Tamanho;
    [SerializeField] float Velocidade = 1;
    [SerializeField] Color ExecucaoColor;
    [SerializeField] Color SobrecargaColor;
    [SerializeField] Color NoneColor;
    [SerializeField] Color DeadlineColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        this.transform.localScale = new Vector3(0, 1, 1);
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (this.transform.localScale.x <= Tamanho - Convert.ToInt32(transform.position.x))
            this.transform.localScale += new Vector3(Time.deltaTime * Velocidade, 0, 0);
    }

    public void Fill(int tamanho)
    {
        Tamanho = tamanho;
    }

    public void SetColor(Status status)
    {
        if (spriteRenderer == null)
            return;

        switch (status)
        {
            case Status.Executando:
                spriteRenderer.color = ExecucaoColor;
                break;
            case Status.Sobrecarregando:
                spriteRenderer.color = SobrecargaColor;
                break;
            case Status.Vazio:
                spriteRenderer.color = NoneColor;
                break;
            case Status.Deadline:
                spriteRenderer.color = DeadlineColor;
                break;
            default:
                spriteRenderer.color = ExecucaoColor;
                break;
        }
    }
}

public enum Status
{
    Executando = 1,
    Sobrecarregando = 2,
    Vazio = 3,
    Deadline = 4
}