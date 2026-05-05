# Service Contract + Service Item — Demo Guide
# 服务合约 + 服务项目 — 演示手册

*English + 中文双语版. For the ATP plugin. Explains the two forms — what each is for, why every field exists, how they link up, and how a photocopier company actually uses them day-to-day.*

*ATP 插件. 解释这两个表单的作用、每个字段为什么存在、它们如何关联, 以及影印机公司实际的操作流程.*

---

## 1. The business context / 业务背景

**English:** A photocopier company's revenue model is roughly:

**中文:** 影印机公司的收入来源主要是:

1. **Sell or rent** physical photocopier machines to customers.
   **卖机或租机** 给客户.
2. **Sign a service contract** — "I'll maintain your machine, supply toner/drum/paper, and charge per page (meter)".
   **签服务合约** — "我帮你保养机器、供应碳粉/硒鼓/纸, 按张数 (抄表) 收费".
3. **Read the meter** every month and invoice for clicks used (BK / COLOR) + spare parts outside the contract.
   **每月抄表** 按使用张数开发票 (黑白 / 彩色) + 合约外的零件.
4. **Schedule preventive maintenance (PM)** — visit every 3 months to clean/swap parts before things break.
   **安排预防性保养 (PM)** — 每 3 个月上门清洁/更换零件, 防患未然.

Two entities / 两个核心实体:

| Entity 实体 | What it is 代表什么 | Real-world analogue 现实对应 |
|---|---|---|
| **Service Item** 服务项目 | One physical photocopier unit at a customer site / 一台实体影印机 | Asset tag / serial number 资产标签 / 序列号 |
| **Service Contract** 服务合约 | One commercial agreement covering one or more Service Items / 一份涵盖多台机器的商业合约 | Signed contract document 签字的合约文件 |

Every other form (Service Note, Appointment, Meter Trans, Spare Parts) is a transaction against one of these two masters.
所有其他表单 (服务单、预约、抄表、零件) 都是针对这两个主档的交易.

---

## 2. How they relate / 关系图

```
         Debtor (customer / 客户, AutoCount AR account)
             │
             │ 1 debtor -> many contracts / 一个客户可以有多份合约
             ▼
      ┌──────────────────┐
      │ Service Contract │  1 contract, 1–3 year term, fixed pricing
      │  服务合约         │  一份合约, 1–3 年合约期, 固定价格
      │  SC-000123       │
      └────────┬─────────┘
               │
               │ 1 contract -> many items / 一份合约涵盖多台机器
               │ (via zSCP_ServiceContractSVI bridge table)
               ▼
      ┌──────────────────┐          ┌──────────────────┐
      │  Service Item    │──────────│  Service Item    │   ... many per contract
      │  服务项目          │          │  服务项目          │       一份合约多台机器
      │  SI-000001       │          │  SI-000002       │
      └────────┬─────────┘          └──────────────────┘
               │
               │ 1 item -> many meter types / 一台机器有多个表 (BK / COLOR / RENTAL)
               ▼
      ┌──────────────────┐
      │   Meter Type     │   BK COPY, COLOR, MONTHLY RENTAL
      │   表类型           │   黑白影印 / 彩色影印 / 月租
      │   @ $0.0285/pg   │
      └────────┬─────────┘
               │
               │ many readings over time / 累积多次读数
               ▼
      ┌──────────────────┐
      │  Meter Trans     │   "On 26/3/2025 machine hit 55,625 clicks"
      │  抄表记录          │   "2025/3/26 机器累计 55,625 张"
      └──────────────────┘
```

**Key rule to remember / 记住这条关键规则:**

EN: A Service Item is the *thing* (hardware); a Service Contract is the *deal* (money). One deal can cover many things, and one thing can move through many deals over its lifetime.

中: Service Item 是 *物件* (硬件); Service Contract 是 *交易* (合约钱). 一份合约可以涵盖多台机器; 一台机器一生可能经历多份合约.

---

## 3. Service Item / 维护服务项目

### 3.1 Purpose / 作用

**EN:** The **single source of truth for one physical copier** — its tag, model, current owner, warranty/service period, PM schedule, meter configuration, and the paper trail of who owned it and when it was last serviced.

**中:** **一台实体影印机的唯一真实档案** — 包括资产标签、型号、当前拥有者、保修/服务期、PM 保养计划、抄表配置、历代拥有者、上次保养日期等完整记录.

### 3.2 Lifecycle / 生命周期

1. **Created when purchased** / **购入时建档**
   Sales team creates the Service Item after delivery.
   销售团队交机后建立 Service Item.
2. **Linked to a Service Contract** / **绑定服务合约**
   Once the customer signs the contract.
   客户签约后绑定.
3. **Meters read monthly** / **每月抄表**
   Meter Trans rows accumulate.
   累积抄表记录.
4. **PM visits on schedule** / **定期 PM 保养**
   Every 3 months by default. 默认每 3 个月一次.
5. **Ownership transfer** / **转手过户**
   Reset Debtor Ownership → archives old debtor, switches owner.
   "Reset Debtor Ownership" 按钮 → 归档旧客户, 切换新客户.
