using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction
{
    public static class myGlobal
    {
        static Dictionary<string, string> _parameters;

        public static string buttonclasses
        {
            get
            {
                return "f6 grow no-underline br-pill ba bw1 ph3 pv2 mb2 black";
            }
        }
        public static Dictionary<string, string> parameters
        {
            get
            {
                return _parameters;
            }
             
            set
            {
                _parameters = value;
            }
           
        }
    }
}