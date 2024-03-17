﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
public class SaluteAnimator : MonoHandler
{


    [SerializeField]
    private UIParticleSystem suitParticle;
    [SerializeField]
    private Texture[] suitTextures;


    // START SHOWING ANIMATION
    public void Show(int suit)
    {
        if (!GameSettings.Instance.isEffectSet) return;
#if UNITY_EDITOR
        if (suit < 0 || suit > 3)
        {
            throw new UnityException("Suit index is out of range");
        }

#endif
        Solitaire_GameStake.Sound.Instance.CardFound();
        //Material mat = suitParticle.GetComponent<Renderer>().material;
        //mat.SetTexture("_MainTex", suitTextures[suit]);
        //suitParticle.material = mat;
        //suitParticle.GetComponent<ParticleSystem>().Play();
        //suitParticle.gameObject.SetActive(true);

    }

    public override void GUIEditor()
    {
#if UNITY_EDITOR
        if (GUILayout.Button("Change Text"))
        {
            suitParticle.GetComponent<Renderer>().material.SetTexture("_MainTex", suitTextures[2]);
        }
        base.GUIEditor();
#endif
    }


}
