using System;
using System.Collections.Generic;
using System.Text;

namespace PingNet1
{
    class ListAll: IListAllInterface
    {
        public List<string> Ip(string ipBase, string secondByte, string thirdByte, string fourthByte)
        {
            List<string> addresses = new List<string>();
            for (int i = Int32.Parse(secondByte); i < Int32.Parse(thirdByte); i++)
            {
                for (int j = 1; j < 256; j++)
                {
                    for (int n = 1; n < Int32.Parse(fourthByte); n++)
                    {
                        addresses.Add(ipBase + i.ToString() + "." + j.ToString() + "." + n.ToString());
                    }
                }
            }
            return addresses;
        }
    }
}
