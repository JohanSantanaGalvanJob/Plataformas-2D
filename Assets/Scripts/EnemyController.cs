using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed; // Referencia a la velocidad de movimiento
    [SerializeField] Rigidbody2D rb; // Referencia al Rigidbody2D
    [SerializeField] float distance; // Referencia a la distancia máxima de movimiento
    Vector2 originalPosition; // Posición original del enemigo
    bool rightMotion = true; // Dirección del movimiento
                             // Esta función se ejecuta una vez al comienzo
    public void Start() {
        // Obtenemos la posición original del enemigo
        originalPosition = transform.position;
    }
    // Esta función se ejecuta cada X tiempo
    public void FixedUpdate() {
        // Si la posición original del enemigo sumada a la distancia máxima
        // es inferior a la posición actual el enemigo sigue moviéndose a la derecha.
        if ((transform.position.x < (originalPosition.x + distance)) && rightMotion) {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocityY);
        } else if ((transform.position.x >= (originalPosition.x + distance)) && rightMotion) rightMotion = false;
        // Si la posición original del enemigo restándole la distancia máxima
        // es inferior a la posición actual el enemigo sigue moviéndose a la izquierda.
        if ((transform.position.x > (originalPosition.x - distance)) && !rightMotion) {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocityY);
        } else if ((transform.position.x <= (originalPosition.x - distance)) && !rightMotion) rightMotion = true;
    }
}
