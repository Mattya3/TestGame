# C# コーディング規約 (Unity向け)

Unityプロジェクトにおいて，チーム全員が読みやすく，バグの少ないコードを書くための基本的なルールです．
> このルールは生成AIによって作成されました．そのため，開発の状況に応じて変更される可能性は十分にあります．

## 1. 命名規則 (Naming Conventions)
誰が書いても同じような名前になるように，名前の付け方を統一します．

- **クラス名・構造体名・enum名**: `PascalCase`（単語の先頭を大文字にする）
  - 例：`PlayerController`, `GameManager`
- **メソッド名（関数）**: `PascalCase`
  - 例：`Move()`, `TakeDamage()`, `CalculateScore()`
- **ローカル変数・引数**: `camelCase`（最初の単語は小文字，以降の単語の先頭を大文字）
  - 例：`speed`, `maxScore`, `targetPosition`
- **フィールド（メンバ変数）**: 基本的に `_camelCase`（アンダースコア + camelCase）
  - 例：`_currentHp`, `_rb`
  - 理由：エディタの補完機能で探しやすいことと，ローカル変数と区別するためです．
- **定数 (const / readonly)**: `PascalCase` または `UPPER_SNAKE_CASE`（すべて大文字でアンダースコア）
  - 例：`DefaultSpeed`, `MAX_ITEM_COUNT`

## 2. 変数の公開設定（Inspectorへの表示）
Unity開発において最も重要なルールの1つです．**安易に `public` フィールドを使わない**ようにしましょう．

- **Inspectorで設定したい変数（Unityエディタから数値をいじりたいもの）**:
  - ❌ 悪い例: `public float speed;`
  - ⭕ 良い例: `[SerializeField] private float _speed;`
  - 理由：`public` にすると，他のスクリプトから意図せず値を書き換えられてしまうバグの温床になります．
- **他のクラスから読み取りたいが，書き換えられたくない変数**:
  - ⭕ プロパティを使用する: `public int Score { get; private set; }`

## 3. Unity固有のパフォーマンス注意点
毎フレーム実行される `Update()` メソッドの中での重い処理は避けてください．

- **`GetComponent` や `GameObject.Find` は `Update()` に書かない**
  - これらは処理が重いため，必ず `Start()` や `Awake()` で一度だけ取得し，変数に保存（キャッシュ）して使い回してください．
  ```csharp
  // 悪い例
  void Update() {
      GetComponent<Rigidbody>().velocity = Vector3.forward;
  }

  // 良い例
  private Rigidbody _rb;
  void Start() {
      _rb = GetComponent<Rigidbody>();
  }
  void Update() {
      _rb.velocity = Vector3.forward;
  }
  ```
- **タグの比較には `CompareTag` を使う**
  - `if (gameObject.tag == "Player")` ではなく `if (gameObject.CompareTag("Player"))` を使います（メモリ確保が発生しないため高速です）．
- **`new` キーワードによるメモリ確保を `Update()` で避ける**
  - 毎フレーム新しいリストなどを作成すると，ガベージコレクション（GC）が発生しゲームがカクつく原因になります．

## 4. `Awake()` と `Start()` の使い分け
初期化処理は以下のように使い分けます．

- **`Awake()`**: 自分自身のコンポーネント（`GetComponent`など）を取得・設定する時に使います．オブジェクトが非アクティブでもインスタンス化されていれば呼ばれます．
- **`Start()`**: 他のオブジェクトやManagerクラスなどを参照する時に使います（他オブジェクトのAwakeが終わっていることが保証されるため安全です）．

## 5. 必須コンポーネントの付け忘れ防止
あるスクリプトが特定のコンポーネント（例：`Rigidbody` や `Animator`）に依存している場合は，`[RequireComponent]` 属性をクラスの上につけます．
```csharp
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // ...
}
```

## 6. コメントとドキュメント
- クラスやpublicメソッドの役割は，XMLコメント（`/// <summary>`）を使って説明します．これにより，Visual Studio等の入力補完時に説明が表示されます．
- クラス内の複雑なロジックには，通常の `//` コメントで「なぜそう書いたか（Why）」を残すようにしてください．「何をしているか」はコードを読めば分かるように，わかりやすい変数名やメソッド名をつけるのが理想です．

## 7. コードの自動フォーマット (GitHub Actions)
本プロジェクトでは，コードのフォーマット（インデントや改行幅などの整形）を **GitHub Actions** によって自動化しています．

- **仕組み**: GitHub上でPR（Pull Request）を作成したタイミング（およびPRブランチへの追記Push時）に，自動でフォーマットツール（CSharpier）が作動し，ソースコードを綺麗に整えてくれます．
- **注意点**: PRを出した直後に，Botが自動であなたのPRブランチに「整形コミット」を追加することがあります．PRを出した後にさらに手元で修正を続ける際は，**必ず最初に `git pull` を行い，Botが整形した状態を手元に取り込んでから**作業を行うように心がけてください．（手元が古いまま作業を進めるとコンフリクトの原因になります）
