using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.Conversores
{
    public class FirestoreDateTimeConvesor : IFirestoreConverter<DateTime>
    {
        public DateTime FromFirestore(object value)
        {
            if (value is Timestamp timestamp)
            {
                return timestamp.ToDateTime();
            }
            throw new ArgumentException("Value is not a Timestamp", nameof(value));
        }

        public object ToFirestore(DateTime value)
        {
            return Timestamp.FromDateTime(value);
        }
    }

}
