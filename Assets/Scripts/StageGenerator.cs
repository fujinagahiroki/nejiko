using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGenerator : MonoBehaviour {

    const int StageTipSize = 30;

    int currentTipIndex;

    //ターゲットキャラクターの指定
    public Transform charcter;
    //ステージチッププレファブ配列
    public GameObject[] stageTips;
    //自動生成開始インデックス
    public int startTipIndex;
    //生成先読み個数
    public int preInstatiate;
    //生成済みステージチップ保持リスト
    public List<GameObject> generateStageList = new List<GameObject>();

	void Start () {
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstatiate);
	}
	
	// Update is called once per frame
	void Update () {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(charcter.position.z / StageTipSize);

        //次のステージチップに入ったらステージの更新処理を行う
        if(charaPositionIndex+preInstatiate>currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstatiate);
        }
	}

    //指定のIndexまでのステージチップを生成して、管理下に置く
    void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex) return;

        //指定のステージチップまでを作成
        for(int i = currentTipIndex+1;i <= toTipIndex;i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加し
            generateStageList.Add(stageObject);
        }
        //ステージ保持上限内になるまで古いステージを削除
        while (generateStageList.Count > preInstatiate + 2) DestroyOldestStage();

        currentTipIndex = toTipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(0, 0, tipIndex * StageTipSize),
            Quaternion.identity);

        return stageObject;
    }

    //一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generateStageList[0];
        generateStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
