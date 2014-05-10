using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;

namespace LetterAmazer.Business.Services.Domain.Checkout
{
    public class Checkout
    {
        public int UserId { get; set; }
        
        /// <summary>
        /// Contains all the letters, where the integer is an officeproductID and the letter is a letter
        /// </summary>
        public List<Tuple<int, Letter>> Letters { get; set; }

        public int PaymentMethodId { get; set; }

        public Checkout()
        {
            this.Letters = new List<Tuple<int, Letter>>();
        }

    }
}
