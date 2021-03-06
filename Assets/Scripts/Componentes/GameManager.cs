﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Jackyjjc.Bayesianet;


/// <summary>
/// Enumerado que controla los distintos estados del juego
/// </summary>
public enum SceneState { SETHERO, SETMAP, PLAY, GAMEOVER, NULL }

/// <summary>
/// Controla el estado del juego 
/// Guarda todas las constantes de juego y los prefabs de Uniades
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager Instance = null;

    //------------------CONSTANTES-------------------

    public const float DISTANCE = 0.64f;
    public const int WIDTH = 12;
    public const int HEIGHT = 6;
    public const int MAXENEMIES = 20;
    public const int MAXALLIES = 5;

    //Probabilidades del combate
    public const float PROBTHREEORMOREALLIES = 0.9f;
    public const float PROBTWOALLIES = 0.5f;
    public const float PROBONEALLY = 0.2f;
    public const float PROBNOLIGHT = -0.1f;

    //Puntos
    public const int POINTSENEMYDEADBYALLY = 1;
    public const int POINTSENEMYDEADBYHERO = 5;
    public const int POINTSALLYDEAD = -10;
    public const int POINTSHERODEAD = -50;


    //------------------CONSTANTES-------------------

    //------------------INSPECTOR-------------------

    public GameObject TilePrefab;
    public GameObject HeroPrefab;
    public GameObject AllyPrefab;
    public GameObject EnemyPrefab;

    public Button ButtonPlay;

    public GameObject GameOverPanel;
    public Text ScoreGameOverText;
    public Text HeroWinText;

    public GameObject ProbPanel;
    public GameObject DecisionPanel;

    //------------------INSPECTOR-------------------

    //------------------PROPIEDADES-------------------
    /// <summary>
    /// Estado actual del juego
    /// </summary>
    public SceneState State { get; set; }

    /// <summary>
    /// Variable principal de la red Bayesiana
    /// Tiene la tabla
    /// </summary>
    public VariableElimination VariableElimination { get; set; }

    //------------------PROPIEDADES-------------------


    //----------------ATRIBUTOS PRIVADOS------------------

    //----------------ATRIBUTOS PRIVADOS------------------

    void Awake()
    {
        //GameManager es Singleton
        Instance = this;

        State = SceneState.NULL;

        ButtonPlay.gameObject.SetActive(false);

        GameOverPanel.SetActive(false);

        string networkJson = (Resources.Load("barcos_json") as TextAsset).text;
        VariableElimination = new VariableElimination(new BayesianJsonParser().Parse(networkJson));

        ProbPanel.SetActive(false);
        DecisionPanel.SetActive(false);

    }

    void Start()
    {
        State = SceneState.SETHERO;
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))    
            Application.Quit(); 
        

    }

    /// <summary>
    /// Es llamado cuando se pulsa el botón que inicia el juego
    /// Cambia al estado de juego, Inicia la probabilidad y empieza la corrutina del juego
    /// </summary>
    public void InitGame()
    {
        //Cambio de estado
        State = SceneState.PLAY;
        ButtonPlay.gameObject.SetActive(false);

        ProbPanel.SetActive(true);
        DecisionPanel.SetActive(true);

        //Empieza la corrutina de juego
        StartCoroutine(Map.Instance.OnTurn());

    }

    /// <summary>
    /// Es llamado cuando acaba la partida
    /// Muestra todas las estadísticas
    /// </summary>
    public void GameOver()
    {
        State = SceneState.GAMEOVER;

        GameOverPanel.SetActive(true);

        ScoreGameOverText.text = "Puntuación: " + Map.Instance.Score;

        //Se modifica el texto dependiendo de si el héroe vence o muere
        if(Map.Instance.Hero!= null)      
            HeroWinText.text = "El héroe ha vencido";
        else
            HeroWinText.text = "El héroe ha muerto";

    }

    /// <summary>
    /// Es llamado por el botón de GameOver
    /// </summary>
    public void ResetScene()
    {
        SceneManager.LoadScene("IAVP4G01");
    }
}
