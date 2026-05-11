# BusinessBase V2.0.0 升級指南

## 概述

BusinessBase V2.0.0 是針對 .NET Framework 4.8 的現代化版本，在保留所有原始功能的基礎上，增加了依賴注入、異步支援和改進的錯誤處理機制。

## 新功能特色

### 1. 依賴注入 (Dependency Injection)
- **ServiceLocator 模式**：統一的服務定位器介面
- **多容器支援**：Simple Container (內建)、Unity、Autofac
- **自動初始化**：確保 DI 容器總是可用

### 2. 異步編程支援
- **異步方法**：`SaveAsync()`, `DeleteAsync()`, `LoadAsync()`
- **OperationResult 模式**：統一的操作結果回傳
- **.NET Framework 4.8 最佳化**：使用 `ConfigureAwait(false)`

### 3. 改進的驗證機制
- **ValidationErrors 集合**：收集所有驗證錯誤
- **HasValidationErrors 屬性**：快速檢查驗證狀態
- **異步驗證**：支援異步驗證邏輯

### 4. 增強的錯誤處理
- **統一日誌介面**：`IErrorLogger` 介面
- **DI 整合**：透過容器注入日誌服務
- **向後相容**：保留原始 `ErrorLogger_Cls.Write()` 功能

## 升級步驟

### 步驟 1：初始化依賴注入

```csharp
// 在應用程式啟動時初始化 DI
using VTACPluginBase.Classes.DI;

// 選項 1：使用 Simple Container (推薦)
DIConfiguration.ConfigureSimpleContainer();

// 選項 2：確保 DI 已初始化 (如未配置則使用預設值)
DIConfiguration.EnsureInitialized();

// 選項 3：自訂服務註冊
DIConfiguration.ConfigureSimpleContainer(container =>
{
    container.RegisterSingleton<IMyService>(new MyServiceImplementation());
});
```

### 步驟 2：更新業務邏輯類別

#### 原始類別 (保持不變)
```csharp
public class MyBusinessEntity : BusinessBase_Cls
{
    public override string Name => "My Business Entity";
    public override long PrimaryKey { get; set; }
    public override string TableName => "MyTable";
    public override string TableQryName => "v_MyTable";
}
```

#### 新 V2.0.0 類別
```csharp
public class MyBusinessEntityV2 : BusinessBaseV2_0_0_Cls
{
    public override string Name => "My Business Entity V2";
    public override long PrimaryKey { get; set; }
    public override string TableName => "MyTable";
    public override string TableQryName => "v_MyTable";
    
    // 可選：自訂驗證
    protected override bool ValidateEntity()
    {
        if (!base.ValidateEntity()) return false;
        
        // 您的驗證邏輯
        if (string.IsNullOrEmpty(SomeRequiredField))
            ValidationErrors.Add("SomeRequiredField is required");
            
        return !HasValidationErrors;
    }
}
```

### 步驟 3：更新使用代碼

#### 同步使用 (與原始版本相同)
```csharp
var entity = new MyBusinessEntityV2();
entity.SomeField = "Some Value";

using (var dbSetting = new DBSetting(connectionString))
{
    string result = entity.Save(dbSetting);
    if (string.IsNullOrEmpty(result))
    {
        // 保存成功
    }
    else
    {
        // 處理錯誤
    }
}
```

#### 異步使用 (新功能)
```csharp
var entity = new MyBusinessEntityV2();
entity.SomeField = "Some Value";

using (var dbSetting = new DBSetting(connectionString))
{
    var result = await entity.SaveAsync(dbSetting);
    if (result.IsSuccess)
    {
        // 保存成功
        Console.WriteLine($"Saved with DocKey: {entity.DocKey}");
    }
    else
    {
        // 處理錯誤
        Console.WriteLine($"Error: {result.Message}");
    }
}
```

## 遷移策略

### 逐步遷移 (推薦)
1. **保留原始類別**：不修改現有的 `BusinessBase_Cls` 相關代碼
2. **新功能使用 V2.0.0**：所有新開發使用 `BusinessBaseV2_0_0_Cls`
3. **逐步重構**：根據需要將舊類別遷移到 V2.0.0

### 並行使用
```csharp
// 原始版本繼續運作
var oldEntity = new MyBusinessEntity();
string oldResult = oldEntity.Save(dbSetting);

// 新版本提供額外功能
var newEntity = new MyBusinessEntityV2();
var newResult = await newEntity.SaveAsync(dbSetting);
```

