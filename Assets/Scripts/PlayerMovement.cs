using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 60.0f;

    NavMeshAgent agent;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        var horizontal = Input.GetAxis("Horizontal");
        transform.rotation *= Quaternion.AngleAxis(horizontal * rotationSpeed * Time.deltaTime, Vector3.up);
        Vector3 side = Vector3.zero; // transform.right * horizontal * movementSpeed * Time.deltaTime;

        var vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical * movementSpeed * Time.deltaTime;
        agent.Move(forward + side);
        agent.SetDestination(transform.position + forward + side);

        //animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }
}