6. **Retirement** / **报废**
   Set `Inactive = Y` (soft delete; history preserved).
   设 `Inactive = Y` (软删除, 历史保留).

### 3.3 Field-by-field / 每个字段为什么存在

#### Header — left column / 表头左栏

| Field 字段 | DB column 资料库栏位 | Why it exists 为什么需要 |
|---|---|---|
| **Purchases Date** 购入日期 | `PurchaseDate` | Warranty countdown starts here. Also baseline for first PM. / 保修起算日. 也是首次 PM 的基准. |
| **Inactive** 停用 | `Inactive` (Y/N) | Soft-delete flag. Never hard-delete — need audit trail. / 软删除. 不直接删除, 保留审计轨迹. |
| **Service Tag** 服务标签 | `ServiceItemCode` | Unique asset ID. Shown on a physical sticker on the machine. / 唯一资产编号. 机器上会贴这个标签. Auto (`SI-000001`) or manual (`XXCSSI 00001632.C`). |
| **Auto (F12)** 自动编号 | — | One-click next running number. / 一键自动派下一个流水号. |
| **Stock Code** 存货代号 | `StockCode` | Links to AutoCount `Item` master. Tells you the model. / 对应 AutoCount 存货主档. 告诉你是哪个型号. (e.g., `iR-ADV DX C5860I`). |
| *(right of Stock)* | — | Auto-fills the item Description so user can confirm correct model. / 自动填入存货名称 供使用者确认选对型号. |
| **Debtor Code** 客户代号 | `DebtorCode` | Current customer. Links to AutoCount `Debtor` master. / 当前客户. 对应 AutoCount 客户主档. |
| *(right of Debtor)* | — | Auto-fills company name. / 自动填入公司名称. |
| **Agent Code** 业务员 | `StaffCode` | Sales rep — used for commission reports. / 销售员 — 用于佣金报表. |
| **Term** 付款条件 | `TermCode` | Payment terms (30 / 60 days). / 付款期 (30 天 / 60 天). |
| **Reference No** 参考号 | `RefNo` | External doc reference (DO / PO number, tender). / 外部单据号 (送货单 / 采购单号 / 标书). |
| **Area** 地区 | `AreaCode` | Geographic zone — for technician routing & regional reports. / 地理区域 — 用于技术员派工和区域报表. |
| **Grade Code** 级别 | `ServiceItemGradeCode` | Condition tier: A / B / C / REFURB. Affects warranty + pricing. / 成色: A (新) / B (次新) / C (经济) / REFURB (翻新). 影响保修和定价. |
| **Unit Price** 单价 | `UnitPrice` | What the customer paid (sale) OR monthly rental rate. / 客户支付的金额 (买机) 或 月租金. |
| **Description** 描述 | `Description` | Free-text model name / marketing description. / 型号描述 / 销售文案. |

#### Header — right column / 表头右栏

| Field 字段 | DB column 栏位 | Why 为什么 |
|---|---|---|
| **Contract No** 合约号 | `ContractNo` | Which Service Contract covers this machine. / 这台机器属于哪份服务合约. |
| **Reset Contract** 解除合约 | — | Button to detach from contract (term end, transfer). / 按钮 — 从合约中脱离 (合约到期 / 转移). |
| **Service Start / To** 服务起始/结束 | `ServiceStartDate`, `ServiceExpiryDate` | Warranty / service period. Status bar shows remaining days. / 服务期. 状态栏显示剩余天数. |
| **Address** 地址 | `Address1-4` | Where the machine physically lives. / 机器实际摆放地点. Technician comes *here*, not to billing address. 技术员到这里, 不是账单地址. |
| **Reset Debtor Ownership** 重置客户 | — | Triggers ownership transfer workflow. / 启动过户流程. Old debtor → history, new debtor → active. 旧客户归档, 新客户生效. |
| **Attention** 联络人 | `Attention` | On-site contact (receptionist, IT admin). / 现场联络人 (前台 / IT). |

#### Tab 1 — Main / 主要

**Preventive Maintenance group / 预防性保养组:**

| Field 字段 | DB column | Why 为什么 |
|---|---|---|
| **Active Preventive Maintenance** 启用 PM | `PMActive` | Toggle PM on/off (some customers opt out). / 开/关 PM (有些客户不需要). |
| **Interval Type** 间隔类型 | `PMIntervalType` | NONE / DAILY / WEEKLY / MONTHLY / YEARLY |
| **Interval Value** 间隔值 | `PMIntervalValue` | Every N units. / 每 N 单位. (e.g., 3 MONTHLY = 每 3 个月) |
| **Start Date** 起始日 | `PMStartDate` | PM schedule baseline. / PM 计划起算日. |
| **Last Service Date** 上次保养 | `PMLastServiceDate` | Updated every PM visit. / 每次保养后更新. |
| **Next Service Date** 下次保养 | `PMNextServiceDate` | When the next visit is due. Dashboard alerts feed off this. / 下次保养日期. 仪表板警报就是抓这个. |

**Right block / 右侧 — Department / Job / Location:**
AutoCount cost-center dimensions. Used by accounting for reporting (which department / project / warehouse the revenue belongs to).
AutoCount 成本中心维度. 会计用来报表分类 (收入归哪个部门 / 项目 / 仓库).

