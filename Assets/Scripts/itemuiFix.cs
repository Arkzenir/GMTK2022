using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemuiFix : MonoBehaviour
{
    public RectTransform rectTrans;

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTrans.localScale = new Vector3(1, 1, 1);
    }
}
