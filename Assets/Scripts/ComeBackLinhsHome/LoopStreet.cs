using DG.Tweening;
using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopStreet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 offset;
    [SerializeField] Camera cam;
    [SerializeField] SuperMap street1;
    [SerializeField] SuperMap street2;

    float deltaCam;

    private void Start()
    {
        deltaCam = -cam.orthographicSize * cam.aspect;
    }

    private void Update()
    {
        Vector3 deltaMove = Vector3.right * speed * Time.deltaTime;
        street1.transform.position += deltaMove;
        street2.transform.position += deltaMove;

        if (street1.transform.position.x >= cam.transform.position.x + deltaCam -0.2f)
        {
            street2.transform.position = street1.transform.position - new Vector3(street2.m_Width, 0) + offset;
        }
        if (street2.transform.position.x >= cam.transform.position.x + deltaCam - 0.2f)
        {
            street1.transform.position = street2.transform.position - new Vector3(street1.m_Width, 0) + offset;
        }
    }
}
