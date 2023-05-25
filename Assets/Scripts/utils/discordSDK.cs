using UnityEngine;

#if !UNITY_EDITOR
using Discord;
#endif

public class discordSDK : MonoBehaviour
{
#if !UNITY_EDITOR
    public Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(
            1110289814584037458,
            (System.UInt64)Discord.CreateFlags.NoRequireDiscord
        );
        ActivityManager activityManager = discord.GetActivityManager();
        Activity activity = new Discord.Activity
        {
            State = "Killin'",
            Details = "playing shoty shoty"
        };
        activityManager.UpdateActivity(
            activity,
            (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Debug.Log("Starting Discord SDK.");
                }
            }
        );
    }

    void Update()
    {
        discord.RunCallbacks();
    }
#endif
}
