using UnityEngine;
using UnityEngine.AI;

public class RandomRoam : MonoBehaviour
{
    [Header("Roaming information")]
    [SerializeField] private float _roamSquareSize;
    [SerializeField] private float _roamRemainingDistance;

    private NavMeshAgent _navmeshAgent;

    private void Awake()
    {
        _navmeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_navmeshAgent.enabled == false)
            return;

        if (_navmeshAgent.hasPath == false || _navmeshAgent.remainingDistance < _roamRemainingDistance)
            ChooseNewPosition();
    }

    private void ChooseNewPosition()
    {
        Vector3 randomOffset = new Vector3(RandomNumberOnSize(), 0, RandomNumberOnSize());
        
        _navmeshAgent.SetDestination(randomOffset);
    }

    private float RandomNumberOnSize()
    {
        return Random.Range(-_roamSquareSize, _roamSquareSize);
    }
}