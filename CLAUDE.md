# QBasic to VB.Net Conversion Project

This repository contains classic QBasic programs from the DOS era and their modern VB.Net conversions.

## Project Structure

- `/` - Original QBasic .BAS files
- `/SpinHouse/` - VB.Net Avalonia port of SPINHOUS.BAS (cross-platform)
- `/Editor3D/` - VB.Net Avalonia port of EDITOR.BAS (cross-platform)

## Original QBasic Programs

| File | Description |
|------|-------------|
| EDITOR.BAS | 3D scene editor with camera controls, object management, save/load |
| SNAKES.BAS | Classic snake game |
| SPINHOUS.BAS | Spinning 3D wireframe house visualization |

## VB.Net Projects

### SpinHouse (Cross-platform - Avalonia)

Spinning 3D wireframe house visualization. Uses [Avalonia UI](https://avaloniaui.net/) for cross-platform support (Windows, macOS, Linux).

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

### Editor3D (Cross-platform - Avalonia)

Full 3D scene editor with camera controls, object management, and file I/O. Uses [Avalonia UI](https://avaloniaui.net/) for cross-platform support.

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
- QBasic `SCREEN` modes map to custom controls with rendering
- QBasic `INKEY$` maps to KeyDown/KeyUp events
- Use a Timer and `InvalidateVisual()` for smooth animation
- QBasic `LINE` command maps to `DrawLine()` methods

### Avalonia-specific notes (SpinHouse)
- Uses `Avalonia.Media` instead of `System.Drawing`
- Custom `Control` with `Render()` override for drawing
- `DispatcherTimer` for animation loop
- `DrawingContext.DrawLine()` for line rendering
