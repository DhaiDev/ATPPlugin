# SystemHelper Dynamic Table Name - 使用說明

**修改日期**: 2025年11月6日  
**修改者**: SCChang  
**版本**: v2.2.1.0

---

## 📋 修改摘要

將 `SystemHelper.cs` 中的 hard-coded table name (`z_SysConfig`, `z_SysRef`) 改為可配置的動態屬性，同時保持向後兼容性。

---

## 🎯 修改目的

1. **彈性化**: 不同 Plugin 可使用不同的 System Table 名稱
2. **向後兼容**: 預設值仍為 `z_SysConfig` / `z_SysRef`，舊專案不受影響
3. **易於維護**: 只需在 Plugin 初始化時設定一次

---

## 🔧 技術實作

### 新增的屬性

```csharp
/// <summary>
/// SysConfig table name (configurable, default: z_SysConfig for backward compatibility)
/// </summary>
public static string SysConfigTableName { get; set; } = "z_SysConfig";

/// <summary>
/// SysRef table name (configurable, default: z_SysRef for backward compatibility)
/// </summary>
public static string SysRefTableName { get; set; } = "z_SysRef";
```

### 修改的方法

所有使用 hard-coded table name 的方法都已更新為使用屬性：

1. ✅ `SetSysConfigValue()` - 2 處
2. ✅ `GetSysConfigValue()` - 1 處
3. ✅ `SysConfigGetIntegerCounter()` - 1 處
4. ✅ `GetSysRefList()` - 1 處
5. ✅ `GetSysRefRow()` - 1 處
6. ✅ `GetSysRefID()` - 1 處

**總計**: 7 處修改，原始程式碼保留並加上註解

---

## 📖 使用方式

### 方式 1: BookHub Plugin 使用自訂 Table Name

在 `PlugInsInitializer.BeforeLoad()` 方法中設定：

```csharp
public override bool BeforeLoad(BeforeLoadArgs e)
{
    // Set custom system table names for BookHub Plugin
    // 設定 BookHub Plugin 使用的 System Table 名稱
    SystemHelper.SysConfigTableName = "zVTS_SysConfig";
    SystemHelper.SysRefTableName = "zVTS_SysRef";

    // 之後所有對 SystemHelper 的呼叫都會使用新的 table name
    RunCreateRequiredUDFs();
    RunEmbeddedSQLScripts(e.DBSetting);
    
    // ...其他初始化程式碼
    
    return true;
}
```

### 方式 2: 舊專案不需修改

**完全不需要做任何修改！** 預設值自動使用 `z_SysConfig` / `z_SysRef`

```csharp
// 舊專案的程式碼完全不用改
SystemHelper.SetSysConfigValue("MyConfig", "Value");
SystemHelper.GetSysConfigValue("MyConfig");
// 自動使用 z_SysConfig 表
```

---

## ✅ 測試檢查清單

### BookHub Plugin 測試

- [ ] 編譯 VTACPluginBase 專案
- [ ] 在 BookHub Plugin 的 `PlugInsInitializer.BeforeLoad()` 中設定：
  ```csharp
  SystemHelper.SysConfigTableName = "zVTS_SysConfig";
  SystemHelper.SysRefTableName = "zVTS_SysRef";
  ```
- [ ] 編譯 BookHub Plugin
- [ ] 執行 Plugin 並檢查 SQL Profiler，確認使用 `zVTS_SysConfig` / `zVTS_SysRef`
- [ ] 測試 `SetSysConfigValue()` 寫入正確的 table
- [ ] 測試 `GetSysConfigValue()` 讀取正確的 table
- [ ] 測試 `GetSysRefList()` 查詢正確的 table

### 舊專案相容性測試

- [ ] 編譯舊專案（不修改任何程式碼）
- [ ] 確認仍使用 `z_SysConfig` / `z_SysRef`
- [ ] 確認功能正常運作

---

## 🎨 SQL 對照表

### 修改前（Hard-coded）

```sql
-- SetSysConfigValue
select * from z_SysConfig where ConfigName='...'

-- GetSysRefList
select * from z_SysRef where RefGroup='...'
```

### 修改後（Dynamic）

```sql
-- 當 SysConfigTableName = "zVTS_SysConfig"
select * from zVTS_SysConfig where ConfigName='...'

-- 當 SysRefTableName = "zVTS_SysRef"
select * from zVTS_SysRef where RefGroup='...'
```

---

## 📊 影響範圍

### ✅ 已修改的檔案

| 檔案 | 路徑 | 修改內容 |
|------|------|---------|
| SystemHelper.cs | VTACPluginBase\Classes\Helpers\ | 新增 2 個屬性，修改 6 個方法 |

### 🔄 需要更新的專案

| 專案 | 動作 | 優先度 |
|------|------|--------|
| BookHub AC Plugins v2.2.1.0 | 設定 table name | ⭐⭐⭐ 高 |
| 其他舊專案 | 不需修改 | - |

---

## 🚨 注意事項

1. **設定時機**: 必須在任何 `SystemHelper` 方法被呼叫之前設定
2. **全域影響**: 設定後會影響整個 AppDomain 內的所有呼叫
3. **Thread-Safe**: 靜態屬性是 thread-safe 的
4. **SQL Injection**: 雖然 table name 現在是動態的，但仍使用字串插值，請確保來源可信

---

## 📝 程式碼變更日誌

### 2025-11-06 (SCChang)

**新增**:
- `SysConfigTableName` 屬性 (預設: "z_SysConfig")
- `SysRefTableName` 屬性 (預設: "z_SysRef")

**修改**:
- `SetSysConfigValue()`: 2 處使用 `SysConfigTableName`
- `GetSysConfigValue()`: 1 處使用 `SysConfigTableName`
- `SysConfigGetIntegerCounter()`: 1 處使用 `SysConfigTableName`
- `GetSysRefList()`: 1 處使用 `SysRefTableName`
- `GetSysRefRow()`: 1 處使用 `SysRefTableName`
- `GetSysRefID()`: 1 處使用 `SysRefTableName`

**保留**:
- 所有原始 hard-coded 程式碼（已註解）
- 向後兼容性（預設值不變）

---

## 🔗 相關文件

- [AutoCount Plugin 知識庫](../../07%20BookHub%20AC%20Plugins/Source%20Code/md/20251025-autocount-plugin-knowledge-report.md)
- [BookHub PO Tracking 實作進度](../../07%20BookHub%20AC%20Plugins/Source%20Code/md/20251105-bookhub-po-tracking-progress.md)

---

**最後更新**: 2025年11月6日  
**文件版本**: v1.0
