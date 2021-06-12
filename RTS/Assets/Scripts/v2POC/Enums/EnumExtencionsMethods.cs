using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace V2.Enums
{

    public static class EnumExtencionsMethods
    {
        public static float TimeToCreate(this EntityType unitType) {
            switch(unitType) {
                case EntityType.Archer:
                    break;
                case EntityType.Citizen:
                    break;
                case EntityType.Knight:
                    break;
                case EntityType.SwordMan:
                    break;
            }
            return 10;
        }

        public static ResourceTypes GetResourceType(this EntityType unitType) {
            switch(unitType) {
                case EntityType.GoldMine:
                    return ResourceTypes.Gold;
                    break;
                case EntityType.WildPig:
                    return ResourceTypes.Food;
                    break;
                case EntityType.Three:
                    return ResourceTypes.Wood;
                    break;
                case EntityType.RockMine:
                    return ResourceTypes.Rock;
                    break;
            }
            return ResourceTypes._None;
        }

    }


}