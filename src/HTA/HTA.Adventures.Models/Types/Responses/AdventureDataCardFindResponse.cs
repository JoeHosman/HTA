using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureDataCardFindResponse : AdventureDataCardBaseResponse
    {
        [DataMember]
        public IList<AdventureDataCard> DataCards { get; set; }

        public AdventureDataCardFindResponse(AdventureDataCard request)
            : base(request)
        {
            DataCards = new List<AdventureDataCard>();
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureDataCardFindResponse;

            if (base.Equals(obj) && null != other)
            {
                if ((null != DataCards) != (null != other.DataCards))
                    return false;

                if (null == DataCards)
                    return true;

                return (DataCards.Count == other.DataCards.Count);

            }

            return false;
        }
    }
}