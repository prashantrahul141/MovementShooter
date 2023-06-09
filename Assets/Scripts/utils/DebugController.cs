using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    public bool showConsole = false;

    [SerializeField]
    private LogType logType;

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

    [SerializeField]
    private TMP_Text logTextField;

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

        if (Input.GetKeyDown(KeyCode.Return) && inputTextField.text.Length > 0)
        {
            OnReturn();
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        if (type > logType)
        {
            // for onscreen...
            logsCollected = logsCollected + "\n" + logString;
            if (logsCollected.Length > maxLogString)
            {
                logsCollected = logsCollected.Substring(logsCollected.Length - maxLogString);
            }
            logTextField.text = logsCollected;

            // for the file ...
            if (saveInFile)
            {
                if (filename == "")
                {
                    string d =
                        System.Environment.GetFolderPath(
                            System.Environment.SpecialFolder.CommonDocuments
                        ) + String.Format("/{0}", Application.productName);
                    System.IO.Directory.CreateDirectory(d);
                    string r = UnityEngine.Random.Range(1000, 9999).ToString();
                    filename = d + "/log-" + r + ".log";
                }
                try
                {
                    System.IO.File.AppendAllText(filename, logString + "\n");
                }
                catch (Exception saveEx)
                {
#if !UNITY_EDITOR
                    throw saveEx;
#else
                    Debug.LogError(saveEx);
#endif
                }
            }
        }
    }
}
