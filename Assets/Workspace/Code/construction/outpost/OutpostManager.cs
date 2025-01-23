using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostManager : MonoBehaviour
{
    private float HP = 800;

    public void armor_hited()
    {
        Debug.Log("×°¼×°å±»»÷´ò");
        HP -= 50;
    }
}
