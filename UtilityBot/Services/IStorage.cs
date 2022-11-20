using System;
using System.Collections.Generic;
using System.Text;
using UtilityBot.Models;

namespace UtilityBot.Services
{
    public interface IStorage
    {
        Session GetSession(long chatId);
    }
}
