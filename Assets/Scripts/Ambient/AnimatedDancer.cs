using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatedDancer : MonoBehaviour
{

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        float rnd = Random.Range(-1.0f, 1.0f);
        bool direction = (Mathf.Sign(rnd) == 1);
        animator.SetBool("direction", direction);
        animator.SetFloat("speed", Random.Range(0.8f, 1.2f));
    }

}
