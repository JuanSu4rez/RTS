namespace V2.Controllers
{
    public partial class UnitController : V2.Interfaces.IDamagable, V2.Interfaces.ISelectable
    {
        public bool IsSelected { get; set; }
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