## 相容性說明

### 完全向後相容
- 所有原始功能保持不變
- 現有代碼無需修改
- 資料庫架構無需變更

### 新功能為可選
- DI 容器：如未配置會自動初始化預設容器
- 異步方法：與同步方法並存，可選擇使用
- 驗證機制：向後相容原始驗證邏輯

## 效能考量

### .NET Framework 4.8 最佳化
```csharp
// 異步方法使用 ConfigureAwait(false) 避免死鎖
var result = await entity.SaveAsync(dbSetting).ConfigureAwait(false);

// DI 容器使用快取機制提升效能
var service = ServiceLocator.GetService<IMyService>();
```

### 記憶體管理
```csharp
// DI 容器支援 IDisposable 自動清理
using (var container = new SimpleContainerAdapter())
{
    // 容器會自動處理資源清理
}
```

## 最佳實踐

### 1. DI 容器配置
```csharp
// 在應用程式啟動時配置一次
public static void Application_Start()
{
    DIConfiguration.ConfigureSimpleContainer(container =>
    {
        // 註冊單例服務
        container.RegisterSingleton<IDataService>(new DataService());
        
        // 註冊暫態服務
        container.RegisterTransient<IEmailService>(() => new EmailService());
    });
}
```

### 2. 錯誤處理
```csharp
// 使用 OperationResult 模式
var result = await entity.SaveAsync(dbSetting);
if (!result.IsSuccess)
{
    // 記錄詳細錯誤
    Logger.Write("SaveOperation", result.Exception);
    
    // 顯示用戶友好訊息
    MessageBox.Show(result.Message);
}
```

### 3. 驗證最佳實踐
```csharp
protected override bool ValidateEntity()
{
    // 先執行基礎驗證
    if (!base.ValidateEntity()) return false;
    
    // 業務邏輯驗證
    if (Amount <= 0)
        ValidationErrors.Add("Amount must be positive");
        
    if (CustomerCode?.Length > 20)
        ValidationErrors.Add("Customer code too long");
    
    // 交叉欄位驗證
    if (StartDate > EndDate)
        ValidationErrors.Add("Start date must be before end date");
    
    return !HasValidationErrors;
}
```

## 故障排除

### 常見問題

#### 1. DI 容器未初始化
```csharp
// 錯誤：ServiceLocator 返回 null
var service = ServiceLocatorProvider.Current.GetService<IMyService>();

// 解決：確保 DI 已初始化
DIConfiguration.EnsureInitialized();
var service = ServiceLocatorProvider.Current.GetService<IMyService>();
```

#### 2. 異步死鎖 (.NET Framework 4.8)
```csharp
// 錯誤：在 UI 線程中同步等待異步方法
var result = entity.SaveAsync(dbSetting).Result; // 可能死鎖

// 解決：使用 ConfigureAwait(false) 或完全異步
var result = await entity.SaveAsync(dbSetting).ConfigureAwait(false);
```

#### 3. 驗證錯誤未顯示
```csharp
// 檢查驗證狀態
if (entity.HasValidationErrors)
{
    foreach (var error in entity.ValidationErrors)
    {
        Console.WriteLine($"Validation Error: {error}");
    }
}
```

## 移除功能說明

**無移除功能** - V2.0.0 版本完全保留原始功能，只新增功能不移除任何現有功能。

## 版本比較

| 功能 | 原始版本 | V2.0.0 版本 |
|------|----------|-------------|
| 基本 CRUD | ✅ | ✅ |
| AutoCount 整合 | ✅ | ✅ |
| 反射機制 | ✅ | ✅ |
| 同步操作 | ✅ | ✅ |
| 異步操作 | ❌ | ✅ |
| 依賴注入 | ❌ | ✅ |
| 驗證集合 | ❌ | ✅ |
| OperationResult | ❌ | ✅ |
| 現代錯誤處理 | ❌ | ✅ |

## 支援資源

- **範例代碼**：`Examples/BusinessBaseV2ExampleUsage.cs`
- **DI 配置**：`Classes/DI/DIConfiguration.cs`
- **容器適配器**：`Classes/DI/ContainerAdapters.cs`

## 結論

BusinessBase V2.0.0 提供了現代化的開發體驗，同時保持與現有系統的完全相容。建議新項目採用 V2.0.0，現有項目可根據需要逐步遷移。
