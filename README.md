# Desktop Calendar Tool

## 📌 簡介
**Desktop Calendar Tool** 是一款 Windows 小工具，能夠連接 **Google 日曆**，顯示當日行程，並自動每 5 秒更新。

- 🖥 **懸浮視窗**：可設定為置頂，隨時查看今日行程
- 🎨 **無邊框設計**：支援拖曳移動，提升使用體驗
- 🔄 **自動更新**：每 5 秒同步 Google 日曆的變更
- 📅 **行程詳情**：點擊行程可顯示詳細資訊

---

## 🚀 安裝與使用
### **1. 下載應用程式**

1. 前往 [GitHub Releases](https://github.com/YOUR-REPO/releases) 下載最新版本的 `.zip`
2. 解壓縮 `.zip` 並執行 `Desktop_Calendar_Tool.exe`

**⚠️ 請注意！**
> **此應用程式需要 Google API 憑證 (`credentials.json`) 才能存取你的 Google 日曆**

### **2. 建立 Google API 憑證**

1. 前往 [Google Cloud Console](https://console.cloud.google.com/)
2. **建立專案**：
   - 選擇 `建立專案` > 命名（如 `Calendar API Project`）
3. **啟用 Google 日曆 API**：
   - 左側選單 > `API 和服務` > `啟用 API 和服務`
   - 搜尋 `Google Calendar API`，並點擊 `啟用`
4. **建立 OAuth 憑證**：
   - `憑證` > `建立憑證` > `OAuth 用戶端 ID`
   - `應用程式類型` 選擇 `桌面應用程式`
   - `建立` 後，點擊 `下載 JSON`，儲存為 `credentials.json`
5. **將 `credentials.json` 放到應用程式資料夾內**

### **3.執行應用程式**
1. 確保 `credentials.json` 存放在 `Desktop_Calendar_Tool.exe` 的同一資料夾內
2. **首次啟動時**，會彈出 Google 驗證視窗，請選擇你的 Google 帳戶並授權存取
3. 成功授權後，視窗將顯示 `今日行程`

---

## ⚙️ 設定
| 設定 | 說明 |
|------|------|
| **置頂模式** | 設定視窗永遠顯示在最上層 |
| **透明度調整** | 設定視窗透明度，默認為 `85%` |
| **自動更新** | 每 `5 秒` 重新載入行程 |

---

## 🛠 技術細節
- **開發語言**：C#
- **框架**：WinForms
- **Google API**：Google Calendar API
- **授權方式**：OAuth 2.0

---

## 📜 開源許可
本專案採用 **MIT License**，你可以自由使用、修改、發佈，但需保留版權聲明。

---

## 🤝 貢獻
歡迎貢獻！如果你有任何建議或改進，請提交 Issue 或 Pull Request。

📧 聯絡方式：`your-email@example.com`

