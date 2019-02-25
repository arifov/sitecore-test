using System;
using MAP.Tasks.Payment.CreditCards.Processors;
using Xunit;

namespace MAP.Tasks.Tests
{
    public class MPIValidatorTests
    {
        [Theory]
        //VISA
        [InlineData(true, "438857XXXXXX8691", "5")]
        [InlineData(true, "4111111111111111", "6")]
        [InlineData(false, "438857XXXXXX8691", "1")]
        [InlineData(false, "485620XXXXXX8903", "2")]
        //MasterCard
        [InlineData(true, "516240XXXXXX8425", "1")]
        [InlineData(true, "516240XXXXXX8425", "2")]
        [InlineData(false, "516240XXXXXX8425", "5")]
        [InlineData(false, "516240XXXXXX8425", "6")]
        //JCB
        [InlineData(true, "3559702766810527", "5")]
        [InlineData(false, "3589527641113936", "6")]
        [InlineData(false, "3589527641113936", "1")]
        [InlineData(false, "3559702766810527", "2")]
        //AMEX
        [InlineData(true, "378734493671000", "5")]
        [InlineData(true, "37828XXXXXX0005", "6")]
        [InlineData(false, "371449635398431", "1")]
        [InlineData(false, "37828XXXXXX0005", "2")]
        //OTHER allow to pass validation for all other card brands. gateway will take responsibility
        [InlineData(true, "38520000023237", null)]
        [InlineData(true, "6011XXXXXXXX1117", null)]
        [InlineData(true, "1111XXXXXXXX1111", null)]

        public void IsECIApproved(bool expected, string cnn, string tx_eci)
        {
            Assert.Equal(expected, MPIValidator.IsECIApproved(cnn, tx_eci));
        }

        [Theory]
        [InlineData(true, "Y", null, "438857XXXXXX8691", "5")]
        [InlineData(true, "Y", "", "438857XXXXXX8691", "5")]
        [InlineData(false, "N", "", "438857XXXXXX8691", "5")]
        [InlineData(true, "A", "", "438857XXXXXX8691", "5")]
        [InlineData(true, "A", "error", "438857XXXXXX8691", "5")]
        [InlineData(false, "U", "error", "438857XXXXXX8691", "5")]
        [InlineData(false, "X", null, "438857XXXXXX8691", "5")]

        public void ValidateCallbackData(bool expected, string tx_status, string errorcode, string cnn, string tx_eci)
        {
            //arrange
            var data = new MPIRes
            {
                ccn = cnn, // 13 to 19 Credit card number (as originally inputted)
                exp = "XXXXXX", // 6 Credit card expiry date (as originally inputted)
                mid = "000702053562583-48001122", // 24 merchantID with the MPI. (as originally inputted)
                orderno = "405278", // 20 Order Number of the transaction (as originally inputted)
                amount = "2500", // 16 Transaction amount (as originally inputted)
                acqbin = null, // Up to 11, usually 6 Acquirer’s BIN
                currcode = null, // 3 ISO-4217 Numeric Currency Code. This will determine the exponential of amount. e.g. 702 for SGD etc.
                purchase_xid = "00000000000000405278", // 20 Transaction Identifier (From ACS – PARES)
                tx_cavv = "BwABBkl2cQAAADEmg3ZxAAAAAAA=", //28 Alphanumeric value Transaction Stain/Cardholder Authentication Verification Value (From ACS – PARES)
                tx_eci = tx_eci, //2 Electronic commerce indicator value (From ACS - PARES)
                tx_status = tx_status, //1 Possible values are Y,N,U,A “Y” – Authenticated “N” – Not Authenticated “U” – Unknown “A” – Proof of Authentication (From ACS - PARES)
                errorcode = errorcode,
                errormsg = null,
                additionalData = null
            };

            //act
            var actual = true;
            try
            {
                MPIValidator.ValidateCallbackData(data);
            }
            catch (ArgumentException ex)
            {
                actual = false;
            }
            
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
