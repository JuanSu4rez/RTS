namespace V2.Controllers
{
    public partial class UnitController : V2.Interfaces.IDamagable
    {
        public void AddDamage(float damage) {
            this.CurrentHealth -= damage;
        }
    }
}