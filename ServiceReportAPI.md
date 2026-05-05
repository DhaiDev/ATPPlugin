# Service Report API Specification

## Authentication

Use a static API key passed via the `X-API-Key` header. No login/session required.

```
X-API-Key: <your-api-key>
```

- For **development**: please provide us a working API key so we can start integration immediately.
- For **production**: ATP staff will generate and manage their own API keys from your system for security purposes.

The API should return `401 Unauthorized` if the key is missing or invalid:

```json
{ "error": "Unauthorized", "message": "Invalid or missing API key" }
```

## Endpoint

```
POST /api/service-report.php
Content-Type: application/json
X-API-Key: <your-api-key>
```

## Request DTO (Filter)

```json
{
  "categories": ["RPS", "SN", "PM", "UP", "OTH", "MR", "INST"],
  "statuses": [
    "Other 1", "Other 2", "New", "Waiting Reply", "Closed", "On Hold",
    "In Progress", "Resolved", "Follow Up", "Cancel", "Appointment Made",
    "Repeat-1", "Repeat-2"
  ],
  "zones": [1, 2, 3, 5, 14],
  "parts_supplies": "No Request | Request | null",
  "date_from": "2026-04-01",
  "date_to": "2026-04-13",
  "date_field": "created_at | last_update",
  "tracking_id": "RPS-260413-016",
  "location": "TLDM TG PENGELIH",
  "last_replier": "HAZIQ",
  "technician": "Saifudin",
  "page": 1,
  "page_size": 25
}
```

### Request Field Details

| Field | Type | Required | Description |
|---|---|---|---|
| `categories` | `string[]` | No | Filter by category codes. Values: `RPS`, `SN`, `PM`, `UP`, `OTH`, `MR`, `INST`. Omit or empty array = all. |
| `statuses` | `string[]` | No | Filter by status. Values: `Other 1`, `Other 2`, `New`, `Waiting Reply`, `Closed`, `On Hold`, `In Progress`, `Resolved`, `Follow Up`, `Cancel`, `Appointment Made`, `Repeat-1`, `Repeat-2`. Omit or empty = all. |
| `zones` | `int[]` | No | Filter by zone numbers (1-20). Omit or empty = all. |
| `parts_supplies` | `string` | No | `"No Request"`, `"Request"`, or `null` for all. |
| `date_from` | `string` | No | Start date, format `YYYY-MM-DD`. |
| `date_to` | `string` | No | End date, format `YYYY-MM-DD`. |
| `date_field` | `string` | No | Which date column to filter on. `"created_at"` (default) or `"last_update"`. |
| `tracking_id` | `string` | No | Filter by tracking ID. Partial match / contains search (e.g. `"260413"` matches `RPS-260413-016`). `null` = all. |
| `location` | `string` | No | Filter by customer name or address. Partial match / contains search. `null` = all. |
| `last_replier` | `string` | No | Filter by last replier name. `null` = all. |
| `technician` | `string` | No | Filter by technician name. `null` = all. |
| `page` | `int` | No | Page number, default `1`. |
| `page_size` | `int` | No | Records per page, default `25`. |

## Response DTO

```json
{
  "total_records": 67007,
  "page": 1,
  "page_size": 25,
  "data": [
    {
      "location": "TLDM TG PENGELIH\n\nTG PENGELIH KOTA TINGGI 0 JOHOR",
      "technician": "HAZIQ",
      "issue_message": "Print Quality (Blur, Dark Line) - print get line",
      "status": "In Progress",
      "device": "IRADV DX C3830i\n\n(2XH01059)",
      "parts_supplies": "",
      "total_bk": null,
      "total_cl": null,
      "zone": 2,
      "tracking_id": "RPS-260413-016",
      "created_at": "2026-04-13 09:54:25",
      "last_update": "2026-04-13 09:54:25",
      "last_replier": "HAZIQ",
      "replies": null
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
| `data` | `object[]` | Array of service report entries |
| `data[].location` | `string` | Customer name + full address |
| `data[].technician` | `string` | Assigned technician name |
| `data[].issue_message` | `string` | Issue / message description |
| `data[].status` | `string` | Current status |
| `data[].device` | `string` | Device model + serial number in parentheses |
| `data[].parts_supplies` | `string` | Parts/supplies info, empty string if none |
| `data[].total_bk` | `int\|null` | Total BK count |
| `data[].total_cl` | `int\|null` | Total CL count |
| `data[].zone` | `int` | Zone number |
| `data[].tracking_id` | `string` | Tracking ID (e.g. `RPS-260413-016`) |
| `data[].created_at` | `string` | Created datetime `YYYY-MM-DD HH:mm:ss` |
| `data[].last_update` | `string` | Last update datetime `YYYY-MM-DD HH:mm:ss` |
| `data[].last_replier` | `string` | Name of last replier |
| `data[].replies` | `int\|null` | Reply count |

## Example cURL

```bash
curl -X POST https://your-domain.com/api/service-report.php \
  -H "Content-Type: application/json" \
  -H "X-API-Key: YOUR_API_KEY_HERE" \
  -d '{
    "categories": ["RPS"],
    "statuses": ["In Progress", "New"],
    "zones": [2, 14],
    "date_from": "2026-04-01",
    "date_to": "2026-04-13",
    "date_field": "created_at",
    "page": 1,
    "page_size": 25
  }'
```
