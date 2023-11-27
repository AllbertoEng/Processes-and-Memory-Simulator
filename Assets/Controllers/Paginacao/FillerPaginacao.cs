using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillerPaginacao : MonoBehaviour
{
    [SerializeField] float Velocidade = 1f;
    [SerializeField] float DestroyCooldown = 2f;
    public int Col;
    private bool destroy;
    public int Process;

    void Update()
    {
        if (destroy)
        {
            DestroyCooldown -= Time.deltaTime;
            if (DestroyCooldown <= 0)
                Destroy(this.gameObject);
        }
        if (this.transform.localScale.x <= 0.5)
            this.transform.localScale += new Vector3(Time.deltaTime * Velocidade, 0, 0);
    }

    public void SetColor(Color color)
    {
        this.GetComponent<SpriteRenderer>().color = color;
    }
    public void Destroy()
    {
        destroy = true;
    }
    public void DestroyImed()
    {
        Destroy(this.gameObject);
    }
}