#### Tab 2 — More Header / 更多表头

**EN:** Everything secondary that clutters the main tab.
**中:** 次要但重要的资料, 避免塞爆主表头:

- **City / Postal Code / State / Country / Phone / Fax** — full site address beyond the 4 address lines / 完整的现场地址.
- **Ref 1–4** — custom reference slots (tender no, project code) / 自定义参考栏 (标书号 / 项目编号).
- **Delivery Address group** 送货地址组 — where spare parts ship, where crews visit. Often different from main address. / 零件送达地址、技术员上门地址. 通常和主地址不同 (总部账单 vs 分行现场).

#### Tab 3 — Note / 备注

Free-form operational notes (e.g., "customer only accepts visits Tue/Thu", "door access code: 1234").
自由备注 (例如: "客户只接受星期二/四上门", "门禁码: 1234").

#### Tab 4 — Remarks / 批注

4 short labelled comment slots — quicker to scan than one big note field.
4 个有标签的短批注栏 — 比一大段 Note 更易扫读.

#### Tab 5 — Service Note History *(read-only)* / 服务单历史 *(只读)*

Every service job done on this machine. Click a row → see what was fixed, by whom, when.
这台机器做过的每一次服务. 点选行 → 查看修了什么, 谁修的, 何时修的.

#### Tab 6 — Debtors Ownership History *(read-only)* / 客户过户历史 *(只读)*

Every time this machine changed hands. Critical for disputes — "I didn't own it then, that meter read isn't mine!"
这台机器的每次过户记录. 发生争议时很关键 — "那时候机器不是我的, 那张帐单不归我!"

#### Tab 7 — Meter Type *(the money tab)* / 表类型 *(计费核心)*

**EN:** One row per meter. A color copier has 2 meters (BK + COLOR) and usually a monthly "rental" meter too. Each row defines how that meter is billed.

**中:** 每个表一行. 一台彩色机通常有 2 个表 (黑白 + 彩色) 再加一个月租表. 每行定义这个表如何计费.

| Column 栏 | Meaning 意义 |
|---|---|
| **Meter Type** 表类型 | Code from meter-type master. / 表类型代号 (e.g., `201-BK COPY+PRINT`). |
| **Meter Type Name** 表名 | Readable description (joined for display). / 可读描述 (连接查询). |
| **Multi Price Code** 多段价格 | Tiered pricing. / 分段计价 — e.g., 0–5000 @ $0.02, 5000+ @ $0.015. |
| **Charges Rate** 单价 | Flat $ per click (if no multi-price). / 每张单价 (无分段时). |
| **Min Charges** 最低收费 | Floor per billing period. / 每期最低消费 (e.g., 月最低 $50). |
| **Free Qty** 免费数 | Bundled free clicks before charging. / 包含的免费张数. |
| **Rebate %** 回扣 % | Discount off the rate (VIP, volume). / 折扣 (VIP / 大量合约). |
| **Initial Meter** 初始读数 | Starting reading when installed. / 安装时的起始读数. Usage = current − initial. |
| **Last Read Date** 上次抄表 | When last read. / 上次抄表日期. |
| **Last Read** 上次读数 | Most recent reading value. / 最近一次读数值. |

Buttons / 按钮: **Add** 新增 / **Edit** 编辑 / **Delete** 删除 / **Transaction** 抄表 (opens meter-reading entry dialog / 开启抄表表单).

---

## 4. Service Contract / 服务合约

### 4.1 Purpose / 作用

**EN:** The **signed commercial agreement** — defines coverage, price, term, and spare parts included for one or more Service Items.

**中:** **签字生效的商业合约** — 定义涵盖范围、价格、合约期, 以及包含的零件 — 一份合约涵盖一台或多台 Service Items.

### 4.2 Lifecycle / 生命周期

1. **Sales signs the deal** / **销售签单**
   Create Service Contract. / 建立服务合约.
2. **Link machines** / **绑定机器**
   Add rows to Tab "Service Items" — one per covered machine. / 在 Tab "Service Items" 中每行一台机器.
3. **List covered parts** / **列出包含零件**
   Tab "Spare Parts" — with qty or "Unlimited" flag. / Tab "Spare Parts" — 填数量或标 "无限".
4. **During term** / **合约期内**
   Service jobs & meter readings billed at contract rates. / 服务单和抄表按合约价计费.
5. **Term ends** / **到期**
   Renew (new contract, same items) or mark `Done = Y`. / 续约 (新合约, 相同机器) 或标 `Done = Y`.

### 4.3 Field-by-field / 每个字段

#### Header / 表头

