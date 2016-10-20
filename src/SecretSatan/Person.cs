using System;

namespace SecretSatan
{
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Person))
            {
                return false;
            }
            var person = (Person) obj;

            return person.Name.Equals(Name)
                   && person.Email.Equals(Email);
        }
    }
}