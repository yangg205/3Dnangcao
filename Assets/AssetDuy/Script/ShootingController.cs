using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public Animator animator;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float fireRange = 10f;
    private float nextFireTime = 0f;

    //Gun Mode
    public bool isAuto = false;

    //Ammo and Reload
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;
    public ParticleSystem muzzleFlash;
    void Start()
    {
        currentAmmo = maxAmmo;
    }
    void Update()
    {
        if(isReloading)
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
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
            {
                Debug.Log(hit.transform.name);

                //apply damage zombie
            }
            muzzleFlash.Play();
            animator.SetBool("Shoot", true);
            currentAmmo--;
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
