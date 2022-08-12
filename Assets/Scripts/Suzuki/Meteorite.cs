using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool DoMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Search" || TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Area")
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

        if (DoMoving)
        {
            Vector3 targetPos = new Vector3(TargetXPosition, 0, TargetZPosition);
            elapsedTime += Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, elapsedTime / Duration);
            if (elapsedTime >= Duration)
            {
                elapsedTime = 0;
                this.transform.position = new Vector3(TargetXPosition, 0, TargetZPosition);
                MoveFinished = true;
                DoMoving = false;
            }
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
        if (MoveFinished == false)
        {
            StartXPosition = (int)this.transform.position.x;
            StartZPosition = (int)this.transform.position.z;
            TargetXPosition = PosX;
            TargetZPosition = PosZ;
            DoMoving = true;
        }
    }
}