| Field 字段 | DB column | Why 为什么 |
|---|---|---|
| **Contract No** 合约号 | `ServiceContractCode` | Unique business code. Auto (F12) assigns next `SC-000000`. / 唯一合约编号. Auto (F12) 自动派下一个 `SC-000000`. |
| **Inactive** 停用 | `Inactive` | Soft-delete. / 软删除. |
| **Contract Type** 合约类型 | `ServiceContractTypeCode` | Warranty / Rental / Maintenance / Support. / 保修 / 租赁 / 保养 / 支援. Drives reports and coverage rules. 决定报表分类和涵盖规则. |
| **Contract Date** 合约日期 | `ServiceContractDate` | Day signed. / 签约日. |
| **Service Start / To** 服务起始 / 结束 | `ServiceStartDate`, `ServiceExpiryDate` | Coverage window. / 合约有效期. |
| **Debtor Code** 客户代号 | `DebtorCode` | The customer. / 客户. |
| **Agent Code / Term** 业务员 / 付款条件 | `StaffCode`, `TermCode` | Sales rep, payment terms. / 销售员, 付款期. |
| **Area** 地区 | `AreaCode` | Geographic zone. / 地区. |
| **Description** 描述 | `Description` | Plain-language summary. / 合约摘要 ("Annual PM + Toner Supply" = "年度 PM + 碳粉供应"). |
| **Contract Amount** 合约金额 | `ServiceContractValue` | Total contract value (for reporting). / 合约总金额 (报表用). |
| **Address / Attention** 地址 / 联络人 | `Address1-4`, `Attention` | Billing address + contact. / 账单地址 + 联络人. |

#### Tab — Spare Parts / 零件 (`zSCP_ServiceContractDTL`)

**EN:** Every spare part / consumable covered under the contract.
**中:** 合约涵盖的每个零件 / 耗材.

| Column 栏 | Meaning 意义 |
|---|---|
| **No** 行号 | Line number. / 行号. |
| **Item Code** 存货代号 | From AutoCount `Item` master. / 对应存货主档. |
| **Description** 描述 | Auto-filled from item. / 自动带出. |
| **Unlimited** 无限量 | If true, no qty cap. / 勾选表示不限量 (toner 常见). Common for toner. |
| **UOM / Quantity** 单位 / 数量 | Unit + bundled qty. / 单位 + 包含数量. (e.g., 12 bottles of toner / 12 瓶碳粉) |
| **Unit Price / Discount / Amount** 单价 / 折扣 / 金额 | Pricing for over-quota or billable items. / 超量或额外计费时的定价. |
| **Tax Type / Tax % / Tax Amount / After Tax** 税类型 / 税率 / 税额 / 税后 | Tax handling. / 税务处理. |

Buttons: **Add** 新增 / **Remove** 删除 / **Move Up/Down** 上下移 (order matters for printouts / 顺序影响打印).

#### Tab — Service Items / 服务项目 (`zSCP_ServiceContractSVI`)

**EN:** Which specific machines are covered by this contract. Auto-fills Stock Code / Stock Name / Grade / Unit Price / Dept / Location / Ref No from the Service Item master.

**中:** 这份合约涵盖哪些具体机器. 自动带出 StockCode / StockName / Grade / UnitPrice / 部门 / 位置 / RefNo (来自 Service Item 主档).

**This is the crucial bridge** — it's how the contract knows which machines it covers and vice versa.
**这是关键桥接表** — 告诉合约它涵盖哪些机器, 反之亦然.

#### Tab — More Header / 更多表头

Same pattern as Service Item — full site address + delivery address + Ref 1-4.
和 Service Item 相同模式 — 完整现场地址 + 送货地址 + Ref 1-4.

#### Tab — Note / Remarks 备注 / 批注

Free-text operational notes.
自由备注文字.

---

## 5. How they work together — full flow / 运作流程

### Scenario 场景: Customer "Arena Stabil" buys 3 copiers with 2-year contract / 客户 "Arena Stabil" 买 3 台机器并签 2 年合约

```
Day 1 — Sales sells 3 machines / 销售卖出 3 台机器
  Create 3 Service Items / 建立 3 个服务项目:
    SI-000101, SI-000102, SI-000103
    - Each with unique Service Tag / 每台独立 Service Tag
    - StockCode = iR-ADV DX C5860I
    - DebtorCode = 3000-A0084  (Arena Stabil)
    - PurchaseDate = today / 今日
    - UnitPrice = 12,000 each / 每台 12,000
    - Tab 7: BK @ $0.0285, COLOR @ $0.0285, RENTAL @ $1,185/mo

Day 2 — Customer signs contract / 客户签合约
  Create 1 Service Contract / 建立 1 份服务合约:
    SC-000042
    - Type = "RENTAL" / 类型: 租赁
    - Debtor = 3000-A0084
    - Start = today, End = today + 2 years / 2 年合约期
    - Value = 85,000 / 合约总值

  Tab "Service Items" add 3 rows / 加 3 行:
    Pos 1 → SI-000101
    Pos 2 → SI-000102
    Pos 3 → SI-000103
  (This is zSCP_ServiceContractSVI / 即为桥接表)

  Tab "Spare Parts" list covered items / 列出包含零件:
    Toner BK    (Unlimited / 无限)
    Toner COLOR (Unlimited / 无限)
    Drum Unit   (qty 2/year / 一年 2 个)

  Back on each Service Item, set ContractNo = SC-000042
  回到每个 Service Item, 设 ContractNo = SC-000042

Day 30 — First meter reading / 第一次抄表
  Technician visits, Tab 7 → "Transaction" button
  技术员上门, Tab 7 → 按 "Transaction":
    SI-000101:  BK = 1,200  clicks, COLOR = 340 clicks
    SI-000102:  BK =   800  clicks, COLOR = 150 clicks
    SI-000103:  BK = 2,100  clicks, COLOR = 500 clicks

  System bills / 系统计费:
    BK:    (1200 + 800 + 2100) × $0.0285 = $117.15
    COLOR: (340  + 150 + 500)  × $0.0285 = $28.11
    + Monthly Rental / 月租 $1,185 × 3 = $3,555
    = Invoice $3,700.26

Day 90 — PM visit due / PM 保养到期
  System alerts (PMNextServiceDate = today) / 系统警报提示
  Technician visits, changes filters / 技术员上门, 更换滤网
  Updates LastServiceDate + NextServiceDate / 更新日期字段
  Creates a Service Note / 建立服务单
  Tab 5 "Service Note History" gains a new row / 历史增加一行

Day 365 — Customer returns 1 machine / 客户退回 1 台机器
  Click "Reset Debtor Ownership" on SI-000103
  点 SI-000103 的 "Reset Debtor Ownership"
  Archives Arena Stabil → Tab 6 history / 归档到 Tab 6
  New debtor or mark Inactive / 新客户 或 标 Inactive

Day 730 — Contract expires / 合约到期
  Mark SC-000042 as Done / 标 SC-000042 为 Done
  Renew or unlink / 续约 或 解除绑定 (Reset Contract)
```

