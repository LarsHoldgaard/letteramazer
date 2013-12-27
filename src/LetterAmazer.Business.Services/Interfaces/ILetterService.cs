using LetterAmazer.Business.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface ILetterService
    {
        decimal GetCost(Letter letter);
    }
}
