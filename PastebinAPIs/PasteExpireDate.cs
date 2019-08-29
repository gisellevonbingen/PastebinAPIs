using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteExpireDate
    {
        private static readonly List<PasteExpireDate> List = new List<PasteExpireDate>();

        public static PasteExpireDate Never { get; }
        public static PasteExpireDate Minutes10 { get; }
        public static PasteExpireDate Hour { get; }
        public static PasteExpireDate Day { get; }
        public static PasteExpireDate Week { get; }
        public static PasteExpireDate Weeks2 { get; }
        public static PasteExpireDate Month { get; }
        public static PasteExpireDate Months6 { get; }
        public static PasteExpireDate Year { get; }

        static PasteExpireDate()
        {
            List.Add(Never = new PasteExpireDate(nameof(Never), "N"));
            List.Add(Minutes10 = new PasteExpireDate(nameof(Minutes10), "10M"));
            List.Add(Hour = new PasteExpireDate(nameof(Hour), "1H"));
            List.Add(Day = new PasteExpireDate(nameof(Day), "1D"));
            List.Add(Week = new PasteExpireDate(nameof(Week), "1W"));
            List.Add(Weeks2 = new PasteExpireDate(nameof(Weeks2), "2W"));
            List.Add(Month = new PasteExpireDate(nameof(Month), "1M"));
            List.Add(Months6 = new PasteExpireDate(nameof(Months6), "6M"));
            List.Add(Year = new PasteExpireDate(nameof(Year), "1Y"));
        }

        public static PasteExpireDate[] Values => List.ToArray();

        public string Name { get; }
        public string Value { get; }

        private PasteExpireDate(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value;
        }

    }

}
