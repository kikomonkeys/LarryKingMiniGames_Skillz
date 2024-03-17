using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Circle : MonoBehaviour
{
    public int totalKnife = 5;
    public List<RotationVariation> RandomRotation = new List<RotationVariation>();
    public List<LevelVariation> RandomLevels = new List<LevelVariation>();

    public ParticleSystem hitParticle, splashParticle;
    [Space(20)]
    public bool isBoss = false;
    public Sprite woodSprite, blueWoodSprite;
    public ParticleSystem WoodSplatParticle, BlueWoodSplatParticle;
    [Space(20)]
    public bool isRandomClockWise = false;

    public List<Knife> hitedKnife = new List<Knife>();
    public AudioClip woodHitSfx, LasthitSfx;
    int currentRoationIndex = 0;
    int currentLevelndex = 0;
    float valueZ;

    [HideInInspector]
    public float circleScale;

    void Start()
    {
        if (!isBoss)
        {
            GetComponent<SpriteRenderer>().sprite = GameManager.Stage % 10 < 5 ? woodSprite : blueWoodSprite;
        }

        if (RandomRotation.Count > 0)
        {
            ApplyRotation();
        }
        currentLevelndex = 0;// Random.Range(0, RandomLevels.Count);
       // print("Current Level" + currentLevelndex);
        //if (RandomLevels[currentLevelndex].applePosibility > Random.value)
        {
            SpawnApple();
        }
        SpawnKnife();
    }

    void ApplyRotation()
    {
        currentRoationIndex = (currentRoationIndex + 1) % RandomRotation.Count;
        LeanTween.rotateZ(gameObject, transform.localRotation.eulerAngles.z + RandomRotation[currentRoationIndex].z, RandomRotation[currentRoationIndex].time).setOnComplete(ApplyRotation).setEase(RandomRotation[currentRoationIndex].curve);
    }

    void SpawnApple()
    {
        foreach (float item in RandomLevels[currentLevelndex].AppleAngles)
        {
            GameObject tempApple = Instantiate<GameObject>(GamePlayManager.instance.ApplePrefab);
            tempApple.transform.SetParent(transform);
            SetPosInCircle(transform, tempApple.transform, item, 0.45f, -90f);//0.28
            //tempApple.transform.localScale = Vector3.one;
            tempApple.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
    }

    void SpawnKnife()
    {
        foreach (float item in RandomLevels[currentLevelndex].KnifeAngles)
        {
            GameObject tempKnife = Instantiate<GameObject>(GamePlayManager.instance.knifePrefab.gameObject);
            tempKnife.transform.SetParent(transform);
            tempKnife.GetComponent<Knife>().isHitted = true;
            tempKnife.GetComponent<Knife>().isFire = true;
            tempKnife.GetComponents<BoxCollider2D>()[0].enabled = true;
            tempKnife.GetComponents<BoxCollider2D>()[1].enabled = true;
            SetPosInCircle(transform, tempKnife.transform, item, 0f, 90f);

            float knifeScale = GamePlayManager.instance.knifeScale / circleScale;
            tempKnife.transform.localScale = Vector3.one;
        }
    }

    void SetPosInCircle(Transform circle, Transform obj, float angle, float spaceBetweenCircleAndObject, float objAngelOffset)
    {
        angle = angle + 90f;
        Vector2 offset = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * (circle.GetComponent<CircleCollider2D>().radius + spaceBetweenCircleAndObject);
        obj.localPosition = (Vector2)circle.localPosition + offset;
        obj.localRotation = Quaternion.Euler(0, 0, -angle + 90f + objAngelOffset);
    }

    public void OnKnifeHit(Knife k)
    {

        k.rb.isKinematic = true;
        k.rb.velocity = Vector2.zero;
        k.transform.SetParent(transform);
        k.isHitted = true;
        hitedKnife.Add(k);
        PlayParticle(k.transform.position, hitParticle);
        LeanTween.moveLocalY(gameObject, 0.1f, 0.05f).setLoopPingPong(1);
        if (hitedKnife.Count >= totalKnife)
        {
            if (!GameManager.isGameOver)
            {
                StartCoroutine(RelaseAllKnife());
                SoundManager.instance.PlaySingle(LasthitSfx);
            }
        }
        else
        {
            var splash = PlayParticle(GamePlayManager.instance.circleSpawnPoint.transform.position, splashParticle);

            var splashScale = GetComponent<CircleCollider2D>().radius * 2 * transform.localScale.x / 10f;
            splash.transform.localScale = Vector3.one * splashScale;

            SoundManager.instance.PlaySingle(woodHitSfx);
        }
        if (!GamePlayManager.isKnifeHitToBubble)
        {
            GamePlayManager.instance.SpawnPointsText(GameManager.Stage + 1, 60);
        }
        else
        {
            GamePlayManager.isKnifeHitToBubble = false;
        }

        GameManager.score+= GameManager.Stage+1;

    }

    private ParticleSystem PlayParticle(Vector3 pos, ParticleSystem _particle)
    {
        ParticleSystem tempParticle = Instantiate(_particle);
        tempParticle.transform.position = pos;
        tempParticle.Play();
        return tempParticle;
    }

    public IEnumerator RelaseAllKnife()
    {
        LeanTween.cancel(gameObject);
        if (!isBoss)
        {
            PlayParticle(transform.position, GameManager.Stage % 10 < 5 ? WoodSplatParticle : BlueWoodSplatParticle);
        }
        else
        {
            PlayParticle(transform.position, WoodSplatParticle);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.02f);
        foreach (Transform item in transform)
        {
            if (item.transform.tag.Equals("Knife"))
            {
                item.GetComponents<BoxCollider2D>()[0].enabled = false;
                item.GetComponents<BoxCollider2D>()[1].enabled = false;

                item.GetComponent<Knife>().rb.isKinematic = false;
                item.GetComponent<Knife>().rb.gravityScale = 2.5f;
                item.GetComponent<Knife>().rb.freezeRotation = false;
                item.GetComponent<Knife>().rb.angularVelocity = Random.Range(-20f, 20f) * 35f;
                item.GetComponent<Knife>().rb.AddForce(new Vector2(Random.Range(-10f, 10f), Random.Range(3f, 10f)), ForceMode2D.Impulse);
                item.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
                item.GetComponent<Knife>().DestroyMe();

            }
            else if (item.transform.tag.Equals("Apple"))
            {
                item.GetComponent<Apple>().rb.isKinematic = false;
                item.GetComponent<Apple>().rb.gravityScale = 2.5f;
                item.GetComponent<Apple>().rb.freezeRotation = false;
                item.GetComponent<Apple>().rb.angularVelocity = Random.Range(-20f, 20f) * 35f;
                item.GetComponent<Apple>().rb.AddForce(new Vector2(Random.Range(-6f, 6f), Random.Range(3f, 10f)), ForceMode2D.Impulse);
                item.GetComponent<CircleCollider2D>().enabled = false;
                item.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }

        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        GamePlayManager.instance.NextLevel();

    }

    public void DestroyMeAndAllKnives()
    {
        foreach (Knife item in hitedKnife)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        Destroy(gameObject);
    }
}

[System.Serializable]
public class RotationVariation
{
    [Range(0f, 2f)]
    public float time = 0f;
    [Range(-180, 180f)]
    public float z = 0f;
    public AnimationCurve curve;
}

[System.Serializable]
public class LevelVariation
{
    //[Range(0f, 1f)]
    //public float applePosibility = 0.5f;
    public List<float> AppleAngles = new List<float>();
    public List<float> KnifeAngles = new List<float>();
}
