using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// ���� ����� ����� �������� �� ��������, ����������� � ����� ���� ��������

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pfsquare = null;
    [SerializeField] private float delaytime = 0;
    [SerializeField] private bool work = false;

    private Dictionary<Vector2Int, GameObject> sqlist;  // ������ ���� ��������� �����������


    // ������ �� ������� ������

    // ���������� true ���� ������ � ������ ������������ �����
    private bool checkFree(Vector2Int cp)
    {
        return !(sqlist.ContainsKey(cp));
    }

    private GameObject findCell(Vector2Int cp)
    {
        if (sqlist.ContainsKey(cp)) return sqlist[cp];
        return null;
    }

// ������� ������ �� ������������ ����������� (� ���������� - � ��������� ����������� � ����������� � ������)
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

// ������� ������������ ������ � ����� (������ �� ������. ������ ���������� ������ ��� �������� ������)
    public void delCell(Vector2Int key)
    {
        sqlist.Remove(key);
    }

    // ������� �� �������� ������������
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
        sqlist=new Dictionary<Vector2Int, GameObject>();  // ������ ���� ��������� �����������
        if (pfsquare == null) pfsquare = null;
        if (delaytime == 0) delaytime = 10;
        work = false;
        StartCoroutine("OneStep");
    }

    private IEnumerator OneStep()
    {
    // ���������� ������� ������
        List<Vector2Int> newadd= new List<Vector2Int>();        // ��, ��� ����� ���������
        List<Vector2Int> newnotadd = new List<Vector2Int>();    // ��, ��� ����������, �� �������� �� �����
        List<ScrKillSquare> newdel= new List<ScrKillSquare>();  // ��, ��� ����� �������

        while (true)
        {
            yield return new WaitForSeconds(delaytime);

            Debug.Log("������ ��������� ������. � ��� " + sqlist.Count+" ������");
// ������� ������ � �� �����
            foreach (KeyValuePair<Vector2Int,GameObject> d in sqlist)
            {
                ScrKillSquare sck = d.Value.GetComponent<ScrKillSquare>();
                Debug.Log(sck.GetDebugInfo());
            }


            if (work)
            {
                // ��������� ������ ��� ������� ���������� ����� ���������
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
                // ��������� ��������� (���� �� �������)
                foreach (Vector2Int curpos in newadd)
                {
                    addCell(curpos);
                }
                // ������� ���������� (������� �� �������)
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
