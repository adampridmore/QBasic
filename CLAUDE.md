# QBasic to VB.Net Conversion Project

This repository contains classic QBasic programs from the DOS era and their modern VB.Net conversions.

## Project Structure

- `/` - Original QBasic .BAS files
- `/SpinHouse/` - VB.Net Windows Forms port of SPINHOUS.BAS
- `/Editor3D/` - VB.Net Windows Forms port of EDITOR.BAS

## Original QBasic Programs

| File | Description |
|------|-------------|
| EDITOR.BAS | 3D scene editor with camera controls, object management, save/load |
| SNAKES.BAS | Classic snake game |
| SPINHOUS.BAS | Spinning 3D wireframe house visualization |

## VB.Net Projects

### SpinHouse

Spinning 3D wireframe house visualization.

```bash
cd SpinHouse
dotnet build
dotnet run
```

**Controls:**
- `Z` - Rotate left
- `X` - Rotate right
- `Space` - Stop rotation
- `K` - Move house away
- `M` - Move house closer
- `Q` / `Escape` - Quit

### Editor3D

Full 3D scene editor with camera controls, object management, and file I/O.

```bash
cd Editor3D
dotnet build
dotnet run
```

**Movement (Numpad):**
- `8` / `2` - Move forward / backward
- `4` / `6` - Strafe left / right
- `9` / `3` - Move up / down
- `5` - Reset camera position

**Rotation:**
- `<` / `>` - Turn left / right
- `'` / `/` - Look up / down
- `O` / `P` - Roll left / right

**Editor Functions:**
- `L` - Cycle object highlight
- `T` - List all objects
- `M` - Move selected object
- `D` - Delete selected object
- `C` - Change movement speed
- `S` - Save world
- `H` - Show help
- `Q` / `Escape` - Quit

**File Formats:**
- `.wld` - World files (entire scene)
- `.obj` - Individual object files

## Conversion Notes

When converting QBasic to VB.Net:
- QBasic arrays are 1-indexed; VB.Net uses 0-indexed by default
- QBasic `SCREEN` modes map to Windows Forms with GDI+ rendering
- QBasic `INKEY$` maps to KeyDown/KeyUp events
- Use double-buffering and a Timer for smooth animation
- QBasic `LINE` command maps to `Graphics.DrawLine()`
