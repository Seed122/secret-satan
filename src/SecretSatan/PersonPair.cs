using System;

namespace SecretSatan
{
    [Serializable]
    public class PersonPair
    {
        public Person Sender { get; set; }
        public Person Receiver { get; set; }
    }
}