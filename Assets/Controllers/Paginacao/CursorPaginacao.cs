using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorPaginacao : MonoBehaviour
{
    [SerializeField] FillerPaginacao filler;
    [SerializeField] GameObject parentFillers;
    [SerializeField] GameObject GameObjectParent;
    private FillerPaginacao fillerAtual;
    List<FillerPaginacao> listFillers;    

    private Vector3 posicaoInicialCursor;

    [SerializeField] List<ProcessTag> processTags;
    [SerializeField] PaginacaoController PaginacaoController;

    private void Start()
    {
        posicaoInicialCursor = this.transform.position;
        listFillers = new List<FillerPaginacao>();
    }

    public void Reset()
    {
        if (listFillers == null)
            return;

        filler.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        foreach (FillerPaginacao filler in listFillers)
            Destroy(filler.transform.gameObject);
        listFillers.Clear();

        PaginacaoController.ResetVirtualMemory();
    }

    public void PreencherColuna(int col, Color color, int process)
    {
        float X = 0.5f * col;
        float Y = 0;

        RemoveFillerByCollum(col);

        for (int i = 0; i < 10; i++)
        {
            this.transform.position = new Vector3(posicaoInicialCursor.x + X,
                                                posicaoInicialCursor.y + Y,
                                                posicaoInicialCursor.z);
            CreateNewFiller(col, color, process);
            Y -= 0.5f;
        }
    }

    public FillerPaginacao CreateNewFiller(int col, Color color, int process)
    {
        filler.SetColor(color);
        filler.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        fillerAtual = Instantiate(filler, this.transform);
        fillerAtual.Col = col;
        fillerAtual.Process = process;        
        fillerAtual.transform.parent = parentFillers.transform;
        fillerAtual.transform.position = new Vector3(this.transform.position.x + GameObjectParent.transform.position.x,
                                                        this.transform.position.y,
                                                        this.transform.position.z);
        listFillers.Add(fillerAtual);
        return fillerAtual;
    }

    public int ReturnProcessOnColumn(int col)
    {
        List<FillerPaginacao> result = listFillers
            .Where(item => item.Col == col)
            .ToList();

        return result != null && result.Count > 0 ? result[0].Process : -1;
    }

    public void RemoveFillerByCollum(int col, bool destroyImediatly = false)
    {
        if (listFillers == null)
            return; 

        List<FillerPaginacao> result = listFillers
            .Where(item => item.Col == col)
            .ToList();

        foreach (FillerPaginacao filler in result)
        {
            listFillers.Remove(filler);
            if (destroyImediatly)
                filler.DestroyImed();  
            else
                filler.Destroy();
        }
    }
}
