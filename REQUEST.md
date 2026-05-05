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

### Commit 1: Game 側で同時左右入力トリガーを実装
- `MovementRuleManager`（必要なら改名先）で2人同時横入力の on/off 遷移を検知
- 入力更新イベント起点で再評価し、毎フレーム監視はしない
- このコミットでは「検知のみ」に絞り、Effect 適用は行わない

### Commit 2: Game 側で単数 ExternalEffect を受け取り適用
- ステージ設定として単数 ExternalEffect を受け取る経路を追加
- on で外的影響版に切り替え、off で通常版に戻す
- 既存状態遷移（死亡/ゴール/凍結）への影響がないことを確認

### Commit 3: MovementRules を廃止
- `IMoveController` / `MoveControllerFactory` / `ReverseMoveController` / `DemoMoveController` を削除
- `Player.MoveController` 依存を除去し、移動を Context 側へ一本化
- コンパイルを通し、既存挙動を維持

### Commit 4: Player 側で複数 ExternalEffect の競合解決
- 複数 Effect をメソッド単位で合成する仕組みを実装
- 競合時は `priority` 高優先、同値は登録順で安定決定
- 被っていないメソッドは併用する

### Commit 5: Game 側で複数 ExternalEffect を受け取り可能化 + 最終確認
- ステージ設定を単数から複数リストへ拡張
- on 時に複数 Effect を適用、off 時に通常版へ復帰
- 受け入れ条件・動作確認項目・コンパイル確認を実施
