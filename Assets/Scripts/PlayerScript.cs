using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //переменная для установки макс. скорости персонажа
    [SerializeField]
    public float maxSpeed = 10f; 
    //переменная для определения направления персонажа вправо/влево
    private bool flipCharacter = false;
    private float moveHorizontal;

    bool isGrounded;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private Transform groundCheckObject;  // объект, от которого происходит BoxCast
    private float groundCheckBoxWidth = 1f;  // ширина BoxCast
    private float groundCheckBoxDistance = 0.1f;  // расстояние для BoxCast

    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

	private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /*
        Прыжок и движение я разделил, т.к. GetKeyDown не будет корректно работать
        в FixedUpdate из-за частоты его обновления
        В то же время, нам не придется умножать движение на Time.deltaTime, если поместить
        движение в FixedUpdate
    */
    private void Update() {
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate() {
        // берем input
        moveHorizontal = Input.GetAxis("Horizontal");

        // задаем велосити для rigidbody
        rb.velocity = new Vector2(moveHorizontal * maxSpeed, rb.velocity.y);

        // переворачиваем персонажа в зависимости от движения
        if(moveHorizontal < 0 && !flipCharacter)
            Flip();
        else if (moveHorizontal > 0 && flipCharacter)
            Flip();
    }

    // Метод для отзеркаливания спрайта персонажа
    private void Flip()
    {
        flipCharacter = !flipCharacter;
        spriteRenderer.flipX = flipCharacter;
    }

    void OnDrawGizmos()
    {
        // рисуем границы бокс каста, видимые только в сцене, для дебага
        Gizmos.color = Color.red;
        Gizmos.DrawCube(
            groundCheckObject.position - new Vector3(0, groundCheckBoxDistance / 2, 0),
            new Vector3(groundCheckBoxWidth, groundCheckBoxDistance, 1f)
        );
    }

    void CheckGround()
    {        
        // производим рейкаст вниз на 0.1 для проверки наличия поверхности
        RaycastHit2D hit = Physics2D.BoxCast(
            new Vector2(groundCheckObject.position.x, groundCheckObject.position.y),
            new Vector2(groundCheckBoxWidth, 0.001f), 
            0, 
            Vector2.down,
            groundCheckBoxDistance,
            LayerMask.GetMask("Ground")
        );
        
        if (hit.collider == null)
        {
            isGrounded = false;
        } 
        else 
        {
            // Debug.Log(hit.collider.name);

            /*  
                Дополнительная проверка, чтобы игрок не мог прыгнуть 
                уже в прыжке, например на середине платформы 
            */
            if (rb.velocity.y < 0.001f)
                isGrounded = true;
        }
    }

    // проверка столкновения с монеткой
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Coin")
            other.GetComponent<Coin>().CoinCollected();
    }
}
