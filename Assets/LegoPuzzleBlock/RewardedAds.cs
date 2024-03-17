using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlockPuzzle_GameStake
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



        }
        public void OnVideoSuccessEvent()
        {
            if (rewardType == RewardType.Lifes)
            {
                //Debug.LogError("video watched successfully. Give lifes to user as a reward");
                //GameUI.instance.VideoSuccessEvent(true);
                GameController.instance.VideoSuccessEvent(true);
            }
            else if (rewardType == RewardType.Timer)
            {
                //Debug.LogError("video watched successfully. Give extra timer to user as a reward");
                //GameUI.instance.VideoSuccessEvent(false);
                GameController.instance.VideoSuccessEvent(false);

            }
        }

    }
}

