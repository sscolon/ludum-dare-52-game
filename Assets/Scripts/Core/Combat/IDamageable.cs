using System;

namespace Mechanizer
{
    public interface IDamageable
    {
        public Health Health { get; }
        public PartyTag GetParty();
    }
}