---

## 6. Demo script / 演示流程

Suggested 18-minute walkthrough / 建议 18 分钟演示流程:

### Part A — "What's a Service Item?" / "什么是服务项目?"  *(5 min / 5 分钟)*

1. Open **Maintain Service Item** via ShadowMain.
   以 ShadowMain 开启 **Maintain Service Item**.

2. Click **Fill Test Data** → every field pre-populated. Walk through:
   按 **Fill Test Data** → 所有字段自动填入. 依序解释:

   - Service Tag → "Each machine has a unique tag printed on a sticker on the device."
     "每台机器有唯一标签, 贴在机身上."
   - Stock Code → "Which model, from the main inventory."
     "对应存货主档中的型号."
   - Debtor Code → "Who currently owns/rents it."
     "当前拥有 / 租用客户."
   - Agent Code → "Which salesperson, for commission."
     "哪位销售员, 用于佣金."
   - Service Start / End → "Coverage period. Status bar shows days remaining."
     "服务期. 状态栏显示剩余天数."

3. Click **Tab 1 Main** → "Preventive Maintenance lives here. Every 3 months we visit."
   点 **Tab 1 Main** → "预防性保养设定在这. 默认每 3 个月上门."

4. Click **Tab 7 Meter Type** → "This is where the money is — every meter gets a rate. One machine can have BK, COLOR, and RENTAL meters side by side."
   点 **Tab 7 Meter Type** → "这里是收钱核心 — 每个表各自单价. 一台机器可以同时有黑白 / 彩色 / 月租 三个表."

5. Click **Tab 6 Debtor Ownership History** → "When a machine changes hands, the old owner goes here. Audit trail."
   点 **Tab 6** → "机器过户时, 旧客户记录在这. 审计轨迹."

### Part B — "What's a Service Contract?" / "什么是服务合约?"  *(5 min / 5 分钟)*

1. Open **Maintain Service Contract**. / 开启 **Maintain Service Contract**.
2. Click **Fill Test Data**. / 按 **Fill Test Data**.
3. "Contract covers *multiple* machines under one commercial agreement."
   "一份合约涵盖多台机器, 是一个商业交易."
4. **Tab Service Items** → "Which machines are covered — one row per Service Item Code."
   **Tab Service Items** → "涵盖哪些机器 — 每行一台机器代号."
5. **Tab Spare Parts** → "What's covered in the price — toner unlimited, drum 2/year. Anything outside this list is billed separately."
   **Tab Spare Parts** → "合约内包含什么 — 碳粉无限, 硒鼓一年 2 个. 清单外的东西另外收费."

### Part C — "How they link" / "两者如何关联"  *(3 min / 3 分钟)*

1. Back on Service Item, point at **Contract No** → "This is the link. One item belongs to one contract at a time."
   回到 Service Item, 指 **Contract No** → "这就是关联. 同一时间一台机器属于一份合约."

2. Show the cascade: delete a contract → service items get unlinked.
   演示级联: 删合约 → 机器被解除绑定.

3. "The relationship is not symmetric — a contract dies with the deal, but machines live longer (multiple contracts over their lifetime)."
   "关系不对称 — 合约随交易结束, 但机器寿命更长 (一生可以签多份合约)."

### Part D — "Full CRUD cycle" / "完整 CRUD 流程"  *(3 min / 3 分钟)*

