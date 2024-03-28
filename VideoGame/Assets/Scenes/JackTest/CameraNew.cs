using UnityEngine;

public class CameraNew : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    private void Update()
    {
        //If the reference isn't null it moves towards the reference
        if(target != null)
        {
            Vector3 targetPosition = target.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
