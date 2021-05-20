namespace V2.Controllers
{
    public partial class UnitController : V2.Interfaces.IDamagable
    {
        public float AddDamage(float damage) {
            var result = this.CurrentHealth - damage;
            if(result < 0) {
                result += damage;
            }
            else {
                result = damage;
            }
            this.CurrentHealth -= result;
            return result;
        }
    }
}