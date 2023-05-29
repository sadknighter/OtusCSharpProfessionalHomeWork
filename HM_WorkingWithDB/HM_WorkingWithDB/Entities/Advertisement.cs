namespace HM_WorkingWithDB.Entities
{
    public class Advertisement : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Advertisement[Id={Id},User={User.LastName} {User.FirstName}, Category={Category.Name}, Created At={CreatedAt}, Name={Name}, Description={Description}]";
        }
    }
}
