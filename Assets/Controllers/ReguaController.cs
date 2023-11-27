using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReguaController : MonoBehaviour
{
    [SerializeField] GameObject ReguaNumberPrefab;
    [SerializeField] TMP_Text NumberText;
    [SerializeField] GameObject Parent;
    [SerializeField] Vector3 Position;
    void Start()
    {
        CreateNumbers(3000);
    }
    
    private void CreateNumbers(int maiorNumero)
    {
        while (maiorNumero >= Position.x)
        {
            GameObject go = Instantiate(ReguaNumberPrefab, this.transform);
            go.GetComponent<TMP_Text>().text = (Position.x - 0.5).ToString();
            //go.transform.parent = Parent.transform;
            go.transform.position = Position;
            Position.x += 5;
        }            
    }
}
