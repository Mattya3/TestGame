# C# コーディング規約 (Unity向け)

Unity プロジェクトにおいて，チーム全員が読みやすく，変更しやすく，壊しにくいコードを書くための規約です．
このドキュメントでは，単なる見た目の統一ではなく，**可読性**，**保守性**，**Unity で起きやすい事故の予防**を重視します．

## 1. 基本方針

- コードは「動けばよい」ではなく，**他の人が 3 か月後に読んでも意図が追えること**を目標にします．
- `MonoBehaviour` は Unity との接着層です．ゲーム固有の判定や計算ロジックまで 1 クラスに詰め込まないでください．
- 1 クラス 1 責務を意識し，役割が複数ある場合は分割を検討します．
- 新規コードでは，**短いメソッド，小さい責務，明確な名前**を優先します．
- 既存コードの癖に無理に合わせるより，**新しく増やすコードは標準的な C# / Unity の流儀**に寄せます．

## 2. 命名規則

### 2.1 型・メンバー名

- **クラス名・構造体名・enum 名・プロパティ名・メソッド名**: `PascalCase`
  - 例: `PlayerController`, `MoveToGoal()`, `CurrentHealth`
- **ローカル変数・引数**: `camelCase`
  - 例: `moveSpeed`, `targetPlayer`, `isGrounded`
- **private / protected フィールド**: `_camelCase`
  - 例: `_rigidbody`, `_moveSpeed`, `_cameraTarget`
- **private / protected メソッド**: `_PascalCase`
  - 例: `_CalculateCenter()`, `_MoveByInput()`
- **定数 (`const`) / 変更しない共有値 (`static readonly`)**: `PascalCase`
  - 例: `DefaultRespawnTime`, `MaxPlayerCount`
- **インターフェース**: `I` + `PascalCase`
  - 例: `ICameraTarget`

### 2.2 名前の付け方

- `bool` は **意味が文章になる名前**にします．
  - 例: `isGrounded`, `hasAuthority`, `canRetry`, `shouldLoop`
- コレクションは複数形にします．
  - 例: `players`, `spawnPoints`
- 省略形は一般的なものだけにします．`hp`, `ui`, `id` は可，`mgr`, `ctrl`, `tmpVal` は避けます．
- ファイル名は原則として **先頭の主要クラス名と一致**させます．

### 2.3 private メソッド名

- private / protected メソッドも `PascalCase` を使います．
  - 推奨: `CalculateCenter()`
  - 非推奨: `_CalculateCenter()`
- 既存コードに `_MethodName()` 形式が残っていても，**新規追加では増やさない**でください．
- 既存クラスを大きく触るタイミングで，周辺に影響が大きくない範囲で段階的に統一します．

## 3. アクセス修飾と公開範囲

- フィールドは **まず `private`** で定義します．
- Inspector に見せたいだけの値は `public` にせず，`[SerializeField] private` を使います．
  ```csharp
  [SerializeField] private float _moveSpeed = 5f;
  ```
- 外部から参照させたいが自由に書き換えられたくない値は，**読み取り専用プロパティ**で公開します．
  ```csharp
  public int Score { get; private set; }
  ```
- 外部からの変更が必要な場合も，直接フィールドを書き換えさせず，**意図が分かるメソッド**を用意します．
  - 例: `TakeDamage(int amount)`, `Initialize(StageData stageData)`
- `public` は「他クラスから使う API」です．
  - Inspector 表示の都合だけで `public` にしないでください．

## 4. クラス設計

- `MonoBehaviour` 1 つに複数の責務を持たせないでください．
  - 例: `PlayerController` が入力処理，移動，SE 再生，UI 更新，スコア管理まですべて持つのは避けます．
- 再利用したいロジック，テストしやすくしたいロジックは，`MonoBehaviour` から切り出した通常の C# クラスに寄せます．
- `Manager` という名前は便利ですが曖昧です．
  - 何を管理するのか 1 文で説明できないクラスは，責務過多を疑ってください．
- Singleton は乱用しません．
  - シーンをまたぐ唯一インスタンスが本当に必要な場合だけに限定します．
  - 使用する場合でも，初期化順序や破棄タイミングを必ず説明できる状態にしてください．

## 5. メソッドの書き方

- メソッドは **短く，1 つのことだけ**を行うようにします．
- ネストが深くなる前に，早期 `return` で条件分岐を整理します．
- 同じ意味の処理が 2 回以上出たら，共通化を検討します．
- 引数が多くなりすぎたら，責務の分割やデータ構造化を検討します．
- メソッドの並び順は，以下を基本とします．
  1. フィールド
  2. プロパティ
  3. Unity のライフサイクルメソッド
  4. public API
  5. private / protected ヘルパー

## 6. Unity ライフサイクルの使い分け

- `Awake()`
  - 自分自身の初期化に使います．
  - `GetComponent` による依存取得，フィールドの初期化，内部状態のセットアップを行います．
