using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 100;
    public float addValue = 4;
    [SerializeField]
    float finalSpeed;
    private void Start()
    {

        finalSpeed = (addValue * PlayerPrefs.GetInt("Level", 1) + speed);
        if(finalSpeed >= 100)
        {
            finalSpeed = 100;
        }
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, finalSpeed * Time.deltaTime , 0));
    }
}
