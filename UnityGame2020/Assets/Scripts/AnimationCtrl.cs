using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
public enum AnimaType { Idle, Move, Attack, Attack1, Attack2, Death }
[RequireComponent(typeof(Animator))]
public class AnimationCtrl : MonoBehaviour {
	public AnimaType type;
	private Animator m_animator;
	private Animator animator
    {
        get
        {
			if (m_animator == null) m_animator = GetComponent<Animator>();
			return m_animator;
		}
    }
	
	// Use this for initialization
	void Start () {
		
	}
	public void SetAttackSpeed(float attackSpeed)
	{
		animator.SetFloat("AttackSpeed", attackSpeed);
	}
	public void SetAttackSpeed2(float attackSpeed)
	{
		animator.SetFloat("Attack1Speed", attackSpeed);
	}
	// Update is called once per frame
	public void SetType(AnimaType type)
	{
		this.type = type;
		if (this.type == AnimaType.Death) animator.SetTrigger("Death");
		animator.SetBool("Move", this.type==AnimaType.Move);
		animator.SetBool("Attack", this.type == AnimaType.Attack);
	}

	public void SetType2(AnimaType type)
	{
		this.type = type;
		if (this.type == AnimaType.Death) animator.SetTrigger("Death");
		animator.SetBool("Move", this.type == AnimaType.Move);
		animator.SetBool("Attack1", this.type == AnimaType.Attack1);
		animator.SetBool("Attack2", this.type == AnimaType.Attack2);
	}
}
