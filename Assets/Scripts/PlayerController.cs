using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    float motionVelocity = 1f;

    [SerializeField]
    float fuerzaSalto = 1f;

    [SerializeField]
    float lengthLine = 0f;

    // Variable privada para el control de saltos
    bool saltar = true;

    bool activeMap = false;

    public int coinsCollected = 0;

    public TextMeshProUGUI txtMonedas;

    // Obtenemos la referencia al sistema de partículas
    [SerializeField] 
    ParticleSystem jumpParticles;

    [SerializeField]
    float offsetRaycast = 0f;
    // Update se llama una vez por cada frame
    private void Update() {
        // Lectura del eje de entrada horizontal
        float xMotion = Input.GetAxisRaw("Horizontal");
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (xMotion < 0) sr.flipX = true;
        else if (xMotion > 0) sr.flipX = false;
        // Referencia al Rigidbody2D del gameObject que contiene este script
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Se genera un nuevo vector con la velocidad a aplicar
        Vector2 speedVector = new Vector2(xMotion * motionVelocity, rb.linearVelocityY);
        // Se aplica la velocidad a las físicas del gameObject que contiene el script
        GetComponent<Rigidbody2D>().linearVelocity = speedVector;

        // Detectamos la pulsación de la tecla (una vez por cada pulsación)
        if (Input.GetKeyDown(KeyCode.Space) && saltar) {
            saltar = false;
            // Aplicamos la fuerza física en la coordenada vertical
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            jumpParticles.Play();
            AudioManager.instance.PlaySFX("Jump");
        }

        if(Input.GetKeyDown(KeyCode.M) && !activeMap) {
            SceneManager.LoadScene("MapScene",LoadSceneMode.Additive);
            activeMap = true;
        } else if (Input.GetKeyDown(KeyCode.M) && activeMap) {
            SceneManager.UnloadSceneAsync("MapScene");
            activeMap = false;
        }

        // Obtenemos el tamaño vertical del collider para configurar la coordenada de generación
        float colliderSizeY = gameObject.GetComponent<Collider2D>().bounds.size.y + offsetRaycast;
        Vector2 raycastOrigin = new Vector2(gameObject.transform.position.x, transform.position.y - (colliderSizeY / 2));
        // Dibujamos una línea de color negro hacia abajo que parta desde la posición calculada anteriormente
        // y que termine en donde indique el segundo parámetro del método DrawLine()
        Debug.DrawLine(raycastOrigin, raycastOrigin - (new Vector2(0f, lengthLine)), Color.black);

        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastOrigin, Vector2.down, lengthLine);
        // Preguntamos si el RayCast está colisionando con el Tilemap de colisiones
        // asegurándonos de que dicho Tilemap tiene una etiqueta correctamente configurada
        if (raycastHit2D.collider != null) {
            if (GetComponent<Rigidbody2D>().linearVelocityX != 0) SetAnimation("running");
            else SetAnimation("idle"); // Si está sobre una superficie pero no se mueve
            if (raycastHit2D.collider.gameObject.CompareTag("Ground")) {
                saltar = true;
            }
            saltar = true;
        } else {
             saltar = false;
            SetAnimation("jumping");
        }
    }

    void SetAnimation(string name) {

        // Obtenemos todos los parámetros del Animator
        AnimatorControllerParameter[] parametros = GetComponent<Animator>().parameters;

        // Recorremos todos los parámetros y los ponemos a false
        foreach (var item in parametros) GetComponent<Animator>().SetBool(item.name, false);

        // Activamos el pasado por parámetro
        GetComponent<Animator>().SetBool(name, true);

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision != null) {
            if (collision.collider.CompareTag("Enemy")) {
                SCManager.instance.LoadScene("GameOver");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
            if (collision.CompareTag("PowerUp")) {
                AudioManager.instance.PlaySFX("CollectCoin");
                Destroy(collision.gameObject);
            }

            if (collision.CompareTag("Coin")) {
                AudioManager.instance.PlaySFX("CollectCoin");
                coinsCollected++;
                txtMonedas.text = coinsCollected.ToString();
                Destroy(collision.gameObject);
            }
        }
    }

}
