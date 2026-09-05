using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Contractors
{
    public class Form
    {
        public string UniqueCode {  get; }
        public int OrderId { get; }
        public int Step {  get; }
        public bool IsFinal {  get; }
        public IReadOnlyList<Field> Fealds { get; }

        public Form(string uniqued,int orderId,int step,bool osFinal,IEnumerable<Field> fealds)
        {
            if (string.IsNullOrWhiteSpace(uniqued))
            {
                throw new ArgumentException(nameof(uniqued));
            }
            if (step < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(step));
            }
            if (fealds == null)
            {
                throw new ArgumentNullException(nameof(fealds));
            }
            UniqueCode=uniqued;
            OrderId = orderId;
            Fealds = fealds.ToArray();
            Step= step;
            IsFinal = IsFinal;
        }
    }
}