1. **Add (F5)** → fresh record, Service Tag auto-picked. / 空白新单, Service Tag 自动派.
2. **Fill Test Data** → everything populates. / 全部填入.
3. **Save (F7)** → transactional commit, form switches to **View** (Service Tag locked). / 交易式储存, 切换至 View 模式 (Tag 锁定).
4. **Edit (F6)** → fields unlock. Change Description. / 字段解锁, 修改描述.
5. **Cancel (F8)** → "Discard?" prompt → reverts. / 提示 "放弃?" → 还原.
6. **Edit** again → change → **Save** → audit columns advance (Modified, ModifiedBy). / 再次编辑 → 储存 → 审计字段更新.
7. **Delete (F9)** → parent + all children cascade in one transaction. / 主档 + 子档一次性级联删除.

### Part E — "Headless testing" / "无界面测试"  *(2 min / 2 分钟)*

1. Open terminal, run `test-crud.bat` → 52 automated tests run in seconds.
   开启终端, 执行 `test-crud.bat` → 52 个自动测试几秒跑完.

2. "AI or QA can run this anytime to verify nothing regresses."
   "AI 或 QA 任何时候都能跑这个验证回归问题."

3. Open `tests/crud-suite.md` → show the test catalog.
   开启 `tests/crud-suite.md` → 展示测试目录.

---

## 7. FAQ / 常见问题

**Q: Why not just one table "contract with meter rates"? / 为什么不用一张表"合约带抄表价格"?**

EN: Because one contract covers many machines, and each machine can have its own meter rates. Splitting keeps the model flexible.

中: 因为一份合约涵盖多台机器, 而每台机器有自己的抄表价格. 分表保留模型弹性.

---

**Q: Why do we need `PMNextServiceDate` if we have Last + Interval? / 有 Last + Interval 为何还需要 NextServiceDate?**

EN: Could be computed, but storing it explicitly lets dashboards query directly ("show me all machines due for PM this week") without recomputing for every row.

中: 可以计算出来, 但独立储存让仪表板能直接查询 ("列出本周要保养的所有机器"), 不需为每行重算.

---

**Q: Why `Inactive` instead of `Delete`? / 为什么用 `Inactive` 而不是直接删?**

EN: Regulatory + audit. A tax auditor 5 years later needs to see history. Soft-delete = history preserved.

中: 法规 + 审计需求. 5 年后税务审计要看历史. 软删除保留历史记录.

---

**Q: What if a machine moves to another customer? / 机器转给另一个客户怎么办?**

EN: "Reset Debtor Ownership" button — archives old debtor to Tab 6, opens dialog to set new debtor. Old meter readings stay attached to the machine, not the customer.

中: 按 "Reset Debtor Ownership" — 归档旧客户到 Tab 6, 开启对话框设新客户. 旧的抄表记录跟着机器, 不跟客户.

---

**Q: What if a customer has 50 machines? / 客户有 50 台机器怎么处理?**

EN: One Service Contract, 50 rows in Tab "Service Items". The contract itself stays a single doc.

中: 一份服务合约, Tab "Service Items" 里 50 行. 合约本身还是一份文件.

---

**Q: Where do meter readings get entered? / 抄表在哪里输入?**

EN: Tab 7 Meter Type → click **Transaction** button → opens meter-reading entry form (one row per reading per date). These accumulate in `zSCP_MeterTrans`.

中: Tab 7 Meter Type → 按 **Transaction** 按钮 → 开启抄表输入表单 (每个日期一行读数). 累积在 `zSCP_MeterTrans`.

---

**Q: Is this the same as AutoCount V8's built-in service module? / 和 AutoCount V8 内建的服务模块一样吗?**

