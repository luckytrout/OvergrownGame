using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBlend : MonoBehaviour {

    public SkyboxBlender skyboxBlenderScript;
    [SerializeField]  public float rotateModifier = (float) 0.5;
    [SerializeField] [Range(-1,1)] public float blendModifier = (float) 100;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //float blend = Mathf.PingPong((float)(0.5 * Time.time), (float)1.0);
        skyboxBlenderScript.blend = Mathf.PingPong((float)(blendModifier* Time.time), (float)1.0);
        //RenderSettings.skybox.SetFloat("_Blend", blend);

        skyboxBlenderScript.rotation = rotateModifier * Time.time;
        if(skyboxBlenderScript.rotation > 358)
        {
            skyboxBlenderScript.rotation = 0;
        }
    }
}
