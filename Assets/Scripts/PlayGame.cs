using UnityEngine;

public class PlayGame : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SCManager.instance.LoadScene("GameScene");
        } 
    }
}
