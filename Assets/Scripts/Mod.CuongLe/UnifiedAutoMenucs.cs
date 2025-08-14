using Mod.CuongLe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class UnifiedAutoMenu
{
    public class SkillTrain
    {
        public string Name;
        public int Id;
        public bool AutoFlag;
    }

    public class AutoItemRef
    {
        public string Name;
        public int IdCon;
        public int Id;
        public volatile bool AutoFlag;
        public Thread Thread;
    }

    public static int ItemHeight = 20;
    public static int CheckBoxSize = 18;
    public static int BasePanelWidth = 90; // Fixed width for each column
    public static int ToggleButtonHeight = 14;
    public static bool isShowingAutoItem = true;
    private static readonly int MaxColumns = 3; // Maximum number of columns

    public static SkillTrain[] SkillTrains;
    public static AutoItemRef[] AutoItems = new AutoItemRef[]
    {
        new AutoItemRef { Name = "Cuồng Nộ", IdCon = 2754, Id = 381 },
        new AutoItemRef { Name = "Khẩu Trang", IdCon = 7149, Id = 764 },
        new AutoItemRef { Name = "Bổ Huyết", IdCon = 2755, Id = 382 },
        new AutoItemRef { Name = "Giáp Xên", IdCon = 2757, Id = 384 },
    };

    public static int[] excludedIds = new int[] { 1, 3, 5, 8, 9, 10, 11, 14, 18, 20, 22, 23, 24, 25, 26 };

    // Calculate number of columns (fixed at 3)
    public static int Columns => MaxColumns;

    // Calculate number of rows based on item count and fixed 3 columns
    public static int Rows
    {
        get
        {
            int itemCount = isShowingAutoItem ? AutoItems.Length : SkillTrains.Length;
            return (int)System.Math.Ceiling((double)itemCount / MaxColumns);
        }
    }

    // Panel width is fixed based on 3 columns
    public static int PanelWidth => BasePanelWidth * MaxColumns;

    public static int PanelHeight =>
        15 + Rows * ItemHeight + (isShowingAutoItem ? 0 : ToggleButtonHeight + 5);

    public static int PanelX => GameCanvas.w / 2 - PanelWidth / 2;
    public static int PanelY => GameCanvas.h / 3 - PanelHeight / 2;

    private static long lastClickSaveTime = 0;
    private static long lastClickResetTime = 0;
    private static int effectDuration = 300;

    static UnifiedAutoMenu()
    {
        int skillCount = Char.myCharz().vSkill.size();
        SkillTrains = new SkillTrain[skillCount];

        for (int i = 0; i < skillCount; i++)
        {
            Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);
            string name = skill.template.name;
            string display = name;

            switch (name)
            {
                case "Chiêu Kamejoko": display = "Kamejoko"; break;
                case "Chiêu đấm Dragon":
                case "Chiêu đấm Demon":
                case "Chiêu đấm Galick": display = "Đấm"; break;
                case "Chiêu Masenko": display = "Masenko"; break;
                case "Chiêu Antomic": display = "Antomic"; break;
                case "Thái Dương Hạ San": display = "TDHS"; break;
                case "Tái tạo năng lượng": display = "Tái tạo"; break;
                case "Quả cầu kênh khi": display = "kênh khi"; break;
                case "Makankosappo": display = "Laze"; break;
                case "Đẻ trứng": display = "Trứng"; break;
                case "Biến hình": display = "Khỉ"; break;
                case "Tự phát nổ": display = "Boom"; break;
                case "Biến Sôcôla": display = "Sôcôla"; break;
                case "Dịch chuyển tức thời": display = "Dịch chuyển"; break;
                case "Khiên năng lượng": display = "Khiên"; break;
                case "Cađíc liên hoàn chưởng": display = "Cađíc liên hoàn"; break;
            }

            SkillTrains[i] = new SkillTrain
            {
                Name = display,
                Id = skill.template.id,
                AutoFlag = !excludedIds.Contains(skill.template.id)
            };
        }

        string planet = Char.myCharz().getGender();
        string shieldKey = "AutoSkillConfig_Shield_" + planet;
        string skillKey = "AutoSkillConfig_" + planet;

        // Load trạng thái skill khiên
        int shieldState = Rms.loadRMSInt(shieldKey);

        // Load cấu hình các skill thường
        string config = Rms.loadRMSString(skillKey);
        if (!string.IsNullOrEmpty(shieldState.ToString()) && !string.IsNullOrEmpty(config))
        {
            LoadConfig();
        }
    }

    public static void AutoItemVIP(AutoItemRef item)
    {
        try
        {
            if (!ModProCuongLe.ExistItemBag(item.Id))
            {
                item.AutoFlag = false;
                GameScr.info1.addInfo("Item auto không tồn tại !!", 0);
                return;
            }

            GameScr.info1.addInfo("|2|Bắt đầu Auto " + item.Name + ".", 0);

            while (item.AutoFlag)
            {
                if (ItemTime.getItemTimeInSeconds(item.IdCon) <= 0)
                {
                    ModProCuongLe.useItem(item.Id);
                    Thread.Sleep(500);
                }

                int wait = ItemTime.getItemTimeInSeconds(item.IdCon);
                while (wait > 0 && item.AutoFlag)
                {
                    Thread.Sleep(1000);
                    wait--;
                    if (!ModProCuongLe.ExistItemBag(item.Id))
                    {
                        item.AutoFlag = false;
                        GameScr.info1.addInfo("|4|Đã hết " + item.Name + " để Auto !", 0);
                        break;
                    }
                }
            }
        }
        finally
        {
            item.Thread = null;
        }
    }

    public static void Paint(mGraphics g)
    {
        int itemCount = isShowingAutoItem ? AutoItems.Length : SkillTrains.Length;
        int headerHeight = 22;
        int totalHeight = headerHeight + Rows * ItemHeight + (isShowingAutoItem ? 0 : ToggleButtonHeight + 5);

        int panelX = PanelX;
        int panelY = PanelY;
        int width = PanelWidth;

        g.setColor(0xF1F1F1);
        g.fillRoundRect(panelX, panelY, width, totalHeight, 10, 10);
        g.setColor(0x666666);
        g.drawRoundRect(panelX, panelY, width, totalHeight, 10, 10);

        // Draw header spanning the entire width
        int halfWidth = width / 2;
        g.setColor(isShowingAutoItem ? 0x3399FF : 0xCCCCCC);
        g.fillRect(panelX, panelY, halfWidth, headerHeight);
        mFont.tahoma_7b_white.drawString(g, "Auto Item", panelX + (halfWidth - mFont.tahoma_7b_white.getWidth("Auto Item")) / 2, panelY + 3, 0);

        g.setColor(!isShowingAutoItem ? 0x3399FF : 0xCCCCCC);
        g.fillRect(panelX + halfWidth, panelY, halfWidth, headerHeight);
        mFont.tahoma_7b_white.drawString(g, "Skill Train", panelX + halfWidth + (halfWidth - mFont.tahoma_7b_white.getWidth("Skill Train")) / 2, panelY + 3, 0);

        // Draw items in a grid layout with fixed 3 columns
        // Vẽ nền theo từng hàng (phủ hết chiều ngang)
        int rowsToDraw = (int)System.Math.Ceiling((float)itemCount / MaxColumns);
        for (int row = 0; row < rowsToDraw; row++)
        {
            int y = panelY + headerHeight + row * ItemHeight;
            g.setColor(row % 2 == 0 ? 0xFFFFFF : 0xEFEFEF);
            g.fillRect(panelX, y, PanelWidth, ItemHeight); // Phủ toàn bộ chiều ngang
        }

        // Vẽ từng item sau khi nền đã có
        for (int i = 0; i < itemCount; i++)
        {
            int col = i % MaxColumns;
            int row = i / MaxColumns;
            int x = panelX + col * BasePanelWidth;
            int y = panelY + headerHeight + row * ItemHeight;

            string text = isShowingAutoItem ? AutoItems[i].Name : SkillTrains[i].Name;
            bool flag = isShowingAutoItem ? AutoItems[i].AutoFlag : SkillTrains[i].AutoFlag;

            mFont.tahoma_7b_dark.drawString(g, text, x + 10, y + 3, 0);
            GameCanvas.paintz.paintCheckPass(g, x + BasePanelWidth - CheckBoxSize - 10, y + 1, flag, false);
        }


        if (!isShowingAutoItem)
        {
            int btnY = panelY + headerHeight + Rows * ItemHeight + 2;

            long now = mSystem.currentTimeMillis();
            bool saveActive = now - lastClickSaveTime <= effectDuration;
            bool resetActive = now - lastClickResetTime <= effectDuration;

            int margin = 10;
            int btnWidth = 40;
            int btnSpacing = width - 2 * margin - btnWidth * 2;
            int btnStartX = panelX + margin;

            g.setColor(saveActive ? 0xFF6666 : 0xFF0000);
            g.fillRect(btnStartX, btnY, btnWidth, ToggleButtonHeight);
            mFont.tahoma_7_white.drawString(g, "Save", btnStartX + btnWidth / 2 - mFont.tahoma_7_white.getWidth("Save") / 2, btnY + 1, 0);

            g.setColor(resetActive ? 0x66FF66 : 0x00AA00);
            g.fillRect(btnStartX + btnWidth + btnSpacing, btnY, btnWidth, ToggleButtonHeight);
            mFont.tahoma_7_white.drawString(g, "Reset", btnStartX + btnWidth + btnSpacing + btnWidth / 2 - mFont.tahoma_7_white.getWidth("Reset") / 2, btnY + 1, 0);
        }
    }

    public static void HandleClick()
    {
        int panelX = PanelX;
        int panelY = PanelY;
        int headerHeight = 22;
        int halfWidth = PanelWidth / 2;

        if (GameCanvas.isPointerHoldIn(panelX, panelY, halfWidth, headerHeight)
            && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
        {
            isShowingAutoItem = true;
            GameCanvas.clearAllPointerEvent();
            return;
        }

        if (GameCanvas.isPointerHoldIn(panelX + halfWidth, panelY, halfWidth, headerHeight)
            && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
        {
            isShowingAutoItem = false;
            GameCanvas.clearAllPointerEvent();
            return;
        }

        int itemCount = isShowingAutoItem ? AutoItems.Length : SkillTrains.Length;

        for (int i = 0; i < itemCount; i++)
        {
            int col = i % MaxColumns;
            int row = i / MaxColumns;
            int x = panelX + col * BasePanelWidth;
            int y = panelY + headerHeight + row * ItemHeight;
            int checkX = x + BasePanelWidth - CheckBoxSize - 10;

            if (GameCanvas.isPointerHoldIn(checkX, y + 1, CheckBoxSize, CheckBoxSize)
                && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                if (isShowingAutoItem)
                {
                    var item = AutoItems[i];
                    item.AutoFlag = !item.AutoFlag;

                    if (item.AutoFlag && (item.Thread == null || !item.Thread.IsAlive))
                    {
                        item.Thread = new Thread(() => AutoItemVIP(item));
                        item.Thread.IsBackground = true;
                        item.Thread.Start();
                    }
                    else
                    {
                        GameScr.info1.addInfo("Đã tắt Auto " + item.Name, 0);
                    }
                }
                else
                {
                    SkillTrains[i].AutoFlag = !SkillTrains[i].AutoFlag;

                    if (SkillTrains[i].Id == 19 && Char.myCharz().getGender().Equals("NM"))
                    {
                        if (SkillTrains[i].AutoFlag)
                        {
                            ModProCuongLe.HealingPower = true;
                            new Thread(ModProCuongLe.AutoHealingPower2).Start();
                        }
                        else
                        {
                            ModProCuongLe.HealingPower = false;
                        }
                    }
                }

                GameCanvas.clearAllPointerEvent();
                return;
            }
        }

        if (!isShowingAutoItem)
        {
            int btnY = panelY + headerHeight + Rows * ItemHeight + 2;
            int margin = 10;
            int btnWidth = 40;
            int btnSpacing = PanelWidth - 2 * margin - btnWidth * 2;
            int btnStartX = panelX + margin;

            if (GameCanvas.isPointerHoldIn(btnStartX, btnY, btnWidth, ToggleButtonHeight)
                && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                SaveConfig();
                lastClickSaveTime = mSystem.currentTimeMillis();
                GameCanvas.clearAllPointerEvent();
                return;
            }

            if (GameCanvas.isPointerHoldIn(btnStartX + btnWidth + btnSpacing, btnY, btnWidth, ToggleButtonHeight)
                && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                ResetConfig();
                lastClickResetTime = mSystem.currentTimeMillis();
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }

        // ✅ Nuốt sự kiện click nếu click trong vùng menu
        int totalHeight = 22 + Rows * ItemHeight + (isShowingAutoItem ? 0 : ToggleButtonHeight + 5);
        if (GameCanvas.isPointerHoldIn(PanelX, PanelY, PanelWidth, totalHeight)
            && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
        {
            GameCanvas.clearAllPointerEvent();
        }
    }

    public static void SaveConfig()
    {
        string planet = Char.myCharz().getGender();
        string config = "";
        string shieldKey = "AutoSkillConfig_Shield_" + planet;
        string skillKey = "AutoSkillConfig_" + planet;
        for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
        {
            Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);
            for (int j = 0; j < SkillTrains.Length; j++)
            {
                if (SkillTrains[j].Id == skill.template.id)
                {
                    if (skill.template.id == 19)
                    {
                        Rms.saveRMSInt(shieldKey, SkillTrains[j].AutoFlag ? 1 : 0);
                    }
                    else
                    {
                        config = config + skill.template.id + '|' + (SkillTrains[j].AutoFlag ? "1" : "0") + ",";
                    }
                    break;
                }
            }
        }
        if (config.Length > 0)
        {
            config = config.TrimEnd(',');
        }
        Rms.saveRMSString(skillKey, config);
        GameScr.info1.addInfo("Đã lưu cấu hình Auto Skill", 0);
    }

    public static void LoadConfig()
    {
        string planet = Char.myCharz().getGender();
        string shieldKey = "AutoSkillConfig_Shield_" + planet;
        string skillKey = "AutoSkillConfig_" + planet;

        // Load trạng thái skill khiên
        int shieldState = Rms.loadRMSInt(shieldKey);

        // Load cấu hình các skill thường
        string config = Rms.loadRMSString(skillKey);
        Dictionary<int, bool> skillMap = new Dictionary<int, bool>();

        if (!string.IsNullOrEmpty(config))
        {
            string[] flags = config.Split(',');
            foreach (string entry in flags)
            {
                string[] parts = entry.Split('|');
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                {
                    skillMap[id] = parts[1] == "1";
                }
            }
        }

        // Gán lại trạng thái AutoFlag cho từng skill train
        for (int i = 0; i < SkillTrains.Length; i++)
        {
            if (SkillTrains[i].Id == 19)
            {
                SkillTrains[i].AutoFlag = shieldState == 1;
            }
            else if (skillMap.TryGetValue(SkillTrains[i].Id, out bool flag))
            {
                SkillTrains[i].AutoFlag = flag;
            }
            else
            {
                // Nếu không có trong cấu hình thì tắt mặc định
                SkillTrains[i].AutoFlag = false;
            }
        }
    }

    public static void ResetConfig()
    {
        SkillTrain[] skillTrains = SkillTrains;
        foreach (SkillTrain skill in skillTrains)
        {
            skill.AutoFlag = !excludedIds.Contains(skill.Id);
        }
        GameScr.info1.addInfo("Đã khôi phục cấu hình mặc định", 0);
    }
}