EN: No — AutoCount V8 has a similar module; this plugin is the ATP rebuild on AutoCount 2.x (which doesn't ship with a service module). UI + schema are deliberately aligned with V8 so V8 users feel at home.

中: 不完全一样 — V8 有类似模块, 这个插件是在 AutoCount 2.x 上重建 (2.x 没有内建服务模块). UI 和资料表刻意对齐 V8, 让熟悉 V8 的使用者上手无痛.

---

## 8. Demo checklist / 演示前检查清单

Before presenting / 演示之前:

- [ ] `atp.exe schema verify` → all OK / 全部通过
- [ ] `atp.exe item count` + `contract count` → known state / 已知状态
- [ ] ShadowMain builds clean / 编译无错 (`msbuild ATPShadowMain/ATPShadowMain.csproj`)
- [ ] `zSCP_LK_ServiceItemGrade` seeded / 已有种子数据 (A, B, C, REFURB)
- [ ] At least one row in AutoCount `Debtor` / 至少一笔客户
- [ ] At least one row in AutoCount `Item` / 至少一笔存货
- [ ] Seed `Dept` + `Project` if demo-ing those fields / 如要演示 Dept / Project 则种下
- [ ] `tests/crud-suite.sh` → all 52 green / 52 个测试全绿
- [ ] This doc open on a second screen for reference / 本文件开第二屏备用

Good luck with the demo!  加油, 演示顺利!

---

## 9. The `Item Code` vs `Service Item Code` question — deep dive / `Item Code` 与 `Service Item Code` 的差别 — 深入讲解

If anyone asks "why does the contract have **two** different 'item' codes?", here's the full answer — logical + real-world + three design options.

如果有人问"为什么合约里有**两个**不同的 item code?", 这里是完整回答 — 逻辑 + 实际 + 三个设计方案.

### 9.1 The logical answer / 逻辑答案

**EN:** They refer to fundamentally different concepts:

**中:** 它们指的是完全不同的概念:

| Code | Where (哪里) | Points to (指向) | What it is (什么) | Example (例子) |
|---|---|---|---|---|
| **Service Item Code** | Contract → Tab "Service Items" (`SVI`) | `zSCP_ServiceItem` (our plugin master) | Physical machine (asset) / 实体机器 (资产) | `SI-000101`, `XXCSSI 00001632.C` |
| **Item Code** | Contract → Tab "Spare Parts" (`DTL`) | AutoCount `Item` master (standard stock) | Spare part / consumable / SKU / 零件 / 耗材 / SKU | `TONER-BK-5860`, `201-BK COPY+PRINT`, `SC001` |

**Logically:**
- Service Item = *thing being serviced* (the copier you fix)
- Item = *thing used to service it* (the toner you put in)

**逻辑上:**
- Service Item = *被保养的物件* (那台影印机)
- Item = *保养用的东西* (你换进去的碳粉)

---

### 9.2 What V8 master DB actually shows / V8 参考资料库的真实数据

I queried the live MariaDB V8 reference (`v8_atp_main`) to confirm how the **real production system** uses these two codes. Surprising results:

我查询了活着的 MariaDB V8 参考资料库 (`v8_atp_main`), 确认**真实生产系统**怎么用这两个代码. 结果出乎意料:

| Table / 表 | Rows / 行数 | Finding / 发现 |
|---|---:|---|
| `servicecontract` | 4 | 只有 4 份合约 — 感觉像**价格模板** |
| `servicecontractserviceitem` (SVI bridge) | **0** | **完全没有使用!** Schema 有这张表但从不填 / Schema has this table but it's never populated |
| `servicecontractsparepart` (DTL) | ~13 | 混合两种含义 (见下) |
| `serviceitem` (machine master) | **3,067** | 3,067 台机器, **`contractno` 字段 100% 空白** |
| `serviceitemmetertype` | 7,144 | **真正计费的地方** — per-machine meter pricing / 每台机器的抄表单价 |
| `serviceitemmetertrans` | 145,825 | 多年累积的真实读数 / Years of actual readings |

**Concrete example — Contract `CSSC 00000004`, its Spare Parts (DTL) rows:**

**具体例子 — 合约 `CSSC 00000004` 的 Spare Parts (DTL) 内容:**

| Pos | Item No | Item Stock Code | Description | Qty | UnitPrice |
|---|---|---|---|---:|---:|
| 1 | *(blank)* | *(blank)* | **MODEL: IRADV5235I  S/N: JWF02723** | NULL | NULL |
| 2 | 1. | `201-BK C+ P` | BK COPY + PRINT A4&A3 | 1 | 0.030 |
| 3 | 2. | `223-COLOR C+ P` | COLOR COPY + PRINT A4&A3 | 1 | 0.450 |

- **Row 1 is a descriptive text line** — machine model + serial, **no ItemStockCode at all** — a note saying "this contract covers this machine".
  **第 1 行是描述性文字** — 机器型号 + 序列号, **没有 ItemStockCode** — 像备注"这合约对应哪台机器".
- **Rows 2-3 are real billable lines** — pointing to meter-type codes in AutoCount's `Item` master.
  **第 2-3 行才是真正计费** — 指向 AutoCount `Item` 主档里的抄表类型代号.

So in V8's actual usage:
- The "Service Items tab" with the SVI bridge table exists in schema but is **never populated**.
- The machine is referenced as a **descriptive text line** in the spare-parts tab.
- **All real billing** is driven by `serviceitem` + `serviceitemmetertype` + `serviceitemmetertrans`, not by the contract.

所以 V8 实际用法是:
- "Service Items tab" + SVI 桥接表存在于 schema, 但**从来没填过**.
- 机器在合约里只是**一行描述文字**放在 spare-parts 里.
- **所有真实计费**由 `serviceitem` + `serviceitemmetertype` + `serviceitemmetertrans` 驱动, 不是合约.

---

### 9.3 Three design options / 三个设计方案

Given what V8 actually does vs what the schema suggests, we have three valid approaches:

基于 V8 实际做法 vs schema 建议, 有三个可行方案:

#### Option A — Follow V8's actual pattern / 跟随 V8 实际模式

- **Remove** the "Service Items" tab from Service Contract (V8 doesn't use it anyway).
  **移除** Service Contract 的 "Service Items" tab (V8 本来就没用).
- Contract becomes a pure **rate template** / "price sheet".
  合约纯粹当作**价格模板** / "价格表".
- Billing flows: `serviceitem` → `serviceitemmetertype` (defines rates) → `serviceitemmetertrans` (records readings) → invoice.
  计费流程: `serviceitem` → `serviceitemmetertype` (定单价) → `serviceitemmetertrans` (记读数) → 开发票.
- Spare-parts tab's first row = descriptive text for the machine (like V8).
  Spare Parts 第一行 = 机器描述文字 (像 V8).

**Pros / 优点:**
- **100% V8 ergonomic parity** — users from V8 feel at home. 与 V8 操作感 100% 一致.
- Simpler schema (drop unused bridge table). 精简 schema.
- Matches the real-world data flow proven over years. 符合多年实战验证的流程.

**Cons / 缺点:**
- Less explicit linkage — have to hunt through meter-type data to know which machines a contract covers. 关联不明显 — 要从 meter-type 追查才知道合约涵盖哪些机器.
- Loses data integrity — orphan risk if machine codes drift. 失去资料完整性 — 代号变动可能脱节.

#### Option B — Keep current design (stricter than V8) / 保留现在的设计 (比 V8 严谨)

- Keep the "Service Items" tab with the SVI bridge **actively populated**.
  保留 "Service Items" tab, SVI 桥接表**实际填入**.
- Contract explicitly lists which machines it covers — via proper FK-style relationship.
  合约明确列出涵盖的机器 — 走正规外键风格.
- Spare-parts tab stays for **parts only** (no mixed machine-description rows).
  Spare Parts 只放零件 (不混机器描述行).

**Pros / 优点:**
- **Explicit, queryable** — "show me all contracts that cover machine SI-000101" is a trivial JOIN. 明确可查 — "查询涵盖 SI-000101 的所有合约" 就是一个简单 JOIN.
- Better data integrity (FK on `ServiceItemCode`). 资料完整性更好.
- Matches the visible UI that V8 users expect (the tab is there). UI 与 V8 用户预期一致.

**Cons / 缺点:**
- **Diverges from V8's actual usage pattern** — V8 users will see empty SVI data in their own DB and be confused. 与 V8 实际用法不同 — V8 用户在自己 DB 看到空的 SVI 会困惑.
- Need migration story if importing V8 data (SVI will be empty on import — need to backfill from spare-parts descriptive lines?). 资料转换需要处理 (从 V8 导入时 SVI 会是空的 — 是否要从 spare-parts 描述行反推?).

#### Option C — Hybrid (closest to current reality) / 混合方案 (最接近现状)

- Keep **both** tabs visible.
  **两个** tab 都保留.
- Spare Parts first row can be a descriptive text line for the machine (V8-style).
  Spare Parts 第一行可以是机器描述文字 (V8 风格).
- Service Items tab becomes **optional** — populate it when users want explicit linkage, leave empty when they prefer V8-style.
  Service Items tab 变**可选** — 使用者要明确关联就填, 要 V8 风格就留空.

**Pros / 优点:**
- Backward compatible with V8 data import. 与 V8 资料导入向后相容.
- Users choose their own discipline level. 使用者自己决定严谨程度.

**Cons / 缺点:**
- **Inconsistent data** — some contracts have SVI rows, some don't. Hard to write reports that work on both. 资料不一致 — 一些合约有 SVI 一些没有. 写报表很痛苦.
- Dual-path code in CRUD (save must handle both patterns). CRUD 程式要处理两种模式.

---

### 9.4 Current status in this plugin / 本插件目前状态

**Current implementation = Option C (Hybrid).** / **目前实现 = 方案 C (混合).**

Our `ServiceContract_Form` keeps both tabs. The SVI bridge table is populated when users add rows to the "Service Items" tab. The Spare Parts tab is clean (parts only). If users never fill the Service Items tab, the SVI bridge stays empty — mirroring V8 behaviour.

目前 `ServiceContract_Form` 保留两个 tab. 使用者在 "Service Items" tab 加行时 SVI 桥接表就会写入. Spare Parts tab 干净 (只放零件). 如果使用者从不填 Service Items tab, SVI 桥接表保持空 — 与 V8 行为一致.

---

### 9.5 Recommendation for the demo / 演示建议

**When asked "why both codes?" during demo:**

**demo 时被问到"为什么两个 code?" 请这样回答:**

> "They're different concepts. `Service Item Code` refers to a physical machine — one row per asset. `Item Code` refers to a stock SKU — toner, drums, consumables. Our contract can cover multiple machines (linked via Service Items tab) and include specified parts (via Spare Parts tab). The V8 reference database we mirror has the same schema but uses a looser convention where machines are sometimes just text rows in the spare-parts tab rather than linked via the Service Items bridge. Our implementation lets you be as strict or as loose as you want."

> "这两个是不同概念. `Service Item Code` 指实体机器 — 一行一个资产. `Item Code` 指存货 SKU — 碳粉、硒鼓、耗材. 我们的合约可以涵盖多台机器 (透过 Service Items tab 关联), 并包含指定零件 (透过 Spare Parts tab). 我们对齐的 V8 参考资料库有相同 schema, 但使用较宽松的惯例 — 机器有时只是 Spare Parts 里的一行文字, 不走 Service Items 桥接. 我们的实现让你可以要严谨就严谨, 要宽松就宽松."

---

### 9.6 Open question for you / 留给你决定

Do we want to keep Option C (hybrid), switch to Option A (pure V8 pattern), or Option B (stricter)? This affects:

要不要改设计? 会影响:

- What gets taught in user training / 培训材料
- Which tabs show/hide in the form / 表单上哪些 tab 显示
- How we import V8 data when customers migrate / V8 资料如何导入

**Tell me and I'll update the form + docs accordingly.**
**告诉我决定, 我就更新表单 + 文档.**

