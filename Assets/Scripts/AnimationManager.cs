using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _Animator;
    // Start is called before the first frame update

    private void Start()
    {
        // Инициализация компонента Animator
        _Animator = GetComponent<Animator>();

        // Проверка, если Animator не найден
        if (_Animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }
    }

    public void SetAnimationIdle()
    {
        _Animator.SetInteger("Animation", 0);
    }
    public void SetAnimationWalk()
    {
        _Animator.SetInteger("Animation", 1);
    }
    public void SetAnimationRun()
    {
        _Animator.SetInteger("Animation", 2);
    }
    public void SetAnimationFire()
    {
        _Animator.SetTrigger("Fire");
    }
    public void SetAnimationReload()
    {
        _Animator.SetTrigger("Reload");

    }





}
