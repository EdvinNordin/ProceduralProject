using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{

    public float numObjects = 0;
    public float radius = 0;
    public GameObject prefab;
    GameObject Closest;
    public float seed = 10;
    //GameObject[] cloudObjects;

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
        float randY = Mathf.PerlinNoise(i * i * randX, i * i * randX) * 2 - 1;
        float randZ = Mathf.PerlinNoise(i * i * randY, i * i * randY) * 2 - 1;

        Debug.Log(i+ ": " + randX + ", " + randY + ", " + randZ);

        Vector3 vec = new Vector3(randX, randY, randZ).normalized * radius;

        Cloud.transform.position = vec;

        Closest = GetClosest(Cloud, i);


        if (Closest != null)
        {
            if (Vector3.Distance(Cloud.transform.position, Closest.transform.position) < 3.0)
            {
                vec = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));

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

    GameObject GetClosest(GameObject Cloud, float amount)
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

    void Update()
    {

        /*float randomF = hash(hash(Time.deltaTime + 1f) + seed);

        //Debug.Log(randomF);
        //Debug.Log(numObjects);
        if (randomF < 6)
        {
            cloudObjects.RemoveAt((int)randomF);
            numObjects--;
        }
        else if (randomF > 284)
        {
            spawnCloud(numObjects);
            numObjects++;
        }
        //
        // spawnCloud(cloudObjects.Count);

        if (a > numObjects || a >= b)
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
        }
        /*/

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


    /*
    public static float Noise3D(float x, float y, float z, float frequency, float amplitude, float persistence, int octave, int seed)
    {
        float noise = 0.0f;

        for (int i = 0; i < octave; ++i)
        {
            // Get all permutations of noise for each individual axis
            float noiseXY = Mathf.PerlinNoise(x * frequency + seed, y * frequency + seed) * amplitude;
            float noiseXZ = Mathf.PerlinNoise(x * frequency + seed, z * frequency + seed) * amplitude;
            float noiseYZ = Mathf.PerlinNoise(y * frequency + seed, z * frequency + seed) * amplitude;

            // Reverse of the permutations of noise for each individual axis
            float noiseYX = Mathf.PerlinNoise(y * frequency + seed, x * frequency + seed) * amplitude;
            float noiseZX = Mathf.PerlinNoise(z * frequency + seed, x * frequency + seed) * amplitude;
            float noiseZY = Mathf.PerlinNoise(z * frequency + seed, y * frequency + seed) * amplitude;

            // Use the average of the noise functions
            noise += (noiseXY + noiseXZ + noiseYZ + noiseYX + noiseZX + noiseZY) / 6.0f;

            amplitude *= persistence;
            frequency *= 2.0f;
        }

        // Use the average of all octaves
        return noise / octave;
    }
    private static readonly byte[] perm = {
    151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,
    140,36,103,30,69,142,8,99,37,240,21,10,23,190, 6,148,247,120,234,75,0,26,197,
    62,94,252,219,203,117,35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,
    168, 68,175,74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,
    133,230,220,105,92,41,55,46,245,40,244,102,143,54, 65,25,63,161, 1,216,80,73,
    209,76,132,187,208, 89,18,169,200,196,135,130,116,188,159,86,164,100,109,198,
    173,186, 3,64,52,217,226,250,124,123,
  5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
  223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
  129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
  251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
  49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
  138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
  151,160,137,91,90,15,
  131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
  190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
  88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
  77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
  102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
  135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
  5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
  223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
  129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
  251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
  49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
  138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
};


    float noise1(float x)
    {
        int ix0, ix1;
        float fx0, fx1;
        float s, n0, n1;

        ix0 = FASTFLOOR(x); // Integer part of x
        fx0 = x - ix0;       // Fractional part of x
        fx1 = fx0 - 1.0f;
        ix1 = (ix0 + 1) & 0xff;
        ix0 = ix0 & 0xff;    // Wrap to 0..255

        s = FADE(fx0);

        n0 = grad1(perm[ix0], fx0);
        n1 = grad1(perm[ix1], fx1);
        return 0.188f * (LERP(s, n0, n1));

       /* var i0 = FastFloor(x);
        var i1 = i0 + 1;
        var x0 = x - i0;
        var x1 = x0 - 1.0f;

        var t0 = 1.0f - x0 * x0;
        t0 *= t0;
        var n0 = t0 * t0 * grad1(perm[i0 & 0xff], x0);

        var t1 = 1.0f - x1 * x1;
        t1 *= t1;
        var n1 = t1 * t1 * grad1(perm[i1 & 0xff], x1);
        // The maximum value of this noise is 8*(3/4)^4 = 2.53125
        // A factor of 0.395 scales to fit exactly within [-1,1]
        return 0.395f * (n0 + n1);


    }

    private static float grad1(int hash, float x)
    {
        var h = hash & 15;
        var grad = 1.0f + (h & 7);   // Gradient value 1.0, 2.0, ..., 8.0
        if ((h & 8) != 0) grad = -grad;         // Set a random sign for the gradient
        return (grad * x);           // Multiply the gradient with the distance
    }

    private static int FASTFLOOR(float x)
    {
        return (x > 0) ? ((int)x) : (((int)x) - 1);
    }

    float FADE(float t) 
    {
        return (t * t * t * (t * (t * 6 - 15) + 10));
    }


    float LERP(float t, float a, float b) 
    {
        return ((a) + (t) * ((b) - (a)));
    }

    float snoise1(float x)
    {

        int i0 = FASTFLOOR(x);
        int i1 = i0 + 1;
        float x0 = x - i0;
        float x1 = x0 - 1.0f;

        float n0, n1;

        float t0 = 1.0f - x0 * x0;
        //  if(t0 < 0.0f) t0 = 0.0f; // this never happens for the 1D case
        t0 *= t0;
        n0 = t0 * t0 * grad1(perm[i0 & 0xff], x0);

        float t1 = 1.0f - x1 * x1;
        //  if(t1 < 0.0f) t1 = 0.0f; // this never happens for the 1D case
        t1 *= t1;
        n1 = t1 * t1 * grad1(perm[i1 & 0xff], x1);
        // The maximum value of this noise is 8*(3/4)^4 = 2.53125
        // A factor of 0.395 would scale to fit exactly within [-1,1], but
        // we want to match PRMan's 1D noise, so we scale it down some more.
        return 0.25f * (n0 + n1);

    }*/

    /*
     int[][] grad3 = {{1,1,0},{-1,1,0},{1,-1,0},{-1,-1,0},{1,0,1},{-1,0,1},{1,0,-1},{-1,0,-1},{0,1,1},{0,-1,1},{0,1,-1},{0,-1,-1}};
     private static int[] p = {151,160,137,91,90,15,
     131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
     190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
     88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
     77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
     102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
     135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
     5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
     223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
     129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
     251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
     49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
     138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};

    // To remove the need for index wrapping, double the permutation table length
    private static int[] perm = new int[512];
    //for(int i=0; i<512; i++) perm[i]=p[i & 255]; }
// This method is a *lot* faster than using (int)Math.floor(x)
private static int fastfloor(double x)
{
    return x > 0 ? (int)x : (int)x - 1;
}
private static double dot(int[] g, double x, double y, double z)
{
    return g[0] * x + g[1] * y + g[2] * z;
}
private static double mix(double a, double b, double t)
{
    return (1 - t) * a + t * b;
}
private static double fade(double t)
{
    return t * t * t * (t * (t * 6 - 15) + 10);
}


public static double noise(double xin, double yin, double zin)
    {
        double n0, n1, n2, n3; // Noise contributions from the four corners
                               // Skew the input space to determine which simplex cell we're in
        double F3 = 1.0 / 3.0;
        double s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D
        int i = fastfloor(xin + s);
        int j = fastfloor(yin + s);
        int k = fastfloor(zin + s);
        double G3 = 1.0 / 6.0; // Very nice and simple unskew factor, too
        double t = (i + j + k) * G3;
        double X0 = i - t; // Unskew the cell origin back to (x,y,z) space
        double Y0 = j - t;
        double Z0 = k - t;
        double x0 = xin - X0; // The x,y,z distances from the cell origin
        double y0 = yin - Y0;
        double z0 = zin - Z0;
        // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
        // Determine which simplex we are in.
        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
        if (x0 >= y0)
        {
            if (y0 >= z0)
            { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
        }
        else
        { // x0<y0
            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
        }
        // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
        // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
        // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
        // c = 1/6.
        double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
        double y1 = y0 - j1 + G3;
        double z1 = z0 - k1 + G3;
        double x2 = x0 - i2 + 2.0 * G3; // Offsets for third corner in (x,y,z) coords
        double y2 = y0 - j2 + 2.0 * G3;
        double z2 = z0 - k2 + 2.0 * G3;
        double x3 = x0 - 1.0 + 3.0 * G3; // Offsets for last corner in (x,y,z) coords
        double y3 = y0 - 1.0 + 3.0 * G3;
        double z3 = z0 - 1.0 + 3.0 * G3;
        // Work out the hashed gradient indices of the four simplex corners
        int ii = i & 255;
        int jj = j & 255;
        int kk = k & 255;
        int gi0 = perm[ii + perm[jj + perm[kk]]] % 12;
        int gi1 = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]] % 12;
        int gi2 = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]] % 12;
        int gi3 = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]] % 12;
        // Calculate the contribution from the four corners
        double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
        if (t0 < 0) n0 = 0.0;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
        }
        double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
        if (t1 < 0) n1 = 0.0;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
        }
        double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
        if (t2 < 0) n2 = 0.0;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
        }
        double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
        if (t3 < 0) n3 = 0.0;
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
        }
        // Add contributions from each corner to get the final noise value.
        // The result is scaled to stay just inside [-1,1]
        return 32.0 * (n0 + n1 + n2 + n3);
    }*/
}

