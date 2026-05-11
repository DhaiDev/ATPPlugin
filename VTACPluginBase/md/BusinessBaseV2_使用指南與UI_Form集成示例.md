# BusinessBaseV2_0_0_Cls 使用指南與UI Form集成示例

## 📋 目錄
1. [基本使用模式](#基本使用模式)
2. [業務類實現示例](#業務類實現示例)
3. [UI Form集成範例](#ui-form集成範例)
4. [與原有BusinessBase的對比](#與原有businessbase的對比)
5. [遷移建議](#遷移建議)

---

## 🎯 基本使用模式

### 1. 創建業務實體類
```csharp
// 司機管理業務類 - 使用 V2.0.0
public class DriverV2_Cls : BusinessBaseV2_0_0_Cls
{
    #region " Constructor "
    public DriverV2_Cls()
    {
        // 基底類自動處理：
        // - DI依賴注入初始化
        // - 資料結構建立
        // - Detail關係設定
        // - 屬性反射收集
    }
    #endregion

    #region " Required Abstract Properties Implementation "
    public override string Name => this.GetType().Name;

    [MyAttributes(IsPrimaryKey = true, PrimaryKeyFieldName = "DriverID", IsDeleteKey = true)]
    public override long PrimaryKey { get; set; } = -1;

    protected override string dbTableNameMaster => "zKHP_Driver";
    protected override string dbQueryNameMaster => "zKHP_Driver";

    protected override string SysConfigDocNoFormat => "DriverDocNoFormat";
    protected override string DocNoFormat => "DR-{yyMM}-{000000}";

    [MyAttributes(IsMainChildClass = true)]
    public override DriverDetailV2_Cls Detail { get; } = new DriverDetailV2_Cls();
    #endregion

    #region " Business Properties "
    [MyAttributes(IsDisplayKey = true, DisplayKeyFieldName = nameof(DriverName))]
    public string DriverName { get; set; } = "";
    
    [MyAttributes(IsDisplayField = true)]
    public int Age { get; set; } = 0;
    
    [MyAttributes(IsDisplayField = true)]
    public string PhoneNo { get; set; } = "";
    
    [MyAttributes(IsDisplayField = true)]
    public string NRIC { get; set; } = "";
    
    [MyAttributes(IsDisplayField = true)]
    public string Email { get; set; } = "";
    
    [MyAttributes(IsDisplayField = true)]
    public Boolean IsActive { get; set; } = true;
    #endregion

    #region " Custom Validation (V2.0.0 Enhancement) "
    protected override bool ValidateEntity()
    {
        // 呼叫基底驗證
        if (!base.ValidateEntity())
            return false;

        // 自訂驗證邏輯
        if (string.IsNullOrWhiteSpace(DriverName))
            ValidationErrors.Add("司機姓名為必填欄位");

        if (string.IsNullOrWhiteSpace(NRIC))
            ValidationErrors.Add("身份證號碼為必填欄位");

        if (string.IsNullOrWhiteSpace(PhoneNo))
            ValidationErrors.Add("聯絡電話為必填欄位");

        return !HasValidationErrors;
    }
    #endregion

    #region " Custom Save Logic (V2.0.0 Enhancement) "
    protected override string BeforeSave(DBSetting dbSetting)
    {
        // 保存前的自訂邏輯
        if (string.IsNullOrEmpty(DocNo) || DocNo == AutoCount.Const.AppConst.NewDocumentNo)
        {
            DocNo = GenerateDriverCode();
        }

        return base.BeforeSave(dbSetting);
    }

    protected override string AfterSave(DBSetting dbSetting)
    {
        // 保存後的自訂邏輯 - 可進行額外的業務處理
        try
        {
            // 例如：更新相關的統計資訊、發送通知等
            UpdateDriverStatistics(dbSetting);
            return base.AfterSave(dbSetting);
        }
        catch (Exception ex)
        {
            Logger.Write($"{this.GetType().Name}.AfterSave", ex);
            return ex.Message;
        }
    }

    private string GenerateDriverCode()
    {
        // 產生司機編號邏輯
        return $"DR{DateTime.Now:yyMM}{PrimaryKey:000000}";
    }

    private void UpdateDriverStatistics(DBSetting dbSetting)
    {
        // 更新統計資訊的範例邏輯
        // 這裡可以加入實際的統計更新邏輯
    }
    #endregion
}

// Detail類別 - 如果需要的話
public class DriverDetailV2_Cls : BusinessBaseDTLV2_0_0_Cls
{
    // Detail類別的實現（如果司機有明細資料）
    // 目前司機類別通常不需要Detail，但保留架構完整性
}
```

---

## 🏗️ UI Form集成範例

### 1. 傳統Form結構（保持不變）
```csharp
public partial class DriverConfigurationV2_Form : Form
{
    private const string Lcstr_TableName = "zKHP_Driver";
    private const string Lcstr_IDName = "DriverID";

    #region " Form Constructor "
    private FormHelper Lcls_formHelper = null;
    private Boolean boo_FirstLoad = false;

    public DriverConfigurationV2_Form(FormMethods enm_FormMethods, long DocKey = -1)
    {
        InitializeComponent();

        // 初始化FormHelper
        Lcls_formHelper = new FormHelper();
        Lcls_formHelper.AssignFormObject(this);
        Lcls_formHelper.AutoAddControlValueChangingEvent();

        // 設定Form模式
        Lenm_FormMethods = enm_FormMethods;
        this.DocKey = DocKey;

        // 根據Form模式設定界面
        switch (enm_FormMethods)
        {
            case FormMethods.NEW:
                this.Text = "新增司機資料";
                break;
            case FormMethods.EDIT:
                this.Text = "編輯司機資料";
                break;
            case FormMethods.VIEW:
                this.Text = "檢視司機資料";
                break;
        }

        boo_FirstLoad = true;
    }
    #endregion

    #region " Form Properties "
    private FormMethods Lenm_FormMethods = FormMethods.NEW;
    public FormMethods Method
    {
        get { return Lenm_FormMethods; }
    }

    protected bool Editable
    {
        get { return Lcls_formHelper.Editable; }
        set { Lcls_formHelper.Editable = value; }
    }

    public bool Modified
    {
        get { return Lcls_formHelper.FormIsModified; }
    }

    public bool IsDataSaved
    {
        get { return Lcls_formHelper.FormIsSaved; }
    }

    // ⭐ 關鍵差異：使用V2.0.0業務類別
    private readonly DriverV2_Cls BusinessObject = new DriverV2_Cls();

    private long Lng_DocKey = -1;
    public long DocKey
    {
        get { return Lng_DocKey; }
        set { Lng_DocKey = value; }
    }
    #endregion

    #region " Form Events "
    private void DriverConfigurationV2_Form_Load(System.Object sender, System.EventArgs e)
    {
        try
        {
            if (boo_FirstLoad)
            {
                boo_FirstLoad = false;
                RefreshDisplay();
            }
        }
        catch (Exception ex)
        {
            ErrorLogger_Cls.Write(this.Name + "." + nameof(DriverConfigurationV2_Form_Load) + "()", ex);
        }
    }
    #endregion

    #region " Ribbon BarButton Events "
    private void Save_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // ⭐ V2.0.0 增強功能：使用內建驗證
            if (!BusinessObject.ValidateEntity())
            {
                string validationMessage = string.Join("\n", BusinessObject.ValidationErrors);
                MessageBox.Show($"資料驗證失敗：\n{validationMessage}", "驗證錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 傳統保存流程（與原版完全相同）
            SaveToBusinessObject();
            string str_savedata = this.BusinessObject.Save();  // ⭐ 保持原有的Save()調用方式
            
            if (str_savedata == "")
            {
                this.Lcls_formHelper.FormIsSaved = true;
                MessageBox.Show("資料已成功儲存！", "儲存成功", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (this.ProceedAfterSave_BarChkItem.Checked)
                {
                    // 繼續新增下一筆
                    ResetForm();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("儲存失敗：" + str_savedata, "錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            ErrorLogger_Cls.Write(this.Name + "." + nameof(Save_BarBtn_ItemClick) + "()", ex);
            MessageBox.Show("儲存過程發生錯誤：" + ex.Message, "系統錯誤", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Delete_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        if (!HasDocKey("Delete")) return;

        // ⭐ 使用V2.0.0的DeleteDlg_Form（可能需要建立V2版本）
        DeleteDlgV2_Form frm_del = new DeleteDlgV2_Form(FormMethods.DELETE, this.BusinessObject, this.DocKey);
        frm_del.ShowDialog();

        if (frm_del.DialogResult == DialogResult.Yes)
        {
            if (frm_del.IsDataSaved)
            {
                MessageBox.Show("資料已成功刪除！", "刪除成功", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
    #endregion

    #region " Data Manipulation "
    private void RefreshDisplay()
    {
        try
        {
            // 載入資料（與原版相同的模式）
            if (this.Method != FormMethods.NEW) 
                this.BusinessObject.Load(this.DocKey.ToString());

            // 顯示資料到界面
            DisplayDataToForm();

            // 設定界面狀態
            SetFormState();
        }
        catch (Exception ex)
        {
            ErrorLogger_Cls.Write(this.Name + "." + nameof(RefreshDisplay) + "()", ex);
        }
    }

    private void DisplayDataToForm()
    {
        // 將BusinessObject的資料顯示到Form控制項
        this.DriverName_TxtEdit.Text = BusinessObject.DriverName;
        this.PhoneNo_TxtEdit.Text = BusinessObject.PhoneNo;
        this.NRIC_TxtEdit.Text = BusinessObject.NRIC;
        this.Email_TxtEdit.Text = BusinessObject.Email;
        this.Age_SpEdit.Value = BusinessObject.Age;
        this.IsActive_ChkEdit.Checked = BusinessObject.IsActive;
        this.DocDate_DateEdit.DateTime = BusinessObject.DocDate;
        this.DocStatus_TxtEdit.Text = BusinessObject.DocStatus;
        
        // ⭐ V2.0.0新增：顯示驗證錯誤（如果有的話）
        if (BusinessObject.HasValidationErrors)
        {
            ValidationErrors_Memo.Text = string.Join("\n", BusinessObject.ValidationErrors);
            ValidationErrors_Memo.Visible = true;
        }
        else
        {
            ValidationErrors_Memo.Visible = false;
        }
    }

    private void SaveToBusinessObject()
    {
        // 將Form控制項的資料保存到BusinessObject
        BusinessObject.DriverName = this.DriverName_TxtEdit.Text.Trim();
        BusinessObject.PhoneNo = this.PhoneNo_TxtEdit.Text.Trim();
        BusinessObject.NRIC = this.NRIC_TxtEdit.Text.Trim();
        BusinessObject.Email = this.Email_TxtEdit.Text.Trim();
        BusinessObject.Age = Convert.ToInt32(this.Age_SpEdit.Value);
        BusinessObject.IsActive = this.IsActive_ChkEdit.Checked;
        BusinessObject.DocDate = this.DocDate_DateEdit.DateTime;
        BusinessObject.PrimaryKey = this.DocKey;
    }

    private bool HasDocKey(string str_Action, bool boo_ShowWarningMsg = true)
    {
        // 檢查是否有DocKey的邏輯（與原版相同）
        if (this.DocKey <= 0)
        {
            if (boo_ShowWarningMsg)
            {
                MessageBox.Show($"請先選擇要{str_Action}的記錄！", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return false;
        }
        return true;
    }
    #endregion
}
```

### 2. 非同步操作範例（V2.0.0新功能）
```csharp
public partial class DriverConfigurationV2_Form : Form
{
    #region " Async Operations (V2.0.0 Enhancement) "
    private async void SaveAsync_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // 顯示進度指示器
            ShowProgressBar("正在儲存資料...");

            // 驗證資料
            if (!await BusinessObject.ValidateEntityAsync())
            {
                string validationMessage = string.Join("\n", BusinessObject.ValidationErrors);
                MessageBox.Show($"資料驗證失敗：\n{validationMessage}", "驗證錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 保存資料到BusinessObject
            SaveToBusinessObject();

            // ⭐ 使用V2.0.0的非同步保存
            var result = await BusinessObject.SaveAsync();
            
            if (result.IsSuccess)
            {
                this.Lcls_formHelper.FormIsSaved = true;
                MessageBox.Show("資料已成功儲存！", "儲存成功", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (this.ProceedAfterSave_BarChkItem.Checked)
                {
                    ResetForm();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("儲存失敗：" + result.Message, "錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            ErrorLogger_Cls.Write(this.Name + "." + nameof(SaveAsync_BarBtn_ItemClick) + "()", ex);
            MessageBox.Show("儲存過程發生錯誤：" + ex.Message, "系統錯誤", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            HideProgressBar();
        }
    }

    private async void LoadAsync_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            if (!HasDocKey("載入")) return;

            ShowProgressBar("正在載入資料...");

            // ⭐ 使用V2.0.0的非同步載入
            var result = await BusinessObject.LoadAsync(this.DocKey.ToString());
            
            if (result.IsSuccess)
            {
                DisplayDataToForm();
                SetFormState();
            }
            else
            {
                MessageBox.Show("載入失敗：" + result.Message, "錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            ErrorLogger_Cls.Write(this.Name + "." + nameof(LoadAsync_BarBtn_ItemClick) + "()", ex);
        }
        finally
        {
            HideProgressBar();
        }
    }

    private void ShowProgressBar(string message)
    {
        // 顯示進度條的實現
        // 可以使用 SplashScreenManager 或其他進度指示器
    }

    private void HideProgressBar()
    {
        // 隱藏進度條的實現
    }
    #endregion
}
```

---

## 🔄 與原有BusinessBase的對比

### 原有使用方式
```csharp
// 原有BusinessBase_Cls使用方式
public class Driver_Cls : BusinessBase_Cls
{
    public Driver_Cls()
    {
        // 較少的自動化處理
    }

    // 基本的override
    public override string Name => this.GetType().Name;
    [MyAttributes(IsPrimaryKey = true, PrimaryKeyFieldName = "DriverID")]
    public override long PrimaryKey { get; set; } = -1;
    
    // 基本保存（無驗證增強）
    public override string Save()
    {
        return base.Save();
    }
}

// UI Form中的使用
private readonly Driver_Cls BusinessObject = new Driver_Cls();

private void Save_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
{
    SaveToBusinessObject();
    string str_savedata = this.BusinessObject.Save();  // 基本保存
    
    if (str_savedata == "")
    {
        // 保存成功
        this.Lcls_formHelper.FormIsSaved = true;
        MessageBox.Show("儲存成功！");
    }
    else
    {
        // 保存失敗
        MessageBox.Show("儲存失敗：" + str_savedata);
    }
}
```

### V2.0.0增強使用方式
```csharp
// V2.0.0 BusinessBaseV2_0_0_Cls使用方式
public class DriverV2_Cls : BusinessBaseV2_0_0_Cls
{
    public DriverV2_Cls()
    {
        // 自動處理DI、資料結構、屬性收集等
    }

    // 增強的validation
    protected override bool ValidateEntity()
    {
        // 自訂驗證邏輯
        if (!base.ValidateEntity()) return false;
        
        if (string.IsNullOrWhiteSpace(DriverName))
            ValidationErrors.Add("司機姓名為必填欄位");
            
        return !HasValidationErrors;
    }

    // 增強的保存流程
    protected override string BeforeSave(DBSetting dbSetting)
    {
        // 保存前邏輯
        return base.BeforeSave(dbSetting);
    }

    protected override string AfterSave(DBSetting dbSetting)
    {
        // 保存後邏輯
        return base.AfterSave(dbSetting);
    }
}

// UI Form中的增強使用
private readonly DriverV2_Cls BusinessObject = new DriverV2_Cls();

private void Save_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
{
    // 增強驗證
    if (!BusinessObject.ValidateEntity())
    {
        string validationMessage = string.Join("\n", BusinessObject.ValidationErrors);
        MessageBox.Show($"驗證失敗：\n{validationMessage}");
        return;
    }

    SaveToBusinessObject();
    string str_savedata = this.BusinessObject.Save();  // ⭐ 保持相同的調用方式
    
    if (str_savedata == "")
    {
        // 保存成功（包含BeforeSave/AfterSave處理）
        this.Lcls_formHelper.FormIsSaved = true;
        MessageBox.Show("儲存成功！");
    }
    else
    {
        MessageBox.Show("儲存失敗：" + str_savedata);
    }
}

// 非同步選項（新功能）
private async void SaveAsync_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
{
    if (!await BusinessObject.ValidateEntityAsync()) return;
    
    SaveToBusinessObject();
    var result = await BusinessObject.SaveAsync();
    
    if (result.IsSuccess)
    {
        MessageBox.Show("儲存成功！");
    }
    else
    {
        MessageBox.Show("儲存失敗：" + result.Message);
    }
}
```

---

## 🚀 遷移建議

### 1. 逐步遷移策略
```csharp
// 第一階段：保持完全相容
public class DriverV2_Cls : BusinessBaseV2_0_0_Cls
{
    // 實現必要的abstract properties
    // 使用原有的Save()方法，V2.0.0內部會自動處理Transaction管理
}

// 第二階段：增加驗證功能
protected override bool ValidateEntity()
{
    // 加入自訂驗證邏輯
}

// 第三階段：使用進階功能
protected override string BeforeSave(DBSetting dbSetting) { }
protected override string AfterSave(DBSetting dbSetting) { }

// 第四階段：導入非同步功能
public async Task SaveWithProgressAsync()
{
    var result = await BusinessObject.SaveAsync();
}
```

### 2. 相容性確保
- ✅ **UI Form代碼無需修改**：所有現有的`BusinessObject.Save()`調用都能正常工作
- ✅ **DBSetting處理**：V2.0.0內部使用`myDBSetting.StartTransaction()`，保持原有模式
- ✅ **屬性系統**：MyAttributesAttribute系統完全相容
- ✅ **Detail關係**：Detail類別關係完全保持

### 3. 新功能採用
- 🆕 **內建驗證**：可選擇性使用`ValidateEntity()`
- 🆕 **保存生命週期**：可選擇性Override`BeforeSave()`/`AfterSave()`
- 🆕 **非同步支援**：可選擇性使用`SaveAsync()`/`LoadAsync()`
- 🆕 **DI支援**：可選擇性使用依賴注入服務

---

## 📝 總結

BusinessBaseV2_0_0_Cls提供了：

1. **完全向後相容**：現有UI Form代碼無需修改
2. **增強功能**：驗證、生命週期掛鉤、非同步支援
3. **現代化架構**：DI支援、錯誤處理、模式增強
4. **漸進式採用**：可以逐步引入新功能，無需一次性重寫

⭐ **關鍵優勢**：你可以將現有的`BusinessBase_Cls`直接替換為`BusinessBaseV2_0_0_Cls`，**UI Form的代碼完全不需要修改**，同時獲得所有V2.0.0的增強功能。