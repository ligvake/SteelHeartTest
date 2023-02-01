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
    private Transform groundCheckObject;

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

    void CheckGround()
    {
        // рисуем луч, видимый только в сцене, для дебага
        Debug.DrawRay(groundCheckObject.position, -Vector2.up / 10, Color.red);
        
        // производим рейкаст вниз на 0.1 для проверки наличия поверхности
        RaycastHit2D hit = Physics2D.Raycast(groundCheckObject.position, -Vector2.up, 0.1f);
        if (hit.collider == null)
        {
            isGrounded = false;
        } else {
            // Debug.Log(hit.collider.name);
            isGrounded = true;
        }
    }

    // проверка столкновения с монеткой
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Coin")
            other.GetComponent<Coin>().CoinCollected();
    }
}
