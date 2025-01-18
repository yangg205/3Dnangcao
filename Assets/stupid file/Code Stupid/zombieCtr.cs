    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.UIElements;

    public class zombieCtr : MonoBehaviour
    {
        [Header("Zombie Health and Damage ")]
        private float zombieHealth = 100f; 
        private float presentHealth; 
        public float GiveDamage = 5f;

        [Header("Zombie things")]
        public NavMeshAgent zombieAgent;
        public Transform playerBody;
        public Camera AttackingRAycastArea;
        public Transform LookPoint;
        public LayerMask PlayerLayer;
        

        [Header("Zombie Guarding Var")]
        public GameObject[] walkPoints;
        int currentZombiePosition = 0; 
        public float ZombieSpeed; 
        public float walkingpointRadius =2f;

        [Header("Zombie Attacking var")]
        public float timeBtwAttack;
        bool previouslyAttack;

        [Header("Zombie mod/states")]
        public float visionRadius;
        public float attackingRadius; 
        public bool playerInvisionRadius; 
        public bool playerInattackingRadius; 
        private void Awake()
        {
            zombieAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            playerInvisionRadius = Physics.CheckSphere(transform.position,visionRadius,PlayerLayer);
            playerInattackingRadius = Physics.CheckSphere(transform.position,attackingRadius,PlayerLayer);
            if(!playerInvisionRadius && !playerInvisionRadius) Guard();
            if (playerInvisionRadius && playerInvisionRadius) Pursueplayer();
            if(playerInvisionRadius && playerInattackingRadius) AttackPlayer();
        }
        private void Guard()
        {
            if(Vector3.Distance(walkPoints[currentZombiePosition].transform.position,transform.position)<walkingpointRadius)
            {
                currentZombiePosition = Random.Range(0,walkPoints.Length);
                if(currentZombiePosition>= walkPoints.Length)
                {
                    currentZombiePosition = 0 ; 
                }
            }
            transform.position= Vector3.MoveTowards(transform.position,walkPoints[currentZombiePosition].transform.position,Time.deltaTime * ZombieSpeed);
            transform.LookAt(walkPoints[currentZombiePosition].transform.position);
        }
    private void Pursueplayer()
    {
        zombieAgent.SetDestination(playerBody.position);
    }
    private void AttackPlayer()
    {
         zombieAgent.SetDestination(playerBody.position);
         transform.LookAt(LookPoint);
        if(!previouslyAttack)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(AttackingRAycastArea.transform.position, AttackingRAycastArea.transform.forward,out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking"+ hitInfo.transform.name);
            }
            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }
    public void ActiveAttacking()
    {
        previouslyAttack = false;
    }
    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if(presentHealth <= 0)
        {
            zombieDie();
        }
    }
    private void zombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        ZombieSpeed = 0f;
        attackingRadius = 0f; 
        visionRadius = 0f;
        playerInattackingRadius = false; 
        playerInvisionRadius = false; 
        Object.Destroy(gameObject,5.0f);
    }
}
