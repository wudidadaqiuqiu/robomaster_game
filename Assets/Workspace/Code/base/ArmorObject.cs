using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorObject : MonoBehaviour
{
    protected float max_HP = 800;
    protected float HP = 800;

    virtual public void small_hit()
    {
        HP -= 5;
    }

    virtual public void big_hit()
    {
        HP -= 50;
    }
}
