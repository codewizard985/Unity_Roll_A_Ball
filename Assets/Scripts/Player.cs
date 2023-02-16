using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int ScoreValue = 0;
    private Vector2 _movementInput;
    private Vector3 _movement;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScenarioData _scenario;
    [SerializeField] private GameObject _wallPrefab;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();


        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            ScoreValue = PlayerPrefs.GetInt("ScoreValue");
        }
        _scoreText.text = "Score : "+ScoreValue;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            _rigidbody.AddForce(Input.GetAxis("Horizontal") * _speed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * _speed * Time.deltaTime);
        }
    }

    void OnMove(InputValue AxisValues)
    {
        _movementInput = AxisValues.Get<Vector2>();
    }

    void OnJump() 
    {
        Debug.Log("test ok");
        transform.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void FixedUpdate()
    {
        _movement = new Vector3(_movementInput.x, 0f, _movementInput.y);
        _rigidbody.AddForce(_movement * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target_Trigger"))
        {
            Destroy(other.gameObject);
            UpdateScore();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        Instantiate(_wallPrefab, _scenario.FirstWalls[ScoreValue % _scenario.FirstWalls.Length].position, Quaternion.Euler(_scenario.FirstWalls[ScoreValue % _scenario.FirstWalls.Length].orientation));

        ScoreValue++;

        _scoreText.text = "Score : " + ScoreValue;

        if (PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetString("Score", "Score : " + ScoreValue.ToString());
        }
        if(ScoreValue >= 8 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("ScoreValue", ScoreValue);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
}
