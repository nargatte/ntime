namespace Server.Models
{
    public class NameIdModel
    {
        public NameIdModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public NameIdModel()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}