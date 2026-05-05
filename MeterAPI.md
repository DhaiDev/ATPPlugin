# Meter API Specification (Active Devices)

## Authentication

Use a static API key passed via the `X-API-Key` header. No login/session required.

```
X-API-Key: <your-api-key>
```

## Endpoint

```
POST /api/meter.php
Content-Type: application/json
X-API-Key: <your-api-key>
```

## Request DTO (Filter)

```json
{
  "online_offline": ["Online", "Offline"],
  "from_pums": ["Yes", "No"],
  "statuses": ["Active", "Suspend"],
  "services": ["Low Paper", "Low Supplies", "Empty Supplies", "Service requested"],
  "manufacturers": ["Fuji Xerox", "Canon", "Ricoh", "HP", "Brother"],
  "code": "CSSI00002204",
  "location": "AH NEW FISHERY",
  "search": "keyword",
  "page": 1,
  "page_size": 25
}
```

### Request Field Details

| Field | Type | Required | Description |
|---|---|---|---|
| `online_offline` | `string[]` | No | `"Online"`, `"Offline"`. Omit or empty = all. |
| `from_pums` | `string[]` | No | `"Yes"`, `"No"`. Omit or empty = all. |
| `statuses` | `string[]` | No | `"Active"`, `"Suspend"`. Omit or empty = all. |
| `services` | `string[]` | No | Values: `"Low Paper"`, `"Low Supplies"`, `"Empty Supplies"`, `"Service requested"`. Omit or empty = all. |
| `manufacturers` | `string[]` | No | Values: `"Fuji Xerox"`, `"Canon"`, `"Ricoh"`, `"HP"`, `"Brother"`. Omit or empty = all. |
| `code` | `string` | No | Filter by device code. Partial match / contains search. `null` = all. |
| `location` | `string` | No | Filter by customer name or address. Partial match / contains search. `null` = all. |
| `search` | `string` | No | General keyword search (same as the Search box on the page). `null` = all. |
| `page` | `int` | No | Page number, default `1`. |
| `page_size` | `int` | No | Records per page, default `25`. |

## Response DTO

```json
{
  "total_records": 2924,
  "page": 1,
  "page_size": 25,
  "data": [
    {
      "account_header": "AH NEW FISHERY SDN BHD - 3000-A0028 (1)",
      "code": "CSSI00002204",
      "serial_number": "2JC13208",
      "model": "Canon iR-ADV C5550 III 47.09",
      "contract": "Rental",
      "location": "AH NEW FISHERY SDN BHD - NO. 2&4, JALAN SRI PURNAMA 2/2,, KANGKAR TEBRAU., JOHOR BAHRU, JOHOR",
      "zone": 1,
      "latest_audit_date": "2026-04-13 10:57:41",
      "latest_meters": {
        "bw": 68359,
        "color": 497,
        "counter_102": 68856,
        "counter_109": 68359,
        "counter_124": 497
      },
      "total_bk": 68359,
      "total_cl": 497,
      "latest_supplies": [
        "Canon C-EXV 51 Black : 75 %",
        "Canon C-EXV 51 Cyan : 100 %",
        "Canon C-EXV 51 Magenta : 100 %",
        "Canon C-EXV 51 Yellow : 100 %",
        "Waste : 20 %",
        "Black Drum Unit : 72 %",
        "Cyan Drum Unit : 64 %",
        "Magenta Drum Unit : 68 %",
        "Yellow Drum Unit : 58 %",
        "Fuser Unit : 53 %"
      ],
      "status": "Active",
      "service": "Low Paper",
      "remarks": "",
      "meter_type": "2 click A3",
      "pums_version": "2.5.8.3"
    }
  ]
}
```

### Response Field Details

| Field | Type | Description |
|---|---|---|
| `total_records` | `int` | Total matching records (for pagination) |
| `page` | `int` | Current page number |
| `page_size` | `int` | Records per page |
| `data` | `object[]` | Array of device entries |
| `data[].account_header` | `string` | Account group header (company name + account code + device count) |
| `data[].code` | `string` | Device code (e.g. `CSSI00002204`) |
| `data[].serial_number` | `string` | Device serial number |
| `data[].model` | `string` | Device model with firmware version |
| `data[].contract` | `string` | Contract type (e.g. `Rental`) |
| `data[].location` | `string` | Customer name + full address |
| `data[].zone` | `int` | Zone number |
| `data[].latest_audit_date` | `string` | Latest audit datetime `YYYY-MM-DD HH:mm:ss` |
| `data[].latest_meters` | `object` | Meter readings breakdown |
| `data[].latest_meters.bw` | `int` | B/W meter reading |
| `data[].latest_meters.color` | `int` | Color meter reading |
| `data[].latest_meters.counter_102` | `int` | Counter 102 reading |
| `data[].latest_meters.counter_109` | `int` | Counter 109 reading |
| `data[].latest_meters.counter_124` | `int` | Counter 124 reading |
| `data[].total_bk` | `int` | Total BK count |
| `data[].total_cl` | `int` | Total CL count |
| `data[].latest_supplies` | `string[]` | Array of supply levels (name : percentage) |
| `data[].status` | `string` | `Active` or `Suspend` |
| `data[].service` | `string` | Service alert (e.g. `Low Paper`, `Low Supplies`) or empty |
| `data[].remarks` | `string` | Remarks, empty string if none |
| `data[].meter_type` | `string` | Meter type (e.g. `2 click A3`) |
| `data[].pums_version` | `string` | PUMS version |

## Example cURL

```bash
curl -X POST https://your-domain.com/api/meter.php \
  -H "Content-Type: application/json" \
  -H "X-API-Key: YOUR_API_KEY_HERE" \
  -d '{
    "statuses": ["Active"],
    "manufacturers": ["Canon"],
    "services": ["Low Paper", "Low Supplies"],
    "zone": [1, 14],
    "page": 1,
    "page_size": 25
  }'
```
