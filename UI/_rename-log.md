# UI Screenshot Rename Log

**Session**: 2026-04-12 overnight full-send build
**Total originals**: 79 (4 MenuItem + 75 AnyDesk)
**Strategy**: auto-categorize based on Phase 1 explorer inventory, preserve originals in `_originals/`

## Final layout

| Folder | Count |
|---|---|
| `00-menu/` | 4 |
| `01-service-contract/` | 9 |
| `02-service-item/` | 16 |
| `03-service-note/` | 10 |
| `04-service-note-assignment/` | 2 |
| `05-service-appointment/` | 7 |
| `06-inquiry/` | 1 |
| `07-preventive-maintenance/` | 0 |
| `08-service-quick-view/` | 2 |
| `09-general-setup/` | 17 |
| `_unsorted/` | 11 |
| `_originals/` | 79 (backup) |

## Unsorted (11 files to re-categorize manually)

These 11 files were not in the Phase 1 explorer's mapping table. Probable content:
Report screens, extra dialogs, or duplicates. Open each and move to the correct folder:

- AnyDesk_I1jCq6vkfp.png
- AnyDesk_RUzAf9rMzc.png
- AnyDesk_SQQlTvgetZ.png
- AnyDesk_Ud9pJKq18t.png
- AnyDesk_ZoQTXDWcFW.png
- AnyDesk_cfdJJ25Fx5.png
- AnyDesk_dWOKI8KQ9s.png
- AnyDesk_ohRhWnPFdY.png
- AnyDesk_uSZkW66iG9.png
- AnyDesk_unMPKOh2Ph.png
- AnyDesk_xWOePxSw7b.png

## Notes

- Originals kept in `_originals/` — move back any mis-categorized files from there, not the renamed copies
- `07-preventive-maintenance/` is empty because the PM dialogs were grouped under `05-service-appointment/` in the mapping; move if desired
- `06-inquiry/` has only 1 file because most inquiries are shared between their submodule folder and this one
- Slice 2 (`/ralph-loop`) should re-read each image in the renamed folders, confirm categorization, and move anything wrong
