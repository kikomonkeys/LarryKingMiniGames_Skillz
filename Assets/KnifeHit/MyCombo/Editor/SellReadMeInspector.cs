﻿//#define UAS
//#define CHUPA
#define SMA

#pragma warning disable 0618

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SellReadMe))]
public class SellReadMeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("1. Edit Game Settings (Admob, In-app Purchase..)", EditorStyles.boldLabel);

        if (GUILayout.Button("Edit Game Settings", GUILayout.MinHeight(40)))
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/KnifeHit/MyCombo/GameMaster.prefab");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("2. Game Documentation", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Full Documentation", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://drive.google.com/open?id=1Ne1QP0qnoBvDjX2rdUByY08x0Cx1F41hsRER71ayEPk");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Setup In-app Purchase Guide", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://drive.google.com/open?id=1hcB7gxL-DYy12VOA-h78Xl5FshwM7jhRcjzGQnL6BJw");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Build For iOS Guide", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://drive.google.com/open?id=1rkgXuyFlJ2BhyNZkcn5ASuHunNExDwW5ypmFdXcd0uA");
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("3. My Other Great Source Codes", EditorStyles.boldLabel);
        
        if (products != null)
        {
            foreach (var product in products)
            {
                if (GUILayout.Button(product.name, GUILayout.MinHeight(30)))
                {
#if UAS
                    Application.OpenURL(product.uas);
#elif CHUPA
                    Application.OpenURL(product.chupa);
#elif SMA
                    Application.OpenURL(product.sma);
#endif
                }
                EditorGUILayout.Space();
            }
        }
        else
        {
            if (GUILayout.Button("Pixel Art - Color by Number", GUILayout.MinHeight(30)))
            {
#if UAS
                Application.OpenURL("https://assetstore.unity.com/packages/templates/systems/pixel-art-color-by-number-117587");
#elif CHUPA
                Application.OpenURL("https://www.chupamobile.com/unity-arcade/pixel-art-color-by-number-top-free-game-20127");
#elif SMA
                Application.OpenURL("https://www.sellmyapp.com/downloads/pixel-art-color-by-number-top-free-game/");
#endif
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("1Line: One-stroke Puzzle", GUILayout.MinHeight(30)))
            {
#if UAS
                Application.OpenURL("https://assetstore.unity.com/packages/templates/packs/1line-one-stroke-puzzle-118439");
#elif CHUPA
                Application.OpenURL("https://www.chupamobile.com/unity-family/1line-one-stroke-puzzle-20370");
#elif SMA
                Application.OpenURL("https://www.sellmyapp.com/downloads/1line-one-stroke-puzzle/");
#endif
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Sudoku Pro", GUILayout.MinHeight(30)))
            {
#if UAS
                Application.OpenURL("https://assetstore.unity.com/packages/templates/packs/sudoku-pro-118434");
#elif CHUPA
                Application.OpenURL("https://www.chupamobile.com/unity-arcade/sudoku-pro-20433");
#elif SMA
                Application.OpenURL("https://www.sellmyapp.com/downloads/sudoku-pro/");
#endif
            }

            EditorGUILayout.Space();
        }

        EditorGUILayout.LabelField("4. Contact Us For Support", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Email: ", "moana.gamestudio@gmail.com");
    }

    private List<MyProduct> products;
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("my_products"))
            products = JsonUtility.FromJson<MyProducts>(PlayerPrefs.GetString("my_products")).products;

        var www = new WWW("http://66.45.240.107/myproducts/my_products.json");
        ContinuationManager.Add(() => www.isDone, () =>
        {
            if (!string.IsNullOrEmpty(www.error)) return;
            PlayerPrefs.SetString("my_products", www.text);
            products = JsonUtility.FromJson<MyProducts>(www.text).products;

            Repaint();
        });
    }
}

[System.Serializable]
public class MyProduct
{
    public string name;
    public string uas;
    public string chupa;
    public string sma;
}

public class MyProducts
{
    public List<MyProduct> products;
}
