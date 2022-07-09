using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    //�o�ߎ���
    private float elapsedTime = 0f;
    [SerializeField, Header("�ڕW����")] private float TargetTime = 1f;
    public bool DoNextTurn = false;
    //�������J�n����O��z���W��ۑ����Ă����ϐ�
    private int StartZPosition = 0;
    public bool FallFinished = false;

    // Update is called once per frame
    void Update()
    {
        if (GameDirector.Instance.gameState == GameDirector.GameState.active)
        {
            FallFinished = false;
        }

        //�����̉��o
        if (DoNextTurn)
        {
            elapsedTime += Time.deltaTime;

            this.transform.position += new Vector3(0, 0, -1.0f * Time.deltaTime);

            if (elapsedTime >= TargetTime)
            {
                elapsedTime = 0;
                this.transform.position = new Vector3(this.transform.position.x, 0, StartZPosition - 1);
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
            StartZPosition = (int)this.transform.position.z;
            DoNextTurn = true;
        }
    }
}
