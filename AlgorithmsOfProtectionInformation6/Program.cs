﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOfProtectionInformation6
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RSA.RSAencryption();
            }
            catch (OverflowException)
            {

            }
        }
    }
}
