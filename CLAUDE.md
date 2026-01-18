# QBasic to VB.Net Conversion Project

This repository contains classic QBasic programs from the DOS era and their modern VB.Net conversions.

## Project Structure

- `/` - Original QBasic .BAS files
- `/SpinHouse/` - VB.Net Windows Forms port of SPINHOUS.BAS

## Original QBasic Programs

| File | Description |
|------|-------------|
| EDITOR.BAS | 3D scene editor with camera controls, object management, save/load |
| SNAKES.BAS | Classic snake game |
| SPINHOUS.BAS | Spinning 3D wireframe house visualization |

## Building

The VB.Net projects target .NET 7.0 on Windows:

```bash
cd SpinHouse
dotnet build
dotnet run
```

## Conversion Notes

When converting QBasic to VB.Net:
- QBasic arrays are 1-indexed; VB.Net uses 0-indexed by default
- QBasic `SCREEN` modes map to Windows Forms with GDI+ rendering
- QBasic `INKEY$` maps to KeyDown/KeyUp events
- Use double-buffering and a Timer for smooth animation
- QBasic `LINE` command maps to `Graphics.DrawLine()`
