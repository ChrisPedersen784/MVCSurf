using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Chatbot
{
    public class AutoResponds
    {
        public async Task<string> Answer(string message)
        {
            string returnMessage = "";
            if(message.ToLower() == "shipping cost")
            {
                returnMessage = "The shipping cost for standard delivery is $5.99.";
            } else if(message.ToLower() == "return policy")
            {
                returnMessage = "Our return policy allows returns within 30 days of purchase. Please check our website for detailed information.";
            }

            return returnMessage;
        }
    }
}