- `OnEnable()`
  - イベント購読，入力受付開始など，**有効化中だけ成立してほしい処理**を置きます．
- `Start()`
  - 他オブジェクトの初期化完了を前提にした処理を置きます．
- `Update()`
  - 入力や軽い状態更新など，毎フレーム必要な処理だけにします．
- `FixedUpdate()`
  - 物理演算に関わる処理を置きます．`Rigidbody` の移動や力の付与はこちらです．
- `LateUpdate()`
  - カメラ追従など，「他の更新が終わったあと」に実行したい処理に使います．
- `OnDisable()`
  - `OnEnable()` で購読したイベントは必ずここで解除します．
- `OnDestroy()`
  - `OnDisable()` では足りない明示的な解放処理だけを置きます．

### ライフサイクルで避けること

- 初期化順序を `Script Execution Order` に依存させないでください．
- 他クラスの `Awake()` が終わっている前提の処理を，自分の `Awake()` に書かないでください．
- `Update()` に重い検索，メモリ確保，LINQ を書かないでください．

## 7. Inspector とシリアライズ

- Inspector に出す値は，**外部から調整したい理由があるものだけ**に絞ります．
- 調整値には必要に応じて属性を付けます．
  - `Header`, `Tooltip`, `Range`, `Min`, `TextArea`
- 参照が必須なら，`Awake()` で null を検知して `Debug.LogError(..., this)` を出し，必要なら `enabled = false` にします．
- コンポーネント依存が明確な場合は `[RequireComponent]` を付けます．
  ```csharp
  [RequireComponent(typeof(Rigidbody2D))]
  public class PlayerMovement : MonoBehaviour
  {
      [SerializeField] private float _moveSpeed = 5f;
  }
  ```
- `OnValidate()` は Editor 上の値チェックや補正に使って構いませんが，**再生中ロジックの本体**は書かないでください．

## 8. 参照取得のルール

- `GetComponent`, `GetComponentInChildren`, `TryGetComponent` は，原則として `Awake()` / `Start()` で取得し，キャッシュして使います．
- `Update()` / `FixedUpdate()` 内で毎回 `GetComponent` しないでください．
- `GameObject.Find`, `FindObjectOfType`, `FindAnyObjectByType` のようなシーン全体検索は，通常コードでは極力避けます．
  - 使う場合は，起動時の一度きりのブートストラップ処理など，理由が明確な場面に限ります．
- Inspector 参照で解決できる依存は，検索よりも **SerializeField による明示的な参照**を優先します．

## 9. パフォーマンスと GC

- 毎フレーム実行される箇所で，**不要なメモリ確保**を避けます．
  - `new List<>()`
  - 文字列連結
  - 毎フレームの LINQ
- タグ比較は `CompareTag()` を使います．
- 頻繁に呼ばれる処理では，`LINQ` よりも通常の `for` / `foreach` を優先します．
- コルーチンは便利ですが，状態が見えにくくなります．
  - 一時的な演出や待機処理には使って構いませんが，複雑なゲーム進行の中核をコルーチンの入れ子だけで組まないでください．

## 10. null・エラー・ログ

- 想定外の null は黙って無視せず，**原因が追える形で失敗**させます．
- `Debug.LogError` / `Debug.LogWarning` には，可能なら第 2 引数に `this` などのコンテキストを渡します．
- 一時調査用の `Debug.Log` は，マージ前に削除します．
- 例外を握りつぶさないでください．
  - `try-catch` を使うなら，「何を復旧するのか」「何を記録するのか」を説明できるようにします．

## 11. コメントとドキュメント

- コメントは「何をしているか」ではなく，**なぜそうしているか**を書くことを優先します．
- public クラスや，他クラスから使われる public メソッドには，必要に応じて XML コメントを付けます．
- 長いコメントが必要になる場合は，コード構造や命名で解決できないかを先に考えます．
- コメントがコードとずれたら，コードではなくコメント側を放置しないで更新します．

## 12. `using` と依存の整理

- `using` は未使用のものを残さないでください．
- `using static` は可読性を下げやすいため，常用しません．
  - 定数群などで使う場合でも，そのクラスを知らない人が読んで意味が追えるかを基準に判断します．
- 依存先 namespace が増えすぎる場合は，クラス責務の分割を検討します．

## 13. フォーマット

- インデントや改行位置は，原則として自動フォーマッタに従います．
- 手動で整形ルールを競わず，差分が読みやすいことを優先します．
- GitHub Actions による自動整形が走る場合があります．PR 作成後に Bot の整形コミットが入ったら，追加修正前に `git pull` してから作業してください．

## 14. レビュー時の確認ポイント

- `public` が必要最小限になっているか
- `MonoBehaviour` に責務が詰め込まれすぎていないか
- `Update()` / `FixedUpdate()` に重い処理や不要な確保がないか
- null 時の失敗が分かりやすいか
- 名前を読むだけで役割が分かるか
- イベント購読が `OnEnable()` / `OnDisable()` で対になっているか
