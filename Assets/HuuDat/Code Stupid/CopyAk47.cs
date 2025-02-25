using Unity.VisualScripting;
using UnityEngine;

public class CopyAk47 : MonoBehaviour
{
    public LayerMask zombie;
    public Animator animator;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float fireRange = 10f;
    private float nextFireTime = 0f;
    public bool isAuto = false;
    public int maxAmmo = 30;
    [SerializeField]
    private int currentAmmo;
    public float reloadTime = 1.5f;
    [SerializeField]
    private bool isReloading = false;
    public ParticleSystem muzzleFlash;
    public int damagePershot = 20;

    //Sound
    public AudioSource soundAudioSource;
    public AudioClip shootingSoundClip;
    public AudioClip reloadSoundClip;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;
        if(isAuto == true)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
            else
            {
                animator.SetBool("Shoot", false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
            else
            {
                animator.SetBool("Shoot", false);
            }
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            Reload();
        }
    }
    private void Shoot()
    {
        if(currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange,zombie))
            {
                Debug.Log(hit.transform.name);
                //apply damage to zombie
                ZombieAi zombieAi = hit.collider.GetComponent<ZombieAi>();
                TankAI tankAi = hit.collider.GetComponent<TankAI>();
                if(zombieAi != null)
                    zombieAi.TakeDamageAmount(1000);
                    Debug.Log("Dame Zombie");
                BoomerAi boomerAi = hit.collider.GetComponent<BoomerAi>();
                if(boomerAi != null)
                {
                    boomerAi.TakeDamage(20);
                }
                if(tankAi !=null)
                {
                    tankAi.TakeDamageAmount(100);
                }
            }
            muzzleFlash.Play();
            animator.SetBool("Shoot", true);
            currentAmmo--;

            soundAudioSource.PlayOneShot(shootingSoundClip);
        }
        else
        {
            //Reload
            Reload();
        }
    }
    private void Reload()
    {
        if(!isReloading && currentAmmo < maxAmmo)
        {
            animator.SetTrigger("Reload");
            isReloading = true;
            //play reload sound
            soundAudioSource.PlayOneShot(reloadSoundClip);
            Invoke("FinishReloading", reloadTime);

        }
    }
    private void FinishReloading()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        animator.ResetTrigger("Reload");

    }
}
