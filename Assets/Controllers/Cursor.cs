using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 posicaoInicial;
    [SerializeField] Vector2Int posicaoAtual;
    [SerializeField] Filler filler;
    [SerializeField] GameObject parentFillers;
    private Filler fillerAtual;
    List<Filler> listFillers;

    [SerializeField] PaginacaoController PaginacaoController;
    private void Start()
    {
        posicaoAtual = new Vector2Int(1, 1);
        listFillers = new List<Filler>();
    }

    public void Reset()
    {
        posicaoInicial = new Vector3(0, -1.5f, 0);
        this.transform.position = posicaoInicial;
        posicaoAtual = new Vector2Int(1, 1);

        if (listFillers == null)
            return;

        foreach (Filler filler in listFillers)
            Destroy(filler.transform.gameObject);
        listFillers.Clear();
    }

    public void MoverLinha(int linha, Status status)
    {
        Vector3 pos = posicaoInicial;
        pos.x += this.transform.position.x + 1;
        pos.y += linha - 1;
        this.transform.position = pos;

        CreateNewFiller();
        FillType(status);

        posicaoAtual.x += 1;
        posicaoAtual.y = linha;
        fillerAtual.Fill(posicaoAtual.x);

        if (status == Status.Executando || status == Status.Deadline)
        {
            PaginacaoController.AlocarProcessoMemoriaFisica(linha-1);
        }
    }

    public Filler CreateNewFiller()
    {
        fillerAtual = Instantiate(filler, this.transform);
        fillerAtual.transform.parent = parentFillers.transform;
        fillerAtual.transform.position = this.transform.position;
        listFillers.Add(fillerAtual);
        return fillerAtual;
    }

    public void FillType(Status status)
    {
        if (fillerAtual != null)
            fillerAtual.SetColor(status);
    }
}
