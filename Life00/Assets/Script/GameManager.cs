using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pfsquare;
    [SerializeField] private float delaytime = 0;

    void Start()
    {
        if (delaytime == 0) delaytime = 10;
        StartCoroutine("OneStep");
    }

    private IEnumerator OneStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(delaytime);
            // Выполняем работу для каждого квадратика среди имеющихся
            GameObject[] squares = GameObject.FindGameObjectsWithTag("Live");
            foreach (GameObject square in squares)
            {
                WorkProc(square);
            }
        }
    }

    private void CheckAndCreate(Vector2 cpos)
    {
        // Проверяем наличие квадратиков по новым позициям
        Vector3 stp = new Vector3(cpos.x, cpos.y, 1);
        Vector3 etp = new Vector3(0,0,-1);
        if (!Physics.Raycast(stp, etp, 2,0xFFFF)) {
            Vector3 curpos = new Vector3(cpos.x, cpos.y, 0);
            GameObject newgo = Instantiate(pfsquare);
            newgo.transform.position = curpos;
        }
    }

    private void WorkProc(GameObject square)
    {
        Vector2 curpos = square.transform.position;
        CheckAndCreate(new Vector2(curpos.x - 1, curpos.y    ));
        CheckAndCreate(new Vector2(curpos.x + 1, curpos.y    ));
        CheckAndCreate(new Vector2(curpos.x    , curpos.y - 1));
        CheckAndCreate(new Vector2(curpos.x    , curpos.y + 1));
        CheckAndCreate(new Vector2(curpos.x - 1, curpos.y - 1));
        CheckAndCreate(new Vector2(curpos.x + 1, curpos.y + 1));
        CheckAndCreate(new Vector2(curpos.x + 1, curpos.y - 1));
        CheckAndCreate(new Vector2(curpos.x - 1, curpos.y + 1));
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
