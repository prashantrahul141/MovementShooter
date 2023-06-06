using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    public bool showConsole = false;

    [SerializeField]
    bool saveInFile = false;

    [SerializeField]
    string filename = "";

    string logsCollected = "*begin log";
    int maxLogString = 700;

    // refs
    [SerializeField]
    private GameObject UIPanel;

    [SerializeField]
    private TMP_InputField inputTextField;

    // commands
    public static DebugCommand QUIT;
    public static DebugCommand TOGGLE_CONSOLE;
    public List<object> commandList;

    void Start()
    {
        UIPanel.SetActive(showConsole);

        // all commands
        QUIT = new DebugCommand(
            "quit",
            "Quit game.",
            "kill_all",
            () =>
            {
                Application.Quit();
            }
        );

        TOGGLE_CONSOLE = new DebugCommand(
            "toggle_console",
            "toggles the console window",
            "toggle_console",
            () =>
            {
                changeUIState(!showConsole);
            }
        );

        commandList = new List<object> { QUIT, TOGGLE_CONSOLE };
    }

    void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            inputTextField.SetTextWithoutNotify("");
        }
    }

    void HandleInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (inputTextField.text.ToString().Contains(commandBase.commandID))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    public void changeUIState(bool state)
    {
        showConsole = state;
        UIPanel.SetActive(showConsole);
        Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = showConsole;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            changeUIState(!showConsole);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnReturn();
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        logsCollected = logsCollected + "\n" + logString;
        if (logsCollected.Length > maxLogString)
        {
            logsCollected = logsCollected.Substring(logsCollected.Length - maxLogString);
        }

        // for the file ...
        if (saveInFile)
        {
            if (filename == "")
            {
                string d =
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
                    + "/YOUR_LOGS";
                System.IO.Directory.CreateDirectory(d);
                string r = Random.Range(1000, 9999).ToString();
                filename = d + "/log-" + r + ".txt";
            }
            try
            {
                System.IO.File.AppendAllText(filename, logString + "\n");
            }
            catch { }
        }
    }
}
