using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

namespace AlgorithmsOfProtectionInformation6
{
    public class RSA
    {
        public static void RSAencryption()
        {
            BigInteger n;
            BigInteger g;
            BigInteger Xa;
            BigInteger Xb;

            int t;
            do
            {
                Console.WriteLine("Enter amount of checks is Miller Rabin test(it should be integer): ");
                while (int.TryParse(Console.ReadLine(), out t) == false)
                {
                    Console.Write("Wrong value! Try again: ");
                }
            } while (t <= 0);

            int k;
            do
            {
                Console.WriteLine("Enter amount of bytes for generation of prime number(it should be integer): ");
                while (int.TryParse(Console.ReadLine(), out k) == false)
                {
                    Console.Write("Wrong value! Try again: ");
                }
            } while (k <= 0);

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Amount of generated digits: ");

            stopwatch.Start();

            do
            {
                n = GetRandomBigInt(k, t);
                g = GetRandomBigInt(k, t);
                Xa = GetRandomBigInt(k, t);
                Xb = GetRandomBigInt(k, t);
            } while (g > n || Xb > n || Xa > n);

            stopwatch.Stop();

            Console.WriteLine($"\nn = {n}\ng = {g}\nXa = {Xa}\nXb = {Xb}");
            Console.WriteLine($"Time is {stopwatch.ElapsedMilliseconds} milliseconds.\n");

            var Ya = BigInteger.ModPow(g, Xa, n);
            var Yb = BigInteger.ModPow(g, Xb, n);
            Console.WriteLine($"Open key of user A: {Ya}\nOpen key of user B: {Yb}\n");

            var keyA = BigInteger.ModPow(Yb, Xa, n);
            var keyB = BigInteger.ModPow(Ya, Xb, n);
            Console.WriteLine($"Private key of user A: {keyA}\nPrivate key of user B: {keyB}");

            Console.ReadLine();
        }

        // Method for getting simple random number.
        private static BigInteger GetRandomBigInt(int k, int t)
        {
            byte[] tempByteArray;
            BigInteger nValue;

            do
            {
                RNGCryptoServiceProvider randomNumberGenerator = new RNGCryptoServiceProvider();
                tempByteArray = new byte[t];
                randomNumberGenerator.GetNonZeroBytes(tempByteArray);
                nValue = new BigInteger(tempByteArray);
                Console.WriteLine($"n = {nValue}");

            } while (PrimeNumberTest(nValue, k) == false || (nValue < 0));

            return nValue;
        }

        // Miller Rabin test for checking primerity of some number
        private static bool PrimeNumberTest(BigInteger n, int k)
        {
            // If n == 2 or n == 3 - return true.
            if (n == 2 || n == 3)
                return true;
            // if n < 2 or n is even number - return false.
            if (n < 2 || n % 2 == 0)
                return false;

            int s = 0;
            var d = n - 1;
            while (d % 2 != 1)
            {
                d /= 2;
                s++;
            }

            for (var i = 0; i < k; i++)
            {
                // Choosing random number a [2, 2 - n].
                RNGCryptoServiceProvider randomNumberGenerator = new RNGCryptoServiceProvider();
                byte[] tempA = new byte[n.ToByteArray().LongLength];

                BigInteger a;
                randomNumberGenerator.GetNonZeroBytes(tempA);
                do
                {
                    randomNumberGenerator.GetNonZeroBytes(tempA);
                    a = new BigInteger(tempA);
                } while (a < 2 || a >= n - 2);

                // x = a^d mod(n)
                var x = BigInteger.ModPow(a, d, n);

                // If x == 1 or x == n - 1, skipping one lap.
                if (x == 1 || x == n - 1)
                    continue;
                // Repeat s - 1 times 
                for(int j = 1; j < s; j++)
                {
                    // x = x^2 mod(n)
                    x = BigInteger.ModPow(x, 2, n);

                    // If x == 1, digit is composite.
                    if (x == 1)
                        return false;

                    // If x == n - 1, going to outer cycle iteration.
                    if (x == n - 1)
                        break;
                }

                // Return "possibly prime".
                if (x != n - 1)
                    return false;
            }

            return true;
        }

    }
}
