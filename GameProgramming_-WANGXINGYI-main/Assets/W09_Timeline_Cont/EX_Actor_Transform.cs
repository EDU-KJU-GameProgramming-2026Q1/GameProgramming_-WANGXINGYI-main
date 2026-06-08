using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Actor_Transform : MonoBehaviour
{
    public Transform Target;
    Vector3 DefaultPos;
    void Start()
    {
        DefaultPos=Target.position;
    }

    public void SetToDefaultPos()
    {
        Target.position=DefaultPos;
    }
}
