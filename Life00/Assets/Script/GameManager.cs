using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// Этот класс будет отвечать за создание, уничтожение и жизнь всех клеточек

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pfsquare = null;
    [SerializeField] private float delaytime = 0;
    [SerializeField] private bool work = false;

    private Dictionary<Vector2Int, GameObject> sqlist;  // Список всех имеющихся квадратиков


    // Работа со списком клеток

    // Возвращает true если клетка с такими координатами пуста
    private bool checkFree(Vector2Int cp)
    {
        return !(sqlist.ContainsKey(cp));
    }

    private GameObject findCell(Vector2Int cp)
    {
        if (sqlist.ContainsKey(cp)) return sqlist[cp];
        return null;
    }

// Создаем клетку по определенным координатам (в дальнейшем - с проверкой целостности и добавлением в списки)
    public void addCell(Vector2Int CreatePos)
    {  
        if (checkFree(CreatePos))
        {
            GameObject newgo = Instantiate(pfsquare);
            ScrKillSquare kkk = newgo.GetComponent<ScrKillSquare>();
            kkk.BornMe(transform, CreatePos);
            kkk.FillNext(findCell(new Vector2Int(CreatePos.x - 1, CreatePos.y + 1)),
                         findCell(new Vector2Int(CreatePos.x, CreatePos.y + 1)),
                         findCell(new Vector2Int(CreatePos.x + 1, CreatePos.y + 1)),
                         findCell(new Vector2Int(CreatePos.x - 1, CreatePos.y)),
                         findCell(new Vector2Int(CreatePos.x + 1, CreatePos.y)),
                         findCell(new Vector2Int(CreatePos.x - 1, CreatePos.y - 1)),
                         findCell(new Vector2Int(CreatePos.x, CreatePos.y - 1)),
                         findCell(new Vector2Int(CreatePos.x + 1, CreatePos.y - 1)));
            //kkk.FillNext();
            sqlist.Add(CreatePos,newgo);
        }
    }

// Удаляем определенную клетку с доски (только из списка. Должно вызываться только при удалении клетки)
    public void delCell(Vector2Int key)
    {
        sqlist.Remove(key);
    }

    // Реакция на действия пользователя
    private void OnMouseDown()
    {
        
        Vector3 newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int crpos = new Vector2Int(Mathf.RoundToInt(newpos.x), Mathf.RoundToInt(newpos.y));
        if (checkFree(crpos))
        {
            addCell(crpos);
        } else
        {
            findCell(crpos).GetComponent<ScrKillSquare>().KillMe();
        }
    }


    void Start()
    {
        sqlist=new Dictionary<Vector2Int, GameObject>();  // Список всех имеющихся квадратиков
        if (pfsquare == null) pfsquare = null;
        if (delaytime == 0) delaytime = 10;
        work = false;
        StartCoroutine("OneStep");
    }

    private IEnumerator OneStep()
    {
    // Внутренние рабочие данные
        List<Vector2Int> newadd= new List<Vector2Int>();        // Те, кто будут добавлены
        List<Vector2Int> newnotadd = new List<Vector2Int>();    // Те, кто просмотрен, но добавлен не будет
        List<ScrKillSquare> newdel= new List<ScrKillSquare>();  // Те, кто будут удалены

        while (true)
        {
            yield return new WaitForSeconds(delaytime);

            Debug.Log("Прошел очередной период. У нас " + sqlist.Count+" клеток");
// Выводим клетки и их связи
            foreach (KeyValuePair<Vector2Int,GameObject> d in sqlist)
            {
                ScrKillSquare sck = d.Value.GetComponent<ScrKillSquare>();
                Debug.Log(sck.GetDebugInfo());
            }


            if (work)
            {
                // Выполняем работу для каждого квадратика среди имеющихся
                GameObject[] squares = GameObject.FindGameObjectsWithTag("Live");
                newadd.Clear();
                newnotadd.Clear();
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
                    foreach (Vector2Int curpos in checksq)
                    {
                        if ((!newadd.Contains(curpos))&&(!newnotadd.Contains(curpos)))
                        {
                            int nnum = 0;
                            if (findCell(new Vector2Int(curpos.x - 1, curpos.y + 1)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x, curpos.y + 1)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x + 1, curpos.y + 1)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x - 1, curpos.y)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x + 1, curpos.y)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x - 1, curpos.y - 1)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x, curpos.y - 1)) != null) nnum++;
                            if (findCell(new Vector2Int(curpos.x + 1, curpos.y - 1)) != null) nnum++;
                            if (nnum == 3)
                            {
                                newadd.Add(curpos);
                            }
                            else if (nnum > 3)
                            {
                                newnotadd.Add(curpos);
                            }
                        }
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
