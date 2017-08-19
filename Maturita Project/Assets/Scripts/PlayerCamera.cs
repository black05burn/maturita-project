using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    Transform player;

    public float smoothSpeed = 10f;
    public Vector3 offset;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = player.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }

}
