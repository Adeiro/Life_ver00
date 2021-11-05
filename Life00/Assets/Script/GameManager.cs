using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// ���� ����� ����� �������� �� ��������, ����������� � ����� ���� ��������

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pfsquare = null;
    [SerializeField] private float delaytime = 0;
    [SerializeField] private bool work = false;

    private List<Vector2Int> newadd;  // ��, ��� ����� ���������
    private List<ScrKillSquare> newdel;  // ��, ��� ����� �������

// ������� ������ �� ������������ ����������� (� ���������� - � ��������� ����������� � ����������� � ������)
    public void addCell(Vector2Int CreatePos)
    {  
        GameObject newgo = Instantiate(pfsquare);
        ScrKillSquare kkk = newgo.GetComponent<ScrKillSquare>();
        kkk.BornMe(transform,CreatePos);
    }

// ������� ������������ ������ � ����� (� ���������� �����������)
    public void delCell(GameObject cell)
    {
        cell.GetComponent<ScrKillSquare>().KillMe();
    }

    // ������� �� �������� ������������
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
                // ��������� ������ ��� ������� ���������� ����� ���������
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
