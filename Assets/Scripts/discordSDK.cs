using UnityEngine;
using Discord;

public class discordSDK : MonoBehaviour
{
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
                    Debug.Log("Everything is fine!");
                }
            }
        );
    }

    void Update()
    {
        discord.RunCallbacks();
    }
}
