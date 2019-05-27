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
            AddNewSecondaryProfession(toProfessionId);
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
