using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TropicTrail.Repository
{
    public class CardManager
    {
        BaseRepository<Card> _card;

        public CardManager()
        {
            _card = new BaseRepository<Card>();
        }

        public Card FindCardByCardNumber(string cardNumber, string expiryDate)
        {
            return _card._table.Where(m => m.cardNumber == cardNumber && m.expireDate == expiryDate).FirstOrDefault();
        }
        public Card EnoughBalance(string cardNumber, decimal payment)
        {
            return _card._table.Where(m => m.cardNumber == cardNumber && m.balance >= payment).FirstOrDefault();
        }
    }
}