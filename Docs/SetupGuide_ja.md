# 巨大怪獣ミニゲーム（Android向け）セットアップ手順

## 1. Unityプロジェクト作成
1. Unity Hubで **New project** を押す
2. テンプレートは **3D (URPではない標準3Dでも可)**
3. Unityバージョンは **LTS最新版** を選択
4. プロジェクト名: `GiantMonsterSimulator`

## 2. シーン構成（Hierarchy）
以下の構成を作成します。

- `Main Camera`
- `Directional Light`（初期状態のまま）
- `Player`（Capsule）
- `Ground`（Plane）
- `Buildings`（Empty）
- `Canvas`
  - `JoystickRoot`（Image）
    - `Handle`（Image）

## 3. オブジェクト設定値

### Player（Capsule）
- Transform
  - Position: `(0, 1, 0)`
  - Scale: `(2, 4, 2)`（巨大怪獣っぽいサイズ感）
- Add Component
  - `CharacterController`
    - Center: `(0, 1, 0)`
    - Radius: `0.8`
    - Height: `2`
    - Step Offset: `0.4`
  - `PlayerController`（後述スクリプト）

### Ground（Plane）
- Transform
  - Position: `(0, 0, 0)`
  - Scale: `(10, 1, 10)`

### Buildings（Empty）
1. `Cube` を1つ作成して `Building_00` にリネーム
2. `Buildings` の子にする
3. `Building_00` を `Ctrl + D` で20〜40個複製
4. それぞれの位置と高さをランダムに変更
   - Position X/Z: `-80〜80`
   - Scale Y: `5〜25`
   - Scale X/Z: `3〜8`

## 4. カメラ設定

### Main Camera
- Position: `(0, 8, -15)`（初期値）
- Rotation: `(20, 0, 0)`（目安）
- Add Component
  - `CameraController`
    - Target: `Player`
    - Distance: `16`
    - Height: `7`
    - Follow Smooth: `7`
    - Pitch: `20`
    - Yaw Speed: `0.15`

## 5. UI（左ジョイスティック + 右スワイプ）
1. `Canvas` を作成（Render Mode: **Screen Space - Overlay**）
2. `Canvas Scaler`
   - UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: `1920 x 1080`
3. `JoystickRoot` (Image)
   - Anchor: 左下
   - Pos X: `160`, Pos Y: `160`
   - Width/Height: `220`
   - 色は半透明グレー
4. `Handle` (Image)
   - `JoystickRoot` の子
   - Anchor中央
   - Width/Height: `100`
   - 色は少し明るい半透明
5. `JoystickRoot` に `Joystick` スクリプトを追加
   - Background: `JoystickRoot` を割り当て
   - Handle: `Handle` を割り当て

## 6. スクリプト配置と参照設定
1. `Assets/Scripts` フォルダを作成
2. 以下3つのスクリプトを配置
   - `PlayerController.cs`
   - `CameraController.cs`
   - `Joystick.cs`
3. `Player` の `PlayerController`
   - Joystick: `JoystickRoot` を割り当て
   - Camera Pivot: `Main Camera` を割り当て
4. `Main Camera` の `CameraController`
   - Target: `Player` を割り当て

## 7. 操作仕様
- 左下ジョイスティック: 前後左右移動
- 右半分ドラッグ: カメラをY軸回転
- プレイヤーは移動方向（カメラ基準）へ向きを補間して回転

## 8. Androidビルド手順
1. `File > Build Settings`
2. Platformで **Android** を選択し `Switch Platform`
3. `Player Settings` を開く
   - Company Name / Product Name を設定
   - `Other Settings > Package Name` を設定（例: `com.example.giantmonster`）
   - `Minimum API Level` を一般的に `Android 8.0 (API 26)` 以上に設定
4. `Build Settings` に現在のSceneを `Add Open Scenes`
5. 実機接続（USBデバッグON）
6. `Build And Run` を実行

## 9. 軽量化の最小ポイント
- 物理挙動は `Rigidbody` ではなく `CharacterController` を使用
- 建物は単色マテリアルで十分
- 影品質は必要最小限（Project Settings > Quality）
- 目標フレームレートは `Application.targetFrameRate = 60`（必要なら設定）
