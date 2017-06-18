﻿using UnityEngine;

public class PlayerInfoUI : MonoBehaviour 
{
    private void Update()
    {
        //将UI对着相机设备
        GetComponent<RectTransform>().rotation = Quaternion.LookRotation(-1 * GameObject.FindGameObjectWithTag("MainCamera").transform.position);
    }

    public void SetNameText(string name)
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        GetComponent<TextMesh>().text = name;
    }
}