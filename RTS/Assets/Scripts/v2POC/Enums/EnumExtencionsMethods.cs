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

    }


}