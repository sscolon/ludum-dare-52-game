using UnityEngine;

public class SampleScript : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    private void Update()
    {
        //Just some movement
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * _movementSpeed;
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * _movementSpeed;
        transform.position += new Vector3(x, y, 0);
    }
}
