﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool inAttackAnimation = false;

    public bool getInAttack()
    {
        return inAttackAnimation;
    }

    public void setInAtack()
    {
        inAttackAnimation = true;
    }
    public void setNotInAtack()
    {
        inAttackAnimation = false;
    }
    
    //protected gives access to children
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int speed;
    [SerializeField]
    protected Transform pointA, pointB;

    [SerializeField] public float maxXDistanceAway = 5f;
    [SerializeField] public float maxYDistanceAway = 1f;
    

    [SerializeField]
    protected float enemyAttackRange;
    protected Vector3 destination;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool inCombat = false;
    protected Transform feyLocation;
    
    protected Rigidbody2D rigid;
    [SerializeField]
    protected float attackCooldown;

    protected bool disabled;
    private void Start()
    {
        //we put our starting calls into Init so we can override it in our children.
        Init();
    }

    public virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        feyLocation = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        disabled = false;
    }
    
    public virtual void WayPointLogic()
    {

        
        //check to see if Fey is far away enough to justify walking
        float feys_X_DistanceAway = Mathf.Abs(feyLocation.position.x - transform.position.x);
        float feys_Y_DistanceAway = Mathf.Abs(feyLocation.position.y - transform.position.y);
        

        if (feys_X_DistanceAway > maxXDistanceAway && feys_Y_DistanceAway < maxYDistanceAway)
        {
            inCombat = false;
            anim.SetBool("InCombat", false);
        }else if (feys_X_DistanceAway < maxXDistanceAway && feys_X_DistanceAway > enemyAttackRange && feys_Y_DistanceAway < maxYDistanceAway)
        {
            anim.SetBool("Chase", true);
            inCombat = true;
            //tells it to hit
            anim.SetBool("InCombat", false);
            //Debug.Log("Fey is nearby lets chase her");
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Vector3 feyCurLocation = new Vector3(feyLocation.position.x, transform.position.y, transform.position.z);
                transform.position =
                    Vector3.MoveTowards(transform.position, feyCurLocation, speed * Time.deltaTime);
            }
        }else if (feys_X_DistanceAway < enemyAttackRange && feys_Y_DistanceAway < maxYDistanceAway)
        {
            anim.SetBool("Chase", false);
            anim.SetBool("InCombat", true);
            }

        //no 360 no scope
        if (anim.GetBool("Chase") || anim.GetBool("InCombat"))
        {
            if (!this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float facingDirection = feyLocation.position.x - transform.position.x;
                if (facingDirection > 0)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (facingDirection < 0)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }

        }
    }
    
    //virtual keyword lets us overwrite this.
    public virtual void Attack()
    {
        
    }

    public virtual void Update()
    {
        Debug.Log("Am I disabled?" + disabled);
        if (!disabled)
        {
            //if idle, we want to prevent movement, so we do nothing, so just return
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return;
            }

            WayPointLogic();
        }
    }
    
}
