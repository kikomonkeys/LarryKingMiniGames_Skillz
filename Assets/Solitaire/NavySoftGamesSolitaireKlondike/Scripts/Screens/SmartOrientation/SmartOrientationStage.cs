using System.Collections.Generic;

namespace SmartOrientation
{
	using UnityEngine;
	using System.Collections;

	public class SmartOrientationStage : CanvasSmartOrientation
	{

		[System.Serializable]
		public class SmartTransformHand
		{
			public Transform target;
			public Transform portraitLeft;
			public Transform portraitRight;
			public Transform landscapeLeft;
			public Transform landscapeRight;
		}

		[System.Serializable]
		public class SmartTransform
		{
			public Transform target;
			public Transform portrait;
			public Transform landscape;
		}

		public List<SmartTransformHand> stackTransforms = new List<SmartTransformHand>();
		public List<SmartTransformHand> foundationTransforms = new List<SmartTransformHand>();
        public List<SmartTransformHand> tableuSmartHandTransforms = new List<SmartTransformHand>();
     
    
        public override void OnDeviceOrientationChanged (bool orientation)
		{
          

			foreach (var s in stackTransforms) {
                StartCoroutine(applyTransformByOrientation (s, orientation));
         
            }
			foreach (var s in foundationTransforms) {
				StartCoroutine( applyTransformByOrientation (s, orientation));
               
            }
            foreach (var s in tableuSmartHandTransforms)
            {
                StartCoroutine(applyTransformByOrientation(s, orientation));
              
            }
            //if (//GoogleMobileAdsScript.instance != null)
            {
                //GoogleMobileAdsScript.instance.VisbileBanner(orientation);
            }
           
        }

		void applyTransformByOrientation(SmartTransform st, bool isPortrait){
			st.target.position = isPortrait? st.portrait.position : st.landscape.position;
		}

        IEnumerator applyTransformByOrientation(SmartTransformHand st, bool isPortrait)
        {
            Transform newTransform;


            bool isLeftHand = GameSettings.Instance.isHandSet;



            yield return new WaitForSeconds(1);


            if (isPortrait)
            {

                newTransform = isLeftHand ? st.portraitLeft : st.portraitRight;
            }
            else
            {

                newTransform = isLeftHand ? st.landscapeLeft : st.landscapeRight;
            }

           
            st.target.position = newTransform.position;

            ConvertSizeCard(isPortrait);
            SolitaireStageViewHelperClass.instance.SetDistanceBetweenCard(true);
        }

        private void ConvertSizeCard(bool isPortrait)
        {
            SolitaireStageViewHelperClass.instance.ScaleCard(isPortrait);

        }

        public void ToggleBottomPanel()
        {
            if (HUDController.instance == null) return;
            HUDController.instance.ToggleBottomPanel();
        }
    }
}
