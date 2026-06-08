using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W07_DoorController : MonoBehaviour
{
    public GameObject Target;
    public Animator  animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();    
    }
    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name+"entered");
        animator.SetInteger("DoorState",1);

    }

    private void OnTriggerExit(Collider other)
    {
        print (other .gameObject.name+"exited");
        animator.SetInteger("DoorState",2);
    }
}
