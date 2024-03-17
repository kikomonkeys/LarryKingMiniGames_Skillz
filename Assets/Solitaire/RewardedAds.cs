using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Solitaire_GameStake
{
    public class RewardedAds : MonoBehaviour
    {

        public static RewardedAds instance;

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
           // OnVideoSuccessEvent();

#if UNITY_IOS
         UnityiOSHandler.instance.SetCallBack(OnVideoSuccessEvent);
         NativeAPI.createRewardedAd("e58b7dc5e5fbbc64");
#endif

        }
        public void OnVideoSuccessEvent()
        {
            if (rewardType == RewardType.Lifes)
            {
                //Debug.LogError("video watched successfully. Give lifes to user as a reward");
                //GameUI.instance.VideoSuccessEvent(true);
                StageManager.instance.VideoSuccessEvent(true);
            }
            else if (rewardType == RewardType.Timer)
            {
                //Debug.LogError("video watched successfully. Give extra timer to user as a reward");
                // GameUI.instance.VideoSuccessEvent(false);
                StageManager.instance.VideoSuccessEvent(false);

            }
        }

    }
}

