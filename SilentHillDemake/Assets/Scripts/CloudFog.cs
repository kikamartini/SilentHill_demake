using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFog : MonoBehaviour
{

    [SerializeField] float minFogSpeed;
    [SerializeField] float maxFogSpeed;
    float fogSpeed;
    int fogDirection;

    // Start is called before the first frame update
    void Start()
    {
        fogSpeed = Random.Range(minFogSpeed, maxFogSpeed);

        int fogDirectionRaw = Random.Range(0, 2);

        fogDirection = (fogDirectionRaw == 0) ? -1 : 1;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * fogDirection * Time.deltaTime * fogSpeed);
    }
}
