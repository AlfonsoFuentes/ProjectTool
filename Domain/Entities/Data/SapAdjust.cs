using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Data
{
    public class SapAdjust : BaseEntity
    {
        public DateTime Date {  get; set; }
        public double ActualSap {  get; set; }
        public double CommitmentSap {  get; set; }
        public double PotencialSap {  get; set; }
        public double BudgetCapital {  get; set; }
        public double ActualSoftware {  get; set; }
        public double CommitmentSoftware { get; set; }
        public double PotencialSoftware { get; set; }
        public string Justification { get; set; } = string.Empty;
        public string ImageTitle { get; set; } = string.Empty;
       
        public string ImageDataFile { get; set; } = string.Empty;
        public MWO MWO { get; set; } = null!;
        public Guid MWOId { get; set; }

        public static SapAdjust Create(Guid mwoid)
        {
            SapAdjust item = new SapAdjust();
            item.MWOId = mwoid;
            item.Id = Guid.NewGuid();

            return item;
        }
    }
}
