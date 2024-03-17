using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PoolGame_GameStake
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
            //OnVideoSuccessEvent();

#if UNITY_IOS
         UnityiOSHandler.instance.SetCallBack(OnVideoSuccessEvent);
         NativeAPI.createRewardedAd("2b03a53be0fb0763");
#endif

        }
        public void OnVideoSuccessEvent()
        {
            if (rewardType == RewardType.Lifes)
            {
                //Debug.LogError("video watched successfully. Give lifes to user as a reward");
                ScoreController.instance.VideoSuccessEvent(true);
            }
            else if (rewardType == RewardType.Timer)
            {
                //Debug.LogError("video watched successfully. Give extra timer to user as a reward");
                ScoreController.instance.VideoSuccessEvent(false);
            }
        }

    }
}

