namespace HM_WorkingWithDB.Entities
{
    public class Category : IEntity
    {
        public long Id {  get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Category[Id={Id},Name={Name}]";
        }
    }
}
