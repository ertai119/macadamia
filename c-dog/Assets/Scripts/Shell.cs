using UnityEngine;
using System.Collections;

public class Shell : PoolObject {

    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifetime = 4;
    float fadetime = 2;
    Color initColor;
    Material mat;

    void Awake()
    {
        mat = GetComponent<Renderer> ().material;
        initColor = mat.color;
    }

    void Start ()
    {
        mat.color = initColor;
        float force = Random.Range (forceMin, forceMax);
        myRigidbody.AddForce (transform.right * force);
        myRigidbody.AddTorque (Random.insideUnitSphere * force);

        StartCoroutine ("Fade");
    }

    public override void OnObjectReuse()
    {
        StopCoroutine("Fade");
        Start();
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadetime;

        while (percent < 1) {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initColor, Color.clear, percent);
            yield return null;
        }

        Destroy ();
    }
}
