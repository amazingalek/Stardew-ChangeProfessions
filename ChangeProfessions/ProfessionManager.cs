using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewValley;

namespace ChangeProfessions
{
    public class ProfessionManager
    {
        private readonly List<ProfessionSet> _professionSets;

        public ProfessionManager(IModHelper modHelper)
        {
            _professionSets = modHelper.ReadConfig<Config>().ProfessionSets;
        }

        public ProfessionSet GetProfessionSetById(int professionId)
        {
            return _professionSets.Single(x => x.Ids.Contains(professionId));
        }

        public void ChangeProfession(int fromProfessionId, int toProfessionId, bool isPrimaryProfession)
        {
            RemoveProfession(fromProfessionId);
            AddProfession(toProfessionId);

            if (!isPrimaryProfession) return;

            RemoveOldSecondaryProfession(fromProfessionId);

            if (ShouldHaveSecondaryProfession(fromProfessionId))
            {
                AddNewSecondaryProfession(toProfessionId);
            }
        }

        private void RemoveOldSecondaryProfession(int primaryId)
        {
            var oldSecondarySet = GetSecondarySetByPrimaryId(primaryId);
            oldSecondarySet.Ids.ForEach(RemoveProfession);
        }

        private void AddNewSecondaryProfession(int primaryId)
        {
            var newSecondarySet = GetSecondarySetByPrimaryId(primaryId);
            AddProfession(newSecondarySet.Ids.First());
        }

        private bool ShouldHaveSecondaryProfession(int professionId)
        {
            var skillLevel = GetSkillLevel(professionId);
            return skillLevel == 10;
        }

        private int GetSkillLevel(int professionId)
        {
            if (professionId <= 5) return Game1.player.FarmingLevel;
            if (professionId <= 11) return Game1.player.FishingLevel;
            if (professionId <= 17) return Game1.player.ForagingLevel;
            if (professionId <= 23) return Game1.player.MiningLevel;
            return Game1.player.CombatLevel;
        }

        private ProfessionSet GetSecondarySetByPrimaryId(int primaryId)
        {
            return _professionSets.Single(x => x.ParentId == primaryId);
        }

        private void AddProfession(int professionId)
        {
            Game1.player.professions.Add(professionId);
        }

        private void RemoveProfession(int professionId)
        {
            Game1.player.professions.Remove(professionId);
        }

    }
}
