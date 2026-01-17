using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private float roamChangeDirFloat = 2f;
    private enum State {
        Roaming
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private IEnumerator RoamingRoutine() {
         while (state == State.Roaming)
        {
            Vector2 roamDirection = GetRoamingDirection();
            enemyPathfinding.MoveTo(roamDirection);
            yield return new WaitForSeconds(roamChangeDirFloat);
        }
    }

    private void Start() {
        StartCoroutine(RoamingRoutine());
    }

    private Vector2 GetRoamingDirection() {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

}
