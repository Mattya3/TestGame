# REQUEST: Issue #137「外的影響 ExternalEffect の実装」

## 1. 対象 Issue
- Main Issue: https://github.com/Mattya3/TestGame/issues/97
- Sub Issue: https://github.com/Mattya3/TestGame/issues/137

## 2. 目的
外的影響の責務を `IMoveController` から分離し、`IPlayerStateContext` 系の振る舞い差し替えで拡張可能にする。  
既存挙動（死亡/ゴール/凍結など）を壊さないことを最優先とする。

## 3. 参照必須ドキュメント
- `Docs/CodingStandards/CSharp.md`
- `Docs/CodingStandards/UnityRules.md`

## 4. 変更範囲（厳守）
- `Assets/Scripts/Characters` 配下
- `Assets/Scripts/Game` 配下

上記以外のフォルダは変更しないこと。  
Scene / Prefab / Asset / ProjectSettings / Docs の変更は禁止。

## 5. 実装方式（確定）

### 5.1 Context 差し替え方針
- `Player` は `IPlayerStateContext` 実体を保持する
- 実体は固定し、内部で利用する「振る舞いオブジェクト」を差し替える
- 差し替え対象は、外的影響で変化しうるメソッド群とする

### 5.2 on/off のトリガー
- 2人同時横入力を `MovementRuleManager` が検知する
- `false -> true` になったタイミングで外的影響適用版へ差し替える
- `true -> false` になったタイミングで通常版へ戻す
- 毎フレーム監視ではなく、入力更新イベント起点で再評価する

### 5.3 複数 ExternalEffect の解決
- 外的影響は複数同時定義を許可する
- Effect ごとに「どのメソッドを上書きするか」を宣言する
- メソッド単位で競合解決する
  - 被っていないメソッド: その Effect を採用
  - 被っているメソッド: `priority` の高い Effect を採用
  - `priority` 同値: 登録順で安定決定

### 5.4 2人プレイ前提
- 本 Issue ではプレイヤー数は2人固定前提とする

## 6. ExternalEffect 仕様（確定）

### 6.1 ReverseInputEffect
- 発火条件: 2人同時横入力中
- 影響内容:
  - 入力 `X` を反転
  - 入力 `Y` は維持

### 6.2 ReverseGravityEffect
- 発火条件: 2人同時横入力中
- 影響内容:
  - 重力スケールを反転

## 7. 既存構成との関係
- `IMoveController` の外的影響責務を外し、Context 側に移す
- 既存の状態遷移（死亡/ゴール/凍結）は破壊しない
- 既存ステート実装から見た `IPlayerStateContext` 契約は維持する

## 8. 受け入れ条件
- [ ] `IPlayerStateContext` 系の振る舞い差し替えで外的影響が適用される
- [ ] 2人同時横入力の on/off で差し替えが正しく切り替わる
- [ ] 複数 Effect でメソッド単位競合解決（priority, 同値時登録順）が機能する
- [ ] ReverseInputEffect が指定条件でのみ発火する
- [ ] ReverseGravityEffect が指定条件でのみ発火する
- [ ] 既存挙動が変化していない（Sub Issue #137 完了条件）
- [ ] コンパイルエラーがない
- [ ] 変更範囲制約を守っている

## 9. 動作確認項目
- 2人とも横入力中のときのみ入力反転が有効
- 片方のみ横入力時は通常入力
- 2人とも非横入力時は通常入力
- on/off 遷移時に通常版と外的影響版が正しく切り替わる
- ReverseGravityEffect 有効時のみ重力反転
- 死亡・ゴール・凍結の遷移が従来通り動作
- priority 変更・同値時の登録順で結果が安定する

## 10. コミット戦略（再定義）

### Commit 1: Context 差し替え基盤を導入
- `Player` 内に「差し替え可能な Context 振る舞い」構造を追加
- 通常版振る舞い（Default）を実装
- 既存挙動を維持する（差し替え未使用では完全互換）

### Commit 2: ExternalEffect のメソッド上書きモデルを導入
- `ExternalEffectBase` に「上書き可能メソッド定義」を追加
- `ReverseInputEffect` / `ReverseGravityEffect` を追加または整備
- Effect からメソッド上書き情報を取得できる状態にする

### Commit 3: Manager で on/off 検知と振る舞い差し替えを実装
- 2人同時横入力の状態遷移検知を追加
- `on` で Effect 合成版振る舞いを適用
- `off` で通常版振る舞いへ復帰

### Commit 4: 複数 Effect の競合解決を実装
- メソッド単位で `priority` / 登録順ルールを適用
- 被るメソッドだけ優先度解決し、被らないメソッドは併用
- 手動確認で期待通りの採用結果を確認

### Commit 5: 旧経路整理と最終確認
- 不要になった `MovementRules`（`IMoveController` 側の外的影響責務）を削除
- コンパイル確認
- 受け入れ条件・動作確認項目を満たすことを最終確認
