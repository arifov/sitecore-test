using MAP.Domain;
using MAP.Domain.Contracts;
using MAP.Domain.Models;
using MAP.Tasks.Helpers;
using MAP.Tasks.Payment;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MAP.Tasks.Tests
{
    public class PaymentHelperTests
    {

        [Theory]
        [InlineData(CardBrand.Visa, "4111111111111111", CardBrand.Visa, CardBrand.DinersClub)]
        [InlineData(CardBrand.ChinaUnionPay, "6250947000000014", CardBrand.Amex, CardBrand.ChinaUnionPay)]
        public void GetTerminalByCardBrand(CardBrand expected, string cardNumber, params object[] brands)
        {
            //arrange
            var terminals = new List<Terminal>();

            var terminal1 = new Terminal();
            terminal1.CardBrands = new CardBrandList();
            terminal1.CardBrands.Items.Add((CardBrand)brands[0]);

            var terminal2 = new Terminal();
            terminal2.CardBrands = new CardBrandList();
            terminal2.CardBrands.Items.Add((CardBrand)brands[1]);

            terminals.Add(terminal1);
            terminals.Add(terminal2);

            //act
            var terminal = PaymentHelper.GetTerminal(Domain.Payment.PaymentUISection.CreditCard, terminals, "123456", cardNumber);

            //assert
            Assert.True(terminal.CardBrands.Contains(expected));
        }

        [Theory]
        [InlineData("MID1", "MID1", "MID2", "MID3")]
        [InlineData("MID2", "MID1", "MID2", "MID3")]
        public void GetTerminalByMID(string expected, params object[] mids)
        {
            //arrange
            var settingProvider = new Mock<ISettingProvider>();
            settingProvider.Setup(m => m.IsPaymentAPITestMode).Returns(() => true);

            PaymentProcessorFactory.SettingProvider = settingProvider.Object;

            var terminals = new List<Terminal>();

            foreach (var mid in mids)
            {
                terminals.Add(new Terminal()
                {
                    GW3_MID = (string)mid
                });
            }

            //act
            var terminal = PaymentHelper.GetTerminal(Domain.Payment.PaymentUISection.CreditCard, terminals, expected);

            //assert
            Assert.Equal(expected, terminal.GW3_MID);
        }

        [Theory]
        [InlineData("SPAY0000001", "UPOP0000001", "FPX0000001", "IB0000001", "SPAY0000001", "PM0000001", "GW20000001", "GW30000001")]
        [InlineData("PM0000002", "UPOP0000002", "FPX0000002", "IB0000002", "SPAY0000002", "PM0000002", "GW20000002", "GW30000002")]
        [InlineData("182153957136153", "", "MCPFP09879", "", "182153957136153", "", "", "")]
        [InlineData("SPAY0000002", "", "FPX0000002", "", "SPAY0000002", "", "", "")]
        [InlineData("3117120002", "", "3117120002_FPX0000002", "3117120002_IB0000002", "SPAY0000002", "", "3117120002", "")]
        public void GetEcomTerminalByMID(string expected, string upop, string fpx, string ib, string spay, string pm, string gw2J, string gw3)
        {
            //arrange
            var settingProvider = new Mock<ISettingProvider>();
            settingProvider.Setup(m => m.IsPaymentAPITestMode).Returns(() => true);

            PaymentProcessorFactory.SettingProvider = settingProvider.Object;

            var terminals = new List<Terminal>();
            
            var terminal = new Terminal()
            {
                Gateway = GatewayHost.UPOP,
                IB_MerchantId = upop,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(upop)) terminals.Add(terminal);

            terminal = new Terminal()
            {
                Gateway = GatewayHost.FPX,
                IB_MerchantId = fpx,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(fpx)) terminals.Add(terminal);

            terminal = new Terminal()
            {
                Gateway = GatewayHost.IB,
                IB_MerchantId = ib,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(ib)) terminals.Add(terminal);

            terminal = new Terminal()
            {
                Gateway = GatewayHost.SPAY,
                EmailInvoice_MCPTID = spay,
                IsDeleted = false,
                Is3DS = true
            };
            if (!string.IsNullOrEmpty(spay)) terminals.Add(terminal);

            terminal = new Terminal()
            {
                Gateway = GatewayHost.PM,
                EmailInvoice_MCPTID = pm,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(pm)) terminals.Add(terminal);


            terminal = new Terminal()
            {
                Gateway = GatewayHost.GW2J,
                EmailInvoice_MCPTID = gw2J,
                MOTO_MCPTID = gw2J,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(gw2J)) terminals.Add(terminal);

            terminal = new Terminal()
            {
                Gateway = GatewayHost.GW3,
                GW3_MID = gw3,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(gw3)) terminals.Add(terminal);

            //act
            var requestTerminal = PaymentHelper.GetEcomTerminal(expected, terminals);

            //assert
            Assert.True( requestTerminal.HasMID(expected));

            //assert
            Assert.NotNull(requestTerminal);
        }

        [Theory]
        [InlineData(3, CardBrand.Amex, CardBrand.Visa, CardBrand.DinersClub)]
        [InlineData(4, CardBrand.Amex, CardBrand.Visa, CardBrand.DinersClub, CardBrand.ChinaUnionPay)]
        public void GetCreditCardBrands(int expected, params object[] brands)
        {
            //arrange
            var terminals = new List<Terminal>();

            var terminal1 = new Terminal();
            terminal1.CardBrands = new CardBrandList();
            for (int i = 0; i < 2; i++)
            {
                terminal1.CardBrands.Items.Add((CardBrand)brands[i]);
            }

            var terminal2 = new Terminal();
            terminal2.CardBrands = new CardBrandList();
            for (int i = 2; i < brands.Length; i++)
            {
                terminal2.CardBrands.Items.Add((CardBrand)brands[i]);
            }

            terminals.Add(terminal1);
            terminals.Add(terminal2);

            var terminal = new Terminal();

            //act
            terminal.CopyAllCardBrandsFrom(terminals);

            //assert
            Assert.Equal(expected, terminal.CardBrands.Items.Count);
        }
    }
}
