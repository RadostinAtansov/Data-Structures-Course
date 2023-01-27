using System.Collections.Generic;

namespace _01.DogVet
{
    public class Owner
    {
        public Owner(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Dogs = new List<Dog>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<Dog> Dogs { get; set; }
    }
}