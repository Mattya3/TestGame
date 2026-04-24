# Unity における開発ルール・作法

Unity では，C# の書き方だけでなく，**Scene / Prefab / アセット参照 / Git 運用**が品質に直結します．
このドキュメントでは，チーム開発で事故が起きやすい Unity 特有のルールをまとめます．

## 1. Scene の扱い

- **複数人で同じ Scene ファイル (`.unity`) を同時編集しない**ことを原則とします．
- 本番 Scene を編集する前には，チャット等で担当を宣言してください．
- 動作確認は，なるべく各自の検証用 Scene か Prefab 単位で行います．
- Scene には「その場限りの調整」を直接積み上げず，繰り返し使うものは Prefab 化します．

### Scene 運用の意図

- Scene はテキストマージが難しく，競合すると復旧コストが高いです．
- 「Scene にしか存在しない設定」が増えるほど，不具合の原因が追いにくくなります．

## 2. Prefab の運用

- 再利用するオブジェクトは，必ず Prefab または Prefab Variant にします．
- 調整値や構成変更は，可能な限り **Scene 上の個体ではなく Prefab 本体**に対して行います．
- 同じ見た目や構成を持つオブジェクトが複数あるのに，Scene 上で個別調整だけで増殖させるのは避けます．
- Variant で吸収できる差分は，Prefab を複製せず Variant を使います．

## 3. シリアライズ参照のルール

- 参照は，**壊れにくく，見て追える形**で持ちます．
- 別 Scene のオブジェクトへの参照を前提にしないでください．
- Scene 内参照は，その Scene で完結するものに限定します．
- 共通設定値や再利用データは，必要に応じて `ScriptableObject` や定義クラスへの切り出しを検討します．
- ファイル移動やリネームは，Finder / Explorer ではなく **Unity Editor 上で実施**します．
  - `.meta` の対応が崩れると参照切れの原因になります．

## 4. `.meta` ファイルの扱い

- アセットを追加・移動・削除・リネームしたときは，**対応する `.meta` ファイルを必ず一緒にコミット**します．
- `.meta` が欠けると，GUID が変わり，Prefab / Scene / Material / Script 参照が壊れます．
- Git GUI を使う場合でも，`.meta` のチェック漏れがないかを確認してください．

## 5. フォルダ構成

- `Assets` 直下にファイルを無秩序に増やさず，用途ごとに整理します．
- 基本構成の目安:
  - `Assets/Scripts/` : C# スクリプト
  - `Assets/Scenes/` : Scene
  - `Assets/Prefabs/` : Prefab
  - `Assets/Sprites/` または `Assets/Textures/` : 画像・テクスチャ
  - `Assets/Audio/` : 音声
  - `Assets/Materials/`, `Assets/Models/` : 3D 関連
  - `Assets/Settings/` : 共通設定，`ScriptableObject`，SceneTemplate など
- スクリプトは機能単位でサブフォルダを切ります．
  - 例: `Characters`, `Camera`, `Game`, `Audio`
- 「とりあえず `Misc` に入れる」は避け，役割が分かる場所に置きます．

## 6. アセット命名

- 何のためのアセットか，一目で分かる名前にします．
- `New Prefab`, `Sprite (1)`, `Material Copy` のような仮名のままコミットしないでください．
- Prefab / Scene / Material / AnimationClip は，用途や対象を名前に含めます．
  - 例: `PlayerBody`, `GoalTrigger`, `MainMenuScene`, `Stage01_BGM`
- 個人作業用の一時 Scene や検証用アセットは，チームで分かる接頭辞やルールを持たせます．
  - 例: `Test_Takumi`, `Sandbox_PlayerJump`

## 7. `MonoBehaviour` の責務

- `MonoBehaviour` は「Scene / GameObject にぶら下がる必要があるもの」だけを持たせます．
- 純粋な計算やルール判定まで `MonoBehaviour` に閉じ込めず，通常の C# クラスに切り出せるかを検討します．
- 1 つのコンポーネントで，入力，物理，UI，SE，再生管理まで抱え込まないでください．

## 8. 初期化順序と依存関係

- `Script Execution Order` に頼る設計は原則避けます．
- 初期化順序が必要なら，明示的な `Initialize()` 呼び出しや依存注入で表現します．
- イベント購読は `OnEnable()`，解除は `OnDisable()` を基本にします．
- `DontDestroyOnLoad` を使うオブジェクトは最小限にし，シーン遷移後の重複生成に注意します．

## 9. 物理・更新処理

- `Rigidbody` を動かす処理は `FixedUpdate()` に寄せます．
- カメラ追従や描画結果に依存する追従は `LateUpdate()` を使います．
- 毎フレームの検索処理やアロケーションは避けます．
- `Time.deltaTime` を使うべき処理と，物理ステップで扱うべき処理を混同しないでください．

## 10. ログとデバッグ補助

- 調査用の `Debug.Log` は，作業完了後に消します．
- 設定不備や欠落参照は，`Debug.LogError(..., this)` のようにコンテキスト付きで出します．
- `OnDrawGizmos` / `OnDrawGizmosSelected` は，**Editor での理解を助ける用途**に限定し，ゲームロジックを持ち込まないでください．

## 11. Git 管理しないもの

- `Library`, `Logs`, `Temp`, `Obj`, `Build` などの自動生成物はコミットしません．
- 容量の大きいファイルは，追加前に相談してください．
  - 目安として 100MB を超えるものは要相談です．
- Unity の自動生成差分が大量に出た場合は，何が原因で変更されたかを確認してからコミットします．

## 12. PR 前チェック

- Scene / Prefab / `.meta` の組が揃っているか
- 検証用アセットや不要な差分が残っていないか
- `Missing Script` や参照切れがないか
- 本番 Scene に個人用の仮オブジェクトを置きっぱなしにしていないか
- Editor 上で最低限の再生確認をしたか
