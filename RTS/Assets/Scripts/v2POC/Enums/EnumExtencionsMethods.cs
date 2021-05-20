using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace V2.Enums
{

    public static class EnumExtencionsMethods
    {
        public static float TimeToCreate(this UnitsEnum unitType) {

            switch(unitType) {
                case UnitsEnum.Archer:
                    break;
                case UnitsEnum.Citizen:
                    break;
                case UnitsEnum.Knight:
                    break;
                case UnitsEnum.SwordMan:
                    break;
            }
            return 10;
        }

    }


}