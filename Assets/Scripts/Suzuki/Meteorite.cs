using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Meteorite : MonoBehaviour
{
    //�o�ߎ���
    private float elapsedTime = 0f;
    [SerializeField, Header("�ڕW����")] private float TargetTime = 0.5f;
    [SerializeField] private float Duration = 2f;
    public bool DoNextTurn = false;
    //�ړ����J�n����O�̍��W��ۑ����Ă����ϐ�
    private int StartXPosition = 0;
    private int StartZPosition = 0;
    private float TargetXPosition = 0;
    private float TargetZPosition = 0;
    //���̃^�[���������s���Ă��邩�ǂ���
    public bool FallFinished = false;
    public bool MoveFinished = false;
    [SerializeField] private Animator meteorAnimator = null;

    // Update is called once per frame
    void Update()
    {
        if (TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Search" || TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Area" || TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Watching")
        {
            meteorAnimator.SetBool("IsArea", true);
        }
        else
        {
            meteorAnimator.SetBool("IsArea",false);
        }

        if (GameDirector.Instance.gameState == GameDirector.GameState.active)
        {
            FallFinished = false;
            MoveFinished = false;
        }

        //�����̉��o
        if (DoNextTurn)
        {
            elapsedTime += Time.deltaTime;
            this.transform.position += new Vector3(0, 0, -1.0f * Time.deltaTime * 2);
            if (elapsedTime >= TargetTime)
            {
                elapsedTime = 0;
                this.transform.position = new Vector3(StartXPosition, 0, StartZPosition - 1);
                FallFinished = true;
                DoNextTurn = false;
            }
        }
    }

    /// <summary>
    /// �������J�n����
    /// </summary>
    public void StartFall()
    {
        if (FallFinished == false)
        {
            StartXPosition = (int)this.transform.position.x;
            StartZPosition = (int)this.transform.position.z;
            DoNextTurn = true;
            //�}�b�v�̌������ꏊ�̋L�^���폜��
            Map.Instance.map[StartZPosition*-1, StartXPosition] = Map.Instance.empty;
            //�}�b�v�̈ړ���ɐV���ɋL�^����������
            Map.Instance.map[StartZPosition*-1+1, StartXPosition] = Map.Instance.meteor;
        }
    }

    public void MoveToTargetPoint(int PosX, int PosZ)
    {
        Vector3 targetPos = new Vector3(PosX, 0, PosZ);
        this.transform.DOMove(targetPos, Duration, true).OnComplete(() => FixPositionAfterMoving(PosX, PosZ));
    }

    public void FixPositionAfterMoving(int PosX, int PosZ)
    {
        this.transform.position = new Vector3(PosX, 0, PosZ);
        MoveFinished = true;
    }
}