using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    public Animator animator;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float fireRange = 10f;
    private float nextFireTime = 0f;
    public AudioSource soundAudioSource;
    public AudioClip shootingAudioClip;
    public AudioClip reloadAudioClip;
    //public AudioClip drawAudioClip;

    // Gun Mode
    public bool isAuto = false;

    // Ammo and Reload
    public int maxAmmo = 30;
    private int currentAmmo;
    public int totalAmmo = 90; // Tổng số đạn có thể mang
    public float reloadTime = 1.5f;
    private bool isReloading = false;
    public ParticleSystem muzzleFlash;
    public ParticleSystem bloodEffect;
    public int damagePerShot = 10;

    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI totalAmmoText;

    void Start()
    {
        //soundAudioSource.PlayOneShot(drawAudioClip);
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading)
            return;

        if (isAuto)
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

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && totalAmmo > 0)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
            {
                Debug.Log(hit.transform.name);

                // Apply damage to zombies
                ZombieAI zombieAI = hit.collider.GetComponent<ZombieAI>();
                if (zombieAI != null)
                {
                    zombieAI.TakeDamage(damagePerShot);
                    ParticleSystem blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood.gameObject, blood.main.duration);
                }

                WaypointZombieAI waypointZombieAI = hit.collider.GetComponent<WaypointZombieAI>();
                if (waypointZombieAI != null)
                {
                    waypointZombieAI.TakeDamage(damagePerShot);
                    ParticleSystem blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood.gameObject, blood.main.duration);
                }
            }
            soundAudioSource.PlayOneShot(shootingAudioClip);

            muzzleFlash.Play();
            animator.SetBool("Shoot", true);
            currentAmmo--;
            UpdateAmmoUI();
        }
        else
        {
            Reload();
        }
    }

    private void Reload()
    {
        if (!isReloading && totalAmmo > 0)
        {
            animator.SetTrigger("Reload");
            isReloading = true;
            Invoke("FinishReloading", reloadTime);
            soundAudioSource.PlayOneShot(reloadAudioClip);
        }
    }

    private void FinishReloading()
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        isReloading = false;
        animator.ResetTrigger("Reload");
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        currentAmmoText.text = currentAmmo.ToString();
        totalAmmoText.text = totalAmmo.ToString();
    }

    public void PickupAmmo(int ammoAmount)
    {
        Debug.Log("Số đạn sau khi nhặt: " + totalAmmo); // Kiểm tra số đạn sau khi nhặt

        totalAmmo += ammoAmount;
        UpdateAmmoUI();
    }
}
