namespace HM_WorkingWithDB.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public override string ToString()
        {
            return $"User[Id={Id},FirstName={FirstName},LastName={LastName},MiddleName={MiddleName},Email={Email}, Telephone={Telephone}]";
        }
    }
}
