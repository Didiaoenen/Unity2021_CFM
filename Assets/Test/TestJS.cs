using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Puerts;

public class TestJS : MonoBehaviour
{
    public GameObject uiRoot;

    //JsEnv jsEnv;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var path = Path.Combine(Application.dataPath, "../TsProj/output/");
        //jsEnv = new JsEnv(new DefaultLoader(path), 9229);
        //jsEnv.Eval("require('GameMain')");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (jsEnv != null)
        //{
        //    jsEnv.Tick();
        //}
    }
}
