using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeDebugUI : MonoBehaviour
{
    public TMP_InputField sideSizeInput;
    public Button stepButton;
    public Button startButton;
    public Button pauseButton;
    public Button resetButton;

    public TextMeshProUGUI moveDurationText;
    public TextMeshProUGUI totalDurationText;

    public MazeGenerator mg;
    private float moveTimer = 0.0f;
    private float totalTimer = 0.0f;

    private void Start()
    {
        stepButton.onClick.AddListener(Step);
        startButton.onClick.AddListener(StartMaze);
        pauseButton.onClick.AddListener(PauseMaze);
        resetButton.onClick.AddListener(ResetMaze);
    }

    private void Update()
    {
        if (mg.isRunning)
        {
            moveTimer += Time.deltaTime;
            totalTimer += Time.deltaTime;

            moveDurationText.text = $"Move Duration: {moveTimer:F2}s";
            totalDurationText.text = $"Total Duration: {totalTimer:F2}s";
        }
    }

    void Step()
    {
        mg.StepMaze();
        moveTimer = 0.0f;
    }

    void StartMaze()
    {
        mg.isRunning = true;
    }

    void PauseMaze()
    {
        mg.isRunning = false;
    }

    void ResetMaze()
    {
        mg.isRunning = false;
        moveTimer = 0.0f;
        totalTimer = 0.0f;

        if (int.TryParse(sideSizeInput.text, out int side))
            mg.ResetMaze(side);
        else
            mg.ResetMaze(10);

        AdjustCamera(side, side, mg.cellSize);
    }

    void AdjustCamera(int width, int height, float cellSize)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }

        //Center
        cam.transform.position = new Vector3(0, 0, -10);

        float halfWidth = (width * cellSize) / 2f;
        float halfHeight = (height * cellSize) / 2f;

        float aspect = (float)Screen.width / Screen.height;
        float sizeX = halfWidth / aspect;
        float sizeY = halfHeight;

        cam.orthographicSize = Mathf.Max(sizeX, sizeY) + 1f; //padding
    }


}
