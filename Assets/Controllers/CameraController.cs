using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Vector3 posicaoInicial;
    [SerializeField] GameObject FollowThis;
    private bool followCursor;

    [SerializeField] GameObject Overlay;
    private Vector3 posicaoInicialUI;

    [SerializeField] float Velocidade = 1f;
    [SerializeField] float Delay = 2f;
    private float timer = 0f;
    [SerializeField] Toggle AutoPosicionamento;

    private void Start()
    {
        posicaoInicial = this.transform.position;
        posicaoInicialUI = Overlay.transform.position;
    }
    private void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (posicaoInicial.x > this.transform.position.x)
                return;

            MoveCamera(1, -Velocidade);
            timer = Delay;
            return;
        }

        if (FollowThis == null || !followCursor)
            return;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (this.transform.position.x > FollowThis.transform.position.x)
                return;
            timer = Delay;
            MoveCamera(1, Velocidade);
            return;
        }

        if (AutoPosicionamento.isOn)
        {
            timer = 0;
            return;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }   

        if (FollowThis.transform.position.x > this.transform.position.x)
        {
            float distance = Vector3.Distance(new Vector3(this.transform.position.x,0,0), new Vector3(FollowThis.transform.position.x, 0, 0));
            float distanceFactor;
            if (distance > 2)
            {
                distanceFactor = (float)Math.Pow(2, Vector3.Distance(new Vector3(this.transform.position.x, 0, 0), new Vector3(FollowThis.transform.position.x, 0, 0)) / 5);
                distanceFactor = Math.Min(distanceFactor, 100);
            }
            else
                distanceFactor = 2f;
            
            MoveCamera(distanceFactor, Time.deltaTime);
        }        
    }

    private void MoveCamera(float distanceFactor, float velocidade)
    {
        if (posicaoInicial.x > this.transform.position.x + velocidade * distanceFactor)
        {
            transform.position = posicaoInicial;
            Overlay.transform.position = posicaoInicialUI;
            return;
        }
            

        Vector3 cameraPos = new Vector3(this.transform.position.x + velocidade * distanceFactor,
                                        this.transform.position.y,
                                        this.transform.position.z);
        transform.position = cameraPos;

        Vector3 OverlayPos = new Vector3(Overlay.transform.position.x + velocidade * distanceFactor,
                                        Overlay.transform.position.y,
                                        Overlay.transform.position.z);
        Overlay.transform.position = OverlayPos;
    }

    public void Reset()
    {
        followCursor = false;
        transform.position = posicaoInicial;
        Overlay.transform.position = posicaoInicialUI;
    }
    public void FollowCursor()
    {
        followCursor = true;
    }
}
