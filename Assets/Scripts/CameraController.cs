using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    GameObject player;

    public float cameraPosX;
    public float cameraPosXMin;
    public float cameraPosXMax;

    void Start()
    {
        AudioManager.instance.PlayMusic("MainTheme");
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosX = Mathf.Clamp(player.transform.position.x, cameraPosXMin, cameraPosXMax);
        transform.position = new Vector3(cameraPosX, transform.position.y, transform.position.z);
            
    }
}
