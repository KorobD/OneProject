using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuperMegaScript : MonoBehaviour {

    public Button startButton;
    public Button exitButton;

    [HideInInspector] public TextMeshPro textPlayerScore;
    [HideInInspector] public TextMeshPro textAIScore;
    [HideInInspector] public int hitCounter;

    public GameObject player;
    public GameObject AI;
    public GameObject ball;
    public GameObject playerScore;
    public GameObject AIScore;
    public float ballInitialSpeed;
    public float ballSpeedIncrease;
    public float playerMoveSpeed;
    public float AIMoveSpeed;

    [HideInInspector] public Vector2 playerMove;
    [HideInInspector] public Vector2 AIMove;
    [HideInInspector] public Rigidbody2D rbPlayer;
    [HideInInspector] public Rigidbody2D rbAI;
    [HideInInspector] public Rigidbody2D rbBall;


    public void Awake() {

    }

    public void Start() {
        rbPlayer = player.GetComponent<Rigidbody2D>();
        rbAI = AI.GetComponent<Rigidbody2D>();
        rbBall = ball.GetComponent<Rigidbody2D>();
        textPlayerScore = playerScore.GetComponent<TextMeshPro>();
        textAIScore = AIScore.GetComponent<TextMeshPro>();
        Invoke("StartBall", 2f);
    }
    
    public void Update() {
        PlayerControll();
        AIControll();
    }

    public void FixedUpdate() {
        rbPlayer.velocity = playerMove * playerMoveSpeed;
        rbAI.velocity = AIMove * AIMoveSpeed;
        rbBall.velocity = Vector2.ClampMagnitude(rbBall.velocity, ballInitialSpeed + (ballSpeedIncrease * hitCounter));
    }


    public void PlayerControll() {
        playerMove = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }

    public void AIControll() {
        if (ball.transform.position.y > AI.transform.position.y + 0.5f) {
            AIMove = new Vector2(0, 1);
        } else if (ball.transform.position.y < AI.transform.position.y - 0.5f) {
            AIMove = new Vector2(0, -1);
        } else {
            AIMove = new Vector2(0, 0);
        }
    }

    public void StartBall() {
        rbBall.velocity = new Vector2(-1, 0) * (ballInitialSpeed + ballSpeedIncrease * hitCounter);
    }

    public void ResetBall() {
        rbBall.velocity = new Vector2(0, 0);
        ball.transform.position = new Vector2(0, 0);
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }

    public void PlayerBounce(Transform myObject) {
        hitCounter++;

        Vector2 ballPos = ball.transform.position;
        Vector2 playerPos = myObject.position;

        float xDirection, yDirection;
        if (ball.transform.position.x > 0) {
            xDirection = -1;
        } else {
            xDirection = 1;
        }
        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        if (yDirection == 0) {
            yDirection = 0.25f;
        }
        rbBall.velocity = new Vector2(xDirection, yDirection) * (ballInitialSpeed + (ballSpeedIncrease * hitCounter));
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "player") {
            PlayerBounce(player.transform);
        }
        if (collision.gameObject.name == "AI") {
            PlayerBounce(AI.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "AIGoal") {
            ResetBall();
            textPlayerScore.text = (int.Parse(textPlayerScore.text) + 1).ToString();
        } else if (collision.gameObject.name == "PlayerGoal") {
            ResetBall();
            textAIScore.text = (int.Parse(textAIScore.text) + 1).ToString();
        }
    }
}
