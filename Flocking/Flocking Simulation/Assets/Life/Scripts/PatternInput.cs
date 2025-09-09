using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatternInput : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button submitButton;
    [SerializeField] TextMeshProUGUI outputText;
    [SerializeField] Pattern targetPattern;
    [SerializeField] GameBoard board;

    private List<Vector2Int> collectedCells = new List<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(ProcessInput);
        outputText.text = "Enter Coordinates as 'x,y' or type 'done'";
    }

    private void ProcessInput()
    {
        string input = inputField.text.Trim().ToLower();

        if (input == "done")
        {
            targetPattern.cells = collectedCells.ToArray();
            outputText.text = "Finished and Saved " + collectedCells.Count + " Coordinates!";
            inputField.text = "";

            board.Begin();
            return;
        }

        string[] parts = input.Split(",");

        if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
        {
            Vector2Int coord = new Vector2Int(x, y);
            collectedCells.Add(coord);
            outputText.text = $"Added ({x}, {y}). Total: {collectedCells.Count}";
        }
        else
        {
            outputText.text = "Invalid input! Please enter 'x,y' or 'done'.";
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
