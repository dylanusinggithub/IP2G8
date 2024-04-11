using System.Collections;
using UnityEngine;

public class CameraNew : MonoBehaviour
{
    public Transform target;
    public float defaultSmoothing = 10f;
    public float targetSmoothing = 50f;
    public float defaultOrthographicSize = 6.81f;
    public float targetOrthographicSize = 8f;
    public float transitionDuration = 1.0f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, GetSmoothing() * Time.deltaTime);

            mainCamera.orthographicSize = GetOrthographicSize();
        }
    }

public void SetTarget(Transform newTarget)
{
    if (newTarget.CompareTag("PlayerCamera") && target != newTarget)
    {
        StopAllCoroutines();
        StartCoroutine(TransitionToNewTarget(newTarget));
    }
    else
    {
        target = newTarget;
    }
}

    private IEnumerator TransitionToNewTarget(Transform newTarget)
    {
        float timeElapsed = 0f;
        Vector3 initialPosition = transform.position;
        float initialOrthographicSize = mainCamera.orthographicSize;
        float initialSmoothing = defaultSmoothing;
        float targetSmooth;

        while (timeElapsed < transitionDuration)
        {
            float t = timeElapsed / transitionDuration;

            transform.position = Vector3.Lerp(initialPosition, newTarget.position, t);
            targetSmooth = Mathf.Lerp(initialSmoothing, targetSmoothing, t);
            mainCamera.orthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = newTarget.position;
        target = newTarget;
    }


    private float GetSmoothing()
    {
        if (target != null && target.CompareTag("PlayerCamera"))
        {
            return targetSmoothing;
        }
        else
        {
            return defaultSmoothing;
        }
    }

    private float GetOrthographicSize()
    {
        if (target != null && target.CompareTag("PlayerCamera"))
        {
            return targetOrthographicSize;
        }
        else
        {
            return defaultOrthographicSize;
        }
    }
}
