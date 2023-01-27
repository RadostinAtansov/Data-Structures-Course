
namespace _01.DogVet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class DogVet : IDogVet
    {

        private Dictionary<Owner, Dog> ownersAndDogs = new Dictionary<Owner, Dog>();
        private Dictionary<string, Dog> dogIdDog = new Dictionary<string, Dog>();
        private Dictionary<string, Owner> ownerIdOwner = new Dictionary<string, Owner>();

        public int Size => dogIdDog.Values.Count;

        public void AddDog(Dog dog, Owner owner)
        {
            if (this.dogIdDog.ContainsKey(dog.Id))
            {
                throw new ArgumentException();
            }

            if (!this.ownerIdOwner.ContainsKey(owner.Id))
            {
                ownerIdOwner[owner.Id] = owner;
                ownersAndDogs.Add(owner, dog);
            }
                dogIdDog[dog.Id] = dog;
                dogIdDog[dog.Id].Owner = owner;

            var ownerSameDog = this.ownerIdOwner[owner.Id].Dogs.FirstOrDefault(x => x.Name == dog.Name);

            if (ownerSameDog != null)
            {
                throw new ArgumentException();
            }

            ownerIdOwner[owner.Id].Dogs.Add(dog);

        }

        public bool Contains(Dog dog)
        {
            return this.dogIdDog.ContainsValue(dog);
        }

        public Dog GetDog(string name, string ownerId)
        {
            var owner = this.ownerIdOwner.FirstOrDefault(o => o.Key == ownerId).Value;

            if (owner == null)
            {
                throw new ArgumentException();
            }

            var dog = this.ownerIdOwner[ownerId].Dogs.FirstOrDefault(d => d.Name == name);

            if (dog == null)
            {
                throw new ArgumentException();
            }

            return dog;
        }

        public Dog RemoveDog(string name, string ownerId)
        {
            var owner = this.ownerIdOwner.FirstOrDefault(o => o.Value.Id == ownerId).Value;
            var dog = this.dogIdDog.FirstOrDefault(d => d.Value.Name == name).Value;

            if (owner == null || dog == null)
            {
                throw new ArgumentException();
            }

            ownerIdOwner[ownerId].Dogs.Remove(dog);
            dogIdDog.Remove(dog.Id);
            return dog;
        }

        public IEnumerable<Dog> GetDogsByOwner(string ownerId)
        {
            return this.ownerIdOwner[ownerId].Dogs;
        }

        public IEnumerable<Dog> GetDogsByBreed(Breed breed)
        {
            var dogsByBreed = this.dogIdDog.Values.Where(d => d.Breed == breed);

            if (!dogsByBreed.Any())
            {
                throw new ArgumentException();
            }
            return dogsByBreed;
        }

        public void Vaccinate(string name, string ownerId)
        {
            var owner = this.ownerIdOwner.FirstOrDefault(o => o.Key == ownerId).Value;
            if (owner == null)
            {
                throw new ArgumentException();
            }
            //var dog = this.ownersAndDogs[owner].Name == name;
            var dog = this.ownersAndDogs.FirstOrDefault(o => o.Key == owner && o.Value.Name == name).Value;

            if (dog == null)
            {
                throw new ArgumentException();
            }
            //ownersAndDogs[owner].FirstOrDefault(d => d.Name == name).Vaccines++;
            dogIdDog[dog.Id].Vaccines++;
        }

        public void Rename(string oldName, string newName, string ownerId)
        {
            var owner = this.ownerIdOwner.FirstOrDefault(o => o.Value.Id == ownerId).Value;
            var dog = this.dogIdDog.FirstOrDefault(d => d.Value.Name == oldName).Value;

            if (owner == null || dog == null)
            {
                throw new ArgumentException();
            }

            dog.Name = newName;
        }

        public IEnumerable<Dog> GetAllDogsByAge(int age)
        {
            var dogsByAge = this.dogIdDog.Values.Where(d => d.Age == age).ToList();

            if (dogsByAge.Count == 0)
            {
                throw new ArgumentException();
            }

            return dogsByAge;
        }

        public IEnumerable<Dog> GetDogsInAgeRange(int lo, int hi)
        {
            return this.dogIdDog.Values.Where(d => d.Age >= lo && d.Age <= hi);
        }

        public IEnumerable<Dog> GetAllOrderedByAgeThenByNameThenByOwnerNameAscending()
        {
            return this.dogIdDog
                .Values
                .OrderBy(a => a.Age)
                .ThenBy(n => n.Name)
                .ThenBy(on => on.Owner.Name);
        }
    }
}