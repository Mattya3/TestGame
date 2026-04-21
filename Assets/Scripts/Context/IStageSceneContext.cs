
public interface IStageSceneContext
{
    public bool AfterRestart { get; }

    public void OnStageRestarted();

    /*
        必要に応じて、ステージシーンをまたいで共有されるべきデータや機能をここに追加
        ・現在のステージのIDや名前
        ・ステージがすでにクリアされているかどうか
        ・重要アイテム（スターメダル的な）の回収済みフラグ
        ・連続で失敗した回数（救済措置用）
        など
    */
}
