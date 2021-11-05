using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// Этот класс будет отвечать за создание, уничтожение и жизнь всех клеточек

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pfsquare = null;
    [SerializeField] private float delaytime = 0;
    [SerializeField] private bool work = false;

    private List<Vector2Int> newadd;  // Те, кто будут добавлены
    private List<ScrKillSquare> newdel;  // Те, кто будут удалены

// Создаем клетку по определенным координатам (в дальнейшем - с проверкой целостности и добавлением в списки)
    public void addCell(Vector2Int CreatePos)
    {  
        GameObject newgo = Instantiate(pfsquare);
        ScrKillSquare kkk = newgo.GetComponent<ScrKillSquare>();
        kkk.BornMe(transform,CreatePos);
    }

// Удаляем определенную клетку с доски (с поддержкой целостности)
    public void delCell(GameObject cell)
    {
        cell.GetComponent<ScrKillSquare>().KillMe();
    }

    // Реакция на действия пользователя
    private void OnMouseDown()
    {
        
        Vector3 newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int crpos = new Vector2Int(Mathf.RoundToInt(newpos.x), Mathf.RoundToInt(newpos.y));
        addCell(crpos);
    }


    void Start()
    {
        newadd = new List<Vector2Int>();
        newdel = new List<ScrKillSquare>();
        if (pfsquare == null) pfsquare = null;
        if (delaytime == 0) delaytime = 10;
        work = false;
        StartCoroutine("OneStep");
    }

    private IEnumerator OneStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(delaytime);
            if (work)
            {
                // Выполняем работу для каждого квадратика среди имеющихся
                GameObject[] squares = GameObject.FindGameObjectsWithTag("Live");
                newadd.Clear();
                newdel.Clear();
                foreach (GameObject square in squares)
                {
                    ScrKillSquare mysq = square.GetComponent<ScrKillSquare>();
                    List<Vector2Int> checksq = mysq.GetNextCellsCount();
                    int NeightNum = 8 - checksq.Count;
                    if ((NeightNum < 2) || (NeightNum > 3))
                    {
                        newdel.Add(mysq);
                    }
                    foreach (Vector2Int chk in checksq)
                    {
                        if (!newadd.Contains(chk)) newadd.Add(chk);
                    }
                }
                // Добавляем новеньких (если им повезло)
                foreach (Vector2Int curpos in newadd)
                {
                    addCell(curpos);
                }
                // Убиваем стареньких (которым не повезло)
                foreach (ScrKillSquare pos in newdel)
                {
                    pos.KillMe();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
