using CameraRig;
using Item.Tank;
using Widget.ArrowPopUp;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Widget.Minimap;

public class GameManager : MonoBehaviour
{
    public PointList spawnPointList;                // ̹�˳�����
    //public AllTanksManager allTanks;                // ����̹�˹�����
    //public AllTeamsManager allTeams;                // �����Ŷӹ�����

    public int numRoundsToWin = 5;                  // Ӯ����Ϸ��ҪӮ�Ļغ���
    public float startDelay = 3f;                   // ��ʼ��ʱʱ��
    public float endDelay = 3f;                     // ������ʱʱ��
    public Text messageText;                        // UI�ı�����һ�ʤ�ȣ�
    public MultiplayerCameraManager cameraControl;  // ����������
    public MinimapManager minimapManager;           // С��ͼ������
    public AllArrowPopUpManager spawnAllPopUpArrow;   // ������ʾ��Ҽ�ͷ
    public TankFireButton tankFireButton;           // ̹����ͨ������ť
    public AllPlayerManager allPlayerManager;       // �������

    private WaitForSeconds startWait;               // ��ʼ�غ���ʱ
    private WaitForSeconds endWait;                 // �����غ���ʱ

    private void Awake()
    {
        //allTanks.SetupInstance();
        //allTeams.SetupInstance();
        allPlayerManager.SetupInstance();
        GameRecord.Instance = new GameRecord(numRoundsToWin); // ����һ����Ϸ��¼ʵ��
        startWait = new WaitForSeconds(startDelay); // ��Ϸ�غϿ�ʼ��ʱ
        endWait = new WaitForSeconds(endDelay);     // ��Ϸ�غϽ�����ʱ
    }

    /// <summary>
    /// ��ʼ����Ϸ��¼ʵ������������̹�ˡ��������Ŀ�ꡢС��ͼ��ʼ������ʼ��Ϸѭ��
    /// </summary>
    private void Start()
    {
        SetupGame();                                // ������Ϸ

        GameRecord.Instance.StartGame();            // ��ʼ��Ϸѭ��������ʤ�ߣ����»غϣ�������Ϸ�ȣ�
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// ��������̹�ˣ�������Һ�AI�������þ�ͷ����׷��Ŀ�ꡢС��ͼ��ʼ��
    /// </summary>
    private void SetupGame()
    {
        GameObject tanks = new GameObject("Tanks");
        //for (int i = 0; i < AllTanksManager.Instance.Count; i++)
        //    AllTanksManager.Instance[i].InitTank(Instantiate(AllTanksManager.Instance[i].tankPerfab, tanks.transform));

        allPlayerManager.CreatePlayerGameObjects();
        for (int i = 0; i < allPlayerManager.Count; i++)
            allPlayerManager[i].transform.position = spawnPointList[i].position;

        //// Ϊ��һ��̹�����ӵ�UI����
        //tankFireButton.tankShooting = AllTanksManager.Instance[0].Instance.GetComponent<TankShooting>();

        //cameraControl.targets = AllTanksManager.Instance.GetTanksTransform();
        //cameraControl.SetStartPositionAndSize();

        //minimapManager.SetupPlayerIconDic();
        //minimapManager.SetTarget(AllTanksManager.Instance[0].Instance.transform);
    }

    /// <summary>
    /// ��������̹�˳�����
    /// </summary>
    private void ResetAllTanksSpawnPoint()
    {
        //spawnPointList.EnableAllPoints();                     // ��ʼ��������
        //for (int i = 0; i < AllTanksManager.Instance.Count; i++)
        //{
        //    //��ȡ��Ч��������㣬��ÿ��̹��λ�ò�һ��
        //    Point spawnPoint = spawnPointList.GetRandomPoint(false, true);
        //    if (spawnPoint == null)
        //        continue;
        //    AllTanksManager.Instance[i].Reset(spawnPoint);
        //}
    }

    /// <summary>
    /// ����������ҿ���Ȩ
    /// </summary>
    /// <param name="enable">����״̬</param>
    private void SetTanksControlEnable(bool enable)
    {
        //for (int i = 0; i < AllTanksManager.Instance.Count; i++)
        //    AllTanksManager.Instance[i].SetControlEnable(enable);
    }

    /// <summary>
    /// ��Ϸ��ѭ��Э��
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());           //�غϿ�ʼ����һ����ʱ

        yield return StartCoroutine(RoundPlaying());            //�غ���

        yield return StartCoroutine(RoundEnding());             //�غϽ���

        // �����������Ϸ�����¼��س��������������һ�غ�
        if (GameRecord.Instance.IsEndOfTheGame())
            SceneManager.LoadScene(0);
        else
            StartCoroutine(GameLoop());
    }

    /// <summary>
    /// �غϿ�ʼ
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundStarting()
    {
        SetTanksControlEnable(false);                   // ����̹���ǵĿ���Ȩ
        ResetAllTanksSpawnPoint();                      // ��������̹��λ��
        spawnAllPopUpArrow.Spawn();      // ��ʾ��Ҽ�ͷ
        GameRecord.Instance.StartRound();

        messageText.text = "ROUND " + GameRecord.Instance.CurrentRound;

        yield return startWait;                         // ��ʱһ��ʱ���ٿ�ʼ
    }

    /// <summary>
    /// �غ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundPlaying()
    {
        SetTanksControlEnable(true);                    // ������ҿ���Ȩ

        messageText.text = string.Empty;                // �����ʾ��Ϣ

        while (!GameRecord.Instance.IsEndOfTheRound())           // �غ�û�����ͼ���
            yield return null;
    }

    /// <summary>
    /// �غϽ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundEnding()
    {
        SetTanksControlEnable(false);                               // ������ҿ���Ȩ

        GameRecord.Instance.UpdateWonData();                        // ���»�ʤ����

        messageText.text = GameRecord.Instance.GetEndingMessage();  // ��ȡ������Ϣ����ʾ֮

        yield return endWait;
    }

    /// <summary>
    /// �ص����˵�
    /// </summary>
    public void BackToMainScene()
    {
        AllSceneManager.LoadScene(GameScene.MainMenuScene);
    }
}