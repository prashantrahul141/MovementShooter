using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField]
    public bool showConsole = false;

    [SerializeField]
    bool saveInFile = false;

    [SerializeField]
    string filename = "";

    string logsCollected = "*begin log";
    string inputString = "";
    int kChars = 700;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            showConsole = !showConsole;
            Cursor.lockState = showConsole ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = showConsole ? true : false;
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        logsCollected = logsCollected + "\n" + logString;
        if (logsCollected.Length > kChars)
        {
            logsCollected = logsCollected.Substring(logsCollected.Length - kChars);
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

    void OnGUI()
    {
        if (!showConsole)
        {
            return;
        }
        GUI.matrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f)
        );
        GUI.TextArea(new Rect(10, 10, Screen.width - 60, 370), logsCollected);
        inputString = GUI.TextField(new Rect(10, 380, Screen.width - 60, 30f), inputString);
    }
}
