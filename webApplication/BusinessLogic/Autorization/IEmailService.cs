using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;

namespace BusinessLogic.Authorization
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }
}
