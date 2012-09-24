using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureDataCardSaveResponse : AdventureDataCardBaseResponse
    {
        [DataMember]
        public AdventureDataCard DataCard { get; set; }

        public AdventureDataCardSaveResponse(AdventureDataCard request)
            : base(request)
        {

        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureDataCardSaveResponse;

            if (base.Equals(obj) && null != other)
            {
                if ((null != DataCard) != (null != other.DataCard))
                    return false;

                if (null == DataCard)
                    return true;

                return (string.Compare(DataCard.Id, other.DataCard.Id) == 0);

            }

            return false;
        }
    }
}