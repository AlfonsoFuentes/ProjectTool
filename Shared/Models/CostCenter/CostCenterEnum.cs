﻿using Shared.Models.BudgetItemTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.CostCenter
{
    public class CostCenterEnum
    {
        public override string ToString()
        {
            return Name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static CostCenterEnum Create(int id, string name) => new CostCenterEnum() { Id = id, Name = name };

        public static CostCenterEnum None = Create(-1, "None");
        public static CostCenterEnum Saponification = Create(0, "1200");
        public static CostCenterEnum PCPFinishing = Create(1, "1202");
        public static CostCenterEnum HCMaking = Create(2, "1300");

        public static List<CostCenterEnum> List = new List<CostCenterEnum>()
        {
            None, Saponification, PCPFinishing, HCMaking
        };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;

        public static CostCenterEnum GetTypeByName(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : CostCenterEnum.None;

        public static CostCenterEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : CostCenterEnum.None;


    }
}