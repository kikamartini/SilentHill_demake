using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{

    [SerializeField] GameObject cloudFog;

    [Header("Main Fog")]
    [SerializeField] float timeToStart;
    [SerializeField] float fogSpeed;

    [Header("Cloud Fog")]
    [SerializeField] float CloudFogStartTime;
    [SerializeField] float minCloudFogSpawnTime;
    [SerializeField] float maxCloudFogSpawnTime;
    float cloudFogSpawnTime;
    bool isSpawning;
    float xPosMax;
    float xPosMin;
    float yPosMax;
    float yPosMin;
    float xPos;
    float yPos;


    float time;
    float fogOpacity;

    
    

    //cached reference

    SpriteRenderer mainFog;


    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector2(0, 0);
        mainFog = GetComponent<SpriteRenderer>();
        fogOpacity = 0;
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;
        
        //main fog
        if(time >= timeToStart)
        {
            fogOpacity = (fogOpacity + (fogSpeed/255) * Time.deltaTime);
        }

        mainFog.color = new Color(255, 255, 255, fogOpacity);


        //cloud fog
        if(time >= CloudFogStartTime)
        {
            if (!isSpawning)
            {
                cloudFogSpawnTime = Random.Range(minCloudFogSpawnTime, maxCloudFogSpawnTime);
                isSpawning = true;
                Invoke("SpawnFogCloud", cloudFogSpawnTime);
            }
        }
    }



     private void SpawnFogCloud()
    {
        xPosMax = (gameObject.transform.localScale.x) / 2;
        xPosMin = -xPosMax;

        yPosMax = (gameObject.transform.localScale.y) / 2;
        yPosMin = -yPosMax;

        yPos = Random.Range(yPosMin, yPosMax);
        xPos = Random.Range(xPosMin, xPosMax);

        Vector2 cloudPos = new Vector2(xPos, yPos);

        Instantiate(cloudFog, cloudPos, Quaternion.identity);

        isSpawning = false;
    }


   

    public float GetFogOpacity()
    {
        return fogOpacity;
    }

}
