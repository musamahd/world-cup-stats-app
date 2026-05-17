# ⚽ World Cup Statistics App

A desktop application for browsing statistics from the **2018 Men's** and **2019 Women's FIFA World Cup**, built as a university project at Algebra University (Software Engineering).

## 📸 Overview

The solution consists of three projects:
- **DataLayer** — shared class library handling API calls, JSON parsing, and file I/O
- **WinFormsApp** — Windows Forms client with player management and rankings
- **WPFApp** — WPF client with responsive UI and animated team/player views

---

## ✨ Features

### Windows Forms
- Choose tournament (men/women) and app language (HR/EN) on first launch — settings saved to file
- Select favourite team from a ComboBox (persisted between sessions)
- Player cards with name, shirt number, position, captain status and photo
- Drag & drop players between "Favourites" and "Others" panels
- Rankings: top scorers, yellow cards, match attendance
- Print rankings to PDF

### WPF
- Fully responsive layout (fullscreen + 3 window sizes)
- Visual football pitch with starting eleven in correct positions
- Animated team info window (0.5s) and player detail window (0.3s)
- Head-to-head match result display
- Shared settings with WinForms app

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# / .NET |
| UI (desktop) | Windows Forms |
| UI (responsive) | WPF (XAML) |
| Data | REST API + JSON files |
| Architecture | 3-tier (DataLayer / WinForms / WPF) |
| Async | async/await |
| Storage | Text files (settings, favourites) |

---

## 🌐 API

Data sourced from: `https://worldcup-vua.nullbit.hr`

Key endpoints used:
- `/men/teams/results` and `/women/teams/results`
- `/men/matches` and `/women/matches`
- `/men/matches/country?fifa_code=XXX`

Data source (API vs local JSON) is configurable via config file.

---

## 🚀 Getting Started

### Prerequisites
- Windows OS
- .NET 6+ SDK (or matching version)
- Visual Studio 2022 recommended

### Run
1. Clone the repo:
   ```bash
   git clone https://github.com/musa-mahd/world-cup-stats-app.git
   ```
2. Open `WorldCupStats.sln` in Visual Studio
3. Set startup project to `WinFormsApp` or `WPFApp`
4. Run with `F5`

> On first launch, you'll be prompted to select tournament and language. Settings are saved automatically.

---

## 📁 Project Structure

```
WorldCupStats/
├── DataLayer/          # Shared class library
│   ├── Models/
│   ├── Services/
│   └── Repositories/
├── WinFormsApp/        # Windows Forms UI
├── WPFApp/             # WPF UI
└── WorldCupStats.sln
```

---

## 🎓 Course

Object-Oriented Programming Practicum in .NET — Algebra University, Zagreb (2023/2024)
