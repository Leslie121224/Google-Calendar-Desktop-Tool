using System;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace Desktop_Calendar_Tool
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private CalendarService calendarService;
        private Events events; // 儲存今日行程
        
        public Form1()
        {
            InitializeComponent();

            this.TopMost = true;
            this.Opacity = 0.85;
            this.MouseDown += Form1_MouseDown;

            InitializeGoogleCalendar();

            // **🔹 設定 Timer**
            timer1.Interval = 5000; // 5 秒更新
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            labelDetails.Text = "請選擇行程...";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private async void InitializeGoogleCalendar()
        {
            try
            {
                string[] Scopes = { CalendarService.Scope.CalendarReadonly };
                string ApplicationName = "Desktop Calendar Tool";

                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None
                    );

                    calendarService = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });

                    await LoadTodayEvents(); // ✅ 確保 `events` 變數被初始化
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Google API 初始化失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTodayEvents()
        {
            if (calendarService == null)
                return;

            try
            {
                var request = calendarService.Events.List("primary");
                request.TimeMin = DateTime.Now.Date;
                request.TimeMax = DateTime.Now.Date.AddDays(1);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var newEvents = await request.ExecuteAsync(); // 取得新事件列表

                // **1️⃣ 記住當前選取的事件 ID，而不是標題**
                string previousSelectedEventId = listBox1.SelectedIndex >= 0 && events?.Items != null
                    ? events.Items[listBox1.SelectedIndex]?.Id
                    : null;

                events = newEvents; // 更新全域 `events`

                this.Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Clear(); // 清空舊行程
                    foreach (var eventItem in events.Items)
                    {
                        string startTime = eventItem.Start.DateTime.HasValue
                            ? eventItem.Start.DateTime.Value.ToString("HH:mm")
                            : "全天";

                        string eventSummary = $"{startTime} - {eventItem.Summary}";
                        listBox1.Items.Add(eventSummary);
                    }

                    // **2️⃣ 嘗試根據事件 ID 找回對應的選取行程**
                    if (previousSelectedEventId != null)
                    {
                        int newIndex = events.Items.ToList().FindIndex(e => e.Id == previousSelectedEventId);
                        if (newIndex >= 0) listBox1.SelectedIndex = newIndex; // ✅ 設回原本選取的行程
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("載入 Google 日曆事件失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (events == null || events.Items == null || listBox1.SelectedIndex < 0 || listBox1.SelectedIndex >= events.Items.Count)
            {
                labelDetails.Text = "無法顯示行程資訊";
                return;
            }

            var eventItem = events.Items[listBox1.SelectedIndex];
            string details = $"行程: {eventItem.Summary}\n時間: {eventItem.Start.DateTime?.ToString("HH:mm") ?? "全天"}";

            if (!string.IsNullOrEmpty(eventItem.Description))
            {
                details += $"\n詳細內容:\n{eventItem.Description}";
            }

            labelDetails.Text = details;
        }

        private async void Timer1_Tick(object sender, EventArgs e)
        {
            await LoadTodayEvents(); // 🔄 重新載入行程

            // 🔹 **確保 UI 不會取消選取**
            this.Invoke((MethodInvoker)delegate
            {
                if (listBox1.SelectedIndex == -1 && listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0; // 🔹 預設選取第一個行程
                }
            });
        }
    }
}
