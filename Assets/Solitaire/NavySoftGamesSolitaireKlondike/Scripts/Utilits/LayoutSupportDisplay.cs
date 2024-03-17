using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum LayoutSupportType
{
    LayoutElement,
    HorizontalOrVerticalLayoutGroup
}
public class LayoutSupportDisplay : MonoBehaviour
{
    [System.Serializable]
    public class LayoutValue
    {
        public float minWidth = 0;
        public float minHeight = 0;
        public bool useDefaultValue = true;
        public bool useRateRect = false;
    }

    [System.Serializable]
    public class HorizontalOrVerticalLayoutValue
    {
        public int left = 0;
        public int right = 0;
        public int top = 0;
        public int bottom = 0;
        public float spacing = 0;
        public bool useDefaultValue = true;
    }
    [SerializeField]
    private LayoutSupportType layoutSupportType;

    [SerializeField]
    private RectTransform rectTarget;


    [SerializeField]
    private LayoutValue layoutNormalRatio;
    [SerializeField]
    private LayoutValue layoutLarge2Ratio;
    [SerializeField]
    private LayoutValue layoutEqual2Ratio;
    [SerializeField]
    private LayoutValue layoutTabletRatio;
   
    [Header("Horizontal Or Vertical LayoutGroup")]

  

    [SerializeField]
    private HorizontalOrVerticalLayoutValue layoutGroupNormalRatio;
    [SerializeField]
    private HorizontalOrVerticalLayoutValue layoutGroupLarge2Ratio;
    [SerializeField]
    private HorizontalOrVerticalLayoutValue layoutGroupEqual2Ratio;
    [SerializeField]
    private HorizontalOrVerticalLayoutValue layoutGroupTabletRatio;


    private bool isUpdate = false;
  
    private void Start()
    {
       
        switch (layoutSupportType)
        {
           
            case LayoutSupportType.LayoutElement:
                LayoutElement layoutElement = GetComponent<LayoutElement>();
 
                if(RatioResolution.GetResolution()>2)
                {
                    if (layoutLarge2Ratio.useDefaultValue) return;

                     SetUpLayoutElement(layoutElement, layoutLarge2Ratio);
                     
                }
                else if (Mathf.FloorToInt( RatioResolution.GetResolution()) == 2)
                {
                    if (layoutEqual2Ratio.useDefaultValue) return;
                    SetUpLayoutElement(layoutElement, layoutEqual2Ratio);
                }
                else if (RatioResolution.GetResolution() <=1.5f)
                {
                    if (layoutTabletRatio.useDefaultValue) return;
                    SetUpLayoutElement(layoutElement, layoutTabletRatio);
                }
                else
                {
                    if (layoutNormalRatio.useDefaultValue) return;
                    SetUpLayoutElement(layoutElement, layoutNormalRatio);
                }

                    break;
            case LayoutSupportType.HorizontalOrVerticalLayoutGroup:
                HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
                if (RatioResolution.GetResolution() > 2)
                {
                    if (layoutGroupLarge2Ratio.useDefaultValue) return;
                    SetUpLayoutGroup(horizontalOrVerticalLayoutGroup, layoutGroupLarge2Ratio);

                }
                else if (Mathf.FloorToInt(RatioResolution.GetResolution()) == 2)
                {
                    if (layoutGroupEqual2Ratio.useDefaultValue) return;
                    SetUpLayoutGroup(horizontalOrVerticalLayoutGroup, layoutGroupEqual2Ratio);
                }
                else if (RatioResolution.GetResolution() <= 1.5f)
                {
                    if (layoutGroupTabletRatio.useDefaultValue) return;
                    SetUpLayoutGroup(horizontalOrVerticalLayoutGroup, layoutGroupTabletRatio);
                }
                else
                {
                    if (layoutGroupNormalRatio.useDefaultValue) return;
                    SetUpLayoutGroup(horizontalOrVerticalLayoutGroup, layoutGroupNormalRatio);
                }
                break;
        }
    }

    private void Update()
    {
        if (!isUpdate) return;
        Start();
    }
     
    private void SetUpLayoutElement(LayoutElement layoutElement ,LayoutValue value)
    {

        
        if (value.useRateRect)
        {
            isUpdate = true;
            layoutElement.minHeight = rectTarget.rect.height/(value.minHeight==0 ? rectTarget.rect.height : value.minHeight);
            layoutElement.minWidth = rectTarget.rect.width / value.minWidth;
        }
        else
        {
            layoutElement.minHeight = value.minHeight;
            layoutElement.minWidth = value.minWidth;
        }
    }

    private void SetUpLayoutGroup(HorizontalOrVerticalLayoutGroup layoutGroup, HorizontalOrVerticalLayoutValue value)
    {

        layoutGroup.padding.left = value.left;
        layoutGroup.padding.right = value.right;
        layoutGroup.padding.top = value.top;
        layoutGroup.padding.bottom = value.bottom;
        layoutGroup.spacing = value.spacing;
    }
}
