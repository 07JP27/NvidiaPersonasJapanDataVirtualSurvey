# NvidiaPersonasJapanDataVirtualSurvey

## 概要

このプロジェクトは、NVIDIA の **Nemotron-Personas-Japan** データセットを活用して、仮想的なアンケート調査を実施するための C# ライブラリとコンソールアプリケーションです。様々な背景を持つ日本の合成ペルソナに対して、Azure OpenAI のバッチ API を使用して大規模なアンケート調査をシミュレートできます。

## 主な機能

- **多様なペルソナデータの活用**: NVIDIA の Nemotron-Personas-Japan データセットから、年齢、性別、職業、居住地などの属性を持つ合成ペルソナを読み込み
- **3つのアンケート形式をサポート**:
  - **自由記述式**: 指定した文字数で自由に意見を述べる形式
  - **Yes/No 形式**: 二択で回答する形式
  - **選択式**: 複数の選択肢から単一または複数選択する形式
  ※ISurveyRequestインターフェースを実装することで、独自のアンケート形式も追加可能
- **Azure OpenAI Batch API 統合**: 大量のアンケートを効率的に処理
- **自動データ取得**: Hugging Face から Parquet ファイルを自動ダウンロード

## プロジェクト構成

```
NvidiaPersonasJapanDataVirtualSurvey/
├── NvidiaPersonasJapanDataVirtualSurvey.Console/  # コンソールアプリケーション
│   └── Program.cs                                   # サンプル実行コード
└── NvidiaPersonasJapanDataVirtualSurvey.Core/      # コアライブラリ
    ├── SurveyService.cs                             # アンケート実行サービス
    ├── AoaiBatchService.cs                          # Azure OpenAI バッチ処理
    ├── DataLoader.cs                                # ペルソナデータ読み込み
    └── Models/                                       # データモデル
        ├── FreeTextSurveyRequest.cs                  # 自由記述式アンケート
        ├── YesNoSurveyRequest.cs                     # Yes/No形式アンケート
        └── OptionSelectSurveyRequest.cs              # 選択式アンケート
```

## セットアップ

### 必要な環境

- .NET 9.0 SDK
- Azure OpenAI アカウントとリソース

### Azure OpenAI の設定

1. Azure Portal で Azure OpenAI リソースを作成
2. モデルをデプロイ（例: gpt-4o など）
3. エンドポイント URL と API キーを取得

### 使用方法

1. `Program.cs` で Azure OpenAI の接続情報を設定:

```csharp
const string endpoint = "https://xxxxx.openai.azure.com";
const string apiKey = "xxxxxxxxxxxxxxxxxxxxx";
const string deploymentName = "xxxxxxxxxxxxxx";
```

2. アンケートリクエストを作成:

```csharp
// 自由記述式
var request = new FreeTextSurveyRequest("昨今のAI(LLM)の目覚ましい進化についてどう思いますか？", 300);

// Yes/No形式
var request = new YesNoSurveyRequest("現在の日本において金融緩和政策は必要だと思いますか？");

// 選択式
var request = new OptionSelectSurveyRequest(
    "あなたが食べて見たいのはどちらですか？",
    new List<string> { "正統派芋煮", "庄内風芋煮" },
    isMultiSelect: false
);
```

3. アンケートを実行:

```csharp
var service = await SurveyService.CreateAsync(
    batchClient, 
    fileClient, 
    deploymentName, 
    sampleSize: 5  // サンプル数を指定
);

var result = await service.RunSurveyAsync(request);
```

## 使用例
完全なコードは[NvidiaPersonasJapanDataVirtualSurvey.Console/Program.cs](https://github.com/07JP27/NvidiaPersonasJapanDataVirtualSurvey/blob/main/NvidiaPersonasJapanDataVirtualSurvey.Console/Program.cs)を参照してください。
```csharp
// 5人のペルソナに対してアンケートを実施
var service = await SurveyService.CreateAsync(batchClient, fileClient, deploymentName, 5);

var request = new FreeTextSurveyRequest("昨今のAI(LLM)の目覚ましい進化についてどう思いますか？", 300);
var result = await service.RunSurveyAsync(request);

// 結果の表示
foreach (var answer in result.Answers)
{
    Console.WriteLine($"{answer.Persona.Age}歳 / {answer.Persona.Sex} / {answer.Persona.Occupation}");
    Console.WriteLine($"回答: {answer.Answer}");
}
```

## ライセンス

This project uses nvidia/Nemotron-Personas-Japan, licensed under CC BY 4.0.
https://creativecommons.org/licenses/by/4.0/

```
@software{nvidia/Nemotron-Personas-Japan,
  author = {Fujita, Atsunori and Gong, Vincent and Ogushi, Masaya and Yamamoto, Kotaro and Suhara, Yoshi and Corneil, Dane and Meyer, Yev},
  title = {{Nemotron-Personas-Japan}: Synthetic Personas Aligned to Real-World Distributions},
  month = {September},
  year = {2025},
  url = {https://huggingface.co/datasets/nvidia/Nemotron-Personas-Japan}
}
```

## 参考リンク

- [NVIDIA Nemotron-Personas-Japan Dataset](https://huggingface.co/datasets/nvidia/Nemotron-Personas-Japan)
- [Azure OpenAI Service](https://azure.microsoft.com/ja-jp/products/ai-services/openai-service)