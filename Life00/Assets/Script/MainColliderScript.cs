using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainColliderScript : MonoBehaviour
{

    [SerializeField] private GameObject pfsquare;
    // Start is called before the first frame update
    void Start()
    {
        if (pfsquare == null) pfsquare = null;
        transform.localScale = new Vector3(1000, 1000, 1); // float.MaxValue, float.MaxValue,1);
    }

    private void OnMouseDown()
    {
        Vector3 newpos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newpos.z = 0;
        newpos.x = Mathf.Round(newpos.x);
        newpos.y = Mathf.Round(newpos.y);
        GameObject newgo=Instantiate(pfsquare);
        newgo.transform.position = newpos;
    }

}
