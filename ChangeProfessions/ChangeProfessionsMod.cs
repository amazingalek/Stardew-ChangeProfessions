using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace ChangeProfessions
{
    public class ChangeProfessionsMod : Mod
    {
        private IModHelper _modHelper;
        private ProfessionManager _professionManager;

        public override void Entry(IModHelper modHelper)
        {
            _modHelper = modHelper;
            _professionManager = new ProfessionManager(modHelper);
            modHelper.Events.Input.ButtonReleased += OnButtonReleased;
        }

        private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (!IsCorrectButton(e.Button))
                return;

            var clickedProfessionBar = GetClickedProfessionBar(e);
            if (clickedProfessionBar == null)
                return;

            var professionId = Convert.ToInt32(clickedProfessionBar.name);
            var professionSet = _professionManager.GetProfessionSetById(professionId);

            ShowProfessionChooserMenu(professionId, professionSet);
        }

        private bool IsCorrectButton(SButton button)
        {
            return button == SButton.MouseLeft || button == SButton.ControllerA;
        }

        private ClickableTextureComponent GetClickedProfessionBar(ButtonReleasedEventArgs e)
        {
            var skillsPage = GetSkillsPage();
            return skillsPage == null ? null : GetProfessionBar(skillsPage, e.Cursor);
        }

        private void ShowProfessionChooserMenu(int professionId, ProfessionSet professionSet)
        {
            var professionChooserMenu = new ProfessionChooserMenu(_modHelper, professionSet.Ids.ToArray());

            professionChooserMenu.OnChangedProfession += newProfessionId =>
            {
                if (newProfessionId == professionId) return;
                _professionManager.ChangeProfession(professionId, newProfessionId, professionSet.IsPrimaryProfession);
            };

            Game1.activeClickableMenu = professionChooserMenu;
        }

        private SkillsPage GetSkillsPage()
        {
            if (!(Game1.activeClickableMenu is GameMenu menu))
                return null;
            var pages = Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue();
            var page = pages[menu.currentTab];
            return page as SkillsPage;
        }

        private ClickableTextureComponent GetProfessionBar(SkillsPage skillPage, ICursorPosition cursor)
        {
            var x = (int)cursor.ScreenPixels.X;
            var y = (int)cursor.ScreenPixels.Y;
            return skillPage.skillBars.FirstOrDefault(skillBar => skillBar.containsPoint(x, y) &&
                                                                  skillBar.hoverText.Length > 0 &&
                                                                  skillBar.name != "-1");
        }

    }
}