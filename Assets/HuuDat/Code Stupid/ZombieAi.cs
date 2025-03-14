﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using Unity.VisualScripting;
using CodeMonkey.HealthSystemCM;
using UnityEngine.InputSystem;

public class ZombieAi : MonoBehaviour
{
    public GameObject hitbox;
    public float DissolveRate = 0.00125f;
    public float refreshRate = 0.0025f;
    public SkinnedMeshRenderer SkinnedMesh;
    private Material[] skinnedMaterrials;
    public AudioClip[] deathSounds;
    public AudioSource audioSource;
    public Collider collider;
    waveSpaner spawner;
    public float TimeRadollDie;
    [SerializeField, Range(0f, 15f)]
    private float ChaseSpeed = 5f;
    public float MaxHp = 100f;
    [SerializeField]
    private float currentHp;
    public NavMeshAgent Nav;
    public enum ZombieState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }
    public ZombieState currentState = ZombieState.Idle;
    public Animator animator;
    public Transform player;
    public float chaseDistance = 10f;
    public float AttackDistance = 2f;
    public float AttackCooldown = 2f;
    public float AttackDelay = 1.5f;
    private bool isAttacking;
    private float lastAttackTime;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (SkinnedMesh != null)
        {
            skinnedMaterrials = SkinnedMesh.materials;
        }

        collider = GetComponentInChildren<CapsuleCollider>();
        currentHp = MaxHp;
        gameObject.name = "Type:" + name;
        animator = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        lastAttackTime = -AttackCooldown;
        foreach (Collider Co in gameObject.GetComponentsInChildren<Collider>())
        {
            Co.enabled = true;

        }
    }
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                //Animation 
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", false);
                if (Vector3.Distance(transform.position, player.position) <= chaseDistance)
                {
                    currentState = ZombieState.Chase;
                }
                break;
            case ZombieState.Chase:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);
                //Animations
                Nav.SetDestination(player.position);
                if (Vector3.Distance(transform.position, player.position) <= AttackDistance)
                {
                    currentState = ZombieState.Attack;
                }
                break;
            case ZombieState.Attack:
                //Animations
                animator.SetBool("IsAttacking", true);
                Nav.SetDestination(transform.position);
                if (!isAttacking && Time.time - lastAttackTime >= AttackCooldown)
                {
                    StartCoroutine(AttackWithdelay());
                    Debug.Log("attack player");
                    //
                }
                if (Vector3.Distance(transform.position, player.position) >= AttackDistance)
                {
                    currentState = ZombieState.Chase;
                }

                break;
            case ZombieState.Dead:
                Nav.SetDestination(transform.position);


                StartCoroutine(RadollDie());
                StartCoroutine(Dissolveco());
                break;
        }
    }
    private IEnumerator AttackWithdelay()
    {
        isAttacking = true;
        SetAtive();
        yield return new WaitForSeconds(AttackDelay);
        NoAtive();
        isAttacking = false;
        lastAttackTime = Time.time;
    }

    public void TakeDamageAmount(int damageAmount)
    {

        if (currentState == ZombieState.Dead)
        {
            audioSource.PlayOneShot(deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)]);
            Debug.Log("die");
            return;
        }
        currentHp -= damageAmount;
        Debug.Log(currentHp);
        if (currentHp <= 0)
        {
            currentHp = 0;
            currentState = ZombieState.Dead;
        }
    }
    private IEnumerator RadollDie()
    {

        animator.enabled = false;
        collider.enabled = false;
        foreach (Collider Co in gameObject.GetComponentsInChildren<Collider>())
        {
            Co.enabled = true;

        }
        foreach (Rigidbody Rig in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Rig.isKinematic = false;
        }
        yield return new WaitForSeconds(TimeRadollDie);
        foreach (Rigidbody Rig in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Rig.isKinematic = true;
        }
        foreach (Collider Co in gameObject.GetComponentsInChildren<Collider>())
        {
            Co.enabled = false;

        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void setSpawner(waveSpaner _spawner)
    {
        spawner = _spawner;
    }
    IEnumerator Dissolveco()
    {
        if (skinnedMaterrials.Length > 0)
        {
            float counter = 0;
            while (counter < 1)
            {
                counter += DissolveRate; // Make it frame-rate independent
                //counter = Mathf.Clamp(counter, 0, 1); // Ensure it does not exceed 1

                for (int i = 0; i < skinnedMaterrials.Length; i++)
                {
                    skinnedMaterrials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return null; // Wait until the next frame
            }

            // Ensure the final state is set to 1
            for (int i = 0; i < skinnedMaterrials.Length; i++)
            {
                skinnedMaterrials[i].SetFloat("_DissolveAmount", 1);
            }
        }
    }
    public void SetAtive()
    {
        hitbox.SetActive(true);
    }
    public void NoAtive()
    {
        hitbox.SetActive(false);
    }
}

