using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.MWOTypes
{
    public class MWOTypeEnum
    {
        public override string ToString()
        {
            return Name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public static MWOTypeEnum Create(int id, string name) => new MWOTypeEnum() { Id = id, Name = name };

        public static MWOTypeEnum None = Create(-1, "NONE");
        public static MWOTypeEnum Savings = Create(0, "SAVINGS");
        public static MWOTypeEnum EHS = Create(1, "EHS");
        public static MWOTypeEnum Improvement = Create(2, "IMPROVEMENT");
        public static MWOTypeEnum Replacement = Create(3, "REPLACEMENT");
        public static MWOTypeEnum Div = Create(4, "DIV");
        public static MWOTypeEnum Quality = Create(5, "QUALITY");
        public static List<MWOTypeEnum> List = new List<MWOTypeEnum>()
            {
          None,  Savings,EHS,Improvement, Replacement, Div, Quality
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;

        public static MWOTypeEnum GetType(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : MWOTypeEnum.None;
        public static MWOTypeEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : MWOTypeEnum.None;
    }
}
