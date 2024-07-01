using Google.Cloud.Firestore;
using System;

namespace Rino.Infra.Conversores
{
    public class FirestoreDateTimeConversor : IFirestoreConverter<DateTime>
    {
        public DateTime FromFirestore(object value)
        {
            if (value is Timestamp timestamp)
            {
                return timestamp.ToDateTime();
            }
            throw new ArgumentException("O valor não é um Timestamp", nameof(value));
        }

        public object ToFirestore(DateTime value)
        {
            return Timestamp.FromDateTime(value);
        }
    }
}
