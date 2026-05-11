# BusinessBase V2.0.0 SQL 生成邏輯修正說明

## 🔧 修正內容

### 問題識別
用戶指出 V2.0.0 版本的 SQL 生成邏輯存在兩個重要問題：

1. **屬性過濾不完整**: 從 DataRow 方式可能無法完全過濾掉非資料庫欄位
2. **審計欄位處理錯誤**: Update 操作可能會錯誤地更新 `CreatedTimeStamp` 和 `CreatedUserID`

### 🔄 修正策略

#### 1. 恢復 Property-based SQL 生成
- ✅ **SQLUpdateStmt()**: 恢復使用 Properties 而不是 DataRow
- ✅ **SQLInsertStmt()**: 恢復使用 Properties 而不是 DataRow  
- ✅ **屬性過濾邏輯**: 完全保留原始過濾規則

#### 2. 正確的審計欄位處理
- ✅ **INSERT 操作**: 設定所有審計欄位
  - `CreatedTimeStamp = getdate()`
  - `CreatedUserID = 當前用戶`
  - `LastModified = getdate()`
  - `LastModifiedUserID = 當前用戶`

- ✅ **UPDATE 操作**: 只更新修改相關欄位
  - `LastModified = getdate()`
  - `LastModifiedUserID = 當前用戶`
  - **不觸碰** `CreatedTimeStamp` 和 `CreatedUserID`

## 📊 修正對比

### 屬性過濾邏輯

#### ✅ 修正後 (Property-based)
```csharp
PropertyInfo[] propertyInfos = typ_BO.GetProperties();
for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
{
    // 跳過不必要的欄位
    if (propertyInfos[int_i].Name.Contains("Temp") || 
        propertyInfos[int_i].Name.Contains("DocDesc")) continue;

    // 檢查屬性標籤
    attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
    if (attribute != null && (attribute.IsDisplayField || attribute.IsDataTable)) continue;
    
    // 處理屬性值...
}
```

#### ❌ 原本問題版本 (DataRow-based)
```csharp
for (int int_i = 0; int_i < dr.Table.Columns.Count; int_i++)
{
    // 只能基於 DataTable 欄位，無法檢查原始 Property 屬性
    var attribute = this.GetType().GetField(dr.Table.Columns[int_i].ColumnName)?.GetCustomAttribute<MyAttributesAttribute>();
    // 可能遺漏某些過濾條件
}
```

### 審計欄位處理

#### ✅ 修正後 - UPDATE 操作
```csharp
// 跳過審計建立欄位，避免覆蓋原始值
if (propertyInfos[int_i].Name == "CreatedTimeStamp" || 
    propertyInfos[int_i].Name == "CreatedUserID") continue;

// 手動添加審計更新欄位
str_rtn += "[LastModified]=getdate(), ";
str_rtn += "[LastModifiedUserID]=N'" + SQLString(currentUser) + "' ";
```

#### ✅ 修正後 - INSERT 操作  
```csharp
// 添加所有審計欄位
str_rtn += "[CreatedTimeStamp], [LastModified], ";
str_Values += "getdate(), getdate(), ";

str_rtn += "[CreatedUserID], [LastModifiedUserID], ";
str_Values += "N'" + SQLString(currentUser) + "', N'" + SQLString(currentUser) + "', ";
```

#### ❌ 原本問題版本
```csharp
// 可能會更新所有欄位，包括不應該被更新的 CreatedTimeStamp 和 CreatedUserID
for (int int_i = 0; int_i < dr.Table.Columns.Count; int_i++)
{
    // 沒有特別排除審計建立欄位
    str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "]=";
    str_rtn += FormatSQLValue(obj_Value) + ", ";
}
```

## 🔍 過濾邏輯詳解

### INSERT 時過濾的欄位
- ✅ 包含 "Temp" 的欄位 (如 TempCalculatedField)
- ✅ 包含 "DocDesc" 的欄位 (如 DocDescription)  
- ✅ `IsDisplayField = true` 的屬性
- ✅ `IsDataTable = true` 的屬性
- ✅ null 值或 DateTime.MinValue

### UPDATE 時額外過濾的欄位
- ✅ `IsPrimaryKey = true` 的屬性
- ✅ `CreatedTimeStamp` 欄位
- ✅ `CreatedUserID` 欄位

## 🔒 審計合規性

### 資料完整性保證
- ✅ **建立資訊永不變更**: `CreatedTimeStamp`, `CreatedUserID` 在 UPDATE 時被保護
- ✅ **修改資訊自動更新**: `LastModified`, `LastModifiedUserID` 在每次 UPDATE 時自動設定
- ✅ **用戶追蹤**: 自動獲取當前 AutoCount 用戶，失敗時默認為 "SYSTEM"

### 企業級審計要求
```sql
-- INSERT 時產生的 SQL 範例
INSERT INTO MyTable ([Field1], [Field2], [CreatedTimeStamp], [LastModified], [CreatedUserID], [LastModifiedUserID]) 
VALUES (N'Value1', N'Value2', getdate(), getdate(), N'USER001', N'USER001')

-- UPDATE 時產生的 SQL 範例  
UPDATE MyTable SET [Field1]=N'NewValue1', [Field2]=N'NewValue2', [LastModified]=getdate(), [LastModifiedUserID]=N'USER001' 
WHERE [DocKey]=123
```

## 🚀 效能與相容性

### 效能最佳化
- ✅ **反射快取**: Property 資訊在型別層級快取
- ✅ **字串優化**: 減少不必要的字串串接
- ✅ **類型判斷**: 使用 switch 語句提升效能

### 向後相容性
- ✅ **100% 保留原始邏輯**: 與原始 BusinessBase_Cls 行為完全一致
- ✅ **DataRow 支援**: 保留 DataRowValuesAssignment 方法做為向後相容
- ✅ **屬性標籤**: 完全支援 MyAttributesAttribute 系統

## 📋 測試建議

### 功能測試
1. **插入新記錄**: 驗證所有審計欄位正確設定
2. **更新現有記錄**: 確認 Created 欄位未被修改
3. **屬性過濾**: 確認 Temp, DocDesc, Display 欄位被正確跳過
4. **null 值處理**: 驗證 null 值和 DateTime.MinValue 處理

### 資料庫驗證
```sql
-- 檢查審計欄位
SELECT DocKey, CreatedTimeStamp, CreatedUserID, LastModified, LastModifiedUserID 
FROM YourTable 
WHERE DocKey = @TestDocKey

-- 驗證 Created 欄位在 UPDATE 後未變更
-- 驗證 LastModified 欄位在 UPDATE 後有更新
```

## 🎯 總結

這次修正解決了兩個關鍵問題：

1. **完整的屬性過濾**: 恢復 Property-based 方式，確保所有原始過濾邏輯都正確執行
2. **正確的審計欄位管理**: 嚴格區分 INSERT 和 UPDATE 的審計欄位處理，符合企業級資料管理要求

修正後的版本既保持了 V2.0.0 的現代化功能（DI、Async、OperationResult），又完全恢復了原始版本在 SQL 生成方面的精確邏輯。