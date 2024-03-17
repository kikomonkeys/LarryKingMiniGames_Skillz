using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stackball_RewardedAds : MonoBehaviour
{

    public static Stackball_RewardedAds instance;

    private void Awake()
    {
        instance = this;
    }
    public enum RewardType
    {
        Lifes,
        Timer
    }
    public RewardType rewardType;

    public void ShowRewardedAD()
    {
         //OnVideoSuccessEvent();

#if UNITY_IOS
         UnityiOSHandler.instance.SetCallBack(OnVideoSuccessEvent);
         NativeAPI.createRewardedAd("5a2dbc4d4eab14ad");
#endif

    }
    public void OnVideoSuccessEvent()
    {
        if (rewardType == RewardType.Lifes)
        {
            //Debug.LogError("video watched successfully. Give lifes to user as a reward");
            GameUI.instance.VideoSuccessEvent(true);
        }
        else if (rewardType == RewardType.Timer)
        {
            //Debug.LogError("video watched successfully. Give extra timer to user as a reward");
            GameUI.instance.VideoSuccessEvent(false);
        }
    }

}
