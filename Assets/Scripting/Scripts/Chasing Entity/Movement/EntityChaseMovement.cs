using System.Collections;
using UnityEngine;

public class EntityChaseMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    [Header("Chase")]
    [SerializeField] bool startChasing;
    [SerializeField] float initSpeed;
    [SerializeField] Vector2 Direction;
    float speed;
    public delegate void ChaseState();
    ChaseState stateBehaviour;

    [Header("Speed Transition")]
    [SerializeField] AnimationCurve speedTransition;
    bool isInTransition;
    Coroutine coroutine;

    [Header("Distance Addition")]
    [SerializeField] TransformRuntimeValue playerTransform;
    [SerializeField] AnimationCurve additiveSpeedTransition;
    [SerializeField] float maxDistance;
    [SerializeField] float maxAdditiveSpeed;
    [SerializeField] float additiveSpeed;
    void Start()
    {
        speed = initSpeed;
        stateBehaviour = startChasing ? Chase : Idle;
    }

    void FixedUpdate()
    {
        stateBehaviour();
    }

    void Chase()
    {
        rb.velocity = Direction.normalized * (speed + AdditiveSpeed());
        additiveSpeed = AdditiveSpeed();
    }

    float AdditiveSpeed()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.Item.position);
        distance = distance > maxDistance ? maxDistance : distance;
        return maxAdditiveSpeed * additiveSpeedTransition.Evaluate(distance/maxDistance);
    }

    void Idle()
    {
    }

    public void SetSpeed(float speed, float duration)
    {
        if (isInTransition)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ChangeSpeed(speed, duration));
    }

    IEnumerator ChangeSpeed(float newSpeed, float duration)
    {
        float elapsedTime = 0;
        float savedSpeed = speed;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            speed = Mathf.Lerp(savedSpeed, newSpeed, speedTransition.Evaluate(elapsedTime / duration));
            yield return null;
        }
        isInTransition = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + ((Vector3)Direction.normalized * maxDistance));
    }
}