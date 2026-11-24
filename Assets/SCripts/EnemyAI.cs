using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;

    public float sightRange;
    public bool playerInSightRange;

    //[Header("Audio Sources")]
    //[SerializeField] private AudioSource foundPlayer;
    //[SerializeField] private AudioSource searchingPlayer;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        //enemy checks if player is in sight, chases player if it is
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if(playerInSightRange) ChasePlayer();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

}
