using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2c2p.Assignment.Data.Context.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ISO4217CurrencyCode",
                columns: table => new
                {
                    Values = table.Column<string>(type: "VARCHAR(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISO4217CurrencyCode", x => x.Values);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStatus",
                columns: table => new
                {
                    Values = table.Column<string>(type: "VARCHAR(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatus", x => x.Values);
                });

            migrationBuilder.InsertData(
                table: "ISO4217CurrencyCode",
                column: "Values",
                values: new object[]
                {
                    "AED",
                    "PYG",
                    "QAR",
                    "RON",
                    "RSD",
                    "RUB",
                    "RWF",
                    "SAR",
                    "SBD",
                    "PLN",
                    "SCR",
                    "SEK",
                    "SGD",
                    "SHP",
                    "SLL",
                    "SOS",
                    "SRD",
                    "SSP",
                    "STN",
                    "SDG",
                    "PKR",
                    "PHP",
                    "PGK",
                    "MNT",
                    "MOP",
                    "MRU",
                    "MUR",
                    "MVR",
                    "MWK",
                    "MXN",
                    "MXV",
                    "MYR",
                    "MZN",
                    "NAD",
                    "NGN",
                    "NIO",
                    "NOK",
                    "NPR",
                    "NZD",
                    "OMR",
                    "PAB",
                    "PEN"
                });

            migrationBuilder.InsertData(
                table: "ISO4217CurrencyCode",
                column: "Values",
                values: new object[]
                {
                    "SVC",
                    "MMK",
                    "SYP",
                    "THB",
                    "XBA",
                    "XBB",
                    "XBC",
                    "XBD",
                    "XCD",
                    "XDR",
                    "XOF",
                    "XPD",
                    "XAU",
                    "XPF",
                    "XSU",
                    "XTS",
                    "XUA",
                    "XXX",
                    "YER",
                    "ZAR",
                    "ZMW",
                    "ZWL",
                    "XPT",
                    "XAG",
                    "XAF",
                    "WST",
                    "TJS",
                    "TMT",
                    "TND",
                    "TOP",
                    "TRY",
                    "TTD",
                    "TWD",
                    "TZS",
                    "UAH",
                    "UGX",
                    "USD",
                    "USN",
                    "UYI",
                    "UYU",
                    "UYW",
                    "UZS"
                });

            migrationBuilder.InsertData(
                table: "ISO4217CurrencyCode",
                column: "Values",
                values: new object[]
                {
                    "VES",
                    "VND",
                    "VUV",
                    "SZL",
                    "MKD",
                    "MGA",
                    "DZD",
                    "CAD",
                    "CDF",
                    "CHE",
                    "CHF",
                    "CHW",
                    "CLF",
                    "CLP",
                    "CNY",
                    "BZD",
                    "COP",
                    "CRC",
                    "CUC",
                    "CUP",
                    "CVE",
                    "CZK",
                    "DJF",
                    "DKK",
                    "DOP",
                    "COU",
                    "BYN",
                    "BWP",
                    "BTN",
                    "AMD",
                    "ANG",
                    "AOA",
                    "ARS",
                    "AUD",
                    "AWG",
                    "AZN",
                    "BAM",
                    "BBD",
                    "BDT",
                    "BGN",
                    "BHD",
                    "BIF"
                });

            migrationBuilder.InsertData(
                table: "ISO4217CurrencyCode",
                column: "Values",
                values: new object[]
                {
                    "BMD",
                    "BND",
                    "BOB",
                    "BOV",
                    "BRL",
                    "BSD",
                    "MDL",
                    "ALL",
                    "EGP",
                    "ETB",
                    "JOD",
                    "JPY",
                    "KES",
                    "KGS",
                    "KHR",
                    "KMF",
                    "KPW",
                    "KRW",
                    "JMD",
                    "KWD",
                    "KZT",
                    "LAK",
                    "LBP",
                    "LKR",
                    "LRD",
                    "LSL",
                    "LYD",
                    "MAD",
                    "KYD",
                    "ISK",
                    "IRR",
                    "IQD",
                    "EUR",
                    "FJD",
                    "FKP",
                    "GBP",
                    "GEL",
                    "GHS",
                    "GIP",
                    "GMD",
                    "GNF",
                    "GTQ"
                });

            migrationBuilder.InsertData(
                table: "ISO4217CurrencyCode",
                column: "Values",
                values: new object[]
                {
                    "GYD",
                    "HKD",
                    "HNL",
                    "HRK",
                    "HTG",
                    "HUF",
                    "IDR",
                    "ILS",
                    "INR",
                    "ERN",
                    "AFN"
                });

            migrationBuilder.InsertData(
                table: "TransactionStatus",
                column: "Values",
                values: new object[]
                {
                    "REJECTED",
                    "APPROVED",
                    "DONE"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ISO4217CurrencyCode");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionStatus");
        }
    }
}
