using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimmerScript : MonoBehaviour
{
    public Animator anim2;
    public Animation anim;
    void Start()
    {
        anim2 = GetComponent<Animator>();
    }

    public void StartDimmingAnim()
    {
        anim2.SetFloat("Direction", 1);
        anim2.SetFloat("Animcontroller", 2);
    }

    public void SetToZero() {
        anim2.SetFloat("Animcontroller", 0);
        anim2.SetFloat("Direction", -3);
    }
}
