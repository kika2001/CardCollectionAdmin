using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownChecklist : MonoBehaviour
{
    public GameObject checkList;
    public void OpenCloseChecklist(bool b)
    {
        checkList.SetActive(!checkList.activeSelf);
    }
    
}
