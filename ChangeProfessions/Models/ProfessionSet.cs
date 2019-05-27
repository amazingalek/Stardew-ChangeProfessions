using System.Collections.Generic;
using System.Linq;
using StardewValley.Menus;

namespace ChangeProfessions
{
    public class ProfessionSet
    {
        public List<int> Ids { get; set; }

        public int? ParentId { get; set; }

        public bool IsPrimaryProfession => ParentId == null;

        public override string ToString()
        {
            return string.Join(", ", Ids.Select(LevelUpMenu.getProfessionTitleFromNumber));
        }
    }
}