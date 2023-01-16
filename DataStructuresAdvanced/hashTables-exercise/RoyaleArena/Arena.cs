namespace RoyaleArena
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;

    public class Arena : IArena
    {
        private Dictionary<int, BattleCard> battlecards;

        public Arena()
        {
            this.battlecards = new Dictionary<int, BattleCard>();
        }

        public int Count => this.battlecards.Count;

        public void Add(BattleCard card)
        {
            if (!battlecards.ContainsKey(card.Id))
            {
                battlecards.Add(card.Id, card);
            }
        }

        public void ChangeCardType(int id, CardType type)
        {
            if (this.battlecards.ContainsKey(id))
            {
                this.battlecards[id].Type = type;
                return;
            }

            throw new InvalidOperationException();
        }

        public bool Contains(BattleCard card)
        {
            return this.battlecards.ContainsKey(card.Id);
        }

        public IEnumerable<BattleCard> FindFirstLeastSwag(int n)
        {
            if (this.Count < n)
            {
                throw new InvalidOperationException();
            }

            return this.battlecards
                .OrderBy(kvp => kvp.Value.Swag)
                .ThenBy(kvp => kvp.Value.Id)
                .Take(n)
                .Select(kvp => kvp.Value);
        }

        public IEnumerable<BattleCard> GetAllInSwagRange(double lo, double hi)
        {
            return this.battlecards
                .Where(kvp => kvp.Value.Swag >= lo && kvp.Value.Swag <= hi)
                .Select(kvp => kvp.Value)
                .OrderBy(bc => bc.Swag);
        }

        public IEnumerable<BattleCard> GetByCardType(CardType type)
        {
            var filteredBattlecards = this.battlecards
                .Where(kvp => kvp.Value.Type == type)
                .Select(kvp => kvp.Value);

            if (filteredBattlecards.Count() == 0)
            {
                throw new InvalidOperationException();
            }

                return filteredBattlecards
                .OrderBy(bc => bc.Damage)
                .ThenBy(bc => bc.Id);
        }

        public IEnumerable<BattleCard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
        {
            var cards = this.battlecards
                .Where(kvp => kvp.Value.Type == type && kvp.Value.Damage <= damage)
                .Select(kvp => kvp.Value)
                .OrderByDescending(bc => bc.Damage)
                .ThenBy(bc => bc.Id);

            if (cards.Count() == 0)
            {
                throw new InvalidOperationException();
            }

            return cards;
        }

        public BattleCard GetById(int id)
        {
            if (this.battlecards.ContainsKey(id))
            {
                return this.battlecards[id];
            }

            throw new InvalidOperationException();
        }

        public IEnumerable<BattleCard> GetByNameAndSwagRange(string name, double lo, double hi)
        {
            var cards = this.battlecards
                .Where(kvp => kvp.Value.Name == name && kvp.Value.Swag >= lo && kvp.Value.Swag < hi)
                .Select(kvp => kvp.Value)
                .OrderByDescending(bc => bc.Swag)
                .ThenBy(bc => bc.Id);

            if (cards.Count() == 0)
            {
                throw new InvalidOperationException();
            }

            return cards;
        }

        public IEnumerable<BattleCard> GetByNameOrderedBySwagDescending(string name)
        {
            var cards = this.battlecards
            .Where(kvp => kvp.Value.Name == name)
            .Select(kvp => kvp.Value)
            .OrderByDescending(bc => bc.Swag)
            .ThenBy(bc => bc.Id);

            if (cards.Count() == 0)
            {
                throw new InvalidOperationException();
            }

            return cards;
        }

        public IEnumerable<BattleCard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
        {
            var cards = this.GetByCardTypeAndMaximumDamage(type, hi)
                .Where(bc => bc.Damage >= lo);

            if (cards.Count() == 0)
            {
                throw new InvalidOperationException();
            }

            return cards;
        }

        public void RemoveById(int id)
        {
            if (!this.battlecards.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            this.battlecards.Remove(id);
        }

        public IEnumerator<BattleCard> GetEnumerator()
        {
            foreach (var item in this.battlecards)
            {
                yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}