﻿using OrderService.Domain.SeedWork;

namespace OrderService.Domain.AggregateModels.CustomerAggregate
{
    public class CardType : Enumeration
    {
        public static CardType Visa = new(1, nameof(Visa));
        public static CardType MasterCard = new(2, nameof(MasterCard));

        public CardType(int id, string name): base(id, name)
        {
        }
    }
}
