using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public int numObjects = 500;
    public float radius = 11;
    public GameObject prefab;
    GameObject Closest;
    public float seed = 123;
    public float distance = 3.0f;
    public float spacing = 1.5f;
    //GameObject[] cloudObjects;
    public float AddCloud = 5;
    public float RemoveCloud = 5;

    List<GameObject> cloudObjects;

    void Start()
    {
        cloudObjects = new List<GameObject>();
        
        for(float i = 0; i<numObjects; i++)
        {
            spawnCloud(i);
        }


    }


    void spawnCloud(float i)
    {
        
        GameObject Cloud = Instantiate(prefab);

        float count = (int)Mathf.Floor(Mathf.Log10(seed) + 1);

        float ran = seed/Mathf.Pow(10, count);


        float randX = Mathf.PerlinNoise(i*i*ran, i*i*ran) * 2f - 1f;
        float randY = Mathf.PerlinNoise(i * i * randX, i * i * randX) * 2f - 1f;
        float randZ = Mathf.PerlinNoise(i * i * randY, i * i * randY) * 2f - 1f;

        //Debug.Log(i+ ": " + randX + ", " + randY + ", " + randZ);

        Vector3 vec = new Vector3(randX, randY, randZ).normalized * radius;

        Cloud.transform.position = vec;
        Cloud.transform.rotation = Quaternion.LookRotation(vec, Vector3.forward);

        Cloud.transform.position = vec;

        Closest = GetClosest(Cloud, i);


        string randomS = randX.ToString();
        int sLength = randomS.Length;
        char randLast0 = randomS[sLength - 2];
        char randLast1 = randomS[sLength - 1];
        float randomI0 = (int)System.Char.GetNumericValue(randLast0);
        float randomI1 = (int)System.Char.GetNumericValue(randLast1);
        float randomIX = ((randomI0 * 10 + randomI1)/ 100) * 2 - 1;

        randomS = randY.ToString();
        sLength = randomS.Length;
        randLast0 = randomS[sLength- 2];
        randLast1 = randomS[sLength - 1];
        randomI0 = (int)System.Char.GetNumericValue(randLast0);
        randomI1 = (int)System.Char.GetNumericValue(randLast1);
        float randomIY = ((randomI0 * 10 + randomI1)/ 100) * 2 - 1;

        randomS = randZ.ToString();
        sLength = randomS.Length;
        randLast0 = randomS[sLength - 2];
        randLast1 = randomS[sLength - 1];
        randomI0 = (int)System.Char.GetNumericValue(randLast0);
        randomI1 = (int)System.Char.GetNumericValue(randLast1);
        float randomIZ = ((randomI0 * 10 + randomI1)/100) * 2 - 1;

        if (Closest != null)
        {
            if (Vector3.Distance(Cloud.transform.position, Closest.transform.position) < distance)
            {
                
                //vec = new Vector3(Random.Range(-spacing, spacing), Random.Range(-spacing, spacing), Random.Range(-spacing, spacing));
                vec = new Vector3(randomIX * spacing, randomIY * spacing, randomIZ * spacing);
                Cloud.transform.position = (Closest.transform.position + vec).normalized * radius;
            }
            else
            {
                Cloud.transform.position = vec;
            }
        }
        cloudObjects.Add(Cloud);
       // Debug.Log(cloudObjects.Count);
    }

    GameObject GetClosest(GameObject Cloud, float amount) //get the closest cloud
    {
        GameObject near = null;
        Vector3 currentPos = Cloud.transform.position;
        float minDist = 999999.0f; //börjar högt så objekten kan vara mindre
        float dist;

        for (int i = 0; i < amount; i++)
        {
            if (cloudObjects[i] != null)
            {
                dist = Vector3.Distance(cloudObjects[i].transform.position, currentPos);

                if (dist < minDist)
                {
                    near = cloudObjects[i];
                    minDist = dist;
                }
            }
        }
        return near;
    }

    float hash(float i)
    {
        return ((34 * i * i + 10 * i) % 289);

    }

    int a = 0;
    int b = 0;
    float updates = 0f;

    void Update()
    {
        updates++;
        float count = (int)Mathf.Floor(Mathf.Log10(seed) + 1);

        float ran = seed / Mathf.Pow(10, count);

        float countObjects = (int)Mathf.Floor(Mathf.Log10(updates) + 1);

        float ranObjects = updates / Mathf.Pow(10, countObjects);

        //float randomF = hash(hash(Time.deltaTime + 1f) + seed);
        float randomF = Mathf.PerlinNoise((float)updates * (float)updates * ran, (float)updates * (float)updates * ran) * 100;

        string randomS = randomF.ToString();
        int sLength = randomS.Length;
        char randLast0 = randomS[sLength - 2];
        char randLast1 = randomS[sLength - 1];
        int randomI0 = (int)System.Char.GetNumericValue(randLast0);
        int randomI1 = (int)System.Char.GetNumericValue(randLast1);
        int randomI = randomI0 * 10 + randomI1;
        //Debug.Log(cloudObjects.Count);
        
        //Debug.Log(Time.deltaTime *Time.deltaTime * ran);
        if (RemoveCloud > randomI && cloudObjects.Count > 10)
        {
            
            numObjects--;
            Destroy(GameObject.Find("Sphere(Clone)"), 0);
            cloudObjects.RemoveAt(0);

        }
        if (randomI > 100- AddCloud)
        {
            Debug.Log(randomI);
            spawnCloud(numObjects);
            numObjects++;
        }
        //
        // spawnCloud(cloudObjects.Count);

        /*if (a > numObjects || a >= b)
            a = 0;

        if (b > numObjects)
            b = 0;

        if (hash(hash(Time.deltaTime + 1) + seed) < 5)
        {
            cloudAppear(a);
            a++;
        }
        else if (hash(hash(Time.deltaTime + 1) + seed) > 284)
        {
            //StartCoroutine(cloudDisappear(cloudObjects[b]));
            b++;
        }*/
        

    }

    void cloudAppear(int a)
    {
        GameObject Cloud = cloudObjects[a];
        float newScale = Mathf.Lerp(0, 1, Time.deltaTime / 10);
        Cloud.transform.localScale = new Vector3(newScale, newScale, newScale);

    }

    IEnumerator cloudAppear2(GameObject Cloud)
    {
        for (float i = 0.0F; i >= 1.0F; i += 0.1F)
        {

            Vector3 expand = new Vector3(i, i, i);
            Cloud.transform.localScale = expand;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator cloudDisappear(GameObject Cloud)
    {

        for(float i = 1.0F; i >= 0.0F; i -= 0.1F)
        {
            Vector3 shrink = new Vector3( i, i, i );
            Cloud.transform.localScale = shrink;
            yield return new WaitForSeconds(1);
        }
        //Destroy(Cloud);
    }
}

