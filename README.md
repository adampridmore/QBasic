# QBasic to VB.Net Conversion

This repository contains classic QBasic programs from the DOS era, along with modern VB.Net Windows Forms conversions.

![3D Editor Screenshot](Screenshots/2019-01-10%20(2).png)

## Original QBasic Programs

These programs were written in QBasic, a BASIC programming language that came bundled with MS-DOS and early versions of Windows.

| File | Description |
|------|-------------|
| `EDITOR.BAS` | A 3D scene editor with wireframe rendering, camera controls, and object management |
| `SNAKES.BAS` | Classic snake game |
| `SPINHOUS.BAS` | Spinning 3D wireframe house visualization |

## VB.Net Conversions

The QBasic programs have been converted to modern VB.Net Windows Forms applications targeting .NET 7.

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or later
- Windows (required for Windows Forms)

### SpinHouse

A spinning 3D wireframe house - port of `SPINHOUS.BAS`.

```bash
cd SpinHouse
dotnet run
```

**Controls:**
| Key | Action |
|-----|--------|
| Z | Rotate left |
| X | Rotate right |
| Space | Stop rotation |
| K | Move away |
| M | Move closer |
| Q / Esc | Quit |

### Editor3D

A full 3D scene editor - port of `EDITOR.BAS`.

```bash
cd Editor3D
dotnet run
```

**Camera Movement (Numpad):**
| Key | Action |
|-----|--------|
| 8 / 2 | Forward / Backward |
| 4 / 6 | Strafe left / right |
| 9 / 3 | Up / Down |
| 5 | Reset camera |

**Camera Rotation:**
| Key | Action |
|-----|--------|
| < / > | Turn left / right |
| ' / / | Look up / down |
| O / P | Roll left / right |

**Editor:**
| Key | Action |
|-----|--------|
| L | Highlight next object |
| T | List all objects |
| M | Move selected object |
| D | Delete selected object |
| S | Save world |
| H | Help |

The editor can save and load world files (`.wld`) and individual objects (`.obj`).

## Building

To build all projects:

```bash
# SpinHouse
cd SpinHouse
dotnet build

# Editor3D
cd ../Editor3D
dotnet build
```

## License

These are personal hobby projects from the DOS era, preserved and modernized for educational purposes.
